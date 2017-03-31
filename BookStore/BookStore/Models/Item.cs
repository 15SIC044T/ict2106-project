using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//Added
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PagedList;

namespace BookStore.Models
{
    [Table("Item")]
    public class Item
    {
        public int itemID { get; set; }

        [Display(Name = "Name:")]
        [Required(ErrorMessage = "Item Name is required")]
        public string iName { get; set; }

        [Display(Name = "Description:")]
        [Required(ErrorMessage = "Item Description is required")]
        public string iDescription { get; set; }

        [Display(Name = "Image:")]
        public string iImage { get; set; }

        [Display(Name = "Price:")]
        [Required(ErrorMessage = "Item Price is required")]
        public decimal iPrice { get; set; }

        [Display(Name = "Category:")]
        [Required(ErrorMessage = "Item Category is required")]
        public string iCategory { get; set; }

        [Display(Name = "Quantity:")]
        [Required(ErrorMessage = "Item Quantity is required")]
        public string iQuantity { get; set; }

    }
}