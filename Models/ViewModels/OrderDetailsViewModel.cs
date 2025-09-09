using Hoshi.Enums;

namespace Hoshi.Models.ViewModels
{
    public class OrderDetailsViewModel
    {
        public int Id { get; set; }
        public int CityId { get; set; }

        public string Description { get; set; }
        public double ProposalPrice { get; set; }

        public string Location { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime ServicingDateTime { get; set; }


        public double? TotalClientCost { get; set; }

        public double? TotalWorkerCost { get; set; }
        public int ClientId { get; set; }
        public int? WorkerId { get; set; }
        public OrderStatus OrderStatus { get; set; }



    }
}
