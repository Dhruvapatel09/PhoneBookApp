using ApiApplicationCore.Models;
using System.ComponentModel.DataAnnotations;

namespace ApiApplicationCore.Dto
{
    public class StateDto
    {
        public int StateId { get; set; }
        [Required(ErrorMessage = "State name is required.")]
        public string StateName { get; set; }
        public int CountryId { get; set; }
        public Country Country { get; set; }
    }
}
