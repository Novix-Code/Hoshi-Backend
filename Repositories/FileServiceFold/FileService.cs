using Hoshi.DTOs.FileServicieResult;

namespace Hoshi.Repositories.FileServiceFold
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _environment;

        public FileService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public bool DeleteFile(string fileName, string folderName)
        {
            if (fileName == FileServiceResults.EmptyFile)
                return true;

            string fileFullPath = Path.Combine(_environment.WebRootPath, folderName, fileName);

            if (File.Exists(fileFullPath))
            {
                string trashFolderPath = Path.Combine(_environment.WebRootPath, "Trash", folderName);
                Directory.CreateDirectory(trashFolderPath);
                string trashFilePath = Path.Combine(trashFolderPath, fileName);
                File.Move(fileFullPath, trashFilePath);

                return true;
            }

            return false;
        }
        public string GetFileFullPath(string fileName, string folderName)
        {
            return Path.Combine(_environment.WebRootPath, folderName, fileName);
        }

        public async Task<string> SaveFileAsync(IFormFile file, string folderName)
        {
            if (file?.Length == 0 || file == null)
                return FileServiceResults.EmptyFile;

            if (!ValidateFileExtension(file))
                return FileServiceResults.UnsupportedFileExtension;

            //Get Folder Full Path
            string folderFullPath = Path.Combine(_environment.WebRootPath, folderName);

            //Make unique name for the file
            string fileUniqueName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            //Get File Full Path
            string fileFullPath = Path.Combine(folderFullPath, fileUniqueName);

            //Copy the file content to the file full path
            using (FileStream fileStream = new FileStream(fileFullPath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return fileUniqueName;
        }

        public bool ValidateFileExtension(IFormFile file)
        {
            List<string> allowedFileExtensions = new List<string> { ".jpeg", ".png", ".jpg" };

            //Get file extension
            string fileExtension = Path.GetExtension(file.FileName);

            if (!allowedFileExtensions.Contains(fileExtension))
                return false;
            return true;
        }
    }
}
