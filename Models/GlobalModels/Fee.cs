using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;
using Hoshi.Enums;
using Hoshi.Models.ServiceModels;

namespace Hoshi.Models.GlobalModels
{
    [NoAction(ControllerAction.Delete)]
    public class Fee : TimestampedModel
    {
        public double MainFees { get; set; }
        public double MaxFees { get; set; }
        public double MinFees { get; set; }

        [PropNotMapped(DtoType.Get, exceptInThisDTO: true)]

        public FeeType? FeeType { get; set; }

        public bool IsSpecial => ServiceId is not null;

        public int? ServiceId { get; set; }
        public Service? Service { get; set; }
    }
}
