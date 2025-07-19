using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.OrderDTOs.InvoiceDTOs
{
    public class InvoicePostDTO 
    {
        public required double OrderPrice { get; set; } = 0.0;
        public required double CommissionFee { get; set; } = 0.0;
        public required double VisitingFee { get; set; } = 0.0;
        public required double CancellationFee { get; set; } = 0.0;
        public required double WorkerPromotionFee { get; set; } = 0.0;
        public required double ClientPromotionFee { get; set; } = 0.0;
        public required double ClientIndebtednessFee { get; set; } = 0.0;
        public required double ClientTotalPrice { get; set; } = 0.0;
        public required double WorkerTotalPrice { get; set; } = 0.0;

        public required int OrderId { get; set; }

        public int? OrderVisitId { get; set; }

    }
}
