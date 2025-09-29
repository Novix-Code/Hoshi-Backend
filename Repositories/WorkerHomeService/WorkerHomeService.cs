using AutoMapper;
using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.OrderDTOs.OrderDTOs;
using Hoshi.DTOs.PromotionDTOs.PromotionDTOs;
using Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerHomeDTOs;
using Hoshi.Enums;
using Hoshi.Repositories.PromotionService;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Hoshi.Repositories.WorkerHomeService
{
    /// <summary>
    /// Provides worker home data such as available offers, upcoming orders,
    /// and nearby orders based on geospatial distance. Also supports order search filters.
    /// </summary>
    public class WorkerHomeService : IWorkerHomeService
    {
        private readonly HoshiDbContext _hoshiDbContext;
        private readonly IMapper _mapper;
        private readonly IPromotionService promotionService;

        public WorkerHomeService(HoshiDbContext hoshiDbContext, IMapper mapper, IPromotionService promotionService)
        {
            _hoshiDbContext = hoshiDbContext;
            _mapper = mapper;
            this.promotionService = promotionService;
        }
        /// <summary>
        /// Get worker home snapshot including offers, upcoming and nearby orders (sorted by distance).
        /// </summary>
        public async Task<ResultDTO<object>> GetWorkerHomeAsync(int workerId)
        {
            var workerExists = await _hoshiDbContext.Users.FindAsync(workerId);

            if (workerExists is null)
            {
                return ResultDTO<object>.NotFound(new ErrorDTO
                {
                    ErrorAr = "العامل غير موجود.",
                    ErrorEn = "Worker not found."
                });
            }

            var workerSpec = await _hoshiDbContext.WorkerSpecifications
                .Include(ws => ws.User)
                .Include(ws => ws.LivingCity)
                .FirstOrDefaultAsync(ws => ws.UserId == workerId);

            if (workerSpec == null)
            {
                return ResultDTO<object>.NotFound(new ErrorDTO
                {
                    ErrorAr = "لا توجد بيانات مواصفات للعامل.",
                    ErrorEn = "Worker specifications not found."
                });
            }
            
            // this line to get all promotions that assigned to All users or Workers
            var workerPromotions = await promotionService.NoneTakenPromotions(workerId, false);

            var upcomingOrdersStatusSet = new HashSet<string>
            {
                OrderStatus.InProgress.ToString(),
                OrderStatus.Assigned.ToString()
            };

            // Get the orders that assigned to the current Worker and Order them from the neerest to be serviced
            var upcomingOrders = await _hoshiDbContext.Orders
                .Include(i => i.Client)
                .Include(i => i.Service)
                .Include(i => i.OrderImages)
                .Where(o => o.WorkerId == workerId && upcomingOrdersStatusSet.Contains(o.OrderStatus))
                .OrderByDescending(o => o.ServicingDateTime)
                .ToListAsync();

            // Select all ids of servics that the worker related to it
            var workerServices = _hoshiDbContext.WorkerServices.Where(ws => ws.WorkerId == workerId).Select(ws => ws.ServiceId).ToHashSet<int>();

            // Get all published orders that matchs worker servcies
            var publishedOrders = await _hoshiDbContext.Orders
                .Include(i => i.Client)
                .Include(i => i.Service)
                .Include(i => i.OrderImages)
                .Where(o => o.OrderStatus == OrderStatus.Published.ToString() && workerServices.Contains(o.ServiceId))
                .Where(o => !_hoshiDbContext.Offers.Where(of => of.OrderId == o.Id && of.WorkerId == workerId).Any())
                .ToListAsync();

            var nearbyOrders = publishedOrders
                .OrderBy(o => GetDistance(
                        workerSpec.LivingCity!.Latitude, 
                        workerSpec.LivingCity!.Longitude, 
                        o.Latitude, 
                        o.Longitude
                    )
                )
                .ThenBy(o => o.CreatedAt)
                .ToList();

            var data = new
            {
                AvailablePromotions = _mapper.Map<List<PromotionGetDTO>>(workerPromotions),
                UpcomingOrders = _mapper.Map<List<OrderBasicDTO>>(upcomingOrders),
                NearbyOrders = _mapper.Map<List<OrderBasicDTO>>(nearbyOrders)
            };

            return ResultDTO<object>.Success(data);
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

        /// <summary>
        /// Search published orders, filter by service/city names, order by creation date descending.
        /// </summary>
        public async Task<ResultDTO<List<OrderBasicDTO>>> SearchOrdersAsync(OrderSearchRequestDto searchRequest)
        {
            if (searchRequest.CityName.IsNullOrEmpty() && searchRequest.ServiceName.IsNullOrEmpty())
                return ResultDTO<List<OrderBasicDTO>>.BadRequest(new ErrorDTO
                {
                    ErrorAr = "لا يمكن البحث بقيم فارغة.",
                    ErrorEn = "Can not search with Null Or Empty values."
                });

            var query = _hoshiDbContext.Orders
                .Include(o => o.Service)
                    .ThenInclude(s => s.ServiceCategory)
                .Include(o => o.City)
                .Include(i => i.Client)
                .Include(i => i.Service)
                .Include(i => i.OrderImages)
                .Where(o => o.OrderStatus == OrderStatus.Published.ToString());

            // Filter by service name if provided
            if (!string.IsNullOrEmpty(searchRequest.ServiceName))
            {
                query = query
                    .Where(o => o.Service.ServiveName.Contains(searchRequest.ServiceName));
            }

            // Filter by city name if provided
            if (!string.IsNullOrEmpty(searchRequest.CityName))
            {
                query = query.Where(o => o.City.CityName.Contains(searchRequest.CityName));
            }

            var orders = await query
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            var mappedOrders = _mapper.Map<List<OrderBasicDTO>>(orders);
            return ResultDTO<List<OrderBasicDTO>>.Success(mappedOrders);

        }


 
    }
}