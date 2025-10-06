using GenericCRUDLibrary.GenericInterfaces;

namespace Hoshi.Models.ViewModels.WorkersPageViews
{
    public class SuspendedWorkerModelForView : IBaseModel
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? ImageURL { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? JobTitle { get; set; }
        public double RateRito { get; set; }
        public int CancellationNumber { get; set; }
        public bool? IsCompany { get; set; }
        public double Balance { get; set; }
        public string Reason { get; set; }
        public DateTime? CreatedAt { get; set; }

        public int SuspentionId { get; set; }
    }

}
