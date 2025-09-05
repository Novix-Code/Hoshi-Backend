using GenericCRUDLibrary.GenericDTOs.ResponsDTOs;
using Hoshi.DTOs.DashboardDTOs.ArchiveDTOs;

namespace Hoshi.Repositories.ArchiveService
{
    /// <summary>
    /// Contract for archive operations with file handling.
    /// </summary>
    public interface IArchiveService
    {
        /// <summary>
        /// Create archive record with file upload.
        /// </summary>
        Task<ResultDTO<ArchiveGetDTO>> AddArchive(ArchivePostDTO postDTO);
        /// <summary>
        /// Update archive record and optionally replace file.
        /// </summary>
        Task<ResultDTO<ArchiveGetDTO>> UpdateArchive(ArchivePutDTO putDTO);

    }
}
