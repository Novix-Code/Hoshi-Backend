using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.OrderDTOs.InvoiceDTOs
{
    public class InvoicePutDTO  : IBaseModel
    {
		public int Id { get; set; }
        public double? OrderPrice { get; set; } = 0.0;
        public double? CommissionFee { get; set; } = 0.0;
        public double? VisitingFee { get; set; } = 0.0;
        public double? CancellationFee { get; set; } = 0.0;
        public double? WorkerPromotionFee { get; set; } = 0.0;
        public double? ClientPromotionFee { get; set; } = 0.0;
        public double? ClientIndebtednessFee { get; set; } = 0.0;
        public double? ClientTotalPrice { get; set; } = 0.0;
        public double? WorkerTotalPrice { get; set; } = 0.0;

        public int? OrderId { get; set; }

        public int? OrderVisitId { get; set; }

    }
}
