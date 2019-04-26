using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PezeshkPlus.Models;
using PezeshkPlus.Models.CustomModel;
using PezeshkPlus.Authoriza;
using PezeshkPlus.Authoriza.CustomFilter;
using MimeKit;
using MimeKit.Utils;
using MailKit.Net.Smtp;

namespace PezeshkPlus.Controllers
{
    [CustomFilter]
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            PezeshkPlusEntities db = new PezeshkPlusEntities();
            List<USP_SEL_Top6MedicalFields_Result> top6MedicalFields = db.USP_SEL_Top6MedicalFields().ToList();
            ViewBag.Top6MedicalFields = top6MedicalFields;
            List<USP_SEL_Top3Articles_Result> top3News = db.USP_SEL_Top3Articles().ToList();
            ViewBag.Top3News = top3News;

            Search model = new Search();

            return View(model);
        }

        public ActionResult SiteMap()
        {
            Search model = new Search();

            return View(model);
        }

        public ActionResult ContactUs()
        {
            EmailContact model = new EmailContact();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ContactUs(EmailContact emailModel)
        {
            if (emailModel.Name == null)
                emailModel.Name = "No Name";
            if (emailModel.Topic == null)
                emailModel.Topic = "From PezeshkPlus";
            try
            {
                MimeMessage message = new MimeMessage();

                message.From.Add(new MailboxAddress(emailModel.Name, emailModel.Email));
                message.To.Add(new MailboxAddress("Peyvand Azadi", "peyvandazadi1996@gmail.com"));
                message.Subject = emailModel.Topic;

                message.Body = new TextPart("plain")
                {
                    Text = emailModel.Text
                };

                using (SmtpClient client = new SmtpClient())
                {
                    // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    client.Connect("smtp.sendgrid.net", 587, false);

                    // Note: only needed if the SMTP server requires authentication
                    client.Authenticate("apikey", "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");

                    client.Send(message);
                    client.Disconnect(true);
                }
                TempData["EmailSent"] = "پیغام شما با موفقیت ارسال شد!";
            }
            catch (Exception)
            {
                TempData["EmailNotSent"] = "خطا در برقراری ارتباط!";
            }

            return RedirectToAction("ContactUs");
        }
        public ActionResult AboutUs()
        {
            Search model = new Search();

            return View(model);
        }
    }
}