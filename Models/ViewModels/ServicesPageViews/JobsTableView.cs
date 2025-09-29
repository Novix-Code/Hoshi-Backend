namespace Hoshi.Models.ViewModels.ServicesPageViews
{
    public class JobsTableView
    {
        public string JobTitle { get; set; }
        public bool IsDeleted { get; set; }
        public int TotalRelatedCategories { get; set; }
        public int TotalRelatedWorkers { get; set; }
        public double IncomeAvg { get; set; }
    }
}
