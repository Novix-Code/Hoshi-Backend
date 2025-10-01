using GenericCRUDLibrary.GenericInterfaces;

namespace Hoshi.Models.ViewModels.ClientsPageViews
{
    public class AllClientModelForView : IBaseModel
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? ImageURL { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public double RateRito { get; set; }
        public int CompletedOrders { get; set; }
        public int CancellationNumber { get; set; }
        public double Balance { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
