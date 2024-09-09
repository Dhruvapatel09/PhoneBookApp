using ApiApplicationCore.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ApiApplicationCore.Dto
{
    public class AddContactDto
    {
        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email address is required.")]
        [StringLength(50)]
        [EmailAddress]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Invalid email format.")]
        [DisplayName("Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Contact number is required.")]
        [StringLength(15)]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?\d{3}\)?[-.\s]?\d{3}[-.\s]?\d{4}$", ErrorMessage = "Invalid contact number.")]
        [DisplayName("Contact Number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Company is required.")]
        public string Company { get; set; }
        public string? Image { get; set; }
        [Required(ErrorMessage = "Gender is required.")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "Favourites is required.")]
        public bool Favourites { get; set; }
        [Required(ErrorMessage = "Country is required.")]
        public int CountryId { get; set; }
        [Required(ErrorMessage = "State is required.")]
        public int StateId { get; set; }
        public byte[]? ImageByte { get; set; }
        public DateTime? Birthdate { get; set; }

    }
}
