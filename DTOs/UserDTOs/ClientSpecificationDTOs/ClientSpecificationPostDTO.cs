using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;
using Microsoft.EntityFrameworkCore;

namespace Hoshi.DTOs.UserDTOs.ClientSpecificationDTOs
{
    public class ClientSpecificationPostDTO 
    {
        public required string ImageURL { get; set; }
        public required string Address { get; set; }
        public required string Bio { get; set; }
        public required int CompletedOrders { get; set; } = 0;
        public required double RateRito { get; set; } = 0.0;
        public required double Balance { get; set; } = 0.0;
        public required double Indebtedness { get; set; } = 0.0;

        public required int UserId { get; set; }

        public required int LivingCityId { get; set; }
    }
}
