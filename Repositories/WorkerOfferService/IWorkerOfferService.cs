using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.DTOs.OrderDTOs.OfferDTOs;

namespace Hoshi.Repositories.WorkerOfferService
{
    public interface IWorkerOfferService
    {
        Task<ResultDTO<CreateOfferResponseDto>> CreateOfferAsync(OfferPostDTO dto);
        Task<ResultDTO<string>> ConfirmOfferAsync(int offerId);
        Task<ResultDTO<string>> CancelOfferAsync(int offerId);
    }
}
