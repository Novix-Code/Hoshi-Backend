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
        public ClientOfferService(HoshiDbContext context, IMapper mapper, INotificationServiceHandler notificationServiceHandler)
        {
            _context = context;
            _mapper = mapper;
            _notificationServiceHandler = notificationServiceHandler;
        }

        public async Task<ResultDTO<object>> AcceptOfferAsync(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // get target offer 
                var targetOffer = await _context.Offers.FindAsync(id);
                if (targetOffer == null)
                {
                    return ResultDTO<object>.Failure(new ErrorDTO { ErrorEn = "target offer not found", ErrorAr = "العرض غير موجود" }, ResponseStatusCodes.NotFound);
                }
                // change status of offer to accepted
                targetOffer.OfferStatus = Enums.OfferStatus.Accepted;
                _context.Offers.Update(targetOffer);
                await _context.SaveChangesAsync();
                // get target order to update  status and worker Id
                var targetOrder = await _context.Orders.FindAsync(targetOffer.OrderId);
                if (targetOrder == null)
                {
                    return ResultDTO<object>.Failure(new ErrorDTO(), ResponseStatusCodes.NotFound);

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
                // get Worker to get the relative details
                var targetWorker = await _context.Users.FindAsync(targetOffer.WorkerId);
                if (targetWorker == null)
                {
                    return ResultDTO<object>.Failure(new ErrorDTO(), ResponseStatusCodes.NotFound);
                }
                var workerSpecificationTarget = await _context.WorkerSpecifications.Include(p => p.Job).FirstOrDefaultAsync(p => p.UserId == targetOffer.WorkerId);
                if (workerSpecificationTarget == null)
                {
                    return ResultDTO<object>.Failure(new ErrorDTO { ErrorAr = "تفاصيل العامل ليست موجوده ", ErrorEn = "worker specification not handled" }, ResponseStatusCodes.NotFound);
                }

                // Convert TempInvoice -> Invoice
                var temp = await _context.TempInvoices.FirstOrDefaultAsync(t => t.OfferId == id);
                if (temp == null)
                {
                    return ResultDTO<object>.Failure(new ErrorDTO { ErrorAr = "الفاتورة المؤقتة غير موجودة", ErrorEn = "Temp invoice not found" }, ResponseStatusCodes.NotFound);
                }
                // handle create a Invoice That is initialCreate 
                var targetInvoice = await _context.Invoices.Where(p => p.OrderId == targetOffer.OrderId).FirstOrDefaultAsync();
                if (targetInvoice == null)
                {
                    targetInvoice = new Models.OrderModels.Invoice
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
                _context.TempInvoices.Remove(temp);
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
                targetOrder.OrderStatus = Enums.OrderStatus.InProgress;
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                OrderGetDTO orderData = _mapper.Map<OrderGetDTO>(targetOrder);
                var workerData = new
                {
                    Name = targetWorker.UserName,
                    workerJobTitle = workerSpecificationTarget.Job.JobTitle,
                    imageUrl = workerSpecificationTarget.IdentityImageURL,
                    RateRatio = workerSpecificationTarget.RateRito
                };
                InvoiceGetDTO invoiceData = _mapper.Map<InvoiceGetDTO>(targetInvoice);
                await _notificationServiceHandler.sendMessagetoWorker("تم قبول العرض الخاص بك", targetOffer.WorkerId);
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
                return ResultDTO<object>.Failure(new ErrorDTO { ErrorEn = ex.Message }, ResponseStatusCodes.InternalServerError);
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
