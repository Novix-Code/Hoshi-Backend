using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.UserDTOs.UserCollectionAlertDTOs
{
    public class UserCollectionAlertPutDTO  : IBaseModel
    {
		public int Id { get; set; }
        public int? AlertDays { get; set; }
        public bool? IsRemoved { get; set; }
        public DateTime? RemovingDate { get; set; }

        public int? UserId { get; set; }
    }
}
