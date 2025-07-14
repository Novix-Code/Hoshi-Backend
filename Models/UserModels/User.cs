using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;
using Hoshi.Enums;
using Microsoft.AspNetCore.Identity;

namespace Hoshi.Models.UserModels
{
    [UseFSPController]
    public class User : IdentityUser<int>, IBaseModel, ISoftDelete
    {
        public string UserCode { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }

        public string UserType { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? ModifiedAt { get; set; }
    }
}
