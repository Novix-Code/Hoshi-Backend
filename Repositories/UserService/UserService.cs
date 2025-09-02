using AutoMapper;
using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.UserDTOs.AdminDTOs.PermissionDTOs;
using Hoshi.DTOs.UserDTOs.AdminDTOs.UserPermissionDTOs;
using Hoshi.DTOs.UserDTOs.SuspendedUserDTOs;
using Hoshi.DTOs.UserDTOs.UserDTOs;
using Hoshi.Enums;
using Hoshi.Models.GlobalModels;
using Hoshi.Models.UserModels;
using Hoshi.Repositories.FileServiceFold;
using Hoshi.Repositories.TokenService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Hoshi.Repositories.UserService
{
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
        /// This Method to save User Personal Image and update user data.
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
                if(user == null)
                    return new Tuple<bool, string>(false, "Invalid User Id.");

                if (isUpdate)
                {
                    if(user.ImageURL !=null)
                    _fileService.DeleteFile(user.ImageURL!);
                }

                // User FileService method to save the image to the images\personalimages folder in wwwroot
                var imageResult = await _fileService.SaveFileAsync(image, "images/personalimages");

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
                // If any thing happends return the exception
                return new Tuple<bool, string>(false, ex.InnerException!.Message);
            }
        }

        public async Task<ResultDTO<object>> BeWorkerApproved(int Id)
        {
            var tergetWorkerSpecif = await _context.WorkerSpecifications.Where(p => p.UserId == Id).FirstOrDefaultAsync();
            if( tergetWorkerSpecif == null)
            {
                return ResultDTO<object>.NotFound(new ErrorDTO { ErrorEn = "worker Specification not found" 
                    ,ErrorAr="لم يتم اضافة بيانات للعامل بعد"});
            }

            // Check if the worker is approved once before so can not be approved again
            if (tergetWorkerSpecif.IsApproved is true)
            {
                return ResultDTO<object>.BadRequest
                (
                    new ErrorDTO { 
                        ErrorAr= "تم قبول العامل بالفعل.",
                        ErrorEn = "Worker already be approved."
                    }
                );
            }

            tergetWorkerSpecif.IsApproved = true;

            // handle add notifications 
            var checkexcist = await _context.NotificationTypes.Where(p=>p.Type == "Success Message").Select(p=>p.Id).FirstOrDefaultAsync(); 
            if (checkexcist ==0){
                var notiType = new NotificationType
                {
                    Title = "Successfully Approved",
                    ForClient = false,
                    Type = "Success Message"
                };
                await _context.NotificationTypes.AddAsync(notiType);
                await _context.SaveChangesAsync();
                _context.UserNotifications.Add( new UserNotification
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

            // Check if the user is already be approved and in this case can not be rejected
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
                return ResultDTO<object>.Failure(new ErrorDTO { ErrorEn = "client not found"} , ResponseStatusCodes.NotFound);

           
            var targetOrders = await _context.OrdersGetView
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
            var clientPage = await _context.ClientPageView4.FirstOrDefaultAsync();
            var newClient = await _context.NewClientView.ToListAsync();
            var allClient = await _context.AllClientView.ToListAsync();
            var susClient = await _context.SuspendedUserView.ToListAsync();
            var result = new
            {
                TotalClients = clientPage.totalClients,
                TotalNewClients = clientPage.totalNewClients,
                TotalActiveClients = clientPage.totalActiveClients,
                averageOrder = clientPage.AverageOrdering,
                NewClients = newClient,
                AllClient  = allClient,
                SuspendedClients = susClient,  

            };
            return ResultDTO<object>.Success(result);
        }

        public async Task<ResultDTO<object>> DashbordWorkerDetails(int id)
            {
            var workerDetails = await _context.WorkerDetailsView.FirstOrDefaultAsync(p => p.UserId == id);

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
            var targetwallet = await _context.WorkerWallets.FirstOrDefaultAsync(p=>p.WorkerId == id);
            var targetCanceldOffers = await _context.Offers.Where(p => p.WorkerId == id && p.OfferStatus == Enums.OfferStatus.Cancelled).CountAsync();
            var targetOrders = await _context.OrdersGetView.Where(p => p.WorkerId == id).ToListAsync();
            var totalIncomeforWorker = await _context.OrdersGetView.Where(p => p.WorkerId == id && p.OrderStatus == Enums.OrderStatus.Completed).Select(p => p.TotalWorkerCost).SumAsync();

            var result = new
            {
                ImageURL = workerDetails.ImageURL ,
                Email    = workerDetails.Email ,
                Phone    = workerDetails.PhoneNumber ,
                Job      = targetJob , 
                IsCompany= workerDetails.IsCompany ,
                City     = targetCity,
                Location = workerDetails.Address , 
                Bio      = workerDetails.Bio,
                RateRatio= workerDetails.RateRito,
                CompletedOrders = workerDetails.CompletedOrders ,
                CancelledOffers = targetCanceldOffers ,
                TotalIncome     = totalIncomeforWorker ,
                Balance         = targetwallet.Balance ,
                IdentityImageURL= workerDetails.IdentityImageURL ,
                Portfolies      = targetPortfolios ,
                Orders          = targetOrders 
            };
            return ResultDTO<object>.Success(result);
        }

        public async Task<ResultDTO<object>> OrderDetails(int id)
        {
            var TargetOrder = await _context.OrdersGetView.FirstOrDefaultAsync(p=>p.Id == id);
            if (TargetOrder == null)
                return ResultDTO<object>.Failure(new ErrorDTO { ErrorAr ="Order Not Found"}, ResponseStatusCodes.NotFound);
            var targetClient = await _context.ClientDetailsView.FirstOrDefaultAsync(p => p.UserId == TargetOrder.ClientId);
            if (targetClient == null)
                return ResultDTO<object>.Failure(new ErrorDTO { ErrorAr="Client Not Found"}, ResponseStatusCodes.NotFound);
        
            var targetOffer = await _context.Offers.FirstOrDefaultAsync(p=>p.OrderId == id);
            if (targetOffer == null)
                return ResultDTO<object>.NotFound(new ErrorDTO {ErrorEn="offered Not Found",
                                                                ErrorAr="لا يوجد عروض على هذا الطلب"});
            var targetimages = await _context.Orders.Where(p => p.Id == id).Select(p => p.OrderImages).ToListAsync();
            if(targetimages == null)
                return ResultDTO<object>.NotFound(new ErrorDTO
                {
                    ErrorEn = "Order Image Not Found",
                    ErrorAr = "لا يوجد صور لهذا الطلب"
                });
            var workerDetails = await _context.WorkerDetailsView.FirstOrDefaultAsync(p => p.UserId == targetOffer.WorkerId);
            if (workerDetails == null)
                return ResultDTO<object>.NotFound(new ErrorDTO
                {
                    ErrorEn = "Worker Not Found",
                    ErrorAr = "لم يتم تحديد عامل لهذا العرض"
                });
            var targetJob = await _context.JobView.FirstOrDefaultAsync(p => p.Id == workerDetails.JobId);
            if (targetJob == null)
                return ResultDTO<object>.NotFound(new ErrorDTO
                {
                    ErrorEn = "worker Job Not Found",
                    ErrorAr = "لم يتم  اضافة وظيفة للعامل بعد"
                });
            var targetCanceldOffers = await _context.Offers.Where(p => p.WorkerId == TargetOrder.WorkerId && p.OfferStatus == Enums.OfferStatus.Cancelled).CountAsync();
            var targetwallet = await _context.WorkerWallets.FirstOrDefaultAsync(p => p.WorkerId == targetOffer.WorkerId);
            if (targetwallet == null)
                return ResultDTO<object>.NotFound(new ErrorDTO
                {
                    ErrorEn = "worker wallet not found",
                    ErrorAr = "لم يتم اضافة محفظه للعالم"
                });
            var clientData = new
            {
                ImageURL = targetClient.ImageURL,
                FullName = targetClient.FullName , 
                Email    = targetClient.Email 
            };
            var OrderData = new
            {
                OrderId = id , 
                ClientData = clientData,
                Description= TargetOrder.Description , 
                OrderStatus= TargetOrder.OrderStatus , 
                Adress       = targetClient.Address,
                Location   = TargetOrder.Location ,
                ServicingDatetime = TargetOrder.ServicingDateTime ,
                OfferedPrice        = targetOffer.OfferedPrice,
                OrderImages         = targetimages 
            };
            var workerData = new
            {
                ImageURL = workerDetails.ImageURL , 
                Email    = workerDetails.Email ,
                FullName = workerDetails.FullName,
                Job      = targetJob , 
                IsCompany= workerDetails.IsCompany , 
                RateRatio= workerDetails.RateRito , 
                CompletedOrders = workerDetails.CompletedOrders ,
                CancelledOffers = targetCanceldOffers,
                Balance = targetwallet.Balance




            };
            var result = new
            {
                OrderData = OrderData,
                WorkerData = workerData
            };
            return ResultDTO<object>.Success(result);

        }

        public async Task<ResultDTO<object>> OrderPage()
        {
            var totalOrders = await _context.OrdersGetView.CountAsync();
            var totalActiveOrders = await _context.OrdersGetView.Where(p => p.OrderStatus == Enums.OrderStatus.InProgress).CountAsync();
            var totalCompletedOrders = await _context.OrdersGetView.Where(p => p.OrderStatus == Enums.OrderStatus.Completed).CountAsync();
            var totalCancelledOrders = await _context.OrdersGetView.Where(p => p.OrderStatus == Enums.OrderStatus.Cancelled).CountAsync();
            var ActiveOrders = await _context.OrdersGetView.Where(p => p.OrderStatus == Enums.OrderStatus.InProgress).ToListAsync();
            var CompletedAndCancelledOrders = await _context.OrdersGetView.Where(p => p.OrderStatus == Enums.OrderStatus.Completed && p.OrderStatus== Enums.OrderStatus.Cancelled).ToListAsync();
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

        public async Task<ResultDTO<object>> overViewPage()
        {
            var overResult = await _context.OverviewView.FirstOrDefaultAsync();
            var newOrders = await _context.Orders.Where(p => p.OrderStatus == Enums.OrderStatus.Published).ToListAsync();
            var completedOrders = await _context.Orders.Where(p => p.OrderStatus == Enums.OrderStatus.Completed).ToListAsync();
            var newComplaints = await _context.Complaints.Where(p => p.ComplaintStatus == Enums.ComplaintStatus.Waitting).ToListAsync();
            if (overResult == null)
            {
                return ResultDTO<object>.Failure(new ErrorDTO() , ResponseStatusCodes.NotFound);
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
            var workerDetails = await _context.WorkerDetailsView.FirstOrDefaultAsync(p => p.UserId == Id);
            if (workerDetails == null) 
            {
                return ResultDTO<object>.Failure(new ErrorDTO(), ResponseStatusCodes.NotFound);
            }
            var targetJob = await _context.JobView.FirstOrDefaultAsync(p => p.Id == workerDetails.JobId);
            var targetPortfolio = await _context.PortfolioView.FirstOrDefaultAsync(p => p.WorkerId == Id);
            var targetCity = await _context.CitiesgetView.FirstOrDefaultAsync(p=>p.Id == workerDetails.LivingCityId);

            var result = new
            {
                ImageURL = workerDetails.ImageURL , 
                Email    = workerDetails.Email ,
                Phone    = workerDetails.PhoneNumber , 
                Job      = targetJob ,
                IsCompany= workerDetails.IsCompany ,
                City     = targetCity , 
                Location = workerDetails.Address , 
                Portfolio= targetPortfolio

            };
            return ResultDTO<object>.Success(result);

        }

        public async Task<ResultDTO<object>> WorkerPage()
        {
            var workerDetails = await _context.WorkerPageView.FirstOrDefaultAsync();
            var newWorkers    = await _context.NewWorkerView.ToListAsync();
            var allWorkers    = await _context.AllWorkertView.ToListAsync();    
            var suspendedWorkers= await _context.SuspendedWorker.ToListAsync();
            var result = new
            {
                TotalWorkers = workerDetails.totalClients,
                TotalNewWorkers = workerDetails.totalNewClients,
                TotalActiveWorker = workerDetails.totalActiveClients,
                AverageWorkersperService = workerDetails.AverageOrdering ,
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
