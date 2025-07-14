using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.ServiceDTOs.ServiceDTOs
{
    public class ServicePutDTO  : IBaseModel
    {
		public int Id { get; set; }
        public string? ServiveName { get; set; }
        public string? ImageURL { get; set; }
        public bool? IsDeleted { get; set; }

        public int? ServiceCategoryId { get; set; }

    }
}
