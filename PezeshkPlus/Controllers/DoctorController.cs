using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PezeshkPlus.Models;
using PezeshkPlus.Models.CustomModel;
using PezeshkPlus.Authoriza;
using PezeshkPlus.Authoriza.CustomFilter;
using System.Web.Security;
using System.IO;

namespace PezeshkPlus.Controllers
{
    public class DoctorController : Controller
    {
        // GET: Docter
        [HttpGet]
        [DoctorLoggedAuthorize]
        public ActionResult Register()
        {
            ViewBag.IsLogin = false;
            RegisterPack model = new RegisterPack();

            Uri previousUri = Request.UrlReferrer;
            if (previousUri != null)
                Session["PreviousUrl"] = previousUri.AbsolutePath;
            else
                Session["PreviousUrl"] = "~/Home";

            PezeshkPlusEntities db = new PezeshkPlusEntities();
            ViewBag.MedicalFields = db.USP_SEL_MedicalFields().ToList();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Confirmation(RegisterPack doctor)
        {
            ViewBag.IsLogin = false;
            try
            {
                if (!ModelState.IsValid)
                {
                    if (doctor.RegisterModel.Agree.ToString() == "False")
                    {
                        return View("Register", doctor);
                    }
                    else
                    {
                        TempData["Error"] = "خطا در مقادیر ورودی...دوباره امتحان کنید";
                        return RedirectToAction("Register", doctor);
                    }
                }
                PezeshkPlusEntities db = new PezeshkPlusEntities();

                List<int?> medicalFieldID = db.USP_SEL_MedicalFieldID(doctor.RegisterModel.MedicalField).ToList();

                db.USP_INS_Doctor(doctor.RegisterModel.FirstName, doctor.RegisterModel.LastName, doctor.RegisterModel.Email, doctor.RegisterModel.Password, doctor.RegisterModel.PhoneNumber, doctor.RegisterModel.Province, doctor.RegisterModel.City, medicalFieldID[0], doctor.RegisterModel.NationalID, doctor.RegisterModel.ClinicPhone, doctor.RegisterModel.Address);

                return View();
            }
            catch (Exception)
            {
                TempData["Error"] = "خطا...دوباره امتحان کنید";
                return RedirectToAction("Register");
            }
        }

        #region Validations
        [HttpPost]
        public JsonResult EmailValidation(RegisterPack doctor)
        {
            PezeshkPlusEntities db = new PezeshkPlusEntities();

            List<int?> CheckMail = db.USP_SEL_Email(doctor.RegisterModel.Email).ToList();
            if (CheckMail.Count == 1)
                return Json("با این ایمیل قبلا ثبت نام شده است", JsonRequestBehavior.DenyGet);
            return Json(true, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public JsonResult NationalIDValidation(RegisterPack doctor)
        {
            PezeshkPlusEntities db = new PezeshkPlusEntities();

            List<int?> CheckNationalID = db.USP_SEL_NationalID(doctor.RegisterModel.NationalID).ToList();
            if (CheckNationalID.Count == 1)
                return Json("با این کد ملی قبلا ثبت نام شده است", JsonRequestBehavior.DenyGet);
            return Json(true, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public JsonResult PhoneNumberValidation(RegisterPack doctor)
        {
            PezeshkPlusEntities db = new PezeshkPlusEntities();

            List<int?> CheckPhoneNumber = db.USP_SEL_PhoneNumber(doctor.RegisterModel.PhoneNumber).ToList();
            if (CheckPhoneNumber.Count == 1)
                return Json("با این شماره موبایل قبلا ثبت نام شده است", JsonRequestBehavior.DenyGet);
            return Json(true, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public JsonResult ClinicPhoneValidation(RegisterPack doctor)
        {
            PezeshkPlusEntities db = new PezeshkPlusEntities();

            List<int?> CheckClinicPhone = db.USP_SEL_ClinicPhone(doctor.RegisterModel.ClinicPhone).ToList();
            if (CheckClinicPhone.Count == 1)
                return Json("با این شماره تلفن مطب قبلا ثبت نام شده است", JsonRequestBehavior.DenyGet);
            return Json(true, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public JsonResult PasswordValidation(DoctorProfile doctor)
        {
            PezeshkPlusEntities db = new PezeshkPlusEntities();

            USP_SEL_Doctor_Result doctorLastInfo = (USP_SEL_Doctor_Result)Session["DoctorInfo"];

            List<int?> CheckPassword = db.USP_SEL_Password(doctorLastInfo.Email, doctor.Password).ToList();
            if (CheckPassword.Count == 0)
                return Json("پسورد وارد شده اشتباه است", JsonRequestBehavior.DenyGet);
            return Json(true, JsonRequestBehavior.DenyGet);
        }
        #endregion

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(RegisterPack doctor)
        {
            ViewBag.IsLogin = false;
            PezeshkPlusEntities db = new PezeshkPlusEntities();
            List<int?> CheckMail = db.USP_SEL_Email(doctor.LoginModel.Email).ToList();
            if (CheckMail.Count == 0)
            {
                TempData["WrongEmail"] = "ایمیل وارد شده صحیح نمی باشد!";
                return RedirectToAction("Register");
            }

            List<int?> CheckPass = db.USP_SEL_Password(doctor.LoginModel.Email, doctor.LoginModel.Password).ToList();
            if (CheckPass.Count == 0)
            {
                TempData["WrongPass"] = "کلمه عبور وارد شده صحیح نمی باشد!";
                return RedirectToAction("Register");
            }

            List<bool?> IsActive = db.USP_SEL_IsDoctorActive(doctor.LoginModel.Email).ToList();
            if (IsActive[0] == false)
            {
                TempData["NotActive"] = "ثبت نام شما هنوز توسط مدیریت تایید نشده!";
                return RedirectToAction("Register");
            }

            string authID = Guid.NewGuid().ToString();
            Session["AuthID"] = authID;

            Response.Cookies["AuthID"].Value = authID;

            Session[authID] = doctor.LoginModel.Email;
            Session[authID + "IP"] = Request.UserHostAddress;

            string previousUrl = Session["PreviousUrl"].ToString();
            Session.Remove("PreviousUrl");

            return Redirect(previousUrl);
        }

        [CustomFilter]
        [DoctorAuthorize]
        public ActionResult Panel()
        {
            string authID = Request.Cookies["AuthID"].Value;

            PezeshkPlusEntities db = new PezeshkPlusEntities();

            List<USP_SEL_Doctor_Result> doctorInfo = db.USP_SEL_Doctor(Session[authID].ToString()).ToList();
            List<USP_SEL_DoctorComments_Result> comments = db.USP_SEL_DoctorComments(Session[authID].ToString()).ToList();

            if (doctorInfo[0].Address == null)
                doctorInfo[0].Address = "ثبت نشده";
            if (doctorInfo[0].ClinicPhone == null)
                doctorInfo[0].ClinicPhone = "ثبت نشده";
            if (doctorInfo[0].WorkTime == null)
                doctorInfo[0].WorkTime = "ثبت نشده";

            ViewBag.DoctorInfo = doctorInfo;
            ViewBag.Comments = comments;

            Search model = new Search();

            return View(model);
        }

        [CustomFilter]
        [DoctorAuthorize]
        public ActionResult ProfileSet()
        {
            string authID = Request.Cookies["AuthID"].Value;

            PezeshkPlusEntities db = new PezeshkPlusEntities();
            List<USP_SEL_Doctor_Result> doctorInfo = db.USP_SEL_Doctor(Session[authID].ToString()).ToList();
            Session["DoctorInfo"] = doctorInfo[0];

            ViewBag.MedicalFields = db.USP_SEL_MedicalFields().ToList();

            DoctorProfile model = new DoctorProfile();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProfileSet(DoctorProfile doctorProfile)
        {
            try
            {
                USP_SEL_Doctor_Result doctorLastInfo = (USP_SEL_Doctor_Result)Session["DoctorInfo"];
                Session.Remove("DoctorInfo");

                PezeshkPlusEntities db = new PezeshkPlusEntities();

                if (doctorProfile.FirstName != doctorLastInfo.FirstName)
                    db.USP_UPD_FirstName(doctorLastInfo.Email, doctorProfile.FirstName);
                if (doctorProfile.LastName != doctorLastInfo.LastName)
                    db.USP_UPD_LastName(doctorLastInfo.Email, doctorProfile.LastName);

                if (doctorProfile.Password != null)
                {
                    if (doctorProfile.NewPassword != null)
                        db.USP_UPD_Password(doctorLastInfo.Email, doctorProfile.NewPassword);
                }

                if (doctorProfile.PhoneNumber != doctorLastInfo.PhoneNumber)
                {
                    List<int?> CheckPhoneNumber = db.USP_SEL_PhoneNumber(doctorProfile.PhoneNumber).ToList();
                    if (CheckPhoneNumber.Count == 1)
                    {
                        TempData["PhoneNumberError"] = "با این شماره موبایل قبلا ثبت نام شده است";
                        return RedirectToAction("ProfileSet");
                    }
                    db.USP_UPD_PhoneNumber(doctorLastInfo.Email, doctorProfile.PhoneNumber);
                }

                if (doctorProfile.ClinicPhone != doctorLastInfo.ClinicPhone)
                {
                    List<int?> CheckClinicPhone = db.USP_SEL_ClinicPhone(doctorProfile.ClinicPhone).ToList();
                    if (CheckClinicPhone.Count == 1)
                    {
                        TempData["ClinicPhoneError"] = "با این شماره تلفن مطب قبلا ثبت نام شده است";
                        return RedirectToAction("ProfileSet");
                    }
                    db.USP_UPD_ClinicPhone(doctorLastInfo.Email, doctorProfile.ClinicPhone);
                }

                if (doctorProfile.Province != doctorLastInfo.Province)
                    db.USP_UPD_Province(doctorLastInfo.Email, doctorProfile.Province);
                if (doctorProfile.City != doctorLastInfo.City)
                    db.USP_UPD_City(doctorLastInfo.Email, doctorProfile.City);
                if (doctorProfile.Address != doctorLastInfo.Address)
                    db.USP_UPD_Address(doctorLastInfo.Email, doctorProfile.Address);

                if (doctorProfile.NationalID != doctorLastInfo.NationalID)
                {
                    List<int?> CheckNationalID = db.USP_SEL_NationalID(doctorProfile.NationalID).ToList();
                    if (CheckNationalID.Count == 1)
                    {
                        TempData["NationalIDError"] = "با این کد ملی قبلا ثبت نام شده است";
                        return RedirectToAction("ProfileSet");
                    }
                    db.USP_UPD_NationalID(doctorLastInfo.Email, doctorProfile.NationalID);
                }

                if (doctorProfile.MedicalField != doctorLastInfo.MedicalField)
                {
                    List<int?> previousMedicalFieldID = db.USP_SEL_MedicalFieldID(doctorLastInfo.MedicalField).ToList();
                    List<int?> medicalFieldID = db.USP_SEL_MedicalFieldID(doctorProfile.MedicalField).ToList();
                    db.USP_UPD_MedicalField(doctorLastInfo.Email, previousMedicalFieldID[0], medicalFieldID[0]);
                }
                if (doctorProfile.WorkTime != doctorLastInfo.WorkTime)
                    db.USP_UPD_WorkTime(doctorLastInfo.Email, doctorProfile.WorkTime);
                if (doctorProfile.Picture != null)
                {
                    if (Path.GetExtension(doctorProfile.Picture.FileName) != ".jpg" && Path.GetExtension(doctorProfile.Picture.FileName) != ".JPG" && Path.GetExtension(doctorProfile.Picture.FileName) != ".png" && Path.GetExtension(doctorProfile.Picture.FileName) != ".PNG" && Path.GetExtension(doctorProfile.Picture.FileName) != ".jpeg")
                    {
                        TempData["PictureError"] = "پسوند فایل ارسالی باید jpg یا jpeg یا png باشد";
                        return RedirectToAction("ProfileSet");
                    }
                    if (doctorProfile.Picture.ContentLength > 1 & doctorProfile.Picture.ContentLength < (5 * 1024 * 1024))
                    {
                        if (doctorLastInfo.PicAddress != null)
                        {
                            if (System.IO.File.Exists(doctorLastInfo.PicAddress))
                                System.IO.File.Delete(doctorLastInfo.PicAddress);
                        }
                        string extention = Path.GetExtension(doctorProfile.Picture.FileName);
                        string guidNamePic = Guid.NewGuid().ToString();
                        string picAddress = $"/Images/Doctors/{guidNamePic}{extention}";
                        doctorProfile.Picture.SaveAs(Server.MapPath("~" + $"{picAddress}"));
                        db.USP_UPD_PicAddress(doctorLastInfo.Email, picAddress);
                    }
                    else
                    {
                        TempData["PictureError"] = "سایز فایل عکس ارسال شده باید حداکثر 5 Mg باشد";
                        return RedirectToAction("ProfileSet");
                    }
                }
            }
            catch (Exception)
            {
                TempData["UpdateError"] = "خطایی در بروزرسانی اطلاعات رخ داد! لطفا مقادیر را چک کنید و دوباره تلاش کنید";
                return RedirectToAction("ProfileSet");
            }

            TempData["UpdateSuccessFully"] = "پروفایل شما با موفقیت بروزرسانی شد";
            return RedirectToAction("ProfileSet");
        }

        [DoctorAuthorize]
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