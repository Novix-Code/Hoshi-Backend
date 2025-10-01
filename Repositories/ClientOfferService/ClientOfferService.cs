using AutoMapper;
using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.OrderDTOs.InvoiceDTOs;
using Hoshi.DTOs.OrderDTOs.OrderDTOs;
using Hoshi.Models.OrderModels;
using Hoshi.Models.PromotionModels;
using Hoshi.Repositories.NotificationService;
using Microsoft.EntityFrameworkCore;

namespace Hoshi.Repositories.ClientOfferService
{
    public class ClientOfferService : IClientOfferService
    {
        private readonly HoshiDbContext _context;
        private readonly IMapper _mapper;
        private readonly INotificationServiceHandler _notificationServiceHandler;
        private readonly ILogger<ClientOfferService> _logger;
        public ClientOfferService(HoshiDbContext context, IMapper mapper, INotificationServiceHandler notificationServiceHandler, ILogger<ClientOfferService> logger)
        {
            _context = context;
            _mapper = mapper;
            _notificationServiceHandler = notificationServiceHandler;
            _logger = logger;
        }

        public async Task<ResultDTO<object>> AcceptOfferAsync(int offerId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {

                _logger.LogInformation("Start AcceptOfferAsync for OfferId={OfferId}", offerId);
                // 1. Get offer and update it's status also update order's status
                var targetOffer = await _context.Offers
                    .Include(p => p.AppliedPromotion)
                    .Include(c => c.Order)
                    .Include(w => w.Worker)
                    .FirstOrDefaultAsync(p => p.Id == offerId);

                if (targetOffer == null)
                    return ResultDTO<object>.NotFound(new ErrorDTO
                    {
                        ErrorEn = "target offer not found",
                        ErrorAr = "العرض غير موجود"
                    });

                // Validate order status before transition to avoid invalid state changes
                if (targetOffer.Order.OrderStatus != Enums.OrderStatus.Published.ToString())
                    return ResultDTO<object>.BadRequest(
                        new ErrorDTO
                        {
                            ErrorEn = "Order not in expected state.",
                            ErrorAr = "حالة الطلب لا تسمح بقبول العرض."
                        });

                targetOffer.OfferStatus = Enums.OfferStatus.Accepted.ToString();
                targetOffer.Order.OrderStatus = Enums.OrderStatus.Assigned.ToString();
                targetOffer.Order.WorkerId = targetOffer.WorkerId;

                _context.Offers.Update(targetOffer);
                _context.Orders.Update(targetOffer.Order);

                // 2. Add order History into OrderStatusHistory table
                await _context.OrderStatusHistory.AddAsync(new OrderStatusHistory
                {
                    CreatedAt = DateTime.UtcNow,
                    OrderId = targetOffer.OrderId,
                    OrderStatus = Enums.OrderStatus.Assigned
                });
                await _context.SaveChangesAsync();

                _logger.LogInformation("Offer {OfferId} status updated to Accepted", offerId);
                _logger.LogInformation("Order {OrderId} assigned to Worker {WorkerId}",
                                        targetOffer.Order.Id, targetOffer.WorkerId);


                // 3. Determin Worker Details ( worker specification )
                var workerSpecificationTarget = await _context.WorkerSpecifications
                    .Include(p => p.Job)
                    .FirstOrDefaultAsync(p => p.UserId == targetOffer.WorkerId);
                if (workerSpecificationTarget == null)
                    return ResultDTO<object>.NotFound(new ErrorDTO
                    {
                        ErrorAr = "تفاصيل العامل ليست موجوده ",
                        ErrorEn = "worker specification not handled"
                    });
                if (workerSpecificationTarget.Job == null)
                    return ResultDTO<object>.NotFound(new ErrorDTO
                    {
                        ErrorAr = "وظيفة العامل غير موجودة",
                        ErrorEn = "worker job not found"
                    });

                // 4. Check Temporary Invoice and also Create a new Invoice
                var temp = await _context.TempInvoices.FirstOrDefaultAsync(t => t.OfferId == offerId);
                if (temp == null)
                    return ResultDTO<object>.NotFound(new ErrorDTO
                    {
                        ErrorAr = "الفاتورة المؤقتة غير موجودة",
                        ErrorEn = "Temp invoice not found"
                    });
                var targetInvoice = await _context.Invoices.FirstOrDefaultAsync(p => p.OrderId == targetOffer.OrderId);
                if (targetInvoice == null)
                {
                    targetInvoice = new Invoice
                    {
                        OrderId = targetOffer.OrderId
                    };
                    _context.Invoices.Add(targetInvoice);
                    await _context.SaveChangesAsync();
                }
                targetInvoice.OrderPrice = temp.OrderPrice;
                targetInvoice.CommissionFee = temp.CommissionFee;
                targetInvoice.VisitingFee = temp.VisitingFee;
                targetInvoice.CancellationFee = temp.CancellationFee;
                targetInvoice.WorkerPromotionFee = temp.WorkerPromotionFee;
                targetInvoice.ClientTotalPrice = temp.ClientTotalPrice;
                targetInvoice.WorkerTotalPrice = temp.WorkerTotalPrice;
                _context.Invoices.Update(targetInvoice);
                _logger.LogInformation("Invoice created/updated for OrderId={OrderId}", targetOffer.OrderId);

                // 5. Determin Applied Promotions and add it to PromotionTaken Table
                //_context.TempInvoices.Remove(temp);
                // ///////////////////////////////////////////////////////
                // this part moved to add when the worker create the offer.
                // ///////////////////////////////////////////////////////
                /*if (targetOffer.AppliedPromotionId is not null)
                {
                    // add offer in Promotion Taken 
                    var promTaken = new PromotionTaken
                    {
                        CreatedAt = DateTime.UtcNow,
                        OfferId = offerId,
                        OrderId = targetOffer.Order.Id,
                        PromotionId = (int)targetOffer.AppliedPromotionId,
                        UserId = targetOffer.WorkerId
                    };
                    await _context.PromotionsTaken.AddAsync(promTaken);
                    _logger.LogInformation("PromotionTaken added for OfferId={OfferId}", offerId);
                }*/
                //targetOrder.OrderStatus = Enums.OrderStatus.InProgress;
                await _context.SaveChangesAsync();
                _logger.LogInformation("Offer {OfferId} accepted successfully", offerId);
                await transaction.CommitAsync();

                // 6. Send notification to the worker that his offer is accepted
                await _notificationServiceHandler.sendMessagetoWorker( 
                    8 ,targetOffer.WorkerId , 
                    $"تم قبول العرض الخاص بك المقدم على الطلب رقم #{targetOffer.OrderId}."
                );

                _logger.LogInformation("Notification sent to Worker {WorkerId}", targetOffer.WorkerId);

                // 7. Handle result section -- this DTOs to match business logic that required
                OrderGetDTO orderData = _mapper.Map<OrderGetDTO>(targetOffer.Order);
                var workerData = new
                {
                    Name = targetOffer.Worker.UserName ?? "Unknown",
                    workerJobTitle = workerSpecificationTarget.Job.JobTitle ?? "Unknown Job",
                    imageUrl = workerSpecificationTarget.IdentityImageURL ?? "",
                    RateRatio = workerSpecificationTarget.RateRito
                };
                InvoiceGetDTO invoiceData = _mapper.Map<InvoiceGetDTO>(targetInvoice);

                return ResultDTO<object>.Success(new
                {
                    orderData,
                    workerData,
                    invoiceData
                });
            }
            catch (Exception ex)

            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error in AcceptOfferAsync for OfferId={OfferId}", offerId);
                return ResultDTO<object>.Failure(new ErrorDTO
                {
                    ErrorEn = ex.InnerException?.Message ?? ex.Message
                }, ResponseStatusCodes.InternalServerError);
            }
        }

