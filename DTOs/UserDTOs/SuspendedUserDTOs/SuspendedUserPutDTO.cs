using GenericCRUDLibrary.GenericInterfaces;

namespace Hoshi.DTOs.UserDTOs.SuspendedUserDTOs
{
    public class SuspendedUserPutDTO  : IBaseModel
    {
		public int Id { get; set; }

        public int? SuspendReasonId { get; set; }
    }
}
