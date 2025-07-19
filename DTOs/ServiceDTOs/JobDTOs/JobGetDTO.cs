using Hoshi.DTOs.ServiceDTOs.ServiceDTOs;
using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.ServiceDTOs.JobDTOs
{

    public class JobGetDTO : TimestampedModel, ISoftDelete
    {
        public string JobTitle { get; set; }
        public bool IsDeleted { get; set; }

        public List<ServiceGetDTO>? Services { get; set; }
    }
}
