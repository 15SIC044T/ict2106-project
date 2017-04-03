using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Controllers
{
    public class FeedbackCreateController : Controller
    {
        //
        // GET: /Builder/
        //
        // Home page for creating documents using the Builder pattern
        //
        public ActionResult Index(string format)
        {
            // pass the output format as view data to be saved in the form
            ViewData["Format"] = format;

            return View();
        }

        public ActionResult Submit(string format, string titleText, string authorText, string bodyText)
        {
            // get the proper document builder for the output type
            IFeedbackBuilder builder = FeedbackCreatorSingleton.GetInstance().GetDocumentBuilder(format);

            // start building
            builder.OpenDocument();

            // build the head with the title and author
            builder.BuildHead(titleText, authorText);

            // build the body 
            builder.BuildBody(bodyText);

            // finished building
            builder.CloseDocument();

            //return View("~/Views/Feedback/Results", builder.GetDocument().GetString());
            return View("~/Views/Feedback/Submit", builder.GetDocument().GetString());
        }
    }
}