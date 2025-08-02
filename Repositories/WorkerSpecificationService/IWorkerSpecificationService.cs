using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerSpecificationDTOs;

namespace Hoshi.Repositories.WorkerSpecificationService
{
    public interface IWorkerSpecificationService
    {
        Task<ResultDTO<WorkerSpecificationGetDTO>> GetWorkerById(int userId);
        Task<ResultDTO<WorkerSpecificationGetDTO>> UpdateWorker(WorkerSpecificationPutDTO putDTO);
        Task<Tuple<bool, string>> AddWorkerIdImg(int userId, IFormFile image, bool isUpdate);
        Task<Tuple<bool, string>> AddWorkerPorFiles(int userId, List<IFormFile> files, bool isUpdate);
    }
}
