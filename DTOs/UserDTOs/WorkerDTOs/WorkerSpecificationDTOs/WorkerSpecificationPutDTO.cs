using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;
using Hoshi.Models.ServiceModels;
using Microsoft.EntityFrameworkCore;

namespace Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerSpecificationDTOs
{
    public class WorkerSpecificationPutDTO  : IBaseModel
    {
		public int Id { get; set; }
        public string? Bio { get; set; }
        public string? ImageURL { get; set; }
        public string? IdentityImageURL { get; set; }

        public string? Address { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public int? CompletedOrders { get; set; } = 0;
        public double? RateRito { get; set; } = 0.0;

        public bool? IsCompany { get; set; } = false;
        public bool? IsApproved { get; set; }

        public int? UserId { get; set; }

        public int? LivingCityId { get; set; }

        public int? JobId { get; set; }

    }
}
