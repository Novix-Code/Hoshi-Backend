using GenericCRUDLibrary.CustomAttributes;

namespace Hoshi.Models.DashboardMdoels
{
    [ModelNotMapped]
    public class AdminNotificationFlag
    {
        public string FlagName { get; set; } = string.Empty;
        public bool FalgOn { get; set; } = false;
        public DateTime LastActivation { get; set; }
    }
}
