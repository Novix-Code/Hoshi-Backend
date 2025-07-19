using System;

namespace Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerHomeDTOs;

public class OrderSearchResultDto
{
    public int Id { get; set; }
    public string Description { get; set; }
    public double ProposalPrice { get; set; }
    public string Location { get; set; }
    public DateTime ServicingDateTime { get; set; }
    public string ServiceName { get; set; }
    public string CityName { get; set; }
    public int ClientId { get; set; }
}
