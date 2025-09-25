using GenericCRUDLibrary.GenericInterfaces;

namespace Hoshi.Models.ViewModels
{
    public class NewWorkerModelForView : IBaseModel
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? ImageURL { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? CityName { get; set; }
        public string? Address { get; set; }
        public string? JobTitle { get; set; }
        public bool? IsCompany { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
