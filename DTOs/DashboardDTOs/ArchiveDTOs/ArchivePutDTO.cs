using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;
using System.ComponentModel.DataAnnotations;

namespace Hoshi.DTOs.DashboardDTOs.ArchiveDTOs
{
    public class ArchivePutDTO  : IBaseModel
    {
        public int Id { get; set; }
        public string? FileURL { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
