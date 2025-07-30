using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;
using Hoshi.DTOs.OrderDTOs.OrderImageDTOs;
using Hoshi.Enums;
using Hoshi.Models.PromotionModels;
using Hoshi.Models.UserModels;

namespace Hoshi.DTOs.OrderDTOs.OrderDTOs
{
    public class OrderPostDTO 
    {
        public required string Description { get; set; }
        public required double ProposalPrice { get; set; }

        public required string Location { get; set; }
        public required double Latitude { get; set; }
        public required double Longitude { get; set; }
        public required DateTime ServicingDateTime { get; set; }

        public required int ClientId { get; set; }

        public required int CityId { get; set; }

        public required int ServiceId { get; set; }

        //public int? AppliedPromotionId { get; set; }

        public List<IFormFile>? OrderImagesFiles { get; set; }
    }
}
