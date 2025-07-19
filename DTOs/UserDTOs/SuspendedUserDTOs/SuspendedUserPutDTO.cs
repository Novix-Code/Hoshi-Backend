using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.UserDTOs.SuspendedUserDTOs
{
    public class SuspendedUserPutDTO  : IBaseModel
    {
		public int Id { get; set; }
        public int? UserId { get; set; }

        public int? SuspendReasonId { get; set; }
    }
}
