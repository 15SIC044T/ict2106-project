using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookStore.Models
{
    public class ownForgetPassModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}