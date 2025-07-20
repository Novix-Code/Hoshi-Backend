using Hoshi.DTOs.ServiceDTOs.ServiceDTOs;
using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.ServiceDTOs.ServiceCategoryDTOs
{

    public class ServiceCategoryGetDTO : TimestampedModel, ISoftDelete
    {
        public string CategoryName { get; set; }
        public bool IsDeleted { get; set; }

        public List<ServiceGetDTO>? Services { get; set; }
    }
}
