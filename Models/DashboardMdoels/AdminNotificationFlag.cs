using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.Models.DashboardMdoels
{
    [ModelNotMapped]
    public class AdminNotificationFlag : TimestampedModel
    {
        public string FlagName { get; set; } = string.Empty;
        public bool FalgOn { get; set; } = false;
        public DateTime LastActivation { get; set; }
    }
}
