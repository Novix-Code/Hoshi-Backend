namespace Hoshi.DTOs.UserDTOs.UserDTOs
{
    public class UserBasicDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? ImageURL { get; set; }

        public string UserType { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}
