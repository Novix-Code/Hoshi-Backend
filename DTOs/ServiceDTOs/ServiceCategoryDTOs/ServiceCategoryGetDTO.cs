using Hoshi.DTOs.ServiceDTOs.ServiceDTOs;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.ServiceDTOs.ServiceCategoryDTOs
{

    public class ServiceCategoryGetDTO : TimestampedModel
    {
        public string CategoryName { get; set; }
        public bool IsDeleted { get; set; }

        public List<ServiceBasicDTO>? Services { get; set; }
    }
}
