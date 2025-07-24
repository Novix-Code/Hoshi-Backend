using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerWalletDTOs;

namespace Hoshi.Repositories.WorkerWalletService
{
    public interface IWorkerWalletService
    {
        Task<ResultDTO<WorkerWalletResponseDTO>> GetWorkerWalletAsync(int workerId);
        Task<ResultDTO<string>> AddPaymentAsync(int workerId, AddPaymentRequestDTO request);
    }
}
