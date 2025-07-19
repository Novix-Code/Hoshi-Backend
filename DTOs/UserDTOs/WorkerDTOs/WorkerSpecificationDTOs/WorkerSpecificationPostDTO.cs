using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;
using Hoshi.DTOs.ServiceDTOs.ServiceDTOs;
using Hoshi.Models.ServiceModels;
using Microsoft.EntityFrameworkCore;

namespace Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerSpecificationDTOs
{
    public class WorkerSpecificationPostDTO 
    {
        public required string Bio { get; set; }
        public required string ImageURL { get; set; }
        public required string IdentityImageURL { get; set; }

        public required string Address { get; set; }
        public required double Latitude { get; set; }
        public required double Longitude { get; set; }

        public required int CompletedOrders { get; set; } = 0;
        public required double RateRito { get; set; } = 0.0;

        public required bool IsCompany { get; set; } = false;
        public bool? IsApproved { get; set; }

        public required int UserId { get; set; }

        public required int LivingCityId { get; set; }

        public required int JobId { get; set; }

        public List<ServicePostDTO>? Services { get; set; }
    }
}
