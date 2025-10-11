using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;
using Hoshi.Models.UserModels;

namespace Hoshi.Models.DashboardModels
{
    [EndpointGroupping("Admin")]
    public class AdminNotification : IBaseModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int AdminId { get; set; }
        public User? Admin { get; set; }
    }
}
