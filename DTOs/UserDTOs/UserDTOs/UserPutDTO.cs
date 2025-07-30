using GenericCRUDLibrary.GenericInterfaces;

namespace Hoshi.DTOs.UserDTOs.UserDTOs
{
    public class UserPutDTO : IBaseModel
    {
        public int Id { get; set; }
        public string? UserCode { get; set; }
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        public string? UserType { get; set; }
    }
}
