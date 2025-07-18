using Hoshi.DTOs.OrderDTOs.InvoiceDTOs;
using Hoshi.DTOs.OrderDTOs.OfferDTOs;
using Hoshi.DTOs.OrderDTOs.OrderVisitDTOs;
using Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerPortfolioDTOs;
using Hoshi.Enums;

namespace Hoshi.DTOs.OrderDTOs.OrderDTOs
{
    public class OrderGetDetailsDto
    {
        public OrderGetDTO OrderData { get; set; }
        public List<OfferGetDTO>? Offers { get; set; }
        public WorkerPortfolioGetDTO? WorkerData { get; set; }
        public List<OrderVisitGetDTO>? VisiteRequest { get; set; }
        public InvoiceGetDTO? ClientOrdeInvoice { get; set; }
    }
}
