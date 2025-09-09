namespace Hoshi.Models.ViewModels
{
    public class NewClientViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string ImageURL { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
