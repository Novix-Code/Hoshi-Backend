using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.DashboardMdoels.TermsAndCondetionsDTOs
{
    public class TermsAndCondetionsPostDTO 
    {
        public required string Title { get; set; }
        public required string Content { get; set; }
    }
}
