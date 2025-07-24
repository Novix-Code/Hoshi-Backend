using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.DashboardDTOs.CompanyRevenueDTOs
{
    public class CompanyRevenuePutDTO  : IBaseModel
    {
        public int Id { get; set; }
        public double? Value { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        public int? OrderId { get; set; }
    }
}
