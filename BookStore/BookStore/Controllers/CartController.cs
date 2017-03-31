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

namespace BookStore.Controllers
{
    public class CartController : Controller
    {
        private CartGateway cartGateway = new CartGateway();

        public ActionResult Index()
        {
            return View(cartGateway.SelectAll());
            //return View(db.Carts.ToList());
        }

        // GET: Cart/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = cartGateway.SelectById(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return View(cart);
        }

        // GET: Cart/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cart/Create
        [HttpPost]
        public ActionResult Create([Bind(Include = "Id,iName,iDescription,iImage,iPrice,iQuantity,iCategory")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                cartGateway.Insert(cart);
                return RedirectToAction("Index");
            }

            return View(cart);
        }

        // GET: Cart/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = cartGateway.SelectById(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return View(cart);
        }

        // POST: Cart/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, [Bind(Include = "Id,iName,iDescription,iImage,iPrice,iQuantity,iCategory")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                cartGateway.Update(cart);
                return RedirectToAction("Index");
            }
            return View(cart);
        }

        // GET: Cart/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = cartGateway.SelectById(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return View(cart);
        }

        // POST: Cart/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            Cart cart = cartGateway.Delete(id);
            return RedirectToAction("Index");
        }
    }
}