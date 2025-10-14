using AutoMapper;
using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.DashboardDTOs;
using Hoshi.DTOs.OrderDTOs.InvoiceDTOs;
using Hoshi.DTOs.OrderDTOs.OrderDTOs;
using Hoshi.DTOs.ServiceDTOs.JobDTOs;
using Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerHomeDTOs;
using Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerSpecificationDTOs;
using Hoshi.Enums;
using Hoshi.Models.ChatModels;
using Hoshi.Models.DashboardModels;
using Hoshi.Models.GlobalModels;
using Hoshi.Models.OrderModels;
using Hoshi.Models.UserModels.WorkerModels;
using Hoshi.Repositories.Hubs;
using Hoshi.Repositories.NotificationService;
using Hoshi.Repositories.WorkerWalletService;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Hoshi.Repositories.OrderService
{
    public class OrderService : IOrderService
    {
        /// <summary>
        /// Handles order detail projections, completion workflow, and dashboard details.
        /// Wraps transactional updates and notification fan-out.
        /// </summary>
        private readonly HoshiDbContext _hoshiDbContext;
        private readonly IMapper _mapper;
        private readonly IHubContext<NotificationHub, INotificationHub> _hubContext;
        private readonly INotificationServiceHandler notificationServiceHandler;
        private readonly IWorkerWalletService WalletService;


        public OrderService(
            HoshiDbContext hoshiDbContext, 
            IMapper mapper, 
            IHubContext<NotificationHub, INotificationHub> hubContext, 
            INotificationServiceHandler notificationServiceHandler, 
            IWorkerWalletService walletService
        )
        {
            _hoshiDbContext = hoshiDbContext;
            _mapper = mapper;
            _hubContext = hubContext;
            WalletService = walletService;
            this.notificationServiceHandler = notificationServiceHandler;
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

            var clientData = new ClientDataDto
            {
                ImageUrl = clientSpec!.User!.ImageURL!,
                Name = clientSpec!.User!.UserName!,
                RateRatio = clientSpec.RateRito,
            };

            var clientRates = await _hoshiDbContext.Rates
                .Where(r => r.ClientId == clientSpec.UserId)
                .Select(r => new ClientRateDto
                {
                    WorkerName = r.Worker!.UserName!,
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
        public async Task<ResultDTO<object>> CompleteOrderAsync(int orderId)
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
                    return ResultDTO<object>.NotFound(new ErrorDTO
                    {
                        ErrorAr = "الطلب غير موجود.",
                        ErrorEn = "order not found."
                    });

                // 2. Change order status to Completed and add status history

                if (order.OrderStatus == OrderStatus.Completed.ToString())
                    return ResultDTO<object>.BadRequest(new ErrorDTO
                    {
                        ErrorAr = "الطلب مكتمل بالفعل.",
                        ErrorEn = "Order already completed."
                    });
                order.OrderStatus = OrderStatus.Completed.ToString();
                order.OrderStatusHistory!.Add(new OrderStatusHistory
                {
                    OrderStatus = OrderStatus.Completed,
                    CreatedAt = DateTime.UtcNow,
                    OrderId = order.Id
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
                    return ResultDTO<object>.NotFound(new ErrorDTO
                    {
                        ErrorAr = "العميل غير موجود.",
                        ErrorEn = "Client not found."
                    });

                var invoices = _hoshiDbContext.Invoices.Where(i => i.OrderId == order.Id).ToList();
                if (invoices == null || !invoices.Any())
                    return ResultDTO<object>.NotFound(new ErrorDTO
                    {
                        ErrorAr = "لا توجد فواتير للطلب.",
                        ErrorEn = "No invoices found for the order."
                    });



                double totalClientCost = invoices.Sum(i => i.ClientTotalPrice);
                double totalWorkerCost = invoices.Sum(i => i.WorkerTotalPrice);



                double totalCommission = invoices.Sum(i => i.CommissionFee);
                double totalIndebtednessFee = invoices.Sum(i => i.ClientIndebtednessFee);

                if (totalClientCost < 0 || totalWorkerCost < 0 || totalCommission < 0 || totalIndebtednessFee < 0)
                    return ResultDTO<object>.BadRequest(new ErrorDTO
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
                    return ResultDTO<object>.NotFound(new ErrorDTO
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
                        NotificationTypeId = 1, // may change this later

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
                            NotificationTypeId = 1, // may change this later
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

                //  Wallet difference adjustment
                if (totalClientCost > totalWorkerCost)
                {
                    try
                    {
                        var diff = totalClientCost - totalWorkerCost;
                        await WalletService.DeductFromWalletAsync((int)order.WorkerId, diff, "فرق بين العميل والعامل");
                    }
                    catch
                    {
                        return ResultDTO<object>.NotFound(new ErrorDTO
                        {
                            ErrorAr = "خطأ في اضافه walletHistory",
                            ErrorEn = "error in adding walletHistory"
                        });
                    }
                }
                else if (totalWorkerCost > totalClientCost)
                {
                    try
                    {
                        var diff = totalWorkerCost - totalClientCost;
                        await WalletService.AddToWalletAsync((int)order.WorkerId, diff, "فرق بين العامل والعميل");
                    }
                    catch
                    {
                        return ResultDTO<object>.NotFound(new ErrorDTO
                        {
                            ErrorAr = "خطأ في اضافه walletHistory",
                            ErrorEn = "error in adding walletHistory"
                        });
                    }
                }
                
                client.Balance = 0;
                client.Indebtedness = 0;

                await _hoshiDbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                // send notification
                await notificationServiceHandler.sendMessagetoAdmin("عمليه استكمال طلب", orderId);
                await notificationServiceHandler.sendMessagetoClient(5, order.ClientId, $"تم اكتمال طلبك رقم #{order.Id}.");
                return ResultDTO<object>.Success(new
                {
                    Success = true,
                    Message = "Order Completed Successfully"
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return ResultDTO<object>.InternalServerError(new ErrorDTO
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

            var clientData = clientSpec != null ? new ClientDataDto
            {
                ImageUrl = clientSpec!.User!.ImageURL!,
                Name = clientSpec!.User!.UserName!,
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

        public async Task<ResultDTO<DashboardOrderDetailsResponseDTO>> GetDashboardOrderDetailsAsync(int orderId)
        {
            var order = await _hoshiDbContext.Orders
                .Include(o => o.Client)
                .Include(o => o.Service)
                .Include(o => o.City)
                .Include(o => o.OrderImages)
                .Include(o => o.Worker)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                return ResultDTO<DashboardOrderDetailsResponseDTO>.NotFound(new ErrorDTO
                {
                    ErrorAr = "الطلب غير موجود.",
                    ErrorEn = "Order not found."
                });

            var orderDto = _mapper.Map<OrderGetDTO>(order);

            // Client Data
            var client = orderDto.Client;

            var getClientData = await _hoshiDbContext.ClientSpecifications
                .Include(cs => cs.User)
                .FirstOrDefaultAsync(cs => cs.UserId == client.Id);

            if (getClientData == null)
                return ResultDTO<DashboardOrderDetailsResponseDTO>.NotFound(new ErrorDTO
                {
                    ErrorAr = "لا يوجد بيانات عن العميل.",
                    ErrorEn = "No client data found."
                });

            var clientData = _mapper.Map<ClientDataDto>(getClientData);

            var getWorkerData = await _hoshiDbContext.WorkerSpecifications
                .Include(ws => ws.User)
                .Include(ws => ws.Job)
                .Include(ws => ws.LivingCity)
                .FirstOrDefaultAsync(ws => ws.UserId == order.WorkerId);

            if (getWorkerData == null)
                return ResultDTO<DashboardOrderDetailsResponseDTO>.NotFound(new ErrorDTO
                {
                    ErrorAr = "لا يوجد بيانات عن العميل.",
                    ErrorEn = "No client data found."
                });

            var workerDataDto = _mapper.Map<WorkerSpecificationGetDTO>(getWorkerData);

            // // Get Worker Job Title
            // string workerJob = string.Empty;
            // if (workerDataDto?.Job != null && !string.IsNullOrEmpty(workerDataDto.Job.JobTitle))
            // {
            //     workerJob = workerDataDto.Job.JobTitle;
            // }

            // Get Worker Wallet Balance
            double workerBalance = 0;
            var workerWallet = await _hoshiDbContext.WorkerWallets
                .FirstOrDefaultAsync(w => w.WorkerId == workerDataDto!.Id);
            if (workerWallet != null)
            {
                workerBalance = workerWallet.Balance;
            }

            // Get Cancelled Offers Count
            int cancelledOffers = await _hoshiDbContext.Offers
                .CountAsync(o => o.WorkerId == workerDataDto!.Id && o.OfferStatus == OfferStatus.Cancelled.ToString());

            var workerData = new WorkerDataDTO
            {
                ImageUrl = workerDataDto.User!.ImageURL!,
                Email = workerDataDto.User.Email,
                FullName = workerDataDto.User.UserName,
                Job = workerDataDto.Job!,
                IsCompany = workerDataDto?.IsCompany ?? false,
                RateRatio = workerDataDto?.RateRito ?? 0,
                CompletedOrders = workerDataDto?.CompletedOrders ?? 0,
                CancelledOffers = cancelledOffers,
                Balance = workerBalance
            };

            var response = new DashboardOrderDetailsResponseDTO
            {
                OrderId = orderDto.Id,
                ClientData = clientData,
                Description = orderDto.Description,
                OrderStatus = orderDto.OrderStatusHistory,
                City = orderDto.City!,
                Location = order.Location,
                ServicingDatetime = order.ServicingDateTime,
                OfferedPrice = order.ProposalPrice,
                OrderImages = orderDto.OrderImages!,
                WorkerData = workerData
            };

            return ResultDTO<DashboardOrderDetailsResponseDTO>.Success(response);
        }
    }
}
