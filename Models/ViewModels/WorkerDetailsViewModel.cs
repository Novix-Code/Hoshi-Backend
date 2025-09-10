namespace Hoshi.Models.ViewModels
{
    public class WorkerDetailsViewModel
    {
        public string? FullName { get; set; }
        public string? ImageURL { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public int LivingCityId { get; set; }
        public string? Address { get; set; }
        public int Id { get; set; }
        public int JobId { get; set; }
        public bool IsCompany { get; set; }
        public string? Bio { get; set; }
        public string? IdentityImageURL { get; set; }
        public int CompletedOrders { get; set; }
        public double RateRito { get; set; }

    }
}
