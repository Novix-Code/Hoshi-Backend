using AutoMapper;
using Azure;
using GenericCRUDLibrary.GenericDTOs.InputsDTOs;
using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.Data;
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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Hoshi.Repositories.UserService
{
    public class UserService : IUserService
    {
        private readonly HoshiDbContext _context;
        public UserService(HoshiDbContext context)
        {
            _context = context;
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
    }

}
