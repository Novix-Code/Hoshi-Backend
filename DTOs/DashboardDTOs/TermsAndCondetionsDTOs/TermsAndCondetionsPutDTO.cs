using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.DashboardDTOs.TermsAndCondetionsDTOs
{
    public class TermsAndCondetionsPutDTO  : IBaseModel
    {
		public int Id { get; set; }
        public bool ForClient { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
    }
}
