using GenericCRUDLibrary.GenericInterfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hoshi.Models.ViewModels.PaymentsPageViews
{
    [Keyless]
    public class UncollectedFeesView : IBaseModel
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? ImageURL { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public double RateRito { get; set; }
        public int CompletedOrders { get; set; }
        public double Balance { get; set; }
        public int TotalCancelledOrders { get; set; }
        public bool HasCollectionAlert { get; set; }
    }
}
