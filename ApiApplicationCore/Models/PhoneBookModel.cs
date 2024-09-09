using System.ComponentModel.DataAnnotations;

namespace ApiApplicationCore.Models
{
    public class PhoneBookModel
    {
        [Key]
        public int PhoneId { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [StringLength(50)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone Number is required.")]
        [StringLength(50)]
        [Phone(ErrorMessage = "Invalid Phone Number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Company is required.")]
        [StringLength(50)]
        public string Company { get; set; }

        public string? Image { get; set; }
        [Required(ErrorMessage = "Gender is required.")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "Favourites is required.")]
        public bool Favourites{get; set;}
        [Required(ErrorMessage = "Country is required.")]
        public int CountryId {  get; set; }
        [Required(ErrorMessage = "State is required.")]
        public int StateId { get; set; }


        public Country Country { get; set; }
        public State State { get; set; }
        public byte[]? ImageByte { get; set; }
        public DateTime? Birthdate { get; set; }
    }
}
