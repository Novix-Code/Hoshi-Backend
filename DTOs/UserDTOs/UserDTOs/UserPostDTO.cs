using GenericCRUDLibrary.CustomAttributes;
using GenericCRUDLibrary.GenericInterfaces;
using Hoshi.Enums;
using Microsoft.AspNetCore.Identity;

namespace Hoshi.DTOs.UserDTOs.UserDTOs
{
    public class UserPostDTO 
    {
        public required string UserCode { get; set; }
        public required bool IsDeleted { get; set; }

        public required string UserType { get; set; }

        public required DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? ModifiedAt { get; set; }
    }
}
