using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PezeshkPlus.Models;
using PezeshkPlus.Models.CustomModel;
using PezeshkPlus.Authoriza;
using PezeshkPlus.Authoriza.CustomFilter;

namespace PezeshkPlus.Controllers
{
    [CustomFilter]
    public class SearchController : Controller
    {
        // GET: Search
        #region SearchRedirects
        [Route("Search/SearchBox0")]
        public ActionResult SearchBox(Search model)
        {
            return RedirectToAction("Index", model);
        }

        [Route("Search/SearchBox1")]
        public ActionResult SearchBox(RegisterPack rModel)
        {
            Search model = rModel.SearchModel;

            return RedirectToAction("Index", model);
        }

        [Route("Search/SearchBox2")]
        public ActionResult SearchBox(EmailContact eModel)
        {
            Search model = eModel.SearchModel;

            return RedirectToAction("Index", model);
        }

        [Route("Search/SearchBox3")]
        public ActionResult SearchBox(Models.CustomModel.Comment cModel)
        {
            Search model = cModel.SearchModel;

            return RedirectToAction("Index", model);
        }

        [Route("Search/SearchBox4")]
        public ActionResult SearchBox(DoctorProfile pModel)
        {
            Search model = pModel.SearchModel;

            return RedirectToAction("Index", model);
        }
        #endregion

        [HttpGet]
        public ActionResult Index(Search model)
        {
                if (model.Name == null)
                    model.Name = "%";
                if (model.Province == "انتخاب استان" || model.Province == null)
                    model.Province = "%";
                if (model.City == "انتخاب شهر" || model.City == null)
                    model.City = "%";
                if (model.MedicalFieldName == "انتخاب نوع تخصص" || model.MedicalFieldName == null)
                    model.MedicalFieldName = "%";

            PezeshkPlusEntities db = new PezeshkPlusEntities();

            List<string> provinces = db.USP_SEL_Provinces().ToList();
            ViewBag.Provinces = provinces;
            List<string> cities = db.USP_SEL_Cities().ToList();
            ViewBag.Cities = cities;
            List<USP_SEL_ActiveMedicalFields_Result> medicalFields = db.USP_SEL_ActiveMedicalFields().ToList();
            ViewBag.MedicalFields = medicalFields;

            List<USP_SEL_SearchDoctors_Result> result = db.USP_SEL_SearchDoctors(model.Name, model.Province, model.City, model.MedicalFieldName).ToList();
            ViewBag.SearchResult = result;

            Search Model = new Search();

            return View(Model);
        }

        [HttpPost, Route("Search/More")]
        [ValidateAntiForgeryToken]
        public ActionResult More(string name, string email)
        {
            Session["DoctorName"] = name;
            Session["DoctorEmail"] = email;

            return RedirectToAction("DoctorPage", new { name = name });
        }

        [Route("Search/{name}")]
        public ActionResult DoctorPage(string name)
        {
            PezeshkPlusEntities db = new PezeshkPlusEntities();
            List<USP_SEL_Doctor_Result> doctorInfo = db.USP_SEL_Doctor(Session["DoctorEmail"].ToString()).ToList();
            List<USP_SEL_DoctorComments_Result> comments = db.USP_SEL_DoctorComments(Session["DoctorEmail"].ToString()).ToList();

            if (doctorInfo[0].Address == null)
                doctorInfo[0].Address = "ثبت نشده";
            if (doctorInfo[0].ClinicPhone == null)
                doctorInfo[0].ClinicPhone = "ثبت نشده";
            if (doctorInfo[0].WorkTime == null)
                doctorInfo[0].WorkTime = "ثبت نشده";

            ViewBag.DoctorInfo = doctorInfo;
            ViewBag.Comments = comments;

            Models.CustomModel.Comment model = new Models.CustomModel.Comment();

            return View(model);
        }

        [HttpPost, Route("Search/Commenting")]
        [ValidateAntiForgeryToken]
        public ActionResult Commenting(Models.CustomModel.Comment commentModel)
        {
            PezeshkPlusEntities db = new PezeshkPlusEntities();

            try
            {
                if (commentModel.Rate != 0)
                    db.USP_UPD_Rate(Session["DoctorEmail"].ToString(), commentModel.Rate);
                db.USP_INS_Comment(Session["DoctorEmail"].ToString(), commentModel.Text, commentModel.Name, null);
                TempData["CommentSent"] = "دیدگاه شما ارسال شد و پس از تایید مدیر نمایش داده خواهد شد";
            }
            catch (Exception)
            {
                TempData["CommentNotSent"] = "خطایی در ارسال دیدگاه رخ داد!";
            }

            return RedirectToAction("DoctorPage", new { name = Session["DoctorName"].ToString() });
        }
    }
}