using AutoMapper;
using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.OrderDTOs.OrderImageDTOs;
using Hoshi.Models.OrderModels;
using Hoshi.Repositories.FileServiceFold;
using Microsoft.IdentityModel.Tokens;

namespace Hoshi.Repositories.OrderImageService
{
    /// <summary>
    /// Handles saving order images to storage and persisting their records.
    /// </summary>
    public class OrderImageService : IOrderImageService
    {
        private readonly IMapper mapper;
        private readonly HoshiDbContext context;
        private readonly IFileService fileService;

        public OrderImageService(
            IMapper mapper, 
            HoshiDbContext context,
            IFileService fileService
        )
        {
            this.mapper = mapper;
            this.context = context;
            this.fileService = fileService;
        }

        /// <summary>
        /// Save images for a given order and return the created image DTOs.
        /// </summary>
        public async Task<ResultDTO<List<OrderImageGetDTO>>> AddImages(int orderId, List<IFormFile> images)
        {

            ErrorDTO error = new();
            MessageDTO message = new();

            if (images.IsNullOrEmpty())
            {
                error.ErrorAr = "لا يمكن ان يكون العنصر فارغ.";
                error.ErrorEn = "Object can not be null.";

                return ResultDTO<List<OrderImageGetDTO>>.BadRequest(error);
            }

            try
            {
                List<OrderImage> orderImages = new();

                foreach (IFormFile image in images)
                {
                    var result = await fileService.SaveFileAsync(image, "images\\orders");

                    if (result.Item1 is false)
                    {
                        error.ErrorAr = "خطاء في الصورة.";
                        error.ErrorEn = result.Item2;

                        return ResultDTO<List<OrderImageGetDTO>>.BadRequest(error);
                    }

                    orderImages.Add(mapper.Map<OrderImage>(new OrderImagePostDTO
                    {
                        ImageURL = result.Item2,
                        OrderId = orderId
                    }));
                }

                await context.Set<OrderImage>().AddRangeAsync(orderImages);

                await context.SaveChangesAsync();

                return ResultDTO<List<OrderImageGetDTO>>.Success(mapper.Map<List<OrderImageGetDTO>>(orderImages));
            }
            catch (Exception ex)
            {
                error.ErrorAr = "يوجد مشكلة في عملية الاضافة.";
                error.ErrorEn = "There is a problem in Adding proccess.";

                return ResultDTO<List<OrderImageGetDTO>>.InternalServerError(error, ex.InnerException!.Message);
                // Suggested improvement (wrap in transaction to ensure all-or-nothing for multi-file):
                // using var tx = await context.Database.BeginTransactionAsync();
                // try { ... await tx.CommitAsync(); } catch { await tx.RollbackAsync(); throw; }
            }
        }
    }
}
