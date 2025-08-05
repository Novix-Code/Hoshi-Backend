using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.ServiceDTOs.ServiceCategoryDTOs
{
    public class ServiceCategoryBasicDTO : TimestampedModel
    {
        public string CategoryName { get; set; }
        public bool IsDeleted { get; set; }
    }
}
