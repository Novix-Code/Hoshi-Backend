using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using GenericCRUDLibrary.GenericControllers;
using GenericCRUDLibrary.GenericRepositories.GenericCRUDService;
using Hoshi.Data;
using Hoshi.DTOs.OrderDTOs.OrderImageDTOs;
using Hoshi.Models.OrderModels;

namespace Hoshi.Controllers.OrderControllers.OrderImageControllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class OrderImageController : SoftDeleteGenericController<
        HoshiDbContext, 
        OrderImage, 
        OrderImageGetDTO, 
        OrderImagePostDTO, 
        OrderImagePutDTO>
    {
        public OrderImageController(
            IMapper mapper, 
            IGenericCRUDService<
                HoshiDbContext, 
                OrderImage, 
                OrderImageGetDTO, 
                OrderImagePostDTO, 
                OrderImagePutDTO> genericCRUDService
        ) : base(mapper, genericCRUDService)
        {
            // Add Includes

			includes = [
				$"{nameof(OrderImage.Order)}",
			];
        }

        [EndpointGroupName("Client")]
        public override Task<IActionResult> Add(OrderImagePostDTO postDTO)
        {
            return base.Add(postDTO);
        }

        [EndpointGroupName("Client")]
        public override Task<IActionResult> Update(OrderImagePutDTO putDTO)
        {
            return base.Update(putDTO);
        }
    }
}
