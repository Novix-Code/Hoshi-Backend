using GenericCRUDLibrary.GenericInterfaces;

namespace Hoshi.DTOs.ServiceDTOs.ServiceCategoryDTOs
{

    public class ServiceCategoryPutDTO  : IBaseModel
    {
		public int Id { get; set; }
        public string? CategoryName { get; set; }
    }
}
