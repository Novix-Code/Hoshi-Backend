using AutoMapper;
using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.UserDTOs.AdminDTOs.PermissionDTOs;
using Hoshi.DTOs.UserDTOs.AdminDTOs.UserPermissionDTOs;
using Hoshi.DTOs.UserDTOs.SuspendedUserDTOs;
using Hoshi.DTOs.UserDTOs.UserDTOs;
using Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerPortfolioDTOs;
using Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerSpecificationDTOs;
using Hoshi.Enums;
using Hoshi.Models.GlobalModels;
using Hoshi.Models.OrderModels;
using Hoshi.Models.UserModels;
using Hoshi.Models.UserModels.WorkerModels;
using Hoshi.Repositories.FileServiceFold;
using Hoshi.Repositories.TokenService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Hoshi.Repositories.UserService
{
    /// <summary>
    /// Implements user-related operations for admin dashboard views, profile image management,
    /// worker application approval/rejection, and admin role-permission listings.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly HoshiDbContext _context;
        private readonly ITokenService _tokenService;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;

        public UserService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            HoshiDbContext context,
            ITokenService tokenService,
            IMapper mapper,
            IFileService fileService
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _tokenService = tokenService;
            _fileService = fileService;
            _mapper = mapper;
        }

        /// <summary>
        /// Save user's personal image to storage and update user's ImageURL.
        /// </summary>
        /// <param name="id">User id that will be updated</param>
        /// <param name="image">Image file that will be saved</param>
        /// <returns>Return a Tuple of Bool and String to check if done successfully or not and get the image url or the error.</returns>
        public async Task<Tuple<bool, string>> AddUserImage(int id, IFormFile image, bool isUpdate)
        {
            try
            {
                // Fetch user data from its id
                User? user = await _context.Set<User>().FindAsync(id);

                // Check if this user is there or not
                if (user == null)
                    return new Tuple<bool, string>(false, "Invalid User Id.");

                if (isUpdate)
                {
                    if (user.ImageURL != null)
                        _fileService.DeleteFile(user.ImageURL!);
                }

                // Use FileService to save the image to images/personalimages in wwwroot
                var imageResult = await _fileService.SaveFileAsync(image, "images\\personalimages");

                // Check if the image saved successfuly
                if (imageResult.Item1)
                {
                    // Add image url to user object data
                    user.ImageURL = imageResult.Item2;
                    // Update it in db and save changes
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }

                // return the images result in all cases
                return imageResult;
            }
            catch (Exception ex)
            {
                // If anything happens return the exception
                return new Tuple<bool, string>(false, ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ResultDTO<object>> BeWorkerApproved(int Id)
        {
            var tergetWorkerSpecif = await _context.WorkerSpecifications.Where(p => p.UserId == Id).FirstOrDefaultAsync();
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
            var checkexcist = await _context.NotificationTypes.Where(p => p.Type == "Success Message").Select(p => p.Id).FirstOrDefaultAsync();
            if (checkexcist == 0)
            {
                var notiType = new NotificationType
                {
                    Title = "Successfully Approved",
                    ForClient = false,
                    Type = "Success Message"
                };
                await _context.NotificationTypes.AddAsync(notiType);
                await _context.SaveChangesAsync();
                _context.UserNotifications.Add(new UserNotification
                {
                    NotificationTypeId = notiType.Id,
                    Description = "success Message",
                    UserId = Id
                });
                _context.WorkerSpecifications.Update(tergetWorkerSpecif);
                await _context.SaveChangesAsync();
                return ResultDTO<object>.Success("Worker is now approved");

            }
            else
            {
                _context.UserNotifications.Add(new UserNotification
                {
                    NotificationTypeId = checkexcist,
                    Description = "success Message",
                    UserId = Id
                });
                _context.WorkerSpecifications.Update(tergetWorkerSpecif);
                await _context.SaveChangesAsync();
                return ResultDTO<object>.Success("Worker is now approved");
            }

        }

        public async Task<ResultDTO<object>> BeWorkerReject(int Id, string rejectResoun)
        {
            var tergetWorkerSpecif = await _context.WorkerSpecifications.Where(p => p.UserId == Id).FirstOrDefaultAsync();
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
            await _context.SaveChangesAsync();
            var checkexcist = await _context.NotificationTypes.Where(p => p.Type == "Reject Message").Select(p => p.Id).FirstOrDefaultAsync();
            if (checkexcist == 0)
            {
                var notiType = new NotificationType
                {
                    Title = "Successfully Reject",
                    ForClient = false,
                    Type = "Reject Message"
                };
                await _context.NotificationTypes.AddAsync(notiType);
                await _context.SaveChangesAsync();
                _context.UserNotifications.Add(new UserNotification
                {
                    NotificationTypeId = notiType.Id,
                    Description = rejectResoun,
                    UserId = Id
                });
                _context.WorkerSpecifications.Update(tergetWorkerSpecif);
                await _context.SaveChangesAsync();
                return ResultDTO<object>.Success("Worker is now Rejected");

            }
            else
            {
                _context.UserNotifications.Add(new UserNotification
                {
                    NotificationTypeId = checkexcist,
                    Description = rejectResoun,
                    UserId = Id
                });
                _context.WorkerSpecifications.Update(tergetWorkerSpecif);
                await _context.SaveChangesAsync();
                return ResultDTO<object>.Success("Worker is now Rejected");
            }
        }

        public async Task<ResultDTO<object>> ClientDetails(int Id)
        {
            var targetClient = await _context.ClientDetailsView.FirstOrDefaultAsync(p => p.UserId == Id);
            if (targetClient == null)
                return ResultDTO<object>.Failure(new ErrorDTO { ErrorEn = "client not found" }, ResponseStatusCodes.NotFound);


            var targetOrders = await _context.OrderDetailsView
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
            var clientPage = await _context.ClientPageView.FirstOrDefaultAsync();
            var newClient = await _context.NewClientView.ToListAsync();
            var allClient = await _context.AllClientView.ToListAsync();
            var susClient = await _context.SuspendedUserView.ToListAsync();
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
            var workerDetails = await _context.WorkerDetailsView.FirstOrDefaultAsync(p => p.Id == id);

            if (workerDetails == null)
            {

                return ResultDTO<object>.NotFound(new ErrorDTO
                {
                    ErrorEn = "worker Specification not found"
                    ,
                    ErrorAr = "لم يتم اضافة بيانات للعامل بعد"
                });

            }
            var targetJob = await _context.JobView.FirstOrDefaultAsync(p => p.Id == workerDetails.JobId);
            var targetPortfolios = await _context.PortfolioView.Where(p => p.WorkerId == id).ToListAsync();
            var targetCity = await _context.CitiesgetView.FirstOrDefaultAsync(p => p.Id == workerDetails.LivingCityId);
            var targetwallet = await _context.WorkerWallets.FirstOrDefaultAsync(p => p.WorkerId == id);
            var targetCanceldOffers = await _context.Offers.Where(p => p.WorkerId == id && p.OfferStatus == Enums.OfferStatus.Cancelled.ToString()).CountAsync();
            var targetOrders = await _context.OrderDetailsView.Where(p => p.WorkerId == id).ToListAsync();
            var totalIncomeforWorker = await _context.OrderDetailsView.Where(p => p.WorkerId == id && p.OrderStatus == Enums.OrderStatus.Completed.ToString()).Select(p => p.TotalWorkerCost).SumAsync();
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
                Order? targetOrder = await _context.Orders
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
                    acceptedOffer = await _context.Offers
                        .FirstOrDefaultAsync(of =>
                            of.OrderId == targetOrder.Id
                            && of.WorkerId == targetOrder.WorkerId
                            && of.OfferStatus == Enums.OfferStatus.Accepted.ToString()
                        );

                    workerDetails = await _context.WorkerSpecifications
                        .Include(i => i.User)
                        .Include(i => i.Job)
                        .Include(i => i.LivingCity)
                        .FirstOrDefaultAsync(ws => ws.UserId == targetOrder.WorkerId);

                    workerBalance = await _context.WorkerWallets
                        .Where(ww => ww.WorkerId == targetOrder.WorkerId)
                        .Select(r => r.Balance)
                        .FirstOrDefaultAsync();

                    workerCancelledOffers = await _context.Offers
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
            var totalOrders = await _context.OrderDetailsView.CountAsync();
            var totalActiveOrders = await _context.OrderDetailsView.Where(p => p.OrderStatus == Enums.OrderStatus.InProgress.ToString()).CountAsync();
            var totalCompletedOrders = await _context.OrderDetailsView.Where(p => p.OrderStatus == Enums.OrderStatus.Completed.ToString()).CountAsync();
            var totalCancelledOrders = await _context.OrderDetailsView.Where(p => p.OrderStatus == Enums.OrderStatus.Cancelled.ToString()).CountAsync();
            var ActiveOrders = await _context.OrderDetailsView.Where(p => p.OrderStatus == Enums.OrderStatus.InProgress.ToString()).ToListAsync();
            var CompletedAndCancelledOrders = await _context.OrderDetailsView.Where(p => p.OrderStatus == Enums.OrderStatus.Completed.ToString() || p.OrderStatus == Enums.OrderStatus.Cancelled.ToString()).ToListAsync();
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

        public async Task<ResultDTO<object>> OverViewPage()
        {
            var overResult = await _context.OverviewView.FirstOrDefaultAsync();
            var newOrders = _context.Orders.Where(p => p.OrderStatus == Enums.OrderStatus.Published.ToString()).Take(10);
            var completedOrders = _context.Orders.Where(p => p.OrderStatus == Enums.OrderStatus.Completed.ToString()).Take(10);
            var newComplaints = _context.Complaints.Where(p => p.ComplaintStatus == Enums.ComplaintStatus.Waitting.ToString()).Take(10);
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

        public async Task<ResultDTO<object>> WorkerDetails(int Id)
        {
            try
            {
                var workerSpecs = await _context.WorkerSpecifications
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

                var workerDeltails = _mapper.Map<WorkerSpecificationGetDTO>(workerSpecs);

                var workerPortfolio = await _context.WorkerPortfolios.Where(wp => wp.WorkerId == workerSpecs.UserId).ToListAsync();

                if (workerPortfolio != null)
                    workerDeltails.Portfolios = _mapper.Map<List<WorkerPortfolioBasicDTO>>(workerPortfolio);

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
            var workerDetails = await _context.WorkerPageView.FirstOrDefaultAsync();
            var newWorkers = await _context.NewWorkerView.ToListAsync();
            var allWorkers = await _context.AllWorkersView.ToListAsync();
            var suspendedWorkers = await _context.SuspendedWorker.ToListAsync();
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
            var admins = await _context.Users
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

            var allRoles = await _context.Roles.ToListAsync();
            var allRolePermissions = await _context.RolePermissions.Include(rp => rp.Permission).ToListAsync();

            var result = new List<AdminWithRolesAndPermissionsDTO>();

            foreach (var admin in admins)
            {
                var roleNames = await _userManager.GetRolesAsync(admin);
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
                    UserGetDto = _mapper.Map<UserGetDTO>(admin),
                    Roles = rolesWithPermissions
                });
            }
            return ResultDTO<List<AdminWithRolesAndPermissionsDTO>>.Success(result);
        }

        public async Task<ResultDTO<object>> SuspendUser(SuspendedUserPostDTO suspendDTO)
        {
            var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Check user
                User? user = await _context.Set<User>().FindAsync(suspendDTO.UserId);

                if (user == null)
                    return ResultDTO<object>.BadRequest(
                        new ErrorDTO()
                        {
                            ErrorAr = "هذا المستخدم غير موجود.",
                            ErrorEn = "This user not existed."
                        }
                    );

                // Check suspend reason
                SuspendReason? reason = await _context.Set<SuspendReason>().FindAsync(suspendDTO.SuspendReasonId);

                if (reason == null)
                    return ResultDTO<object>.BadRequest(
                        new ErrorDTO()
                        {
                            ErrorAr = "هذا السبب غير موجود.",
                            ErrorEn = "This reason not existed."
                        }
                    );

                // Check if user already is suspended
                SuspendedUser? suspendUser = await _context.Set<SuspendedUser>()
                    .FirstOrDefaultAsync(su => su.UserId == suspendDTO.UserId);

                if (suspendUser != null)
                    return ResultDTO<object>.BadRequest(
                        new ErrorDTO()
                        {
                            ErrorAr = "هذا المستخدم تم تعليقه بالفعل.",
                            ErrorEn = "This user is already suspended."
                        }
                    );
                else
                {
                    // If the user not has an active suspention will add the new suspention
                    await _context.Set<SuspendedUser>().AddAsync(_mapper.Map<SuspendedUser>(suspendDTO));

                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    return ResultDTO<object>.Success(new MessageDTO()
                    {
                        MessageAr = "تم التعليق بنجاح.",
                        MessageEn = "Suspention done successfully."
                    });
                }
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                return ResultDTO<object>.InternalServerError(
                    new ErrorDTO()
                    {
                        ErrorAr = "يوجد مشكلة في عملية التعليق.",
                        ErrorEn = "There is a problem in Suspending process."
                    },
                    ex.InnerException != null ? ex.InnerException.Message : ex.Message
                );
            }
        }
    }
}
