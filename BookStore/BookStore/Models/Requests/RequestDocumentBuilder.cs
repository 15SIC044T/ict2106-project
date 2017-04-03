using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.Models.Requests
{
    public class RequestDocumentBuilder : IRequestDocumentBuilder
    {
        // the document being built
        private RequestDocument doc;

        // constructor
        public RequestDocumentBuilder()
        {
            // nothing to do
        }

        // build the body
        public void BuildBody(string text)
        {
            // add a paragraph containing the text
            doc.Append(new RequestParagraph(text));
        }

        // build header
        public void BuildHead(string title, string author)
        {
            // set the title and author of the document
            doc.SetTitle(title);
            doc.SetAuthor(author);
        }

        //finish the document
        public void CloseDocument()
        {
            //nothing to do
        }

        // get the document being built
        public IRequestDocument GetDocument()
        {
            return doc;
        }

        // start building document
        public void OpenDocument()
        {
            doc = new RequestDocument();
        }
    }
}