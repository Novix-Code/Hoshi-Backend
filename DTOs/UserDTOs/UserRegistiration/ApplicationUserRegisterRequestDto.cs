using System.ComponentModel.DataAnnotations;

namespace Hoshi.DTOs.UserDTOs.UserRegistiration
{
    public class ApplicationUserRegisterRequestDto
    {
        [Required(ErrorMessage = "Name must not be empty")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Phone must not be empty")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Email must not be empty")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password must not be empty")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public IFormFile? ProfilePicture { get; set; }
    }
}
