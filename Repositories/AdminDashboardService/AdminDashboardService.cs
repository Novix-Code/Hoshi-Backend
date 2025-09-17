using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.DTOs.DashboardDTOs.ComplaintDTOs;
using Hoshi.DTOs.DashboardDTOs;
using Hoshi.DTOs.GlobalDTOs.ComplaintDTOs;
using Hoshi.DTOs.ServiceDTOs.JobDTOs;
using Hoshi.DTOs.UserDTOs.ClientSpecificationDTOs;
using Hoshi.Enums;
using Hoshi.Models.UserModels.WorkerModels;
using Hoshi.Models.UserModels;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Hoshi.Data;
using Hoshi.Repositories.FileServiceFold;
using Hoshi.DTOs.UserDTOs.AdminDTOs.PermissionDTOs;
using Hoshi.DTOs.UserDTOs.AdminDTOs.UserPermissionDTOs;
using Hoshi.DTOs.UserDTOs.UserDTOs;
using Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerPortfolioDTOs;
using Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerSpecificationDTOs;
using Hoshi.Models.GlobalModels;
using Hoshi.Models.OrderModels;
using Microsoft.AspNetCore.Identity;

namespace Hoshi.Repositories.AdminDashboardService
{
    public class AdminDashboardService : IAdminDashboardService
    {
        private readonly HoshiDbContext context;
        private readonly IMapper mapper;
        private readonly IFileService fileService;
        private readonly UserManager<User> userManager;

        public AdminDashboardService(
            HoshiDbContext context,
            IMapper mapper,
            IFileService fileService,
            UserManager<User> userManager
        )
        {
            this.context = context;
            this.mapper = mapper;
            this.fileService = fileService;
            this.userManager = userManager;
        }

        public async Task<ResultDTO<object>> OverViewPage()
        {
            // Get page statistics
            var overResult = await context.OverviewView.FirstOrDefaultAsync();

            // Get top 10 new orders
            var newOrders = context.Orders
                .Include(i => i.Client)
                .Where(p => 
                    p.OrderStatus == Enums.OrderStatus.Published.ToString() 
                    || p.OrderStatus == Enums.OrderStatus.Assigned.ToString()
                )
                .Select(o => new { o.Id, o.Description, o.ServicingDateTime, o.Client!.FullName, o.Client!.ImageURL})
                .Take(10);
            
            // Get top 10 completed orders
            var completedOrders = context.Orders
                .Where(p => p.OrderStatus == Enums.OrderStatus.Completed.ToString()).Take(10);

            // Get top 10 new complaints
            var newComplaints = context.Complaints.Where(p => p.ComplaintStatus == Enums.ComplaintStatus.Waitting.ToString()).Take(10);

            // Get top 10 new payment requests
            var newPaymentRequests = context.WorkerPaymentHistroys.Where(p => !p.IsApproved).Take(10);

            if (overResult == null)
            {
                return ResultDTO<object>.Failure(new ErrorDTO(), ResponseStatusCodes.NotFound);
            }
            var result = new
            {
                TotalUsers = overResult.TotalUsers,
                TotalClients = overResult.TotalClients,
                TotalWorkers = overResult.TotalWorkers,
                TotalOrders = overResult.TotalOrders,
                TotalCompletedOrders = overResult.TotalCompletedOrders,
                TotalOrderIncome = overResult.TotalOrderIncome,
                TotalOrderPrice = overResult.TotalOrderPrice,
                NewOrders = newOrders,
                CompletedOrders = completedOrders,
                NewComplients = newComplaints,

            };
            return ResultDTO<object>.Success(result);

        }

