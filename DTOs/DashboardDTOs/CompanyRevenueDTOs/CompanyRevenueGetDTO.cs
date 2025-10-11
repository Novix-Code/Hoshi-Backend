using Hoshi.DTOs.OrderDTOs.OrderDTOs;
using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.DashboardDTOs.CompanyRevenueDTOs
{
    public class CompanyRevenueGetDTO : IBaseModel
    {
        public int Id { get; set; }
        public double Value { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public OrderGetDTO? Order { get; set; }
    }
}
