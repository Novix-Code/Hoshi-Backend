using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.DTOs.ClientDTOs;
using Hoshi.DTOs.DashboardDTOs;
using Hoshi.DTOs.DashboardDTOs.ComplaintDTOs;
using Hoshi.DTOs.GlobalDTOs.ComplaintDTOs;
using Hoshi.DTOs.ServiceDTOs.ServiceCategoryDTOs;
using Hoshi.DTOs.ServiceDTOs.ServiceDTOs;

namespace Hoshi.Repositories.ServiceService
{
    public interface IServiceService
    {
        Task<ResultDTO<List<ServiceCategoryGetDTO>>> searchServiceAsyn(string serviceName);
        Task<ResultDTO<ServiceGetDTO>> AddService(ServicePostDTO postDTO);
        Task<ResultDTO<ServiceGetDTO>> UpdateService(ServicePutDTO putDTO);
        Task<ResultDTO<object>> GetServicesPageAsync();
        Task<ResultDTO<object>> GetPaymentsPageAsync();
        Task<ResultDTO<PaymentDetailsResponseDTO>> GetPaymentDetailsAsync(int paymentId);
        Task<ResultDTO<string>> AddWorkerPayment(int workerId, double paymentValue);
        Task<ResultDTO<ComplaintPageResponseDTO>> GetComplaintsPageAsync();
        Task<ResultDTO<ComplaintGetDTO>> GetComplaintDetailsAsync(int complaintId);
        Task<ResultDTO<MessageDTO>> ComplaintResponse(ComplaintResponseDTO complaintCreateDto);
        Task<ResultDTO<bool>> CloseComplaintAsync(int complaintId);
        Task<ResultDTO<StatisticPageResponseDTO>> GetStatisticPageAsync();
    }
}
