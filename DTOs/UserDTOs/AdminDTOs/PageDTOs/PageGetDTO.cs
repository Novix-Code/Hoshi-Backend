using Hoshi.DTOs.UserDTOs.AdminDTOs.PageDTOs;
using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.UserDTOs.AdminDTOs.PageDTOs
{
    public class PageGetDTO : TimestampedModel, ISoftDelete
    {
        public string PageName { get; set; }
        public bool IsDeleted { get; set; }

        public int ParentPageId { get; set; }
        public PageGetDTO? ParentPage { get; set; }
    }
}
