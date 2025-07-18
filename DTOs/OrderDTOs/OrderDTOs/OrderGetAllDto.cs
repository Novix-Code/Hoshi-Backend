using Hoshi.Enums;
using Hoshi.Models.ServiceModels;

namespace Hoshi.DTOs.OrderDTOs.OrderDTOs
{
    public class OrderGetAllDto
    {
        public int OrderId { get; set; }
        public string OrderDescription { get; set; }
        public DateTime CurrentDateTime { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public Service Service { get; set; }
    }
}
