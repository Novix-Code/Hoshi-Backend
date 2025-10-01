using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.DTOs.UserDTOs.AdminDTOs.UserPermissionDTOs;
using Hoshi.DTOs.UserDTOs.SuspendedUserDTOs;

namespace Hoshi.Repositories.UserService
{
    /// <summary>
    /// User-related operations for admin dashboards and profile utilities.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Save or update user's personal image; returns success flag and path or error.
        /// </summary>
        Task<Tuple<bool, string>> AddUserImage(int id, IFormFile image, bool isUpdate);

        /// <summary>
        /// Suspend a user by reason if not already suspended.
        /// </summary>
        Task<ResultDTO<object>> SuspendUser(SuspendedUserPostDTO suspendDTO);
    }
}
