
using Hoshi.Data;
using Hoshi.Repositories.NotificationService;
using Microsoft.EntityFrameworkCore;

namespace Hoshi.Repositories.OrderVisitService
{
    public class OrderVisitService : IOrderVisitService
    {
        private readonly INotificationServiceHandler notificationServiceHandler;
        private readonly HoshiDbContext _context;
        public OrderVisitService(INotificationServiceHandler notificationServiceHandler, HoshiDbContext context)
        {
            this.notificationServiceHandler = notificationServiceHandler;
            this._context = context;
        }
        public async Task sendNoificationforclient(int orderId , string message)
        {
            var targetOrder = await _context.Orders.Where(p => p.Id == orderId).Include(p => p.ClientId).FirstOrDefaultAsync();
            await notificationServiceHandler.sendMessagetoClient(message, targetOrder.ClientId);
        }

        public async Task sendNoificationforclient2(int VisitId, string message)
        {
            var targetVisit = await _context.OrderVisits.FindAsync(VisitId);
            var targetOrder = await _context.Orders.FindAsync(targetVisit.OrderId);
            await notificationServiceHandler.sendMessagetoClient(message, targetOrder.ClientId);

        }
    }
}
