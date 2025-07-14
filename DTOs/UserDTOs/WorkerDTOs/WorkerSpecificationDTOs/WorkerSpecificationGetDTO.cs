using Hoshi.DTOs.ServiceDTOs.ServiceDTOs;
using Hoshi.DTOs.ServiceDTOs.JobDTOs;
using Hoshi.DTOs.GlobalDTOs.CityDTOs;
using Hoshi.DTOs.UserDTOs.UserDTOs;
using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;
using Hoshi.Models.ServiceModels;
using Microsoft.EntityFrameworkCore;

namespace Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerSpecificationDTOs
{
    public class WorkerSpecificationGetDTO : TimestampedModel
    {
        public string Bio { get; set; }
        public string ImageURL { get; set; }
        public string IdentityImageURL { get; set; }

        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public int CompletedOrders { get; set; } = 0;
        public double RateRito { get; set; } = 0.0;

        public bool IsCompany { get; set; } = false;
        public bool? IsApproved { get; set; }

        public UserGetDTO? User { get; set; }

        public int LivingCityId { get; set; }
        public CityGetDTO? LivingCity { get; set; }

        public JobGetDTO? Job { get; set; }

        public List<ServiceGetDTO>? Services { get; set; }
    }
}
