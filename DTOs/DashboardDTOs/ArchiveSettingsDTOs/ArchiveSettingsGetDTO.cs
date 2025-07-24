using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.DashboardDTOs.ArchiveSettingsDTOs
{
    public class ArchiveSettingsGetDTO : TimestampedModel
    {
        public int AutoSaveDuration = 30;
        public bool AllData = true;
        public bool TotalOrders = false;
        public bool TotalOrderValue = false;
        public bool OrderFrequency = false;
        public bool TotalCustomers = false;
        public bool TotalRevenue = false;
        public bool CustomerGrowthRates = false;
        public bool ServiceRequestRates = false;
        public bool OrderCompletionRates = false;
        public bool ComplaintFilingRates = false;
        public bool ProblemResolutionRates = false;
        public bool RevenueGrowthRates = false;
    }
}
