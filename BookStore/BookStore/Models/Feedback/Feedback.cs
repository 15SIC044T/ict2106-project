using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace BookStore.Models.Feedback
{
    public class Feedback : IFeedback
    {
        // the title of the document
        private string title = null;

        // the author of the document
        private string author = null;

        // the elements making up the body of the document
        private IList<FeedbackElement> body = new List<FeedbackElement>();

        // constructor
        public Feedback()
        {
            // nothing to do
        }

        // append an element to the body of the document
        public void Append(FeedbackElement elementIn)
        {
            body.Add(elementIn);
        }

        // make a deep clone of the document
        public IFeedback Clone()
        {
            Feedback clone = new Feedback();
            clone.SetTitle(title);
            clone.SetAuthor(author);

            return clone;
        }

        public string GetString()
        {
            // create a new string containing the document
            StringBuilder sbuilder = new StringBuilder();

            // add the title and author
            if (title != null)
                sbuilder.Append("Title: " + title.ToUpper() + "\n");
            if (author != null)
                sbuilder.Append("Author: " + author + "\n");

            // start the document body
            sbuilder.Append("\n");

            // append the body elements
            foreach (FeedbackElement elem in body)
                sbuilder.Append(elem.GetString());

            // end the document
            sbuilder.Append("\n");

            return sbuilder.ToString();
        }

        // set the author of the document
        public void SetAuthor(string authorIn)
        {
            author = authorIn;
        }


        // set the title of the document
        public void SetTitle(string titleIn)
        {
            title = titleIn;
        }
    }
}