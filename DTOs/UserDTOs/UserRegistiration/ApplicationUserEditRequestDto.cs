using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Hoshi.DTOs.UserDTOs.UserRegistiration
{
    public class ApplicationUserEditRequestDto
    {
        public int Id { get; set; }

        [RegularExpression(
            @"^[\p{L}]{3,20}(\s[\p{L}]{3,20}){1,4}$",
            ErrorMessage = "Full name must be 2 to 4 words, each 3–20 letters and alphabetic only."
        )]
        [DefaultValue("Full Name")]
        public string FullName { get; set; } = string.Empty;

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; } = string.Empty;

        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;
    }
}
