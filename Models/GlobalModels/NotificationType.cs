using GenericCRUDLibrary.GenericModels;

namespace Hoshi.Models.GlobalModels
{
    public class NotificationType: TimestampedModel
    {
        public string Title { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public bool ForClient { get; set; }
    }
}
