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

        public async Task<ResultDTO<object>> AcceptOfferAsync(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _logger.LogInformation("Start AcceptOfferAsync for OfferId={OfferId}", id);

                // get target offer 
                var targetOffer = await _context.Offers.Include(p=>p.AppliedPromotion).FirstOrDefaultAsync(p=>p.Id == id);
                if (targetOffer == null)
                {
                    _logger.LogWarning("Offer with Id={OfferId} not found", id);
                    return ResultDTO<object>.Failure(new ErrorDTO { ErrorEn = "target offer not found", ErrorAr = "العرض غير موجود" }, ResponseStatusCodes.NotFound);
                }
                // change status of offer to accepted
                targetOffer.OfferStatus = Enums.OfferStatus.Accepted;
                _context.Offers.Update(targetOffer);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Offer {OfferId} status updated to Accepted", id);

                // get target order to update  status and worker Id
                var targetOrder = await _context.Orders.FirstOrDefaultAsync(to=>to.Id == targetOffer.OrderId);

                if (targetOrder == null)
                {
                    _logger.LogWarning("Order with Id={OrderId} not found for OfferId={OfferId}", targetOffer.OrderId, id);
                    return ResultDTO<object>.Failure(new ErrorDTO { ErrorEn = "target order not found", ErrorAr = "الطلب غير موجود" }, ResponseStatusCodes.NotFound);
                }

                // check if targetOffer.WorkerId is null
                if (targetOffer.WorkerId == null)
                {
                    _logger.LogError("Offer {OfferId} has null WorkerId", id);
                    return ResultDTO<object>.Failure(new ErrorDTO { ErrorEn = "Worker ID is null", ErrorAr = "معرف العامل فارغ" }, ResponseStatusCodes.BadRequest);
                }

                // check if targetOrder.OrderId is null
                if (targetOffer.OrderId == null)
                {
                    _logger.LogError("Offer {OfferId} has null OrderId", id);
                    return ResultDTO<object>.Failure(new ErrorDTO { ErrorEn = "Order ID is null", ErrorAr = "معرف الطلب فارغ" }, ResponseStatusCodes.BadRequest);
                }

                targetOrder.OrderStatus = Enums.OrderStatus.Assigned;
                targetOrder.WorkerId = targetOffer.WorkerId;


                // add order History to the table
                var ordHst = new OrderStatusHistory
                {
                    CreatedAt = DateTime.UtcNow,
                    OrderId = targetOffer.OrderId,
                    OrderStatus = Enums.OrderStatus.Assigned
                };
                await _context.OrderStatusHistory.AddAsync(ordHst);
                _context.Orders.Update(targetOrder);

                await _context.SaveChangesAsync();
                _logger.LogInformation("Order {OrderId} assigned to Worker {WorkerId}", targetOrder.Id, targetOffer.WorkerId);

                // get Worker to get the relative details
                var targetWorker = await _context.Users.FirstOrDefaultAsync(u=>u.Id == targetOffer.WorkerId);

                if (targetWorker == null)
                {
                    _logger.LogWarning("Worker with Id={WorkerId} not found", targetOffer.WorkerId);
                    return ResultDTO<object>.Failure(new ErrorDTO(), ResponseStatusCodes.NotFound);
                }
                var workerSpecificationTarget = await _context.WorkerSpecifications.Include(p => p.Job).FirstOrDefaultAsync(p => p.UserId == targetOffer.WorkerId);
                
                if (workerSpecificationTarget == null)
                {
                    _logger.LogWarning("Worker specification not found for WorkerId={WorkerId}", targetOffer.WorkerId);
                    return ResultDTO<object>.Failure(new ErrorDTO { ErrorAr = "تفاصيل العامل ليست موجوده ", ErrorEn = "worker specification not handled" }, ResponseStatusCodes.NotFound);
                }

                // Check if Job is null
                if (workerSpecificationTarget.Job == null)
                {
                    _logger.LogWarning("Worker job not found for WorkerId={WorkerId}", targetOffer.WorkerId);
                    return ResultDTO<object>.Failure(new ErrorDTO { ErrorAr = "وظيفة العامل غير موجودة", ErrorEn = "worker job not found" }, ResponseStatusCodes.NotFound);
                }


                // Convert TempInvoice -> Invoice
                var temp = await _context.TempInvoices.FirstOrDefaultAsync(t => t.OfferId == id);
                if (temp == null)
                {
                    _logger.LogWarning("Temp invoice not found for OfferId={OfferId}", id);
                    return ResultDTO<object>.Failure(new ErrorDTO { ErrorAr = "الفاتورة المؤقتة غير موجودة", ErrorEn = "Temp invoice not found" }, ResponseStatusCodes.NotFound);
                }

                // handle create a Invoice That is initialCreate 
                var targetInvoice = await _context.Invoices.Where(p => p.OrderId == targetOffer.OrderId).FirstOrDefaultAsync();
                if (targetInvoice == null)
                {
                    targetInvoice = new Invoice
                    {
                        OrderId = targetOffer.OrderId
                    };
                    _context.Invoices.Add(targetInvoice);
                }

                targetInvoice.OrderPrice = temp.OrderPrice;
                targetInvoice.CommissionFee = temp.CommissionFee;
                targetInvoice.VisitingFee = temp.VisitingFee;
                targetInvoice.CancellationFee = temp.CancellationFee;
                targetInvoice.WorkerPromotionFee = temp.WorkerPromotionFee;
                targetInvoice.ClientTotalPrice = temp.ClientTotalPrice;
                targetInvoice.WorkerTotalPrice = temp.WorkerTotalPrice;
                
                _context.Invoices.Update(targetInvoice);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Invoice created/updated for OrderId={OrderId}", targetOffer.OrderId);

                //_context.TempInvoices.Remove(temp);

                if (targetOffer.AppliedPromotionId is not null)
                {
                    // add offer in Promotion Taken 
                    var promTaken = new PromotionTaken
                    {
                        CreatedAt = DateTime.UtcNow,
                        OfferId = id,
                        OrderId = targetOrder.Id,
                        PromotionId = (int)targetOffer.AppliedPromotionId,
                        UserId = targetOffer.WorkerId
                    };

                    await _context.PromotionsTaken.AddAsync(promTaken);
                    _logger.LogInformation("PromotionTaken added for OfferId={OfferId}", id);
                }

                //targetOrder.OrderStatus = Enums.OrderStatus.InProgress;

                await _context.SaveChangesAsync();
                _logger.LogInformation("Offer {OfferId} accepted successfully", id);
                await transaction.CommitAsync();

                if (_mapper == null)
                {
                    _logger.LogError("AutoMapper is not configured!");
                    return ResultDTO<object>.Failure(new ErrorDTO { ErrorEn = "AutoMapper not configured", ErrorAr = "خطأ في النظام" }, ResponseStatusCodes.InternalServerError);
                }


                OrderGetDTO orderData = _mapper.Map<OrderGetDTO>(targetOrder);

                var workerData = new
                {
                    Name = targetWorker.UserName ?? "Unknown",
                    workerJobTitle = workerSpecificationTarget.Job.JobTitle ?? "Unknown Job",
                    imageUrl = workerSpecificationTarget.IdentityImageURL ?? "",
                    RateRatio = workerSpecificationTarget.RateRito
                };
                InvoiceGetDTO invoiceData = _mapper.Map<InvoiceGetDTO>(targetInvoice);

                // Check if notification service is null before using it
                if (_notificationServiceHandler != null)
                {
                    await _notificationServiceHandler.sendMessagetoWorker("تم قبول العرض الخاص بك", targetOffer.WorkerId);
                    _logger.LogInformation("Notification sent to Worker {WorkerId}", targetOffer.WorkerId);

                }

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
                _logger.LogError(ex, "Error in AcceptOfferAsync for OfferId={OfferId}", id);
                return ResultDTO<object>.Failure(new ErrorDTO { ErrorEn = ex.InnerException?.Message ?? ex.Message }, ResponseStatusCodes.InternalServerError);
            }
        }

        public async Task<ResultDTO<object>> GetByIdAsync(int id)
        {
            var targetOffer = await _context.Offers.FindAsync(id);
            if (targetOffer == null)
            {
                return ResultDTO<object>.Failure(new ErrorDTO(), ResponseStatusCodes.NotFound);
            }
            var targetWorker = await _context.Users.FindAsync(targetOffer.WorkerId);
            var workerSpecificationTarget = await _context.WorkerSpecifications.Where(p => p.UserId == targetOffer.WorkerId).FirstOrDefaultAsync();
            if (workerSpecificationTarget == null)
            {
                return ResultDTO<object>.Failure(new ErrorDTO(), ResponseStatusCodes.NotFound);
            }
            var job = await _context.Jobs.FindAsync(workerSpecificationTarget.JobId);
            if (job == null)
            {
                return ResultDTO<object>.Failure(new ErrorDTO(), ResponseStatusCodes.NotFound);

            }
            var workerData = new
            {
                WorkerName = targetWorker.UserName,
                WorkerJobTitle = job.JobTitle,
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
