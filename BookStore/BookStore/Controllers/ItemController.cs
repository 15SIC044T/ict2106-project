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
using PagedList;
using System.Data.Entity.Infrastructure;
using System.IO;

namespace BookStore.Controllers
{
    public class ItemController : Controller
    {
        private ItemGateway itemGateway = new ItemGateway();
        private BookStoreContext db = new BookStoreContext();
         
        // GET: Item
        public ActionResult Index(string option, string search, int? pageNumber, string sort)
        { 
            //Search by name, description and category
            List<Item> itemList = (db.Items.Where(x => x.iName.Contains(search) || x.iDescription.Contains(search) || x.iCategory.Contains(search) || search == null).OrderBy(x=>x.iName).ThenBy(x=>x.iCategory).ToList());
  
            if (System.Web.HttpContext.Current.User.Identity.Name != null) {
                updateSession();
                List<bool> itemListCheck = null;
                //Using iterator to check for every item if its available or not
                //Get all the item details and store it in the list;
                if (Convert.ToInt32(Session["currentCart"]) > 0) { 
                    int currentCartID = Convert.ToInt32(Session["currentCart"]);
                    itemListCheck = new List<bool>();
                 
                    IIterator<Item> iter = new IteratorGeneric<Item>(itemList); 

                    while (!iter.IsDone())
                    {
                        int? newItemID = iter.current().itemID;
                        if (newItemID != null)
                        {
                            CartItem item = db.CartItems.SingleOrDefault(x => x.itemID == newItemID && x.cartID == currentCartID);
                            if (item == null)
                                itemListCheck.Add(false);
                            else
                                itemListCheck.Add(true);

                            iter.Next();
                        }
                    }

                    ViewBag.check = itemListCheck;

                    }
            }

            return View(itemList.ToPagedList(pageNumber ?? 1, 3));
            //return View(itemGateway.SelectAll());
        }

        // GET: Item/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = itemGateway.SelectById(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // GET: Item/Create 
        public ActionResult Create()
        {
             return View();
        }

        // POST: Item/Create 
        [HttpPost] 
        public ActionResult Create([Bind(Include = "itemID,iName,iDescription,iImage,iPrice,iQuantity,iCategory")] Item item, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    if (file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var path = Path.Combine(Server.MapPath("~/Uploads"), fileName);
                        var pathString = Path.Combine("Uploads/", fileName);
                        file.SaveAs(path);

                        item.iImage = pathString;
                    } 

                } 
                    itemGateway.Insert(item);
                    return RedirectToAction("Index");  
            }

            return View(item);
        }

        // GET: Item/Edit/5  
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = itemGateway.SelectById(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Item/Edit/5
        [HttpPost] 
        public ActionResult Edit(int id, [Bind(Include = "itemID,iName,iDescription,iImage,iPrice,iQuantity,iCategory")] Item item, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    if (file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var path = Path.Combine(Server.MapPath("~/Uploads"), fileName);
                        var pathString = "Uploads/" + fileName;
                        file.SaveAs(path);

                        item.iImage = pathString;
                    }
                }
                else {
                    var existingItem = db.Items.SingleOrDefault(x => x.itemID == item.itemID);
                    item.iImage = existingItem.iImage;
                }
                itemGateway.Update(item); 
                return RedirectToAction("Index"); 
            }
            return View(item);
        }

        // GET: Item/Delete/5 
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = itemGateway.SelectById(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Item/Delete/5
        [HttpPost] 
        public ActionResult Delete(int id)
        {
            Item item = itemGateway.Delete(id);
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
    }

}

// Image Upload
// http://haacked.com/archive/2010/07/16/uploading-files-with-aspnetmvc.aspx/