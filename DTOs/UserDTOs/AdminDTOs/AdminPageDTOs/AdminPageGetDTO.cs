using Hoshi.DTOs.UserDTOs.AdminDTOs.PageDTOs;
using Hoshi.DTOs.UserDTOs.UserDTOs;
using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.UserDTOs.AdminDTOs.AdminPageDTOs
{
    public class AdminPageGetDTO : TimestampedModel
    {
        public UserGetDTO? User { get; set; }

        public PageGetDTO? Page { get; set; }
    }
}
