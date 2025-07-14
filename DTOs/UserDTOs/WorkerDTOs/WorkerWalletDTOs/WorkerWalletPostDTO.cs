using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerWalletDTOs
{
    public class WorkerWalletPostDTO 
    {
        public required double Balance { get; set; } = 0.0;
        public required bool HitLimit { get; set; } = false;

        public required int WorkerId { get; set; }
    }
}
