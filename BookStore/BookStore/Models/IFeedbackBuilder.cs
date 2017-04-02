using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.Models
{
    public interface IFeedbackBuilder
    {
        void OpenDocument();
        void BuildHead(string title, string author);
        void BuildBody(string text);
        void CloseDocument();
        IFeedback GetDocument();
    }
}