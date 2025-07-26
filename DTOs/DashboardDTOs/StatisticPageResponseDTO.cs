namespace Hoshi.DTOs.DashboardDTOs;

public class StatisticPageResponseDTO
{
    public int TotalOrders { get; set; }
    public double TotalOrdersIncome { get; set; }
    public double TotalOrdersPrices { get; set; }
    public int TotalClients { get; set; }
    public double AverageOrderingPerUser { get; set; }
}