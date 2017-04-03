using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.Models
{
    public interface IRequestDocumentBuilder
    {
        void OpenDocument();
        void BuildHead(string title, string author);
        void BuildBody(string text);
        void CloseDocument();
        IRequestDocument GetDocument();
    }
}