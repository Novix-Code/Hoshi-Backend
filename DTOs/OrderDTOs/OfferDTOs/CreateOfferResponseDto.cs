namespace Hoshi.DTOs.OrderDTOs.OfferDTOs
{
    public class CreateOfferResponseDto
    {
        public int OfferId { get; set; }
        public int OrderId { get; set; }
        public double OfferedPrice { get; set; }
        public double VisitFee { get; set; }
        public double CancellationFee { get; set; }
        public double ServiceFee { get; set; }
        public string? PromotionTitle { get; set; }
        public double? PromotionValue { get; set; }
        public double WorkerRevenue { get; set; }
        public double ClientWillPay { get; set; }
    }
}
