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
        private UserGateway userGateway = new UserGateway();
        private CartGateway cartGateway = new CartGateway();
        private CartItemGateway cartItemGateway = new CartItemGateway();
        private BookStoreContext db = new BookStoreContext();
        //private ApplicationDbContext adb = new ApplicationDbContext();

        [Authorize(Roles = "User, PremiumUser, BasicUser, Admin")]
        public ActionResult Index()
        {
            //ApplicationUser user = updateSession();
            updateSession();

            //Create a new cart if the cart is empty
            //if (string.IsNullOrEmpty(Session["currentCart"].ToString()))
            if (Convert.ToInt32(Session["currentCart"]) == 0)
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
                var updateUser = db.Users.Single(x => x.Username == System.Web.HttpContext.Current.User.Identity.Name);
                updateUser.currentCart = newCart.cartID;
                userGateway.Update(updateUser);

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
            var cartDataCarts = new Cart();
            cartDataCarts = db.Carts.SingleOrDefault(x => x.cartID == currentCartID);
            cartData.cartItems = cartItemList;
            cartData.items = itemList;

            //Set default carts values
            cartDataCarts.gst = 7;
            cartDataCarts.discountAmount = 0;
            cartDataCarts.discountPercent = 0;
            cartDataCarts.subTotal = Convert.ToDouble(db.CartItems.AsEnumerable().Where(x => x.cartID == currentCartID).Sum(x => x.price));
            cartDataCarts.totalPrice = cartDataCarts.subTotal + (cartDataCarts.subTotal * cartDataCarts.gst / 100 * cartDataCarts.discountPercent / 100);
            cartData.carts = cartDataCarts;

            viewModel.Add(cartData);

            return View(viewModel);

        }

        [Authorize(Roles = "User, PremiumUser, BasicUser, Admin")]
        public ActionResult AddToCart(int id)
        {
            //ApplicationUser user = updateSession(); 
            updateSession();

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
        [Authorize(Roles = "User, PremiumUser, BasicUser, Admin")]
        public ActionResult RemoveFromCart(int id)
        {
            //ApplicationUser user = updateSession(); 
            updateSession();

            //CurrentCartID
            int currentCartID = Convert.ToInt32(Session["currentCart"]);

            CartItem removeCartItem = db.CartItems.FirstOrDefault(item => item.itemID == id && item.cartID == currentCartID);

            //Delete from cart
            cartItemGateway.Delete(removeCartItem.CId);
            return RedirectToAction("Index");
        }


        public void updateSession()
        {
           
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                Session["username"] = System.Web.HttpContext.Current.User.Identity.Name;
                var userObj = db.Users.Single(x => x.Username == System.Web.HttpContext.Current.User.Identity.Name);

                Session["userID"] = userObj.Id;
                Session["currentCart"] = userObj.currentCart; 
                Session["Role"] = userObj.Role; 
            } 
        }

        [Authorize(Roles = "User, PremiumUser, BasicUser, Admin")]
        public ActionResult OrderSummary()
        {

            //ApplicationUser user = updateSession();
            updateSession();

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
            cartData.carts.subTotal = Convert.ToDouble(db.CartItems.AsEnumerable().Where(x => x.cartID == currentCartID).Sum(x => x.price));
            cartData.carts.totalPrice = cartData.carts.subTotal + (cartData.carts.subTotal * cartData.carts.gst / 100 * cartData.carts.discountPercent / 100);
            cartData.carts.dateOfPurchase = DateTime.Now;

            //Update carts value to existing cart
            cartGateway.Update(cartData.carts);

            //Clear current user shopping cart
            Session["currentCart"] = 0;
            var updateUser = db.Users.Single(x => x.Username == System.Web.HttpContext.Current.User.Identity.Name);
            updateUser.currentCart = currentCartID;
            userGateway.Update(updateUser); 

            viewModel.Add(cartData); 
            return View();
        }

    }
}