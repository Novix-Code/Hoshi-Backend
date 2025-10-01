using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.DashboardDTOs.TermsAndCondetionsDTOs
{
    public class TermsAndCondetionsPostDTO
    {
        public bool ForClient { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
    }
}
