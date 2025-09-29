namespace Hoshi.Models.ViewModels.OrdersPageViews
{
    public class OrdersPageViewModel
    {
        public int TotalOrders { get; set; }
        public int TotalActiveOrders { get; set; }
        public int TotalCompletedOrders { get; set; }
        public int TotalCancelledOrders { get; set; }
    }
}
