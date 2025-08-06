using AutoMapper;
using Hoshi.Data;
using Hoshi.Models.UserModels;
using Hoshi.Repositories.FileServiceFold;
using Hoshi.DTOs.FileServicieResult;
using Hoshi.DTOs.UserDTOs.UserDTOs;
using Hoshi.DTOs.UserDTOs.UserRegistiration;
using Hoshi.Models.GlobalModels;
using Hoshi.Models.ServiceModels;
using Hoshi.Models.UserModels;
using Hoshi.Models.UserModels.Resets;
using Hoshi.Models.ViewModels;
using Hoshi.Repositories.FileServiceFold;
using Hoshi.Repositories.TokenService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit.Cryptography;
using Org.BouncyCastle.Crypto.Engines;
using OtpNet;
using System;
using System.Runtime.ConstrainedExecution;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Hoshi.DTOs.UserDTOs.AdminDTOs.AdminPageDTOs;
using Hoshi.DTOs.UserDTOs.AdminDTOs.PermissionDTOs;
using Hoshi.DTOs.UserDTOs.AdminDTOs.UserPermissionDTOs;
using Hoshi.Enums;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Object = System.Object;
using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;

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
                    _fileService.DeleteFile(user.ImageURL!);
                }

                // User FileService method to save the image to the images\personalimages folder in wwwroot
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
                // If any thing happends return the exception
                return new Tuple<bool, string>(false, ex.InnerException!.Message);
            }
        }

        public async Task<ResultDTO<object>> BeWorkerApproved(int Id)
        {
            var tergetWorkerSpecif = await _context.WorkerSpecifications.Where(p => p.UserId == Id).FirstOrDefaultAsync();
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
                await _context.SaveChangesAsync();
                return ResultDTO<object>.Success("Worker is now approved");
            }
            
        }

        public async Task<ResultDTO<object>> BeWorkerReject(int Id, string rejectResoun)
        {
            var tergetWorkerSpecif = await _context.WorkerSpecifications.Where(p => p.UserId == Id).FirstOrDefaultAsync();
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
                await _context.SaveChangesAsync();
                return ResultDTO<object>.Success("Worker is now Rejected");
            }
        }

        public async Task<ResultDTO<object>> ClientDetails(int Id)
        {
            var targetClient = await _context.ClientDetailsView.FirstOrDefaultAsync(p => p.UserId == Id);
            if (targetClient == null)
                return ResultDTO<object>.Failure(new ErrorDTO() , ResponseStatusCodes.NotFound);

            var targetCity = await _context.CitiesgetView.FirstOrDefaultAsync(p => p.Id == targetClient.LivingCityId);
            var targetOrders = await _context.OrdersGetView
                .Where(p => p.ClientId == Id)
                .ToListAsync();

            var result = new
            {
                ImageURL = targetClient.ImageURL,
                Email = targetClient.Email,
                Phone = targetClient.PhoneNumber,
                Location = targetClient.Address,
                City = targetCity,
                Orders = targetOrders,
            };
            return ResultDTO<object>.Success(result);
        }


        public async Task<ResultDTO<object>> Clientpage()
        {
            var clientPage = await _context.ClientPageView4.FirstOrDefaultAsync();
            var newClient = await _context.NewClientView.FirstOrDefaultAsync();
            var allClient = await _context.AllClientView.FirstOrDefaultAsync();
            var susClient = await _context.SuspendedUser.FirstOrDefaultAsync();
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
                return ResultDTO<object>.Failure(new ErrorDTO(), ResponseStatusCodes.NotFound);
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
            var targetCity = await _context.CitiesgetView.FirstOrDefaultAsync(p => p.Id == targetClient.LivingCityId);
            if (targetCity == null)
            {
                return ResultDTO<object>.Failure(new ErrorDTO { ErrorAr = "City Not Found" }, ResponseStatusCodes.NotFound);

            }
            var targetOffer = await _context.Offers.FirstOrDefaultAsync(p=>p.OrderId == id);
            var targetimages = await _context.Orders.Where(p => p.Id == id).Select(p => p.OrderImages).ToListAsync();
            var workerDetails = await _context.WorkerDetailsView.FirstOrDefaultAsync(p => p.UserId == TargetOrder.WorkerId);
            var targetJob = await _context.JobView.FirstOrDefaultAsync(p => p.Id == workerDetails.JobId);
            var targetCanceldOffers = await _context.Offers.Where(p => p.WorkerId == TargetOrder.WorkerId && p.OfferStatus == Enums.OfferStatus.Cancelled).CountAsync();
            var targetwallet = await _context.WorkerWallets.FirstOrDefaultAsync(p => p.WorkerId == TargetOrder.WorkerId);
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
                City       = targetCity,
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
    }
}
