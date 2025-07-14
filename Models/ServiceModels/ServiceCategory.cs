using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.Models.ServiceModels
{

    [UseFSPController]
    [NoAction(ControllerAction.Pagination)]
    [NoAction(ControllerAction.FilterPagination)]
    public class ServiceCategory : TimestampedModel, ISoftDelete
    {
        public string ServiveName { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }

        public List<Service>? Services { get; set; }
    }
}
