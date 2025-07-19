using AutoMapper;
using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.ServiceDTOs.ServiceCategoryDTOs;
using Hoshi.DTOs.ServiceDTOs.ServiceDTOs;
using Microsoft.EntityFrameworkCore;

namespace Hoshi.Repositories.ServiceService
{
    public class ServiceService : IServiceService
    {
        private readonly HoshiDbContext _context;
        private readonly IMapper _mapper;

        public ServiceService(HoshiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ResultDTO<List<ServiceCategoryGetDTO>>> searchServiceAsyn(string serviceName)
        {

            var selectedCategories = await _context.Services.Where(p => p.ServiveName == serviceName).Select(p => p.ServiceCategoryId).ToListAsync();
            var allCat = await _context.ServiceCategories.ToListAsync();
            var resultLST          = new List<ServiceCategoryGetDTO>();
            foreach (var CatId in selectedCategories)
            {
                var targetCat = allCat.Where(p => p.Id == CatId).First();
                var targetMapper = _mapper.Map<ServiceCategoryGetDTO>(targetCat);
                var targetRelatedServiceActive = await _context.Services.Where(p => p.ServiceCategoryId == CatId && p.IsDeleted==false).ToListAsync();
                var targetServices = _mapper.Map<List<ServiceGetDTO>>(targetRelatedServiceActive);
                targetMapper.Services = targetServices;
                resultLST.Add(targetMapper);

            }
            return ResultDTO<List<ServiceCategoryGetDTO>>.Success(resultLST);   
        }
    }
}
