using AutoMapper;
using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.OrderDTOs.OfferDTOs;
using Hoshi.Enums;
using Hoshi.Models.DashboardModels;
using Hoshi.Models.OrderModels;
using Hoshi.Models.UserModels.WorkerModels;
using Hoshi.Repositories.Hubs;
using Hoshi.Repositories.NotificationService;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Hoshi.Repositories.WorkerOfferService
{
    public class WorkerOfferService : IWorkerOfferService
    {
        private readonly HoshiDbContext _hoshiDbContext;
        private readonly IMapper _mapper;
        private readonly IHubContext<NotificationHub, INotificationHub> _hubContext;
        private readonly INotificationServiceHandler notificationServiceHandler;


        public WorkerOfferService(HoshiDbContext hoshiDbContext, IMapper mapper, IHubContext<NotificationHub, INotificationHub> hubContext, INotificationServiceHandler notificationServiceHandler)
        {
            _hoshiDbContext = hoshiDbContext;
            _mapper = mapper;
            _hubContext = hubContext;
            this.notificationServiceHandler = notificationServiceHandler;
        }

        public async Task<ResultDTO<CreateOfferResponseDto>> CreateOfferAsync(OfferPostDTO dto)
        {
            using var transaction = await _hoshiDbContext.Database.BeginTransactionAsync();
            try
            {
                // 1. Validate order and worker existence
                var order = await _hoshiDbContext.Orders
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

                var promotion = await _hoshiDbContext.Promotions.FirstOrDefaultAsync();
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
                // send notification
                await notificationServiceHandler.sendMessagetoAdmin("عمليه اضافة عرض", offer.Id);
                await notificationServiceHandler.sendMessagetoClient("تم اضافة عرض على الطلب الخاص بك" ,order.ClientId);
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

        public async Task<ResultDTO<string>> ConfirmOfferAsync(int offerId)
        {
            using var transaction = await _hoshiDbContext.Database.BeginTransactionAsync();
            try
            {
                var offer = await _hoshiDbContext.Offers.FindAsync(offerId);
                if (offer == null)
                {
                    return ResultDTO<string>.NotFound(new ErrorDTO
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
                return ResultDTO<string>.Success("Offer Confirmed");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return ResultDTO<string>.InternalServerError(new ErrorDTO
                {
                    ErrorAr = "حدث خطأ في الخادم.",
                    ErrorEn = $"Internal server error: {ex.Message}"
                });
            }
        }

        public async Task<ResultDTO<string>> CancelOfferAsync(int offerId)
        {
            using var transaction = await _hoshiDbContext.Database.BeginTransactionAsync();
            try
            {
                var offer = await _hoshiDbContext.Offers.FindAsync(offerId);
                if (offer == null)
                {
                    return ResultDTO<string>.NotFound(new ErrorDTO
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
                        return ResultDTO<string>.NotFound(new ErrorDTO
                        {
                            ErrorAr = "محفظة العامل غير موجودة.",
                            ErrorEn = "Worker wallet not found."
                        });
                    }

                    if (workerWallet.Balance < workerCancellationFee)
                    {
                        return ResultDTO<string>.NotFound(new ErrorDTO
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
                    await notificationServiceHandler.sendMessagetoClient($"   قام العامل ب إلغاء الطلب والغرامه هي: {workerCancellationFee}",order.ClientId);

                }

                _hoshiDbContext.Offers.Update(offer);
                await transaction.CommitAsync();
                await _hoshiDbContext.SaveChangesAsync();


                return ResultDTO<string>.Success("Offer Cancelled");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return ResultDTO<string>.InternalServerError(new ErrorDTO
                {
                    ErrorAr = "حدث خطأ في الخادم.",
                    ErrorEn = $"Internal server error: {ex.Message}"
                });
            }
        }
        
    }
}
