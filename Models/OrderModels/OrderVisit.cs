using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;
using Hoshi.Enums;

namespace Hoshi.Models.OrderModels
{

    [UseFSPController]
    [NoAction(ControllerAction.Delete)]
    [NoAction(ControllerAction.Pagination)]
    [NoAction(ControllerAction.FilterPagination)]
    [EndpointGroupping("Worker", ControllerAction.Add)]
    [EndpointGroupping("Worker", ControllerAction.Update)]
    [CreateRepoPattern("ClientVisitService")]
    [CreateRepoPattern("WorkerVisitService")]
    public class OrderVisit : TimestampedModel
    {
        public string VisitNote { get; set; } = string.Empty;
        public double VisitPrice { get; set; }
        public DateTime VisitingDateTime { get; set; }


        [PropNotMapped(DtoType.Get, exceptInThisDTO: true)]
        public VisitStatus VisitStatus { get; set; } = VisitStatus.Waitting;

        // This prop auto generated in post process
        [PropNotMapped(DtoType.Get, exceptInThisDTO: true)]
        public int VisitNumber { get; set; }
    }
}
