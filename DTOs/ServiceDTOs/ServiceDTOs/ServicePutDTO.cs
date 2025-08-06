using GenericCRUDLibrary.GenericInterfaces;

namespace Hoshi.DTOs.ServiceDTOs.ServiceDTOs
{
    public class ServicePutDTO  : IBaseModel
    {
		public int Id { get; set; }
        public string? ServiveName { get; set; }
        public IFormFile? Image { get; set; }

        public int? ServiceCategoryId { get; set; }

    }
}
