using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.Models.Requests
{
    public class RequestHeading : IHeading
    {
        private int level;
        private string text;

        // constructor
        public RequestHeading(int levelIn, string textIn)
        {
            level = levelIn;
            text = textIn;
        }

        // get the level of the header
        public int GetLevel()
        {
            return level;
        }

        // plain text headings denoted by uppercase and new line
        public string GetString()
        {
            return text.ToUpper() + "\n";
        }
    }
}