using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;
using Hoshi.DTOs.ServiceDTOs.ServiceDTOs;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.ServiceDTOs.JobDTOs
{

    public class JobPostDTO 
    {
        public required string JobTitle { get; set; }
        public required bool IsDeleted { get; set; }

        public List<ServicePostDTO>? Services { get; set; }
    }
}
