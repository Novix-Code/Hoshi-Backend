using GenericCRUDLibrary.GenericModels;

namespace Hoshi.Models.OrderModels
{
    public class TempInvoice : TimestampedModel
    {
        public double OrderPrice { get; set; } = 0.0;
        public double CommissionFee { get; set; } = 0.0;
        public double VisitingFee { get; set; } = 0.0;
        public double CancellationFee { get; set; } = 0.0;
        public double WorkerPromotionFee { get; set; } = 0.0;
        public double ClientPromotionFee { get; set; } = 0.0;
        public double ClientIndebtednessFee { get; set; } = 0.0;
        public double ClientTotalPrice { get; set; } = 0.0;
        public double WorkerTotalPrice { get; set; } = 0.0;

        public int? OfferId { get; set; }
        public Offer? Offer { get; set; }

        public int? OrderVisitId { get; set; }
        public OrderVisit? OrderVisit { get; set; }
    }
}
