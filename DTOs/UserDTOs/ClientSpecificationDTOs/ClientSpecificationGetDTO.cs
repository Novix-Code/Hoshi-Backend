using Hoshi.DTOs.GlobalDTOs.CityDTOs;
using Hoshi.DTOs.UserDTOs.UserDTOs;
using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;
using Microsoft.EntityFrameworkCore;

namespace Hoshi.DTOs.UserDTOs.ClientSpecificationDTOs
{
    public class ClientSpecificationGetDTO : TimestampedModel
    {
        public string ImageURL { get; set; }
        public string Address { get; set; }
        public string Bio { get; set; }
        public int CompletedOrders { get; set; } = 0;
        public double RateRito { get; set; } = 0.0;
        public double Balance { get; set; } = 0.0;
        public double Indebtedness { get; set; } = 0.0;

        public UserGetDTO? User { get; set; }

        public int LivingCityId { get; set; }
        public CityGetDTO? LivingCity { get; set; }
    }
}
