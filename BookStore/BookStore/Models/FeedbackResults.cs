using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BookStore.Models
{
    [Table("Feedback")]
    public class FeedbackResults
    {
        [Key]
        public int feedbackID { get; set; }
        
        [Display(Name = "title:")]
        [Required(ErrorMessage = "Title is required")]
        public String Title { get; set; }

        [Display(Name = "Author:")]
        [Required(ErrorMessage = "Author is required")]
        public String Author { get; set; }

        [Display(Name = "Sypnosis:")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Sypnosis is required")]
        public String Sypnosis { get; set; }
    }
}