using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.Models.DashboardMdoels
{
    [EndpointGroupping("Admin")]
    public class TermsAndCondetions : TimestampedModel
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }
}
