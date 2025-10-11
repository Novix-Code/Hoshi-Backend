using AutoMapper;
using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.Data;
using Hoshi.DTOs.DashboardDTOs.ArchiveDTOs;
using Hoshi.Models.DashboardModels;
using Hoshi.Repositories.FileServiceFold;
using Microsoft.EntityFrameworkCore;

namespace Hoshi.Repositories.ArchiveService
{
    /// <summary>
    /// Handles creation and update of archive records with associated file uploads.
    /// </summary>
    public class ArchiveService : IArchiveService
    {
        private readonly IMapper mapper;
        private readonly HoshiDbContext context;
        private readonly IFileService fileService;

        public ArchiveService(
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
        /// Create an archive entry and save its file via FileService.
        /// </summary>
        public async Task<ResultDTO<ArchiveGetDTO>> AddArchive(ArchivePostDTO postDTO)
        {
            try
            {
                Archive archive = mapper.Map<Archive>(postDTO);

                var imageResult = await fileService.SaveFileAsync(postDTO.File, Path.Combine("files", "archives"));

                if (imageResult.Item1 is false)
                    return ResultDTO<ArchiveGetDTO>.BadRequest(new ErrorDTO()
                    {
                        ErrorAr = "حدثت مشكلة في عملية الاضافة.",
                        ErrorEn = imageResult.Item2.ToString()
                    });

                archive.FileURL = imageResult.Item2;

                await context.Set<Archive>().AddAsync(archive);

                await context.SaveChangesAsync();

                return ResultDTO<ArchiveGetDTO>.Success(mapper.Map<ArchiveGetDTO>(archive));
            }
            catch (Exception ex)
            {
                return ResultDTO<ArchiveGetDTO>.BadRequest(new ErrorDTO()
                {
                    ErrorAr = "حدثت مشكلة في عملية الاضافة.",
                    ErrorEn = ex.InnerException is null ? ex.InnerException!.Message : ex.Message
                });
            }
        }

        /// <summary>
        /// Update an archive entry and optionally replace its file (deletes the old file first).
        /// </summary>
        public async Task<ResultDTO<ArchiveGetDTO>> UpdateArchive(ArchivePutDTO putDTO)
        {
            try
            {
                Archive? archive = await context.Set<Archive>()
                    .FirstAsync(s => s.Id == putDTO.Id);

                if (archive == null)
                    return ResultDTO<ArchiveGetDTO>.BadRequest(new ErrorDTO()
                    {
                        ErrorAr = "هذا المعرف غير موجود.",
                        ErrorEn = "This id not exist."
                    });

                if (putDTO.File != null)
                {
                    fileService.DeleteFile(archive.FileURL);

                    var imageResult = await fileService.SaveFileAsync(putDTO.File, Path.Combine("files","archives"));

                    if (imageResult.Item1 is false)
                        return ResultDTO<ArchiveGetDTO>.BadRequest(new ErrorDTO()
                        {
                            ErrorAr = "حدثت مشكلة في عملية الاضافة.",
                            ErrorEn = imageResult.Item2.ToString()
                        });

                    archive.FileURL = imageResult.Item2;
                }

                mapper.Map(putDTO, archive);

                await context.SaveChangesAsync();

                return ResultDTO<ArchiveGetDTO>.Success(mapper.Map<ArchiveGetDTO>(archive));
            }
            catch (Exception ex)
            {
                return ResultDTO<ArchiveGetDTO>.BadRequest(new ErrorDTO()
                {
                    ErrorAr = "حدثت مشكلة في عملية الاضافة.",
                    ErrorEn = ex.InnerException is null ? ex.InnerException!.Message : ex.Message
                });
            }
        }
    }
}
