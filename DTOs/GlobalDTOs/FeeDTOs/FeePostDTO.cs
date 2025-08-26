using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;
using Hoshi.Enums;

namespace Hoshi.DTOs.GlobalDTOs.FeeDTOs
{
    public class FeePostDTO 
    {
        public required double MainFees { get; set; }
        public required double MaxFees { get; set; }
        public required double MinFees { get; set; }

        public required FeeType FeeType { get; set; }

        public bool IsSpecial { get; set; } = false;

        public int? ServiceId { get; set; }
    }
}
