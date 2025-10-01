using GenericCRUDLibrary.GenericInterfaces;
using Hoshi.DTOs.UserDTOs.UserDTOs;
using System.ComponentModel.DataAnnotations;

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

        [RegularExpression(
            @"^\+?\d{1,3}?[-\s]?(\(\d{1,4}\)|\d{1,4})?[-\s]?\d{3,4}[-\s]?\d{3,4}$",
            ErrorMessage = "صيغة رقم الهاتف غير صحيحة."
        )]
        [DataType(DataType.PhoneNumber)]
        public string? PhoneNumber { get; set; } = string.Empty;

        public int? JobId { get; set; }
        public int? LivingCityId { get; set; }
        public List<int>? ServicesIds { get; set; }

        public IFormFile? PersonalImage { get; set; }
        public List<IFormFile>? PortfolioFiles { get; set; }
    }
}
