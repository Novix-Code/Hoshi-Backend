using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.GlobalDTOs.ComplaintTypeDTOs
{
    public class ComplaintTypePutDTO  : IBaseModel
    {
		public int Id { get; set; }
        public bool? ForClient { get; set; }
        public string? Type { get; set; }
    }
}
