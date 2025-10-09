using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.GenericModels;
using Hoshi.Enums;

namespace Hoshi.DTOs.OrderDTOs.OrderStatusHistoryDTOs
{
    public class OrderStatusHistoryPutDTO  : IBaseModel
    {
        public int Id { get; set; }
        public OrderStatus? OrderStatus { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

        public int? OrderId { get; set; }
    }
}
