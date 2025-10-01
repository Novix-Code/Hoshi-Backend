using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.DTOs.DashboardDTOs.ComplaintDTOs;
using Hoshi.DTOs.DashboardDTOs;
using Hoshi.DTOs.GlobalDTOs.ComplaintDTOs;
using Hoshi.DTOs.UserDTOs.AdminDTOs.UserPermissionDTOs;

namespace Hoshi.Repositories.AdminDashboardService
{
    public interface IAdminDashboardService
    {
        /// <summary>
        /// Overview metrics (users, orders, income, complaints) for admin.
        /// </summary>
        Task<ResultDTO<object>> OverViewPage();

        // ---------------------
        // Client Page Endpoints
        // ---------------------

        /// <summary>
        /// Client page metrics and lists.
        /// </summary>
        Task<ResultDTO<object>> Clientpage();

        /// <summary>
        /// Client profile details and orders.
        /// </summary>
        Task<ResultDTO<object>> ClientDetails(int Id);

        // ---------------------
        // Worker Page Endpoints
        // ---------------------

        /// <summary>
        /// Worker page metrics and lists.
        /// </summary>
        Task<ResultDTO<object>> WorkerPage();

        /// <summary>
        /// Worker profile details.
        /// </summary>
        Task<ResultDTO<object>> BeWorkerRequest(int Id);

        /// <summary>
        /// Approve worker application and notify.
        /// </summary>
        Task<ResultDTO<object>> BeWorkerApproval(int Id);

        /// <summary>
        /// Reject worker application with reason and notify.
        /// </summary>
        Task<ResultDTO<object>> BeWorkerRejection(int Id, string rejectResoun);

        /// <summary>
        /// Dashboard worker details aggregation.
        /// </summary>
        Task<ResultDTO<object>> DashbordWorkerDetails(int id);

        // ---------------------
        // Order Page Endpoints
        // ---------------------

        /// <summary>
        /// Orders page metrics.
        /// </summary>
        Task<ResultDTO<Object>> OrderPage();

        /// <summary>
        /// Specific order details for admin view.
        /// </summary>
        Task<ResultDTO<Object>> OrderDetails(int id);

        /// <summary>
        /// Build services analytics page aggregates.
        /// </summary>
        Task<ResultDTO<object>> GetServicesPageAsync();

        // ---------------------
        // Payment Page Endpoints
        // ---------------------

        /// <summary>
        /// Payments dashboard aggregates and lists.
        /// </summary>
        Task<ResultDTO<object>> GetPaymentsPageAsync();

        /// <summary>
        /// Payment details for a submitted payment request.
        /// </summary>
        Task<ResultDTO<PaymentDetailsResponseDTO>> GetPaymentDetailsAsync(int paymentId);

        /// <summary>
        /// Top up worker wallet balance and record history.
        /// </summary>
        Task<ResultDTO<string>> AddWorkerPayment(int workerId, int requestId, double paymentValue);

        // ---------------------
        // Complaints Page Endpoints
        // ---------------------

        /// <summary>
        /// Complaints page summary and listing.
        /// </summary>
        Task<ResultDTO<ComplaintPageResponseDTO>> GetComplaintsPageAsync();

        /// <summary>
        /// Complaint details including order data.
        /// </summary>
        Task<ResultDTO<ComplaintGetDTO>> GetComplaintDetailsAsync(int complaintId);

        /// <summary>
        /// Update complaint with a response.
        /// </summary>
        Task<ResultDTO<MessageDTO>> ComplaintResponse(ComplaintResponseDTO complaintCreateDto);



        /// <summary>
        /// Statistics aggregates for dashboard.
        /// </summary>
        Task<ResultDTO<StatisticPageResponseDTO>> GetStatisticPageAsync();

        /// <summary>
        /// List admins with their roles and permissions.
        /// </summary>
        Task<ResultDTO<List<AdminWithRolesAndPermissionsDTO>>> GetAllAdminsWithRolesAndPermissionsAsync();
    }
}
