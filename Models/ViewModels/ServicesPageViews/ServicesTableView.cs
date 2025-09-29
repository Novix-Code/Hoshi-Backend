namespace Hoshi.Models.ViewModels.ServicesPageViews
{
    public class ServicesTableView
    {
        public string ServiveName { get; set; }
        public string CategoryName { get; set; }
        public bool IsDeleted { get; set; }
        public int OrdersNum { get; set; }
        public int WorkersNum { get; set; }
        public double IncomeAvg { get; set; }
    }
}
