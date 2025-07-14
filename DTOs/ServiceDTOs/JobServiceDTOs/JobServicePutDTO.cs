using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.ServiceDTOs.JobServiceDTOs
{
    public class JobServicePutDTO  : IBaseModel
    {
		public int Id { get; set; }
        public int? JobId { get; set; }

        public int? ServiceId { get; set; }
    }
}
