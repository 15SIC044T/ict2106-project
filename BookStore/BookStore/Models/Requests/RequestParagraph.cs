using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.Models.Requests
{
    public class RequestParagraph : RequestElement, IParagraph
    {
        private string text;

        // constructor
        public RequestParagraph(string textIn)
        {
            text = textIn;
        }

        // create a deep clone of the heading
        public override RequestElement Clone()
        {
            return new RequestParagraph(text);
        }

        // paragraphs in Text are denoted as text and blank line 
        public override string GetString()
        {
            return "\n" + text + "\n";
        }
    }
}