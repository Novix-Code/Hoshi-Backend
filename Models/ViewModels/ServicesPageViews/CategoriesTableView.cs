namespace Hoshi.Models.ViewModels.ServicesPageViews
{
    public class CategoriesTableView
    {
        public string CategoryName { get; set; }
        public bool IsDeleted { get; set; }
        public int ServicesNum { get; set; }
        public int WorkersNum { get; set; }
        public double IncomeAvg { get; set; }
    }
}
