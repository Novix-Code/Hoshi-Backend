using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.DashboardDTOs.TermsAndCondetionsDTOs
{
    public class TermsAndCondetionsGetDTO : TimestampedModel
    {
        public bool ForClient { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
