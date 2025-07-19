using Hoshi.DTOs.ServiceDTOs.ServiceDTOs;
using Hoshi.DTOs.ServiceDTOs.JobDTOs;
using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.ServiceDTOs.JobServiceDTOs
{
    public class JobServiceGetDTO : TimestampedModel
    {
        public JobGetDTO? Job { get; set; }

        public ServiceGetDTO? Service { get; set; }
    }
}
