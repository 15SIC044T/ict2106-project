using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

//Added
using System.Data;
using BookStore.DAL;
using BookStore.Models;
using System.Net;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace BookStore.Controllers
{
    public class CartController : Controller
    {
        private CartGateway cartGateway = new CartGateway();
        private CartItemGateway cartItemGateway = new CartItemGateway();
        private BookStoreContext db = new BookStoreContext();
        private ApplicationDbContext adb = new ApplicationDbContext();


        public ActionResult Index()
        {
            ApplicationUser user = updateSession();

            //Create a new cart if the cart is empty
            //if (string.IsNullOrEmpty(Session["currentCart"].ToString()))
            if (Session["currentCart"] == null)
            {
                //Create a new cart
                Cart newCart = new Cart();
                newCart.cartUserID = Session["userID"].ToString();
                cartGateway.Insert(newCart);

                //After create a new cart, get and assign current cart to applicationUser
                //After create a new cart, get and assign current cart to session

                string cartUserID = Session["userID"].ToString();

                //var currentCart = db.Carts.SingleOrDefault(c => c.cartUserID.Equals(cartUserID) && c.dateOfPurchase.Equals(null));
                Session["currentCart"] = newCart.cartID;
                ApplicationUser u = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
                u.currentCart = newCart.cartID;
                System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().Update(u);
            }

            //Get a MAIN single cart ID
            //Get all the cart items   
            int currentCartID = Convert.ToInt32(Session["currentCart"]);
            List<CartItem> cartItemList = db.CartItems.Where(x => x.cartID == currentCartID).ToList();

            List<Item> itemList = new List<Item>();
             
            //Get all the item details and store it in the list;
            IIterator<CartItem> iter = new IteratorGeneric<CartItem>(cartItemList);
            while (!iter.IsDone())
            {
                int? newItemID = iter.current().itemID; 
                if (newItemID != null)
                {
                    itemList.Add(db.Items.SingleOrDefault(x => x.itemID == newItemID));
                    iter.Next();
                }
            }

            //Create a new model to contain all the data..

            List<CartViewModels> viewModel = new List<CartViewModels>();

            var cartData = new CartViewModels();
            cartData.carts = db.Carts.SingleOrDefault(x => x.cartID == currentCartID);
            cartData.cartItems = cartItemList;
            cartData.items = itemList;

            //Set default carts values
            cartData.carts.gst = 7;
            cartData.carts.discountAmount = 0;
            cartData.carts.discountPercent = 0;
            cartData.carts.subTotal = Convert.ToDecimal(db.CartItems.AsEnumerable().Where(x => x.cartID == currentCartID).Sum(x => x.price));
            cartData.carts.totalPrice = cartData.carts.subTotal + (cartData.carts.subTotal * cartData.carts.gst / 100 * cartData.carts.discountPercent / 100);

            viewModel.Add(cartData);

            return View(viewModel);

        }

        
        public ActionResult AddToCart(int id)
        {
            ApplicationUser user = updateSession(); 

            //CurrentCartID
            int currentCartID = Convert.ToInt32(Session["currentCart"]);

            // Retrieve the album from the database
            Item addedItem = db.Items.Single(item => item.itemID == id);
            CartItem addedItemCart = db.CartItems.FirstOrDefault(item => item.itemID == id && item.cartID == currentCartID);

            if (addedItemCart == null)
            {
                // Add it to the shopping cart
                CartItem cartItem = new CartItem();
                cartItem.cartID = currentCartID;
                cartItem.itemID = id;
                cartItem.quantity = 1;
                cartItem.price = addedItem.iPrice * cartItem.quantity;
                cartItemGateway.Insert(cartItem);
            }
            else
            {
                //Update existing item into the cart
                addedItemCart.quantity += 1;
                addedItemCart.price = addedItem.iPrice * addedItemCart.quantity;
                cartItemGateway.Update(addedItemCart); 
            }

            // Go back to the main store page for more shopping
            return RedirectToAction("Index");
        }
        //
        // AJAX: /ShoppingCart/RemoveFromCart/5 
        public ActionResult RemoveFromCart(int id)
        {
            ApplicationUser user = updateSession(); 

            //CurrentCartID
            int currentCartID = Convert.ToInt32(Session["currentCart"]);

            CartItem removeCartItem = db.CartItems.FirstOrDefault(item => item.itemID == id && item.cartID == currentCartID);

            //Delete from cart
            cartItemGateway.Delete(removeCartItem.CId);
            return RedirectToAction("Index");
        }

        public ApplicationUser updateSession()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                Session["username"] = System.Web.HttpContext.Current.User.Identity.Name;
                Session["userID"] = System.Web.HttpContext.Current.User.Identity.GetUserId();

                //user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
                Session["currentCart"] = user.currentCart;
            }
            return user;
        }

        
        public ActionResult OrderSummary()
        {

            ApplicationUser user = updateSession();

            //Get a MAIN single cart ID
            //Get all the cart items   
            int currentCartID = Convert.ToInt32(Session["currentCart"]);
            List<CartItem> cartItemList = db.CartItems.Where(x => x.cartID == currentCartID).ToList();

            List<Item> itemList = new List<Item>();

            //Get all the item details and store it in the list;
            IIterator<CartItem> iter = new IteratorGeneric<CartItem>(cartItemList);
            while (!iter.IsDone())
            {
                int? newItemID = iter.current().itemID;
                if (newItemID != null)
                {
                    itemList.Add(db.Items.SingleOrDefault(x => x.itemID == newItemID));
                    iter.Next();
                }
            }

            //Create a new model to contain all the data..

            List<CartViewModels> viewModel = new List<CartViewModels>();

            var cartData = new CartViewModels();
            cartData.carts = db.Carts.SingleOrDefault(x => x.cartID == currentCartID);
            cartData.cartItems = cartItemList;
            cartData.items = itemList;

            //Set default carts values
            cartData.carts.gst = 7;
            cartData.carts.discountAmount = 0;
            cartData.carts.discountPercent = 0;
            cartData.carts.subTotal = Convert.ToDecimal(db.CartItems.AsEnumerable().Where(x => x.cartID == currentCartID).Sum(x => x.price));
            cartData.carts.totalPrice = cartData.carts.subTotal + (cartData.carts.subTotal * cartData.carts.gst / 100 * cartData.carts.discountPercent / 100);
            cartData.carts.dateOfPurchase = DateTime.Now;

            //Update carts value to existing cart
            cartGateway.Update(cartData.carts);

            //Clear current user shopping cart
            Session["currentCart"] = "";
            ApplicationUser u = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            u.currentCart = null;
            System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().Update(u);


            viewModel.Add(cartData);
            //return View(viewModel);
            return View();
        }

    }
}