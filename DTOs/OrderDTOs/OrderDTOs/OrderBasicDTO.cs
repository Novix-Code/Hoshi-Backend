using Hoshi.DTOs.OrderDTOs.OrderImageDTOs;
using Hoshi.DTOs.ServiceDTOs.ServiceDTOs;
using Hoshi.DTOs.UserDTOs.UserDTOs;

namespace Hoshi.DTOs.OrderDTOs.OrderDTOs
{
    public class OrderBasicDTO
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public double ProposalPrice { get; set; }
        public string OrderStatus { get; set; }

        public string Location { get; set; }
        public DateTime ServicingDateTime { get; set; }

        public UserBasicDTO? Client { get; set; }

        public ServiceBasicDTO? Service { get; set; }

        public List<OrderImageGetDTO>? OrderImages { get; set; }
    }
}
