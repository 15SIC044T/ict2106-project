using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BookStore.DAL;
using BookStore.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace BookStore.Controllers
{
    public class ReviewController : Controller
    {
        private BookStoreContext db = new BookStoreContext();
        internal IDataGateway<Review> dataGateway;

        public ReviewController()
         {
             dataGateway = new ReviewGateway();
         }

        // GET: Reviews
        public ActionResult Index(int? itemID, string itemName)
        {
            updateSession();
            Session["itemName"] = itemName;
            if (itemID != null)
            {
                //return View(db.Reviews.ToList());
                TempData["reviewItemID"] = itemID;
                int value = itemID.GetValueOrDefault();
                return View(((ReviewGateway)dataGateway).SelectReviewByItemID(itemID.ToString()));
            }
            else
            {
                return View(db.Reviews.ToList());
            }
        }

        // GET: Reviews/Details/5
        public ActionResult Details(int? reviewID, int? itemID)
        {
            if (reviewID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(reviewID);
            if (review == null)
            {
                return HttpNotFound();
            }
            TempData["reviewItemID"] = itemID;
            return View(review);
        }

        // GET: Reviews/Create
        public ActionResult Create(int? itemID)
        {
            TempData["reviewItemID"] = itemID;
            //ApplicationUser user = updateSession();
            TempData["TEST"] = itemID;
            int testing = itemID.GetValueOrDefault();
            TempData["TEST2"] = testing;
            if (itemID != null)
            {
                Review review = new Models.Review();
                review.itemID = (int)itemID;
                updateSession();
                review.reviewerID = Session["userID"].ToString();
                review.reviewerName = Session["username"].ToString();
                review.reviewDate = DateTime.Today;
                return View(review);
            }
            else
            {
                return View();
            }
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "reviewID,itemID,reviewerID,reviewerName,reviewDate,reviewDescription,reviewRating")] Review review)
        {
            if (ModelState.IsValid)
            {
                dataGateway.Insert(review);
                return RedirectToAction("Index",new { itemID = review.itemID , itemName = Session["itemName"] });
            }

            return View(review);
        }

        public void updateSession()
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                Session["username"] = System.Web.HttpContext.Current.User.Identity.Name;
                var userObj = db.Users.Single(x => x.Username == System.Web.HttpContext.Current.User.Identity.Name);

                Session["userID"] = userObj.Id;
            }
        }
        // GET: Reviews/Edit/5
        public ActionResult Edit(int? reviewID, int? itemID)
        {
            if (reviewID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(reviewID);
            if (review == null)
            {
                return HttpNotFound();
            }
            TempData["reviewItemID"] = itemID;
            return View(review);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "reviewID,itemID,reviewerID,reviewerName,reviewDate,reviewDescription,reviewRating")] Review review)
        {
            if (ModelState.IsValid)
            {
                dataGateway.Update(review);
                return RedirectToAction("Index", new { itemID = review.itemID, itemName = Session["itemName"] });
            }
            TempData["reviewItemID"] = review.reviewID;
            return View(review);
        }

        // GET: Reviews/Delete/5
        public ActionResult Delete(int? reviewID, int? itemID)
        {
            if (reviewID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(reviewID);
            if (review == null)
            {
                return HttpNotFound();
            }
            TempData["reviewItemID"] = itemID;
            return View(review);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int reviewID)
        {
            Review review = db.Reviews.Find(reviewID);
            db.Reviews.Remove(review);
            db.SaveChanges();
            return RedirectToAction("Index", new { itemID = review.itemID });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
