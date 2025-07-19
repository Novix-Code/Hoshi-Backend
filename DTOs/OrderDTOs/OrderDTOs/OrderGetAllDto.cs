using Hoshi.Enums;
using Hoshi.Models.ServiceModels;

namespace Hoshi.DTOs.OrderDTOs.OrderDTOs
{
    public class OrderGetAllDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime ServicingDateTime { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public Service Service { get; set; }

    }
}
