using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.ServiceDTOs.JobServiceDTOs
{
    public class JobServicePostDTO 
    {
        public required int JobId { get; set; }

        public required int ServiceId { get; set; }
    }
}
