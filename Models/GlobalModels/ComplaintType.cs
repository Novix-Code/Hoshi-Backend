using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.Models.GlobalModels
{
    [UseFSPController]
    [EndpointGroupping("Admin")]
    public class ComplaintType : TimestampedModel
    {
        public bool ForClient { get; set; }
        public string Type { get; set; } = string.Empty;
    }
}
