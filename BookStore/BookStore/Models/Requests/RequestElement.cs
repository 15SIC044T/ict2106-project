using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.Models.Requests
{
    public abstract class RequestElement
    {
        // create a deep clone of the element
        public abstract RequestElement Clone();

        // get a string representing the element
        public abstract string GetString();
    }
}