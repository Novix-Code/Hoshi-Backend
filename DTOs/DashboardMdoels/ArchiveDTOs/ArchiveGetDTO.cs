using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;
using System.ComponentModel.DataAnnotations;

namespace Hoshi.DTOs.DashboardMdoels.ArchiveDTOs
{
    public class ArchiveGetDTO : IBaseModel, ISoftDelete
    {
        public int Id { get; set; }
        public string FileURL { get; set; }
        public bool IsDeleted { get; set; }
    }
}
