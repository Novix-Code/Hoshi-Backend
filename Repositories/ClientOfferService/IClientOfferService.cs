using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;

namespace Hoshi.Repositories.ClientOfferService
{
    /// <summary>
    /// Client-side offer operations: view details and accept offer.
    /// </summary>
    public interface IClientOfferService
    {
        /// <summary>
        /// Get worker-facing details for an offer.
        /// </summary>
        Task<ResultDTO<Object>> GetOfferDetailsByIdAsync(int offerId);
        /// <summary>
        /// Accept an offer, assign order to worker, and initialize invoice/promotions.
        /// </summary>
        Task<ResultDTO<Object>> AcceptOfferAsync(int offerId);
    }
}
