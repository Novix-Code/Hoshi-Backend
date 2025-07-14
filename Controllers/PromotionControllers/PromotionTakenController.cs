using Microsoft.AspNetCore.Mvc;
using GenericCRUDLibrary.GenericControllers;
using GenericCRUDLibrary.GenericRepositories.GenericFSPService;
using Hoshi.Models.OrderModels;
using GenericCRUDLibrary.GenericDTOs.InputsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.PromotionDTOs.PromotionTakenDTOs;
using Hoshi.Models.PromotionModels;

namespace Hoshi.Controllers.PromotionControllers.PromotionTakenControllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class PromotionTakenController : GenericJustFSPController<
        HoshiDbContext, 
        PromotionTaken, 
        PromotionTakenGetDTO>
    {
        public PromotionTakenController(
            IGenericFSPService<
                HoshiDbContext, 
                PromotionTaken, 
                PromotionTakenGetDTO> genericFSPService 
        ) : base(genericFSPService)
        {
            // Add Includes

			includes = [
				$"{nameof(PromotionTaken.User)}",
				$"{nameof(PromotionTaken.Promotion)}",
				$"{nameof(PromotionTaken.Order)}.{nameof(Order.Client)}",
				$"{nameof(PromotionTaken.User)}",
				$"{nameof(PromotionTaken.Promotion)}",
				$"{nameof(PromotionTaken.Order)}.{nameof(Order.Worker)}",
				$"{nameof(PromotionTaken.User)}",
				$"{nameof(PromotionTaken.Promotion)}",
				$"{nameof(PromotionTaken.Order)}.{nameof(Order.City)}",
				$"{nameof(PromotionTaken.User)}",
				$"{nameof(PromotionTaken.Promotion)}",
				$"{nameof(PromotionTaken.Order)}.{nameof(Order.Service)}",
				$"{nameof(PromotionTaken.User)}",
				$"{nameof(PromotionTaken.Promotion)}",
				$"{nameof(PromotionTaken.Order)}.{nameof(Order.AppliedPromotion)}",
				$"{nameof(PromotionTaken.User)}",
				$"{nameof(PromotionTaken.Promotion)}",
				$"{nameof(PromotionTaken.Order)}.{nameof(Order.OrderImages)}",
				$"{nameof(PromotionTaken.User)}",
				$"{nameof(PromotionTaken.Promotion)}",
				$"{nameof(PromotionTaken.Order)}.{nameof(Order.OrderVisits)}",
				$"{nameof(PromotionTaken.User)}",
				$"{nameof(PromotionTaken.Promotion)}",
				$"{nameof(PromotionTaken.Order)}.{nameof(Order.OrderStatusHistory)}",
				$"{nameof(PromotionTaken.Offer)}.{nameof(Offer.Worker)}",
				$"{nameof(PromotionTaken.User)}",
				$"{nameof(PromotionTaken.Promotion)}",
				$"{nameof(PromotionTaken.Order)}.{nameof(Order.Client)}",
				$"{nameof(PromotionTaken.User)}",
				$"{nameof(PromotionTaken.Promotion)}",
				$"{nameof(PromotionTaken.Order)}.{nameof(Order.Worker)}",
				$"{nameof(PromotionTaken.User)}",
				$"{nameof(PromotionTaken.Promotion)}",
				$"{nameof(PromotionTaken.Order)}.{nameof(Order.City)}",
				$"{nameof(PromotionTaken.User)}",
				$"{nameof(PromotionTaken.Promotion)}",
				$"{nameof(PromotionTaken.Order)}.{nameof(Order.Service)}",
				$"{nameof(PromotionTaken.User)}",
				$"{nameof(PromotionTaken.Promotion)}",
				$"{nameof(PromotionTaken.Order)}.{nameof(Order.AppliedPromotion)}",
				$"{nameof(PromotionTaken.User)}",
				$"{nameof(PromotionTaken.Promotion)}",
				$"{nameof(PromotionTaken.Order)}.{nameof(Order.OrderImages)}",
				$"{nameof(PromotionTaken.User)}",
				$"{nameof(PromotionTaken.Promotion)}",
				$"{nameof(PromotionTaken.Order)}.{nameof(Order.OrderVisits)}",
				$"{nameof(PromotionTaken.User)}",
				$"{nameof(PromotionTaken.Promotion)}",
				$"{nameof(PromotionTaken.Order)}.{nameof(Order.OrderStatusHistory)}",
				$"{nameof(PromotionTaken.Offer)}.{nameof(Offer.Order)}",
				$"{nameof(PromotionTaken.User)}",
				$"{nameof(PromotionTaken.Promotion)}",
				$"{nameof(PromotionTaken.Order)}.{nameof(Order.Client)}",
				$"{nameof(PromotionTaken.User)}",
				$"{nameof(PromotionTaken.Promotion)}",
				$"{nameof(PromotionTaken.Order)}.{nameof(Order.Worker)}",
				$"{nameof(PromotionTaken.User)}",
				$"{nameof(PromotionTaken.Promotion)}",
				$"{nameof(PromotionTaken.Order)}.{nameof(Order.City)}",
				$"{nameof(PromotionTaken.User)}",
				$"{nameof(PromotionTaken.Promotion)}",
				$"{nameof(PromotionTaken.Order)}.{nameof(Order.Service)}",
				$"{nameof(PromotionTaken.User)}",
				$"{nameof(PromotionTaken.Promotion)}",
				$"{nameof(PromotionTaken.Order)}.{nameof(Order.AppliedPromotion)}",
				$"{nameof(PromotionTaken.User)}",
				$"{nameof(PromotionTaken.Promotion)}",
				$"{nameof(PromotionTaken.Order)}.{nameof(Order.OrderImages)}",
				$"{nameof(PromotionTaken.User)}",
				$"{nameof(PromotionTaken.Promotion)}",
				$"{nameof(PromotionTaken.Order)}.{nameof(Order.OrderVisits)}",
				$"{nameof(PromotionTaken.User)}",
				$"{nameof(PromotionTaken.Promotion)}",
				$"{nameof(PromotionTaken.Order)}.{nameof(Order.OrderStatusHistory)}",
				$"{nameof(PromotionTaken.Offer)}.{nameof(Offer.AppliedPromotion)}",
			];
        }
    }
}
