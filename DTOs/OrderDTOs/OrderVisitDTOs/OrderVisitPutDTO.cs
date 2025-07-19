using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;
using Hoshi.Enums;

namespace Hoshi.DTOs.OrderDTOs.OrderVisitDTOs
{

    public class OrderVisitPutDTO  : IBaseModel
    {
		public int Id { get; set; }
        public string? VisitNote { get; set; }
        public double? VisitPrice { get; set; }
        public DateTime? VisitingDateTime { get; set; }



        // This prop auto generated in post process
    }
}
