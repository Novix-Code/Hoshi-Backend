using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;
using Hoshi.Enums;
using Microsoft.AspNetCore.Identity;

namespace Hoshi.DTOs.UserDTOs.UserDTOs
{
    public class UserPutDTO  : IdentityUser<int>, IBaseModel
    {
        public string? UserCode { get; set; }
        public bool? IsDeleted { get; set; }

        public string? UserType { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public DateTime? ModifiedAt { get; set; }
    }
}
