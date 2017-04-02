using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//Added
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Models
{
    [Table("Carts")]
    public class Cart
    { 
        public int cartID { get; set; }

        public string cartUserID { get; set; }

        [Display(Name = "SubTotal:")]
        public double? subTotal { get; set; }

        [Display(Name = "GST:")]
        public double? gst { get; set; }

        [Display(Name = "Discount Percent:")]
        public double? discountPercent { get; set; }

        [Display(Name = "Discount Amount:")]
        public double? discountAmount { get; set; }

        [Display(Name = "Delivery Charge:")]
        public double? deliveryCharge { get; set; }

        [Display(Name = "Collection Day:")]
        public int? collectionDay { get; set; }

        [Display(Name = "Total Price:")]
        public double? totalPrice { get; set; }

        [Display(Name = "Date of Purchase:")]
        public DateTime? dateOfPurchase { get; set; }
    }
}