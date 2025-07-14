using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.GenericModels;
using Hoshi.Enums;

namespace Hoshi.Models.OrderModels
{
    [UseJustFSPController]
    [NoAction(ControllerAction.Pagination)]
    [NoAction(ControllerAction.FilterPagination)]
    public class OrderStatusHistory : IBaseModel
    {
        public int Id { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int OrderId { get; set; }
        public Order? Order { get; set; }
    }
}
