using AutoMapper;
using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerWalletDTOs;
using Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerWalletHistoryDTOs;
using Hoshi.Models.UserModels.WorkerModels;
using Hoshi.Repositories.FileServiceFold;
using Microsoft.EntityFrameworkCore;

namespace Hoshi.Repositories.WorkerWalletService
{
    /// <summary>
    /// Provides worker wallet read and write operations, including payment submissions and balance updates.
    /// </summary>
    public class WorkerWalletService : IWorkerWalletService
    {
        private readonly HoshiDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;
        private readonly IFileService _fileService;

        public WorkerWalletService(
            HoshiDbContext context,
            IMapper mapper,
            IWebHostEnvironment environment,
            IFileService fileService
        )
        {
            _context = context;
            _mapper = mapper;
            _environment = environment;
            this._fileService = fileService;
        }

        /// <summary>
        /// Get worker wallet and recent history; returns NotFound if wallet missing.
        /// </summary>
        public async Task<ResultDTO<WorkerWalletResponseDTO>> GetWorkerWalletAsync(int workerId)
        {
            // Check if worker exists
            var workerExists = await _context.Users.AnyAsync(u => u.Id == workerId);
            if (!workerExists)
            {
                return ResultDTO<WorkerWalletResponseDTO>.NotFound(new ErrorDTO
                {
                    ErrorAr = "العامل غير موجود.",
                    ErrorEn = "Worker not found."
                });
            }

            // Get or create worker wallet
            var workerWallet = await _context.WorkerWallets
                .FirstOrDefaultAsync(w => w.WorkerId == workerId);

            if (workerWallet == null)
            {
                return ResultDTO<WorkerWalletResponseDTO>.NotFound(new ErrorDTO
                {
                    ErrorAr = "المحفظة غير موجودة.",
                    ErrorEn = "wallet not found."
                });
            }

            // Get wallet history
            var walletHistory = await _context.WorkerWalletHistories
                .Where(h => h.WorkerWalletId == workerWallet.Id)
                .OrderByDescending(h => h.CreatedAt)
                .ToListAsync();

            var historyDTOs = _mapper.Map<List<WorkerWalletHistoryGetDTO>>(walletHistory);

            var response = new WorkerWalletResponseDTO
            {
                Balance = workerWallet.Balance,
                WalletHistory = historyDTOs
            };

            return ResultDTO<WorkerWalletResponseDTO>.Success(response);
        }

        /// <summary>
        /// Submit a payment with an uploaded bill image; image is saved under /uploads/bills.
        /// </summary>
        public async Task<ResultDTO<string>> AddPaymentAsync(int workerId, AddPaymentRequestDTO request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Check if worker exists
                var workerExists = await _context.Users.AnyAsync(u => u.Id == workerId);
                if (!workerExists)
                {
                    return ResultDTO<string>.NotFound(new ErrorDTO
                    {
                        ErrorAr = "العامل غير موجود.",
                        ErrorEn = "Worker not found."
                    });
                }

                // Validate file
                if (request.BillImage == null || request.BillImage.Length == 0)
                {
                    return ResultDTO<string>.BadRequest(new ErrorDTO
                    {
                        ErrorAr = "يجب إرفاق صورة الفاتورة.",
                        ErrorEn = "Bill image is required."
                    });
                }

                // Save the image file
                var saveResult = await _fileService.SaveFileAsync(request.BillImage, Path.Combine("uploads","bills"));
                if (!saveResult.Item1)
                    return ResultDTO<string>.BadRequest(new ErrorDTO
                    {
                        ErrorAr = "يوجد مشكلة في اضافة الصورة.",
                        ErrorEn = saveResult.Item2
                    });

                // Create payment history record
                var paymentHistory = new WorkerPaymentHistroy
                {
                    WorkerId = workerId,
                    BillImageURL = saveResult.Item2,
                    IsApproved = false
                };

                _context.WorkerPaymentHistroys.Add(paymentHistory);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return ResultDTO<string>.Success("Payment request submitted successfully.");
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

        /// <summary>
        /// Add funds to wallet and append an income history record.
        /// </summary>
        public async Task AddToWalletAsync(int workerId, double amount, string title)
        {
            var wallet = await _context.WorkerWallets.FirstOrDefaultAsync(w => w.WorkerId == workerId);
            if (wallet == null)
            {
                wallet = new WorkerWallet
                {
                    WorkerId = workerId,
                    Balance = 0
                };
                _context.WorkerWallets.Add(wallet);
                await _context.SaveChangesAsync();
            }

            wallet.Balance += amount;

            _context.WorkerWalletHistories.Add(new WorkerWalletHistory
            {
                WorkerWalletId = wallet.Id,
                Title = title,
                Value = amount,
                IsIncome = true
            });

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Deduct funds from wallet and append an expense history record.
        /// </summary>
        public async Task DeductFromWalletAsync(int workerId, double amount, string title)
        {
            var wallet = await _context.WorkerWallets.FirstOrDefaultAsync(w => w.WorkerId == workerId);
            if (wallet == null)
                throw new Exception("Worker wallet not found.");

            wallet.Balance -= amount;

            _context.WorkerWalletHistories.Add(new WorkerWalletHistory
            {
                WorkerWalletId = wallet.Id,
                Title = title,
                Value = amount,
                IsIncome = false
            });

            await _context.SaveChangesAsync();
        }
    }
}
