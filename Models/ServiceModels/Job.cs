using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.Models.ServiceModels
{

    [EndpointGroupping("Admin", ControllerAction.Add)]
    [EndpointGroupping("Admin", ControllerAction.AddList)]
    [EndpointGroupping("Admin", ControllerAction.Update)]
    [EndpointGroupping("Admin", ControllerAction.Delete)]
    [EndpointGroupping("Admin", ControllerAction.Restore)]
    public class Job : TimestampedModel, ISoftDelete
    {
        public string JobTitle { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }

        public List<Service>? Services { get; set; } = new List<Service>();
    }
}
