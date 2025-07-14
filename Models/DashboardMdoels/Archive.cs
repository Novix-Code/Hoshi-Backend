using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;

namespace Hoshi.Models.DashboardMdoels
{
    [EndpointGroupping("Admin")]
    public class Archive : IBaseModel, ISoftDelete
    {
        public int Id { get; set; }
        public string FileURL { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }
    }
}
