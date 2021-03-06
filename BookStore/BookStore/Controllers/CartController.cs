﻿using System;
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
        private ItemGateway itemGateway = new ItemGateway();
        private CartItemGateway cartItemGateway = new CartItemGateway();
        private BookStoreContext db = new BookStoreContext(); 
         
        [Authorize]
        public ActionResult Index()
        { 
            updateSession();

            createNewCart();

            //Get a MAIN single cart ID
            //Get all the cart items   
            int currentCartID = Convert.ToInt32(Session["currentCart"]);
            List<CartItem> cartItemList = db.CartItems.Where(x => x.cartID == currentCartID).ToList();

            List<Item> itemList = new List<Item>();
            List<bool> checkItemAvailable = new List<bool>();

            //Get all the item details and store it in the list;
            List<bool> itemListCheck = new List<bool>();
            IIterator<CartItem> iter = new IteratorGeneric<CartItem>(cartItemList);
            while (!iter.IsDone())
            {
                int? newItemID = iter.current().itemID; 
                if (newItemID != null)
                {
                    Item item = db.Items.SingleOrDefault(x => x.itemID == newItemID);
                    if (item.iQuantity < iter.current().quantity)
                        itemListCheck.Add(false);
                    else
                        itemListCheck.Add(true);
                    itemList.Add(item);
                    
                    iter.Next();
                }
            }

            //Create a new model to contain all the data.. 
            List<CartViewModels> viewModel = new List<CartViewModels>();
            
                var updateUsers = db.Users.Single(x => x.Username == System.Web.HttpContext.Current.User.Identity.Name);

            var cartData = new CartViewModels();
            var cartDataCarts = new Cart();
            cartDataCarts = db.Carts.SingleOrDefault(x => x.cartID == currentCartID);
            cartData.cartItems = cartItemList;
            cartData.items = itemList;

            double discountPercent = 0;
            double deliveryCharge = 20;
            if (System.Web.HttpContext.Current.User.IsInRole("PremiumUser")) {
                discountPercent = 20;
                deliveryCharge = 0;
            }
            else if (System.Web.HttpContext.Current.User.IsInRole("BasicUser")) {
                discountPercent = 10; 
            } 

            //Set default carts values
            cartDataCarts.gst = 7; 
            cartDataCarts.discountPercent = discountPercent; 
            cartDataCarts.subTotal = Convert.ToDouble(db.CartItems.AsEnumerable().Where(x => x.cartID == currentCartID).Sum(x => x.price));
            cartDataCarts.discountAmount = cartDataCarts.subTotal * cartDataCarts.discountPercent / 100.0;
            cartDataCarts.deliveryCharge = deliveryCharge;
            cartDataCarts.totalPrice = (cartDataCarts.subTotal + deliveryCharge - cartDataCarts.discountAmount) * (1 + (cartDataCarts.gst / 100.0));
              
            cartData.carts = cartDataCarts;

            viewModel.Add(cartData);
            ViewBag.check = itemListCheck;
            return View(viewModel);

        }
         
        [Authorize]
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
        [Authorize]
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
         
        [Authorize]
        public ActionResult OrderSummary()
        { 
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
                    Item newItem = db.Items.SingleOrDefault(x => x.itemID == newItemID);
                    itemList.Add(newItem);

                    //Minus Item Quantity
                    newItem.iQuantity -= 1;
                    itemGateway.Update(newItem);

                    iter.Next();
                }
            } 

            //Create a new model to contain all the data..

            List<CartViewModels> viewModel = new List<CartViewModels>();

            var cartData = new CartViewModels();
            cartData.carts = db.Carts.SingleOrDefault(x => x.cartID == currentCartID);
            cartData.cartItems = cartItemList;
            cartData.items = itemList;

            double discountPercent = 0;
            double deliveryCharge = 20;
            if (System.Web.HttpContext.Current.User.IsInRole("PremiumUser"))
            {
                discountPercent = 20;
                deliveryCharge = 0;
            }
            else if (System.Web.HttpContext.Current.User.IsInRole("BasicUser"))
            {
                discountPercent = 10;
            }

            //Set default carts values
            cartData.carts.gst = 7;
            cartData.carts.discountPercent = discountPercent;
            cartData.carts.subTotal = Convert.ToDouble(db.CartItems.AsEnumerable().Where(x => x.cartID == currentCartID).Sum(x => x.price));
            cartData.carts.discountAmount = cartData.carts.subTotal * cartData.carts.discountPercent / 100.0;
            cartData.carts.deliveryCharge = deliveryCharge;
            cartData.carts.totalPrice = (cartData.carts.subTotal + deliveryCharge - cartData.carts.discountAmount) * (1 + (cartData.carts.gst / 100.0));
            cartData.carts.dateOfPurchase = DateTime.Now;

            //Update carts value to existing cart
            cartGateway.Update(cartData.carts);

                //Clear current user shopping cart
                Session["currentCart"] = 0;
                var updateUser = db.Users.Single(x => x.Username == System.Web.HttpContext.Current.User.Identity.Name);
                updateUser.currentCart = 0;
                userGateway.Update(updateUser);


            createNewCart();


            viewModel.Add(cartData); 
            return View();
        }

        public void createNewCart()
        {
            //Create new shopping cart
            //Create a new cart if the cart is empty 
            if (Convert.ToInt32(Session["currentCart"]) == 0)
            {
                //Create a new cart
                Cart newCart = new Cart();
                newCart.cartUserID = Session["userID"].ToString();
                cartGateway.Insert(newCart);

                //After create a new cart, get and assign current cart to applicationUser
                //After create a new cart, get and assign current cart to session

                string cartUserID = Session["userID"].ToString();

                Session["currentCart"] = newCart.cartID;
                var updateUser = db.Users.Single(x => x.Username == System.Web.HttpContext.Current.User.Identity.Name);
                updateUser.currentCart = newCart.cartID;
                userGateway.Update(updateUser);

            } 
        }

        [Authorize]
        public ActionResult OrderHistory()
        {
            updateSession();
            string currentUserID = Session["userID"].ToString();
            List<Cart> cartData = db.Carts.Where(x => x.cartUserID == currentUserID && x.dateOfPurchase != null).OrderByDescending(x=> x.dateOfPurchase).ToList();

            return View(cartData);

        }

        [Authorize]
        public ActionResult OrderDetails(int? existingCartID)
        {
            updateSession();
            ViewBag.ID = existingCartID;

            //Get all the cart items    
            List<CartItem> cartItemList = db.CartItems.Where(x => x.cartID == existingCartID).ToList();

            List<Item> itemList = new List<Item>();
            List<bool> checkItemAvailable = new List<bool>();

            //Get all the item details and store it in the list;
            IIterator<CartItem> iter = new IteratorGeneric<CartItem>(cartItemList);
            while (!iter.IsDone())
            {
                int? newItemID = iter.current().itemID;
                if (newItemID != null)
                {
                    Item item = db.Items.SingleOrDefault(x => x.itemID == newItemID);
                    itemList.Add(item);

                    iter.Next();
                }
            }

            //Create a new model to contain all the data.. 
            List<CartViewModels> viewModel = new List<CartViewModels>();
             
            var cartData = new CartViewModels();
            var cartDataCarts = new Cart();
            cartDataCarts = db.Carts.SingleOrDefault(x => x.cartID == existingCartID);
            cartData.cartItems = cartItemList;
            cartData.items = itemList;
            cartData.carts = cartDataCarts;

            viewModel.Add(cartData);

            return View(viewModel);
        }

    }
}