using GenericCRUDLibrary.GenericInterfaces;

namespace Hoshi.DTOs.ServiceDTOs.JobDTOs
{

    public class JobPutDTO  : IBaseModel
    {
		public int Id { get; set; }
        public string? JobTitle { get; set; }

    }
}
