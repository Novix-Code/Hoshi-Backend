using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerWalletDTOs;

namespace Hoshi.Repositories.WorkerWalletService
{
    /// <summary>
    /// Contract for worker wallet operations: retrieval, payment submission, and balance adjustments.
    /// </summary>
    public interface IWorkerWalletService
    {
        /// <summary>
        /// Get wallet balance and history for a worker.
        /// </summary>
        Task<ResultDTO<WorkerWalletResponseDTO>> GetWorkerWalletAsync(int workerId);
        /// <summary>
        /// Submit a payment (bill) request for wallet top-up.
        /// </summary>
        Task<ResultDTO<string>> AddPaymentAsync(int workerId, AddPaymentRequestDTO request);
        /// <summary>
        /// Add funds to a worker's wallet and record as income.
        /// </summary>
        Task AddToWalletAsync(int workerId, double amount, string title);
        /// <summary>
        /// Deduct funds from a worker's wallet and record as expense.
        /// </summary>
        Task DeductFromWalletAsync(int workerId, double amount, string title);
    }
}
