using AutoMapper;
using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.OrderDTOs.OfferDTOs;
using Hoshi.Enums;
using Hoshi.Models.GlobalModels;
using Hoshi.Models.OrderModels;
using Hoshi.Models.PromotionModels;
using Hoshi.Models.UserModels;
using Hoshi.Models.UserModels.WorkerModels;
using Hoshi.Repositories.Hubs;
using Hoshi.Repositories.NotificationService;
using Hoshi.Repositories.PromotionService;
using Hoshi.Repositories.WorkerVisitService;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Hoshi.Repositories.WorkerOfferService
{
    /// <summary>
    /// Handles worker offer creation, confirmation, and cancellation including fee computations,
    /// temp invoice upserts, and notifications.
    /// </summary>
    public class WorkerOfferService : IWorkerOfferService
    {
        private readonly HoshiDbContext _hoshiDbContext;
        private readonly IMapper _mapper;
        private readonly IHubContext<NotificationHub, INotificationHub> _hubContext;
        private readonly INotificationServiceHandler notificationServiceHandler;
        private readonly IWorkerVisitService workerVisitService;
        private readonly IPromotionService promotionService;

        public WorkerOfferService(
            HoshiDbContext hoshiDbContext, 
            IMapper mapper, 
            IHubContext<NotificationHub, INotificationHub> hubContext, 
            INotificationServiceHandler notificationServiceHandler, 
            IWorkerVisitService workerVisitService,
            IPromotionService promotionService
        )
        {
            _hoshiDbContext = hoshiDbContext;
            _mapper = mapper;
            _hubContext = hubContext;
            this.notificationServiceHandler = notificationServiceHandler;
            this.workerVisitService = workerVisitService;
            this.promotionService = promotionService;
        }

        /// <summary>
        /// Create an offer, compute fees, promotions, and upsert temp invoice; notify client/admin.
        /// </summary>
        public async Task<ResultDTO<CreateOfferResponseDto>> CreateOfferAsync(OfferPostDTO dto)
        {
            using var transaction = await _hoshiDbContext.Database.BeginTransactionAsync();
            try
            {
                // 1. Validate order and worker existence
                var order = await _hoshiDbContext.Orders
                    .Include(p => p.AppliedPromotion)
                    .FirstOrDefaultAsync(o => o.Id == dto.OrderId);
                if (order == null)
                    return ResultDTO<CreateOfferResponseDto>.NotFound(new ErrorDTO
                    {
                        ErrorAr = "الطلب غير موجود.",
                        ErrorEn = "Order not found."
                    });

                // Ensure only Published orders accept new offers
                if (order.OrderStatus != OrderStatus.Published.ToString())
                    return ResultDTO<CreateOfferResponseDto>.BadRequest(new ErrorDTO
                    {
                        ErrorAr = "لا يمكن تقديم عرض لهذا الطلب.",
                        ErrorEn = "Order cannot accept offers in its current state."
                    });

                var worker = await _hoshiDbContext.Users.FindAsync(dto.WorkerId);
                if (worker == null)
                    return ResultDTO<CreateOfferResponseDto>.NotFound(new ErrorDTO
                    {
                        ErrorAr = "العامل غير موجود.",
                        ErrorEn = "Worker not found."
                    });

                // Ensure that the offered price is not Zero or less than Commission Minimum Value
                var commMinFee = await _hoshiDbContext.Fees.Where(f => f.FeeType == FeeType.CommissionFee.ToString()).Select(f => f.MinFees).FirstOrDefaultAsync();
                if (dto.OfferedPrice <= commMinFee)
                    return ResultDTO<CreateOfferResponseDto>.BadRequest(new ErrorDTO
                    {
                        ErrorAr = $"لا يمكن ادخال قيمة العرض اقل من او تساوي {commMinFee}.",
                        ErrorEn = $"Can not enter Offered Price less than or equal {commMinFee}."
                    });


                // 2. Create the offer
                var offer = _mapper.Map<Offer>(dto);

                // Block duplicate active offers by same worker for same order (business-dependent)
                bool duplicate = await _hoshiDbContext.Offers.AnyAsync(o =>
                    o.OrderId == dto.OrderId
                    && o.WorkerId == dto.WorkerId
                    && !o.IsDeleted);

                if (duplicate)
                    return ResultDTO<CreateOfferResponseDto>.BadRequest(new ErrorDTO
                    {
                        ErrorAr = "يوجد عرض سابق لهذا العامل.",
                        ErrorEn = "Duplicate worker offer for this order."
                    });
                else
                {
                    _hoshiDbContext.Offers.Add(offer);
                    await _hoshiDbContext.SaveChangesAsync();
                }

                // 3. Calculate fees (prefer special by service, else base)
                var (visitMain, visitMin, visitMax) = await workerVisitService.GetFeeAsync(FeeType.VisitingFee, order);
                var (cancelMain, cancelMin, cancelMax) = await workerVisitService.GetFeeAsync(FeeType.CancellationFee, order);
                var (commissionMain, commissionMin, commissionMax) = await workerVisitService.GetFeeAsync(FeeType.CommissionFee, order);

                // This to get the value of the Fee according to its range
                var visitFeeValue = workerVisitService.Clamp(
                    workerVisitService.ValueFromPercentage(dto.OfferedPrice, visitMain),
                    visitMin,
                    visitMax
                );
                var cancellationFeeValue = workerVisitService.Clamp(
                    workerVisitService.ValueFromPercentage(dto.OfferedPrice, cancelMain),
                    cancelMin,
                    cancelMax
                );
                var commissionFeeValue = workerVisitService.Clamp(
                    workerVisitService.ValueFromPercentage(dto.OfferedPrice, commissionMain),
                    commissionMin,
                    commissionMax
                );

                // 4. Determine applied promotion for this order and split between client/worker

                var workerPromotions = await promotionService.NoneTakenPromotions(worker.Id, false);

                var promotion = workerPromotions.FirstOrDefault();

                // if there a promo for this worker will be add to the offer
                if (promotion != null)
                {
                    // Add Promotion on Offer
                    offer.AppliedPromotionId = promotion.Id;
                    _hoshiDbContext.Offers.Update(offer);

                    // Add Promotion to Promotion Taken
                    await _hoshiDbContext.PromotionsTaken.AddAsync(new PromotionTaken()
                    {
                        OfferId = offer.Id,
                        OrderId = offer.OrderId,
                        UserId = offer.WorkerId,
                        PromotionId = promotion.Id,
                        CreatedAt = DateTime.UtcNow,
                    });

                    await _hoshiDbContext.SaveChangesAsync();
                }

                string promotionTitle = string.Empty;
                double workerPromotionFee = 0.0;
                //double clientPromotionFee = order.AppliedPromotion?.Value ?? 0.0;

                if (promotion != null)
                {
                    promotionTitle = promotion.IsPercentage
                        ? $"{promotion.TitleFirstPart} {promotion.Value}% {promotion.TitleSecondPart}".Trim()
                        : $"{promotion.TitleFirstPart} {promotion.Value} {promotion.TitleSecondPart}".Trim();
                    
                    workerPromotionFee = promotion.IsPercentage ? (dto.OfferedPrice * (promotion.Value / 100)) : promotion.Value;
                }

                var commissionAfterWorkerPromo = Math.Max(commissionFeeValue - workerPromotionFee, 0.0);

                double workerTotalPrice = dto.OfferedPrice - commissionAfterWorkerPromo;

                var invoice = await _hoshiDbContext.Invoices.FirstOrDefaultAsync(i => i.OrderId == order.Id);

                double clientTotalPrice =
                    (double)(dto.OfferedPrice + invoice?.ClientIndebtednessFee - invoice?.ClientPromotionFee)!;

                // 7. Upsert TempInvoice for this offer
                var existingTemp = await _hoshiDbContext.TempInvoices
                    .FirstOrDefaultAsync(t => t.OfferId == offer.Id);
                if (existingTemp == null)
                {
                    _hoshiDbContext.TempInvoices.Add(new TempInvoice
                    {
                        OrderPrice = dto.OfferedPrice,
                        CommissionFee = commissionFeeValue,
                        VisitingFee = visitFeeValue,
                        CancellationFee = cancellationFeeValue,
                        WorkerPromotionFee = workerPromotionFee,
                        ClientPromotionFee = invoice?.ClientPromotionFee ?? 0.0,
                        ClientIndebtednessFee = invoice?.ClientIndebtednessFee ?? 0.0,
                        ClientTotalPrice = clientTotalPrice,
                        WorkerTotalPrice = workerTotalPrice,
                        OfferId = offer.Id
                    });
                }
                else
                {
                    existingTemp.OrderPrice = dto.OfferedPrice;
                    existingTemp.CommissionFee = commissionFeeValue;
                    existingTemp.VisitingFee = visitFeeValue;
                    existingTemp.CancellationFee = cancellationFeeValue;
                    existingTemp.WorkerPromotionFee = workerPromotionFee;
                    existingTemp.ClientPromotionFee = invoice?.ClientPromotionFee ?? 0.0;
                    existingTemp.ClientIndebtednessFee = invoice?.ClientIndebtednessFee ?? 0.0;
                    existingTemp.ClientTotalPrice = clientTotalPrice;
                    existingTemp.WorkerTotalPrice = workerTotalPrice;
                    _hoshiDbContext.TempInvoices.Update(existingTemp);
                }
                await _hoshiDbContext.SaveChangesAsync();
                // Suggested: add unique index on TempInvoices.OfferId to ensure 1:1 temp invoice per offer

                // 8. Build response DTO
                var response = new CreateOfferResponseDto
                {
                    OfferId = offer.Id,
                    OrderId = offer.OrderId,
                    OfferedPrice = offer.OfferedPrice,
                    VisitFee = visitFeeValue,
                    CancellationFee = cancellationFeeValue,
                    ServiceFee = commissionFeeValue,
                    PromotionTitle = promotionTitle,
                    PromotionValue = workerPromotionFee,
                    WorkerRevenue = workerTotalPrice,
                    ClientWillPay = clientTotalPrice
                };

                await transaction.CommitAsync();
                // send notification
                await notificationServiceHandler.sendMessagetoAdmin("عمليه اضافة عرض", offer.Id);
                await notificationServiceHandler.sendMessagetoClient(
                    1, order.ClientId, $"تم اضافة عرض على الطلب رقم #{order.Id} الخاص بك");
                return ResultDTO<CreateOfferResponseDto>.Success(response);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return ResultDTO<CreateOfferResponseDto>.InternalServerError(new ErrorDTO
                {
                    ErrorAr = "حدث خطأ في الخادم.",
                    ErrorEn = $"Internal server error: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Confirm an offer; sets IsConfirmed and OfferStatus to Waitting.
        /// </summary>
        public async Task<ResultDTO<string>> ConfirmOfferAsync(int offerId)
        {
            using var transaction = await _hoshiDbContext.Database.BeginTransactionAsync();
            try
            {
                var offer = await _hoshiDbContext.Offers.FindAsync(offerId);
                if (offer == null)
                {
                    return ResultDTO<string>.NotFound(new ErrorDTO
                    {
                        ErrorAr = "العرض غير موجود.",
                        ErrorEn = "Offer not found."
                    });
                }

                offer.OfferStatus = OfferStatus.Waitting.ToString();
                offer.IsConfirmed = true;
                _hoshiDbContext.Offers.Update(offer);
                await _hoshiDbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return ResultDTO<string>.Success("Offer Confirmed");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return ResultDTO<string>.InternalServerError(new ErrorDTO
                {
                    ErrorAr = "حدث خطأ في الخادم.",
                    ErrorEn = $"Internal server error: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Cancel a worker offer; deduct cancellation fee from worker wallet and log history.
        /// </summary>
        public async Task<ResultDTO<string>> CancelOfferAsync(int offerId)
        {
            using var transaction = await _hoshiDbContext.Database.BeginTransactionAsync();
            try
            {
                var offer = await _hoshiDbContext.Offers.FindAsync(offerId);
                if (offer == null || offer.IsDeleted)
                {
                    return ResultDTO<string>.NotFound(new ErrorDTO
                    {
                        ErrorAr = "العرض غير موجود.",
                        ErrorEn = "Offer not found."
                    });
                }

                // Update the status of the offer
                offer.IsDeleted = true;
                offer.OfferStatus = OfferStatus.Cancelled.ToString();

                // Get Order Data that the offer on
                var order = await _hoshiDbContext.Orders.FindAsync(offer.OrderId);

                if (order is null) return ResultDTO<string>.BadRequest(new ErrorDTO
                {
                    ErrorAr = "الطلب غير موجود بالرجاء المحاولة مرة اخرى.",
                    ErrorEn = "The order dose not exist, try again leter."
                });

                var orderActiveStatus = new HashSet<string>()
                {
                    OrderStatus.Assigned.ToString(),
                    OrderStatus.InProgress.ToString()
                };

                // Get order invoice data
                var orderInvoice = await _hoshiDbContext.Invoices.FirstAsync(i => i.OrderId == order.Id);

                // Calculate datetime threshold
                DateTime threshold = order.ServicingDateTime.AddHours(-12);

                // Check if this offer is:
                //      - In active status
                //      - Current offer is the approved one by worker Id.
                //      - Servicing time is Earlier than current moment.
                if (
                    orderActiveStatus.Contains(order.OrderStatus) 
                    && order.WorkerId == offer.WorkerId
                )
                {
                    if (DateTime.UtcNow > threshold)
                    {
                        // update the status of the order
                        order.OrderStatus = OrderStatus.Cancelled.ToString();

                        // Get cancellation Fee to be applied on this worker
                        double workerCancellationFee = orderInvoice.CancellationFee;

                        // Get Max Indebtednes Fee to check if the worker hit his limit or not
                        double maxIndebtednessFee = await _hoshiDbContext.Fees
                            .Where(f =>
                                f.FeeType == FeeType.WorkerIndebtednessFee.ToString()
                                && (!f.IsSpecial || f.ServiceId == order.ServiceId)
                            )
                            .Select(f => f.MaxFees)
                            .FirstAsync();

                        // Get worker wallet to update
                        WorkerWallet workerWallet = await _hoshiDbContext.WorkerWallets.FirstAsync(w => w.WorkerId == offer.WorkerId);

                        // Calculate the new balance
                        workerWallet.Balance -= workerCancellationFee;

                        // Check if the new balance is less than the max indebtedness fee and if it true update HitLimit flag
                        if (workerWallet.Balance < (maxIndebtednessFee * -1))
                            workerWallet.HitLimit = true;

                        // Add transaction to wallet history
                        _hoshiDbContext.WorkerWalletHistories.Add(new WorkerWalletHistory
                        {
                            Title = "غرامة الغاء.",
                            Value = workerCancellationFee,
                            IsIncome = false,
                            WorkerWalletId = workerWallet.Id,
                            CreatedAt = DateTime.UtcNow
                        });

                        // Get client specifictaion 
                        ClientSpecification client = await _hoshiDbContext.ClientSpecifications
                            .FirstAsync(c => c.UserId == order.ClientId);

                        // Client take the half of fee and company take the other half
                        double clientBounce = workerCancellationFee / 2;

                        // Add bounce to Client balance
                        // First check if client has indebtedness or not
                        if (client.Indebtedness > 0)
                        {
                            // Check if the indebtedness greater than the bounce and if true will sub bounce from it
                            if (client.Indebtedness > clientBounce)
                                client.Indebtedness -= clientBounce;
                            else
                            {
                                // if not then remove available indebtedness and the remaning add it to balance
                                double remaning = clientBounce - client.Indebtedness;
                                client.Indebtedness = 0;
                                client.Balance += remaning;
                            }
                        }
                        // if there is no indebtedness will add it direct to balance
                        else
                            client.Balance += clientBounce;

                        // Send notification to user that the order has been cancelled and the bounce add to his balance.
                        await notificationServiceHandler.sendMessagetoClient(
                            11,
                            order.ClientId,
                            $"قام العامل ب إلغاء الطلب رقم #{order.Id}، والتعويض هو: {clientBounce} تم اضافته الي رصيدك."
                        );
                    }
                    // If cancellation before the limited time will re publish the order and cleare its data
                    else
                    {

                        // update the status of the order
                        order.OrderStatus = OrderStatus.Published.ToString();
                        // remove current worker from it
                        order.WorkerId = null;
                        // remove data from invoice
                        orderInvoice.OrderPrice = 0;
                        orderInvoice.CancellationFee = 0;
                        orderInvoice.VisitingFee = 0;
                        orderInvoice.CommissionFee = 0;
                        orderInvoice.WorkerPromotionFee = 0;
                        orderInvoice.WorkerTotalPrice = 0;
                        orderInvoice.ClientTotalPrice = 0;

                        // Send notification to user that the order has been cancelled and the bounce add to his balance.
                        await notificationServiceHandler.sendMessagetoClient(
                            11,
                            order.ClientId,
                            $"قام العامل ب إلغاء الطلب رقم #{order.Id} داخل المدة المسموحة ولذلك سيتم اعادة نشر طلبك."
                        );
                    }
                }

                await _hoshiDbContext.SaveChangesAsync();
                await transaction.CommitAsync();


                return ResultDTO<string>.Success("Offer Cancelled");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return ResultDTO<string>.InternalServerError(new ErrorDTO
                {
                    ErrorAr = "حدث خطأ في الخادم.",
                    ErrorEn = $"Internal server error: {ex.Message}"
                });
            }
        }

    }
}
