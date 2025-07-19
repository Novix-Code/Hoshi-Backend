using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.Models.ServiceModels
{
    [UseFSPController]
    [NoAction(ControllerAction.Pagination)]
    [NoAction(ControllerAction.FilterPagination)]
    [EndpointGroupping("Admin", ControllerAction.Add)]
    [EndpointGroupping("Admin", ControllerAction.AddList)]
    [EndpointGroupping("Admin", ControllerAction.Update)]
    [EndpointGroupping("Admin", ControllerAction.Delete)]
    [EndpointGroupping("Admin", ControllerAction.Restore)]
    [CreateRepoPattern("ClientHomeService")]
    public class Service : TimestampedModel, ISoftDelete
    {
        public string ServiveName { get; set; } = string.Empty;
        public string ImageURL { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }

        public int ServiceCategoryId { get; set; }
        public ServiceCategory? ServiceCategory { get; set; }

    }
}
