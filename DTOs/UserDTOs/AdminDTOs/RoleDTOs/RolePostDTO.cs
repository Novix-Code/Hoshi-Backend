using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;
using Microsoft.AspNetCore.Identity;

namespace Hoshi.DTOs.UserDTOs.AdminDTOs.RoleDTOs
{
    public class RolePostDTO 
    {
        public required bool IsDeleted { get; set; }

        public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedAt { get; set; }
    }
}