        /// <summary>
        /// Build services analytics page: per-job, per-category, and per-service aggregates.
        /// </summary>
        public async Task<ResultDTO<object>> GetServicesPageAsync()
        {
            var jobData = await context.JobServices
                .Include(js => js.Job)
                .Include(js => js.Service)
                .ThenInclude(s => s.ServiceCategory)
                .AsNoTracking()
                .ToListAsync();

            var allOrders = await context.Orders.AsNoTracking().ToListAsync();

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

                    var totalWorkers = context.WorkerSpecifications.Count(w => w.JobId == group.Key.Id);

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
                var totalRelatedWorkers = context.WorkerSpecifications.Count(w => jobIds.Contains(w.JobId));
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
                            var totalRelatedWorkers = context.WorkerServices.Count(ws => ws.ServiceId == service.Id);
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
            var invoiceSums = await context.Invoices
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
            var workerBalances = await context.WorkerWallets
                .AsNoTracking()
                .Select(w => new { w.WorkerId, w.Balance })
                .ToListAsync();
            var workerBalanceDict = workerBalances.ToDictionary(w => w.WorkerId, w => w.Balance);

            // Preload notification user IDs
            var notifiedUserIds = await context.UserNotifications
                .AsNoTracking()
                .Select(n => n.UserId)
                .Distinct()
                .ToListAsync();
            var notifiedUserSet = new HashSet<int>(notifiedUserIds);

            // Workers payment requests
            var workerPaymentRequests = await context.WorkerPaymentHistroys
                .AsNoTracking()
                .Include(u => u.Worker)
                .Where(p => p.IsApproved == false)
                .Select(r => new
                {
                    WorkerData = context.WorkerSpecifications
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
            var workersUncollectedFees = await context.WorkerWallets
                .AsNoTracking()
                .Include(ws => ws.Worker)
                .Where(ww => ww.Balance < 0)
                .Select(r => new
                {
                    WorkerData = context.WorkerSpecifications
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
            var clientsUncollectedFees = await context.ClientSpecifications
            .AsNoTracking()
            .Include(cs => cs.User)
                .Where(cs => cs.Indebtedness > 0)
                .Select(cs => new
                {
                    Client = mapper.Map<ClientSpecificationGetDTO>(cs),
                    RateRatio = cs.RateRito,
                    TotalCompletedOrders = cs.CompletedOrders,
                    TotalCancelledOrders = context.Orders.Count(o => o.ClientId == cs.UserId && o.OrderStatus == OrderStatus.Cancelled.ToString()),
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
            var payment = await context.WorkerPaymentHistroys
            .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == paymentId);
            if (payment == null)
                return ResultDTO<PaymentDetailsResponseDTO>.NotFound(new ErrorDTO
                {
                    ErrorAr = "الدفع غير موجود.",
                    ErrorEn = "payement not found."
                });

            var workerSpec = await context.WorkerSpecifications
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

            var wallet = await context.WorkerWallets
                .AsNoTracking()
                .FirstOrDefaultAsync(w => w.WorkerId == workerSpec.UserId);

            var cancelledOffers = await context.Offers
                .AsNoTracking()
                .CountAsync(o => o.WorkerId == workerSpec.UserId && o.OfferStatus == OfferStatus.Cancelled.ToString());

            var WorkerDataDTO = new WorkerDataDTO
            {
                WorkerId = workerSpec.UserId,
                ImageUrl = workerSpec.IdentityImageURL,
                FullName = workerSpec.User.FullName,
                Email = workerSpec.User.Email,
                Job = mapper.Map<JobGetDTO>(workerSpec.Job),
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
            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                // 1- Check if worker exist
                User? worker = await context.Users.FindAsync(workerId);
                if (worker is null || worker.UserType != UserType.Worker.ToString())
                    return ResultDTO<string>.BadRequest(new ErrorDTO
                    {
                        ErrorAr = "معرف العامل خاطئ.",
                        ErrorEn = "Worker id is not correct."
                    });

                // 2- Get user wallet
                WorkerWallet? workerWallet = await context.WorkerWallets.FirstOrDefaultAsync(ww => ww.WorkerId == workerId);

                // if there is no wallet for this worker will create a new one
                if (workerWallet is null)
                {
                    var result = await context.WorkerWallets.AddAsync(new WorkerWallet()
                    {
                        WorkerId = workerId,
                        CreatedAt = DateTime.UtcNow,
                    });

                    // save changes to get new wallet id
                    await context.SaveChangesAsync();

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
                WorkerPaymentHistroy? paymentRequest = await context.WorkerPaymentHistroys.FindAsync(requestId);
                if (paymentRequest is null)
                    return ResultDTO<string>.BadRequest(new ErrorDTO
                    {
                        ErrorAr = "لا توجد طلب بهذا المعرف.",
                        ErrorEn = "There is no payment request with this Id."
                    });

                paymentRequest.IsApproved = true;
                paymentRequest.ModifiedAt = DateTime.UtcNow;

                // 4- Add it to wallet history
                await context.WorkerWalletHistories.AddAsync(new WorkerWalletHistory
                {
                    IsIncome = true,
                    CreatedAt = DateTime.UtcNow,
                    WorkerWalletId = workerWallet.Id,
                    Value = paymentValue,
                    Title = "اضافة رصيد جديد على المحفظة"
                });

                // 5- Save changes
                await context.SaveChangesAsync();
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
            var query = context.Complaints
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
            var complaint = await context.Complaints
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
            var response = mapper.Map<ComplaintGetDTO>(complaint);

            return ResultDTO<ComplaintGetDTO>.Success(response);
        }

        /// <summary>
        /// Update complaint response text.
        /// </summary>
        public async Task<ResultDTO<MessageDTO>> ComplaintResponse(ComplaintResponseDTO complaintCreateDto)
        {
            using var transaction = await context.Database.BeginTransactionAsync();
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
                var getComplaint = await context.Complaints.FirstOrDefaultAsync(c => c.Id == complaintCreateDto.ComplaintId);
                if (getComplaint == null)
                    return ResultDTO<MessageDTO>.NotFound(new ErrorDTO
                    {
                        ErrorAr = "الشكوى غير موجودة.",
                        ErrorEn = "Complaint not found."
                    });

                // Update complaint
                getComplaint.Response = complaintCreateDto.Response.Trim();
                getComplaint.ModifiedAt = DateTime.UtcNow;

                await context.SaveChangesAsync();
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
            var complaint = await context.Complaints.FirstOrDefaultAsync(c => c.Id == complaintId);
            if (complaint == null)
                return ResultDTO<bool>.NotFound(new ErrorDTO { ErrorAr = "الشكوى غير موجودة.", ErrorEn = "Complaint not found." });

            complaint.ComplaintStatus = ComplaintStatus.Solved.ToString();
            complaint.ModifiedAt = DateTime.UtcNow;

            await context.SaveChangesAsync();
            return ResultDTO<bool>.Success();
        }

        /// <summary>
        /// Compute basic system statistics and averages for dashboard.
        /// </summary>
        public async Task<ResultDTO<StatisticPageResponseDTO>> GetStatisticPageAsync()
        {
            var totalOrders = await context.Orders.CountAsync();
            var totalOrdersIncome = await context.Invoices.SumAsync(i => i.WorkerTotalPrice);
            var totalOrdersPrices = await context.Invoices.SumAsync(i => i.OrderPrice);
            var totalClients = await context.ClientSpecifications.CountAsync();

            double averageOrderingPerUser = 0;
            if (totalClients > 0)
            {
                var totalUserOrders = await context.Orders.CountAsync();
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

        public async Task<ResultDTO<object>> BeWorkerApproved(int Id)
        {
            var tergetWorkerSpecif = await context.WorkerSpecifications.Where(p => p.UserId == Id).FirstOrDefaultAsync();
            if (tergetWorkerSpecif == null)
            {
                return ResultDTO<object>.NotFound(new ErrorDTO
                {
                    ErrorEn = "worker Specification not found"
                    ,
                    ErrorAr = "لم يتم اضافة بيانات للعامل بعد"
                });
            }

            // Check if the worker is approved once before so cannot be approved again
            if (tergetWorkerSpecif.IsApproved is true)
            {
                return ResultDTO<object>.BadRequest
                (
                    new ErrorDTO
                    {
                        ErrorAr = "تم قبول العامل بالفعل.",
                        ErrorEn = "Worker already be approved."
                    }
                );
            }

            tergetWorkerSpecif.IsApproved = true;

            // handle add notifications 
            var checkexcist = await context.NotificationTypes.Where(p => p.Type == "Success Message").Select(p => p.Id).FirstOrDefaultAsync();
            if (checkexcist == 0)
            {
                var notiType = new NotificationType
                {
                    Title = "Successfully Approved",
                    ForClient = false,
                    Type = "Success Message"
                };
                await context.NotificationTypes.AddAsync(notiType);
                await context.SaveChangesAsync();
                context.UserNotifications.Add(new UserNotification
                {
                    NotificationTypeId = notiType.Id,
                    Description = "success Message",
                    UserId = Id
                });
                context.WorkerSpecifications.Update(tergetWorkerSpecif);
                await context.SaveChangesAsync();
                return ResultDTO<object>.Success("Worker is now approved");

            }
            else
            {
                context.UserNotifications.Add(new UserNotification
                {
                    NotificationTypeId = checkexcist,
                    Description = "success Message",
                    UserId = Id
                });
                context.WorkerSpecifications.Update(tergetWorkerSpecif);
                await context.SaveChangesAsync();
                return ResultDTO<object>.Success("Worker is now approved");
            }

        }

        public async Task<ResultDTO<object>> BeWorkerReject(int Id, string rejectResoun)
        {
            var tergetWorkerSpecif = await context.WorkerSpecifications.Where(p => p.UserId == Id).FirstOrDefaultAsync();
            if (tergetWorkerSpecif == null)
            {
                return ResultDTO<object>.NotFound(new ErrorDTO
                {
                    ErrorEn = "worker Specification not found"
                    ,
                    ErrorAr = "لم يتم اضافة بيانات للعامل بعد"
                });
            }

            // Check if the user is already approved; in this case cannot be rejected
            if (tergetWorkerSpecif.IsApproved is true)
            {
                return ResultDTO<object>.BadRequest
                (
                    new ErrorDTO
                    {
                        ErrorAr = "تم قبول العامل بالفعل، ولا يمكن رفضه.",
                        ErrorEn = "Worker already be approved and can not be rejected."
                    }
                );
            }

            tergetWorkerSpecif.IsApproved = false;
            await context.SaveChangesAsync();
            var checkexcist = await context.NotificationTypes.Where(p => p.Type == "Reject Message").Select(p => p.Id).FirstOrDefaultAsync();
            if (checkexcist == 0)
            {
                var notiType = new NotificationType
                {
                    Title = "Successfully Reject",
                    ForClient = false,
                    Type = "Reject Message"
                };
                await context.NotificationTypes.AddAsync(notiType);
                await context.SaveChangesAsync();
                context.UserNotifications.Add(new UserNotification
                {
                    NotificationTypeId = notiType.Id,
                    Description = rejectResoun,
                    UserId = Id
                });
                context.WorkerSpecifications.Update(tergetWorkerSpecif);
                await context.SaveChangesAsync();
                return ResultDTO<object>.Success("Worker is now Rejected");

            }
            else
            {
                context.UserNotifications.Add(new UserNotification
                {
                    NotificationTypeId = checkexcist,
                    Description = rejectResoun,
                    UserId = Id
                });
                context.WorkerSpecifications.Update(tergetWorkerSpecif);
                await context.SaveChangesAsync();
                return ResultDTO<object>.Success("Worker is now Rejected");
            }
        }

        public async Task<ResultDTO<object>> ClientDetails(int Id)
        {
            var targetClient = await context.ClientDetailsView.FirstOrDefaultAsync(p => p.UserId == Id);
            if (targetClient == null)
                return ResultDTO<object>.Failure(new ErrorDTO { ErrorEn = "client not found" }, ResponseStatusCodes.NotFound);


            var targetOrders = await context.OrderDetailsView
                .Where(p => p.ClientId == Id)
                .ToListAsync();

            var result = new
            {
                ImageURL = targetClient.ImageURL,
                Email = targetClient.Email,
                Phone = targetClient.PhoneNumber,
                Location = targetClient.Address,
                Orders = targetOrders,
            };
            return ResultDTO<object>.Success(result);
        }

        public async Task<ResultDTO<object>> Clientpage()
        {
            var clientPage = await context.ClientPageView.FirstOrDefaultAsync();
            var newClient = await context.NewClientView.ToListAsync();
            var allClient = await context.AllClientView.ToListAsync();
            var susClient = await context.SuspendedUserView.ToListAsync();
            var result = new
            {
                TotalClients = clientPage.TotalClients,
                TotalNewClients = clientPage.TotalNewClientsThisMonth,
                TotalActiveClients = clientPage.TotalActiveClients,
                averageOrder = clientPage.AverageOrdering,
                NewClients = newClient,
                AllClient = allClient,
                SuspendedClients = susClient,

            };
            return ResultDTO<object>.Success(result);
        }

        public async Task<ResultDTO<object>> DashbordWorkerDetails(int id)
        {
            var workerDetails = await context.WorkerDetailsView.FirstOrDefaultAsync(p => p.Id == id);

            if (workerDetails == null)
            {

                return ResultDTO<object>.NotFound(new ErrorDTO
                {
                    ErrorEn = "worker Specification not found"
                    ,
                    ErrorAr = "لم يتم اضافة بيانات للعامل بعد"
                });

            }
            var targetJob = await context.JobView.FirstOrDefaultAsync(p => p.Id == workerDetails.JobId);
            var targetPortfolios = await context.PortfolioView.Where(p => p.WorkerId == id).ToListAsync();
            var targetCity = await context.CitiesgetView.FirstOrDefaultAsync(p => p.Id == workerDetails.LivingCityId);
            var targetwallet = await context.WorkerWallets.FirstOrDefaultAsync(p => p.WorkerId == id);
            var targetCanceldOffers = await context.Offers.Where(p => p.WorkerId == id && p.OfferStatus == Enums.OfferStatus.Cancelled.ToString()).CountAsync();
            var targetOrders = await context.OrderDetailsView.Where(p => p.WorkerId == id).ToListAsync();
            var totalIncomeforWorker = await context.OrderDetailsView.Where(p => p.WorkerId == id && p.OrderStatus == Enums.OrderStatus.Completed.ToString()).Select(p => p.TotalWorkerCost).SumAsync();
            var balance = 0.0;
            if (targetwallet is not null)
                balance = targetwallet.Balance;
            var result = new
            {
                ImageURL = workerDetails.ImageURL,
                Email = workerDetails.Email,
                Phone = workerDetails.PhoneNumber,
                Job = targetJob,
                IsCompany = workerDetails.IsCompany,
                City = targetCity,
                Location = workerDetails.Address,
                Bio = workerDetails.Bio,
                RateRatio = workerDetails.RateRito,
                CompletedOrders = workerDetails.CompletedOrders,
                CancelledOffers = targetCanceldOffers,
                TotalIncome = totalIncomeforWorker,
                Balance = balance,
                IdentityImageURL = workerDetails.IdentityImageURL,
                Portfolies = targetPortfolios,
                Orders = targetOrders
            };
            return ResultDTO<object>.Success(result);
        }

        public async Task<ResultDTO<object>> OrderDetails(int id)
        {
            try
            {
                Order? targetOrder = await context.Orders
                    .Include(i => i.Worker)
                    .Include(i => i.Client)
                    .Include(i => i.City)
                    .Include(i => i.Service)
                    .Include(i => i.OrderImages)
                    .FirstOrDefaultAsync(o => o.Id == id);

                if (targetOrder == null)
                {
                    return ResultDTO<object>.NotFound(
                        new ErrorDTO
                        {
                            ErrorAr = "الطلب غير موجود",
                            ErrorEn = "Order not found"
                        }
                    );
                }

                Offer? acceptedOffer = new();
                WorkerSpecification? workerDetails = new();
                double workerBalance = 0.0;
                int workerCancelledOffers = 0;

                // If worker id is not null that means that the order has an accepted offer
                if (targetOrder.WorkerId is not null)
                {
                    acceptedOffer = await context.Offers
                        .FirstOrDefaultAsync(of =>
                            of.OrderId == targetOrder.Id
                            && of.WorkerId == targetOrder.WorkerId
                            && of.OfferStatus == Enums.OfferStatus.Accepted.ToString()
                        );

                    workerDetails = await context.WorkerSpecifications
                        .Include(i => i.User)
                        .Include(i => i.Job)
                        .Include(i => i.LivingCity)
                        .FirstOrDefaultAsync(ws => ws.UserId == targetOrder.WorkerId);

                    workerBalance = await context.WorkerWallets
                        .Where(ww => ww.WorkerId == targetOrder.WorkerId)
                        .Select(r => r.Balance)
                        .FirstOrDefaultAsync();

                    workerCancelledOffers = await context.Offers
                        .Where(of =>
                            of.WorkerId == targetOrder.WorkerId
                            && of.OfferStatus == Enums.OfferStatus.Cancelled.ToString()
                        ).CountAsync();
                }

                var clientData = new
                {
                    ClientId = targetOrder.Client?.Id,
                    ImageURL = targetOrder.Client?.ImageURL,
                    FullName = targetOrder.Client?.FullName,
                    Email = targetOrder.Client?.Email
                };

                var orderData = new
                {
                    OrderId = id,
                    Description = targetOrder.Description,
                    OrderStatus = targetOrder.OrderStatus,
                    Location = targetOrder.Location,
                    ServicingDatetime = targetOrder.ServicingDateTime,
                    OrderImages = targetOrder.OrderImages?.Select(i => i.ImageURL).ToList(),
                    Service = targetOrder.Service?.ServiveName,
                    City = targetOrder.City?.CityName,

                    // if the order has an accepted offer will return the offerd price else will return the order price
                    OrderPrice = (targetOrder.WorkerId is not null) ? acceptedOffer?.OfferedPrice : targetOrder.ProposalPrice,
                };

                dynamic workerData;

                if (targetOrder.WorkerId is null)
                    workerData = new { IsSuccess = false, Message = "لم يتم تعيين عامل على هذا الطلب حتى الان." };
                else
                    workerData = new
                    {
                        IsSuccess = true,
                        WorkerId = workerDetails?.UserId,
                        ImageURL = workerDetails?.User?.ImageURL,
                        Email = workerDetails?.User?.Email,
                        FullName = workerDetails?.User?.FullName,
                        Job = workerDetails?.Job?.JobTitle,
                        IsCompany = workerDetails?.IsCompany,
                        RateRatio = workerDetails?.RateRito,
                        CompletedOrders = workerDetails?.CompletedOrders,
                        CancelledOffers = workerCancelledOffers,
                        Balance = workerBalance
                    };

                var result = new
                {
                    ClientData = clientData,
                    OrderData = orderData,
                    WorkerData = workerData
                };
                return ResultDTO<object>.Success(result);
            }
            catch (Exception ex)
            {
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

        public async Task<ResultDTO<object>> OrderPage()
        {
            var totalOrders = await context.OrderDetailsView.CountAsync();
            var totalActiveOrders = await context.OrderDetailsView.Where(p => p.OrderStatus == Enums.OrderStatus.InProgress.ToString()).CountAsync();
            var totalCompletedOrders = await context.OrderDetailsView.Where(p => p.OrderStatus == Enums.OrderStatus.Completed.ToString()).CountAsync();
            var totalCancelledOrders = await context.OrderDetailsView.Where(p => p.OrderStatus == Enums.OrderStatus.Cancelled.ToString()).CountAsync();
            var ActiveOrders = await context.OrderDetailsView.Where(p => p.OrderStatus == Enums.OrderStatus.InProgress.ToString()).ToListAsync();
            var CompletedAndCancelledOrders = await context.OrderDetailsView.Where(p => p.OrderStatus == Enums.OrderStatus.Completed.ToString() || p.OrderStatus == Enums.OrderStatus.Cancelled.ToString()).ToListAsync();
            // Suggested FIX: previous filter used && which is unsatisfiable; using || to include completed or cancelled
            var result = new
            {
                TotalOrders = totalOrders,
                TotalActiveOrders = totalActiveOrders,
                totalCompletedOrders = totalCompletedOrders,
                TotalCancelledOrder = totalCancelledOrders,
                ActiveOrders = ActiveOrders,
                CompletedAndCancelledOrders = CompletedAndCancelledOrders

            };
            return ResultDTO<object>.Success(result);

        }

        public async Task<ResultDTO<object>> WorkerDetails(int Id)
        {
            try
            {
                var workerSpecs = await context.WorkerSpecifications
                    .Include(i => i.User)
                    .Include(i => i.LivingCity)
                    .Include(i => i.Job)
                    .FirstOrDefaultAsync(ws => ws.UserId == Id);

                if (workerSpecs == null)
                    return ResultDTO<object>.BadRequest(new ErrorDTO()
                    {
                        ErrorAr = "هذا المعرف غير صالح.",
                        ErrorEn = "Is Id is not valid."
                    });

                var workerDeltails = mapper.Map<WorkerSpecificationGetDTO>(workerSpecs);

                var workerPortfolio = await context.WorkerPortfolios.Where(wp => wp.WorkerId == workerSpecs.UserId).ToListAsync();

                if (workerPortfolio != null)
                    workerDeltails.Portfolios = mapper.Map<List<WorkerPortfolioBasicDTO>>(workerPortfolio);

                return ResultDTO<object>.Success(workerDeltails);
            }
            catch (Exception ex)
            {
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

        public async Task<ResultDTO<object>> WorkerPage()
        {
            var workerDetails = await context.WorkerPageView.FirstOrDefaultAsync();
            var newWorkers = await context.NewWorkerView.ToListAsync();
            var allWorkers = await context.AllWorkersView.ToListAsync();
            var suspendedWorkers = await context.SuspendedWorker.ToListAsync();
            var result = new
            {
                TotalWorkers = workerDetails.TotalWorkers,
                TotalNewWorkers = workerDetails.TotalNewWorkers,
                TotalActiveWorker = workerDetails.TotalActiveWorkers,
                AverageWorkersperService = workerDetails.AverageWorkersPerService,
                NewWorkers = newWorkers,
                AllWorkers = allWorkers,
                SuspendedWorkers = suspendedWorkers,
            };
            return ResultDTO<object>.Success(result);
        }

        public async Task<ResultDTO<List<AdminWithRolesAndPermissionsDTO>>> GetAllAdminsWithRolesAndPermissionsAsync()
        {
            var admins = await context.Users
                .Where(u => u.UserType == UserType.Admin.ToString() && !u.IsDeleted)
                .ToListAsync();

            if (!admins.Any())
            {
                return ResultDTO<List<AdminWithRolesAndPermissionsDTO>>.NotFound(new ErrorDTO
                {
                    ErrorAr = "لا يوجد مشرفين في النظام",
                    ErrorEn = "There is no admins in the system"
                });
            }

            var allRoles = await context.Roles.ToListAsync();
            var allRolePermissions = await context.RolePermissions.Include(rp => rp.Permission).ToListAsync();

            var result = new List<AdminWithRolesAndPermissionsDTO>();

            foreach (var admin in admins)
            {
                var roleNames = await userManager.GetRolesAsync(admin);
                var roles = allRoles.Where(r => roleNames.Contains(r.Name)).ToList();
                // var roleIds = roles.Select(r => r.Id).ToList();

                var rolesWithPermissions = roles.Select(role => new RoleWithPermissionsDTO
                {
                    Id = role.Id,
                    Name = role.Name,
                    Permissions = allRolePermissions
                        .Where(rp => rp.RoleId == role.Id)
                        .Select(rp => new PermissionGetDTO()
                        {
                            Id = rp.PermissionId,
                            PermissionName = rp.Permission.PermissionName
                        })
                        .ToList()
                }).ToList();

                result.Add(new AdminWithRolesAndPermissionsDTO
                {
                    UserGetDto = mapper.Map<UserGetDTO>(admin),
                    Roles = rolesWithPermissions
                });
            }
            return ResultDTO<List<AdminWithRolesAndPermissionsDTO>>.Success(result);
        }

    }
}
