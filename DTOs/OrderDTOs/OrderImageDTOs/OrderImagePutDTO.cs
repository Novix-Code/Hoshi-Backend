using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;

namespace Hoshi.DTOs.OrderDTOs.OrderImageDTOs
{
    public class OrderImagePutDTO  : IBaseModel
    {
        public int Id { get; set; }
        public string? ImageURL { get; set; }
        public int? ImageNumber { get; set; }
        public bool? IsDeleted { get; set; }

        public int? OrderId { get; set; }

}
}
