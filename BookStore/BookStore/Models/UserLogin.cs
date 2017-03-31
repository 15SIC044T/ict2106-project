using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class UserLogin
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember on this computer")]
        public bool RememberMe { get; set; }

        [Required]
        [Display(Name = "Calculate the sum:")]
        public string Captcha { get; set; }
    }

}