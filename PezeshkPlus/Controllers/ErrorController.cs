using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PezeshkPlus.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            

            return View();
        }
        public ActionResult NotFound()
        {
            if (Response.StatusCode == 200)
                Response.StatusCode = 404;
            return View();
        }
    }
}