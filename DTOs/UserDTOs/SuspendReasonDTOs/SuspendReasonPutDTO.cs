using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.UserDTOs.SuspendReasonDTOs
{
    public class SuspendReasonPutDTO  : IBaseModel
    {
		public int Id { get; set; }
        public string? Reason { get; set; }
    }
}
