using AutoMapper;
using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.DashboardDTOs;
using Hoshi.DTOs.DashboardDTOs.ComplaintDTOs;
using Hoshi.DTOs.GlobalDTOs.ComplaintDTOs;
using Hoshi.DTOs.ServiceDTOs.JobDTOs;
using Hoshi.DTOs.ServiceDTOs.ServiceCategoryDTOs;
using Hoshi.DTOs.ServiceDTOs.ServiceDTOs;
using Hoshi.Models.ServiceModels;
using Hoshi.Repositories.FileServiceFold;
using Hoshi.DTOs.UserDTOs.ClientSpecificationDTOs;
using Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerHomeDTOs;
using Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerSpecificationDTOs;
using Hoshi.Enums;
using Microsoft.EntityFrameworkCore;
using Hoshi.Models.UserModels;
using Hoshi.Models.UserModels.WorkerModels;
using System.Transactions;

namespace Hoshi.Repositories.ServiceService
{
    /// <summary>
    /// Aggregates service-related operations including creation/update with image handling,
    /// service search, payments/complaints/statistics dashboards, and analytics projections.
    /// </summary>
    public class ServiceService : IServiceService
    {
        private readonly HoshiDbContext _context;
        private readonly IMapper _mapper;
        private readonly IFileService fileService;

        public ServiceService(
            HoshiDbContext context,
            IMapper mapper,
            IFileService fileService
        )
        {
            _context = context;
            _mapper = mapper;
            this.fileService = fileService;
        }

        /// <summary>
        /// Search services by exact name and group them under their categories with active services.
        /// </summary>
        public async Task<ResultDTO<List<ServiceCategoryGetDTO>>> searchServiceAsyn(string serviceName)
        {

            var selectedCategories = await _context.Services.Where(p => p.ServiveName == serviceName).Select(p => p.ServiceCategoryId).ToListAsync();
            var allCat = await _context.ServiceCategories.ToListAsync();
            var resultLST = new List<ServiceCategoryGetDTO>();
            foreach (var CatId in selectedCategories)
            {
                var targetCat = allCat.Where(p => p.Id == CatId).First();
                var targetMapper = _mapper.Map<ServiceCategoryGetDTO>(targetCat);
                var targetRelatedServiceActive = await _context.Services.Where(p => p.ServiceCategoryId == CatId && p.IsDeleted == false).ToListAsync();
                var targetServices = _mapper.Map<List<ServiceBasicDTO>>(targetRelatedServiceActive);
                targetMapper.Services = targetServices;
                resultLST.Add(targetMapper);

            }
            return ResultDTO<List<ServiceCategoryGetDTO>>.Success(resultLST);
        }

        /// <summary>
        /// Create a new service and persist its image via FileService.
        /// </summary>
        public async Task<ResultDTO<ServiceGetDTO>> AddService(ServicePostDTO postDTO)
        {
            try
            {
                Service service = _mapper.Map<Service>(postDTO);

                var imageResult = await fileService.SaveFileAsync(postDTO.Image, "images\\services");

                if (imageResult.Item1 is false)
                    return ResultDTO<ServiceGetDTO>.BadRequest(new ErrorDTO()
                    {
                        ErrorAr = "حدثت مشكلة في عملية الاضافة.",
                        ErrorEn = imageResult.Item2.ToString()
                    });

                service.ImageURL = imageResult.Item2;

                await _context.Set<Service>().AddAsync(service);

                await _context.SaveChangesAsync();

                Service finalResult = await _context.Set<Service>()
                    .Include(nameof(Service.ServiceCategory))
                    .FirstAsync(s => s.Id == service.Id);

                return ResultDTO<ServiceGetDTO>.Success(_mapper.Map<ServiceGetDTO>(finalResult));
            }
            catch (Exception ex)
            {
                return ResultDTO<ServiceGetDTO>.BadRequest(new ErrorDTO()
                {
                    ErrorAr = "حدثت مشكلة في عملية الاضافة.",
                    ErrorEn = ex.InnerException is null ? ex.InnerException!.Message : ex.Message
                });
            }
        }

