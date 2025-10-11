using GenericCRUDLibrary.GenericInterfaces;

namespace Hoshi.Models.ViewModels.ServicesPageViews
{
    public class JobsTableView : IBaseModel
    {
        public int Id { get; set; }
        public string JobTitle { get; set; }
        public bool IsDeleted { get; set; }
        public int TotalRelatedCategories { get; set; }
        public int TotalRelatedWorkers { get; set; }
        public double IncomeAvg { get; set; }
    }
}
