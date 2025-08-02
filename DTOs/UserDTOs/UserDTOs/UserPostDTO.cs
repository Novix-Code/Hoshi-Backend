using Hoshi.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Hoshi.DTOs.UserDTOs.UserDTOs
{
    public class UserPostDTO
    {
        [Required(ErrorMessage = "Name must not be empty")]
        [RegularExpression(
            @"^[\p{L}]{3,20}(\s[\p{L}]{3,20}){1,4}$",
            ErrorMessage = "Full name must be 2 to 4 words, each 3–20 letters and alphabetic only."
        )]
        [DefaultValue("Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone must not be empty")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email must not be empty")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;

        public UserType UserType { get; set; }

        // Add image when adding a new user is optional
        public IFormFile? Image { get; set; }
    }
}
