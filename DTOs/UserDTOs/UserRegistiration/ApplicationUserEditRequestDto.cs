using System.ComponentModel.DataAnnotations;

namespace Hoshi.DTOs.UserDTOs.UserRegistiration
{
    public class ApplicationUserEditRequestDto
    {
        [Required(ErrorMessage = "Email can't be empty")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Name can't be empty")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Phone can't be empty")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        public IFormFile? Profile_Picture { get; set; }
    }
}