        /// <summary>
        /// Update a service and optionally replace its image (old image is deleted first).
        /// </summary>
        public async Task<ResultDTO<ServiceGetDTO>> UpdateService(ServicePutDTO putDTO)
        {
            try
            {
                Service? service = await _context.Set<Service>()
                    .Include(nameof(Service.ServiceCategory))
                    .FirstAsync(s => s.Id == putDTO.Id);

                if (service == null)
                    return ResultDTO<ServiceGetDTO>.BadRequest(new ErrorDTO()
                    {
                        ErrorAr = "هذا المعرف غير موجود.",
                        ErrorEn = "This id not exist."
                    });

                if (putDTO.Image != null)
                {
                    fileService.DeleteFile(service.ImageURL);

                    var imageResult = await fileService.SaveFileAsync(putDTO.Image, "images\\services");

                    if (imageResult.Item1 is false)
                        return ResultDTO<ServiceGetDTO>.BadRequest(new ErrorDTO()
                        {
                            ErrorAr = "حدثت مشكلة في عملية الاضافة.",
                            ErrorEn = imageResult.Item2.ToString()
                        });

                    service.ImageURL = imageResult.Item2;
                }

                _mapper.Map(putDTO, service);

                await _context.SaveChangesAsync();

                return ResultDTO<ServiceGetDTO>.Success(_mapper.Map<ServiceGetDTO>(service));
            }
            catch (Exception ex)
            {
                return ResultDTO<ServiceGetDTO>.BadRequest(new ErrorDTO()
                {
                    ErrorAr = "حدثت مشكلة في عملية الاضافة.",
                    ErrorEn = ex.InnerException is null ? ex.InnerException!.Message : ex.Message
                });
            }
        }

        /// <summary>
        /// Build services analytics page: per-job, per-category, and per-service aggregates.
        /// </summary>
        public async Task<ResultDTO<object>> GetServicesPageAsync()
        {
            var jobData = await _context.JobServices
                .Include(js => js.Job)
                .Include(js => js.Service)
                .ThenInclude(s => s.ServiceCategory)
                .AsNoTracking()
                .ToListAsync();

            var allOrders = await _context.Orders.AsNoTracking().ToListAsync();

            var jobResult = jobData
                .GroupBy(js => js.Job)
                .Select(group =>
                {
                    var job = group.Key.JobTitle;
                    var serviceIds = group.Select(g => g.ServiceId).Distinct().ToList();
                    var services = group.Select(g => g.Service).Distinct().ToList();

                    var categoryCount = services.Select(s => s.ServiceCategoryId).Distinct().Count();

                    var relatedOrders = allOrders.Where(o => serviceIds.Contains(o.ServiceId));
                    var incomeAvg = relatedOrders.Any() ? (int)relatedOrders.Average(o => o.ProposalPrice) : 0;

                    var totalWorkers = _context.WorkerSpecifications.Count(w => w.JobId == group.Key.Id);

                    return new
                    {
                        JobTitle = job,
                        TotalRelatedCategories = categoryCount,
                        TotalRelatedWorkers = totalWorkers,
                        IncomeAvg = incomeAvg
                    };
                })
                .ToList();


            var categoryResult = jobData
                .Where(js => js.Service.ServiceCategory != null)
                .GroupBy(js => js.Service.ServiceCategory)
                .Select(group =>
                {
                    var category = group.Key;
                    var serviceIds = group.Select(g => g.ServiceId).Distinct().ToList();
                    var totalRelatedServices = serviceIds.Count;
                    // If you want workers for all jobs in this category:
                    var jobIds = group.Select(g => g.JobId).Distinct().ToList();
                    var totalRelatedWorkers = _context.WorkerSpecifications.Count(w => jobIds.Contains(w.JobId));
                    var relatedOrders = allOrders.Where(o => serviceIds.Contains(o.ServiceId));
                    var incomeAvg = relatedOrders.Any() ? (int)relatedOrders.Average(o => o.ProposalPrice) : 0;

                    return new
                    {
                        CategoryTitle = category.CategoryName,
                        TotalRelatedServices = totalRelatedServices,
                        TotalRelatedWorkers = totalRelatedWorkers,
                        IncomeAvg = incomeAvg
                    };
                })
                .ToList();

            var categoryServicesResult = jobData
                .Where(js => js.Service.ServiceCategory != null)
                .GroupBy(js => js.Service.ServiceCategory)
                .Select(categoryGroup =>
                {
                    var category = categoryGroup.Key;

                    var services = categoryGroup
                        .Select(g => g.Service)
                        .Distinct()
                        .Select(service =>
                        {
                            var totalRelatedOrders = allOrders.Count(o => o.ServiceId == service.Id);
                            var totalRelatedWorkers = _context.WorkerServices.Count(ws => ws.ServiceId == service.Id);
                            var incomeAvg = allOrders.Where(o => o.ServiceId == service.Id).Any()
                                ? (int)allOrders.Where(o => o.ServiceId == service.Id).Average(o => o.ProposalPrice)
                                : 0;

                            return new
                            {
                                ServiceTitle = service.ServiveName,
                                ImageUrl = service.ImageURL,
                                TotalRelatedOrders = totalRelatedOrders,
                                TotalRelatedWorkers = totalRelatedWorkers,
                                IncomeAvg = incomeAvg
                            };
                        }).ToList();

                    return new
                    {
                        CategoryTitle = category.CategoryName,
                        Services = services
                    };
                })
                .ToList();

            return ResultDTO<object>.Success(new
            {
                Jobs = jobResult,
                Categories = categoryResult,
                CategoryServices = categoryServicesResult
            });
        }


