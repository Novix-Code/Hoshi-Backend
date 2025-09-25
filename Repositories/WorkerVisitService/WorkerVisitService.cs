using AutoMapper;
using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.OrderDTOs.OrderVisitDTOs;
using Hoshi.Enums;
using Hoshi.Models.DashboardModels;
using Hoshi.Models.GlobalModels;
using Hoshi.Models.OrderModels;
using Hoshi.Models.UserModels.WorkerModels;
using Hoshi.Repositories.NotificationService;
using Microsoft.EntityFrameworkCore;

namespace Hoshi.Repositories.WorkerVisitService
{
    /// <summary>
    /// Executes worker visit flows: add, complete, and cancel, with fee calculations and notifications.
    /// </summary>
    public class WorkerVisitService : IWorkerVisitService
    {
        private readonly HoshiDbContext _hoshiDbContext;
        private readonly IMapper _mapper;
        private readonly INotificationServiceHandler _notificationServiceHandler;
        public WorkerVisitService(HoshiDbContext hoshiDbContext, IMapper mapper, INotificationServiceHandler notificationServiceHandler)
        {
            _hoshiDbContext = hoshiDbContext;
            _mapper = mapper;
            _notificationServiceHandler = notificationServiceHandler;
        }
        /// <summary>
        /// Create a visit, compute fees, and upsert temp invoice entries for the visit.
        /// </summary>
        public async Task<ResultDTO<OrderVisitGetDTO>> AddVisitAsync(OrderVisitPostDTO dto)
        {
            using var transaction = await _hoshiDbContext.Database.BeginTransactionAsync();
            try
            {
                var order = await _hoshiDbContext.Orders.FindAsync(dto.OrderId);
                if (order == null)
                {
                    return ResultDTO<OrderVisitGetDTO>.NotFound(new ErrorDTO
                    {
                        ErrorAr = "الطلب غير موجود.",
                        ErrorEn = "Order not found."
                    });
                }
                
                var visit = _mapper.Map<OrderVisit>(dto);
                _hoshiDbContext.OrderVisits.Add(visit);
                await _hoshiDbContext.SaveChangesAsync();

                var (visitMain, visitMin, visitMax) = await GetFeeAsync(FeeType.VisitingFee, order);
                var (cancelMain, cancelMin, cancelMax) = await GetFeeAsync(FeeType.CancellationFee, order);
                var (commissionMain, commissionMin, commissionMax) = await GetFeeAsync(FeeType.CommissionFee, order);

                // This to get the value of the Fee according to its range
                var visitFeeValue = Clamp(
                    ValueFromPercentage(dto.VisitPrice, visitMain),
                    visitMin,
                    visitMax
                );
                var cancellationFeeValue = Clamp(
                    ValueFromPercentage(dto.VisitPrice, cancelMain),
                    cancelMin,
                    cancelMax
                );
                var commissionFeeValue = Clamp(
                    ValueFromPercentage(dto.VisitPrice, commissionMain),
                    commissionMin,
                    commissionMax
                );

                var invoice = await _hoshiDbContext.Invoices.FirstOrDefaultAsync(i => i.OrderId == order.Id);

                var commissionAfterWorkerPromo = Math.Max(commissionFeeValue - 0, 0.0);
                var workerRevenue = visit.VisitPrice - commissionAfterWorkerPromo;
                var clientWillPay = visit.VisitPrice + (invoice?.ClientIndebtednessFee ?? 0) - 0;

                var existingTemp = await _hoshiDbContext.TempInvoices.FirstOrDefaultAsync(t => t.OrderVisitId == visit.Id);
                if (existingTemp == null)
                {
                    _hoshiDbContext.TempInvoices.Add(new TempInvoice
                    {
                        OrderPrice = visit.VisitPrice,
                        CommissionFee = commissionFeeValue,
                        VisitingFee = visitFeeValue,
                        CancellationFee = cancellationFeeValue,
                        WorkerPromotionFee = 0,
                        ClientPromotionFee = 0,
                        ClientIndebtednessFee = 0.0,
                        ClientTotalPrice = clientWillPay,
                        WorkerTotalPrice = workerRevenue,
                        OrderVisitId = visit.Id
                    });
                }
                else
                {
                    existingTemp.OrderPrice = visit.VisitPrice;
                    existingTemp.CommissionFee = commissionFeeValue;
                    existingTemp.VisitingFee = visitFeeValue;
                    existingTemp.CancellationFee = cancellationFeeValue;
                    existingTemp.ClientIndebtednessFee = 0.0;
                    existingTemp.ClientTotalPrice = clientWillPay;
                    existingTemp.WorkerTotalPrice = workerRevenue;
                    _hoshiDbContext.TempInvoices.Update(existingTemp);
                }
                await _hoshiDbContext.SaveChangesAsync();
                var visitDto = _mapper.Map<OrderVisitGetDTO>(visit);
                await transaction.CommitAsync();
                return ResultDTO<OrderVisitGetDTO>.Success(visitDto);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return ResultDTO<OrderVisitGetDTO>.InternalServerError(new ErrorDTO
                {
                    ErrorAr = "حدث خطأ في الخادم.",
                    ErrorEn = $"Internal server error: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Complete a visit: deduct client balance/indebtedness, credit worker wallet, deduct commission, add revenue.
        /// </summary>
        public async Task<ResultDTO<bool>> CompleteVisitAsync(int visitId)
        {
            using var transaction = await _hoshiDbContext.Database.BeginTransactionAsync();
            try
            {
                var visit = await _hoshiDbContext.OrderVisits
                    .Include(v => v.Order)
                        .ThenInclude(o => o.Client)
                    .Include(v => v.Order)
                        .ThenInclude(o => o.Worker).Include(p=>p.OrderId)
                    .FirstOrDefaultAsync(v => v.Id == visitId && v.VisitStatus != VisitStatus.Completed.ToString());
                var targetorder = await _hoshiDbContext.Orders.FindAsync(visit.OrderId);
                if (visit == null)
                {
                    return ResultDTO<bool>.NotFound(new ErrorDTO
                    {
                        ErrorAr = "الزيارة غير موجودة.",
                        ErrorEn = "Visit not found."
                    });
                }
                
                // Update visit status
                visit.VisitStatus = VisitStatus.Completed.ToString();
                
                // Get client and validate
                var client = await _hoshiDbContext.ClientSpecifications.FirstOrDefaultAsync(c => c.UserId == visit.Order.ClientId);
                if (client == null)
                {
                    return ResultDTO<bool>.NotFound(new ErrorDTO
                    {
                        ErrorAr = "العميل غير موجود.",
                        ErrorEn = "Client not found."
                    });
                }

                // Visit financial calculations
                double visitPrice = visit.VisitPrice;
                if (visitPrice <= 0)
                {
                    return ResultDTO<bool>.BadRequest(new ErrorDTO
                    {
                        ErrorAr = "سعر الزيارة غير صالح.",
                        ErrorEn = "Invalid visit price."
                    });
                }

                // Get fees from Fee table for this service
                int serviceId = visit.Order.ServiceId;
                double commissionFee = await _hoshiDbContext.Fees
                    .Where(f => f.ServiceId == serviceId && f.FeeType == FeeType.CommissionFee.ToString() && !f.IsSpecial)
                    .Select(f => f.MainFees)
                    .FirstOrDefaultAsync();

                double workerEarnings = visitPrice - commissionFee;

                // Client balance deduction
                if (client.Balance >= visitPrice)
                {
                    client.Balance -= visitPrice;
                }
                else
                {
                    double shortage = visitPrice - client.Balance;
                    client.Balance = 0;
                    client.Indebtedness += shortage;
                }

                // Worker wallet credit
                var workerWallet = await _hoshiDbContext.WorkerWallets.FirstOrDefaultAsync(w => w.WorkerId == visit.Order.WorkerId);
                if (workerWallet == null)
                {
                    return ResultDTO<bool>.NotFound(new ErrorDTO
                    {
                        ErrorAr = "محفظة العامل غير موجودة.",
                        ErrorEn = "Worker wallet not found."
                    });
                }

                workerWallet.Balance += workerEarnings;
                _hoshiDbContext.WorkerWallets.Update(workerWallet);

                // Log worker income
                _hoshiDbContext.WorkerWalletHistories.Add(new WorkerWalletHistory
                {
                    Title = "Visit Completion Income",
                    Value = workerEarnings,
                    IsIncome = true,
                    WorkerWalletId = workerWallet.Id
                });

                // Company commission deduction from worker wallet
                if (workerWallet.Balance >= commissionFee)
                {
                    workerWallet.Balance -= commissionFee;
                    _hoshiDbContext.WorkerWalletHistories.Add(new WorkerWalletHistory
                    {
                        Title = "Commission Fee",
                        Value = commissionFee,
                        IsIncome = false,
                        WorkerWalletId = workerWallet.Id
                    });

                    // Add company revenue
                    _hoshiDbContext.CompanyRevenues.Add(new CompanyRevenue
                    {
                        OrderId = visit.OrderId,
                        Value = commissionFee ,
                    });
                }
                else
                {
                    // Notify about insufficient balance
                    _hoshiDbContext.UserNotifications.Add(new UserNotification
                    {
                        UserId = visit.Order.WorkerId ?? 0,
                        Description = "رصيد المحفظة غير كافي لخصم العمولة.",
                        NotificationTypeId = 1 , // may change this later

                    });
                }

                // Create visit invoice
                _hoshiDbContext.Invoices.Add(new Invoice
                {
                    OrderPrice = visitPrice,
                    CommissionFee = commissionFee,
                    ClientTotalPrice = visitPrice,
                    WorkerTotalPrice = workerEarnings,
                    OrderId = visit.OrderId,
                    OrderVisitId = visit.Id
                });

                _hoshiDbContext.OrderVisits.Update(visit);
                _hoshiDbContext.ClientSpecifications.Update(client);
                await transaction.CommitAsync();
                await _hoshiDbContext.SaveChangesAsync();
                await _notificationServiceHandler.sendMessagetoWorker( 8, (int)targetorder.WorkerId, $"تم قبول طلب زيارتك على الطلب رقم #{targetorder.Id}.");

                return ResultDTO<bool>.Success(true);
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

        /// <summary>
        /// Cancel a visit: apply cancellation fee to worker wallet if configured and log company revenue.
        /// </summary>
        public async Task<ResultDTO<bool>> CancelVisitAsync(int visitId)
        {
            using var transaction = await _hoshiDbContext.Database.BeginTransactionAsync();
            try
            {
                var visit = await _hoshiDbContext.OrderVisits
                    .Include(v => v.Order)
                    .FirstOrDefaultAsync(v => v.Id == visitId && v.VisitStatus != VisitStatus.Cancelled.ToString());

                if (visit == null)
                {
                    return ResultDTO<bool>.NotFound(new ErrorDTO
                    {
                        ErrorAr = "الزيارة غير موجودة.",
                        ErrorEn = "Visit not found."
                    });
                }

                // Mark as canceled
                visit.VisitStatus = VisitStatus.Cancelled.ToString();

                // Apply cancellation fee logic (for worker)
                if (visit.Order != null)
                {
                    double visitCancellationFee = await _hoshiDbContext.Fees
                        .Where(f => f.ServiceId == visit.Order.ServiceId && f.FeeType == FeeType.CancellationFee.ToString() && !f.IsSpecial)
                        .Select(f => f.MainFees)
                        .FirstOrDefaultAsync();

                    if (visitCancellationFee > 0)
                    {
                        var workerWallet = await _hoshiDbContext.WorkerWallets.FirstOrDefaultAsync(w => w.WorkerId == visit.Order.WorkerId);
                        if (workerWallet == null)
                        {
                            return ResultDTO<bool>.NotFound(new ErrorDTO
                            {
                                ErrorAr = "محفظة العامل غير موجودة.",
                                ErrorEn = "Worker wallet not found."
                            });
                        }

                        if (workerWallet.Balance < visitCancellationFee)
                        {
                            return ResultDTO<bool>.BadRequest(new ErrorDTO
                            {
                                ErrorAr = "رصيد المحفظة غير كافي لدفع رسوم الإلغاء.",
                                ErrorEn = "Worker wallet balance is not enough for cancellation fee."
                            });
                        }

                        // Deduct cancellation fee from worker wallet
                        workerWallet.Balance -= visitCancellationFee;
                        _hoshiDbContext.WorkerWallets.Update(workerWallet);

                        // Log cancellation fee transaction
                        _hoshiDbContext.WorkerWalletHistories.Add(new WorkerWalletHistory
                        {
                            Title = "Visit Cancellation Fee",
                            Value = visitCancellationFee,
                            IsIncome = false,
                            WorkerWalletId = workerWallet.Id
                        });

                        // Add company revenue from cancellation fee
                        _hoshiDbContext.CompanyRevenues.Add(new CompanyRevenue
                        {
                            OrderId = visit.OrderId,
                            Value = visitCancellationFee
                        });
                    }
                }

                _hoshiDbContext.OrderVisits.Update(visit);
                await transaction.CommitAsync();
                await _hoshiDbContext.SaveChangesAsync();
                return ResultDTO<bool>.Success(true);
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

        // Create/Update TempInvoice for the visit with full fee and promotion logic
        public async Task<(double main, double min, double max)> GetFeeAsync(FeeType type,Order order)
        {
            var special = await _hoshiDbContext.Fees
                .Where(f => f.ServiceId == order.ServiceId && f.FeeType == type.ToString() && f.IsSpecial)
                .Select(f => new { f.MainFees, f.MinFees, f.MaxFees })
                .FirstOrDefaultAsync();
            if (special != null)
                return (special.MainFees, special.MinFees, special.MaxFees);

            var basic = await _hoshiDbContext.Fees
                .Where(f => f.FeeType == type.ToString() && !f.IsSpecial)
                .Select(f => new { f.MainFees, f.MinFees, f.MaxFees })
                .FirstOrDefaultAsync();
            if (basic == null)
                return (0, 0, 0);
            return (basic.MainFees, basic.MinFees, basic.MaxFees);
        }

        public double Clamp(double value, double min, double max)
        {
            if (max > 0 && value > max) return max;
            if (min > 0 && value < min) return min;
            return value;
        }

        public double ValueFromPercentage(double value, double percentage)
        {
            if(percentage <= 0) return value;

            return value * (percentage / 100) ;
        }
    }
}
