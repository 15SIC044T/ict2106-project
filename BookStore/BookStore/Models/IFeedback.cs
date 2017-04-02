using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.Models
{
    public interface IFeedback
    {
        // make a deep clone of the document
        IFeedback Clone();

        // get a string containing the formatted document
        string GetString();
    }
}