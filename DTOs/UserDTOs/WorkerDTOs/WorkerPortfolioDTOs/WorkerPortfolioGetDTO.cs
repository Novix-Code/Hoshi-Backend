using Hoshi.DTOs.UserDTOs.UserDTOs;
using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerPortfolioDTOs
{
    public class WorkerPortfolioGetDTO : TimestampedModel
    {
        public string FileURL { get; set; }

        public int WorkerId { get; set; }
        public UserGetDTO? Worker { get; set; }
    }
}
