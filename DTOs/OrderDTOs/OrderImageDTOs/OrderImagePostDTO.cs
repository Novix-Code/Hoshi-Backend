using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;

namespace Hoshi.DTOs.OrderDTOs.OrderImageDTOs
{
    public class OrderImagePostDTO 
    {
        public required string ImageURL { get; set; }
        public required int ImageNumber { get; set; }
        public required bool IsDeleted { get; set; }

        public required int OrderId { get; set; }

}
}
