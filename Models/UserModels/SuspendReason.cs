    using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.Models.UserModels
{
    [EndpointGroupping("Admin")]
    public class SuspendReason : TimestampedModel
    {
        public string Reason { get; set; } = string.Empty;
    }
}
