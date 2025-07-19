using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using GenericCRUDLibrary.GenericControllers;
using GenericCRUDLibrary.GenericRepositories.GenericCRUDService;
using Hoshi.Data;
using Hoshi.DTOs.UserDTOs.ClientSpecificationDTOs;
using Hoshi.Models.UserModels;

namespace Hoshi.Controllers.UserControllers.ClientSpecificationControllers
{

    [ApiController]
    [Route("api/[controller]")]
	[EndpointGroupName("Client")]
    public class ClientSpecificationController : GenericController<
        HoshiDbContext, 
        ClientSpecification, 
        ClientSpecificationGetDTO, 
        ClientSpecificationPostDTO, 
        ClientSpecificationPutDTO>
    {
        public ClientSpecificationController(
            IMapper mapper, 
            IGenericCRUDService<
                HoshiDbContext, 
                ClientSpecification, 
                ClientSpecificationGetDTO, 
                ClientSpecificationPostDTO, 
                ClientSpecificationPutDTO> genericCRUDService 
        ) : base(mapper, genericCRUDService)
        {
            // Add Includes

			includes = [
				$"{nameof(ClientSpecification.User)}",
				$"{nameof(ClientSpecification.LivingCity)}",
			];
        }

        [HttpDelete("Delete{id}")]
        public override Task<IActionResult> Delete(int id)
        {
            return base.Delete(id);
        }
    }
}
