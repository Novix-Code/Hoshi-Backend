using AutoMapper;
using GenericCRUDLibrary.GenericControllers;
using GenericCRUDLibrary.GenericRepositories.GenericCRUDService;
using GenericCRUDLibrary.GenericRepositories.GenericFSPService;
using Hoshi.Data;
using Hoshi.DTOs.OrderDTOs.OfferDTOs;
using Hoshi.Models.OrderModels;
using Hoshi.Repositories.ClientOfferService;
using Hoshi.Repositories.WorkerOfferService;
using Microsoft.AspNetCore.Mvc;

namespace Hoshi.Controllers.OrderControllers.OfferControllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class OfferController : SoftDeleteGenericFSPController<
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

        [EndpointGroupName("Worker")]
        public override Task<IActionResult> Update(OfferPutDTO putDTO)
        {
            return base.Update(putDTO);
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
        public override async Task<IActionResult> GetById(int id)
        {
            var response = await clientOfferService.GetByIdAsync(id);
            return StatusCode((int)response.StatusCode, response);
        }

        [EndpointGroupName("Client")]
        [HttpPost("AcceptOffer{id}")]
        public async Task<IActionResult> AcceptOffer(int id)
        {
            var response = await clientOfferService.AcceptOfferAsync(id);
            return StatusCode((int)response.StatusCode, response);
        }
    }
}
