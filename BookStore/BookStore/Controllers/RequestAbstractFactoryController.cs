using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Controllers
{
    public class RequestAbstractFactoryController : Controller
    {
        // GET: RequestAbstractFactory
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /RequestAbstractFactory/
        //
        // Home page for creating documents with the Abstract Factory pattern
        //
        public ActionResult Index(string format)
        {
            // pass the output format as view data to be saved in the form
            ViewData["Format"] = format;

            return View();
        }

        //
        // GET: /AbstractFactory/Submit
        //
        // Process a submission.
        //
        public ActionResult Submit(string format, string headingText, string paragraphText)
        {
            // get the proper document factory for the output type
            IRequestDocumentFactory factory = RequestDocumentCreatorSingleton.GetInstance().GetDocumentFactory(format);

            // create the document parts
            IHeading headingObj = factory.CreateHeading(1, headingText);
            IParagraph paragraphObj = factory.CreateParagraph(paragraphText);

            return View("~/Views/RequestBuilder/Submit", headingObj.GetString() + paragraphObj.GetString());
        }
    }
}