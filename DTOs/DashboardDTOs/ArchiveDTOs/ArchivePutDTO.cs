using GenericCRUDLibrary.GenericInterfaces;

namespace Hoshi.DTOs.DashboardDTOs.ArchiveDTOs
{
    public class ArchivePutDTO  : IBaseModel
    {
        public int Id { get; set; }
        public IFormFile? File { get; set; }
    }
}
