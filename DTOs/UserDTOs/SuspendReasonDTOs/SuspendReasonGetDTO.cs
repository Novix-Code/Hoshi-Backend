using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.UserDTOs.SuspendReasonDTOs
{
    public class SuspendReasonGetDTO : TimestampedModel
    {
        public string Reason { get; set; }
    }
}
