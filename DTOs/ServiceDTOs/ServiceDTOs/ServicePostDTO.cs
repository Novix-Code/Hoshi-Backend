namespace Hoshi.DTOs.ServiceDTOs.ServiceDTOs
{
    public class ServicePostDTO 
    {
        public required string ServiveName { get; set; }
        public required IFormFile Image { get; set; }

        public required int ServiceCategoryId { get; set; }

    }
}
