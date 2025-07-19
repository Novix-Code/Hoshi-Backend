using Microsoft.AspNetCore.Http;

namespace Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerWalletDTOs
{
    public class AddPaymentRequestDTO
    {
        public required IFormFile BillImage { get; set; }
    }
}
