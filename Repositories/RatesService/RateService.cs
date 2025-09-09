using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.GlobalDTOs.RateDTOs;
using Hoshi.Models.GlobalModels;
using Microsoft.EntityFrameworkCore;

namespace Hoshi.Repositories.RatesService
{
    public class RateService : IRateService

    {
        private readonly HoshiDbContext context;
        public RateService(HoshiDbContext context)
        {
            this.context = context;
        }

        public async Task<ResultDTO<object>> AddRateForClient(RatePostDTO dto)  
        {
            // 1. Get details about client from client specification
            var clientSpecifics = await context.ClientSpecifications
                                        .FirstOrDefaultAsync(c=>c.UserId == dto.ClientId);
            if (clientSpecifics == null)
                return ResultDTO<object>.NotFound(new ErrorDTO
                {
                    ErrorEn= "Client specification not found",
                    ErrorAr= "البيانات غير مكتمله للعميل"
                });
            // 2. calculate average of rates
            var averageOfRates = await GetClientAverageRateAsync(dto.ClientId);
            // 3. update client specification
            clientSpecifics.RateRito = averageOfRates;
            context.ClientSpecifications.Update(clientSpecifics);
            await context.SaveChangesAsync();   
            return ResultDTO<object>.Success(new
            {
                Success = true,
                Message = "successfully update Rate You can now get the rate from specifcaton for client" 
            }); 
        }

        public async Task<ResultDTO<object>> AddRateForWorker(RatePostDTO dto)
        {
            // 1. get specification for worker 
            var workerSpecifics = await context.WorkerSpecifications
                .FirstOrDefaultAsync(s => s.UserId == dto.WorkerId);
            if(workerSpecifics==null)
                return ResultDTO<object>.NotFound(new ErrorDTO
                {
                    ErrorEn = "Worker specification not found",
                    ErrorAr = "البيانات غير مكتمله للعامل"
                });
            // 2. calculate rate average for worker
            var averageRateForWorker = await GetWorkerAverageRateAsync(dto.WorkerId);
            // 3. update specifications
            workerSpecifics.RateRito = averageRateForWorker;
            context.WorkerSpecifications.Update(workerSpecifics);
            await context.SaveChangesAsync();
            return ResultDTO<object>.Success(new
            {
                Success = true,
                Message = "successfully update Rate You can now get the rate from specifcaton for Worker"
            });

        }

        public async Task<double> GetWorkerAverageRateAsync(int workerId)
        {
            var rates = await context.Set<Rate>()
                .Where(r => r.WorkerId == workerId && r.FromClient == false)
                .Select(r => r.RateValue)
                .ToListAsync();

            if (rates.Count == 0)
                return 0;

            return rates.Average();
        }

        public async Task<double> GetClientAverageRateAsync(int clientId)
        {
            var rates = await context.Set<Rate>()
                .Where(r => r.ClientId == clientId && r.FromClient==true)
                .Select(r => r.RateValue)
                .ToListAsync();

            if (rates.Count == 0)
                return 0;

            return rates.Average();
        }

    }
}
