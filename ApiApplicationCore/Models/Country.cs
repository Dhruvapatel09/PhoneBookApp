using System.ComponentModel.DataAnnotations;

namespace ApiApplicationCore.Models
{
    public class Country
    {
        [Key]
        public int CountryId { get; set; }
        [Required(ErrorMessage = "Country name is required.")]
        public string CountryName { get; set; }
        public ICollection<State> States { get; set; }
        public ICollection<PhoneBookModel> phoneBookModels { get; set; }
    }
}
