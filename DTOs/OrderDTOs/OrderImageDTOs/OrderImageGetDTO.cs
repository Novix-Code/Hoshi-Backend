using Hoshi.DTOs.OrderDTOs.OrderDTOs;
using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;

namespace Hoshi.DTOs.OrderDTOs.OrderImageDTOs
{
    public class OrderImageGetDTO : IBaseModel, ISoftDelete
    {
        public int Id { get; set; }
        public string ImageURL { get; set; }
        public int ImageNumber { get; set; }
        public bool IsDeleted { get; set; }

        public OrderGetDTO? Order { get; set; }

}
}
