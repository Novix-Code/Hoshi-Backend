using GenericCRUDLibrary.GenericModels;
using Hoshi.DTOs.OrderDTOs.OrderDTOs;

namespace Hoshi.DTOs.OrderDTOs.OrderVisitDTOs
{

    public class OrderVisitGetDTO : TimestampedModel
    {
        
        public string VisitNote { get; set; }
        public double VisitPrice { get; set; }
        public DateTime VisitingDateTime { get; set; }
        public OrderGetDTO? Order { get; set; }

        public string VisitStatus { get; set; }

        // This prop auto generated in post process
        public int VisitNumber { get; set; }
    }
}
