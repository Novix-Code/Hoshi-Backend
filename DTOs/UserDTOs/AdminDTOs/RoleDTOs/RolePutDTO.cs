using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;
using Microsoft.AspNetCore.Identity;

namespace Hoshi.DTOs.UserDTOs.AdminDTOs.RoleDTOs
{
    public class RolePutDTO  : IdentityRole<int>, IBaseModel
    {
        public bool? IsDeleted { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedAt { get; set; }
    }
}
