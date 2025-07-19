using AutoMapper;
using Azure;
using GenericCRUDLibrary.GenericDTOs.InputsDTOs;
using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.FileServicieResult;
using Hoshi.DTOs.UserDTOs.UserDTOs;
using Hoshi.DTOs.UserDTOs.UserRegistiration;
using Hoshi.Models.UserModels;
using Hoshi.Models.UserModels.Resets;
using Hoshi.Repositories.FileServiceFold;
using Hoshi.Repositories.TokenServ;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Hoshi.Repositories.UserService
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenService _tokenService;
            
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;
        private readonly HoshiDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public UserService(UserManager<User> userManager,
            SignInManager<User> signInManager,
            IFileService fileService,
            IMapper mapper,
            HoshiDbContext unitOfWork,
            IHttpContextAccessor httpContextAccessor,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _fileService = fileService;
            _mapper = mapper;
            _context = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _tokenService = tokenService;
        }

        public async Task<ResultDTO<string>> CreateResetPasswordTokenAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null) return ResultDTO<string>.Failure(new ErrorDTO(), ResponseStatusCodes.BadRequest);

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            if (string.IsNullOrEmpty(token)) return ResultDTO<string>.Failure(new ErrorDTO(), ResponseStatusCodes.BadRequest);

            // Hash the token before storing
            var hashedToken = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(token)));

            //Add this to the Password Reset Requests Table
            var passwordResetRequest = new PasswordResetRequest
            {
                ExpiresAt = DateTime.UtcNow.AddHours(1),
                ResetToken = hashedToken,
                UserId = user.Id
            };
            var passwordResetTokenRequestRepo = await _context.PasswordResetRequests.ToListAsync();
            passwordResetTokenRequestRepo.Add(passwordResetRequest);
            await _context.SaveChangesAsync();

            return ResultDTO<string>.Success( token);
        }

        public async Task<ResultDTO<string>> ResetPasswordAsync(ResetPasswordRequestDto resetPasswordRequestDto)
        {
            var user = await _userManager.FindByEmailAsync(resetPasswordRequestDto.Email);
            if (user is null) return ResultDTO<string>.Failure(new ErrorDTO(), ResponseStatusCodes.BadRequest);

            //Hash the recieved token for comparison
            var hashedRecievedToken = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(resetPasswordRequestDto.Token)));

            var passwordResetTokenRequestRepo = await _context.PasswordResetRequests.ToListAsync();
            var passwordResetRequest = passwordResetTokenRequestRepo
                .Where(r => r.UserId == user.Id && r.ResetToken == hashedRecievedToken).FirstOrDefault();

            //Validate the token and its expiry
            if (passwordResetRequest is null || passwordResetRequest.ExpiresAt < DateTime.UtcNow)
                return  ResultDTO<string>.Failure(new ErrorDTO(), ResponseStatusCodes.BadRequest);

            var resutl = await _userManager.ResetPasswordAsync(user, resetPasswordRequestDto.Token,
                resetPasswordRequestDto.NewPassword);

            if (!resutl.Succeeded)
            {
                var errors = resutl.Errors.Select(e => e.Description).ToList();
                return ResultDTO<string>.Failure(new ErrorDTO(), ResponseStatusCodes.BadRequest);
            }

            //Remove the used token as it is one time use
            passwordResetTokenRequestRepo.Remove(passwordResetRequest);
            await _context.SaveChangesAsync();

            return ResultDTO<string>.Success("Password has been changed successfully");
        }

        public async Task<ResultDTO<string>> Delete(string id)
        {
            var applicationUser = await _userManager.FindByIdAsync(id);
            if (applicationUser is null)
                return ResultDTO<string>.Failure(new ErrorDTO(), ResponseStatusCodes.BadRequest);

            var logoutResult = await Logout(id);
            if ((int)logoutResult.StatusCode < 200 || (int)logoutResult.StatusCode > 299)
                return logoutResult;
            //Prevent the admin from deleting himself
            var isAdmin = await _userManager.IsInRoleAsync(applicationUser, "admin");
            if (isAdmin)
                return ResultDTO<string>.Failure(new ErrorDTO(), ResponseStatusCodes.BadRequest);


            var identityResult = await _userManager.UpdateAsync(applicationUser);
            if (!identityResult.Succeeded)
            {
                var errors = identityResult.Errors.Select(e => e.Description).ToList();
                return ResultDTO<string>.Failure(new ErrorDTO(), ResponseStatusCodes.BadRequest);
            }

          
            return ResultDTO<string>.NoContent();
        }

        public async Task<ResultDTO<string>> Edit(ApplicationUserEditRequestDto userEditRequestDto)
        {
            var applicationUser = await _userManager.FindByEmailAsync(userEditRequestDto.Email);
            if (applicationUser is null)
                return ResultDTO<string>.Failure(new ErrorDTO(), ResponseStatusCodes.BadRequest);

            applicationUser.UserName = userEditRequestDto.Name;
            applicationUser.PhoneNumber = userEditRequestDto.Phone;

            ////Updating the profile picture
            //var fileServiceResult = await _fileService.SaveFileAsync(userEditRequestDto.Profile_Picture,
            //    "Images\\Users");
            //if (fileServiceResult != FileServiceResults.EmptyFile &&
            //    fileServiceResult != FileServiceResults.UnsupportedFileExtension)
            //{
            //    //First delete the old picture
            //    _fileService.DeleteFile(applicationUser.ProfilePictureUrl, "Images\\Users");

            //    //Set the new ImageUrl
            //    applicationUser.ProfilePictureUrl = fileServiceResult;
            //}

            var identityResult = await _userManager.UpdateAsync(applicationUser);
            if (!identityResult.Succeeded)
            {
                var errors = identityResult.Errors.Select(e => e.Description).ToList();
                return ResultDTO<string>.Failure(new ErrorDTO(), ResponseStatusCodes.BadRequest);
            }

            return ResultDTO<string>.Success("successfully updated");
        }

        public async Task<ResultDTO<UserGetDTO>> GetById(string id)
        {
            var applicationUser = await _userManager.FindByIdAsync(id);
            if (applicationUser is null)
                return ResultDTO<UserGetDTO>.Failure(new ErrorDTO(), ResponseStatusCodes.BadRequest);

            var mappedUser = _mapper.Map<UserGetDTO>(applicationUser);
            return ResultDTO<UserGetDTO>.Success(mappedUser);
        }

        public async Task<ResultDTO<object>> Login(ApplicationUserLoginRequestDto loginRequestDto)
        {
            var applicationUser = await _userManager.FindByEmailAsync(loginRequestDto.Email);
            if (applicationUser is null)
                return ResultDTO<object>.Failure(new ErrorDTO(), ResponseStatusCodes.BadRequest);


            var signInResult = await _signInManager.CheckPasswordSignInAsync(applicationUser, loginRequestDto.Password,
                false);

            if (!signInResult.Succeeded)
                return ResultDTO<object>.Failure(new ErrorDTO(), ResponseStatusCodes.BadRequest);

            var token = await _tokenService.CreateTokenAsync(applicationUser);
            await _context.SaveChangesAsync();

            return  ResultDTO<object>.Success(new { UserId = applicationUser.Id, Token = token , Message = "Logged successfully" });
        }

        public async Task<ResultDTO<object>> Register([FromForm] ApplicationUserRegisterRequestDto registerRequestDto)
        {
            var fileSavingResult = await _fileService.SaveFileAsync(registerRequestDto.ProfilePicture, "Images\\Users");
            if (fileSavingResult == FileServiceResults.UnsupportedFileExtension)
                return ResultDTO<object>.Failure(new ErrorDTO(), ResponseStatusCodes.BadRequest); 

            var applicationUser = new User
            {
                UserName = registerRequestDto.Name,
                PhoneNumber = registerRequestDto.Phone,
                Email = registerRequestDto.Email,
            };

            var identityResult = await _userManager.CreateAsync(applicationUser, registerRequestDto.Password);

            if (!identityResult.Succeeded)
            {
                var errors = identityResult.Errors.Select(e => e.Description).ToList();
                return ResultDTO<object>.BadRequest(new ErrorDTO { ErrorAr =$"{errors}" });
            }

            //Assign the registered user to the buyer role by default
            await _userManager.AddToRoleAsync(applicationUser, "buyer");

            //Generate token
            var token = await _tokenService.CreateTokenAsync(applicationUser);

            return ResultDTO<object>.Success(
                new 
                {
                    UserId = applicationUser.Id,
                    Token = token
                }
                );
        }

        public async Task<ResultDTO<object>> GetAllUsers()
        {
            var usersQuery = await _userManager.Users
                .Where(u => u.IsDeleted == false).ToListAsync();

            if (!usersQuery.Any())
                return ResultDTO<object>.NoContent();

            return ResultDTO<object>.Success(usersQuery);
        }

        public ResultDTO<object> GetCurrentUserId()
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null) return ResultDTO<object>.NoContent();

            return ResultDTO<object>.Success(new { UserId = userId });
        }


        //public async Task<ResultDTO<string>> BanUserAsync(string userId)
        //{
        //    var user = await _userManager.FindByIdAsync(userId);

        //    var validationResult = await ValidateUserForBanOperation(user);
        //    if (validationResult != null)
        //        return validationResult;

        //    if (user.IsBanned)
        //        return new BadRequestResponse(
        //            message: "Ban operation failed",
        //            errors: new List<string> { "User is already banned" });
        //    var logoutResult = await Logout(userId);
        //    if ((int)logoutResult.StatusCode < 200 || (int)logoutResult.StatusCode > 299)
        //        return logoutResult;
        //    user.IsBanned = true;
        //    var result = await _userManager.UpdateAsync(user);

        //    if (!result.Succeeded)
        //    {
        //        return new BadRequestResponse(
        //            message: "Failed to ban user",
        //            errors: result.Errors.Select(e => e.Description).ToList());
        //    }

        //    return new OkResponse<string>("User banned successfully");
        //}

        //public async Task<ResultDTO<string>> UnbanUserAsync(string userId)
        //{
        //    var user = await _userManager.FindByIdAsync(userId);

        //    var validationResult = await ValidateUserForBanOperation(user);
        //    if (validationResult != null)
        //        return validationResult;

        //    if (!user.IsBanned)
        //        return new BadRequestResponse(
        //            message: "Unban operation failed",
        //            errors: new List<string> { "User is Arleady Unbanned" });

        //    user.IsBanned = false;
        //    var result = await _userManager.UpdateAsync(user);

        //    if (!result.Succeeded)
        //    {
        //        return new BadRequestResponse(
        //            message: "Failed to unban user",
        //            errors: result.Errors.Select(e => e.Description).ToList());
        //    }

        //    return new OkResponse<string>("User unbanned successfully");
        //}

        //private async Task<ResultDTO<string>?> ValidateUserForBanOperation(User user)
        //{
        //    if (user == null)
        //        return new NotFoundResponse("User not found");

        //    var protectedRoles = new List<string> { "admin" };

        //    // Properly await the async operation
        //    var userRoles = await _userManager.GetRolesAsync(user);
        //    var hasProtectedRole = userRoles.Any(r => protectedRoles.Contains(r));

        //    if (hasProtectedRole)
        //        return new BadRequestResponse(
        //            message: "Operation not permitted",
        //            errors: new List<string> { "Cannot ban/unban privileged users" });

        //    return null;
        //}


        public async Task<ResultDTO<string>> Logout(string userId = null)
        {
            // 1. Get current JWT from header
            var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"]
                .ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
                return ResultDTO<string>.NotFound(new ErrorDTO(), "token not found");

            // 2. Blacklist the token (critical for JWT invalidation)
            await _tokenService.InvalidateTokenAsync(token);

            return ResultDTO<string>.Success("Logged out successfully");
        }

     
    }
}
