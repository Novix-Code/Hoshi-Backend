
using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.Data;
using Hoshi.Models.DashboardModels;
using Hoshi.Models.GlobalModels;
using Hoshi.Repositories.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Hoshi.Repositories.NotificationService
{
    public class NotificationServiceHandler : INotificationServiceHandler
    {
        private readonly IHubContext<NotificationHub, INotificationHub> _hubContext;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly HoshiDbContext _context;
        public NotificationServiceHandler(IHubContext<NotificationHub, INotificationHub> hubContext, HoshiDbContext context, IHttpContextAccessor contextAccessor)
        {
            _hubContext = hubContext;
            _context = context;
            _contextAccessor = contextAccessor;
        }

        public async Task<ResultDTO<object>> getNotificationsAdminAsync()
        {
            var userId = _contextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) 
            {
                return ResultDTO<object>.Failure(new ErrorDTO { ErrorAr = "من فضلك سجل دخول اولا",
                ErrorEn = "please signIn first"} , ResponseStatusCodes.Forbidden);
            }
            var userInt = int.Parse(userId);
            var targetNoties = await _context.AdminNotifications.Where(p => p.AdminId == userInt).Select(p => new
            {
                p.Id,
                p.Title,
                p.Content,
                p.IsRead

            }).ToListAsync();
            return ResultDTO<object>.Success(targetNoties);
            
        }

        public async Task<ResultDTO<object>> getNotificationsClientAndWorkerAsync()
        {
            var userId = _contextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return ResultDTO<object>.Failure(new ErrorDTO
                {
                    ErrorAr = "من فضلك سجل دخول اولا",
                    ErrorEn = "please signIn first"
                }, ResponseStatusCodes.Forbidden);
            }
            var userInt = int.Parse(userId);
            var targetNoties = await _context.UserNotifications.Where(p => p.UserId == userInt)
                .Select(p => new { 
                    p.Id,
                    p.Description ,
                    p.NotificationTypeId
                }).ToListAsync();
            return ResultDTO<object>.Success(targetNoties);

        }

   

        public async Task sendMessagetoAdmin(string messge , int subId)
        {
            var allAdmins = await _context.UserRoles.Where(p => p.RoleId == 2).ToListAsync();
            if (allAdmins.Any())
            {
                foreach (var admin in allAdmins)
                {
                    await _context.AdminNotifications.AddAsync(new AdminNotification
                    {
                        Title = messge,
                        Content = $" Id :{subId} that registered Rigth Now",
                        CreatedAt = DateTime.UtcNow,
                        IsRead = false,
                        AdminId = admin.UserId,
                    });
                    await _context.SaveChangesAsync();
                }
                await _hubContext.Clients.Group("admin").ReceiveMessage(messge);

            }
        }

        public async Task sendMessagetoClient(string message, int Id)
        {
            var notificationhandle = new UserNotification
            {
                UserId = Id,
                CreatedAt = DateTime.UtcNow , 
                NotificationTypeId = 2,
                Description = message
            };
            await _context.UserNotifications.AddAsync(notificationhandle);
            await _context.SaveChangesAsync();
            await _hubContext.Clients.Group("client").ReceiveMessage(message);

        }

        public async Task sendMessagetoWorker(string message, int Id)
        {
            var notificationhandle = new UserNotification
            {
                UserId = Id,
                CreatedAt = DateTime.UtcNow , 
                NotificationTypeId = 1,
                Description = message
            };
            await _context.UserNotifications.AddAsync(notificationhandle);
            await _context.SaveChangesAsync();
            await _hubContext.Clients.Group("worker").ReceiveMessage(message);

        }
    }
}
