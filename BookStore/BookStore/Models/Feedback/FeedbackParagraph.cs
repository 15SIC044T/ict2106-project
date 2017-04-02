using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.Models.Feedback
{
    public class FeedbackParagraph : FeedbackElement, IParagraph
    {
        private string text;

        // constructor
        public FeedbackParagraph(string textIn)
        {
            text = textIn;
        }

        // create a deep clone of the heading
        public override FeedbackElement Clone()
        {
            return new FeedbackParagraph(text);
        }

        // paragraphs in Text are denoted as text and blank line 
        public override string GetString()
        {
            return "\n" + text + "\n";
        }
    }
}