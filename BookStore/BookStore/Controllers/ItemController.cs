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

        /*public ActionResult Index()
        {
            return View(itemGateway.SelectAll());
            //return View(db.Tours.ToList());
        }*/

        // GET: Item
        public ActionResult Index(string option, string search, int? pageNumber, string sort)
        {
            List<Item> itemList = db.Items.ToList();
            //pass the StudentList list object to the view.  
            //return View(ProductList);
            // sort 
            ViewBag.SortByName = string.IsNullOrEmpty(sort) ? "descending name" : "";
            ViewBag.SortByCategory = sort == "Category" ? "descending Category" : "Category";

            var records = db.Items.AsQueryable();

            //if a user choose the radio button option as Subject  
            if (option == "Name")
            {
                //Index action method will return a view with a student records based on what a user specify the value in textbox  
                return View(db.Items.Where(x => x.iName.Contains(search) || search == null).ToList().ToPagedList(pageNumber ?? 1, 3));
            }
            else if (option == "Category")
            {
                return View(db.Items.Where(x => x.iCategory.Contains(search) || search == null).ToList().ToPagedList(pageNumber ?? 1, 3));
            }
            else
            {
                return View(db.Items.Where(x => x.iName.Contains(search) || search == null).ToList().ToPagedList(pageNumber ?? 1, 3));
            }

            switch (sort)
            {

                case "descending name":
                    records = records.OrderByDescending(x => x.iName);
                    break;

                case "descending Category":
                    records = records.OrderByDescending(x => x.iCategory);
                    break;

                default:
                    records = records.OrderBy(x => x.iName);
                    break;

            }

            return View(itemList);
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
        //[Route("Create")]
        [HttpPost]
        public ActionResult Create([Bind(Include = "itemID,iName,iDescription,iImage,iPrice,iQuantity,iCategory")] Item item, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Uploads"), fileName);
                    var pathString = Path.Combine("Uploads/", fileName);
                    file.SaveAs(path);

                    item.iImage = pathString;

                    itemGateway.Insert(item);
                    return RedirectToAction("Index");

                }
                else
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
            { if (file.ContentLength > 0)
                    { 
                        var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Uploads"), fileName); 
                    var pathString = "Uploads/" + fileName;
                    file.SaveAs(path);

                        item.iImage = pathString;  

                        itemGateway.Update(item);
                    } 

                //itemGateway.Update(item);
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
    }
}

// Image Upload
// http://haacked.com/archive/2010/07/16/uploading-files-with-aspnetmvc.aspx/