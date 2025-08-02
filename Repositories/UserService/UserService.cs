using AutoMapper;
using Hoshi.Data;
using Hoshi.Models.UserModels;
using Hoshi.Repositories.FileServiceFold;

namespace Hoshi.Repositories.UserService
{
    public class UserService : IUserService
    {
        private readonly HoshiDbContext context;
        private readonly IMapper mapper;
        private readonly IFileService fileService;

        public UserService(
            HoshiDbContext context,
            IMapper mapper,
            IFileService fileService
        )
        {
            this.context = context;
            this.mapper = mapper;
            this.fileService = fileService;
        }

        /// <summary>
        /// This Method to save User Personal Image and update user data.
        /// </summary>
        /// <param name="id">User id that will be updated</param>
        /// <param name="image">Image file that will be saved</param>
        /// <returns>Return a Tuple of Bool and String to check if done successfully or not and get the image url or the error.</returns>
        public async Task<Tuple<bool, string>> AddUserImage(int id, IFormFile image, bool isUpdate)
        {
            try
            {
                // Fetch user data from its id
                User? user = await context.Set<User>().FindAsync(id);

                // Check if this user is there or not
                if(user == null)
                    return new Tuple<bool, string>(false, "Invalid User Id.");

                if (isUpdate)
                {
                    fileService.DeleteFile(user.ImageURL!);
                }

                // User FileService method to save the image to the images\personalimages folder in wwwroot
                var imageResult = await fileService.SaveFileAsync(image, "images\\personalimages");

                // Check if the image saved successfuly
                if (imageResult.Item1)
                {
                    // Add image url to user object data
                    user.ImageURL = imageResult.Item2;
                    // Update it in db and save changes
                    context.Update(user);
                    await context.SaveChangesAsync();
                }

                // return the images result in all cases
                return imageResult;
            }
            catch (Exception ex)
            {
                // If any thing happends return the exception
                return new Tuple<bool, string>(false, ex.InnerException!.Message);
            }
        }
    }
}