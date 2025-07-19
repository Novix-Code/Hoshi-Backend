namespace Hoshi.DTOs.ClientDTOs
{
    public class GetAllHomeServiceDTO
    {
        public int ClienId { get; set; }
        public string ClienName { get; set; }
        public ClientHomeDto DetailsAboutServices { get; set; }
    }
}
