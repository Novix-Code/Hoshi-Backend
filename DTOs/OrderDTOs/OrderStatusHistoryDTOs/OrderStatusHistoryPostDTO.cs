using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.GenericModels;
using Hoshi.Enums;

namespace Hoshi.DTOs.OrderDTOs.OrderStatusHistoryDTOs
{
    public class OrderStatusHistoryPostDTO 
    {
        public required OrderStatus OrderStatus { get; set; }
        public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public required int OrderId { get; set; }
    }
}
