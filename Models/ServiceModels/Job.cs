using GenericCRUDLibrary.GenericInterfaces;
using GenericCRUDLibrary.GenericModels;

namespace Hoshi.Models.ServiceModels
{
    public class Job : TimestampedModel, ISoftDelete
    {
        public string JobTitle { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }

        public List<Service>? Services { get; set; }
    }
}
