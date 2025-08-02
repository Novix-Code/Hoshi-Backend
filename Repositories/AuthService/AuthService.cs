using AutoMapper;
using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.UserDTOs.UserDTOs;
using Hoshi.DTOs.UserDTOs.UserRegistiration;
using Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerSpecificationDTOs;
using Hoshi.Enums;
using Hoshi.Models.GlobalModels;
using Hoshi.Models.UserModels;
using Hoshi.Models.UserModels.Resets;
using Hoshi.Models.UserModels.WorkerModels;
using Hoshi.Repositories.FileServiceFold;
using Hoshi.Repositories.TokenService;
using Hoshi.Repositories.UserService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Hoshi.Repositories.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly IUserService _userService;
        private readonly HoshiDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly ITokenService _tokenService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(
            IMapper mapper,
            IFileService fileService,
            IUserService userService,
            HoshiDbContext context,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole<int>> roleManager,
            ITokenService tokenService,
            IHttpContextAccessor httpContextAccessor
        )
        {
            _mapper = mapper;
            _fileService = fileService;
            _userService = userService;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
            _httpContextAccessor = httpContextAccessor;

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

            var logoutResult = await Logout();
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
            var applicationUser = await _userManager.FindByIdAsync(userEditRequestDto.Id.ToString());

            if (applicationUser is null)
                return ResultDTO<string>.Failure(new ErrorDTO(), ResponseStatusCodes.BadRequest);

            applicationUser.FullName = userEditRequestDto.FullName;
            applicationUser.Email = userEditRequestDto.Email;
            applicationUser.PhoneNumber = userEditRequestDto.PhoneNumber;

        
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

        public async Task<ResultDTO<UserGetDTO>> Login(ApplicationUserLoginRequestDto loginRequestDto)
        {
            var applicationUser = await _userManager.FindByEmailAsync(loginRequestDto.Email);
           
            var signInResult = await _signInManager.CheckPasswordSignInAsync(
                applicationUser!, loginRequestDto.Password,false);

            if (applicationUser is null || !signInResult.Succeeded)
                return ResultDTO<UserGetDTO>.BadRequest(new ErrorDTO { 
                    ErrorAr = ".الحساب او كلمة السر خاطئة",
                    ErrorEn = "Invalid email or password."
                });


            var token = await _tokenService.CreateTokenAsync(applicationUser);
            await _context.SaveChangesAsync();

            return ResultDTO<UserGetDTO>.Success(
                _mapper.Map<UserGetDTO>(applicationUser),
                token,
                new MessageDTO
                {
                    MessageAr = "تم تسجيل الدخول بنجاح.",
                    MessageEn = "Login successfully."
                }
            );
        }

        public async Task<ResultDTO<UserGetDTO>> Register(
            UserType userType, 
            ApplicationUserRegisterRequestDto registerRequestDto
        )
        {
            if(userType is UserType.Admin)
            {
                return ResultDTO<UserGetDTO>.BadRequest(
                    new ErrorDTO 
                    { 
                        ErrorAr = "المشرف لا يمكنه انشاء حساب لنفسه.",
                        ErrorEn = "Admin can not Register."
                    }
                );
            }

            if (registerRequestDto == null)
            {
                return ResultDTO<UserGetDTO>.BadRequest(
                    new ErrorDTO
                    {
                        ErrorAr = "البيانات غير مكتملة.",
                        ErrorEn = "Information not completed."
                    }
                );
            }

            if (string.IsNullOrWhiteSpace(registerRequestDto.Password))
            {
                return ResultDTO<UserGetDTO>.BadRequest(
                    new ErrorDTO
                    {
                        ErrorAr = "كلمة السر مكتوبة بشكل غير صحيح.",
                        ErrorEn = "Password written in wrong format."
                    }
                );
            }

            var applicationUser = new User
            {
                UserName = GenerateUniqueUsername(registerRequestDto.FullName),
                FullName = registerRequestDto.FullName,
                PhoneNumber = registerRequestDto.PhoneNumber,
                Email = registerRequestDto.Email,
                UserType = userType.ToString(),
                CreatedAt = DateTime.UtcNow
            };

            try
            {
                var identityResult = await _userManager.CreateAsync(applicationUser, registerRequestDto.Password);

                // Create User code from first to chars of its type and its Id
                applicationUser.UserCode = $"{userType.ToString()[..2].ToUpper()}-{applicationUser.Id:D6}";

                // Update User to add the new value
                _context.Set<User>().Update(applicationUser);

                // Add main role to user
                string role = userType.ToString().ToLower();

                if (!await _roleManager.RoleExistsAsync(role))
                {
                    return ResultDTO<UserGetDTO>.InternalServerError(new ErrorDTO
                    {
                        ErrorAr = "هذا الدور غير متوفر.",
                        ErrorEn = "This Role not available."
                    });
                }

                var roleResult = await _userManager.AddToRoleAsync(applicationUser, role);
                if (!roleResult.Succeeded)
                {
                    var roleErrors = roleResult.Errors.Select(e => e.Description).ToList();
                    return ResultDTO<UserGetDTO>.InternalServerError(
                        new ErrorDTO
                        {
                            ErrorAr = "فشل في اضافة الدور للمستخدم.",
                            ErrorEn = "Faild to add role" + string.Join(", ", roleErrors)
                        }
                    );
                }

                if(userType is UserType.Client)
                {
                    ClientSpecification clientSpecification = new ClientSpecification()
                    {
                        UserId = applicationUser.Id,
                        CreatedAt = DateTime.UtcNow
                    };

                    await _context.Set<ClientSpecification>().AddAsync(clientSpecification);
                }


                var token = await _tokenService.CreateTokenAsync(applicationUser);
                await _context.SaveChangesAsync();

                return ResultDTO<UserGetDTO>.Success(
                    _mapper.Map<UserGetDTO>(applicationUser),
                    token,
                    new MessageDTO
                    {
                        MessageAr = "تم انشاء الحساب بنجاح.",
                        MessageEn = "Registeration successfully completed."
                    }
                );
            }
            catch (DbUpdateException ex)
            {
                return ResultDTO<UserGetDTO>.InternalServerError(
                    new ErrorDTO
                    {
                        ErrorAr = "رقم الهاتف او الحساب مكرر.",
                        ErrorEn = ex.InnerException!.Message
                    }
                );
            }
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

        public async Task<ResultDTO<string>> Logout()
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
                    // Adding User personal image
                    var perImgResult = await AddPersonalImage(
                        existingWorkerSpec.UserId, 
                        request.PersonalImage,
                        new Tuple<bool, string?>(true, existingWorkerSpec.User!.ImageURL)
                    );

                    if (perImgResult.IsSuccess is false)
                        return perImgResult;

                    // Update existing worker specification (re-application case)
                    existingWorkerSpec.Bio = request.Bio;
                    existingWorkerSpec.IsCompany = request.IsCompany;
                    existingWorkerSpec.Address = request.Address;
                    existingWorkerSpec.Latitude = request.Latitude;
                    existingWorkerSpec.Longitude = request.Longitude;
                    existingWorkerSpec.JobId = request.JobId;
                    existingWorkerSpec.LivingCityId = request.CityId;
                    existingWorkerSpec.IsApproved = null; // Reset approval status for re-review
                    existingWorkerSpec.ModifiedAt = DateTime.UtcNow;

                    // Adding Identity image
                    var idImgResult = await AddIdentityImage(
                        request.IdentityImage,
                        new Tuple<bool, string?>(true, existingWorkerSpec.IdentityImageURL)
                    );

                    if(idImgResult.IsSuccess)
                        existingWorkerSpec.IdentityImageURL = idImgResult.Data!;
                    else
                        return idImgResult;

                    // Update services
                    var services = await _context.Services
                        .Where(s => request.ServicesIds.Contains(s.Id))
                        .Select(s => s.Id)
                        .ToListAsync();

                    var oldServices = await _context.WorkerServices
                        .Where(ws => request.ServicesIds.Contains(ws.ServiceId) && ws.WorkerId == request.UserId)
                        .Select(s => s.ServiceId)
                        .ToListAsync();

                    var newServices = services.Except(oldServices);

                    List<WorkerService> workerServices = new();

                    foreach (var serviceId in newServices)
                    {
                        workerServices.Add(new()
                        {
                            ServiceId = serviceId,
                            WorkerId = request.UserId,
                            CreatedAt = DateTime.UtcNow
                        });
                    }

                    await _context.AddRangeAsync(workerServices);

                    // Update portfolio if provided
                    if (request.PortfolioFiles != null && request.PortfolioFiles.Any())
                    {
                        // Remove existing portfolio files
                        var existingPortfolio = await _context.WorkerPortfolios
                            .Where(wp => wp.WorkerId == request.UserId)
                            .ToListAsync();
                        _context.WorkerPortfolios.RemoveRange(existingPortfolio);

                        // Delete old portfolio files
                        foreach (var portfolio in existingPortfolio)
                        {
                            _fileService.DeleteFile(portfolio.FileURL);
                        }

                        // Add new portfolio files
                        foreach (var file in request.PortfolioFiles)
                        {
                            Tuple<bool, string> fileResult =
                                await _fileService.SaveFileAsync(file, "files\\portfolios");

                            // Chekc if it done successfuly or not
                            if (fileResult.Item1 is false)
                                return ResultDTO<string>.BadRequest(new ErrorDTO
                                {
                                    ErrorAr = "يوجد مشكلة في اضافة الملف.",
                                    ErrorEn = fileResult.Item2
                                });
                            else
                            {
                                var portfolio = new WorkerPortfolio
                                {
                                    FileURL = fileResult.Item2,
                                    WorkerId = request.UserId,
                                    CreatedAt = DateTime.UtcNow,
                                    ModifiedAt = DateTime.UtcNow
                                };
                                _context.WorkerPortfolios.Add(portfolio);
                            }
                        }
                    }
                }
                else
                {
                    // Adding User personal image
                    var perImgResult = await AddPersonalImage(
                        request.UserId,
                        request.PersonalImage,
                        new Tuple<bool, string?>(false, null)
                    );

                    if (perImgResult.IsSuccess is false)
                        return perImgResult;

                    // Create new worker specification
                    var workerSpec = new WorkerSpecification
                    {
                        Bio = request.Bio,
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

                    // Adding Identity image
                    var idImgResult = await AddIdentityImage(
                        request.IdentityImage,
                        new Tuple<bool, string?>(false, null)
                    );

                    if (idImgResult.IsSuccess)
                        workerSpec.IdentityImageURL = idImgResult.Data!;
                    else
                        return idImgResult;

                    // Add services
                    var servicesIds = await _context.Services
                        .Where(s => request.ServicesIds.Contains(s.Id))
                        .Select(s => s.Id)
                        .ToListAsync();

                    List<WorkerService> workerServices = new();

                    foreach (var serviceId in servicesIds)
                    {
                        workerServices.Add(new()
                        {
                            ServiceId = serviceId,
                            WorkerId = request.UserId,
                            CreatedAt = DateTime.UtcNow
                        });
                    }

                    await _context.AddRangeAsync(workerServices);

                    await _context.WorkerSpecifications.AddAsync(workerSpec);
                    await _context.SaveChangesAsync(); // Save to get the ID

                    // Add portfolio files if provided
                    if (request.PortfolioFiles != null && request.PortfolioFiles.Any())
                    {
                        foreach (var file in request.PortfolioFiles)
                        {
                            Tuple<bool, string> fileResult =
                                await _fileService.SaveFileAsync(file, "files\\portfolios");

                            // Chekc if it done successfuly or not
                            if (fileResult.Item1 is false)
                                return ResultDTO<string>.BadRequest(new ErrorDTO
                                {
                                    ErrorAr = "يوجد مشكلة في اضافة الملف.",
                                    ErrorEn = fileResult.Item2
                                });
                            else
                            {
                                var portfolio = new WorkerPortfolio
                                {
                                    FileURL = fileResult.Item2,
                                    WorkerId = request.UserId,
                                    CreatedAt = DateTime.UtcNow
                                };
                                _context.WorkerPortfolios.Add(portfolio);
                            }
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

        private async Task<ResultDTO<string>> AddPersonalImage(
            int id, 
            IFormFile image, 
            Tuple<bool, string?> isUpdate
        )
        {
            // Check if this call to update user image and if true will remove last image and add the new one
            if (isUpdate.Item1)
            {
                _fileService.DeleteFile(isUpdate.Item2!);
            }

            // Add personal image to user
            Tuple<bool, string> imageResult =
                await _userService.AddUserImage(id, image, true);

            // Chekc if it done successfuly or not
            if (imageResult.Item1 is false)
                return ResultDTO<string>.BadRequest(new ErrorDTO
                {
                    ErrorAr = "يوجد مشكلة في اضافة الصورة.",
                    ErrorEn = imageResult.Item2
                });
            else
                return ResultDTO<string>.Success();
        }

        private async Task<ResultDTO<string>> AddIdentityImage(
            IFormFile image,
            Tuple<bool, string?> isUpdate
        )
        {
            // Check if this call to update identity image and if true will remove last image and add the new one
            if (isUpdate.Item1)
            {
                _fileService.DeleteFile(isUpdate.Item2!);
            }

            Tuple<bool, string> identityImageResult =
                await _fileService.SaveFileAsync(image, "images\\identityimages");

            // Chekc if it done successfuly or not
            if (identityImageResult.Item1 is false)
                return ResultDTO<string>.BadRequest(new ErrorDTO
                {
                    ErrorAr = "يوجد مشكلة في اضافة الصورة.",
                    ErrorEn = identityImageResult.Item2
                });
            else
                return ResultDTO<string>.Success(identityImageResult.Item2);
        }

        private string GenerateUniqueUsername(string fullName)
        {
            string username = string.Empty;

            var checkLang = new Regex(@"^[a-zA-Z]{3,20}(\s[a-zA-Z]{3,20}){1,4}$");

            if (checkLang.IsMatch(fullName))
            {

                List<string> names = fullName.Split(' ').ToList();

                foreach (string name in names)
                {
                    int length = new Random().Next(1, name.Length);

                    username += name.Substring(0, length);
                }

                int num = new Random().Next(1, 1000);

                username += $"{num:D4}";
            }
            else
            {
                int num = new Random().Next(1, 1000);

                username = $"username{num:D4}";
            }

            return username;
        }
    }
}
