using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.UserDTOs.SuspendedUserDTOs
{
    public class SuspendedUserPostDTO 
    {
        public required int UserId { get; set; }

        public required int SuspendReasonId { get; set; }
    }
}
