using GenericCRUDLibrary.GenericModels;
using Hoshi.DTOs.GlobalDTOs.CityDTOs;
using Hoshi.DTOs.OrderDTOs.OrderImageDTOs;
using Hoshi.DTOs.ServiceDTOs.ServiceDTOs;

namespace Hoshi.DTOs.OrderDTOs.OrderDTOs
{
    public class OrderGetAllDto : TimestampedModel
    {
        public string Description { get; set; }
        public DateTime ServicingDateTime { get; set; }
        public string OrderStatus { get; set; }
        public ServiceBasicDTO? Service { get; set; }
        public CityGetDTO? City { get; set; }
        public List<OrderImageGetDTO>? OrderImages { get; set; }

    }
}
