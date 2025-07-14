using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;
using Hoshi.Enums;

namespace Hoshi.DTOs.OrderDTOs.OrderVisitDTOs
{

    public class OrderVisitGetDTO : TimestampedModel
    {
        public string VisitNote { get; set; }
        public double VisitPrice { get; set; }
        public DateTime VisitingDateTime { get; set; }


        public VisitStatus VisitStatus { get; set; } = VisitStatus.Waitting;

        // This prop auto generated in post process
        public int VisitNumber { get; set; }
    }
}
