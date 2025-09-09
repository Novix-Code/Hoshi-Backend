using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using GenericCRUDLibrary.GenericControllers;
using GenericCRUDLibrary.GenericRepositories.GenericCRUDService;
using Hoshi.Data;
using Hoshi.DTOs.GlobalDTOs.CityDTOs;
using Hoshi.Models.GlobalModels;
using Microsoft.AspNetCore.Authorization;

namespace Hoshi.Controllers.GlobalControllers.CityControllers
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CityController : GenericController<
        HoshiDbContext, 
        City, 
        CityGetDTO, 
        CityPostDTO, 
        CityPutDTO>
    {
        public CityController(
            IMapper mapper, 
            IGenericCRUDService<
                HoshiDbContext, 
                City, 
                CityGetDTO, 
                CityPostDTO, 
                CityPutDTO> genericCRUDService 
        ) : base(mapper, genericCRUDService)
        {
        }

        [NonAction]
        public override Task<IActionResult> Delete(int id)
        {
            return base.Delete(id);
        }

        [EndpointGroupName("Admin")]
        public override Task<IActionResult> Add(CityPostDTO postDTO)
        {
            return base.Add(postDTO);
        }

        [EndpointGroupName("Admin")]
        public override Task<IActionResult> AddList(List<CityPostDTO> postDTOsList)
        {
            return base.AddList(postDTOsList);
        }

        [EndpointGroupName("Admin")]
        public override Task<IActionResult> Update(CityPutDTO putDTO)
        {
            return base.Update(putDTO);
        }
    }
}
