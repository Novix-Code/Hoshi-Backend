using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.DTOs.DashboardDTOs.ArchiveDTOs;

namespace Hoshi.Repositories.ArchiveService
{
    public interface IArchiveService
    {
        Task<ResultDTO<ArchiveGetDTO>> AddArchive(ArchivePostDTO postDTO);
        Task<ResultDTO<ArchiveGetDTO>> UpdateArchive(ArchivePutDTO putDTO);

    }
}
