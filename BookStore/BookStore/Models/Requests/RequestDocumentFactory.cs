using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.Models.Requests
{
    public class RequestDocumentFactory : IRequestDocumentFactory
    {
        // constructor
        public RequestDocumentFactory()
        {
            // nothing to do
        }

        // create a heading
        public IHeading CreateHeading(int level, string text)
        {
            return new RequestHeading(level, text);
        }

        // create a paragraph
        public IParagraph CreateParagraph(string text)
        {
            return new RequestParagraph(text);
        }
    }
}