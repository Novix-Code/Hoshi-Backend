using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;
using System.ComponentModel.DataAnnotations;

namespace Hoshi.Models.DashboardMdoels
{
    [EndpointGroupping("Admin")]
    public class Archive : IBaseModel, ISoftDelete
    {
        [Key]
        public int Id { get; set; }
        public string FileURL { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }
    }
}
