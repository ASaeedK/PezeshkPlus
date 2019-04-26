using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PezeshkPlus.Models;
using PezeshkPlus.Models.CustomModel;
using PezeshkPlus.Models.Extension;
using PezeshkPlus.Authoriza;
using PezeshkPlus.Authoriza.CustomFilter;

namespace PezeshkPlus.Controllers
{
    [CustomFilter]
    public class BlogController : Controller
    {
        // GET: Blog
        [Route("تازه-های-سلامت")]
        public ActionResult TopNews()
        {
            PezeshkPlusEntities db = new PezeshkPlusEntities();
            List<USP_SEL_Articles_Result> result = db.USP_SEL_Articles().ToList();
            ViewBag.TopNews = result;

            Search model = new Search();

            return View(model);
        }

        [HttpPost, Route("تازه-های-سلامت/More")]
        public ActionResult More(string name, string id)
        {
            Session["NewsID"] = id;

            return RedirectToAction("TopNewsMore", new { name = name });
        }

        [Route("تازه-های-سلامت/{name}")]
        public ActionResult TopNewsMore(string name)
        {
            PezeshkPlusEntities db = new PezeshkPlusEntities();
            List<USP_SEL_Article_Result> result = db.USP_SEL_Article(Convert.ToInt32(Session["NewsID"].ToString())).ToList();
            ViewBag.Article = result;

            Search model = new Search();

            return View(model);
        }
    }
}