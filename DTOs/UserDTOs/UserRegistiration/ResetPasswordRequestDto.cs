using System.ComponentModel.DataAnnotations;

namespace Hoshi.DTOs.UserDTOs.UserRegistiration
{
    public class ResetPasswordRequestDto
    {
        [Required(ErrorMessage = "You must enter your email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Token can't be empty")]
        public string Token { get; set; }

        [Required(ErrorMessage = "You must enter the new password")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}
