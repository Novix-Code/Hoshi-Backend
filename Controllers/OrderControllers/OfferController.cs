using AutoMapper;
using GenericCRUDLibrary.GenericControllers;
using GenericCRUDLibrary.GenericRepositories.GenericCRUDService;
using GenericCRUDLibrary.GenericRepositories.GenericFSPService;
using Hoshi.Data;
using Hoshi.DTOs.OrderDTOs.OfferDTOs;
using Hoshi.Models.OrderModels;
using Hoshi.Repositories.ClientOfferService;
using Hoshi.Repositories.WorkerOfferService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hoshi.Controllers.OrderControllers.OfferControllers
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class OfferController : GenericFSPController<
        HoshiDbContext, 
        Offer, 
        OfferGetDTO, 
        OfferPostDTO, 
        OfferPutDTO>
    {
		private readonly IClientOfferService clientOfferService;
		private readonly IWorkerOfferService workerOfferService;
        public OfferController(
            IMapper mapper, 
            IGenericCRUDService<
                HoshiDbContext, 
                Offer, 
                OfferGetDTO, 
                OfferPostDTO, 
                OfferPutDTO> genericCRUDService, 
            IGenericFSPService<
                HoshiDbContext, 
                Offer, 
                OfferGetDTO> genericFSPService,
			IClientOfferService clientOfferService,
			IWorkerOfferService workerOfferService 
        ) : base(mapper, genericCRUDService, genericFSPService)
        {
            // Add Includes

			includes = [
				$"{nameof(Offer.Worker)}",
				$"{nameof(Offer.Order)}.{nameof(Order.City)}",
				$"{nameof(Offer.Order)}.{nameof(Order.Service)}",
				$"{nameof(Offer.Order)}.{nameof(Order.OrderImages)}",
				$"{nameof(Offer.AppliedPromotion)}",
			];
			this.clientOfferService = clientOfferService;
			this.workerOfferService = workerOfferService;
        }

        [NonAction]
        public override Task<IActionResult> AddList(List<OfferPostDTO> postDTOsList)
        {
            return base.AddList(postDTOsList);
        }

        [NonAction]
        public override Task<IActionResult> Add(OfferPostDTO postDTO)
        {
            return base.Add(postDTO);
        }

        [NonAction]
        public override Task<IActionResult> Update(OfferPutDTO putDTO)
        {
            return base.Update(putDTO);
        }

        [NonAction]
        public override Task<IActionResult> Delete(int id)
        {
            return base.Delete(id);
        }

        [EndpointGroupName("Worker")]
        [HttpPost("create-offer")]
        public async Task<IActionResult> CreateOffer([FromBody] OfferPostDTO dto)
        {
	        var result = await workerOfferService.CreateOfferAsync(dto);
	        return StatusCode(result.StatusCode, result);
        }

        [EndpointGroupName("Worker")]
        [HttpPatch("confirm-offer")]
        public async Task<IActionResult> ConfirmOffer([FromQuery] int OfferId)
        {
	        var result = await workerOfferService.ConfirmOfferAsync(OfferId);
	        return StatusCode(result.StatusCode, result);
        }

        [EndpointGroupName("Worker")]
        [HttpDelete("cancel-offer")]
        public async Task<IActionResult> CancelOffer([FromQuery] int offerId)
        {
	        var result = await workerOfferService.CancelOfferAsync(offerId);
	        return StatusCode(result.StatusCode, result);
        }

        [EndpointGroupName("Client")]
        [HttpGet("get-offer-details-by")]
        public override async Task<IActionResult> GetById([FromQuery] int id)
        {
            var response = await clientOfferService.GetOfferDetailsByIdAsync(id);
            return StatusCode((int)response.StatusCode, response);
        }

        [EndpointGroupName("Client")]
        [HttpPost("accept-offer")]
        public async Task<IActionResult> AcceptOffer([FromQuery] int id)
        {
            var response = await clientOfferService.AcceptOfferAsync(id);
            return StatusCode((int)response.StatusCode, response);
        }
    }
}
