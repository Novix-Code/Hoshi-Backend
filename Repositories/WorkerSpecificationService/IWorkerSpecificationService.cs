using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerSpecificationDTOs;

namespace Hoshi.Repositories.WorkerSpecificationService
{
    /// <summary>
    /// Worker specification operations: read/update profile, id image, portfolio files.
    /// </summary>
    public interface IWorkerSpecificationService
    {
        /// <summary>
        /// Get worker specification by user id including services and portfolios.
        /// </summary>
        Task<ResultDTO<WorkerSpecificationGetDTO>> GetWorkerById(int userId);
        /// <summary>
        /// Update worker specification including services and portfolio.
        /// </summary>
        Task<ResultDTO<WorkerSpecificationGetDTO>> UpdateWorker(WorkerSpecificationPutDTO putDTO);
        /// <summary>
        /// Add or replace national ID image for a worker.
        /// </summary>
        Task<Tuple<bool, string>> AddWorkerIdImg(int userId, IFormFile image, bool isUpdate);
        /// <summary>
        /// Add or replace worker portfolio files.
        /// </summary>
        Task<Tuple<bool, string>> AddWorkerPorFiles(int userId, List<IFormFile> files, bool isUpdate);
    }
}
