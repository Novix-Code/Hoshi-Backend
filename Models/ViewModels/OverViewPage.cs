using Hoshi.Models.GlobalModels;
using Hoshi.Models.OrderModels;

namespace Hoshi.Models.ViewModels
{

    public class OverViewPage
    {
        public int TotalUsers { get; set; }
        public int TotalClients { get; set; }
        public int TotalWorkers { get; set; }
        public int TotalOrders { get; set; }
        public int TotalCompletedOrders { get; set; }
        public decimal? TotalOrderIncome { get; set; }
        public decimal? TotalOrderPrice { get; set; }
    }
}
