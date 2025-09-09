namespace Hoshi.Models.ViewModels
{
    public class WorkerPageViewModel
    {
        public int TotalWorkers { get; set; }
        public int TotalNewWorkers { get; set; }
        public int TotalActiveWorkers { get; set; }
        public double AverageWorkersPerService { get; set; }
    }
}
