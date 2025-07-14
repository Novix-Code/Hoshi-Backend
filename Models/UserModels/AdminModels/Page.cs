using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.Models.UserModels.AdminModels
{
    public class Page : TimestampedModel, ISoftDelete
    {
        public string PageName { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }

        public int ParentPageId { get; set; }
        public Page? ParentPage { get; set; }
    }
}
