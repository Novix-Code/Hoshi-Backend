using AutoMapper;
using AutoMapper.QueryableExtensions;
using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.OrderDTOs.InvoiceDTOs;
using Hoshi.DTOs.OrderDTOs.OfferDTOs;
using Hoshi.DTOs.OrderDTOs.OrderDTOs;
using Hoshi.DTOs.OrderDTOs.OrderStatusHistoryDTOs;
using Hoshi.DTOs.OrderDTOs.OrderVisitDTOs;
using Hoshi.DTOs.PromotionDTOs.PromotionTakenDTOs;
using Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerPortfolioDTOs;
using Hoshi.Models.DashboardModels;
using Hoshi.Models.OrderModels;
using Hoshi.Models.PromotionModels;
using Hoshi.Repositories.ClientHomeService;
using Hoshi.Repositories.Hubs;
using Hoshi.Repositories.NotificationService;
using Hoshi.Repositories.OrderImageService;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Hoshi.Repositories.ClientOrderService
{
    public class ClientOrderService : IClientOrderService
    {
        private readonly HoshiDbContext _Context;
        private readonly IMapper _mapper;
        private readonly IClientHomeService _clientHomeService;
        private readonly IOrderImageService orderImageService;
        private readonly IHubContext<NotificationHub, INotificationHub> _hubContext;
        private readonly INotificationServiceHandler notificationServiceHandler;


        public ClientOrderService(
            HoshiDbContext context,
            IMapper mapper,
            IClientHomeService clientHomeService,
            IOrderImageService orderImageService
,
            IHubContext<NotificationHub, INotificationHub> hubContext,
            INotificationServiceHandler notificationServiceHandler)
        {
            _Context = context;
            _mapper = mapper;
            _clientHomeService = clientHomeService;
            this.orderImageService = orderImageService;
            _hubContext = hubContext;
            this.notificationServiceHandler = notificationServiceHandler;
        }
        public async Task<ResultDTO<string>> AddOrderAsync(OrderPostDTO dto)
        {
            ErrorDTO error = new ErrorDTO();

            try
            {
                var orderMapper = _mapper.Map<Order>(dto);
                _Context.Orders.Add(orderMapper);
                await _Context.SaveChangesAsync();

                var histOrder = new OrderStatusHistoryPostDTO
                {
                    CreatedAt = DateTime.Now,
                    OrderId = orderMapper.Id,
                    OrderStatus = orderMapper.OrderStatus
                };
                var histMapper = _mapper.Map<OrderStatusHistory>(histOrder);

                var invoicMapper = new Invoice
                {
                    OrderId = orderMapper.Id
                };

                _Context.Invoices.Add(invoicMapper);
                _Context.OrderStatusHistory.Add(histMapper);

                var clientPromotionNonTaken = await _clientHomeService.GetByIdServiceAsync(dto.ClientId);
                var slectedPromotionId = clientPromotionNonTaken.Data.Promotions.Select(p => p.Id).FirstOrDefault();

                //var _offerId = await _Context.Offers.Where(p => p.OrderId == orderMapper.Id).Select(p => (int?)p.Id).FirstOrDefaultAsync();

                if (slectedPromotionId is not 0)
                {
                    var promotionOrder = new PromotionTakenPostDTO
                    {
                        UserId = dto.ClientId,
                        OrderId = orderMapper.Id,
                        OfferId = null,
                        PromotionId = slectedPromotionId
                    };
                    var promotionMapper = _mapper.Map<PromotionTaken>(promotionOrder);
                    _Context.PromotionsTaken.Add(promotionMapper);
                }

                // Add order images
                if (!dto.OrderImagesFiles.IsNullOrEmpty())
                    await orderImageService.AddImages(orderMapper.Id, dto.OrderImagesFiles!);
                await _Context.SaveChangesAsync();
                // send Notification
                await notificationServiceHandler.sendMessagetoAdmin("عمليه اضافة اوردر" , orderMapper.Id);
                return ResultDTO<string>.Success(orderMapper.Id.ToString());
            }
            catch (Exception ex)
            {
                error.ErrorAr = "يوجد مشكلة في عملية الاضافة.";
                error.ErrorEn = "There is a problem in Adding proccess.";

                return ResultDTO<string>.InternalServerError(error, ex.InnerException!.Message);
            }
        }

        public async Task<ResultDTO<string>> DeleteOrder(int orderId)
        {
            var targetOrder = await _Context.Orders.FindAsync(orderId);
            targetOrder.OrderStatus = Enums.OrderStatus.Cancelled;
            await _Context.SaveChangesAsync();
            // send Notification
            await notificationServiceHandler.sendMessagetoAdmin("عمليه حذف اوردر", orderId);
            await notificationServiceHandler.sendMessagetoWorker("قام العميل بإلغاء الطلب", (int)targetOrder.WorkerId);

            return ResultDTO<string>.Success("Successfully deleted");
        }

        public async Task<ResultDTO<List<OrderGetAllDto>>> GetAllClientsAsync()
        {
            var AllOrders = await _Context.Orders
                .Include(nameof(Order.Service))
                .Include(nameof(Order.City))
                .Include(nameof(Order.OrderImages))
                .Where(p => p.OrderStatus != Enums.OrderStatus.Cancelled)
                .ProjectTo<OrderGetAllDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return ResultDTO<List<OrderGetAllDto>>.Success( AllOrders); 
        }

        public async Task<ResultDTO<OrderGetDetailsDto>> GetOrderDetails(int orderId)
        {
            var resultDto = new OrderGetDetailsDto();
            var orderData = await _Context.Orders.FindAsync(orderId);
            var target = _mapper.Map<OrderGetDTO>(orderData);
            var targetOffers = await _Context.Offers.Where(p => p.OrderId == orderId).ProjectTo<OfferGetDTO>(_mapper.ConfigurationProvider).ToListAsync();
            if (targetOffers.Count() > 0)
            {
                var workerId = targetOffers[0].Worker!.Id;
                var workerData = await _Context.WorkerPortfolios.FindAsync(workerId);
                resultDto.WorkerData = _mapper.Map<WorkerPortfolioGetDTO>(workerData);
            }
            if(orderData.OrderVisits != null)
            {
                resultDto.VisiteRequest = _mapper.Map<List<OrderVisitGetDTO>>(orderData.OrderVisits);
            }
            var orderInvoice = await _Context.Invoices.Where(p => p.OrderId == orderId).ProjectTo<InvoiceGetDTO>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
            if (orderInvoice != null) 
            {
                resultDto.ClientOrdeInvoice = orderInvoice;
            }
            resultDto.OrderData = target;
            resultDto.Offers = targetOffers;
            return ResultDTO<OrderGetDetailsDto>.Success(resultDto);
        }
    }
}
