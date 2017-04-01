using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BookStore.Models
{
    public class RegisterUser
    {
        [Key]
        public int userID { get; set; }

        [DisplayName("Name:")]
        [Required(ErrorMessage = "username is required")]
        public string uName { get; set; }

        [DisplayName("Password:")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^.{5,}$", ErrorMessage = "Minimum 3 characters required")]
        [Required(ErrorMessage = "Password is Required")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Invalid")]
        public string uPwd { get; set; }

        [DisplayName("Confirm Password:")]
        [Compare("uPwd", ErrorMessage = "Please confirm your password.")]
        [DataType(DataType.Password)]
        public string confirmUserpwd { get; set; }

        [DisplayName("Date of birth:")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Date of birth is required")]
        public string uDOB { get; set; }

        [DisplayName("Email:")]
        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3]\.)|(([\w-]+\.)+))([a-zA-Z{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter valid email.")]
        public string uEmail { get; set; }

        [DisplayName("Premium user?")]
        [Required(ErrorMessage = "Choice of being a premium user is required")]
        public bool uPremiumUser { get; set; }

    }
    //public class ResetPasswordViewModel2
    //{
    //    [Required]
    //    [EmailAddress]
    //    [Display(Name = "Email")]
    //    public string Email { get; set; }

    //    [Required]
    //    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
    //    [DataType(DataType.Password)]
    //    [Display(Name = "Password")]
    //    public string Password { get; set; }

    //    [DataType(DataType.Password)]
    //    [Display(Name = "Confirm password")]
    //    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    //    public string ConfirmPassword { get; set; }

    //    public string Code { get; set; }
    //}

    //public class ForgotPasswordViewModel2
    //{
    //    [Required]
    //    [EmailAddress]
    //    [Display(Name = "Email")]
    //    public string Email { get; set; }
    //}
}