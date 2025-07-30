namespace Hoshi.Repositories.FileServiceFold
{
    public interface IFileService
    {
        Task<Tuple<bool, string>> SaveFileAsync(IFormFile file, string folderShortPath);
        bool DeleteFile(string fileURL);
        bool ValidateFileExtension(IFormFile file);
    }
}
