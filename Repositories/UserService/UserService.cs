using AutoMapper;
using Azure;
using GenericCRUDLibrary.GenericDTOs.InputsDTOs;
using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.FileServicieResult;
using Hoshi.DTOs.UserDTOs.UserDTOs;
using Hoshi.DTOs.UserDTOs.UserRegistiration;
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

        //public Task<ResultDTO<object>> Clientpage()
        //{
        //    throw new NotImplementedException();
        //}

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

    }

}