        /// <summary>
        /// Aggregate payments page data: invoice sums, payment requests, and uncollected fees.
        /// </summary>
        public async Task<ResultDTO<object>> GetPaymentsPageAsync()
        {
            // Aggregate invoice sums
            var invoiceSums = await _context.Invoices
                .AsNoTracking()
                .GroupBy(i => 1)
                .Select(g => new
                {
                    TotalOrdersIncome = g.Sum(i => i.WorkerTotalPrice),
                    TotalOrdersPrices = g.Sum(i => i.OrderPrice),
                    TotalOrdersFees = g.Sum(i => i.CommissionFee + i.VisitingFee + i.CancellationFee),
                    TotalUncollectedFees = g.Where(i => i.Order != null && i.Order.OrderStatus != OrderStatus.Completed.ToString())
                        .Sum(i => i.CommissionFee + i.VisitingFee + i.CancellationFee)
                })
                .FirstOrDefaultAsync();

            // Preload worker balances
            var workerBalances = await _context.WorkerWallets
                .AsNoTracking()
                .Select(w => new { w.WorkerId, w.Balance })
                .ToListAsync();
            var workerBalanceDict = workerBalances.ToDictionary(w => w.WorkerId, w => w.Balance);

            // Preload notification user IDs
            var notifiedUserIds = await _context.UserNotifications
                .AsNoTracking()
                .Select(n => n.UserId)
                .Distinct()
                .ToListAsync();
            var notifiedUserSet = new HashSet<int>(notifiedUserIds);

            // Workers payment requests
            var workerPaymentRequests = await _context.WorkerPaymentHistroys
                .AsNoTracking()
                .Include(u => u.Worker)
                .Where(p => p.IsApproved == false)
                .Select(r => new
                {
                    WorkerData = _context.WorkerSpecifications
                        .AsNoTracking()
                        .Include(ws => ws.Job)
                        .Include(ws => ws.LivingCity)
                        .Where(p => p.UserId == r.Worker!.Id)
                        .Select(ws => new
                        {
                            WorkerId = ws.UserId,
                            Name = r.Worker!.FullName,
                            Email = ws.User!.Email,
                            Image = ws.User!.ImageURL,
                            Job = ws.Job!.JobTitle,
                            City = ws.LivingCity!.CityName,
                            Balance = workerBalanceDict.ContainsKey(ws.UserId) ? workerBalanceDict[ws.UserId] : 0.0,
                        }).FirstOrDefault(),
                    RequestData = new
                    {
                        RequestId = r.Id,
                        CreatedAt = r.CreatedAt,
                    }
                })
                .ToListAsync();


            // Worker uncollected fees (optimized)
            var workersUncollectedFees = await _context.WorkerWallets
                .AsNoTracking()
                .Include(ws => ws.Worker)
                .Where(ww => ww.Balance < 0)
                .Select(r => new
                {
                    WorkerData = _context.WorkerSpecifications
                        .AsNoTracking()
                        .Include(ws => ws.Job)
                        .Include(ws => ws.LivingCity)
                        .Where(p => p.UserId == r.Worker!.Id)
                        .Select(ws => new
                        {
                            WorkerId = ws.UserId,
                            Name = r.Worker!.FullName,
                            Email = ws.User!.Email,
                            Image = ws.User!.ImageURL,
                            Job = ws.Job!.JobTitle,
                            City = ws.LivingCity!.CityName,
                        }).FirstOrDefault(),

                    Balance = r.Balance,
                })
                .ToListAsync();

            // Clients uncollected fees (optimized)
            var clientsUncollectedFees = await _context.ClientSpecifications
                .AsNoTracking()
                .Include(cs => cs.User)
                .Where(cs => cs.Indebtedness > 0)
                .Select(cs => new
                {
                    Client = _mapper.Map<ClientSpecificationGetDTO>(cs),
                    RateRatio = cs.RateRito,
                    TotalCompletedOrders = cs.CompletedOrders,
                    TotalCancelledOrders = _context.Orders.Count(o => o.ClientId == cs.UserId && o.OrderStatus == OrderStatus.Cancelled.ToString()),
                    Indebtedness = cs.Indebtedness,
                    HasCollectionAlert = notifiedUserSet.Contains(cs.UserId)
                })
                .ToListAsync();

            return ResultDTO<object>.Success(new
            {
                TotalOrdersIncome = invoiceSums?.TotalOrdersIncome ?? 0.0,
                TotalOrdersPrices = invoiceSums?.TotalOrdersPrices ?? 0.0,
                TotalOrdersFees = invoiceSums?.TotalOrdersFees ?? 0.0,
                TotalUncollectedFees = invoiceSums?.TotalUncollectedFees ?? 0.0,
                WorkerPaymentRequests = workerPaymentRequests,
                WorkersUncollectedFees = workersUncollectedFees,
                ClientsUncollectedFees = clientsUncollectedFees
            });
        }


