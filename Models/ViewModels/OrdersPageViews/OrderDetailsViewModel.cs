using GenericCRUDLibrary.GenericInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Hoshi.Models.ViewModels.OrdersPageViews
{
    [Keyless]
    public class OrderDetailsViewModel : IBaseModel
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public double ProposalPrice { get; set; }
        public DateTime ServicingDateTime { get; set; }
        public string OrderStatus { get; set; }

        public string CityName { get; set; }
        public string ServiveName { get; set; }

        public int ClientId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string? ImageURL { get; set; }

    }
}
