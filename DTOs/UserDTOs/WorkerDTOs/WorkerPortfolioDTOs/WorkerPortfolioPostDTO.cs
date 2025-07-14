using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerPortfolioDTOs
{
    public class WorkerPortfolioPostDTO 
    {
        public required string FileURL { get; set; }

        public required int WorkerId { get; set; }
    }
}