        /// <summary>
        /// Retrieve payment details and worker snapshot for a given payment id.
        /// </summary>
        public async Task<ResultDTO<PaymentDetailsResponseDTO>> GetPaymentDetailsAsync(int paymentId)
        {
            var payment = await _context.WorkerPaymentHistroys
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == paymentId);
            if (payment == null)
                return ResultDTO<PaymentDetailsResponseDTO>.NotFound(new ErrorDTO
                {
                    ErrorAr = "الدفع غير موجود.",
                    ErrorEn = "payement not found."
                });

            var workerSpec = await _context.WorkerSpecifications
                .AsNoTracking()
                .Include(ws => ws.User)
                .Include(ws => ws.Job)
                .FirstOrDefaultAsync(ws => ws.UserId == payment.WorkerId);
            if (workerSpec == null)
                return ResultDTO<PaymentDetailsResponseDTO>.NotFound(new ErrorDTO
                {
                    ErrorAr = "العامل غير موجود.",
                    ErrorEn = "worker not found."
                });

            var wallet = await _context.WorkerWallets
                .AsNoTracking()
                .FirstOrDefaultAsync(w => w.WorkerId == workerSpec.UserId);

            var cancelledOffers = await _context.Offers
                .AsNoTracking()
                .CountAsync(o => o.WorkerId == workerSpec.UserId && o.OfferStatus == OfferStatus.Cancelled.ToString());

            var WorkerDataDTO = new WorkerDataDTO
            {
                WorkerId = workerSpec.UserId,
                ImageUrl = workerSpec.IdentityImageURL,
                FullName = workerSpec.User.FullName,
                Email = workerSpec.User.Email,
                Job = _mapper.Map<JobGetDTO>(workerSpec.Job),
                IsCompany = workerSpec.IsCompany,
                RateRatio = workerSpec.RateRito,
                CompletedOrders = workerSpec.CompletedOrders,
                CancelledOffers = cancelledOffers,
                Balance = wallet?.Balance ?? 0.0
            };

            return ResultDTO<PaymentDetailsResponseDTO>.Success(new PaymentDetailsResponseDTO
            {
                BillImageUrl = payment.BillImageURL,
                Worker = WorkerDataDTO
            });
        }

