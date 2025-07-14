using Hoshi.DTOs.ServiceDTOs.ServiceCategoryDTOs;
using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.ServiceDTOs.ServiceDTOs
{
    public class ServiceGetDTO : TimestampedModel, ISoftDelete
    {
        public string ServiveName { get; set; }
        public string ImageURL { get; set; }
        public bool IsDeleted { get; set; }

        public ServiceCategoryGetDTO? ServiceCategory { get; set; }

    }
}