        public async Task<ResultDTO<object>> GetOfferDetailsByIdAsync(int offerId)
        {
            var targetOffer = await _context.Offers
                .Include(p => p.Worker)
                .FirstOrDefaultAsync(I => I.Id == offerId);
            if (targetOffer == null)
                return ResultDTO<object>.NotFound(new ErrorDTO
                {
                    ErrorEn = "this offer not exist",
                    ErrorAr = "لم يتم ايجاد العرض"
                });

            // get relative information about worker that's in WorkerSpecification table
            var workerSpecificationTarget = await _context.WorkerSpecifications
                .Include(p => p.Job)
                .FirstOrDefaultAsync(p => p.UserId == targetOffer.WorkerId);
            if (workerSpecificationTarget == null)
                return ResultDTO<object>.NotFound(new ErrorDTO
                {
                    ErrorEn = "Worker Specification Not Found",
                    ErrorAr = "بيانات العامل غير مكتمله"
                });

            // need to check if the worker details entered or not else           
            if (workerSpecificationTarget.Job == null)
                return ResultDTO<object>.NotFound(new ErrorDTO
                {
                    ErrorEn = "Worker Job Not Found",
                    ErrorAr = "لم يتم ارفاق وظيفة للعامل "
                });

            // as businees logic need this object matches to the returned output from this method
            var workerData = new
            {
                WorkerName = targetOffer.Worker.UserName,
                WorkerJobTitle = workerSpecificationTarget.Job.JobTitle,
                workerImageUrl = workerSpecificationTarget.IdentityImageURL,
                WorkerRateRatio = workerSpecificationTarget.RateRito,
                WorkerTotalFinishedOrders = workerSpecificationTarget.CompletedOrders,
            };
            var OfferData = new
            {

                OfferPrice = targetOffer.OfferedPrice,
                OfferNote = targetOffer.Note
            };
            return ResultDTO<object>.Success(new
            {
                workerData,
                OfferData
            });

        }
    }
}
