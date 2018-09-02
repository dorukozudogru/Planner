using System;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Web.Mvc;

namespace Planner.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Menu()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult MessageShow(string messageBody, string returnUrl)
        {
            ViewBag.Message = messageBody;
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        public static bool SendEMail(string email, string email_body, string email_subject)
        {
            try
            {
                MailMessage msg = new MailMessage();
                //SmtpClient smtp = new SmtpClient("mail.citech.com.tr");
                SmtpClient smtp = new SmtpClient();

                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                //smtp.EnableSsl = false;
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = null;
                //smtp.Credentials = new System.Net.NetworkCredential("projesepeti@citech.com.tr", "qwerty11");
                smtp.Credentials = new System.Net.NetworkCredential("dorukozudogru.oglog@gmail.com", "doruk.,.,23");

                //msg.From = new MailAddress("projesepeti@citech.com.tr", "Proje Sepeti");
                msg.From = new MailAddress("dorukozudogru.oplog@gmail.com", "Proje Sepeti");
                msg.To.Add(email);
                msg.Subject = email_subject;
                msg.Body = email_body;
                msg.IsBodyHtml = true;
                smtp.Send(msg);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}