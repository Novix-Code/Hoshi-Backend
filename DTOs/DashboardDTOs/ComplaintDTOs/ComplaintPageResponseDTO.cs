namespace Hoshi.DTOs.DashboardDTOs.ComplaintDTOs;

public class ComplaintPageResponseDTO
{
    public int TotalComplaints { get; set; }
    public int TotalComplaintsOpened { get; set; }
    public int TotalComplaintsClosed { get; set; }
    public List<ComplaintItemDTO> Complaints { get; set; }
}