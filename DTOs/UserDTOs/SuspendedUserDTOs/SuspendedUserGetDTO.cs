using Hoshi.DTOs.UserDTOs.SuspendReasonDTOs;
using Hoshi.DTOs.UserDTOs.UserDTOs;
using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.UserDTOs.SuspendedUserDTOs
{
    public class SuspendedUserGetDTO : TimestampedModel
    {
        public UserGetDTO? User { get; set; }

        public SuspendReasonGetDTO? SuspendReason { get; set; }
    }
}
