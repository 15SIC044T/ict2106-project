using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Controllers
{
    public class RequestBuilderController : Controller
    {
        //
        // GET: /Request/
        //
        // Home page for creating documents using the Builder pattern
        //
        public ActionResult Index(string format)
        {
            // pass the output format as view data to be saved in the form
            ViewData["Format"] = "request";

            return View();
        }

        //
        // GET: /RequestBuilder/Submit
        //
        // Process a submission.
        //
        public ActionResult Submit(string format, string titleText, string authorText, string bodyText)
        {
            // get the proper document builder for the output type
            IRequestDocumentBuilder builder = RequestDocumentCreatorSingleton.GetInstance().GetDocumentBuilder("request");

            // start building
            builder.OpenDocument();

            // build the head with the title and author
            builder.BuildHead(titleText, authorText);

            // build the body 
            builder.BuildBody(bodyText);

            // finished building
            builder.CloseDocument();

            return View();
            //return View("~/Views/RequestBuilder/Submit", builder.GetDocument().GetString());
            //return RedirectToAction("Submit", "RequestBuilder", builder.GetDocument().GetString());
        }

    }
}