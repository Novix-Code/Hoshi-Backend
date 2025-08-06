using AutoMapper;
using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.OrderDTOs.OfferDTOs;
using Hoshi.Enums;
using Hoshi.Models.DashboardModels;
using Hoshi.Models.OrderModels;
using Hoshi.Models.UserModels.WorkerModels;
using Hoshi.Repositories.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Hoshi.Repositories.WorkerOfferService
{
    public class WorkerOfferService : IWorkerOfferService
    {
        private readonly HoshiDbContext _hoshiDbContext;
        private readonly IMapper _mapper;
        private readonly IHubContext<NotificationHub, INotificationHub> _hubContext;

        public WorkerOfferService(HoshiDbContext hoshiDbContext, IMapper mapper, IHubContext<NotificationHub, INotificationHub> hubContext)
        {
            _hoshiDbContext = hoshiDbContext;
            _mapper = mapper;
            _hubContext = hubContext;
        }

        public async Task<ResultDTO<CreateOfferResponseDto>> CreateOfferAsync(OfferPostDTO dto)
        {
            using var transaction = await _hoshiDbContext.Database.BeginTransactionAsync();
            try
            {
                // 1. Validate order and worker existence
                var order = await _hoshiDbContext.Orders
                    .Include(o => o.Client)
                    .FirstOrDefaultAsync(o => o.Id == dto.OrderId);
                if (order == null)
                    return ResultDTO<CreateOfferResponseDto>.NotFound(new ErrorDTO
                    {
                        ErrorAr = "الطلب غير موجود.",
                        ErrorEn = "Order not found."
                    });

                var worker = await _hoshiDbContext.Users.FindAsync(dto.WorkerId);
                if (worker == null)
                    return ResultDTO<CreateOfferResponseDto>.NotFound(new ErrorDTO
                    {
                        ErrorAr = "العامل غير موجود.",
                        ErrorEn = "Worker not found."
                    });

                // 2. Create the offer
                var offer = _mapper.Map<Offer>(dto);
                _hoshiDbContext.Offers.Add(offer);
                await _hoshiDbContext.SaveChangesAsync();

                // 3. Calculate fees
                var visitFee = await _hoshiDbContext.Fees.FirstOrDefaultAsync(f => f.FeeType == FeeType.VisitingFee);
                var cancellationFee = await _hoshiDbContext.Fees.FirstOrDefaultAsync(f => f.FeeType == FeeType.CancellationFee);
                var commissionFee = await _hoshiDbContext.Fees.FirstOrDefaultAsync(f => f.FeeType == FeeType.CommissionFee);

                // 4. Calculate promotion discount
                var promotionTitle = string.Empty;
                var promotionValue = 0.0;

                var promotion = await _hoshiDbContext.Promotions.FirstAsync();
                if (promotion != null)
                {
                    promotionTitle = $"{promotion.TitleFirstPart} {promotion.TitleSecondPart}";
                    promotionValue = promotion.IsPercentage ? (dto.OfferedPrice * promotion.Value / 100) : promotion.Value;
                }

                // 5. Calculate final amounts
                var workerRevenue = dto.OfferedPrice - (commissionFee.MainFees);
                var clientWillPay = dto.OfferedPrice + (visitFee.MainFees) - promotionValue;

                // 6. Build response DTO
                var response = new CreateOfferResponseDto
                {
                    OfferId = offer.Id,
                    ClientName = order.Client?.UserName ?? string.Empty,
                    OrderId = offer.OrderId,
                    OfferedPrice = offer.OfferedPrice,
                    VisitFee = visitFee.MainFees,
                    CancellationFee = cancellationFee.MainFees,
                    ServiceFee = commissionFee.MainFees,
                    PromotionTitle = promotionTitle,
                    PromotionValue = promotionValue,
                    WorkerRevenue = workerRevenue,
                    ClientWillPay = clientWillPay
                };

                await transaction.CommitAsync();

                var allAdmins = await _hoshiDbContext.UserRoles.Where(p => p.RoleId == 2)
                                                               .Include(p => p.UserId).ToListAsync();
                if (allAdmins.Any())
                {
                    foreach (var admin in allAdmins)
                    {
                        await _hoshiDbContext.AdminNotifications.AddAsync(new AdminNotification
                        {
                            Title = "عمليه اضافة عرض",
                            Content = $"offer Id: {offer.Id}",
                            CreatedAt = DateTime.UtcNow,
                            IsRead = false,
                            AdminId = admin.UserId,
                        });
                        await _hoshiDbContext.SaveChangesAsync();
                    }
                    await _hubContext.Clients.Group("admin").ReceiveMessage("عمليه اضافة عرض");

                }


                return ResultDTO<CreateOfferResponseDto>.Success(response);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return ResultDTO<CreateOfferResponseDto>.InternalServerError(new ErrorDTO
                {
                    ErrorAr = "حدث خطأ في الخادم.",
                    ErrorEn = $"Internal server error: {ex.Message}"
                });
            }
        }

        public async Task<ResultDTO<bool>> ConfirmOfferAsync(int offerId)
        {
            using var transaction = await _hoshiDbContext.Database.BeginTransactionAsync();
            try
            {
                var offer = await _hoshiDbContext.Offers.FindAsync(offerId);
                if (offer == null)
                {
                    return ResultDTO<bool>.NotFound(new ErrorDTO
                    {
                        ErrorAr = "العرض غير موجود.",
                        ErrorEn = "Offer not found."
                    });
                }

                offer.OfferStatus = OfferStatus.Waitting;
                offer.IsConfirmed = true;
                _hoshiDbContext.Offers.Update(offer);
                await _hoshiDbContext.SaveChangesAsync();
                await transaction.CommitAsync();
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

        public async Task<ResultDTO<bool>> CancelOfferAsync(int offerId)
        {
            using var transaction = await _hoshiDbContext.Database.BeginTransactionAsync();
            try
            {
                var offer = await _hoshiDbContext.Offers.FindAsync(offerId);
                if (offer == null)
                {
                    return ResultDTO<bool>.NotFound(new ErrorDTO
                    {
                        ErrorAr = "العرض غير موجود.",
                        ErrorEn = "Offer not found."
                    });
                }

                offer.IsDeleted = true;
                offer.OfferStatus = OfferStatus.Cancelled;

                // Apply cancellation fee logic (for worker)
                // Get the cancellation fee for this service
                var order = await _hoshiDbContext.Orders.FindAsync(offer.OrderId);
                if (order != null)
                {
                    double workerCancellationFee = await _hoshiDbContext.Fees
                        .Where(f => f.ServiceId == order.ServiceId && f.FeeType == FeeType.CancellationFee && !f.IsSpecial)
                        .Select(f => f.MainFees)
                        .FirstOrDefaultAsync();

                    var workerWallet = await _hoshiDbContext.WorkerWallets.FirstOrDefaultAsync(w => w.WorkerId == order.WorkerId);
                    if (workerWallet == null)
                    {
                        return ResultDTO<bool>.NotFound(new ErrorDTO
                        {
                            ErrorAr = "محفظة العامل غير موجودة.",
                            ErrorEn = "Worker wallet not found."
                        });
                    }

                    if (workerWallet.Balance < workerCancellationFee)
                    {
                        return ResultDTO<bool>.NotFound(new ErrorDTO
                        {
                            ErrorAr = "رصيد المحفظة غير كافي.",
                            ErrorEn = "Worker wallet balance is not enough."
                        });
                    }

                    workerWallet.Balance -= workerCancellationFee;
                    _hoshiDbContext.WorkerWallets.Update(workerWallet);
                    
                    _hoshiDbContext.WorkerWalletHistories.Add(new WorkerWalletHistory
                    {
                        Title = "Cancellation Fee",
                        Value = workerCancellationFee,
                        IsIncome = false,
                        WorkerWalletId = workerWallet.Id
                    });
                    
                    await _hoshiDbContext.SaveChangesAsync();
                }

                _hoshiDbContext.Offers.Update(offer);
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
        
    }
}
