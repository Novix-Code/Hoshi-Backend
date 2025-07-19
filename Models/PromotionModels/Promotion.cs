using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.GenericModels;
using Hoshi.Enums;

namespace Hoshi.Models.PromotionModels
{
    [EndpointGroupping("Admin", ControllerAction.Add)]
    [EndpointGroupping("Admin", ControllerAction.AddList)]
    [EndpointGroupping("Admin", ControllerAction.Update)]
    [EndpointGroupping("Admin", ControllerAction.Delete)]
    [EndpointGroupping("Admin", ControllerAction.Restore)]
    public class Promotion : TimestampedModel, ISoftDelete
    {
        public double Value { get; set; }
        public string TitleFirstPart { get; set; } = string.Empty;
        public string TitleSecondPart { get; set; } = string.Empty;
        public string ImageURL { get; set; } = string.Empty;

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public bool IsPercentage { get; set; } = true;
        public bool UntilBeUsed { get; set; } = true;
        public bool IsDeleted { get; set; }

        public PromotionFor PromotionFor { get; set; }
    }
}
