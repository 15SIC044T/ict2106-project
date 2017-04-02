using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.Models
{
    public class RecommendedList
    {
        public Item Item { get; set; }
        public string iImage { get; set; }
        public string iName { get; set; }
        public IEnumerable<Item> Items { get; set;}

        public Review Review { get; set; }
        public int itemID { get; set; }
        public IEnumerable<Review> Reviews { get; set; }
    }
}