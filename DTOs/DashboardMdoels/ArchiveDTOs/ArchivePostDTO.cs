using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;
using System.ComponentModel.DataAnnotations;

namespace Hoshi.DTOs.DashboardMdoels.ArchiveDTOs
{
    public class ArchivePostDTO 
    {
        public required string FileURL { get; set; }
        public required bool IsDeleted { get; set; }
    }
}
