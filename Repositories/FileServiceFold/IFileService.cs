namespace Hoshi.Repositories.FileServiceFold
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile file, string folderName);
        bool DeleteFile(string fileName, string folderName);
        bool ValidateFileExtension(IFormFile file);
        string GetFileFullPath(string fileName, string folderName);
    }
}
