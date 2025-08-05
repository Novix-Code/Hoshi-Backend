using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;

namespace Hoshi.DTOs.OrderDTOs.OrderImageDTOs
{
    public class OrderImagePostDTO 
    {
        public required string ImageURL { get; set; }

        public required int OrderId { get; set; }

}
}
