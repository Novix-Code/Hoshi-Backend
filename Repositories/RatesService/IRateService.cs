using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.DTOs.GlobalDTOs.RateDTOs;

namespace Hoshi.Repositories.RatesService
{
    public interface IRateService
    {
        Task<ResultDTO<object>> AddRateForClient(RatePostDTO dto);
        Task<ResultDTO<object>> AddRateForWorker(RatePostDTO dto);
    }
}
