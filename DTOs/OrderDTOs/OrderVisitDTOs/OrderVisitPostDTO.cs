using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;
using Hoshi.Enums;

namespace Hoshi.DTOs.OrderDTOs.OrderVisitDTOs
{

    public class OrderVisitPostDTO 
    {
        public required int OrderId { get; set; }
        public required string VisitNote { get; set; }
        public required double VisitPrice { get; set; }
        public required DateTime VisitingDateTime { get; set; }



        // This prop auto generated in post process
    }
}
