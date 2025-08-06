using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;
using Hoshi.DTOs.ServiceDTOs.ServiceDTOs;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.ServiceDTOs.ServiceCategoryDTOs
{

    public class ServiceCategoryPostDTO 
    {
        public required string CategoryName { get; set; } = string.Empty;
    }
}
