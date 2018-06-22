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

        public static string Encrypt(string _password)
        {
            MD5CryptoServiceProvider _sp = new MD5CryptoServiceProvider();
            byte[] _byteValue = System.Text.Encoding.UTF8.GetBytes(_password);
            byte[] _encodedbyte = _sp.ComputeHash(_byteValue);
            return Convert.ToBase64String(_encodedbyte);
        }

        public static void SendEMail(string email, string email_body, string email_subject)
        {
            MailMessage msg = new MailMessage();
            SmtpClient smtp = new SmtpClient("mail.citech.com.tr");
            //SmtpClient smtp = new SmtpClient("smtp.gmail.com");

            //smtp.Port = 587;
            smtp.EnableSsl = false;
            //smtp.Host = "smtp.gmail.com";
            //smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = null;
            smtp.Credentials = new System.Net.NetworkCredential("projesepeti@citech.com.tr", "qwerty11");
            //smtp.Credentials = new System.Net.NetworkCredential("dorukozudogru.oglog@gmail.com", "doruk.,.,23");

            msg.From = new MailAddress("projesepeti@citech.com.tr", "Proje Sepeti");
            //msg.From = new MailAddress("dorukozudogru@gmail.com", "Proje Sepeti");
            msg.To.Add(email);
            msg.Subject = email_subject;
            msg.Body = email_body;
            smtp.Send(msg);
        }

        public static bool ControlCitizenshipNo(string citizenshipNo)
        {
            bool returnvalue;
            try
            {
                if (citizenshipNo.Length == 11)
                {
                    Int64 atcno, btcno, tcno;
                    long c1, c2, c3, c4, c5, c6, c7, c8, c9, q1, q2;
                    tcno = Int64.Parse(citizenshipNo);
                    atcno = tcno / 100;
                    btcno = tcno / 100;
                    c1 = atcno % 10;
                    atcno = atcno / 10;
                    c2 = atcno % 10;
                    atcno = atcno / 10;
                    c3 = atcno % 10;
                    atcno = atcno / 10;
                    c4 = atcno % 10;
                    atcno = atcno / 10;
                    c5 = atcno % 10;
                    atcno = atcno / 10;
                    c6 = atcno % 10;
                    atcno = atcno / 10;
                    c7 = atcno % 10;
                    atcno = atcno / 10;
                    c8 = atcno % 10;
                    atcno = atcno / 10;
                    c9 = atcno % 10;
                    atcno = atcno / 10;
                    q1 = ((10 - ((((c1 + c3 + c5 + c7 + c9) * 3) + (c2 + c4 + c6 + c8)) % 10)) % 10);
                    q2 = ((10 - (((((c2 + c4 + c6 + c8) + q1) * 3) + (c1 + c3 + c5 + c7 + c9)) % 10)) % 10);
                    return returnvalue = ((btcno * 100) + (q1 * 10) + q2 == tcno);
                }
            }
            catch (Exception ex)
            {

            }
            return false;
        }
    }
}