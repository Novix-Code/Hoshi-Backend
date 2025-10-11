using GenericCRUDLibrary.GenericInterfaces;

namespace Hoshi.Models.ViewModels.ServicesPageViews
{
    public class CategoriesTableView : IBaseModel
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public bool IsDeleted { get; set; }
        public int ServicesNum { get; set; }
        public int WorkersNum { get; set; }
        public double IncomeAvg { get; set; }
    }
}
