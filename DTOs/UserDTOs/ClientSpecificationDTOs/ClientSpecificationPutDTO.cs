using GenericCRUDLibrary.GenericInterfaces;

namespace Hoshi.DTOs.UserDTOs.ClientSpecificationDTOs
{
    public class ClientSpecificationPutDTO  : IBaseModel
    {
		public int Id { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? Bio { get; set; }

        public IFormFile? Image { get; set; }
    }
}
