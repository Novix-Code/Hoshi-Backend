namespace Hoshi.DTOs.UserDTOs.ClientSpecificationDTOs
{
    public class ClientSpecificationPostDTO 
    {
        public required string Address { get; set; }
        public required string Bio { get; set; }

        public required IFormFile Image { get; set; }

        public required int UserId { get; set; }
    }
}
