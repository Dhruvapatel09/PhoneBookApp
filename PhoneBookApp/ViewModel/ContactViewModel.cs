using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Http;

namespace PhoneBookApp.ViewModel
{
    public class ContactViewModel
    {
        [DisplayName("Contact id")]
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

        [Required(ErrorMessage = "Contact is required.")]
        [StringLength(50)]
        [Phone(ErrorMessage = "Invalid Phone Number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Company is required.")]
        [StringLength(50)]
        public string Company { get; set; }

        [DisplayName("Upload File")]
        public string FileName { get; set; } = string.Empty;

        [DisplayName("File")]
        [Required(ErrorMessage = "File is required.")]
        public IFormFile File { get; set; }
    }
}
