using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.DTOs.OrderDTOs.OfferDTOs;

namespace Hoshi.Repositories.WorkerOfferService
{
    public interface IWorkerOfferService
    {
        Task<ResultDTO<CreateOfferResponseDto>> CreateOfferAsync(OfferPostDTO dto);
        Task<ResultDTO<bool>> ConfirmOfferAsync(int offerId);
        Task<ResultDTO<bool>> CancelOfferAsync(int offerId);
    }
}
