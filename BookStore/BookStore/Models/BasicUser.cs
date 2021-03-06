﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//Added
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Models
{
    public class BasicUser : User
    {
        public BasicUser()
        {
            birthdayDiscount = 10;
            deliveryCharge = 20;
        }

        public virtual string Role { get { return "BasicUser"; } }

        [Display(Name = "Birthday Discount:")] 
        public decimal birthdayDiscount { get; set; }

        [Display(Name = "Delivery Charge:")] 
        public decimal deliveryCharge { get; set; }
    }
}