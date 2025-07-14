using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.Models.UserModels.AdminModels
{
    [UseFSPController]
    public class Permission : TimestampedModel, ISoftDelete
    {
        public string PermissionName { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }
    }
}
