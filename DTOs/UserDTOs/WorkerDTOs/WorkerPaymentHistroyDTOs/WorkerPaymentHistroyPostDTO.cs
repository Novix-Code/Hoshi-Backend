namespace Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerPaymentHistroyDTOs
{

    public class WorkerPaymentHistroyPostDTO 
    {
        public required IFormFile BillImage { get; set; }

        public required int WorkerId { get; set; }
    }
}
