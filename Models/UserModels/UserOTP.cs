using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.Models.UserModels
{
    [NoController]
    public class UserOTP : TimestampedModel
    {
        public string Code { get; set; }
        public byte[] SecreteKey { get; set; }
        public bool IsRevoked { get; set; } = false;

        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
