using Hoshi.DTOs.UserDTOs.UserDTOs;
using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.UserDTOs.UserCollectionAlertDTOs
{
    public class UserCollectionAlertGetDTO : TimestampedModel
    {
        public int AlertDays { get; set; }
        public bool IsRemoved { get; set; }
        public DateTime RemovingDate { get; set; }

        public UserGetDTO? User { get; set; }
    }
}
