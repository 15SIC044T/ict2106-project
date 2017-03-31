using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//Added
using System.Data.Entity;
using BookStore.Models;

namespace BookStore.DAL
{
    public class BookStoreContext : DbContext
    {
        public BookStoreContext() : base("DefaultConnection")
        {

        }

        //public DbSet<User> Users { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Review> Reviews { get; set; }
    }
}