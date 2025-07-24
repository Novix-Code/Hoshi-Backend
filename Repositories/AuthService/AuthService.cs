using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.DTOs.FileServicieResult;
using Hoshi.DTOs.UserDTOs.UserDTOs;
using Hoshi.DTOs.UserDTOs.UserRegistiration;
using Hoshi.Models.UserModels.Resets;
using Hoshi.Models.UserModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Hoshi.Data;
using Hoshi.Repositories.FileServiceFold;
using Hoshi.Repositories.TokenServ;
using System.Security.Cryptography;
using Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerSpecificationDTOs;
using Hoshi.Enums;
using Hoshi.Models.GlobalModels;
using Hoshi.Models.UserModels.WorkerModels;

namespace Hoshi.Repositories.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenService _tokenService;

        private readonly IFileService _fileService;
        private readonly IMapper _mapper;
        private readonly HoshiDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthService(UserManager<User> userManager,
            SignInManager<User> signInManager,
            IFileService fileService,
            IMapper mapper,
            HoshiDbContext unitOfWork,
            IHttpContextAccessor httpContextAccessor,
            ITokenService tokenService,
            RoleManager<IdentityRole<int>> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _fileService = fileService;
            _mapper = mapper;
            _context = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _tokenService = tokenService;
            _roleManager = roleManager;

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
            _context.PasswordResetRequests.Add(passwordResetRequest);
            await _context.SaveChangesAsync();

            return ResultDTO<string>.Success(token);
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
                return ResultDTO<string>.Failure(new ErrorDTO(), ResponseStatusCodes.BadRequest);

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

        
            var identityResult = await _userManager.UpdateAsync(applicationUser);
            if (!identityResult.Succeeded)
            {
                var errors = identityResult.Errors.Select(e => e.Description).ToList();


                return ResultDTO<string>.Failure(new ErrorDTO
                {
                    ErrorAr = string.Join(" | ", errors),
                    ErrorEn = string.Join(" | ", errors)
                }, ResponseStatusCodes.BadRequest);
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
            {
                return ResultDTO<object>.BadRequest(new ErrorDTO { ErrorAr = "Invalid email or password." });
            }


            var token = await _tokenService.CreateTokenAsync(applicationUser);
            await _context.SaveChangesAsync();

            return ResultDTO<object>.Success(new { UserId = applicationUser.Id, Token = token, Message = "Logged successfully" });
        }

        public async Task<ResultDTO<object>> Register([FromForm] ApplicationUserRegisterRequestDto registerRequestDto)
        {
            if (registerRequestDto == null)
            {
                return ResultDTO<object>.BadRequest(new ErrorDTO { ErrorAr = "informations not completed" });
            }

            if (string.IsNullOrWhiteSpace(registerRequestDto.Password))
            {
                return ResultDTO<object>.BadRequest(new ErrorDTO { ErrorAr = "incorrect password" });
            }
            if (registerRequestDto.ProfilePicture != null)
            {
                var fileSavingResult = await _fileService.SaveFileAsync(registerRequestDto.ProfilePicture, "Images\\Users");

                if (fileSavingResult == FileServiceResults.UnsupportedFileExtension)
                {
                    return ResultDTO<object>.Failure(
                        new ErrorDTO { ErrorAr = "the extension not allowed" },
                        ResponseStatusCodes.BadRequest
                    );
                }


            }


            var applicationUser = new User
            {
                UserName = registerRequestDto.Name,
                PhoneNumber = registerRequestDto.Phone,
                Email = registerRequestDto.Email,
                UserType = "Client",
                UserCode = GenerateUniqueUserCode(),
                CreatedAt = DateTime.UtcNow
            };


            var identityResult = await _userManager.CreateAsync(applicationUser, registerRequestDto.Password);

            if (!identityResult.Succeeded)
            {
                var errors = identityResult.Errors.Select(e => e.Description).ToList();
                return ResultDTO<object>.BadRequest(new ErrorDTO { ErrorAr = string.Join(" | ", errors) });
            }

            const string defaultRole = "client";
            var roleExists = await _roleManager.RoleExistsAsync(defaultRole);

            if (!roleExists)
            {
                return ResultDTO<object>.Failure(
                    new ErrorDTO { ErrorAr = $"not exit {defaultRole} the role" },
                    ResponseStatusCodes.InternalServerError
                );
            }

            var roleResult = await _userManager.AddToRoleAsync(applicationUser, defaultRole);
            if (!roleResult.Succeeded)
            {
                var roleErrors = roleResult.Errors.Select(e => e.Description).ToList();
                return ResultDTO<object>.Failure(new ErrorDTO { ErrorAr = "faild to add the role" + string.Join(", ", roleErrors) }, ResponseStatusCodes.InternalServerError);
            }

            // 🔹 8. إرجاع النتيجة
            return ResultDTO<object>.Success(new
            {
                UserId = applicationUser.Id
            });
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

        private string GenerateUniqueUserCode()
        {
            return $"USR-{DateTime.UtcNow.Ticks.ToString()[^6..]}"; 
        }
        
        public async Task<ResultDTO<string>> BeWorkerAsync(BeWorkerRequestDTO request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Validate user exists
                var userExists = await _userManager.Users.AnyAsync(u => u.Id == request.UserId);
                if (!userExists)
                {
                    return ResultDTO<string>.NotFound(new ErrorDTO
                    {
                        ErrorAr = "المستخدم غير موجود.",
                        ErrorEn = "User not found."
                    });
                }

                // Validate job exists
                var jobExists = await _context.Jobs.AnyAsync(j => j.Id == request.JobId);
                if (!jobExists)
                {
                    return ResultDTO<string>.BadRequest(new ErrorDTO
                    {
                        ErrorAr = "الوظيفة المحددة غير موجودة.",
                        ErrorEn = "Specified job not found."
                    });
                }

                // Validate city exists
                var cityExists = await _context.Cities.AnyAsync(c => c.Id == request.CityId);
                if (!cityExists)
                {
                    return ResultDTO<string>.BadRequest(new ErrorDTO
                    {
                        ErrorAr = "المدينة المحددة غير موجودة.",
                        ErrorEn = "Specified city not found."
                    });
                }

                // Validate services exist
                var validServicesCount = await _context.Services
                    .CountAsync(s => request.ServicesIds.Contains(s.Id));
                if (validServicesCount != request.ServicesIds.Count)
                {
                    return ResultDTO<string>.BadRequest(new ErrorDTO
                    {
                        ErrorAr = "بعض الخدمات المحددة غير موجودة.",
                        ErrorEn = "Some specified services not found."
                    });
                }

                // Check if worker specification already exists
                var existingWorkerSpec = await _context.WorkerSpecifications
                    .FirstOrDefaultAsync(ws => ws.UserId == request.UserId);

                if (existingWorkerSpec != null)
                {
                    // Update existing worker specification (re-application case)
                    existingWorkerSpec.Bio = request.Bio;
                    existingWorkerSpec.ImageURL = request.PersonalImageURL;
                    existingWorkerSpec.IdentityImageURL = request.IdentityImageURL;
                    existingWorkerSpec.IsCompany = request.IsCompany;
                    existingWorkerSpec.Address = request.Address;
                    existingWorkerSpec.Latitude = request.Latitude;
                    existingWorkerSpec.Longitude = request.Longitude;
                    existingWorkerSpec.JobId = request.JobId;
                    existingWorkerSpec.LivingCityId = request.CityId;
                    existingWorkerSpec.IsApproved = null; // Reset approval status for re-review
                    existingWorkerSpec.ModifiedAt = DateTime.UtcNow;

                    // Update services
                    var services = await _context.Services
                        .Where(s => request.ServicesIds.Contains(s.Id))
                        .ToListAsync();
                    existingWorkerSpec.Services = services;

                    // Update portfolio if provided
                    if (request.PortfolioFilesURL != null && request.PortfolioFilesURL.Any())
                    {
                        // Remove existing portfolio files
                        var existingPortfolio = await _context.WorkerPortfolios
                            .Where(wp => wp.WorkerId == request.UserId)
                            .ToListAsync();
                        _context.WorkerPortfolios.RemoveRange(existingPortfolio);

                        // Add new portfolio files
                        foreach (var fileUrl in request.PortfolioFilesURL)
                        {
                            var portfolio = new WorkerPortfolio
                            {
                                FileURL = fileUrl,
                                WorkerId = request.UserId,
                                CreatedAt = DateTime.UtcNow,
                                ModifiedAt = DateTime.UtcNow
                            };
                            _context.WorkerPortfolios.Add(portfolio);
                        }
                    }
                }
                else
                {
                    // Create new worker specification
                    var workerSpec = new WorkerSpecification
                    {
                        Bio = request.Bio,
                        ImageURL = request.PersonalImageURL,
                        IdentityImageURL = request.IdentityImageURL,
                        IsCompany = request.IsCompany,
                        Address = request.Address,
                        Latitude = request.Latitude,
                        Longitude = request.Longitude,
                        JobId = request.JobId,
                        LivingCityId = request.CityId,
                        UserId = request.UserId,
                        IsApproved = null, // Pending approval
                        CompletedOrders = 0,
                        RateRito = 0.0,
                        CreatedAt = DateTime.UtcNow,
                        ModifiedAt = DateTime.UtcNow
                    };

                    // Add services
                    var services = await _context.Services
                        .Where(s => request.ServicesIds.Contains(s.Id))
                        .ToListAsync();
                    workerSpec.Services = services;

                    _context.WorkerSpecifications.Add(workerSpec);
                    await _context.SaveChangesAsync(); // Save to get the ID

                    // Add portfolio files if provided
                    if (request.PortfolioFilesURL != null && request.PortfolioFilesURL.Any())
                    {
                        foreach (var fileUrl in request.PortfolioFilesURL)
                        {
                            var portfolio = new WorkerPortfolio
                            {
                                FileURL = fileUrl,
                                WorkerId = request.UserId,
                                CreatedAt = DateTime.UtcNow,
                                ModifiedAt = DateTime.UtcNow
                            };
                            _context.WorkerPortfolios.Add(portfolio);
                        }
                    }
                }
                
                // Send notification to all admin users
                var adminUsers = await _context.Users
                    .Where(u => u.UserType == UserType.Admin.ToString())
                    .ToListAsync();

                foreach (var admin in adminUsers)
                {
                    _context.UserNotifications.Add(new UserNotification
                    {
                        UserId = admin.Id,
                        Description =" Worker application submitted by user with ID " + request.UserId,
                        CreatedAt = DateTime.UtcNow,
                        NotificationTypeId = 1 , // may change this later
                    });
                }
                
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return ResultDTO<string>.Success(new MessageDTO
                {
                    MessageAr = "تم إرسال طلب أن تصبح عاملاً بنجاح. سيتم مراجعة طلبك من قبل المشرف.",
                    MessageEn = "Worker application submitted successfully. Your request will be reviewed by the supervisor."
                });
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return ResultDTO<string>.InternalServerError(new ErrorDTO
                {
                    ErrorAr = "حدث خطأ أثناء معالجة طلبك.",
                    ErrorEn = "An error occurred while processing your request."
                });
            }
        }
    }
}
