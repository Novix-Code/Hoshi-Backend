using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.GenericModels;
using Hoshi.Enums;

namespace Hoshi.Models.PromotionModels
{
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

        public PromotionFor PromotionFor { get; set; } = PromotionFor.All;
    }
}
