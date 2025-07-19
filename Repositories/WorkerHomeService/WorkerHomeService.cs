using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerHomeDTOs;
using Hoshi.Enums;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Hoshi.DTOs.OrderDTOs.OfferDTOs;
using Hoshi.DTOs.OrderDTOs.OrderDTOs;

namespace Hoshi.Repositories.WorkerHomeService
{
    public class WorkerHomeService : IWorkerHomeService
    {
        private readonly HoshiDbContext _hoshiDbContext;
        private readonly IMapper _mapper;
        public WorkerHomeService(HoshiDbContext hoshiDbContext, IMapper mapper)
        {
            _hoshiDbContext = hoshiDbContext;
            _mapper = mapper;
        }
        public async Task<ResultDTO<WorkerOrderDetailsDto>> GetWorkerHomeAsync(int workerId)
        {
            var workerExists = await _hoshiDbContext.Users.AnyAsync(u => u.Id == workerId);

            if (!workerExists)
            {
                return ResultDTO<WorkerOrderDetailsDto>.NotFound(new ErrorDTO
                {
                    ErrorAr = "العامل غير موجود.",
                    ErrorEn = "Worker not found."
                });
            }

            var workerSpec = await _hoshiDbContext.WorkerSpecifications
                .Include(ws => ws.User)
                .FirstOrDefaultAsync(ws => ws.UserId == workerId);

            if (workerSpec == null)
            {
                return ResultDTO<WorkerOrderDetailsDto>.NotFound(new ErrorDTO
                {
                    ErrorAr = "لا توجد بيانات مواصفات للعامل.",
                    ErrorEn = "Worker specifications not found."
                });
            }

            var unconsumedOffers = await _hoshiDbContext.Offers
                .Where(o => o.WorkerId == workerId &&
                            (o.OfferStatus == OfferStatus.Waitting || o.OfferStatus == OfferStatus.Accepted) &&
                            !o.IsDeleted)
                .ToListAsync();

            var upcomingOrders = await _hoshiDbContext.Orders
                .Where(o => o.WorkerId == workerId && o.ServicingDateTime > DateTime.UtcNow)
                .OrderBy(o => o.ServicingDateTime)
                .ToListAsync();

            var publishedOrders = upcomingOrders
                .Where(o => o.OrderStatus == OrderStatus.Published);

            var nearbyOrders = publishedOrders
                .OrderBy(o => GetDistance(workerSpec.Latitude, workerSpec.Longitude, o.Latitude, o.Longitude))
                .ThenBy(o => o.CreatedAt)
                .ToList();

            var data = new WorkerOrderDetailsDto
            {
                AvailableOffers = _mapper.Map<List<OfferGetDTO>>(unconsumedOffers),
                UpcomingOrders = _mapper.Map<List<OrderGetDTO>>(upcomingOrders),
                NearbyOrders = _mapper.Map<List<OrderGetDTO>>(nearbyOrders)
            };

            return ResultDTO<WorkerOrderDetailsDto>.Success(data);
        }

        private double GetDistance(double lat1, double lon1, double lat2, double lon2)
        {
            double R = 6371; 
            double dLat = ToRadians(lat2 - lat1);
            double dLon = ToRadians(lon2 - lon1);
            double a =
                Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c; // in KM
        }

        private double ToRadians(double angle)
        {
            return angle * (Math.PI / 180);
        }

        public async Task<ResultDTO<List<OrderGetDTO>>> SearchOrdersAsync(OrderSearchRequestDto searchRequest)
        {

            var query = _hoshiDbContext.Orders
                .Include(o => o.Service)
                    .ThenInclude(s => s.ServiceCategory)
                .Include(o => o.City)
                .Where(o => o.OrderStatus == OrderStatus.Published);

            // Filter by service name if provided
            if (!string.IsNullOrEmpty(searchRequest.ServiceName))
            {
                query = query.Where(o => o.Service.ServiveName.Contains(searchRequest.ServiceName));
            }

            // Filter by city name if provided
            if (!string.IsNullOrEmpty(searchRequest.CityName))
            {
                query = query.Where(o => o.City.CityName.Contains(searchRequest.CityName));
            }

            var orders = await query
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            var mappedOrders = _mapper.Map<List<OrderGetDTO>>(orders);
            return ResultDTO<List<OrderGetDTO>>.Success(mappedOrders);

        }


 
    }
}