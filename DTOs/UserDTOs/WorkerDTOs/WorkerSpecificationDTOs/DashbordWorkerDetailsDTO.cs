using Hoshi.DTOs.OrderDTOs.OrderDTOs;

namespace Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerSpecificationDTOs
{
    public class DashbordWorkerDetailsDTO : WorkerSpecificationGetDTO
    {
        public double Balance { get; set; }
        public double TotalWorkerIncome { get; set; }
        public double TotalCommissionFee { get; set; }
        public List<OrderBasicDTO> Orders { get; set; } = new List<OrderBasicDTO>();
    }
}
