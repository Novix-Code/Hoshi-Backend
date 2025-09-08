using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using GenericCRUDLibrary.GenericControllers;
using GenericCRUDLibrary.GenericRepositories.GenericCRUDService;
using Hoshi.Data;
using Hoshi.DTOs.OrderDTOs.OrderImageDTOs;
using Hoshi.Models.OrderModels;
using Hoshi.Repositories.OrderImageService;
using Microsoft.AspNetCore.Authorization;

namespace Hoshi.Controllers.OrderControllers.OrderImageControllers
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class OrderImageController : GenericController<
        HoshiDbContext, 
        OrderImage, 
        OrderImageGetDTO, 
        OrderImagePostDTO, 
        OrderImagePutDTO>
    {
        private readonly IOrderImageService imageService;

        public OrderImageController(
            IMapper mapper, 
            IGenericCRUDService<
                HoshiDbContext, 
                OrderImage, 
                OrderImageGetDTO, 
                OrderImagePostDTO, 
                OrderImagePutDTO> genericCRUDService,
            IOrderImageService imageService
        ) : base(mapper, genericCRUDService)
        {
            // Add Includes

			includes = [
				$"{nameof(OrderImage.Order)}",
			];
            this.imageService = imageService;
        }

        [EndpointGroupName("Client")]
        [HttpPost("Add")]
        public async Task<IActionResult> AddImage(int orderId, IFormFile image)
        {
            List<IFormFile> images = new List<IFormFile>() { image };

            var result = await imageService.AddImages(orderId, images);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("AddList")]
        public async Task<IActionResult> AddImages(int orderId, List<IFormFile> images)
        {
            var result = await imageService.AddImages(orderId, images);

            return StatusCode(result.StatusCode, result);
        }

        [NonAction]
        public override Task<IActionResult> Add(OrderImagePostDTO postDTO)
        {
            return base.Add(postDTO);
        }

        [NonAction]
        public override Task<IActionResult> Update(OrderImagePutDTO putDTO)
        {
            return base.Update(putDTO);
        }

        [NonAction]
        public override Task<IActionResult> AddList(List<OrderImagePostDTO> postDTOsList)
        {
            return base.AddList(postDTOsList);
        }

        [NonAction]
        public override Task<IActionResult> Delete(int id)
        {
            return base.Delete(id);
        }
    }
}
