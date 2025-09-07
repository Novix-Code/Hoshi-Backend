
using AutoMapper;
using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.OrderDTOs.OrderVisitDTOs;
using Hoshi.Models.OrderModels;
using Hoshi.Repositories.NotificationService;
using Microsoft.EntityFrameworkCore;

namespace Hoshi.Repositories.OrderVisitService
{
    /// <summary>
    /// Manages creation of order visits and dispatches client notifications.
    /// </summary>
    public class OrderVisitService : IOrderVisitService
    {
        private readonly INotificationServiceHandler notificationServiceHandler;
        private readonly HoshiDbContext _context;
        private readonly IMapper _mapper;
        public OrderVisitService(INotificationServiceHandler notificationServiceHandler, HoshiDbContext context, IMapper mapper)
        {
            this.notificationServiceHandler = notificationServiceHandler;
            this._context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Create an order visit and attach a new invoice record with client fees if applicable.
        /// </summary>
        public async Task<ResultDTO<object>> addVisitAsync(OrderVisitPostDTO orderVisit)
        {
            var targetOrder = await _context.Orders.FindAsync(orderVisit.OrderId);
            if (targetOrder is null) 
                return ResultDTO<object>.NotFound(new ErrorDTO { ErrorEn = "the Target Order Not Found" ,
                                                                 ErrorAr="الطلب غير موجود"});
            var id = targetOrder.ClientId;
            var clientSpecificationDetails = await _context.ClientSpecifications.FirstOrDefaultAsync(p=>p.UserId == id);
            if (clientSpecificationDetails is null)
                return ResultDTO<object>.NotFound(new ErrorDTO
                {
                    ErrorEn = "Client specification Not Found",
                    ErrorAr = "تفاصيل العميل غير مكتمله"
                });
            var visitMapper = _mapper.Map<OrderVisit>(orderVisit);
            await _context.OrderVisits.AddAsync(visitMapper);
            await _context.SaveChangesAsync();  
            var invoicMapper = new Invoice
            {
                OrderId = orderVisit.OrderId,
                OrderVisitId= visitMapper.Id
            };
            if (clientSpecificationDetails.Indebtedness > 0)
                invoicMapper.ClientIndebtednessFee = clientSpecificationDetails.Indebtedness;
            if (clientSpecificationDetails.Balance > 0)
                invoicMapper.ClientPromotionFee = clientSpecificationDetails.Balance;
            await _context.Invoices.AddAsync(invoicMapper);
            await _context.SaveChangesAsync();

            return ResultDTO<object>.Success(new
            {
                VisitId = visitMapper.Id ,
                InvoiceId = invoicMapper.Id
            });



        }

        /// <summary>
        /// Send a message to the client of an order.
        /// </summary>
        public async Task sendNoificationforclient(int orderId , string message)
        {
            var targetOrder = await _context.Orders.FirstOrDefaultAsync(p => p.Id == orderId);
            await notificationServiceHandler.sendMessagetoClient(message, targetOrder.ClientId);
        }

        /// <summary>
        /// Send a message to the client of a given visit's order.
        /// </summary>
        public async Task sendNoificationforclient2(int VisitId, string message)
        {
            var targetVisit = await _context.OrderVisits.FindAsync(VisitId);
            var targetOrder = await _context.Orders.FindAsync(targetVisit.OrderId);
            await notificationServiceHandler.sendMessagetoClient(message, targetOrder.ClientId);

        }
    }
}
