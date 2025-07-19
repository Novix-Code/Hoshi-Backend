using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;
using Hoshi.DTOs.ServiceDTOs.ServiceDTOs;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.ServiceDTOs.ServiceCategoryDTOs
{

    public class ServiceCategoryPostDTO 
    {
        public required string ServiveName { get; set; }
        public required bool IsDeleted { get; set; }

        public List<ServicePostDTO>? Services { get; set; }
    }
}
