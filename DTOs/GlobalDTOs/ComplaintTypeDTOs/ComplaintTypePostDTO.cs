using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.GlobalDTOs.ComplaintTypeDTOs
{
    public class ComplaintTypePostDTO 
    {
        public required bool ForClient { get; set; }
        public required string Type { get; set; }
    }
}
