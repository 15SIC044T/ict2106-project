using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BookStore.Models
{

    public class UserChangePassword
    {
        [DisplayName("Name:")]
        public string uName { get; set; }

        [DisplayName("Old Password:")]
        [Required(ErrorMessage = "Old Password is required")]
        [DataType(DataType.Password)]
        public string uOPwd { get; set; }

        [DisplayName("Password:")]
        [Required(ErrorMessage = "New Password is required")]
        [DataType(DataType.Password)]
        public string uPwd { get; set; }

        [DisplayName("Confirm Password:")]
        [Required(ErrorMessage = "Confirm password is required")]
        //[Compare("uPwd", ErrorMessage = "Please confirm your password.")] //already checked so dont need
        [DataType(DataType.Password)]
        public string confirmUserpwd { get; set; }
        
    }
}