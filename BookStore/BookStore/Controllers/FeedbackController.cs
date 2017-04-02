using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BookStore.Controllers
{
    public class FeedbackController : Controller
    {
        // GET: Feedback
        public ActionResult Index()
        {
            return View();
        }


        //
        // GET: /Feedback/Submit/
        //
        // Process submission of the form on the home page
        //
        public ActionResult Submit(string pattern, string format)
        {
            // put parameters to be redirected into a route value dictionary
            RouteValueDictionary parameters = new RouteValueDictionary();
            parameters.Add("format", format);

            // redirect to the controller that handles the method
            switch (pattern)
            {
                case "feedbackbuilder":
                    // use the Feedback pattern
                    return RedirectToAction("Index", "FeedbackCreate", parameters);

                default:
                    // unrecognised method; return to the blank form
                    return RedirectToAction("Index");
            }

        }
    }
}