using Hoshi.DTOs.GlobalDTOs.CityDTOs;
using Hoshi.DTOs.OrderDTOs.OrderImageDTOs;
using Hoshi.DTOs.OrderDTOs.OrderStatusHistoryDTOs;
using Hoshi.DTOs.OrderDTOs.OrderVisitDTOs;
using Hoshi.DTOs.PromotionDTOs.PromotionDTOs;
using Hoshi.DTOs.ServiceDTOs.ServiceDTOs;
using Hoshi.DTOs.UserDTOs.UserDTOs;
using Hoshi.Enums;

namespace Hoshi.Models.ViewModels
{
    public class orderViewModelDetails
    {
        public string Description { get; set; }
        public double ProposalPrice { get; set; }

        public string Location { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime ServicingDateTime { get; set; }


        public double? TotalClientCost { get; set; }

        public double? TotalWorkerCost { get; set; }
        public int ClientId { get; set; }



    }
}
