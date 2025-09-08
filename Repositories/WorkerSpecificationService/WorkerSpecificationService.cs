using AutoMapper;
using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.ServiceDTOs.ServiceDTOs;
using Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerPortfolioDTOs;
using Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerSpecificationDTOs;
using Hoshi.Models.ServiceModels;
using Hoshi.Models.UserModels.WorkerModels;
using Hoshi.Repositories.FileServiceFold;
using Hoshi.Repositories.UserService;
using Microsoft.EntityFrameworkCore;

namespace Hoshi.Repositories.WorkerSpecificationService
{
    /// <summary>
    /// Provides read/update operations for worker specifications including images, portfolios, and services.
    /// </summary>
    public class WorkerSpecificationService : IWorkerSpecificationService
    {
        private readonly HoshiDbContext context;
        private readonly IMapper mapper;
        private readonly IFileService fileService;
        private readonly IUserService userService;

        public WorkerSpecificationService(
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
        /// Get worker specification by user id with related user, job, city, portfolios and services.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ResultDTO<WorkerSpecificationGetDTO>> GetWorkerById(int userId)
        {

            ErrorDTO error = new ErrorDTO();

            try
            {
                WorkerSpecification? workerSpecification = await context.Set<WorkerSpecification>()
                    .Include(p=>p.User)
                    .Include(j=>j.Job)
                    .Include(l=>l.LivingCity)
                    .FirstOrDefaultAsync( x => x.UserId == userId);

                if (workerSpecification == null)
                {
                    error.ErrorAr = "لا يوجد تفاصيل لهذا المستخدم.";
                    error.ErrorEn = "There is no Specification for this User.";

                    return ResultDTO<WorkerSpecificationGetDTO>.BadRequest(error);
                }

                var wsDto = mapper.Map<WorkerSpecificationGetDTO>(workerSpecification);

                List<WorkerPortfolio> workerPortfolio = await context.Set<WorkerPortfolio>()
                    .Where( x => x.WorkerId == userId)
                    .ToListAsync();

                wsDto.Portfolios = mapper.Map<List<WorkerPortfolioBasicDTO>>(workerPortfolio);


                List<WorkerService> workerServices = await context.Set<WorkerService>()
                    .Include(nameof(WorkerService.Service))
                    .Where(x => x.WorkerId == userId)
                    .ToListAsync();

                List<Service> services = workerServices.Select(w => w.Service).ToList()!;

                wsDto.Services = mapper.Map<List<ServiceBasicDTO>>(services);

                return ResultDTO<WorkerSpecificationGetDTO>.Success(wsDto);
            }
            catch (Exception ex)
            {
                error.ErrorAr = "يوجد مشكلة في عملية العرض.";
                error.ErrorEn = "There is a problem in Getting proccess.";

                return ResultDTO<WorkerSpecificationGetDTO>.InternalServerError(error, ex.InnerException!.Message);
            }
        }

        /// <summary>
        /// Update worker specification: personal image, portfolio files, and services.
        /// </summary>
        /// <param name="putDTO"></param>
        /// <returns></returns>
        public async Task<ResultDTO<WorkerSpecificationGetDTO>> UpdateWorker(WorkerSpecificationPutDTO putDTO)
        {
            ErrorDTO error = new ErrorDTO();

            using var transaction = await context.Database.BeginTransactionAsync();

            try
            {
                // Confirm if specification exists
                WorkerSpecification? specification = await context.Set<WorkerSpecification>().FindAsync(putDTO.Id);

                if (specification == null)
                    return ResultDTO<WorkerSpecificationGetDTO>.BadRequest(new ErrorDTO()
                    {
                        ErrorAr = "هذا المعرف خاطئ.",
                        ErrorEn = "This is a wrong Id"
                    });

                // Update Personal Image
                if (putDTO.PersonalImage is not null)
                {
                    // Update image by adding a the new one
                    Tuple<bool, string> result = await userService.AddUserImage(specification.UserId, putDTO.PersonalImage, true);

                    // Chekc if it done successfully or not
                    if (result.Item1 is false)
                        return ResultDTO<WorkerSpecificationGetDTO>.BadRequest(new ErrorDTO()
                        {
                            ErrorAr = "يوجد مشكلة في تعديل الصورة الشخصية.",
                            ErrorEn = result.Item2
                        });
                }

                // Update Worker Portfolio
                if (putDTO.PortfolioFiles is not null)
                {
                    // Update image by adding a the new one
                    Tuple<bool, string> result = await AddWorkerPorFiles(specification.UserId, putDTO.PortfolioFiles, true);

                    // Chekc if it done successfully or not
                    if (result.Item1 is false)
                        return ResultDTO<WorkerSpecificationGetDTO>.BadRequest(new ErrorDTO()
                        {
                            ErrorAr = "يوجد مشكلة في تعديل ملفات معرض الاعمال.",
                            ErrorEn = result.Item2
                        });
                }

                // Update Worker Services// Update Worker Services
                if (putDTO.ServicesIds is not null)
                {
                    // Validate services exist
                    var validServicesCount = await context.Services
                        .CountAsync(s => putDTO.ServicesIds.Contains(s.Id));

                    if (validServicesCount != putDTO.ServicesIds.Count)
                    {
                        return ResultDTO<WorkerSpecificationGetDTO>.BadRequest(new ErrorDTO
                        {
                            ErrorAr = "بعض الخدمات المحددة غير موجودة.",
                            ErrorEn = "Some specified services not found."
                        });
                    }

                    // Update services

                    // Get existing worker services
                    var existingServices = await context.WorkerServices
                        .Where(ws => ws.WorkerId == specification.UserId)
                        .ToListAsync();

                    var existingServiceIds = existingServices.Select(ws => ws.ServiceId).ToHashSet();

                    // Calculate differences
                    var requestedServiceIds = putDTO.ServicesIds.ToHashSet();

                    var toRemove = existingServices
                        .Where(ws => !requestedServiceIds.Contains(ws.ServiceId))
                        .ToList();

                    var toAdd = requestedServiceIds
                        .Except(existingServiceIds)
                        .Select(serviceId => new WorkerService
                        {
                            ServiceId = serviceId,
                            WorkerId = specification.UserId,
                            CreatedAt = DateTime.UtcNow
                        })
                        .ToList();

                    // Apply changes
                    if (toRemove.Any())
                        context.WorkerServices.RemoveRange(toRemove);

                    if (toAdd.Any())
                        await context.WorkerServices.AddRangeAsync(toAdd);
                }

                await context.SaveChangesAsync();
                await transaction.CommitAsync();

                return ResultDTO<WorkerSpecificationGetDTO>.Success();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return ResultDTO<WorkerSpecificationGetDTO>.InternalServerError(new ErrorDTO()
                {
                    ErrorAr = "يوجد مشكلة في تعديل ملف العامل.",
                    ErrorEn = ex.InnerException is null ? ex.Message : ex.InnerException.Message,
                });
            }
        }

        /// <summary>
        /// Add or replace worker national ID image.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="image"></param>
        /// <param name="isUpdate"></param>
        /// <returns></returns>
        public async Task<Tuple<bool, string>> AddWorkerIdImg(int userId, IFormFile image, bool isUpdate)
        {
            try
            {
                // Fetch worker data from its id
                WorkerSpecification? worker =
                    await context.Set<WorkerSpecification>().FirstAsync(w => w.UserId == userId);

                // Check if this worker is there or not
                if (worker == null)
                    return new Tuple<bool, string>(false, "Invalid User Id.");

                if (isUpdate)
                {
                    fileService.DeleteFile(worker.IdentityImageURL);
                }

                // User FileService method to save the image to the images\identityimages folder in wwwroot
                var imageResult = await fileService.SaveFileAsync(image, "images\\identityimages");

                // Check if the image saved successfuly
                if (imageResult.Item1)
                {
                    // Add image url to user object data
                    worker.IdentityImageURL = imageResult.Item2;
                    // Update it in db and save changes
                    context.Update(worker);
                    await context.SaveChangesAsync();
                }

                // return the images result in all cases
                return imageResult;
            }
            catch (Exception ex)
            {
                // If any thing happends return the exception
                return new Tuple<bool, string>(false, ex.InnerException!.Message);
            }
        }

        /// <summary>
        /// Add or replace worker portfolio files.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="files"></param>
        /// <param name="isUpdate"></param>
        /// <returns></returns>
        public async Task<Tuple<bool, string>> AddWorkerPorFiles(int userId, List<IFormFile> files, bool isUpdate)
        {
            try
            {
                // Fetch worker data from its id
                WorkerSpecification? worker =
                    await context.Set<WorkerSpecification>().FirstAsync(w => w.UserId == userId);

                // Check if this worker is there or not
                if (worker == null)
                    return new Tuple<bool, string>(false, "Invalid User Id.");

                if (isUpdate)
                {
                    // Remove existing portfolio files
                    var existingPortfolio = await context.WorkerPortfolios
                        .Where(wp => wp.WorkerId == userId)
                        .ToListAsync();
                    context.WorkerPortfolios.RemoveRange(existingPortfolio);

                    // Delete old portfolio files
                    foreach (var portfolio in existingPortfolio)
                    {
                        fileService.DeleteFile(portfolio.FileURL);
                    }
                }

                foreach (var file in files)
                {
                    var fileResult = await fileService.SaveFileAsync(file, "files\\portfolios");

                    if (fileResult.Item1)
                    {
                        WorkerPortfolio portfolio = new WorkerPortfolio()
                        {
                            FileURL = fileResult.Item2,
                            WorkerId = userId,
                            CreatedAt = DateTime.UtcNow,
                        };

                        await context.Set<WorkerPortfolio>().AddAsync(portfolio);
                    }
                }

                await context.SaveChangesAsync();

                return new Tuple<bool, string>(true, "Add all Files successfully.");
            }
            catch (Exception ex)
            {
                // If any thing happends return the exception
                return new Tuple<bool, string>(false, ex.InnerException!.Message);
            }
        }
    }
}
