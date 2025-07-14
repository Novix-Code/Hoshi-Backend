using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.GlobalDTOs.CityDTOs
{
    public class CityPostDTO 
    {
        public required string CityName { get; set; }
        public required string CityCode { get; set; }

        public required double Latitude { get; set; }
        public required double Longitude { get; set; }
    }
}
