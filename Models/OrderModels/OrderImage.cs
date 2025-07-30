using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;

namespace Hoshi.Models.OrderModels
{
    [NoAction(ControllerAction.Update)]
    [EndpointGroupping("Client", ControllerAction.Add)]
    public class OrderImage : IBaseModel, ISoftDelete
    {
        public int Id { get; set; }
        public string ImageURL { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }

        public int OrderId { get; set; }
        public Order? Order { get; set; }

}
}
