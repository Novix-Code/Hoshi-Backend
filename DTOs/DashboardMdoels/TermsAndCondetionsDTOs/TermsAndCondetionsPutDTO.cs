using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.DashboardMdoels.TermsAndCondetionsDTOs
{
    public class TermsAndCondetionsPutDTO  : IBaseModel
    {
		public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
    }
}
