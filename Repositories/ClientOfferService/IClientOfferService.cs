using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;

namespace Hoshi.Repositories.ClientOfferService
{
    public interface IClientOfferService
    {
        Task<ResultDTO<Object>> GetByIdAsync(int id);
        Task<ResultDTO<Object>> AcceptOfferAsync(int id);
    }
}
