using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.GenericModels;
using Hoshi.Enums;
using Hoshi.Models.UserModels;

namespace Hoshi.DTOs.GlobalDTOs.ComplaintDTOs
{
    public class ComplaintPutDTO  : IBaseModel
    {
		public int Id { get; set; }
        public string? Description { get; set; }
        public string? Response { get; set; }
        public bool? IsDeleted { get; set; }


        public int? ComplaintTypeId { get; set; }

        public int? OrderId { get; set; }

        public int? UserId { get; set; }
    }
}
