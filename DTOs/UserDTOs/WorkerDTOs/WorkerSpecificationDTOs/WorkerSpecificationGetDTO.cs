using GenericCRUDLibrary.GenericModels;
using Hoshi.DTOs.GlobalDTOs.CityDTOs;
using Hoshi.DTOs.ServiceDTOs.JobDTOs;
using Hoshi.DTOs.ServiceDTOs.ServiceDTOs;
using Hoshi.DTOs.UserDTOs.UserDTOs;
using Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerPortfolioDTOs;

namespace Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerSpecificationDTOs
{
    public class WorkerSpecificationGetDTO : TimestampedModel
    {
        public string Bio { get; set; } = string.Empty;
        public string IdentityImageURL { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public int CompletedOrders { get; set; } = 0;
        public double RateRito { get; set; } = 0.0;

        public bool IsCompany { get; set; }
        public bool? IsApproved { get; set; }

        public UserGetDTO? User { get; set; }

        public CityGetDTO? LivingCity { get; set; }

        public JobGetDTO? Job { get; set; }

        public List<ServiceBasicDTO>? Services { get; set; }
        public List<WorkerPortfolioBasicDTO>? Portfolios { get; set; }
    }
}
