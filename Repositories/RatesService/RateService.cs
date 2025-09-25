using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.GlobalDTOs.RateDTOs;
using Hoshi.Enums;
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
            var result = await CheckUsersTuyps(dto.ClientId, dto.WorkerId);
            if (result.Item1 == false)
                return ResultDTO<object>.BadRequest(new ErrorDTO
                {
                    ErrorAr = result.Item2,
                    ErrorEn = result.Item3,
                });

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
            var result = await CheckUsersTuyps(dto.ClientId, dto.WorkerId);
            if (result.Item1 == false)
                return ResultDTO<object>.BadRequest(new ErrorDTO
                {
                    ErrorAr = result.Item2,
                    ErrorEn = result.Item3,
                });

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
            // get the rates of this worker and to get it will chick if it from clients
            var rates = await context.Set<Rate>()
                .Where(r => r.WorkerId == workerId && r.FromClient == true)
                .Select(r => r.RateValue)
                .ToListAsync();

            if (rates.Count == 0)
                return 0;

            return rates.Average();
        }

        public async Task<double> GetClientAverageRateAsync(int clientId)
        {
            // get the rates of this client and to get it will chick if it from workers
            var rates = await context.Set<Rate>()
                .Where(r => r.ClientId == clientId && r.FromClient == false)
                .Select(r => r.RateValue)
                .ToListAsync();

            if (rates.Count == 0)
                return 0;

            return rates.Average();
        }

        public async Task<Tuple<bool, string, string>> CheckUsersTuyps(int clientId, int workerId)
        {
            var clientData = await context.Users.FindAsync(clientId);

            if(clientData == null || clientData.UserType != UserType.Client.ToString())
                return new Tuple<bool, string, string>(false, "هذا ليس معرف عميل.", "This is not a client Id.");
        
            var workerData = await context.Users.FindAsync(workerId);

            if(workerData == null || workerData.UserType != UserType.Worker.ToString())
                return new Tuple<bool, string, string>(false,  "هذا ليس معرف عامل.", "This is not a worker Id.");

            return new Tuple<bool, string, string>(true, "كل شيء جيد.", "All good.");
        }
    }
}
