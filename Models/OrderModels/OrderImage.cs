using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;

namespace Hoshi.Models.OrderModels
{
    [EndpointGroupping("Client", ControllerAction.Add)]
    [EndpointGroupping("Client", ControllerAction.Update)]
    public class OrderImage : IBaseModel, ISoftDelete
    {
        public int Id { get; set; }
        public string ImageURL { get; set; } = string.Empty;
        public int ImageNumber { get; set; }
        public bool IsDeleted { get; set; }

        public int OrderId { get; set; }
        public Order? Order { get; set; }

}
}
