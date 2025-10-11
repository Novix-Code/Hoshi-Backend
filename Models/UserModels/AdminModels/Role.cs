using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;
using Microsoft.AspNetCore.Identity;

namespace Hoshi.Models.UserModels.AdminModels
{
    [UseFSPController]
    [EndpointGroupping("Admin")]
    public class Role : IdentityRole<int>, IBaseModel, ISoftDelete
    {
        public bool IsDeleted { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedAt { get; set; }
    }
}
