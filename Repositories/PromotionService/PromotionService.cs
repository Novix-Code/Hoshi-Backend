using AutoMapper;
using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.PromotionDTOs.PromotionDTOs;
using Hoshi.Models.PromotionModels;
using Hoshi.Repositories.FileServiceFold;
using Microsoft.EntityFrameworkCore;

namespace Hoshi.Repositories.PromotionService
{
    public class PromotionService : IPromotionService
    {
        private readonly IMapper mapper;
        private readonly HoshiDbContext context;
        private readonly IFileService fileService;

        public PromotionService(
            IMapper mapper,
            HoshiDbContext context,
            IFileService fileService
        )
        {
            this.mapper = mapper;
            this.context = context;
            this.fileService = fileService;
        }


        public async Task<ResultDTO<PromotionGetDTO>> AddPromotion(PromotionPostDTO postDTO)
        {
            try
            {
                Promotion promotion = mapper.Map<Promotion>(postDTO);

                var imageResult = await fileService.SaveFileAsync(postDTO.Image, "images\\promotions");

                if (imageResult.Item1 is false)
                    return ResultDTO<PromotionGetDTO>.BadRequest(new ErrorDTO()
                    {
                        ErrorAr = "حدثت مشكلة في عملية الاضافة.",
                        ErrorEn = imageResult.Item2.ToString()
                    });

                promotion.ImageURL = imageResult.Item2;

                await context.Set<Promotion>().AddAsync(promotion);

                await context.SaveChangesAsync();

                return ResultDTO<PromotionGetDTO>.Success(mapper.Map<PromotionGetDTO>(promotion));
            }
            catch (Exception ex)
            {
                return ResultDTO<PromotionGetDTO>.BadRequest(new ErrorDTO()
                {
                    ErrorAr = "حدثت مشكلة في عملية الاضافة.",
                    ErrorEn = ex.InnerException is null ? ex.InnerException!.Message : ex.Message
                });
            }
        }

        public async Task<ResultDTO<PromotionGetDTO>> UpdatePromotion(PromotionPutDTO putDTO)
        {
            try
            {
                Promotion? promotion = await context.Set<Promotion>()
                    .FirstAsync(s => s.Id == putDTO.Id);

                if (promotion == null)
                    return ResultDTO<PromotionGetDTO>.BadRequest(new ErrorDTO()
                    {
                        ErrorAr = "هذا المعرف غير موجود.",
                        ErrorEn = "This id not exist."
                    });

                if (putDTO.Image != null)
                {
                    fileService.DeleteFile(promotion.ImageURL);

                    var imageResult = await fileService.SaveFileAsync(putDTO.Image, "images\\promotions");

                    if (imageResult.Item1 is false)
                        return ResultDTO<PromotionGetDTO>.BadRequest(new ErrorDTO()
                        {
                            ErrorAr = "حدثت مشكلة في عملية الاضافة.",
                            ErrorEn = imageResult.Item2.ToString()
                        });

                    promotion.ImageURL = imageResult.Item2;
                }

                mapper.Map(putDTO, promotion);

                await context.SaveChangesAsync();

                return ResultDTO<PromotionGetDTO>.Success(mapper.Map<PromotionGetDTO>(promotion));
            }
            catch (Exception ex)
            {
                return ResultDTO<PromotionGetDTO>.BadRequest(new ErrorDTO()
                {
                    ErrorAr = "حدثت مشكلة في عملية الاضافة.",
                    ErrorEn = ex.InnerException is null ? ex.InnerException!.Message : ex.Message
                });
            }
        }
    }
}
