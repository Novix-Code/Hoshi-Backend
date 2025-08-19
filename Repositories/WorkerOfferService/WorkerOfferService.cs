using AutoMapper;
using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.OrderDTOs.OfferDTOs;
using Hoshi.Enums;
using Hoshi.Models.OrderModels;
using Hoshi.Models.UserModels.WorkerModels;
using Hoshi.Repositories.Hubs;
using Hoshi.Repositories.NotificationService;
using Hoshi.Repositories.WorkerVisitService;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Hoshi.Repositories.WorkerOfferService
{
    public class WorkerOfferService : IWorkerOfferService
    {
        private readonly HoshiDbContext _hoshiDbContext;
        private readonly IMapper _mapper;
        private readonly IHubContext<NotificationHub, INotificationHub> _hubContext;
        private readonly INotificationServiceHandler notificationServiceHandler;
        private readonly IWorkerVisitService workerVisitService;
        

        public WorkerOfferService(HoshiDbContext hoshiDbContext, IMapper mapper, IHubContext<NotificationHub, INotificationHub> hubContext, INotificationServiceHandler notificationServiceHandler, IWorkerVisitService workerVisitService)
        {
            _hoshiDbContext = hoshiDbContext;
            _mapper = mapper;
            _hubContext = hubContext;
            this.notificationServiceHandler = notificationServiceHandler;
            this.workerVisitService = workerVisitService;
        }

        public async Task<ResultDTO<CreateOfferResponseDto>> CreateOfferAsync(OfferPostDTO dto)
        {
            using var transaction = await _hoshiDbContext.Database.BeginTransactionAsync();
            try
            {
                // 1. Validate order and worker existence
                var order = await _hoshiDbContext.Orders
                    .Include(p=>p.AppliedPromotion)
                    .FirstOrDefaultAsync(o => o.Id == dto.OrderId);
                if (order == null)
                    return ResultDTO<CreateOfferResponseDto>.NotFound(new ErrorDTO
                    {
                        ErrorAr = "الطلب غير موجود.",
                        ErrorEn = "Order not found."
                    });
                
                var worker = await _hoshiDbContext.Users.FindAsync(dto.WorkerId);
                if (worker == null)
                    return ResultDTO<CreateOfferResponseDto>.NotFound(new ErrorDTO
                    {
                        ErrorAr = "العامل غير موجود.",
                        ErrorEn = "Worker not found."
                    });

                // 2. Create the offer
                var offer = _mapper.Map<Offer>(dto);
                _hoshiDbContext.Offers.Add(offer);
                await _hoshiDbContext.SaveChangesAsync();

                // 3. Calculate fees (prefer special by service, else base)
                var (visitMain, visitMin, visitMax) = await workerVisitService.GetFeeAsync(FeeType.VisitingFee,order);
                var (cancelMain, cancelMin, cancelMax) = await workerVisitService.GetFeeAsync(FeeType.CancellationFee,order);
                var (commissionMain, commissionMin, commissionMax) = await workerVisitService.GetFeeAsync(FeeType.CommissionFee,order);

                var visitFeeValue = workerVisitService.Clamp(visitMain, visitMin, visitMax);
                var cancellationFeeValue = workerVisitService.Clamp(cancelMain, cancelMin, cancelMax);
                var commissionFeeValue = workerVisitService.Clamp(commissionMain, commissionMin, commissionMax);

                // 4. Determine applied promotion for this order and split between client/worker
                var promotionsUsed = await _hoshiDbContext.PromotionsTaken
                    .Include(pt => pt.Promotion)
                    .Where(pt => pt.OrderId == dto.OrderId)
                    .Select(pt => pt.PromotionId)
                    .ToListAsync();

                var promotion = await _hoshiDbContext.Promotions.FirstOrDefaultAsync(p => !promotionsUsed.Contains(p.Id));
                if (promotion != null)
                   {
                    offer.AppliedPromotionId = promotion.Id;
                    _hoshiDbContext.Offers.Update(offer);
                    await _hoshiDbContext.SaveChangesAsync();
                }



                string promotionTitle = string.Empty;
                double workerPromotionFee = 0.0;
                double clientPromotionFee = order.AppliedPromotion?.Value ?? 0.0;

                if (promotion != null)
                {
                    promotionTitle = promotion.IsPercentage 
                        ? $"{promotion.TitleFirstPart} {promotion.Value} {promotion.TitleSecondPart}".Trim()
                        : $"{promotion.TitleFirstPart} {promotion.Value}% {promotion.TitleSecondPart}".Trim();
                    double promotionAmount = promotion.IsPercentage ? (dto.OfferedPrice * promotion.Value / 100) : promotion.Value;

                    if (promotion.PromotionFor == PromotionFor.Worker)
                        workerPromotionFee = promotionAmount;
                }

                var commissionAfterWorkerPromo = Math.Max(commissionFeeValue - workerPromotionFee, 0.0);

                double workerTotalPrice = dto.OfferedPrice - commissionAfterWorkerPromo;

                var invoice = await _hoshiDbContext.Invoices.FirstOrDefaultAsync(i => i.OrderId == order.Id);

                var clientTotalPrice = dto.OfferedPrice + ( invoice?.ClientIndebtednessFee ?? 0 ) - clientPromotionFee;

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
                        ClientPromotionFee = clientPromotionFee,
                        ClientIndebtednessFee = 0.0,
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
                    existingTemp.ClientPromotionFee = clientPromotionFee;
                    existingTemp.ClientIndebtednessFee = 0.0;
                    existingTemp.ClientTotalPrice = clientTotalPrice;
                    existingTemp.WorkerTotalPrice = workerTotalPrice;
                    _hoshiDbContext.TempInvoices.Update(existingTemp);
                }
                await _hoshiDbContext.SaveChangesAsync();

                // 8. Build response DTO
                var response = new CreateOfferResponseDto
                {
                    OfferId = offer.Id,
                    ClientName = order.Client?.UserName ?? string.Empty,
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
                await notificationServiceHandler.sendMessagetoClient("تم اضافة عرض على الطلب الخاص بك" ,order.ClientId);
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

                offer.OfferStatus = OfferStatus.Waitting;
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

        public async Task<ResultDTO<string>> CancelOfferAsync(int offerId)
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

                offer.IsDeleted = true;
                offer.OfferStatus = OfferStatus.Cancelled;

                // Apply cancellation fee logic (for worker)
                // Get the cancellation fee for this service
                var order = await _hoshiDbContext.Orders.FindAsync(offer.OrderId);
                if (order != null)
                {
                    double workerCancellationFee = await _hoshiDbContext.Fees
                        .Where(f => f.ServiceId == order.ServiceId && f.FeeType == FeeType.CancellationFee && !f.IsSpecial)
                        .Select(f => f.MainFees)
                        .FirstOrDefaultAsync();

                    var workerWallet = await _hoshiDbContext.WorkerWallets.FirstOrDefaultAsync(w => w.WorkerId == offer.WorkerId);
                    if (workerWallet == null)
                    {
                        return ResultDTO<string>.NotFound(new ErrorDTO
                        {
                            ErrorAr = "محفظة العامل غير موجودة.",
                            ErrorEn = "Worker wallet not found."
                        });
                    }

                    if (workerWallet.Balance < workerCancellationFee)
                    {
                        return ResultDTO<string>.NotFound(new ErrorDTO
                        {
                            ErrorAr = "رصيد المحفظة غير كافي.",
                            ErrorEn = "Worker wallet balance is not enough."
                        });
                    }

                    workerWallet.Balance -= workerCancellationFee;
                    _hoshiDbContext.WorkerWallets.Update(workerWallet);
                    
                    _hoshiDbContext.WorkerWalletHistories.Add(new WorkerWalletHistory
                    {
                        Title = "Cancellation Fee",
                        Value = workerCancellationFee,
                        IsIncome = false,
                        WorkerWalletId = workerWallet.Id
                    });
                    
                    await _hoshiDbContext.SaveChangesAsync();
                    await notificationServiceHandler.sendMessagetoClient($"   قام العامل ب إلغاء الطلب والغرامه هي: {workerCancellationFee}",order.ClientId);

                }

                _hoshiDbContext.Offers.Update(offer);
                await transaction.CommitAsync();
                await _hoshiDbContext.SaveChangesAsync();


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
