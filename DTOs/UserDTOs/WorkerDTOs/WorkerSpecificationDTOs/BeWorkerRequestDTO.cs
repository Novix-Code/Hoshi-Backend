namespace Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerSpecificationDTOs
{
    public class BeWorkerRequestDTO
    {
        public required int UserId { get; set; }
        public required string Bio { get; set; }
        public bool IsCompany { get; set; } = false;
        public required string Address { get; set; }
        public required double Latitude { get; set; }
        public required double Longitude { get; set; }

        public required int JobId { get; set; }
        public required int CityId { get; set; }
        public required List<int> ServicesIds { get; set; }

        public required IFormFile PersonalImage { get; set; }
        public required IFormFile IdentityImage { get; set; }
        public List<IFormFile>? PortfolioFiles { get; set; }
    }
}