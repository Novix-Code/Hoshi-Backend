using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerPaymentHistroyDTOs;

namespace Hoshi.Repositories.WorkerPaymentHistroyService
{
    public interface IWorkerPaymentHistroyService
    {
        Task<ResultDTO<WorkerPaymentHistroyGetDTO>> AddService(WorkerPaymentHistroyPostDTO postDTO);
        Task<ResultDTO<WorkerPaymentHistroyGetDTO>> UpdateService(WorkerPaymentHistroyPutDTO putDTO);
    }
}
