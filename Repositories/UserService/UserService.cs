using AutoMapper;
using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.UserDTOs.AdminDTOs.PermissionDTOs;
using Hoshi.DTOs.UserDTOs.AdminDTOs.UserPermissionDTOs;
using Hoshi.DTOs.UserDTOs.SuspendedUserDTOs;
using Hoshi.DTOs.UserDTOs.UserDTOs;
using Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerPortfolioDTOs;
using Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerSpecificationDTOs;
using Hoshi.Enums;
using Hoshi.Models.GlobalModels;
using Hoshi.Models.OrderModels;
using Hoshi.Models.UserModels;
using Hoshi.Models.UserModels.WorkerModels;
using Hoshi.Repositories.FileServiceFold;
using Hoshi.Repositories.TokenService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Hoshi.Repositories.UserService
{
    /// <summary>
    /// Implements user-related operations for admin dashboard views, profile image management,
    /// worker application approval/rejection, and admin role-permission listings.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly HoshiDbContext _context;
        private readonly ITokenService _tokenService;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;

        public UserService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            HoshiDbContext context,
            ITokenService tokenService,
            IMapper mapper,
            IFileService fileService
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _tokenService = tokenService;
            _fileService = fileService;
            _mapper = mapper;
        }

        /// <summary>
        /// Save user's personal image to storage and update user's ImageURL.
        /// </summary>
        /// <param name="id">User id that will be updated</param>
        /// <param name="image">Image file that will be saved</param>
        /// <returns>Return a Tuple of Bool and String to check if done successfully or not and get the image url or the error.</returns>
        public async Task<Tuple<bool, string>> AddUserImage(int id, IFormFile image, bool isUpdate)
        {
            try
            {
                // Fetch user data from its id
                User? user = await _context.Set<User>().FindAsync(id);

                // Check if this user is there or not
                if (user == null)
                    return new Tuple<bool, string>(false, "Invalid User Id.");

                if (isUpdate)
                {
                    if (user.ImageURL != null)
                        _fileService.DeleteFile(user.ImageURL!);
                }

                // Use FileService to save the image to images/personalimages in wwwroot
                var imageResult = await _fileService.SaveFileAsync(image, Path.Combine("images","personalimages"));

                // Check if the image saved successfuly
                if (imageResult.Item1)
                {
                    // Add image url to user object data
                    user.ImageURL = imageResult.Item2;
                    // Update it in db and save changes
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }

                // return the images result in all cases
                return imageResult;
            }
            catch (Exception ex)
            {
                // If anything happens return the exception
                return new Tuple<bool, string>(false, ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ResultDTO<object>> SuspendUser(SuspendedUserPostDTO suspendDTO)
        {
            var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Check user
                User? user = await _context.Set<User>().FindAsync(suspendDTO.UserId);

                if (user == null)
                    return ResultDTO<object>.BadRequest(
                        new ErrorDTO()
                        {
                            ErrorAr = "هذا المستخدم غير موجود.",
                            ErrorEn = "This user not existed."
                        }
                    );

                // Check suspend reason
                SuspendReason? reason = await _context.Set<SuspendReason>().FindAsync(suspendDTO.SuspendReasonId);

                if (reason == null)
                    return ResultDTO<object>.BadRequest(
                        new ErrorDTO()
                        {
                            ErrorAr = "هذا السبب غير موجود.",
                            ErrorEn = "This reason not existed."
                        }
                    );

                // Check if user already is suspended
                SuspendedUser? suspendUser = await _context.Set<SuspendedUser>()
                    .FirstOrDefaultAsync(su => su.UserId == suspendDTO.UserId);

                if (suspendUser != null)
                    return ResultDTO<object>.BadRequest(
                        new ErrorDTO()
                        {
                            ErrorAr = "هذا المستخدم تم تعليقه بالفعل.",
                            ErrorEn = "This user is already suspended."
                        }
                    );
                else
                {
                    // If the user not has an active suspention will add the new suspention
                    await _context.Set<SuspendedUser>().AddAsync(_mapper.Map<SuspendedUser>(suspendDTO));

                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    return ResultDTO<object>.Success(new MessageDTO()
                    {
                        MessageAr = "تم التعليق بنجاح.",
                        MessageEn = "Suspention done successfully."
                    });
                }
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                return ResultDTO<object>.InternalServerError(
                    new ErrorDTO()
                    {
                        ErrorAr = "يوجد مشكلة في عملية التعليق.",
                        ErrorEn = "There is a problem in Suspending process."
                    },
                    ex.InnerException != null ? ex.InnerException.Message : ex.Message
                );
            }
        }
    }
}
