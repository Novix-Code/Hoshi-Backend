using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.DashboardDTOs.ArchiveSettingsDTOs
{
    public class ArchiveSettingsPostDTO 
    {
        public required int AutoSaveDuration = 30;
        public required bool AllData = true;
        public required bool TotalOrders = false;
        public required bool TotalOrderValue = false;
        public required bool OrderFrequency = false;
        public required bool TotalCustomers = false;
        public required bool TotalRevenue = false;
        public required bool CustomerGrowthRates = false;
        public required bool ServiceRequestRates = false;
        public required bool OrderCompletionRates = false;
        public required bool ComplaintFilingRates = false;
        public required bool ProblemResolutionRates = false;
        public required bool RevenueGrowthRates = false;
    }
}
