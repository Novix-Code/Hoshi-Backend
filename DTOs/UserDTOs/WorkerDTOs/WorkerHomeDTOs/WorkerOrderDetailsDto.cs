using Hoshi.DTOs.OrderDTOs.OfferDTOs;
using Hoshi.DTOs.OrderDTOs.OrderDTOs;

namespace Hoshi.DTOs.UserDTOs.WorkerDTOs.WorkerHomeDTOs;

public class WorkerOrderDetailsDto
{
    public List<OfferGetDTO> AvailableOffers { get; set; }
    public List<OrderGetDTO> UpcomingOrders { get; set; }
    public List<OrderGetDTO> NearbyOrders { get; set; }
}
