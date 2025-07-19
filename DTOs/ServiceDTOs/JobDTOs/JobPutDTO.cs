using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.ServiceDTOs.JobDTOs
{

    public class JobPutDTO  : IBaseModel
    {
		public int Id { get; set; }
        public string? JobTitle { get; set; }
        public bool? IsDeleted { get; set; }

    }
}
