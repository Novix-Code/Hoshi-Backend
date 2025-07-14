using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using GenericCRUDLibrary.GenericControllers;
using GenericCRUDLibrary.GenericRepositories.GenericCRUDService;
using GenericCRUDLibrary.GenericRepositories.GenericFSPService;
using GenericCRUDLibrary.GenericDTOs.InputsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.UserDTOs.SuspendedUserDTOs;
using Hoshi.Models.UserModels;

namespace Hoshi.Controllers.UserControllers.SuspendedUserControllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class SuspendedUserController : GenericFSPController<
        HoshiDbContext, 
        SuspendedUser, 
        SuspendedUserGetDTO, 
        SuspendedUserPostDTO, 
        SuspendedUserPutDTO>
    {
        public SuspendedUserController(
            IMapper mapper, 
            IGenericCRUDService<
                HoshiDbContext, 
                SuspendedUser, 
                SuspendedUserGetDTO, 
                SuspendedUserPostDTO, 
                SuspendedUserPutDTO> genericCRUDService, 
            IGenericFSPService<
                HoshiDbContext, 
                SuspendedUser, 
                SuspendedUserGetDTO> genericFSPService 
        ) : base(mapper, genericCRUDService, genericFSPService)
        {
            // Add Includes

			includes = [
				$"{nameof(SuspendedUser.User)}",
				$"{nameof(SuspendedUser.SuspendReason)}",
			];
        }

        [EndpointGroupName("Admin")]
        public override Task<IActionResult> Add(SuspendedUserPostDTO postDTO)
        {
            return base.Add(postDTO);
        }

        [EndpointGroupName("Admin")]
        public override Task<IActionResult> AddList(List<SuspendedUserPostDTO> postDTOsList)
        {
            return base.AddList(postDTOsList);
        }

        [EndpointGroupName("Admin")]
        public override Task<IActionResult> Update(SuspendedUserPutDTO putDTO)
        {
            return base.Update(putDTO);
        }

        [EndpointGroupName("Admin")]
        public override Task<IActionResult> Delete(int id)
        {
            return base.Delete(id);
        }
    }
}
