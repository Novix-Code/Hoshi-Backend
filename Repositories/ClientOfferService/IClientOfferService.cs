using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;

namespace Hoshi.Repositories.ClientOfferService
{
    public interface IClientOfferService
    {
        Task<ResultDTO<Object>> GetOfferDetailsByIdAsync(int offerId);
        Task<ResultDTO<Object>> AcceptOfferAsync(int offerId);
    }
}
