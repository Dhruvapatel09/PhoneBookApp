using System.ComponentModel.DataAnnotations;

namespace ApiApplicationCore.Dto
{
    public class CountryDto
    {
        public int CountryId { get; set; }
        [Required(ErrorMessage = "Country name is required.")]
        public string CountryName { get; set; }
    }
}
