using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;
using Microsoft.AspNetCore.Identity;

namespace Hoshi.DTOs.UserDTOs.AdminDTOs.RoleDTOs
{
    public class RoleGetDTO : IdentityRole<int>, IBaseModel, ISoftDelete
    {
        public bool IsDeleted { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? ModifiedAt { get; set; }
    }
}
