using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.Models.DashboardModels
{
    [EndpointGroupping("Admin")]
    public class TermsAndCondetions : TimestampedModel
    {
        public bool ForClient { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }
}
