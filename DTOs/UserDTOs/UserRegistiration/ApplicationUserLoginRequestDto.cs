using System.ComponentModel.DataAnnotations;

namespace Hoshi.DTOs.UserDTOs.UserRegistiration
{
    public class ApplicationUserLoginRequestDto
    {
        [Required(ErrorMessage = "Email must not be empty")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password must not be empty")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
