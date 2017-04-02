using BookStore.DAL;
using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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
                                 on c.itemID equals r.itemID
                             select
                                 new RecommendedList
                                 {
                                     itemID = r.itemID,
                                     iImage = c.iImage,
                                     iName = c.iName
                                 }).ToList();

                return View(comps);
            }

        }

        /*private List<Item> GetImages()
        {
            string query = "SELECT * FROM tblFiles";
            List<Item> images = new List<Item>();
            string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            images.Add(new ImageModel
                            {
                                Id = Convert.ToInt32(sdr["Id"]),
                                Name = sdr["Name"].ToString(),
                                ContentType = sdr["ContentType"].ToString(),
                                Data = (byte[])sdr["Data"]
                            });
                        }
                    }
                    con.Close();
                }

                return images;
            }
        }*/

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