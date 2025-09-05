using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerPaymentHistroyDTOs;

namespace Hoshi.Repositories.WorkerPaymentHistroyService
{
    /// <summary>
    /// Worker payment history operations: add and update payment records.
    /// </summary>
    public interface IWorkerPaymentHistroyService
    {
        /// <summary>
        /// Add a payment history with uploaded bill image.
        /// </summary>
        Task<ResultDTO<WorkerPaymentHistroyGetDTO>> AddService(WorkerPaymentHistroyPostDTO postDTO);
        /// <summary>
        /// Update a pending payment history (cannot modify once approved).
        /// </summary>
        Task<ResultDTO<WorkerPaymentHistroyGetDTO>> UpdateService(WorkerPaymentHistroyPutDTO putDTO);
    }
}
