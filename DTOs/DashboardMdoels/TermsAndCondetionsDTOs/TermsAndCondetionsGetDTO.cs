using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.DashboardMdoels.TermsAndCondetionsDTOs
{
    public class TermsAndCondetionsGetDTO : TimestampedModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
