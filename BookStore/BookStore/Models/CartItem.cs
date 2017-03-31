using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BookStore.Models
{
    [Table("CartItems")]
    public class CartItem
    {
        [Key]
        public int CId { get; set; }

        public int? cartID { get; set; } 
        public int? itemID { get; set; }

        [Display(Name = "Quantity: ")]
        public int quantity { get; set; }

        [Display(Name = "Price:")]
        public decimal price { get; set; }
    }
}