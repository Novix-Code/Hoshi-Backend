using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Hoshi.DTOs.UserDTOs.UserRegistiration
{
    public class ApplicationUserRegisterRequestDto
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
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email must not be empty")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password must not be empty")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
