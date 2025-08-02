namespace Hoshi.Repositories.UserService
{
    public interface IUserService
    {
        Task<Tuple<bool, string>> AddUserImage(int id, IFormFile image, bool isUpdate);
    }
}