        /// <summary>
        /// Add worker payment by top-up value, append wallet history, and clear HitLimit.
        /// </summary>
        public async Task<ResultDTO<string>> AddWorkerPayment(int workerId, int requestId, double paymentValue)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1- Check if worker exist
                User? worker = await _context.Users.FindAsync(workerId);
                if (worker is null || worker.UserType != UserType.Worker.ToString())
                    return ResultDTO<string>.BadRequest(new ErrorDTO
                    {
                        ErrorAr = "معرف العامل خاطئ.",
                        ErrorEn = "Worker id is not correct."
                    });

                // 2- Get user wallet
                WorkerWallet? workerWallet = await _context.WorkerWallets.FirstOrDefaultAsync(ww => ww.WorkerId == workerId);

                // if there is no wallet for this worker will create a new one
                if (workerWallet is null)
                {
                    var result = await _context.WorkerWallets.AddAsync(new WorkerWallet()
                    {
                        WorkerId = workerId,
                        CreatedAt = DateTime.UtcNow,
                    });

                    // save changes to get new wallet id
                    await _context.SaveChangesAsync();

                    workerWallet = result.Entity;
                }

                // 3- Update user balance and hitlimit
                if (paymentValue <= 0)
                    return ResultDTO<string>.BadRequest(new ErrorDTO
                    {
                        ErrorAr = "لا يمكن اضافة قيمة اقل من او يساوي 0.",
                        ErrorEn = "Can not add value less than or equal 0."
                    });

                workerWallet.Balance = paymentValue;
                workerWallet.HitLimit = false;

                // 4- Close Payment Request
                WorkerPaymentHistroy? paymentRequest = await _context.WorkerPaymentHistroys.FindAsync(requestId);
                if (paymentRequest is null)
                    return ResultDTO<string>.BadRequest(new ErrorDTO
                    {
                        ErrorAr = "لا توجد طلب بهذا المعرف.",
                        ErrorEn = "There is no payment request with this Id."
                    });

                paymentRequest.IsApproved = true;
                paymentRequest.ModifiedAt = DateTime.UtcNow;

                // 4- Add it to wallet history
                await _context.WorkerWalletHistories.AddAsync(new WorkerWalletHistory
                {
                    IsIncome = true,
                    CreatedAt = DateTime.UtcNow,
                    WorkerWalletId = workerWallet.Id,
                    Value = paymentValue,
                    Title = "اضافة رصيد جديد على المحفظة"
                });

                // 5- Save changes
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return ResultDTO<string>.Success(new MessageDTO
                {
                    MessageAr = $"تم اضافة {paymentValue} دينار الى العامل {worker.FullName}.",
                    MessageEn = $"Successfully add {paymentValue} D.L to worker {worker.FullName}."
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                return ResultDTO<string>.InternalServerError(new ErrorDTO
                {
                    ErrorAr = "يوجد مشكلة في عملية اضافة الدفع للخطأ التالي.",
                    ErrorEn = "There is a problem in Payment adding process."
                },
                    innerError: ex.InnerException is null ? ex.Message : ex.InnerException.Message
                );
            }
        }

        /// <summary>
        /// Build complaints page summary and list with essential fields.
        /// </summary>
        public async Task<ResultDTO<ComplaintPageResponseDTO>> GetComplaintsPageAsync()
        {
            var query = _context.Complaints
                .AsNoTracking()
                .Include(c => c.User)
                .Include(c => c.ComplaintType);

            var total = await query.CountAsync();
            var opened = await query.CountAsync(c => c.ComplaintStatus != ComplaintStatus.Solved.ToString());
            var closed = await query.CountAsync(c => c.ComplaintStatus == ComplaintStatus.Solved.ToString());

            var complaints = await query
                .OrderByDescending(c => c.CreatedAt)
                .Select(c => new ComplaintItemDTO
                {
                    ComplaintId = c.Id,
                    FullName = c.User.FullName,
                    ComplaintType = c.ComplaintType.Type,
                    CreatedAt = c.CreatedAt
                })
                .ToListAsync();

            var result = new ComplaintPageResponseDTO
            {
                TotalComplaints = total,
                TotalComplaintsOpened = opened,
                TotalComplaintsClosed = closed,
                Complaints = complaints
            };

            return ResultDTO<ComplaintPageResponseDTO>.Success(result);
        }

