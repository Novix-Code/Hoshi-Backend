using GenericCRUDLibrary.GenericModels;
using Hoshi.Models.GlobalModels;
using Hoshi.Models.OrderModels;
using Hoshi.Models.ServiceModels;

namespace Hoshi.DTOs.OrderDTOs.OrderDTOs
{
    public class OrderGetAllDto : TimestampedModel
    {
        public string Description { get; set; }
        public DateTime ServicingDateTime { get; set; }
        public string OrderStatus { get; set; }
        public Service? Service { get; set; }
        public City? City { get; set; }
        public List<OrderImage>? OrderImages { get; set; }

    }
}
