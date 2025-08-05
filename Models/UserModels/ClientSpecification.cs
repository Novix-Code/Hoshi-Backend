using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;
using Hoshi.Models.GlobalModels;
using Microsoft.EntityFrameworkCore;

namespace Hoshi.Models.UserModels
{
    [Index(nameof(UserId), IsUnique = true)]
    [NoAction(ControllerAction.Delete)]
    [EndpointGroupping("Client")]
    public class ClientSpecification : TimestampedModel
    {
        public string Address { get; set; } = "لم يتم الاضافة بعد";
        public string Bio { get; set; } = "لم يتم الاضافة بعد";
        public int CompletedOrders { get; set; } = 0;
        public double RateRito { get; set; } = 0.0;
        public double Balance { get; set; } = 0.0;
        public double Indebtedness { get; set; } = 0.0;

        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
