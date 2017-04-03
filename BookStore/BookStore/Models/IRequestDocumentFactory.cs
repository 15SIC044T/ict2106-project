using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public interface IRequestDocumentFactory
    {
        IHeading CreateHeading(int level, string text);
        IParagraph CreateParagraph(string text);
    }
}
