using Hoshi.DTOs.ServiceDTOs.ServiceDTOs;
using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;
using Hoshi.Enums;

namespace Hoshi.DTOs.GlobalDTOs.FeeDTOs
{
    public class FeeGetDTO : TimestampedModel
    {
        public double MainFees { get; set; }
        public double MaxFees { get; set; }
        public double MinFees { get; set; }

        public FeeType? FeeType { get; set; }

        public bool IsSpecial { get; set; }

        public ServiceGetDTO? Service { get; set; }
    }
}
