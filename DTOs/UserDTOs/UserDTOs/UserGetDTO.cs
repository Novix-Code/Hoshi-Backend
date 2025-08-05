using GenericCRUDLibrary.GenericInterfaces;

namespace Hoshi.DTOs.UserDTOs.UserDTOs
{
    public class UserGetDTO : IBaseModel, ISoftDelete
    {
        public int Id { get; set; }
        public string? UserCode { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? ImageURL { get; set; }

        public bool IsDeleted { get; set; }

        public string UserType { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
