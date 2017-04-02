using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.Models.Feedback
{
    public abstract class FeedbackElement
    { 
        // create a deep clone of the element
        public abstract FeedbackElement Clone();

        // get a string representing the element
        public abstract string GetString();
    }
}