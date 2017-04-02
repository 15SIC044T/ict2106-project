using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.Models.Feedback
{
    public class FeedbackBuilder : IFeedbackBuilder
    {
        // the document being built
        private Feedback doc;

        // constructor
        public FeedbackBuilder()
        {
            // nothing to do
        }

        // build the body
        public void BuildBody(string text)
        {
            // add a paragraph containing the text
            doc.Append(new FeedbackParagraph(text));
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
        public IFeedback GetDocument()
        {
            return doc;
        }

        // start building document
        public void OpenDocument()
        {
            doc = new Feedback();
        }
    }
}