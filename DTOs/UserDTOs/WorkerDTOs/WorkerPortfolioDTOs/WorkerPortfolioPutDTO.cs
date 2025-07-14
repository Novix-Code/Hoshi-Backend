using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerPortfolioDTOs
{
    public class WorkerPortfolioPutDTO  : IBaseModel
    {
		public int Id { get; set; }
        public string? FileURL { get; set; }

        public int? WorkerId { get; set; }
    }
}
