using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.GlobalDTOs.CityDTOs
{
    public class CityGetDTO : TimestampedModel
    {
        public string CityName { get; set; }
        public string CityCode { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
