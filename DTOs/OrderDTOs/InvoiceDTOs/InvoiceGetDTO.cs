using Hoshi.DTOs.OrderDTOs.OrderVisitDTOs;
using Hoshi.DTOs.OrderDTOs.OrderDTOs;
using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.OrderDTOs.InvoiceDTOs
{
    public class InvoiceGetDTO : TimestampedModel
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

        public OrderGetDTO? Order { get; set; }

        public OrderVisitGetDTO? OrderVisit { get; set; }

    }
}
