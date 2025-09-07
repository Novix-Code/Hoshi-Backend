using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.DTOs.UserDTOs.ClientSpecificationDTOs;
using Microsoft.AspNetCore.Mvc;

namespace Hoshi.Repositories.ClientSpecificationService
{
    /// <summary>
    /// Client specifications operations for profile retrieval and updates (including user fields).
    /// </summary>
    public interface IClientSpecificationService
    {
        /// <summary>
        /// Get client specification by user id.
        /// </summary>
        Task<ResultDTO<ClientSpecificationGetDTO>> GetById(int userId);
        /// <summary>
        /// Update client specification and related user properties; may update image via user service.
        /// </summary>
        Task<ResultDTO<ClientSpecificationGetDTO>> Update(ClientSpecificationPutDTO putDTO);
    }
}
