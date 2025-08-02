using AutoMapper;
using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.ServiceDTOs.ServiceDTOs;
using Hoshi.DTOs.UserDTOs.ClientSpecificationDTOs;
using Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerPortfolioDTOs;
using Hoshi.Models.ServiceModels;
using Hoshi.Models.UserModels;
using Hoshi.Models.UserModels.WorkerModels;
using Hoshi.Repositories.FileServiceFold;
using Hoshi.Repositories.UserService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Hoshi.Repositories.ClientSpecificationService
{
    public class ClientSpecificationService : IClientSpecificationService
    {
        private readonly HoshiDbContext context;
        private readonly IMapper mapper;
        private readonly IFileService fileService;
        private readonly IUserService userService;

        public ClientSpecificationService(
            HoshiDbContext context,
            IMapper mapper,
            IFileService fileService,
            IUserService userService
        )
        {
            this.context = context;
            this.mapper = mapper;
            this.fileService = fileService;
            this.userService = userService;
        }

        /// <summary>
        /// Get Client Specifications by its UserId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Client Specification GetDTO for the provided user</returns>
        public async Task<ResultDTO<ClientSpecificationGetDTO>> GetById(int userId)
        {

            ErrorDTO error = new ErrorDTO();

            try
            {
                ClientSpecification? clientSpecification = await context.Set<ClientSpecification>()
                    .Include(nameof(ClientSpecification.User))
                    .FirstOrDefaultAsync(x => x.UserId == userId);

                if (clientSpecification == null)
                {
                    error.ErrorAr = "لا يوجد تفاصيل لهذا المستخدم.";
                    error.ErrorEn = "There is no Specification for this User.";

                    return ResultDTO<ClientSpecificationGetDTO>.BadRequest(error);
                }

                var csDto = mapper.Map<ClientSpecificationGetDTO>(clientSpecification);

                return ResultDTO<ClientSpecificationGetDTO>.Success(csDto);
            }
            catch (Exception ex)
            {
                error.ErrorAr = "يوجد مشكلة في عملية العرض.";
                error.ErrorEn = "There is a problem in Getting proccess.";

                return ResultDTO<ClientSpecificationGetDTO>.InternalServerError(
                    error, 
                    ex.InnerException is null ? ex.Message : ex.InnerException.Message
                );
            }
        }

        /// <summary>
        /// Update Client Specification and its User data.
        /// </summary>
        /// <param name="putDTO"> Dto of data that will be updated</param>
        /// <returns>Client Specification GetDTO after updating</returns>
        public async Task<ResultDTO<ClientSpecificationGetDTO>> Update(ClientSpecificationPutDTO putDTO)
        {

            ErrorDTO error = new ErrorDTO();

            try
            {
                ClientSpecification? clientSpecification = await context.Set<ClientSpecification>()
                    .Include(nameof(ClientSpecification.User))
                    .FirstOrDefaultAsync(x => x.Id == putDTO.Id);

                if (clientSpecification == null)
                {
                    error.ErrorAr = "لا يوجد تفاصيل لهذا المستخدم.";
                    error.ErrorEn = "There is no Specification for this User.";

                    return ResultDTO<ClientSpecificationGetDTO>.BadRequest(error);
                }

                // Update Client Specs data
                clientSpecification.Address = 
                    putDTO.Address ?? clientSpecification.Address;

                clientSpecification.Bio = 
                    putDTO.Bio ?? clientSpecification.Bio;

                // Update User data
                clientSpecification.User!.FullName = 
                    putDTO.FullName ?? clientSpecification.User.FullName;

                clientSpecification.User.Email = 
                    putDTO.Email ?? clientSpecification.User.Email;

                clientSpecification.User.PhoneNumber = 
                    putDTO.PhoneNumber ?? clientSpecification.User.PhoneNumber;

                // Save changes
                await context.SaveChangesAsync();

                if (clientSpecification.User.ImageURL.IsNullOrEmpty())
                    // Add User image
                    await userService.AddUserImage(clientSpecification.User.Id, putDTO.Image!, false);
                else
                    // Update User image
                    await userService.AddUserImage(clientSpecification.User.Id, putDTO.Image!, true);

                // Return client data
                var csDto = mapper.Map<ClientSpecificationGetDTO>(clientSpecification);

                return ResultDTO<ClientSpecificationGetDTO>.Success(csDto);
            }
            catch (Exception ex)
            {
                error.ErrorAr = "يوجد مشكلة في عملية التعديل.";
                error.ErrorEn = "There is a problem in Updating proccess.";

                return ResultDTO<ClientSpecificationGetDTO>.InternalServerError(
                    error, 
                    ex.InnerException is null ? ex.Message : ex.InnerException.Message
                );
            }

        }
    }
}
