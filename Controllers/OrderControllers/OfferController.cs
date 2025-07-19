using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using GenericCRUDLibrary.GenericControllers;
using GenericCRUDLibrary.GenericRepositories.GenericCRUDService;
using GenericCRUDLibrary.GenericRepositories.GenericFSPService;
using GenericCRUDLibrary.GenericDTOs.InputsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.OrderDTOs.OfferDTOs;
using Hoshi.Repositories.WorkerOfferService;

using Hoshi.Repositories.ClientOfferService;

using Hoshi.Models.OrderModels;

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
				$"{nameof(Offer.Order)}.{nameof(Order.Client)}",
				$"{nameof(Offer.Worker)}",
				$"{nameof(Offer.Order)}.{nameof(Order.Worker)}",
				$"{nameof(Offer.Worker)}",
				$"{nameof(Offer.Order)}.{nameof(Order.City)}",
				$"{nameof(Offer.Worker)}",
				$"{nameof(Offer.Order)}.{nameof(Order.Service)}",
				$"{nameof(Offer.Worker)}",
				$"{nameof(Offer.Order)}.{nameof(Order.AppliedPromotion)}",
				$"{nameof(Offer.Worker)}",
				$"{nameof(Offer.Order)}.{nameof(Order.OrderImages)}",
				$"{nameof(Offer.Worker)}",
				$"{nameof(Offer.Order)}.{nameof(Order.OrderVisits)}",
				$"{nameof(Offer.Worker)}",
				$"{nameof(Offer.Order)}.{nameof(Order.OrderStatusHistory)}",
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

        [EndpointGroupName("Worker")]
        public override Task<IActionResult> Add(OfferPostDTO postDTO)
        {
            return base.Add(postDTO);
        }

        [EndpointGroupName("Worker")]
        public override Task<IActionResult> Update(OfferPutDTO putDTO)
        {
            return base.Update(putDTO);
        }
    }
}
