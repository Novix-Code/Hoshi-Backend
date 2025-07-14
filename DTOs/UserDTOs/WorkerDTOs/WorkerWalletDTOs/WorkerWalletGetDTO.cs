using Hoshi.DTOs.UserDTOs.UserDTOs;
using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerWalletDTOs
{
    public class WorkerWalletGetDTO : TimestampedModel
    {
        public double Balance { get; set; } = 0.0;
        public bool HitLimit { get; set; } = false;

        public int WorkerId { get; set; }
        public UserGetDTO? Worker { get; set; }
    }
}
