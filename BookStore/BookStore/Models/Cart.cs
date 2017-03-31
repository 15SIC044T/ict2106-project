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
        public decimal? subTotal { get; set; }

        [Display(Name = "GST:")]
        public decimal? gst { get; set; }

        [Display(Name = "Discount Percent:")]
        public decimal? discountPercent { get; set; }

        [Display(Name = "Discount Amount:")]
        public decimal? discountAmount { get; set; }

        [Display(Name = "Delivery Charge:")]
        public decimal? deliveryCharge { get; set; }

        [Display(Name = "Collection Day:")]
        public int? collectionDay { get; set; }

        [Display(Name = "Total Price:")]
        public decimal? totalPrice { get; set; }

        [Display(Name = "Date of Purchase:")]
        public DateTime? dateOfPurchase { get; set; }
    }
}