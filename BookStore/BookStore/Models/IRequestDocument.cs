using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.Models
{
    public interface IRequestDocument
    {
        // make a deep clone of the document
        IRequestDocument Clone();

        // get a string containing the formatted document
        string GetString();
    }
}