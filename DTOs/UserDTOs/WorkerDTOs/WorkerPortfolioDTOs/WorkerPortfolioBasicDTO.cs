using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerPortfolioDTOs
{
    public class WorkerPortfolioBasicDTO: TimestampedModel
    {
        public string FileURL { get; set; }
    }
}
