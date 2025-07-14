using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.GlobalDTOs.NotificationTypeDTOs
{
    public class NotificationTypePutDTO : IBaseModel
    {
		public int Id { get; set; }
        public string? Title { get; set; }
        public string? Type { get; set; }
        public bool? ForClient { get; set; }
    }
}
