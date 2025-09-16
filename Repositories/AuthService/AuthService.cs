using AutoMapper;
using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.UserDTOs.UserDTOs;
using Hoshi.DTOs.UserDTOs.UserRegistiration;
using Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerSpecificationDTOs;
using Hoshi.Enums;
using Hoshi.Models.UserModels;
using Hoshi.Models.UserModels.Resets;
using Hoshi.Models.UserModels.WorkerModels;
using Hoshi.Repositories.EmailServiceFold;
using Hoshi.Repositories.FileServiceFold;
using Hoshi.Repositories.Hubs;
using Hoshi.Repositories.NotificationService;
using Hoshi.Repositories.TokenService;
using Hoshi.Repositories.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Hoshi.Repositories.AuthService
{
    /// <summary>
    /// Handles user authentication, registration, password reset, profile edits and worker application flow.
    /// <br></br>
    /// Uses ASP.NET Identity, EF Core, and app services (files, notifications, tokens).
    /// </summary>
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
        private readonly INotificationServiceHandler notificationServiceHandler;

        private readonly IHubContext<NotificationHub, INotificationHub> _hubContext;
        private readonly IEmailService _emailService;

        public AuthService(

            IMapper mapper,
            IFileService fileService,
            IUserService userService,
            HoshiDbContext context,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole<int>> roleManager,
            ITokenService tokenService
,
            IHubContext<NotificationHub, INotificationHub> hubContext,

            IEmailService emailService,
            IHttpContextAccessor httpContextAccessor,
            INotificationServiceHandler notificationServiceHandler)

        {
            _mapper = mapper;
            _fileService = fileService;
            _userService = userService;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;


            _tokenService = tokenService;
            _hubContext = hubContext;
            _emailService = emailService;
            _httpContextAccessor = httpContextAccessor;
            this.notificationServiceHandler = notificationServiceHandler;
        }

        public async Task<ResultDTO<string>> CreateResetPasswordTokenAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null) return ResultDTO<string>.BadRequest(new ErrorDTO()
            {
                ErrorAr = "هذا الحساب غير صحيح.",
                ErrorEn = "This Accunt not valid."
            });

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            if (string.IsNullOrEmpty(token)) return ResultDTO<string>.InternalServerError(new ErrorDTO()
            {
                ErrorAr = "يوجد مشكلة في النظام.",
                ErrorEn = "There is a problem in system."
            });

            // Hash the token before storing
            var hashedToken = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(token)));

            // Add this to the Password Reset Requests Table
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
            if (user is null) return ResultDTO<string>.BadRequest(new ErrorDTO()
            {
                ErrorAr = "هذا الحساب غير صحيح.",
                ErrorEn = "This Accunt not valid."
            });

            // Hash the received token for comparison
            var hashedRecievedToken = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(resetPasswordRequestDto.Token)));

            var passwordResetTokenRequestRepo = await _context.PasswordResetRequests.ToListAsync();
            var passwordResetRequest = passwordResetTokenRequestRepo
                .Where(r => r.UserId == user.Id && r.ResetToken == hashedRecievedToken).FirstOrDefault();

            // Validate the token and its expiry
            if (passwordResetRequest is null || passwordResetRequest.ExpiresAt < DateTime.UtcNow)
                return ResultDTO<string>.Failure(new ErrorDTO(), ResponseStatusCodes.BadRequest);

            var resutl = await _userManager.ResetPasswordAsync(user, resetPasswordRequestDto.Token,
                resetPasswordRequestDto.NewPassword);

            if (!resutl.Succeeded)
            {
                var errors = resutl.Errors.Select(e => e.Description).ToList();
                return ResultDTO<string>.Failure(new ErrorDTO(), ResponseStatusCodes.BadRequest);
            }

            // Remove the used token as it is one time use
            passwordResetTokenRequestRepo.Remove(passwordResetRequest);
            await _context.SaveChangesAsync();

            return ResultDTO<string>.Success("Password has been changed successfully");
        }

        /// <summary>
        /// Soft deletes a user account by setting the <c>IsDeleted</c> flag instead of permanently removing the record.
        /// </summary>
        /// <param name="id">The unique identifier of the user to delete.</param>
        /// <remarks>
        /// <para>
        /// Deletion rules enforced:
        /// </para>
        /// <list type="number">
        ///   <item><description>Normal users can delete only their own accounts.</description></item>
        ///   <item><description>Admins cannot delete themselves.</description></item>
        ///   <item><description>Admins cannot delete other admins unless the current user is a super admin.</description></item>
        ///   <item><description>Super admins can delete any account, including admins.</description></item>
        /// </list>
        /// </remarks>

        public async Task<ResultDTO<string>> Delete(string id)
        {
            try
            {
                var applicationUser = await _userManager.FindByIdAsync(id);
                if (applicationUser is null)
                    return ResultDTO<string>.Failure(
                        new ErrorDTO { ErrorEn = "User not found.", ErrorAr = "المستخدم غير موجود." },
                        ResponseStatusCodes.BadRequest
                    );

                // Current logged-in user
                var currentUserId = _userManager.GetUserId(_httpContextAccessor.HttpContext.User);
                var currentUser = await _userManager.FindByIdAsync(currentUserId);

                // Role checks
                var isTargetAdmin = await _userManager.IsInRoleAsync(applicationUser, "admin");
                var isCurrentAdmin = currentUser != null && await _userManager.IsInRoleAsync(currentUser, "admin");
                var isCurrentSuperAdmin = currentUser != null && await _userManager.IsInRoleAsync(currentUser, "superadmin");

                // 1. Normal user can delete only themselves
                if (!isCurrentAdmin && !isCurrentSuperAdmin)
                {
                    if (currentUserId != id)
                    {
                        return ResultDTO<string>.Failure(
                            new ErrorDTO
                            {
                                ErrorEn = "You can only delete your own account.",
                                ErrorAr = "يمكنك حذف حسابك الشخصي فقط."
                            },
                            ResponseStatusCodes.Forbidden
                        );
                    }
                }

                // 2. Admin cannot delete themselves
                if (isCurrentAdmin && currentUserId == id)
                {
                    return ResultDTO<string>.Failure(
                        new ErrorDTO
                        {
                            ErrorEn = "Admins cannot delete themselves.",
                            ErrorAr = "لا يمكن للمشرف حذف نفسه."
                        },
                        ResponseStatusCodes.Forbidden
                    );
                }

                // 3. Admin cannot delete other admins (only super admin can)
                if (isCurrentAdmin && isTargetAdmin && !isCurrentSuperAdmin)
                {
                    return ResultDTO<string>.Failure(
                        new ErrorDTO
                        {
                            ErrorEn = "Admins cannot delete other admins.",
                            ErrorAr = "لا يمكن للمشرف حذف مشرف آخر."
                        },
                        ResponseStatusCodes.Forbidden
                    );
                }

                // 4. Soft delete
                applicationUser.IsDeleted = true;
                applicationUser.ModifiedAt = DateTime.UtcNow;

                var identityResult = await _userManager.UpdateAsync(applicationUser);
                if (!identityResult.Succeeded)
                {
                    var errors = string.Join(", ", identityResult.Errors.Select(e => e.Description));
                    return ResultDTO<string>.Failure(
                        new ErrorDTO { ErrorEn = $"Failed to delete user: {errors}", ErrorAr = "فشل في حذف المستخدم." },
                        ResponseStatusCodes.InternalServerError
                    );
                }

                return ResultDTO<string>.Success(new MessageDTO()
                {
                    MessageAr = "تم حذف الحساب بنجاح.",
                    MessageEn = "Account successfully deleted."
                });
            }
            catch (Exception ex)
            {
                return ResultDTO<string>.InternalServerError(new ErrorDTO
                {
                    ErrorAr = "حدث خطأ أثناء معالجة طلبك.",
                    ErrorEn = $"An error occurred while processing your request. {ex.InnerException?.Message ?? ex.Message}"
                });
            }
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

            if (applicationUser is null || applicationUser.IsDeleted is true)
                return ResultDTO<UserGetDTO>.BadRequest(new ErrorDTO
                {
                    ErrorAr = ".الحساب او كلمة السر خاطئة",
                    ErrorEn = "Invalid email or password."
                });

            var signInResult = await _signInManager.CheckPasswordSignInAsync(
                applicationUser!, loginRequestDto.Password, false);

            if (!signInResult.Succeeded)
                return ResultDTO<UserGetDTO>.BadRequest(new ErrorDTO
                {
                    ErrorAr = ".الحساب او كلمة السر خاطئة",
                    ErrorEn = "Invalid email or password."
                });

            // check if account is Suspended and the reason 
            var checkSuspend = await _context.SuspendedUsers.Where(p => p.UserId == applicationUser.Id).FirstOrDefaultAsync();

            if (checkSuspend is not null)
            {
                var getResoun = await _context.SuspendReasons.FindAsync(checkSuspend.SuspendReasonId);
                return ResultDTO<UserGetDTO>.BadRequest(new ErrorDTO
                {
                    ErrorAr = $"الحساب معلق للسبب التالي : {getResoun.Reason}",
                    ErrorEn = $"Acount is Suspended for : {getResoun.Reason}"
                }
                );
            }

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
            ApplicationUserRegisterRequestDto registerRequestDto,
            bool canAddAdmin = false
        )
        {
            if (userType is UserType.Admin && !canAddAdmin)
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


            // Check if email domain has MX records
            if (!await _emailService.CanConnectToMailServerAsync(registerRequestDto.Email))
            {
                return ResultDTO<UserGetDTO>.BadRequest(
                    new ErrorDTO
                    {
                        ErrorAr = "هذا الحساب غير صحيح، بالرجاء ادخال حساب فعال.",
                        ErrorEn = "This email is invalid, please enter a valid account."
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

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var identityResult = await _userManager.CreateAsync(applicationUser, registerRequestDto.Password);

                // Create User code from first to chars of its type and its Id
                applicationUser.UserCode = $"{userType.ToString()[..2].ToUpper()}-{applicationUser.Id:D6}";

                // Update User to add the new value
                _context.Set<User>().Update(applicationUser);

                // Add main role to user
                string role = userType.ToString();

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

                if (userType is UserType.Client)
                {
                    ClientSpecification clientSpecification = new ClientSpecification()
                    {
                        UserId = applicationUser.Id,
                        CreatedAt = DateTime.UtcNow
                    };

                    await _context.Set<ClientSpecification>().AddAsync(clientSpecification);
                }
                if (userType is UserType.Worker)
                {
                    WorkerSpecification workerSpecification = new WorkerSpecification
                    {
                        UserId = applicationUser.Id,
                        CreatedAt = DateTime.UtcNow
                    };
                    await _context.Set<WorkerSpecification>().AddAsync(workerSpecification);

                    WorkerWallet workerWallet = new WorkerWallet()
                    {
                        WorkerId = applicationUser.Id,
                        CreatedAt = DateTime.UtcNow
                    };
                    await _context.Set<WorkerWallet>().AddAsync(workerWallet);
                }


                var token = await _tokenService.CreateTokenAsync(applicationUser);

                /// Handle Send Notification for admin that there are new worker registered

                if (userType is UserType.Worker)
                {
                    await notificationServiceHandler.sendMessagetoAdmin("عمليه تسجيل عامل جديد", applicationUser.Id);
                }

                var otpResult = await _emailService.SendOTP(applicationUser.Email);

                // Suggested improvement (handle OTP failure gracefully):
                if (!otpResult.IsSuccess)
                    return ResultDTO<UserGetDTO>.BadRequest(otpResult.Error!);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

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
                await transaction.RollbackAsync();
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

        [Authorize(Roles = "admin,client,worker")]
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

                // Get existing worker specification (should always exist now due to default creation)
                var existingWorkerSpec = await _context.WorkerSpecifications
                                                .Include(p => p.User)
                                                .FirstOrDefaultAsync(ws => ws.UserId == request.UserId);

                if (existingWorkerSpec == null)
                {
                    // This should not happen with the new logic, but handle as fallback
                    return ResultDTO<string>.BadRequest(new ErrorDTO
                    {
                        ErrorAr = "لم يتم العثور على بيانات العامل.",
                        ErrorEn = "Worker specification not found."
                    });
                }

                // Check if this is the first time completing the profile (not a re-application)
                bool isFirstTimeCompletion = string.IsNullOrEmpty(existingWorkerSpec.Bio) &&
                                           existingWorkerSpec.JobId == null;

                // Adding User personal image
                var perImgResult = await AddPersonalImage(
                    request.UserId,
                    request.PersonalImage,
                    new Tuple<bool, string?>(
                        !string.IsNullOrEmpty(existingWorkerSpec.User?.ImageURL),
                        existingWorkerSpec.User?.ImageURL
                    )
                );

                if (perImgResult.IsSuccess is false)
                    return perImgResult;

                // Update worker specification
                existingWorkerSpec.Bio = request.Bio;
                existingWorkerSpec.IsCompany = request.IsCompany;
                existingWorkerSpec.Address = request.Address;
                existingWorkerSpec.Latitude = request.Latitude;
                existingWorkerSpec.Longitude = request.Longitude;
                existingWorkerSpec.JobId = request.JobId;
                existingWorkerSpec.LivingCityId = request.CityId;

                // Reset approval status for review (both first time and re-application)
                existingWorkerSpec.IsApproved = null;
                existingWorkerSpec.ModifiedAt = DateTime.UtcNow;

                // Adding Identity image
                var idImgResult = await AddIdentityImage(
                    request.IdentityImage,
                    new Tuple<bool, string?>(
                        !string.IsNullOrEmpty(existingWorkerSpec.IdentityImageURL),
                        existingWorkerSpec.IdentityImageURL
                    )
                );

                if (idImgResult.IsSuccess)
                    existingWorkerSpec.IdentityImageURL = idImgResult.Data!;
                else
                    return idImgResult;

                // Handle Services
                // Get old service Ids for this worker
                var oldServiceIds = await _context.WorkerServices
                    .Where(ws => ws.WorkerId == request.UserId)
                    .Select(ws => ws.ServiceId)
                    .ToListAsync();

                // Requested service Ids
                var requestedServiceIds = request.ServicesIds;

                // Services to remove (old - requested)
                var servicesToRemove = oldServiceIds.Except(requestedServiceIds).ToList();

                // Services to add (requested - old)
                var servicesToAdd = requestedServiceIds.Except(oldServiceIds).ToList();

                // Remove services that are no longer selected
                if (servicesToRemove.Any())
                {
                    var workerServicesToRemove = await _context.WorkerServices
                        .Where(ws => ws.WorkerId == request.UserId && servicesToRemove.Contains(ws.ServiceId))
                        .ToListAsync();

                    _context.WorkerServices.RemoveRange(workerServicesToRemove);
                }

                // Add new services
                if (servicesToAdd.Any())
                {
                    var newWorkerServices = servicesToAdd.Select(serviceId => new WorkerService
                    {
                        WorkerId = request.UserId,
                        ServiceId = serviceId,
                        CreatedAt = DateTime.UtcNow
                    });

                    await _context.WorkerServices.AddRangeAsync(newWorkerServices);
                }

                // Handle Portfolio
                if (request.PortfolioFiles != null && request.PortfolioFiles.Any())
                {
                    // Remove existing portfolio files
                    var existingPortfolio = await _context.WorkerPortfolios
                        .Where(wp => wp.WorkerId == request.UserId)
                        .ToListAsync();

                    if (existingPortfolio.Any())
                    {
                        _context.WorkerPortfolios.RemoveRange(existingPortfolio);

                        // Delete old portfolio files
                        foreach (var portfolio in existingPortfolio)
                        {
                            if (portfolio.FileURL != null)
                                _fileService.DeleteFile(portfolio.FileURL);
                        }
                    }

                    // Add new portfolio files
                    foreach (var file in request.PortfolioFiles)
                    {
                        Tuple<bool, string> fileResult =
                            await _fileService.SaveFileAsync(file, "files/portfolios");

                        // Check if it done successfully or not
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

                // Send notifications
                string notificationMessage = isFirstTimeCompletion ?
                    $"تم تقديم طلب عامل من قبل المستخدم رقم {existingWorkerSpec.UserId} " :
                    $" تم تقديم طلب إعادة تسجيل عامل من قبل المستخدم رقم {existingWorkerSpec.UserId} ";

                await notificationServiceHandler.sendMessagetoAdmin(notificationMessage, request.UserId);
                await notificationServiceHandler.sendMessagetoWorker(6, request.UserId, "تم ارسال طلب ان تصبح عامل");

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return ResultDTO<string>.Success(new MessageDTO
                {
                    MessageAr = "تم إرسال طلب أن تصبح عاملاً بنجاح. سيتم مراجعة طلبك من قبل المشرف.",
                    MessageEn = "Worker application submitted successfully. Your request will be reviewed by the supervisor."
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return ResultDTO<string>.InternalServerError(new ErrorDTO
                {
                    ErrorAr = "حدث خطأ أثناء معالجة طلبك.",
                    ErrorEn = $"An error occurred while processing your request. {ex.InnerException?.Message ?? ex.Message}"
                });
            }
        }

        private async Task<ResultDTO<string>> AddPersonalImage(
            int id,
            IFormFile image,
            Tuple<bool, string?> isUpdate
        )
        {
            try
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

            catch (IOException ioEx)
            {
                return ResultDTO<string>.InternalServerError(new ErrorDTO
                {
                    ErrorAr = "حدث خطأ في حفظ الصورة الشخصية. يرجى المحاولة مرة أخرى.",
                    ErrorEn = $"Error saving personal image. Please try again. {ioEx.InnerException?.Message ?? ioEx.Message}"
                });
            }
            catch (Exception ex)
            {
                return ResultDTO<string>.InternalServerError(new ErrorDTO
                {
                    ErrorAr = "حدث خطأ غير متوقع في حفظ الصورة الشخصية.",
                    ErrorEn = $"Unexpected error saving personal image. {ex.InnerException?.Message ?? ex.Message}"
                });
            }
        }

        private async Task<ResultDTO<string>> AddIdentityImage(
            IFormFile image,
            Tuple<bool, string?> isUpdate
        )
        {
            try
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
            catch (IOException ioEx)
            {
                return ResultDTO<string>.InternalServerError(new ErrorDTO
                {
                    ErrorAr = "حدث خطأ في حفظ الصورة الشخصية. يرجى المحاولة مرة أخرى.",
                    ErrorEn = $"Error saving personal image. Please try again. {ioEx.InnerException?.Message ?? ioEx.Message}"
                });
            }
            catch (Exception ex)
            {
                return ResultDTO<string>.InternalServerError(new ErrorDTO
                {
                    ErrorAr = "حدث خطأ غير متوقع في حفظ الصورة الشخصية.",
                    ErrorEn = $"Unexpected error saving personal image. {ex.InnerException?.Message ?? ex.Message}"
                });
            }
        }

        private string GenerateUniqueUsername(string fullName)
        {
            string username = string.Empty;

            string uniqueId = Guid.NewGuid().ToString("N")[..8]; // First 8 chars

            var checkLang = new Regex(@"^[a-zA-Z]{3,20}(\s[a-zA-Z]{3,20}){1,4}$");

            if (checkLang.IsMatch(fullName))
            {

                List<string> names = fullName.Split(' ').ToList();

                foreach (string name in names)
                {
                    int length = new Random().Next(1, name.Length);

                    username += name.Substring(0, length);
                }

                username += uniqueId;
            }
            else
                username = $"user{uniqueId}";

            return username;
        }

    }
}
