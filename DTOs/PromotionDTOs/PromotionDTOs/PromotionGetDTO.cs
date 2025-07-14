using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.GenericModels;
using Hoshi.Enums;

namespace Hoshi.DTOs.PromotionDTOs.PromotionDTOs
{
    public class PromotionGetDTO : TimestampedModel, ISoftDelete
    {
        public double Value { get; set; }
        public string TitleFirstPart { get; set; }
        public string TitleSecondPart { get; set; }
        public string ImageURL { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public bool IsPercentage { get; set; } = true;
        public bool UntilBeUsed { get; set; } = true;
        public bool IsDeleted { get; set; }

        public PromotionFor PromotionFor { get; set; }
    }
}
