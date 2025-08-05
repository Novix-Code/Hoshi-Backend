using Hoshi.DTOs.OrderDTOs.OrderDTOs;
using GenericCRUDLibrary.GenericInterfaces;

namespace Hoshi.DTOs.OrderDTOs.OrderImageDTOs
{
    public class OrderImageGetDTO : IBaseModel, ISoftDelete
    {
        public int Id { get; set; }
        public string ImageURL { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }

    }
}
