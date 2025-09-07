using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.DTOs.OrderDTOs.OfferDTOs;

namespace Hoshi.Repositories.WorkerOfferService
{
    /// <summary>
    /// Worker offer lifecycle operations: create, confirm, cancel.
    /// </summary>
    public interface IWorkerOfferService
    {
        /// <summary>
        /// Create an offer and compute fees; upsert temp invoice.
        /// </summary>
        Task<ResultDTO<CreateOfferResponseDto>> CreateOfferAsync(OfferPostDTO dto);
        /// <summary>
        /// Confirm an offer, marking it ready for client decision.
        /// </summary>
        Task<ResultDTO<string>> ConfirmOfferAsync(int offerId);
        /// <summary>
        /// Cancel an offer, apply cancellation fee, and log wallet history.
        /// </summary>
        Task<ResultDTO<string>> CancelOfferAsync(int offerId);
    }
}
