using Hoshi.Enums;

namespace Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerHomeDTOs;

public class SubmittedOrderDetailsDto
{
    public int OrderNumber { get; set; }
    public ClientDataDto ClientData { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public ServiceDataDto Service { get; set; }
    public DateTime ServicingDateTime { get; set; }
    public double ProposalPrice { get; set; }
    public string Location { get; set; }
    public string Description { get; set; }
    public List<string> ImagesUrl { get; set; }
}

public class ClientDataDto
{
    public string ImageUrl { get; set; }
    public string Name { get; set; }
    public double RateRatio { get; set; }
}

public class ServiceDataDto
{
    public int Id { get; set; }
    public string ServiceName { get; set; }
    public string CategoryName { get; set; }
}

public class OrderClientDetailsDto
{
    public ClientDataDto ClientData { get; set; }
    public List<ClientRateDto> ClientRates { get; set; }
}


public class ClientRateDto
{
    public string WorkerName { get; set; }
    public double Rate { get; set; }
    public string Comment { get; set; }
}