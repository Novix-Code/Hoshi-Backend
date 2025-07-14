using GenericCRUDLibrary.GenericModels;

namespace Hoshi.Models.UserModels
{
    public class SuspendReason : TimestampedModel
    {
        public string Reason { get; set; } = string.Empty;
    }
}
