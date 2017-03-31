using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Models
{
    [Table("Review")]
    public class Review
    {
        [Key]
        public int reviewID { get; set; }

        public int itemID { get; set; }
        [Display(Name = "Reviewer:")]
        [Required(ErrorMessage = "Reviewer ID is required")]
        public int reviewerID { get; set; }

        [Display(Name = "Date:")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [Required(ErrorMessage = "Date is required")]
        public DateTime reviewDate { get; set; }

        [Display(Name = "Review:")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Review is required")]
        public String reviewDescription { get; set; }

        [Display(Name = "Rating:")]
        [Required(ErrorMessage = "Rating is required")]
        [Range(1, 5, ErrorMessage = "Can only be between 0 to 5")]
        public int reviewRating { get; set; }
    }
}