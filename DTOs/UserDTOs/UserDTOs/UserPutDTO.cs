using GenericCRUDLibrary.GenericInterfaces;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Hoshi.DTOs.UserDTOs.UserDTOs
{
    public class UserPutDTO : IBaseModel
    {
        public required int Id { get; set; }

        [DefaultValue("Full Name")]
        [Required(ErrorMessage = "يجب ألا يكون الاسم فارغًا")]
        [RegularExpression(
            @"^[\p{L}\p{M}'\-.\s]{2,100}$",
            ErrorMessage = "يمكن أن يحتوي الاسم فقط على الحروف والمسافات وعلامة الاقتباس المفردة والواصلات والنقاط."
        )]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "يجب ألا يكون رقم الهاتف فارغًا")]
        [RegularExpression(
            @"^\+?\d{1,3}?[-\s]?(\(\d{1,4}\)|\d{1,4})?[-\s]?\d{3,4}[-\s]?\d{3,4}$",
            ErrorMessage = "صيغة رقم الهاتف غير صحيحة."
        )]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "يجب ألا يكون البريد الإلكتروني فارغًا")]
        [DataType(DataType.EmailAddress, ErrorMessage = "صيغة البريد الإلكتروني غير صحيحة")]
        public string Email { get; set; } = string.Empty;

        public IFormFile? Image { get; set; }
    }
}
