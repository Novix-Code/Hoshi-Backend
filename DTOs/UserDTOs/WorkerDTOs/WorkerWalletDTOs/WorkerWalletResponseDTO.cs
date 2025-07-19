using Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerWalletHistoryDTOs;

namespace Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerWalletDTOs
{
    public class WorkerWalletResponseDTO
    {
        public double Balance { get; set; }
        public List<WorkerWalletHistoryGetDTO> WalletHistory { get; set; } = new List<WorkerWalletHistoryGetDTO>(); 
    }
}
