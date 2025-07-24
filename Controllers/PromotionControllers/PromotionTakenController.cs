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
				$"{nameof(PromotionTaken.Order)}",
				$"{nameof(PromotionTaken.Offer)}",
			];
        }
    }
}
