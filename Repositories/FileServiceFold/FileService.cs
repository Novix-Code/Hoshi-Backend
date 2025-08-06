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

        public bool DeleteFile(string fileURL)
        {
            string fileFullPath = Path.Combine(_environment.WebRootPath, fileURL);
            string trashFolderPath = Path.Combine(_environment.WebRootPath, "trash");

            if (!Directory.Exists(trashFolderPath)) 
                Directory.CreateDirectory(trashFolderPath);

            if (File.Exists(fileFullPath))
            {
                string trashFilePath = Path.Combine(trashFolderPath, fileURL);

                string fileTrashFolder = Path.Combine(trashFolderPath, Path.GetDirectoryName(fileURL)!);

                if (!Directory.Exists(fileTrashFolder))
                    Directory.CreateDirectory(fileTrashFolder);

                File.Move(fileFullPath, trashFilePath);

                return true;
            }


            return false;
        }
        
        public async Task<Tuple<bool, string>> SaveFileAsync(IFormFile file, string folderShortPath)
        {
            if (file?.Length == 0 || file == null)
                return new Tuple<bool, string>(false, FileServiceResults.EmptyFile);

            if (!ValidateFileExtension(file))
                return new Tuple<bool, string>(false, FileServiceResults.UnsupportedFileExtension);

            //Get Folder Full Path
            string folderFullPath = Path.Combine(_environment.WebRootPath, folderShortPath);

            if (!Directory.Exists(folderFullPath))
            {
                Directory.CreateDirectory(folderFullPath);
            }

            //Make unique name for the file
            string fileUniqueName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            //Get File Full Path
            string fileFullPath = Path.Combine(folderFullPath, fileUniqueName);

            //Copy the file content to the file full path
            using (FileStream fileStream = new FileStream(fileFullPath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return new Tuple<bool, string>(true, Path.Combine(folderShortPath, fileUniqueName));
        }

        public bool ValidateFileExtension(IFormFile file)
        {
            List<string> allowedFileExtensions = 
                new List<string> { ".jpeg", ".png", ".jpg", ".pdf", ".docx", ".xlsx" };

            //Get file extension
            string fileExtension = Path.GetExtension(file.FileName);

            if (!allowedFileExtensions.Contains(fileExtension))
                return false;
            else
                return true;
        }
    }
}
