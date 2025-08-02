using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.DTOs.ServiceDTOs.ServiceDTOs
{
    public class ServiceBasicDTO : TimestampedModel
    {
        public string ServiveName { get; set; }
        public string ImageURL { get; set; }
        public bool IsDeleted { get; set; }
    }
}
