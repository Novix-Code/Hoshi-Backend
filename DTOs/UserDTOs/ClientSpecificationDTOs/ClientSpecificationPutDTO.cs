using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;
using Microsoft.EntityFrameworkCore;

namespace Hoshi.DTOs.UserDTOs.ClientSpecificationDTOs
{
    public class ClientSpecificationPutDTO  : IBaseModel
    {
		public int Id { get; set; }
        public string? ImageURL { get; set; }
        public string? Address { get; set; }
        public string? Bio { get; set; }
        public int? CompletedOrders { get; set; } = 0;
        public double? RateRito { get; set; } = 0.0;
        public double? Balance { get; set; } = 0.0;
        public double? Indebtedness { get; set; } = 0.0;

        public int? UserId { get; set; }

        public int? LivingCityId { get; set; }
    }
}
