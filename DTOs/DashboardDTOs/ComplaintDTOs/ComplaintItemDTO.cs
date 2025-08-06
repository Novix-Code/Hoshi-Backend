namespace Hoshi.DTOs.DashboardDTOs.ComplaintDTOs;

public class ComplaintItemDTO
{
    public int ComplaintId { get; set; }
    public string UserImageUrl { get; set; }
    public string FullName { get; set; }
    public string ComplaintType { get; set; }
    public DateTime CreatedAt { get; set; }
}