using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.GenericModels;
using Hoshi.Enums;

namespace Hoshi.DTOs.PromotionDTOs.PromotionDTOs
{
    public class PromotionPostDTO 
    {
        public required double Value { get; set; }
        public required string TitleFirstPart { get; set; }
        public required string TitleSecondPart { get; set; }
        public required string ImageURL { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public required bool IsPercentage { get; set; } = true;
        public required bool UntilBeUsed { get; set; } = true;
        public required bool IsDeleted { get; set; }

        public required PromotionFor PromotionFor { get; set; }
    }
}
