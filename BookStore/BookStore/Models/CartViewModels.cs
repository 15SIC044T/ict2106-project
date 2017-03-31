using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.Models
{
    public class CartViewModels
    {
        public Cart carts { get; set; }
        public List<CartItem> cartItems { get; set; }
        public List<Item> items { get; set; } 
    }
}