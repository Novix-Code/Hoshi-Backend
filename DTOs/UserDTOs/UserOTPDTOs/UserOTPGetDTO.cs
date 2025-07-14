using Hoshi.DTOs.UserDTOs.UserDTOs;
using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.UserDTOs.UserOTPDTOs
{
    public class UserOTPGetDTO : TimestampedModel
    {
        public int Code { get; set; }
        public bool IsRevoked { get; set; } = false;

        public UserGetDTO? User { get; set; }
    }
}
