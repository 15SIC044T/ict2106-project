using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BookStore.Controllers
{
    public class RequestController : Controller
    {
        // GET: Request
        public ActionResult Index(string format)
        {
            // pass the output format as view data to be saved in the form
            ViewData["Format"] = "request";
            return View();
        }

        //
        // GET: /Request/Submit/
        //
        // Process submission of the form on the home page
        //
        public ActionResult Submit(string pattern, string format)
        {
            // put parameters to be redirected into a route value dictionary
            RouteValueDictionary parameters = new RouteValueDictionary();
            parameters.Add("format", "request");
            pattern = "requestbuilder";

            // redirect to the controller that handles the method
            switch (pattern)
            {
                case "requestbuilder":
                    // use the Feedback pattern
                    return RedirectToAction("Index", "RequestBuilder", parameters);

                default:
                    // unrecognised method; return to the blank form
                    return RedirectToAction("Index");
            }

        }

        public ActionResult Results()
        {
            return View();
        }
    }
}