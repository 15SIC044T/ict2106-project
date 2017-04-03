using BookStore.DAL;
using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (BookStoreContext db = new BookStoreContext())
            {
                

                var comps = (from c in db.Items
                             join r in db.Reviews
                             on c.itemID.ToString() equals r.itemID
                             select new RecommendedList
                                 {
                                     itemID = r.itemID,
                                     iImage = c.iImage,
                                     iName = c.iName
                                 }).Distinct().ToList();

                return View(comps);
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}