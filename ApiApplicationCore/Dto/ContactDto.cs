using ApiApplicationCore.Models;
using System.ComponentModel.DataAnnotations;

namespace ApiApplicationCore.Dto
{
    public class ContactDto
    {
        public int PhoneId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
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
        public Country Country { get; set; }
        public State State { get; set; }
        public byte[]? ImageByte { get; set; }
        public DateTime? Birthdate { get; set; }

    }
}
