using AutoMapper;
using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.DashboardDTOs;
using Hoshi.DTOs.DashboardDTOs.ComplaintDTOs;
using Hoshi.DTOs.GlobalDTOs.ComplaintDTOs;
using Hoshi.DTOs.OrderDTOs.OrderDTOs;
using Hoshi.DTOs.ServiceDTOs.JobDTOs;
using Hoshi.DTOs.UserDTOs.AdminDTOs.PermissionDTOs;
using Hoshi.DTOs.UserDTOs.AdminDTOs.UserPermissionDTOs;
using Hoshi.DTOs.UserDTOs.ClientSpecificationDTOs;
using Hoshi.DTOs.UserDTOs.UserDTOs;
using Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerPortfolioDTOs;
using Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerSpecificationDTOs;
using Hoshi.Enums;
using Hoshi.Models.GlobalModels;
using Hoshi.Models.OrderModels;
using Hoshi.Models.UserModels;
using Hoshi.Models.UserModels.WorkerModels;
using Hoshi.Repositories.FileServiceFold;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
                .Select(o => new
                {
                    o.Id,
                    o.Description,
                    o.ServicingDateTime,
                    o.OrderStatus,
                    o.Client!.FullName,
                    o.Client!.ImageURL
                })
                .Take(10);

            // Get top 10 completed orders
            var completedOrders = context.Orders
                .Include(i => i.City)
                .Include(i => i.Service)
                .Where(p => p.OrderStatus == Enums.OrderStatus.Completed.ToString())
                .Select(o => new
                {
                    o.Id,
                    o.Description,
                    o.ServicingDateTime,
                    o.OrderStatus,
                    o.City!.CityName,
                    o.Service!.ServiveName
                })
                .Take(10);

            // Get top 10 new complaints
            var newComplaints = context.Complaints
                .Include(i => i.ComplaintType)
                .Include(i => i.Order)
                .ThenInclude(o => o.Service)
                .Where(p => p.ComplaintStatus == Enums.ComplaintStatus.Waitting.ToString())
                .Select(o => new
                {
                    o.Id,
                    o.Description,
                    o.ComplaintType!.Type,
                    o.CreatedAt,
                    o.ComplaintStatus,
                    OrderId = o.Order!.Id,
                    o.Order!.Service!.ServiveName
                })
                .Take(10);

            // Get top 10 new payment requests
            var newPaymentRequests =
                from wp in context.WorkerPaymentHistroys
                where wp.IsApproved == false
                join ww in context.WorkerWallets on wp.WorkerId equals ww.WorkerId
                join ws in context.WorkerSpecifications on wp.WorkerId equals ws.UserId
                select new
                {
                    wp.Id,
                    ww.Balance,
                    ws.User!.FullName,
                    ws.User!.ImageURL,
                    ws.Job!.JobTitle
                };

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
                NewPaymentRequests = newPaymentRequests
            };
            return ResultDTO<object>.Success(result);

        }

        // ---------------------
        // Client Page Endpoints
        // ---------------------

        public async Task<ResultDTO<object>> Clientpage()
        {
            // Get page statistics
            var clientPage = await context.ClientPageView.FirstOrDefaultAsync();

            // Get top 10 new Clients
            var newClient = context.NewClientView.Take(10);
            int newClientPagesNum = (int)Math.Ceiling(await context.NewClientView.CountAsync() / 10.0);

            // Get top 10 of all Clients
            var allClient = context.AllClientView.Take(10);
            int allClientPagesNum = (int)Math.Ceiling(await context.AllClientView.CountAsync() / 10.0);

            // Get top 10 of Suspended Clients
            var susClient = context.SuspendedUserView.Take(10);
            int susClientPagesNum = (int)Math.Ceiling(await context.SuspendedUserView.CountAsync() / 10.0);

            var result = new
            {
                TotalClients = clientPage.TotalClients,
                TotalNewClients = clientPage.TotalNewClientsThisMonth,
                TotalActiveClients = clientPage.TotalActiveClients,
                AverageOrder = clientPage.AverageOrdering,
                NewClientsFirstPage = newClient,
                AllClientsFirstPage = allClient,
                SuspendedClientsFirstPage = susClient,
                NewClientPagesNum = newClientPagesNum,
                AllClientPagesNum = allClientPagesNum,
                SuspendedClientPagesNum = susClientPagesNum
            };
            return ResultDTO<object>.Success(result);
        }

        public async Task<ResultDTO<object>> ClientDetails(int Id)
        {
            var targetClient = await context.ClientDetailsView.FirstOrDefaultAsync(p => p.UserId == Id);
            if (targetClient == null)
                return ResultDTO<object>.BadRequest(new ErrorDTO
                {
                    ErrorAr = "هذا المستخدم غير موجود.",
                    ErrorEn = "This user not exist."
                });


            var targetOrders = await context.OrderDetailsView
                .Where(p => p.ClientId == Id)
                .ToListAsync();

            var result = new
            {
                ClientData = targetClient,
                Orders = targetOrders,
            };
            return ResultDTO<object>.Success(result);
        }

        // ---------------------
        // Worker Page Endpoints
        // ---------------------

        public async Task<ResultDTO<object>> WorkerPage()
        {
            // Get page statistics
            var workerDetails = await context.WorkerPageView.FirstOrDefaultAsync();

            // Get top 10 new Workers
            var newWorkers = context.NewWorkerView.Take(10);
            int newWorkerPagesNum = (int)Math.Ceiling(await context.NewWorkerView.CountAsync() / 10.0);


            // Get top 10 of all Workers
            var allWorkers = context.AllWorkersView.Take(10);
            int allWorkerPagesNum = (int)Math.Ceiling(await context.AllWorkersView.CountAsync() / 10.0);


            // Get top 10 of Suspended Workers
            var suspendedWorkers = context.SuspendedWorker.Take(10);
            int susWorkerPagesNum = (int)Math.Ceiling(await context.SuspendedWorker.CountAsync() / 10.0);


            var result = new
            {
                TotalWorkers = workerDetails.TotalWorkers,
                TotalNewWorkers = workerDetails.TotalNewWorkers,
                TotalActiveWorker = workerDetails.TotalActiveWorkers,
                AverageWorkersperService = workerDetails.AverageWorkersPerService,
                NewWorkers = newWorkers,
                AllWorkers = allWorkers,
                SuspendedWorkers = suspendedWorkers,
                NewWorkerPagesNum = newWorkerPagesNum,
                AllWorkerPagesNum = allWorkerPagesNum,
                SuspendedWorkerPagesNum = susWorkerPagesNum

            };
            return ResultDTO<object>.Success(result);
        }

        public async Task<ResultDTO<object>> BeWorkerRequest(int Id)
        {
            try
            {
                // 1- Get worker specifications with its includes
                var workerSpecs = await context.WorkerSpecifications
                    .Include(i => i.Job)
                    .Include(i => i.User)
                    .Include(i => i.LivingCity)
                    .FirstOrDefaultAsync(ws => ws.UserId == Id);

                // Check if it exist
                if (workerSpecs == null)
                    return ResultDTO<object>.BadRequest(new ErrorDTO()
                    {
                        ErrorAr = "هذا المعرف غير صالح.",
                        ErrorEn = "Is Id is not valid."
                    });

                // Map result to DashbordWorkerDetailsDTO to prepare the Result
                var workerDetails = mapper.Map<DashbordWorkerDetailsDTO>(workerSpecs);

                // 2- Get worker portfolio and check if it not null add it to workerDetails.Portfolios
                var workerPortfolio = await context.WorkerPortfolios.Where(wp => wp.WorkerId == workerSpecs.UserId).ToListAsync();

                if (workerPortfolio != null)
                    workerDetails.Portfolios = mapper.Map<List<WorkerPortfolioBasicDTO>>(workerPortfolio);

                // 3- Return final WorkerDetails
                return ResultDTO<object>.Success(workerDetails);
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

        public async Task<ResultDTO<object>> BeWorkerApproval(int Id)
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

        public async Task<ResultDTO<object>> BeWorkerRejection(int Id, string rejectResoun)
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

        public async Task<ResultDTO<object>> DashbordWorkerDetails(int Id)
        {
            try
            {
                // 1- Get worker specifications with its includes
                var workerSpecs = await context.WorkerSpecifications
                    .Include(i => i.Job)
                    .Include(i => i.User)
                    .Include(i => i.LivingCity)
                    .FirstOrDefaultAsync(ws => ws.UserId == Id);

                // Check if it exist
                if (workerSpecs == null)
                    return ResultDTO<object>.BadRequest(new ErrorDTO()
                    {
                        ErrorAr = "هذا المعرف غير صالح.",
                        ErrorEn = "Is Id is not valid."
                    });

                // Map result to DashbordWorkerDetailsDTO to prepare the Result
                var workerDetails = mapper.Map<DashbordWorkerDetailsDTO>(workerSpecs);

                // 2- Get worker portfolio and check if it not null add it to workerDetails.Portfolios
                var workerPortfolio = await context.WorkerPortfolios.Where(wp => wp.WorkerId == workerSpecs.UserId).ToListAsync();
                if (workerPortfolio != null)
                    workerDetails.Portfolios = mapper.Map<List<WorkerPortfolioBasicDTO>>(workerPortfolio);

                // 3- Get all worker orders and check if it not null add it to workerDetails.Orders
                var workerOrders = await context.Orders.Where(o => o.WorkerId == Id).ToListAsync();
                if (workerOrders != null)
                {
                    workerDetails.Orders = mapper.Map<List<OrderBasicDTO>>(workerOrders);

                    // 4- Calculate total user income and total commission fee from completed orders only
                    // First select completed orders from worker orders
                    HashSet<int> completedOrdersIds = workerOrders
                        .Where(o => o.OrderStatus == Enums.OrderStatus.Completed.ToString())
                        .Select(o => o.Id).ToHashSet<int>();

                    // Second select CommissionFee and WorkerTotalPrice from each completed order invoice
                    // Sum those data to get the total of each one and add it to worker details
                    var totals = await context.Invoices
                        .Where(i => completedOrdersIds.Contains(i.OrderId))
                        .GroupBy(i => 1) // group values to fake group to aggregate them
                        .Select(g => new
                        {
                            TotalIncome = g.Sum(i => i.WorkerTotalPrice),
                            TotalCommission = g.Sum(i => i.CommissionFee)
                        })
                        .FirstOrDefaultAsync();

                    workerDetails.TotalWorkerIncome = totals?.TotalIncome ?? 0;
                    workerDetails.TotalCommissionFee = totals?.TotalCommission ?? 0;
                }

                // 5- Get worker balance from worker wallet
                workerDetails.Balance = await context.WorkerWallets.Where(w => w.WorkerId == Id).Select(w => w.Balance).FirstOrDefaultAsync();

                // 6- Returt Worker Details
                return ResultDTO<object>.Success(workerDetails);
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

        // ---------------------
        // Order Page Endpoints
        // ---------------------

        public async Task<ResultDTO<object>> OrderPage()
        {
            var ordersPageValues = await context.OrdersPageView.FirstOrDefaultAsync();

            var finishedOrders = context.FinishedOrdersView
                .Take(10);

            var activeOrders = context.ActiveOrdersView
                .Take(10);

            var result = new
            {
                TotalOrders = ordersPageValues?.TotalOrders,
                TotalActiveOrders = ordersPageValues?.TotalActiveOrders,
                totalCompletedOrders = ordersPageValues?.TotalCompletedOrders,
                TotalCancelledOrder = ordersPageValues?.TotalCancelledOrders,
                ActiveOrders = activeOrders,
                CompletedAndCancelledOrders = finishedOrders,
                ActiveTablePagesNum = (int)Math.Ceiling((double)(ordersPageValues?.TotalOrders ?? 0) / 10.0),
                FinishedTablePagesNum = (int)Math.Ceiling(
                    (double)((ordersPageValues?.TotalCompletedOrders ?? 0)
                    + (ordersPageValues?.TotalCancelledOrders ?? 0))
                    / 10.0
                )
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

        public async Task<ResultDTO<object>> GetServicesPageAsync()
        {
            try
            {
                var jobs = await context.JobsTableView.ToListAsync();

                var categories = await context.CategoriesTableView.ToListAsync();

                var allServices = await context.ServicesTableView.ToListAsync();

                var services = allServices
                    .GroupBy(s => s.CategoryName)
                    .Select(g => new
                    {
                        CategoryName = g.Key,
                        TotalServsNum = g.Count(),
                        ActiveServsNum = g.Count(s => !s.IsDeleted),
                        Services = g.ToList()
                    })
                    .ToList();

                //Dictionary<string, List<object>> servicesTables = 

                return ResultDTO<object>.Success(new
                {
                    JobsTable = jobs,
                    CategoriesTable = categories,
                    ServicesTable = services
                });
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

        // ---------------------
        // Payment Page Endpoints
        // ---------------------

        /// <summary>
        /// Aggregate payments page data: invoice sums, payment requests, and uncollected fees.
        /// </summary>
        public async Task<ResultDTO<object>> GetPaymentsPageAsync()
        {
            var pageStatistics = await context.PaymentsPageView.FirstOrDefaultAsync();

            var workerPaymentRequests = context.PaymentRequestsView.Take(10);

            var workersUncollectedFees = context.WorkerUncollectedFeesView.Take(10);

            var clientsUncollectedFees = context.ClientUncollectedFeesView.Take(10);

            int wprPagesNum = (int)Math.Ceiling(await context.PaymentRequestsView.CountAsync() / 10.0);
            int wufPagesNum = (int)Math.Ceiling(await context.WorkerUncollectedFeesView.CountAsync() / 10.0);
            int cufPagesNum = (int)Math.Ceiling(await context.ClientUncollectedFeesView.CountAsync() / 10.0);

            return ResultDTO<object>.Success(new
            {
                TotalOrdersIncome = pageStatistics?.TotalOrdersIncome,
                TotalOrdersPrices = pageStatistics?.TotalOrdersPrices,
                TotalOrdersFees = pageStatistics?.TotalOrdersFees,
                TotalUncollectedFees = pageStatistics?.TotalUncollectedFees,
                WorkerPaymentRequests = workerPaymentRequests,
                WorkersUncollectedFees = workersUncollectedFees,
                ClientsUncollectedFees = clientsUncollectedFees,
                RequestsPagesNum = wprPagesNum,
                WorkerUFPagesNum = wufPagesNum,
                ClientUFPagesNum = cufPagesNum
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

        // ---------------------
        // Complaints Page Endpoints
        // ---------------------

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
            var opened = await query.CountAsync(c => c.ComplaintStatus != ComplaintStatus.Solved.ToString()); // all except solved
            var closed = await query.CountAsync(c => c.ComplaintStatus == ComplaintStatus.Solved.ToString()); // just solved

            var complaints = await query
                .OrderByDescending(c => c.CreatedAt)
                .Select(c => new ComplaintItemDTO
                {
                    ComplaintId = c.Id,
                    Description = c.Description,
                    FullName = c.User!.FullName,
                    UserImageUrl = c.User.ImageURL,
                    ComplaintType = c.ComplaintType!.Type,
                    ComplaintStatus = c.ComplaintStatus,
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
                // Use async for better performance
                var getComplaint = await context.Complaints.FirstOrDefaultAsync(c => c.Id == complaintCreateDto.ComplaintId);
                if (getComplaint == null)
                    return ResultDTO<MessageDTO>.NotFound(new ErrorDTO
                    {
                        ErrorAr = "الشكوى غير موجودة.",
                        ErrorEn = "Complaint not found."
                    });

                if (getComplaint.ComplaintStatus == ComplaintStatus.Solved.ToString())
                    return ResultDTO<MessageDTO>.BadRequest(new ErrorDTO
                    {
                        ErrorAr = "تم الرد على الشكوى بالفعل.",
                        ErrorEn = "The complaint has already been answered."
                    });

                // Update complaint
                if (string.IsNullOrWhiteSpace(complaintCreateDto.Response))
                    complaintCreateDto.Response = "تم حل المشكلة الخاصة بك من قبل احد المشرفين.";
                
                getComplaint.Response = complaintCreateDto.Response.Trim();

                getComplaint.ComplaintStatus = ComplaintStatus.Solved.ToString();
                getComplaint.ModifiedAt = DateTime.UtcNow;

                await context.SaveChangesAsync();
                await transaction.CommitAsync();

                return ResultDTO<MessageDTO>.Success(new MessageDTO
                {
                    MessageAr = "تم الرد على الشكوى وغلقها بنجاح.",
                    MessageEn = "The complaint has been successfully responded to and closed."
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