        /// <summary>
        /// Get full complaint details including order images and status history.
        /// </summary>
        public async Task<ResultDTO<ComplaintGetDTO>> GetComplaintDetailsAsync(int complaintId)
        {
            var complaint = await _context.Complaints
                .AsNoTracking()
                .Include(c => c.User)
                .Include(c => c.ComplaintType)
                .Include(c => c.Order)
                    .ThenInclude(o => o.OrderImages)
                .Include(c => c.Order)
                    .ThenInclude(o => o.OrderStatus)
                .FirstOrDefaultAsync(c => c.Id == complaintId);

            if (complaint == null)
                return ResultDTO<ComplaintGetDTO>.NotFound(new ErrorDTO { ErrorAr = "الشكوى غير موجودة.", ErrorEn = "Complaint not found." });

            var response = _mapper.Map<ComplaintGetDTO>(complaint);

            return ResultDTO<ComplaintGetDTO>.Success(response);
        }

        /// <summary>
        /// Update complaint response text.
        /// </summary>
        public async Task<ResultDTO<MessageDTO>> ComplaintResponse(ComplaintResponseDTO complaintCreateDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Basic validation
                if (string.IsNullOrWhiteSpace(complaintCreateDto.Response))
                    return ResultDTO<MessageDTO>.BadRequest(new ErrorDTO
                    {
                        ErrorAr = "نص الرد مطلوب.",
                        ErrorEn = "Response text is required."
                    });

                // Use async for better performance
                var getComplaint = await _context.Complaints.FirstOrDefaultAsync(c => c.Id == complaintCreateDto.ComplaintId);
                if (getComplaint == null)
                    return ResultDTO<MessageDTO>.NotFound(new ErrorDTO
                    {
                        ErrorAr = "الشكوى غير موجودة.",
                        ErrorEn = "Complaint not found."
                    });

                // Update complaint
                getComplaint.Response = complaintCreateDto.Response.Trim();
                getComplaint.ModifiedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return ResultDTO<MessageDTO>.Success(new MessageDTO
                {
                    MessageAr = "تم الرد على الشكوى بنجاح.",
                    MessageEn = "Complaint response sent successfully."
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                return ResultDTO<MessageDTO>.InternalServerError(new ErrorDTO
                {
                    ErrorAr = "يوجد مشكلة داخلية في النظام.",
                    ErrorEn = "There is an internal server error."
                },
                    innerError: ex.InnerException is null ? ex.Message : ex.InnerException.Message
                );
            }
        }

        /// <summary>
        /// Mark a complaint as solved and set modification timestamp.
        /// </summary>
        public async Task<ResultDTO<bool>> CloseComplaintAsync(int complaintId)
        {
            var complaint = await _context.Complaints.FirstOrDefaultAsync(c => c.Id == complaintId);
            if (complaint == null)
                return ResultDTO<bool>.NotFound(new ErrorDTO { ErrorAr = "الشكوى غير موجودة.", ErrorEn = "Complaint not found." });

            complaint.ComplaintStatus = ComplaintStatus.Solved.ToString();
            complaint.ModifiedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return ResultDTO<bool>.Success();
        }

        /// <summary>
        /// Compute basic system statistics and averages for dashboard.
        /// </summary>
        public async Task<ResultDTO<StatisticPageResponseDTO>> GetStatisticPageAsync()
        {
            var totalOrders = await _context.Orders.CountAsync();
            var totalOrdersIncome = await _context.Invoices.SumAsync(i => i.WorkerTotalPrice);
            var totalOrdersPrices = await _context.Invoices.SumAsync(i => i.OrderPrice);
            var totalClients = await _context.ClientSpecifications.CountAsync();

            double averageOrderingPerUser = 0;
            if (totalClients > 0)
            {
                var totalUserOrders = await _context.Orders.CountAsync();
                averageOrderingPerUser = (double)totalUserOrders / totalClients;
            }

            var result = new StatisticPageResponseDTO
            {
                TotalOrders = totalOrders,
                TotalOrdersIncome = totalOrdersIncome,
                TotalOrdersPrices = totalOrdersPrices,
                TotalClients = totalClients,
                AverageOrderingPerUser = Math.Round(averageOrderingPerUser, 2)
            };

            return ResultDTO<StatisticPageResponseDTO>.Success(result);
        }
    }
}
