using GenericCRUDLibrary.GenericInterfaces;
using Hoshi.Enums;

namespace Hoshi.DTOs.PromotionDTOs.PromotionDTOs
{
    public class PromotionPutDTO  : IBaseModel
    {
		public int Id { get; set; }
        public double? Value { get; set; }
        public string? TitleFirstPart { get; set; }
        public string? TitleSecondPart { get; set; }
        public IFormFile? Image { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public bool? IsPercentage { get; set; } = true;
        public bool? UntilBeUsed { get; set; } = true;

        public PromotionFor? PromotionFor { get; set; }
    }
}
