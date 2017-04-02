using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.Models
{
    public interface IParagraph
    {
        // get a string containing the paragraph
        string GetString();
    }
}