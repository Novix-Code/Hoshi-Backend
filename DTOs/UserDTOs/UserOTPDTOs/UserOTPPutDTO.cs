using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.UserDTOs.UserOTPDTOs
{
    public class UserOTPPutDTO  : IBaseModel
    {
		public int Id { get; set; }
        public int? Code { get; set; }
        public bool? IsRevoked { get; set; } = false;

        public int? UserId { get; set; }
    }
}
