using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.Models.GlobalModels
{
    [NoAction(ControllerAction.Delete)]
    [EndpointGroupping("Admin", ControllerAction.Add)]
    [EndpointGroupping("Admin", ControllerAction.AddList)]
    [EndpointGroupping("Admin", ControllerAction.Update)]
    public class City : TimestampedModel
    {
        public string CityName { get; set; } = string.Empty;
        public string CityCode { get; set; } = string.Empty;

        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
