using Hoshi.DTOs.DashboardDTOs.ComplaintDTOs;
using Hoshi.Repositories.AdminDashboardService;
using Hoshi.Repositories.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hoshi.Controllers.DashboardControllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "SuperAdmin, Admin")]

    public class AdminDashboardController : ControllerBase
    {
        private readonly IAdminDashboardService dashboardService;

        public AdminDashboardController(IAdminDashboardService adminPagesService)
        {
            this.dashboardService = adminPagesService;
        }

        [HttpGet("OverViewPage")]
        public async Task<IActionResult> overView()
        {
            var reponse = await dashboardService.OverViewPage();
            return StatusCode((int)Response.StatusCode, reponse);
        }

        [HttpGet("get-services-page")]
        public async Task<IActionResult> GetServicesPageAsync()
        {
            var response = await dashboardService.GetServicesPageAsync();
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet("get-payments-page")]
        public async Task<IActionResult> GetPaymentsPageAsync()
        {
            var response = await dashboardService.GetPaymentsPageAsync();
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet("get-payment-details")]
        public async Task<IActionResult> GetPaymentDetailsAsync([FromQuery] int paymentId)
        {
            var response = await dashboardService.GetPaymentDetailsAsync(paymentId);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPatch("add-payment")]
        public async Task<IActionResult> AddWorkerPayment(int workerId, int requestId, double paymentValue)
        {
            var response = await dashboardService.AddWorkerPayment(workerId, requestId, paymentValue);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet("get-complaints-page")]
        public async Task<IActionResult> GetComplaintsPageAsync()
        {
            var response = await dashboardService.GetComplaintsPageAsync();
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet("get-complaint-details")]
        public async Task<IActionResult> GetComplaintDetailsAsync([FromQuery] int complaintId)
        {
            var response = await dashboardService.GetComplaintDetailsAsync(complaintId);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPatch("complaint-response")]
        public async Task<IActionResult> ComplaintResponse(ComplaintResponseDTO complaintCreateDto)
        {
            var response = await dashboardService.ComplaintResponse(complaintCreateDto);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPatch("close-complaint")]
        public async Task<IActionResult> CloseComplaintAsync([FromQuery] int complaintId)
        {
            var result = await dashboardService.CloseComplaintAsync(complaintId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("get-statistic-page")]
        public async Task<IActionResult> GetStatisticPageAsync()
        {
            var response = await dashboardService.GetStatisticPageAsync();
            return StatusCode((int)response.StatusCode, response);
        }


        [HttpGet("ClientPage")]
        public async Task<IActionResult> clientpage()
        {
            var response = await dashboardService.Clientpage();
            return StatusCode((int)Response.StatusCode, response);
        }

        [HttpGet("ClientDetails{id}")]
        public async Task<IActionResult> clientDetails(int id)
        {
            var response = await dashboardService.ClientDetails(id);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet("WorkerPage")]
        public async Task<IActionResult> workerPage()
        {
            var response = await dashboardService.WorkerPage();
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet("BeWorkerRequest{id}")]
        public async Task<IActionResult> beWorkerReq(int id)
        {
            var response = await dashboardService.WorkerDetails(id);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost("BeWorkerApproved{id}")]
        public async Task<IActionResult> beworkerapproved(int id)
        {
            var resonse = await dashboardService.BeWorkerApproved(id);
            return StatusCode((int)resonse.StatusCode, resonse);
        }

        [HttpPost("BeWorkerRejected")]
        public async Task<IActionResult> beworkerreject(int id, string RejectResoun)
        {
            var resonse = await dashboardService.BeWorkerReject(id, RejectResoun);
            return StatusCode((int)resonse.StatusCode, resonse);
        }

        [HttpGet("DashbordWorkerDetails{id}")]
        public async Task<IActionResult> dashWOrker(int id)
        {
            var response = await dashboardService.DashbordWorkerDetails(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("OrdersPage")]
        public async Task<IActionResult> ordrsDetails()
        {
            var response = await dashboardService.OrderPage();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("DashbordOrderDetails{id}")]
        public async Task<IActionResult> dashorderDetls(int id)
        {
            var response = await dashboardService.OrderDetails(id);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Get admins along with their roles and permissions.
        /// </summary>
        [HttpGet("get-all-admins-with-roles-and-permissions")]
        public async Task<IActionResult> GetAllAdminsWithRolesAndPermissionsAsync()
        {
            var response = await dashboardService.GetAllAdminsWithRolesAndPermissionsAsync();
            return StatusCode(response.StatusCode, response);
        }
    }
}
