using System.ComponentModel.DataAnnotations;

namespace FruitMVC.ViewModels
{
    public class LoginVM
    {
        [Required]
        public string UserOrEmail { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
