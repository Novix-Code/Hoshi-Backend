using AutoMapper;
using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.ServiceDTOs.ServiceCategoryDTOs;
using Hoshi.DTOs.ServiceDTOs.ServiceDTOs;
using Hoshi.Models.ServiceModels;
using Hoshi.Repositories.FileServiceFold;
using Microsoft.EntityFrameworkCore;

namespace Hoshi.Repositories.ServiceService
{
    /// <summary>
    /// Aggregates service-related operations including creation/update with image handling,
    /// service search, payments/complaints/statistics dashboards, and analytics projections.
    /// </summary>
    public class ServiceService : IServiceService
    {
        private readonly HoshiDbContext _context;
        private readonly IMapper _mapper;
        private readonly IFileService fileService;

        public ServiceService(
            HoshiDbContext context,
            IMapper mapper,
            IFileService fileService
        )
        {
            _context = context;
            _mapper = mapper;
            this.fileService = fileService;
        }

        /// <summary>
        /// Search services by exact name and group them under their categories with active services.
        /// </summary>
        public async Task<ResultDTO<List<ServiceCategoryGetDTO>>> searchServiceAsyn(string serviceName)
        {

            var selectedCategories = await _context.Services.Where(p => p.ServiveName == serviceName).Select(p => p.ServiceCategoryId).ToListAsync();
            var allCat = await _context.ServiceCategories.ToListAsync();
            var resultLST = new List<ServiceCategoryGetDTO>();
            foreach (var CatId in selectedCategories)
            {
                var targetCat = allCat.Where(p => p.Id == CatId).First();
                var targetMapper = _mapper.Map<ServiceCategoryGetDTO>(targetCat);
                var targetRelatedServiceActive = await _context.Services.Where(p => p.ServiceCategoryId == CatId && p.IsDeleted == false).ToListAsync();
                var targetServices = _mapper.Map<List<ServiceBasicDTO>>(targetRelatedServiceActive);
                targetMapper.Services = targetServices;
                resultLST.Add(targetMapper);

            }
            return ResultDTO<List<ServiceCategoryGetDTO>>.Success(resultLST);
        }

        /// <summary>
        /// Create a new service and persist its image via FileService.
        /// </summary>
        public async Task<ResultDTO<ServiceGetDTO>> AddService(ServicePostDTO postDTO)
        {
            try
            {
                Service service = _mapper.Map<Service>(postDTO);

                var imageResult = await fileService.SaveFileAsync(postDTO.Image, "images\\services");

                if (imageResult.Item1 is false)
                    return ResultDTO<ServiceGetDTO>.BadRequest(new ErrorDTO()
                    {
                        ErrorAr = "حدثت مشكلة في عملية الاضافة.",
                        ErrorEn = imageResult.Item2.ToString()
                    });

                service.ImageURL = imageResult.Item2;

                await _context.Set<Service>().AddAsync(service);

                await _context.SaveChangesAsync();

                Service finalResult = await _context.Set<Service>()
                    .Include(nameof(Service.ServiceCategory))
                    .FirstAsync(s => s.Id == service.Id);

                return ResultDTO<ServiceGetDTO>.Success(_mapper.Map<ServiceGetDTO>(finalResult));
            }
            catch (Exception ex)
            {
                return ResultDTO<ServiceGetDTO>.BadRequest(new ErrorDTO()
                {
                    ErrorAr = "حدثت مشكلة في عملية الاضافة.",
                    ErrorEn = ex.InnerException is null ? ex.InnerException!.Message : ex.Message
                });
            }
        }

        /// <summary>
        /// Update a service and optionally replace its image (old image is deleted first).
        /// </summary>
        public async Task<ResultDTO<ServiceGetDTO>> UpdateService(ServicePutDTO putDTO)
        {
            try
            {
                Service? service = await _context.Set<Service>()
                    .Include(nameof(Service.ServiceCategory))
                    .FirstAsync(s => s.Id == putDTO.Id);

                if (service == null)
                    return ResultDTO<ServiceGetDTO>.BadRequest(new ErrorDTO()
                    {
                        ErrorAr = "هذا المعرف غير موجود.",
                        ErrorEn = "This id not exist."
                    });

                if (putDTO.Image != null)
                {
                    fileService.DeleteFile(service.ImageURL);

                    var imageResult = await fileService.SaveFileAsync(putDTO.Image, "images\\services");

                    if (imageResult.Item1 is false)
                        return ResultDTO<ServiceGetDTO>.BadRequest(new ErrorDTO()
                        {
                            ErrorAr = "حدثت مشكلة في عملية الاضافة.",
                            ErrorEn = imageResult.Item2.ToString()
                        });

                    service.ImageURL = imageResult.Item2;
                }

                _mapper.Map(putDTO, service);

                await _context.SaveChangesAsync();

                return ResultDTO<ServiceGetDTO>.Success(_mapper.Map<ServiceGetDTO>(service));
            }
            catch (Exception ex)
            {
                return ResultDTO<ServiceGetDTO>.BadRequest(new ErrorDTO()
                {
                    ErrorAr = "حدثت مشكلة في عملية الاضافة.",
                    ErrorEn = ex.InnerException is null ? ex.InnerException!.Message : ex.Message
                });
            }
        }
    }
}
