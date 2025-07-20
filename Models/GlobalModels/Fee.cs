using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.GenericModels;
using Hoshi.Enums;
using Hoshi.Models.ServiceModels;

namespace Hoshi.Models.GlobalModels
{
    [UseFSPController]
    [NoAction(ControllerAction.Pagination)]
    [NoAction(ControllerAction.FilterPagination)]
    [EndpointGroupping("Admin")]
    public class Fee : TimestampedModel, ISoftDelete
    {
        public double MainFees { get; set; }
        public double MaxFees { get; set; }
        public double MinFees { get; set; }

        [PropNotMapped(DtoType.Get, exceptInThisDTO: true)]
        public FeeType? FeeType { get; set; }

        public bool IsSpecial { get; set; } = false;
        public bool IsDeleted { get; set; } = false;

        public int? ServiceId { get; set; }
        public Service? Service { get; set; }
    }
}
