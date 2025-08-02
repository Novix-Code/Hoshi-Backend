using GenericCRUDLibrary.GenericInterfaces;

namespace Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerSpecificationDTOs
{
    public class WorkerSpecificationPutDTO  : IBaseModel
    {
		public int Id { get; set; }
        public string? Bio { get; set; }
        public bool? IsCompany { get; set; }
        public string? Address { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public int? JobId { get; set; }
        public int? LivingCityId { get; set; }
        public List<int>? ServicesIds { get; set; }

        public IFormFile? PersonalImage { get; set; }
        public List<IFormFile>? PortfolioFiles { get; set; }
    }
}
