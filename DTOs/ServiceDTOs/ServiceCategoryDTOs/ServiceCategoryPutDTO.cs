using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.ServiceDTOs.ServiceCategoryDTOs
{

    public class ServiceCategoryPutDTO  : IBaseModel
    {
		public int Id { get; set; }
        public string? CategoryName { get; set; }
        public bool? IsDeleted { get; set; }

    }
}
