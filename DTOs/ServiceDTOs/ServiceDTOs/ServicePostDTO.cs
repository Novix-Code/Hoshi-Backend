using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.ServiceDTOs.ServiceDTOs
{
    public class ServicePostDTO 
    {
        public required string ServiveName { get; set; }
        public required string ImageURL { get; set; }
        public required bool IsDeleted { get; set; }

        public required int ServiceCategoryId { get; set; }

    }
}
