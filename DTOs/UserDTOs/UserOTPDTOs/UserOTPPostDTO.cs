using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.UserDTOs.UserOTPDTOs
{
    public class UserOTPPostDTO 
    {
        public required int Code { get; set; }
        public required bool IsRevoked { get; set; } = false;

        public required int UserId { get; set; }
    }
}
