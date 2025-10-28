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
using Hoshi.Enums;
using Hoshi.Models.DashboardModels;
using Hoshi.Models.OrderModels;
using Hoshi.Models.PromotionModels;
using Hoshi.Models.UserModels.WorkerModels;
using Hoshi.Repositories.ClientHomeService;
using Hoshi.Repositories.Hubs;
using Hoshi.Repositories.NotificationService;
using Hoshi.Repositories.OrderImageService;
using Hoshi.Repositories.PromotionService;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Hoshi.Repositories.ClientOrderService
{
    /// <summary>
    /// Implements client-facing order operations: creation, deletion with time-window rules,
    /// listing and details aggregation. Sends admin/worker notifications on key events.
    /// </summary>
    public class ClientOrderService : IClientOrderService
    {
        private readonly HoshiDbContext _context;
        private readonly IMapper _mapper;
        private readonly IClientHomeService _clientHomeService;
        private readonly IOrderImageService orderImageService;
        private readonly IHubContext<NotificationHub, INotificationHub> _hubContext;
        private readonly INotificationServiceHandler notificationServiceHandler;
        private readonly IPromotionService promotionService;

        public ClientOrderService(
            HoshiDbContext context,
            IMapper mapper,
            IClientHomeService clientHomeService,
            IOrderImageService orderImageService,
            IHubContext<NotificationHub, INotificationHub> hubContext,
            INotificationServiceHandler notificationServiceHandler,
            IPromotionService promotionService
        )
        {
            _context = context;
            _mapper = mapper;
            _clientHomeService = clientHomeService;
            this.orderImageService = orderImageService;
            _hubContext = hubContext;
            this.notificationServiceHandler = notificationServiceHandler;
            this.promotionService = promotionService;
        }
        /// <summary>
        /// Create an order, add initial status history, create invoice and attach promotions/images if applicable.
        /// </summary>
        public async Task<ResultDTO<object>> AddOrderAsync(OrderPostDTO dto)
        {

            // 1. Get Client Specification
            var clientSpecificationDetails = await _context.ClientSpecifications
                                            .FirstOrDefaultAsync(p => p.UserId == dto.ClientId);
            if (clientSpecificationDetails is null)
                return ResultDTO<object>.NotFound(new ErrorDTO
                {
                    ErrorAr = "بيانات العميل لم يتم ادخالها",
                    ErrorEn = "Client specification not found"
                });
            using var tx = await _context.Database.BeginTransactionAsync();
            try
            {

                // 2. Add Order
                var orderMapper = _mapper.Map<Order>(dto);
                await _context.Orders.AddAsync(orderMapper);
                await _context.SaveChangesAsync();

                // 3. Add Order Status History
                await _context.OrderStatusHistory.AddAsync(new OrderStatusHistory
                {
                    CreatedAt = DateTime.Now,
                    OrderId = orderMapper.Id,
                    OrderStatus = Enum.Parse<OrderStatus>(orderMapper.OrderStatus)
                });

                // 4. Add Invoice
                var invoicMapper = new Invoice
                {
                    OrderId = orderMapper.Id
                };
                if (clientSpecificationDetails.Indebtedness > 0)
                    invoicMapper.ClientIndebtednessFee = clientSpecificationDetails.Indebtedness;

                // If user has balance will be add to the order promotion and in this case will not take a normal promotion
                if (clientSpecificationDetails.Balance > 0)
                    invoicMapper.ClientPromotionFee = clientSpecificationDetails.Balance;
                // If not in this case can take a promotion if is there a one for him
                else
                {
                    // 5. Determine promotion taken and add it
                    var clientPromotionNonTaken = await promotionService.NoneTakenPromotions(dto.ClientId, true);

                    var slectedPromotionId = clientPromotionNonTaken.Select(p => p.Id).FirstOrDefault();

                    if (slectedPromotionId is not 0)
                    {
                        await _context.PromotionsTaken.AddAsync(new PromotionTaken
                        {
                            UserId = dto.ClientId,
                            OrderId = orderMapper.Id,
                            OfferId = null,
                            PromotionId = slectedPromotionId,
                            CreatedAt = DateTime.Now
                        });
                    }
                }
               await _context.Invoices.AddAsync(invoicMapper);

                // 6. Add order images
                if (!dto.OrderImagesFiles.IsNullOrEmpty())
                    await orderImageService.AddImages(orderMapper.Id, dto.OrderImagesFiles!);

                await _context.SaveChangesAsync();
                await tx.CommitAsync();

                // 7. Send Notification
                await notificationServiceHandler.sendMessagetoAdmin("عمليه اضافة اوردر", orderMapper.Id);
                return ResultDTO<object>.Success(orderMapper.Id.ToString());
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync();
                return ResultDTO<object>.InternalServerError(
                    new ErrorDTO()
                    {
                        ErrorAr = "يوجد مشكلة في النظام.",
                        ErrorEn = "There is a Internal Server Error."
                    },
                    ex.InnerException != null ? ex.InnerException.Message : ex.Message
                );
            }
        }

        /// <summary>
        /// Cancel an order respecting business time windows (12h before/after service time).
        /// Applies client fee and worker wallet credit if late.
        /// </summary>
        public async Task<ResultDTO<string>> DeleteOrder(int orderId)
        {
            // 1. Get Order and check if it is Deleted or not
            var targetOrder = await _context.Orders.Include(i => i.Client).FirstOrDefaultAsync(o => o.Id == orderId);
            if (targetOrder == null) return ResultDTO<string>.BadRequest(new ErrorDTO
            {
                ErrorAr = "معرف الطلب غير صحيح.",
                ErrorEn = "Order Id is not correct"
            });

            if (targetOrder.OrderStatus == Enums.OrderStatus.Cancelled.ToString())
                return ResultDTO<string>.BadRequest(new ErrorDTO
                {
                    ErrorEn = "this order is already deleted",
                    ErrorAr = "العرض تم حذفه بالفعل"
                });
            // 2. Check If Order Is assigned
            if (targetOrder.OrderStatus == Enums.OrderStatus.Assigned.ToString())
            {
                // 2.1 Determin the Time that is matching with business logic which is (12H before and after)
                var befor12H = targetOrder.ServicingDateTime.AddHours(-12);
                var after12H = targetOrder.ServicingDateTime.AddHours(12);
                // Suggested: ensure ServicingDateTime stored in UTC and compare against DateTime.Now

                // 2.2 Befor 12H
                if (DateTime.Now <= befor12H)
                {
                    targetOrder.OrderStatus = Enums.OrderStatus.Cancelled.ToString();
                    _context.Orders.Update(targetOrder);
                    await _context.SaveChangesAsync();
                    await notificationServiceHandler.sendMessagetoAdmin("عمليه حذف اوردر", orderId);
                    await notificationServiceHandler.sendMessagetoWorker(
                        13 , (int) targetOrder.WorkerId! , 
                        $"قام العميل {targetOrder.Client?.FullName} بإلغاء الطلب رقم #{targetOrder.Id}."
                    );

                    return ResultDTO<string>.Success("Successfully deleted");
                }

                // 2.3 After 12H
                if (DateTime.Now >= after12H)
                {

                    var clientDetails = await _context.ClientSpecifications
                            .FirstOrDefaultAsync(p => p.UserId == targetOrder.ClientId);

                    if (clientDetails == null)
                        return ResultDTO<string>.NotFound(new ErrorDTO
                        {
                            ErrorEn = "client Specification not found",
                            ErrorAr = "بيانات العميل لم يتم ادخالها بعد"
                        });
                    var workerWallet = await _context.WorkerWallets
                            .FirstOrDefaultAsync(w => w.WorkerId == targetOrder.WorkerId);
                    if (workerWallet == null)
                        return ResultDTO<string>.NotFound(new ErrorDTO
                        {
                            ErrorAr = "محفظة العامل غير موجودة.",
                            ErrorEn = "Worker wallet not found."
                        });

                    // 2.4 Determin Fee for client and get details about him and Walledt information about worker
                    Invoice? orderInvoice = await _context.Invoices.FirstOrDefaultAsync(i => i.OrderId == orderId);

                    double commissionPercentage = _context.Fees
                        .FirstOrDefaultAsync(f => f.FeeType == FeeType.CommissionFee.ToString()).Result!.MainFees;

                    double cancelationvalue = 
                        orderInvoice!.CancellationFee - (orderInvoice!.CancellationFee * (commissionPercentage / 100));

                    // 2.5 Check the balance
                    if (clientDetails.Balance < cancelationvalue)
                    {
                        double remaningValue = cancelationvalue - clientDetails.Balance;
                        clientDetails.Indebtedness += remaningValue;
                        clientDetails.Balance = 0;
                    }
                    else
                        clientDetails.Balance -= cancelationvalue;

                    // 2.6 Add amount to worker wallet
                    workerWallet.Balance += cancelationvalue;

                    // add wallet transaction to history
                    await _context.WorkerWalletHistories.AddAsync(new WorkerWalletHistory
                    {
                        Value = cancelationvalue,
                        Title = "اضافة تعويض الغاء",
                        IsIncome = true,
                        WorkerWalletId = workerWallet.Id,
                        CreatedAt = DateTime.Now,
                    });

                    _context.WorkerWallets.Update(workerWallet);
                    _context.ClientSpecifications.Update(clientDetails);

                    targetOrder.OrderStatus = Enums.OrderStatus.Cancelled.ToString();
                    _context.Orders.Update(targetOrder);

                    await _context.SaveChangesAsync();

                    //2.7 send notification with indept
                    await notificationServiceHandler.sendMessagetoAdmin("عمليه حذف طلب", orderId);
                    await notificationServiceHandler.sendMessagetoWorker(
                        13, (int)targetOrder.WorkerId!,
                        $"قام العميل {targetOrder.Client?.FullName} بإلغاء الطلب رقم #{targetOrder.Id}، وقيمة التعويض هي {cancelationvalue} تم اضافتها الى محفظتك."
                    );
                    return ResultDTO<string>.Success("Successfully deleted");

                }
                // 2.8 This time is early the date (no fee here; just cancellation)
                else
                {
                    targetOrder.OrderStatus = Enums.OrderStatus.Cancelled.ToString();
                    _context.Orders.Update(targetOrder);
                    await _context.SaveChangesAsync();
                    await notificationServiceHandler.sendMessagetoAdmin("عمليه حذف طلب", orderId);
                    await notificationServiceHandler.sendMessagetoWorker(
                        13, (int)targetOrder.WorkerId!,
                        $"قام العميل {targetOrder.Client?.FullName} بإلغاء الطلب رقم #{targetOrder.Id}."
                    );
                    return ResultDTO<string>.Success("Successfully deleted");
                }
            }
            // 3. The order is still published -> no fee for client
            else
            {

                targetOrder.OrderStatus = Enums.OrderStatus.Cancelled.ToString();

                await _context.Offers
                    .Where(o => o.OrderId == targetOrder.Id)
                    .ExecuteUpdateAsync(
                        o => o.SetProperty(p => p.OfferStatus, OfferStatus.Rejected.ToString())
                    );
                
                await _context.SaveChangesAsync();
                await notificationServiceHandler.sendMessagetoAdmin("عمليه حذف طلب", orderId);
                return ResultDTO<string>.Success("Successfully deleted");
            }
        }

        /// <summary>
        /// Get all non-cancelled orders projected to lightweight DTO.
        /// </summary>
        public async Task<ResultDTO<List<OrderGetAllDto>>> GetAllClientsAsync()
        {
            var AllOrders = await _context.Orders
                .Include(nameof(Order.Service))
                .Include(nameof(Order.City))
                .Include(nameof(Order.OrderImages))
                .Where(p => p.OrderStatus != Enums.OrderStatus.Cancelled.ToString())
                .ProjectTo<OrderGetAllDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return ResultDTO<List<OrderGetAllDto>>.Success(AllOrders);
        }

        /// <summary>
        /// Aggregate order details including offers, visits, and invoice into a single view model.
        /// </summary>
        public async Task<ResultDTO<OrderGetDetailsDto>> GetOrderDetails(int orderId)
        {
            // 1. Initiate DTO for result
            var resultDto = new OrderGetDetailsDto();

            // 2. Get Order Data
            var orderData = await _context.Orders
                .Include(o => o.OrderVisits)
                .ProjectTo<OrderGetDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(i => i.Id == orderId);

            // 3. Get Offer Data
            var targetOffers = await _context.Offers.Where(p => p.OrderId == orderId)
                .ProjectTo<OfferGetDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
            if (targetOffers.Count() > 0)
            {
                var workerId = targetOffers[0].Worker!.Id;
                var workerData = await _context.WorkerPortfolios.FindAsync(workerId);
                resultDto.WorkerData = _mapper.Map<WorkerPortfolioGetDTO>(workerData);
            }

            // 4. Get order visit data if exists
            if (orderData.OrderVisits != null)
            {
                resultDto.VisiteRequest = _mapper.Map<List<OrderVisitGetDTO>>(orderData.OrderVisits);
            }

            // 5. Get Invoice Data
            var orderInvoice = await _context.Invoices
                .Where(p => p.OrderId == orderId)
                .ProjectTo<InvoiceGetDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
            if (orderInvoice != null)
                resultDto.ClientOrdeInvoice = orderInvoice;

            //6. the result model handling
            resultDto.OrderData = orderData;
            resultDto.Offers = targetOffers;
            return ResultDTO<OrderGetDetailsDto>.Success(resultDto);
        }
    }
}
