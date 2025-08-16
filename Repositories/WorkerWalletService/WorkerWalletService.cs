using AutoMapper;
using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerWalletDTOs;
using Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerWalletHistoryDTOs;
using Hoshi.Models.UserModels.WorkerModels;
using Microsoft.EntityFrameworkCore;

namespace Hoshi.Repositories.WorkerWalletService
{
    public class WorkerWalletService : IWorkerWalletService
    {
        private readonly HoshiDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;

        public WorkerWalletService(HoshiDbContext context, IMapper mapper, IWebHostEnvironment environment)
        {
            _context = context;
            _mapper = mapper;
            _environment = environment;
        }

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
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "bills");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var fileName = $"{Guid.NewGuid()}_{request.BillImage.FileName}";
                var filePath = Path.Combine(uploadsFolder, fileName);
                var fileUrl = $"/uploads/bills/{fileName}";

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await request.BillImage.CopyToAsync(fileStream);
                }

                // Create payment history record
                var paymentHistory = new WorkerPaymentHistroy
                {
                    WorkerId = workerId,
                    BillImageURL = fileUrl,
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
