using GenericCRUDLibrary.GenericInterfaces;
using Hoshi.Models.UserModels;

namespace Hoshi.Models.UserModels.Resets
{
    public class PasswordResetRequest : IBaseModel
    {
        public int Id { get; set; }
        public string ResetToken { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ExpiresAt { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
