using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.DTOs.ClientDTOs;
using Hoshi.DTOs.DashboardDTOs;
using Hoshi.DTOs.DashboardDTOs.ComplaintDTOs;
using Hoshi.DTOs.GlobalDTOs.ComplaintDTOs;
using Hoshi.DTOs.ServiceDTOs.ServiceCategoryDTOs;
using Hoshi.DTOs.ServiceDTOs.ServiceDTOs;

namespace Hoshi.Repositories.ServiceService
{
    /// <summary>
    /// Service-related operations: creation/update with image handling, search, and dashboards.
    /// </summary>
    public interface IServiceService
    {
        /// <summary>
        /// Search services by name and group under categories.
        /// </summary>
        Task<ResultDTO<List<ServiceCategoryGetDTO>>> searchServiceAsyn(string serviceName);
        /// <summary>
        /// Create a new service with image upload.
        /// </summary>
        Task<ResultDTO<ServiceGetDTO>> AddService(ServicePostDTO postDTO);
        /// <summary>
        /// Update a service and optionally its image.
        /// </summary>
        Task<ResultDTO<ServiceGetDTO>> UpdateService(ServicePutDTO putDTO);
        /// <summary>
        /// Build services analytics page aggregates.
        /// </summary>
        Task<ResultDTO<object>> GetServicesPageAsync();
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
        Task<ResultDTO<string>> AddWorkerPayment(int workerId, double paymentValue);
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
        /// Close a complaint (set status to solved).
        /// </summary>
        Task<ResultDTO<bool>> CloseComplaintAsync(int complaintId);
        /// <summary>
        /// Statistics aggregates for dashboard.
        /// </summary>
        Task<ResultDTO<StatisticPageResponseDTO>> GetStatisticPageAsync();
    }
}
