using System.ComponentModel.DataAnnotations;

namespace ClientApplicationCore.ViewModels
{
    public class ContactViewModel
    {
        public int PhoneId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Company { get; set; }
        public string? Image { get; set; }
        public byte[]? ImageByte { get; set; }

        public IFormFile? file { get; set; }
        [Required(ErrorMessage = "Gender is required.")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "Favourites is required.")]
        public bool Favourites { get; set; }
        [Required(ErrorMessage = "Country is required.")]
        public int CountryId { get; set; }
        [Required(ErrorMessage = "State is required.")]
        public int StateId { get; set; }
        public DateTime? Birthdate { get; set; }
        public StateViewModel? State { get; set; }
        public CountryViewModel? Country { get; set; }
    }
}
