using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ApiApplicationCore.Dto
{
    public class ForegetDto
    {
        [Required(ErrorMessage = "Username is required.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        
        public string ConfirmPassword { get; set; }
    }
}
