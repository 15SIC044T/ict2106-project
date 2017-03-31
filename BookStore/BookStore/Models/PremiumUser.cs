using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


//Added
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Models
{
    public class PremiumUser : User
    {
        public PremiumUser()
        {

            birthdayDiscount = 20;
            deliveryCharge = 0;
        }

        [Display(Name = "Birthday Discount:")] 
        public decimal birthdayDiscount { get; set; }

        [Display(Name = "Delivery Charge:")] 
        public decimal deliveryCharge { get; set; }
    }
}