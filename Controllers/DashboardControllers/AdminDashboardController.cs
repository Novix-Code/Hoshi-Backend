using AutoMapper;
using GenericCRUDLibrary.GenericDTOs.InputsDTOs;
using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.GenericRepositories.GenericFSPService;
using Hoshi.Data;
using Hoshi.DTOs.DashboardDTOs;
using Hoshi.DTOs.DashboardDTOs.ComplaintDTOs;
using Hoshi.Models.ViewModels.ClientsPageViews;
using Hoshi.Models.ViewModels.OrdersPageViews;
using Hoshi.Models.ViewModels.PaymentsPageViews;
using Hoshi.Models.ViewModels.WorkersPageViews;
using Hoshi.Repositories.AdminDashboardService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Hoshi.Controllers.DashboardControllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "SuperAdmin, Admin")]
    [EndpointGroupName("Admin")]
    public class AdminDashboardController : ControllerBase
    {
        private readonly HoshiDbContext context;
        private readonly IMapper mapper;
        private readonly IAdminDashboardService dashboardService;

        public AdminDashboardController(
            HoshiDbContext context,
            IMapper mapper,
            IAdminDashboardService adminPagesService
        )
        {
            this.context = context;
            this.mapper = mapper;
            this.dashboardService = adminPagesService;
        }

        [HttpGet("OverViewPage")]
        public async Task<IActionResult> OverView()
        {
            var reponse = await dashboardService.OverViewPage();
            return StatusCode((int)Response.StatusCode, reponse);
        }

        // ---------------------
        // Client Page Endpoints
        // ---------------------

        [HttpGet("ClientPage")]
        public async Task<IActionResult> Clientpage()
        {
            var response = await dashboardService.Clientpage();
            return StatusCode((int)Response.StatusCode, response);
        }

        [HttpPatch("NewClientsTable")]
        public IActionResult NewClientsTable(TablesFSPDTO dto) => TableFSP<NewClientViewModel>(dto);

        [HttpPatch("AllClientsTable")]
        public IActionResult AllClientsTable(TablesFSPDTO dto) => TableFSP<AllClientModelForView>(dto);

        [HttpPatch("SuspendedClientsTable")]
        public IActionResult SuspendedClientsTable(TablesFSPDTO dto) => TableFSP<SuspendedUserModelForView>(dto);

        [HttpGet("ClientDetails")]
        public async Task<IActionResult> ClientDetails(int id)
        {
            var response = await dashboardService.ClientDetails(id);
            return StatusCode((int)response.StatusCode, response);
        }

        // ---------------------
        // Worker Page Endpoints
        // ---------------------

        [HttpGet("WorkerPage")]
        public async Task<IActionResult> WorkerPage()
        {
            var response = await dashboardService.WorkerPage();
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPatch("NewWorkersTable")]
        public IActionResult NewWorkersTable(TablesFSPDTO dto) => TableFSP<NewWorkerModelForView>(dto);

        [HttpPatch("AllWorkersTable")]
        public IActionResult AllWorkersTable(TablesFSPDTO dto) => TableFSP<AllWorkersModelForView>(dto);

        [HttpPatch("SuspendedWorkersTable")]
        public IActionResult SuspendedWorkersTable(TablesFSPDTO dto) => TableFSP<SuspendedWorkerModelForView>(dto);

        [HttpGet("BeWorkerRequest")]
        public async Task<IActionResult> BeWorkerRequest(int id)
        {
            var response = await dashboardService.BeWorkerRequest(id);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost("BeWorkerApproved")]
        public async Task<IActionResult> BeWorkerApproved(int id)
        {
            var resonse = await dashboardService.BeWorkerApproval(id);
            return StatusCode((int)resonse.StatusCode, resonse);
        }

        [HttpPost("BeWorkerRejected")]
        public async Task<IActionResult> BeWorkerReject(int id, string RejectResoun)
        {
            var resonse = await dashboardService.BeWorkerRejection(id, RejectResoun);
            return StatusCode((int)resonse.StatusCode, resonse);
        }

        [HttpGet("DashbordWorkerDetails")]
        public async Task<IActionResult> DashbordWorkerDetails(int id)
        {
            var response = await dashboardService.DashbordWorkerDetails(id);
            return StatusCode(response.StatusCode, response);
        }

        // ---------------------
        // Order Page Endpoints
        // ---------------------

        [HttpGet("OrdersPage")]
        public async Task<IActionResult> OrdersPage()
        {
            var response = await dashboardService.OrderPage();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("DashboardOrderDetails")]
        public async Task<IActionResult> DashboardOrderDetails(int id)
        {
            var response = await dashboardService.OrderDetails(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPatch("ActiveOrdersTable")]
        public IActionResult ActiveOrdersTable(TablesFSPDTO dto) => TableFSP<ActiveOrdersViewModel>(dto);

        [HttpPatch("FinishedOrdersTable")]
        public IActionResult FinishedOrdersTable(TablesFSPDTO dto) => TableFSP<FinishedOrdersViewModel>(dto);

        [HttpGet("ServicesPage")]
        public async Task<IActionResult> ServicesPageAsync()
        {
            var response = await dashboardService.GetServicesPageAsync();
            return StatusCode((int)response.StatusCode, response);
        }

        // ---------------------
        // Payment Page Endpoints
        // ---------------------

        [HttpGet("PaymentsPage")]
        public async Task<IActionResult> PaymentsPageAsync()
        {
            var response = await dashboardService.GetPaymentsPageAsync();
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPatch("PaymentRequestsTable")]
        public IActionResult PaymentRequestsTable(TablesFSPDTO dto) => TableFSP<PaymentRequestsView>(dto);

        [HttpPatch("WorkerUncollectedFeesTable")]
        public IActionResult WorkerUncollectedFeesTable(TablesFSPDTO dto) => TableFSP<WorkerUncollectedFeesView>(dto);

        [HttpPatch("ClientUncollectedFeesTable")]
        public IActionResult ClientUncollectedFeesTable(TablesFSPDTO dto) => TableFSP<ClientUncollectedFeesView>(dto);

        [HttpGet("PaymentDetails")]
        public async Task<IActionResult> PaymentDetailsAsync([FromQuery] int paymentId)
        {
            var response = await dashboardService.GetPaymentDetailsAsync(paymentId);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPatch("AddWorkerPayment")]
        public async Task<IActionResult> AddWorkerPayment(int workerId, int requestId, double paymentValue)
        {
            var response = await dashboardService.AddWorkerPayment(workerId, requestId, paymentValue);
            return StatusCode((int)response.StatusCode, response);
        }

        // ---------------------
        // Complaints Page Endpoints
        // ---------------------

        [HttpGet("ComplaintsPage")]
        public async Task<IActionResult> ComplaintsPageAsync()
        {
            var response = await dashboardService.GetComplaintsPageAsync();
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet("ComplaintDetails")]
        public async Task<IActionResult> ComplaintDetailsAsync([FromQuery] int complaintId)
        {
            var response = await dashboardService.GetComplaintDetailsAsync(complaintId);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPatch("ComplaintResponse")]
        public async Task<IActionResult> ComplaintResponse(ComplaintResponseDTO complaintCreateDto)
        {
            var response = await dashboardService.ComplaintResponse(complaintCreateDto);
            return StatusCode((int)response.StatusCode, response);
        }



        [NonAction]
        [HttpGet("StatisticPage")]
        public async Task<IActionResult> StatisticPageAsync()
        {
            var response = await dashboardService.GetStatisticPageAsync();
            return StatusCode((int)response.StatusCode, response);
        }

        [NonAction]
        [HttpGet("AllAdmins")]
        public async Task<IActionResult> AllAdmins()
        {
            var response = await dashboardService.GetAllAdminsWithRolesAndPermissionsAsync();
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Performs a paginated search (with optional filtering and sorting)
        /// on a generic entity type <typeparamref name="T"/> and returns
        /// the results as an <see cref="IActionResult"/>.
        /// </summary>
        /// <typeparam name="T">The entity type that implements <see cref="IBaseModel"/>.</typeparam>
        /// <param name="dto">
        /// An object containing pagination, sorting, and filtering parameters 
        /// (page number, sort order, and optional filter conditions).
        /// </param>
        /// <returns>
        /// An <see cref="IActionResult"/> containing the paginated data if successful, 
        /// or a bad request result if the operation fails.
        /// </returns>
        private IActionResult TableFSP<T>(TablesFSPDTO dto)
            where T : class, IBaseModel
        {
            var genericFSPService = new GenericFSPService<HoshiDbContext, T, T>(context, mapper);

            dynamic result;

            if (!dto.Filters.IsNullOrEmpty())
            {
                PaginationFilteredSearchDTO fspDTO = new PaginationFilteredSearchDTO()
                {
                    Filters = dto.Filters!,
                    Ascending = dto.Ascending,
                    PageNumber = dto.PageNumber,
                    PageSize = 10
                };

                result = genericFSPService.PaginationFilteredSearch(fspDTO, null);
            }
            else
                result = genericFSPService.Pagination(null, dto.PageNumber, 10, dto.Ascending);

            if (result.IsSuccess) return Ok(result);

            else return BadRequest(result);
        }
    }
}
