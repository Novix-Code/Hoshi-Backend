using AutoMapper;
using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.OrderDTOs.InvoiceDTOs;
using Hoshi.DTOs.OrderDTOs.OrderDTOs;
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
            var targetOffer = await _context.Offers.FindAsync(id);
            if (targetOffer == null)
            {
                return ResultDTO<object>.Failure(new ErrorDTO(), ResponseStatusCodes.NotFound);
            }
            targetOffer.OfferStatus = Enums.OfferStatus.Accepted;
            await _context.SaveChangesAsync();
            var targetOrder = await _context.Orders.FindAsync(targetOffer.OrderId);
            if (targetOrder == null)
            {
                return ResultDTO<object>.Failure(new ErrorDTO(), ResponseStatusCodes.NotFound);

            }
            var targetWorker = await _context.Users.FindAsync(targetOffer.WorkerId);
            if (targetWorker == null)
            {
                return ResultDTO<object>.Failure(new ErrorDTO(), ResponseStatusCodes.NotFound);
            }
            var workerSpecificationTarget = await _context.WorkerSpecifications.Where(p => p.UserId == targetOffer.WorkerId).FirstOrDefaultAsync();
            if (workerSpecificationTarget == null)
            {
                return ResultDTO<object>.Failure(new ErrorDTO { ErrorAr = "تفاصيل العامل ليست موجوده ", ErrorEn = "worker specification not handled"}, ResponseStatusCodes.NotFound);
            }
            var targetInvoice = await _context.Invoices.Where(p => p.OrderId == targetOffer.OrderId).FirstOrDefaultAsync();
            if (targetInvoice == null)
            {
                return ResultDTO<object>.Failure(new ErrorDTO(), ResponseStatusCodes.NotFound);
            }
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
