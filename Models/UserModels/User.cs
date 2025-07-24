using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;
using Hoshi.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Hoshi.Models.UserModels
{
    [UseFSPController]
    [EndpointGroupping("Admin")]
    [CreateRepoPattern("AuthService")]
    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(PhoneNumber), IsUnique = true)]
    public class User : IdentityUser<int>, IBaseModel, ISoftDelete
    {
        public string FullName { get; set; } = string.Empty;
        public string? UserCode { get; set; }
        public bool IsDeleted { get; set; }

        public string UserType { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? ModifiedAt { get; set; }
    }
}
