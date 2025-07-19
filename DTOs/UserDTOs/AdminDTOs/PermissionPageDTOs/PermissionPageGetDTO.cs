using Hoshi.DTOs.UserDTOs.AdminDTOs.PermissionDTOs;
using Hoshi.DTOs.UserDTOs.AdminDTOs.PageDTOs;
using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.UserDTOs.AdminDTOs.PermissionPageDTOs
{
    public class PermissionPageGetDTO : TimestampedModel
    {
        public PageGetDTO? Page { get; set; }

        public PermissionGetDTO? Permission { get; set; }
    }
}
