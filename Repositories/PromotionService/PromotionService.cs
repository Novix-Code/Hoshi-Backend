using AutoMapper;
using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.PromotionDTOs.PromotionDTOs;
using Hoshi.Enums;
using Hoshi.Models.PromotionModels;
using Hoshi.Repositories.FileServiceFold;
using Microsoft.EntityFrameworkCore;

namespace Hoshi.Repositories.PromotionService
{
    /// <summary>
    /// Implements promotion creation and updates, including image persistence via FileService.
    /// </summary>
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


        /// <summary>
        /// Create a promotion and save its image; returns the created DTO.
        /// </summary>
        public async Task<ResultDTO<PromotionGetDTO>> AddPromotion(PromotionPostDTO postDTO)
        {
            try
            {
                Promotion promotion = mapper.Map<Promotion>(postDTO);

                var imageResult = await fileService.SaveFileAsync(postDTO.Image, Path.Combine("images","promotions"));

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

        /// <summary>
        /// Update a promotion and optionally replace its image (old image is deleted first).
        /// </summary>
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

                    var imageResult = await fileService.SaveFileAsync(putDTO.Image, Path.Combine("images", "promotions"));

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

        /// <summary>
        /// Get all Non-Taken Promotion for spicific user and check its validations:
        /// <br></br>- Its for specified user.
        /// <br></br>- Time validation to check its valid to be used.
        /// <br></br>- Check its not used yet by this user.
        /// </summary>
        /// <param name="userId">User that will be get data for him.</param>
        /// <param name="forClient">Flag to check if this user is Client or Worker.</param>
        /// <returns>List of Non-Taken Promotions.</returns>
        public async Task<List<Promotion>> NoneTakenPromotions(int userId, bool forClient)
        {
            // Check if it for client than will except Workers from condition and the opeasite
            string exceptedUser = forClient ? PromotionFor.Worker.ToString() : PromotionFor.Client.ToString();

            // var currentDate = DateTime.UtcNow;

            // return await context.Promotions
            //     .Where(p => p.PromotionFor != exceptedUser && !p.IsDeleted) // get data for all or only provided user and not deleted
            //     .Where(p => p.UntilBeUsed ||
            //                (p.StartDate != null && p.EndDate != null &&
            //                 p.StartDate <= currentDate && p.EndDate >= currentDate)) // check if it untilBeUsed or not and if not will check if the current date in the range of start and end of the promotion
            //     .Where(p => !context.PromotionsTaken
            //         .Any(pt => pt.UserId == userId && pt.PromotionId == p.Id))
            //     .ToListAsync();
            return await context.Promotions
                .Where(p => p.PromotionFor != exceptedUser && !p.IsDeleted) 
                .Where(p => p.UntilBeUsed) 
                .Where(p => !context.PromotionsTaken
                    .Any(pt => pt.UserId == userId && pt.PromotionId == p.Id))
                .ToListAsync();
        }
    }
}
