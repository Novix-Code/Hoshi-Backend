using AutoMapper;
using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.OrderDTOs.InvoiceDTOs;
using Hoshi.DTOs.OrderDTOs.OrderDTOs;
using Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerHomeDTOs;
using Hoshi.Enums;
using Hoshi.Models.DashboardModels;
using Hoshi.Models.GlobalModels;
using Hoshi.Models.OrderModels;
using Hoshi.Models.UserModels.WorkerModels;
using Microsoft.EntityFrameworkCore;
using ClientRateDataDto = Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerHomeDTOs.ClientDataDto;

namespace Hoshi.Repositories.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly HoshiDbContext _hoshiDbContext;
        private readonly IMapper _mapper;
        public OrderService(HoshiDbContext hoshiDbContext, IMapper mapper)
        {
            _hoshiDbContext = hoshiDbContext;
            _mapper = mapper;
        }

        public async Task<ResultDTO<SubmittedOrderDetailsDto>> GetSubmittedOrderDetailsAsync(int orderId)
        {

            var order = await _hoshiDbContext.Orders
                .Include(o => o.Client)
                .Include(o => o.Service)
                    .ThenInclude(s => s.ServiceCategory)
                .Include(o => o.OrderImages)
                .Include(o => o.City)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                return ResultDTO<SubmittedOrderDetailsDto>.NotFound(new ErrorDTO
                {
                    ErrorAr = "لا يوجد طلب بهذا الرقم.",
                    ErrorEn = "there is no order with this number."
                });

            // Get client info
            var clientSpec = await _hoshiDbContext.ClientSpecifications
                .Include(cs => cs.User)
                .FirstOrDefaultAsync(cs => cs.UserId == order.ClientId);

            var clientData = clientSpec != null ? _mapper.Map<ClientDataDto>(clientSpec) : new ClientDataDto();

            var serviceData = order.Service != null ? _mapper.Map<ServiceDataDto>(order.Service) : new ServiceDataDto();
            var dto = _mapper.Map<SubmittedOrderDetailsDto>(order);
            dto.ClientData = clientData;
            dto.Service = serviceData;

            return ResultDTO<SubmittedOrderDetailsDto>.Success(dto);

        }

        public async Task<ResultDTO<OrderClientDetailsDto>> GetOrderClientDetailsAsync(int orderId)
        {
            var order = await _hoshiDbContext.Orders
                .Include(o => o.Client)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null || order.Client == null)
                return ResultDTO<OrderClientDetailsDto>.NotFound(new ErrorDTO
                {
                    ErrorAr = "الطلب غير موجود.",
                    ErrorEn = "order not found."
                });
            
            var clientSpec = await _hoshiDbContext.ClientSpecifications
                .Include(cs => cs.User)
                .FirstOrDefaultAsync(cs => cs.UserId == order.ClientId);
            
            var clientData = new ClientRateDataDto
            {
                ImageUrl = clientSpec.ImageURL, 
                Name = clientSpec.User.UserName,         
                RateRatio = clientSpec.RateRito, 
            };

            var clientRates = await _hoshiDbContext.Rates
                .Where(r => r.ClientId == clientSpec.UserId)
                .Select(r => new ClientRateDto
                {
                    WorkerName = r.Worker.UserName, 
                    Rate = r.RateValue,
                    Comment = r.Description
                })
                .ToListAsync();
            
            return ResultDTO<OrderClientDetailsDto>.Success(new OrderClientDetailsDto
            {
                ClientData = clientData,
                ClientRates = clientRates
            });
        }

        /// <summary>
        /// Completes an order by updating its status, handling all financial transactions, and sending notifications.
        /// </summary>
        public async Task<ResultDTO<bool>> CompleteOrderAsync(int orderId)
        {
            using var transaction = await _hoshiDbContext.Database.BeginTransactionAsync();
            try
            {
                // 1. Retrieve order and related data
                var order = await _hoshiDbContext.Orders
                    .Include(o => o.Client)
                    .Include(o => o.Worker)
                    .Include(o => o.OrderVisits)
                    .Include(o => o.OrderStatusHistory)
                    .FirstOrDefaultAsync(o => o.Id == orderId);

                if (order == null)
                    return ResultDTO<bool>.NotFound(new ErrorDTO
                    {
                        ErrorAr = "الطلب غير موجود.",
                        ErrorEn = "order not found."
                    });

                // 2. Change order status to Completed and add status history
                order.OrderStatus = OrderStatus.Completed;
                order.OrderStatusHistory.Add(new OrderStatusHistory
                {
                    OrderStatus = OrderStatus.Completed,
                    CreatedAt = DateTime.UtcNow,
                    OrderId = order.Id
                });

                // 3. Send notification to client (order completed)
                _hoshiDbContext.UserNotifications.Add(new UserNotification
                {
                    UserId = order.ClientId,
                    Description = "تم اكتمال طلبك.",
                    NotificationTypeId = 1 , // may change this later
                });

                // 4. Update completed orders count for worker and client
                var worker = await _hoshiDbContext.WorkerSpecifications.FirstOrDefaultAsync(w => w.UserId == order.WorkerId);
                if (worker != null)
                    worker.CompletedOrders++;
                var client = await _hoshiDbContext.ClientSpecifications.FirstOrDefaultAsync(c => c.UserId == order.ClientId);
                if (client != null)
                    client.CompletedOrders++;

                // --- Validations before financial operations ---
                if (client == null)
                    return ResultDTO<bool>.NotFound(new ErrorDTO
                    {
                        ErrorAr = "العميل غير موجود.",
                        ErrorEn = "Client not found."
                    });

                var invoices = _hoshiDbContext.Invoices.Where(i => i.OrderId == order.Id).ToList();
                if (invoices == null || !invoices.Any())
                    return ResultDTO<bool>.NotFound(new ErrorDTO
                    {
                        ErrorAr = "لا توجد فواتير للطلب.",
                        ErrorEn = "No invoices found for the order."
                    });

                if (order.OrderStatus == OrderStatus.Completed)
                    return ResultDTO<bool>.BadRequest(new ErrorDTO
                    {
                        ErrorAr = "الطلب مكتمل بالفعل.",
                        ErrorEn = "Order already completed."
                    });

                double totalClientCost = invoices.Sum(i => i.ClientTotalPrice);
                double totalWorkerCost = invoices.Sum(i => i.WorkerTotalPrice);
                double totalCommission = invoices.Sum(i => i.CommissionFee);
                double totalIndebtednessFee = invoices.Sum(i => i.ClientIndebtednessFee);

                if (totalClientCost < 0 || totalWorkerCost < 0 || totalCommission < 0 || totalIndebtednessFee < 0)
                    return ResultDTO<bool>.BadRequest(new ErrorDTO
                    {
                        ErrorAr = "قيمة مالية غير صحيحة.",
                        ErrorEn = "Invalid financial value."
                    });

                // --- 6. Deduct total cost from client ---
                // If balance is enough, deduct from balance. Else, set balance to 0 and add the rest to indebtedness.
                if (totalClientCost > 0)
                {
                    if (client.Balance >= totalClientCost)
                    {
                        client.Balance -= totalClientCost;
                    }
                    else
                    {
                        double remaining = totalClientCost - client.Balance;
                        client.Balance = 0;
                        client.Indebtedness += remaining;
                    }
                }

                // --- 7. Add worker's earnings to wallet ---
                var workerWallet = await _hoshiDbContext.WorkerWallets.FirstOrDefaultAsync(w => w.WorkerId == order.WorkerId);
                if (workerWallet == null)
                {
                    return ResultDTO<bool>.NotFound(new ErrorDTO
                    {
                        ErrorAr = "محفظة العامل غير موجودة.",
                        ErrorEn = "Worker wallet not found."
                    });
                }
                if (totalWorkerCost > 0)
                {
                    // Add to wallet and log as income
                    workerWallet.Balance += totalWorkerCost;
                    _hoshiDbContext.WorkerWalletHistories.Add(new WorkerWalletHistory
                    {
                        Title = $"إيداع أرباح الطلب رقم {order.Id}",
                        Value = totalWorkerCost,
                        IsIncome = true,
                        WorkerWalletId = workerWallet.Id
                    });
                }

                // --- 8. Deduct company commission from worker's wallet ---
                if (workerWallet.Balance < totalCommission)
                {
                    // If worker's wallet is insufficient for commission, set HitLimit and send notifications
                    workerWallet.HitLimit = true;
                    _hoshiDbContext.UserNotifications.Add(new UserNotification
                    {
                        UserId = order.WorkerId ?? 0,
                        Description = "رصيد محفظتك أقل من قيمة العمولة المطلوبة!",
                        NotificationTypeId = 1 , // may change this later

                    });

                    // Send notification to all admin users
                    var adminUsers = await _hoshiDbContext.Users
                        .Where(u => u.UserType == UserType.Admin.ToString())
                        .ToListAsync();

                    foreach (var admin in adminUsers)
                    {
                        _hoshiDbContext.UserNotifications.Add(new UserNotification
                        {
                            UserId = admin.Id,
                            Description = $"محفظة العامل رقم {order.WorkerId} أقل من قيمة العمولة المطلوبة!",
                            NotificationTypeId = 1 , // may change this later
                        });
                    }
                }
                else if (workerWallet.Balance >= totalCommission)
                {
                    // Deduct commission and log as expense
                    workerWallet.Balance -= totalCommission;
                    _hoshiDbContext.WorkerWalletHistories.Add(new WorkerWalletHistory
                    {
                        Title = $"خصم عمولة الطلب رقم {order.Id}",
                        Value = totalCommission,
                        IsIncome = false,
                        WorkerWalletId = workerWallet.Id
                    });
                    // Add to company revenue
                    _hoshiDbContext.CompanyRevenues.Add(new CompanyRevenue
                    {
                        Value = totalCommission,
                        CreatedAt = DateTime.UtcNow,
                        OrderId = order.Id
                    });
                }

                // --- 9. Handle any additional client indebtedness fees ---
                if (totalIndebtednessFee > 0)
                {
                    if (client.Balance >= totalIndebtednessFee)
                    {
                        client.Balance -= totalIndebtednessFee;
                    }
                    else
                    {
                        double remaining = totalIndebtednessFee - client.Balance;
                        client.Balance = 0;
                        client.Indebtedness += remaining;
                    }
                }

                await _hoshiDbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return ResultDTO<bool>.Success();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return ResultDTO<bool>.InternalServerError(new ErrorDTO
                {
                    ErrorAr = "حدث خطأ في الخادم.",
                    ErrorEn = $"Internal server error: {ex.Message}"
                });
            }
        }

        public async Task<ResultDTO<object>> GetAssignedOrderAsync(int orderId)
        {
            var order = await _hoshiDbContext.Orders
                .Include(o => o.Client)
                .Include(o => o.Service)
                .Include(o => o.OrderStatusHistory)
                .Include(o => o.OrderImages)
                .Include(o => o.OrderVisits)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                return ResultDTO<object>.NotFound(new ErrorDTO
                {
                    ErrorAr = "الطلب غير موجود.",
                    ErrorEn = "Order not found."
                });

            // Get invoices for this order
            var invoices = await _hoshiDbContext.Invoices
                .Where(i => i.OrderId == order.Id)
                .ToListAsync();

            // Get client data
            var clientSpec = await _hoshiDbContext.ClientSpecifications
                .Include(cs => cs.User)
                .FirstOrDefaultAsync(cs => cs.UserId == order.ClientId);

            var clientData = clientSpec != null ? new ClientRateDataDto
            {
                ImageUrl = clientSpec.ImageURL,
                Name = clientSpec.User.UserName,
                RateRatio = clientSpec.RateRito,
            } : null;

            
            var orderDto = _mapper.Map<OrderGetDTO>(order);
            var clientDataDto = clientData != null ? _mapper.Map<ClientDataDto>(clientData) : null;
            var orderInvoices = _mapper.Map<List<InvoiceGetDTO>>(invoices);
            
            return ResultDTO<object>.Success(new
            {
                Order = orderDto,
                ClientData = clientDataDto,
                OrderInvoices = orderInvoices
            });
        }

    }
}
