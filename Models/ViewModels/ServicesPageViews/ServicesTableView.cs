using GenericCRUDLibrary.GenericInterfaces;

namespace Hoshi.Models.ViewModels.ServicesPageViews
{
    public class ServicesTableView : IBaseModel
    {
        public int Id { get; set; }
        public string ServiveName { get; set; }
        public string ImageURL { get; set; }
        public int ServiceCategoryId { get; set; }
        public string CategoryName { get; set; }
        public bool IsDeleted { get; set; }
        public int OrdersNum { get; set; }
        public int WorkersNum { get; set; }
        public double IncomeAvg { get; set; }
    }
}
