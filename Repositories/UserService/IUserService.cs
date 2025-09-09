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
        /// Overview metrics (users, orders, income, complaints) for admin.
        /// </summary>
        Task<ResultDTO<object>> OverViewPage();
        /// <summary>
        /// Client page metrics and lists.
        /// </summary>
        Task<ResultDTO<object>> Clientpage();
        /// <summary>
        /// Client profile details and orders.
        /// </summary>
        Task<ResultDTO<object>> ClientDetails(int Id);
        /// <summary>
        /// Worker profile details.
        /// </summary>
        Task<ResultDTO<object>> WorkerDetails(int Id);
        /// <summary>
        /// Approve worker application and notify.
        /// </summary>
        Task<ResultDTO<object>> BeWorkerApproved(int Id);
        /// <summary>
        /// Reject worker application with reason and notify.
        /// </summary>
        Task<ResultDTO<object>> BeWorkerReject(int Id , string rejectResoun);
        /// <summary>
        /// Worker page metrics and lists.
        /// </summary>
        Task<ResultDTO<object>> WorkerPage();
        /// <summary>
        /// Dashboard worker details aggregation.
        /// </summary>
        Task<ResultDTO<object>> DashbordWorkerDetails(int id);
        /// <summary>
        /// Orders page metrics.
        /// </summary>
        Task<ResultDTO<Object>> OrderPage();
        /// <summary>
        /// Specific order details for admin view.
        /// </summary>
        Task<ResultDTO<Object>> OrderDetails(int id);
        /// <summary>
        /// List admins with their roles and permissions.
        /// </summary>
        Task<ResultDTO<List<AdminWithRolesAndPermissionsDTO>>> GetAllAdminsWithRolesAndPermissionsAsync();

        /// <summary>
        /// Suspend a user by reason if not already suspended.
        /// </summary>
        Task<ResultDTO<object>> SuspendUser(SuspendedUserPostDTO suspendDTO);
    }
}
