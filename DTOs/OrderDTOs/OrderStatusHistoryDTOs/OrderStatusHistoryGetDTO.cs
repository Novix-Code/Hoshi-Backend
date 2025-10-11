using Hoshi.DTOs.OrderDTOs.OrderDTOs;
using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.GenericModels;
using Hoshi.Enums;

namespace Hoshi.DTOs.OrderDTOs.OrderStatusHistoryDTOs
{
    public class OrderStatusHistoryGetDTO : IBaseModel
    {
        public int Id { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public OrderGetDTO? Order { get; set; }
    }
}
