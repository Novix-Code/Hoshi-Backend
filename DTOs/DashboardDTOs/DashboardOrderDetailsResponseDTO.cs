using Hoshi.DTOs.GlobalDTOs.CityDTOs;
using Hoshi.DTOs.OrderDTOs.OrderImageDTOs;
using Hoshi.DTOs.OrderDTOs.OrderStatusHistoryDTOs;
using Hoshi.DTOs.ServiceDTOs.JobDTOs;
using Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerHomeDTOs;

namespace Hoshi.DTOs.DashboardDTOs
{
    public class DashboardOrderDetailsResponseDTO
    {
        public int OrderId { get; set; }
        public ClientDataDto ClientData { get; set; }
        public JobGetDTO Job { get; set; }
        public string Description { get; set; }
        public List<OrderStatusHistoryGetDTO>? OrderStatus { get; set; }
        public CityGetDTO City { get; set; }
        public string Location { get; set; }
        public DateTime ServicingDatetime { get; set; }
        public double OfferedPrice { get; set; }
        public List<OrderImageGetDTO> OrderImages { get; set; }
        public WorkerDataDTO WorkerData { get; set; }
    }
    
    public class WorkerDataDTO
    {
        public int WorkerId { get; set; }
        public string ImageUrl { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public JobGetDTO Job { get; set; }
        public bool IsCompany { get; set; }
        public double RateRatio { get; set; }
        public int CompletedOrders { get; set; }
        public int CancelledOffers { get; set; }
        public double Balance { get; set; }
    }
    
    public class PaymentDetailsResponseDTO
    {
        public string BillImageUrl { get; set; }
        public WorkerDataDTO Worker { get; set; }
    }
}
