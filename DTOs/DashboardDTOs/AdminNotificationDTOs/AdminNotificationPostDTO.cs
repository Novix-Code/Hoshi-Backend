using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;

namespace Hoshi.DTOs.DashboardDTOs.AdminNotificationDTOs
{
    public class AdminNotificationPostDTO 
    {
        public required string Title { get; set; }
        public required string Content { get; set; }
        public required bool IsRead { get; set; } = false;
        public required DateTime CreatedAt { get; set; } = DateTime.Now;

        public required int AdminId { get; set; }
    }
}
