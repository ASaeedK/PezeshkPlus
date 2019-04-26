using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PezeshkPlus.Models;
using PezeshkPlus.Models.CustomModel;
using PezeshkPlus.Authoriza;
using PezeshkPlus.Authoriza.CustomFilter;
using System.IO;

namespace PezeshkPlus.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        [Route("Admin/1234-Login")]
        public ActionResult Login()
        {
            //09198221786
            AdminLogin model = new AdminLogin();
            Uri previousUri = Request.UrlReferrer;
            if (previousUri != null)
                Session["PreviousUrl"] = previousUri.AbsolutePath;
            else
                Session["PreviousUrl"] = "~/Admin/Panel";

            return View(model);
        }

        [HttpPost, Route("Admin/Login")]
        [ValidateAntiForgeryToken]
        public ActionResult Login(AdminLogin admin)
        {
            ViewBag.IsLogin = false;
            PezeshkPlusEntities db = new PezeshkPlusEntities();
            List<int?> CheckMail = db.USP_SEL_Email(admin.Email).ToList();
            if (CheckMail.Count == 0)
            {
                TempData["WrongEmail"] = "ایمیل وارد شده صحیح نمی باشد!";
                return RedirectToAction("Login");
            }

            List<int?> CheckPass = db.USP_SEL_Password(admin.Email, admin.Password).ToList();
            if (CheckPass.Count == 0)
            {
                TempData["WrongPass"] = "کلمه عبور وارد شده صحیح نمی باشد!";
                return RedirectToAction("Login");
            }

            string authID = Guid.NewGuid().ToString();
            Session["AuthID"] = authID;

            Response.Cookies["AuthID"].Value = authID;

            Session[authID] = admin.Email;
            Session[authID + "IP"] = Request.UserHostAddress;

            string previousUrl = Session["PreviousUrl"].ToString();
            Session.Remove("PreviousUrl");

            return Redirect(previousUrl);
        }

        [CustomFilter]
        [AdminAuthorize]
        public ActionResult Panel()
        {
            PezeshkPlusEntities db = new PezeshkPlusEntities();

            List<USP_SEL_NotActivatedComments_Result> notActivatedComments = db.USP_SEL_NotActivatedComments().ToList();
            ViewBag.NotActivatedComments = notActivatedComments;
            List<USP_SEL_NotActivatedDoctors_Result> notActivatedDoctors = db.USP_SEL_NotActivatedDoctors().ToList();
            ViewBag.NotActivatedDoctors = notActivatedDoctors;

            Search model = new Search();

            return View(model);
        }

        [Route("Admin/Doctor/{id:int}")]
        [AdminAuthorize]
        public ActionResult AcceptDoctor()
        {
            try
            {
                string url = Request.Url.AbsolutePath;
                url = url.Substring(url.LastIndexOf('/') + 1, (url.Length - 1) - url.LastIndexOf('/'));

                PezeshkPlusEntities db = new PezeshkPlusEntities();
                db.USP_UPD_ActiveDoctor(Convert.ToInt32(url));

                TempData["ActiveDoctor"] = "دکتر تایید شد";
            }
            catch (Exception)
            {
                TempData["ActiveDoctor"] = "خطا دکتر تایید نشد";
            }
            return RedirectToAction("Panel");
        }

        [Route("Admin/Doctor/Reject/{id:int}")]
        [AdminAuthorize]
        public ActionResult RejectDoctor()
        {
            try
            {
                string url = Request.Url.AbsolutePath;
                url = url.Substring(url.LastIndexOf('/') + 1, (url.Length - 1) - url.LastIndexOf('/'));

                PezeshkPlusEntities db = new PezeshkPlusEntities();
                db.USP_DEL_Doctor(Convert.ToInt32(url));

                TempData["ActiveDoctor"] = "دکتر رد شد";
            }
            catch (Exception)
            {
                TempData["ActiveDoctor"] = "خطا دکتر رد نشد";
            }
            return RedirectToAction("Panel");
        }

        [Route("Admin/Comment/{id:int}")]
        [AdminAuthorize]
        public ActionResult AcceptComment()
        {
            try
            {
                string url = Request.Url.AbsolutePath;
                url = url.Substring(url.LastIndexOf('/') + 1, (url.Length - 1) - url.LastIndexOf('/'));

                PezeshkPlusEntities db = new PezeshkPlusEntities();
                db.USP_UPD_ActiveComment(Convert.ToInt32(url));

                TempData["ActiveComment"] = "کامنت تایید شد";
            }
            catch (Exception)
            {
                TempData["ActiveComment"] = "خطا کامنت تایید نشد";
            }
            return RedirectToAction("Panel");
        }

        [Route("Admin/Comment/Reject/{id:int}")]
        [AdminAuthorize]
        public ActionResult RejectComment()
        {
            try
            {
                string url = Request.Url.AbsolutePath;
                url = url.Substring(url.LastIndexOf('/') + 1, (url.Length - 1) - url.LastIndexOf('/'));

                PezeshkPlusEntities db = new PezeshkPlusEntities();
                db.USP_DEL_Comment(Convert.ToInt32(url));

                TempData["ActiveComment"] = "کامنت رد شد";
            }
            catch (Exception)
            {
                TempData["ActiveComment"] = "خطا کامنت رد نشد";
            }
            return RedirectToAction("Panel");
        }

        [HttpPost]
        [AdminAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult NewArticle(string Name, string Topic, string Text, HttpPostedFileBase Picture)
        {
            try
            {
                PezeshkPlusEntities db = new PezeshkPlusEntities();
                string picAddress;
                if (Picture != null)
                {
                    if (Path.GetExtension(Picture.FileName) != ".jpg" && Path.GetExtension(Picture.FileName) != ".JPG" && Path.GetExtension(Picture.FileName) != ".png" && Path.GetExtension(Picture.FileName) != ".PNG" && Path.GetExtension(Picture.FileName) != ".jpeg")
                    {
                        TempData["PictureError"] = "پسوند فایل ارسالی باید jpg یا jpeg یا png باشد";
                        return RedirectToAction("Panel");
                    }
                    if (Picture.ContentLength > 1 & Picture.ContentLength < (5 * 1024 * 1024))
                    {
                        string extention = Path.GetExtension(Picture.FileName);
                        string guidNamePic = Guid.NewGuid().ToString();
                        picAddress = $"/Images/Article/{guidNamePic}{extention}";
                        Picture.SaveAs(Server.MapPath("~" + picAddress));
                    }
                    else
                    {
                        TempData["PictureError"] = "سایز فایل عکس ارسال شده باید حداکثر 5 Mg باشد";
                        return RedirectToAction("Panel");
                    }
                }
                else
                {
                    TempData["PictureError"] = "عکس اجباری است";
                    return RedirectToAction("Panel");
                }

                db.USP_INS_Article(Topic, Text, Name, picAddress);
                TempData["ArticleSent"] = "مقاله ثبت شد";
            }
            catch (Exception)
            {
                TempData["ArticleNotSent"] = "خطا مقاله ثبت نشد";
            }
            return RedirectToAction("Panel");
        }

        [AdminAuthorize]
        public ActionResult Logout()
        {
            string authID = Session["AuthID"].ToString();
            Session.Remove(authID);
            Session.Remove(authID + "IP");
            Session.Remove("AuthID");
            Request.Cookies.Remove("AuthID");

            return RedirectToAction("Index", "Home");
        }
    }
}