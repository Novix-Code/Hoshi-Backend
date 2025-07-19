using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.GlobalDTOs.ComplaintTypeDTOs
{
    public class ComplaintTypeGetDTO : TimestampedModel
    {
        public bool ForClient { get; set; }
        public string Type { get; set; }
    }
}
