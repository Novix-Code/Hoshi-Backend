using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.GenericModels;
using Hoshi.Enums;
using Hoshi.Models.UserModels;

namespace Hoshi.DTOs.GlobalDTOs.ComplaintDTOs
{
    public class ComplaintPostDTO 
    {
        public required string Description { get; set; }
        public string? Response { get; set; }
        public required bool IsDeleted { get; set; }


        public required int ComplaintTypeId { get; set; }

        public required int OrderId { get; set; }

        public required int UserId { get; set; }
    }
}
