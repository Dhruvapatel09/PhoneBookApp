using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;

namespace ClientApplicationCore.ViewModels
{
    public class CountryViewModel
    {
        public int CountryId { get; set; }
        [Required(ErrorMessage = "Country name is required.")]
        public string CountryName { get; set; }
       
    }
}
