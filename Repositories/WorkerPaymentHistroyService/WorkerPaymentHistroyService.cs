using AutoMapper;
using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerPaymentHistroyDTOs;
using Hoshi.Models.UserModels.WorkerModels;
using Hoshi.Repositories.FileServiceFold;
using Microsoft.EntityFrameworkCore;

namespace Hoshi.Repositories.WorkerPaymentHistroyService
{
    public class WorkerPaymentHistroyService : IWorkerPaymentHistroyService
    {
        private readonly IMapper mapper;
        private readonly HoshiDbContext context;
        private readonly IFileService fileService;

        public WorkerPaymentHistroyService(
            IMapper mapper,
            HoshiDbContext context,
            IFileService fileService
        )
        {
            this.mapper = mapper;
            this.context = context;
            this.fileService = fileService;
        }

        public async Task<ResultDTO<WorkerPaymentHistroyGetDTO>> AddService(WorkerPaymentHistroyPostDTO postDTO)
        {
            try
            {
                WorkerPaymentHistroy payment = mapper.Map<WorkerPaymentHistroy>(postDTO);

                var imageResult = await fileService.SaveFileAsync(postDTO.BillImage, "images\\paymentbills");

                if (imageResult.Item1 is false)
                    return ResultDTO<WorkerPaymentHistroyGetDTO>.BadRequest(new ErrorDTO()
                    {
                        ErrorAr = "حدثت مشكلة في عملية الاضافة.",
                        ErrorEn = imageResult.Item2.ToString()
                    });

                payment.BillImageURL = imageResult.Item2;

                await context.Set<WorkerPaymentHistroy>().AddAsync(payment);

                await context.SaveChangesAsync();

                WorkerPaymentHistroy finalResult = await context.Set<WorkerPaymentHistroy>()
                    .Include(nameof(WorkerPaymentHistroy.Worker))
                    .FirstAsync(s => s.Id == payment.Id);

                return ResultDTO<WorkerPaymentHistroyGetDTO>.Success(mapper.Map<WorkerPaymentHistroyGetDTO>(finalResult));
            }
            catch (Exception ex)
            {
                return ResultDTO<WorkerPaymentHistroyGetDTO>.BadRequest(new ErrorDTO()
                {
                    ErrorAr = "حدثت مشكلة في عملية الاضافة.",
                    ErrorEn = ex.InnerException is null ? ex.InnerException!.Message : ex.Message
                });
            }
        }

        public async Task<ResultDTO<WorkerPaymentHistroyGetDTO>> UpdateService(WorkerPaymentHistroyPutDTO putDTO)
        {
            try
            {
                WorkerPaymentHistroy? payment = await context.Set<WorkerPaymentHistroy>()
                    .Include(nameof(WorkerPaymentHistroy.Worker))
                    .FirstAsync(s => s.Id == putDTO.Id);

                if (payment == null)
                    return ResultDTO<WorkerPaymentHistroyGetDTO>.BadRequest(new ErrorDTO()
                    {
                        ErrorAr = "هذا المعرف غير موجود.",
                        ErrorEn = "This id not exist."
                    });

                if (payment.IsApproved is true)
                    return ResultDTO<WorkerPaymentHistroyGetDTO>.BadRequest(new ErrorDTO()
                    {
                        ErrorAr = "لقد تم الموافقة على التحويل ولا يمكن تعديله.",
                        ErrorEn = "The transfer has been approved and cannot be modified."
                    });

                if (putDTO.BillImage != null)
                {
                    fileService.DeleteFile(payment.BillImageURL);

                    var imageResult = await fileService.SaveFileAsync(putDTO.BillImage, "images\\paymentbills");

                    if (imageResult.Item1 is false)
                        return ResultDTO<WorkerPaymentHistroyGetDTO>.BadRequest(new ErrorDTO()
                        {
                            ErrorAr = "حدثت مشكلة في عملية الاضافة.",
                            ErrorEn = imageResult.Item2.ToString()
                        });

                    payment.BillImageURL = imageResult.Item2;
                }

                mapper.Map(putDTO, payment);

                await context.SaveChangesAsync();

                return ResultDTO<WorkerPaymentHistroyGetDTO>.Success(mapper.Map<WorkerPaymentHistroyGetDTO>(payment));
            }
            catch (Exception ex)
            {
                return ResultDTO<WorkerPaymentHistroyGetDTO>.BadRequest(new ErrorDTO()
                {
                    ErrorAr = "حدثت مشكلة في عملية الاضافة.",
                    ErrorEn = ex.InnerException is null ? ex.InnerException!.Message : ex.Message
                });
            }
        }
    }
}
