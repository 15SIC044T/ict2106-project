using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//Added
using BookStore.Models;

namespace BookStore.DAL
{
    public class ReviewGateway : DataGateway<Review>
    {
        public IEnumerable<Review> SelectReviewByItemID(int itemID)
        {
            return from p in this.data
                   where p.itemID == itemID
                   select p;
        }
    }
}