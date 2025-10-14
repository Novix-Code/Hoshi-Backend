using GenericCRUDLibrary.GenericInterfaces;

namespace Hoshi.Models.ViewModels.PaymentsPageViews
{
    public class PaymentRequestsView : IBaseModel
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? ImageURL { get; set; }
        public string? JobTitle { get; set; }
        public string? CityName { get; set; }
        public int? RequestId { get; set; }
        public double? Balance { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
