using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.DashboardMdoels.CompanyRevenueDTOs
{
    public class CompanyRevenuePostDTO 
    {
        public required double Value { get; set; }
        public required DateTime CreatedAt { get; set; } = DateTime.Now;

        public required int OrderId { get; set; }
    }
}
