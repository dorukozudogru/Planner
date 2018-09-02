using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Planner.Enums;
using Planner.Helpers;
using Planner.Models;

namespace Planner.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private DBContext db = new DBContext();

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = System.Web.HttpContext.Current.Request.RawUrl.ToString();
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User model)
        {
            string encPass = EncryptionHelper.Encrypt(model.Password);
            var loginUser = db.User.FirstOrDefault(a => a.EMail == model.EMail);
            if (loginUser != null)
            {
                if (loginUser.IsActive)
                {
                    if (loginUser.IsEmailVerified)
                    {
                        if (loginUser.EMail != model.EMail || loginUser.Password != encPass)
                        {
                            ViewBag.Message = "Kullanıcı Adınızı veya Şifrenizi Hatalı Girdiniz";
                            return View();
                        }
                        else
                        {
                            var loginUserCV = db.UserCV.FirstOrDefault(a => a.UserId == loginUser.Id);

                            if (loginUser.IsApproved == Convert.ToInt32(UserApproveEnum.WaitingToApprove))
                            {
                                ViewBag.Message = "Hesabınız Henüz Yöneticiler Tarafından Onaylanmamıştır";
                                return View();
                            }
                            else if (loginUser.IsApproved == Convert.ToInt32(UserApproveEnum.Blocked) || loginUser.IsApproved == Convert.ToInt32(UserApproveEnum.NotApproved))
                            {
                                ViewBag.Message = "Hesabınız Yöneticiler Tarafından Onaylanmamıştır";
                                return View();
                            }
                            else if (loginUser.IsApproved == Convert.ToInt32(UserApproveEnum.Approved) || loginUser.IsApproved == Convert.ToInt32(UserApproveEnum.ApproveAfterBlock))
                            {
                                if (loginUser.IsAdmin != true)
                                {
                                    Session["IsLoggedIn"] = true;
                                    Session["UserId"] = loginUser.Id.ToString();
                                    Session["UserEMail"] = loginUser.EMail.ToString();
                                    Session["UserPassword"] = loginUser.Password.ToString();
                                    Session["UserName"] = loginUser.Name.ToString();
                                    Session["UserSurname"] = loginUser.Surname.ToString();
                                    Session["UserCitizenshipNo"] = loginUser.CitizenshipNo.ToString();
                                    Session["UserIsCvUploaded"] = Convert.ToBoolean(loginUser.IsCvUploaded);
                                    if (loginUserCV != null)
                                    {
                                        Session["UserCvId"] = loginUserCV.Id;
                                    }
                                    Session["UserIsApproved"] = Convert.ToInt32(loginUser.IsApproved);
                                    Session["UserIsAdmin"] = Convert.ToInt32(loginUser.IsAdmin);
                                    return RedirectToAction("UserMenu", "User");
                                }
                                else if (loginUser.IsAdmin == true)
                                {
                                    Session["IsLoggedIn"] = true;
                                    Session["UserId"] = loginUser.Id.ToString();
                                    Session["UserEMail"] = loginUser.EMail.ToString();
                                    Session["UserPassword"] = loginUser.Password.ToString();
                                    Session["UserName"] = loginUser.Name.ToString();
                                    Session["UserSurname"] = loginUser.Surname.ToString();
                                    Session["UserCitizenshipNo"] = loginUser.CitizenshipNo.ToString();
                                    Session["UserIsApproved"] = Convert.ToInt32(loginUser.IsApproved);
                                    Session["UserIsAdmin"] = Convert.ToInt32(loginUser.IsAdmin);
                                    return RedirectToAction("AdminUserMenu", "User");
                                }
                            }
                        }
                    }
                    else
                    {
                        ViewBag.Message = "E-Posta Doğrulamasını Yapınız";
                        ViewBag.Message2 = "Doğrulama Postasını Almak İçin Tıklayınız";
                        ViewBag.Link = Url.Action("VerifyEmail", "Account", new { userId = loginUser.Id }, protocol: Request.Url.Scheme);
                        HomeController.SendEMail(model.EMail, "E-postanızı yandaki linke tıklayarak onaylayabilirsiniz. " + ViewBag.Link, "Üyelik Onayı");
                        return View();
                    }
                }
                else
                {
                    ViewBag.Message = "Hesabınız Pasif Durumdadır";
                    return View();
                }
            }
            else
            {
                ViewBag.Message = "Kullanıcı Adınızı veya Şifrenizi Hatalı Girdiniz";
                //Aslında Giriş Yapan Kişi, Giriş Yapmaya Çalıştığı E-Postasıyla Sisteme Kayıt Olmamış. Ama Bunu Yazmak Sistemde Açık Oluşturabilir.
                return View();
            }
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult Logout()
        {
            Session.RemoveAll();
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User model)
        {
            try
            {
                //KPS Entegrasyonu
                //int birthYear = user.BirthDate.Year;
                //KPS_Servis.KPSPublicSoapClient ws = new KPS_Servis.KPSPublicSoapClient();
                //bool? confirmCitizenshipNo = ws.TCKimlikNoDogrula(Convert.ToInt64(user.CitizenshipNo), (user.Name).ToUpper(), (user.Surname).ToUpper(), birthYear);
                //if (confirmCitizenshipNo)
                //{
                //    user.Password = HomeController.Encrypt(user.Password);
                //    user.IsApproved = Convert.ToInt32(UserApproveEnum.WaitingToApprove);
                //    user.IsAdmin = false;
                //    user.RegisterDate = DateTime.Now;
                //    db.User.Add(user);
                //    db.SaveChanges();
                //    return RedirectToAction("Approvement");
                //}

                //Yalnızca Kimlik No Kontrolü
                User user = db.User.FirstOrDefault(x => x.EMail == model.EMail);
                if (user == null)
                {
                    bool controlCitizenshipno = CitizenshipNoHelper.ControlCitizenshipNo(model.CitizenshipNo);
                    if (controlCitizenshipno)
                    {
                        var guid = Guid.NewGuid().ToString();
                        User newUser = new User()
                        {
                            Id = guid,
                            EMail = model.EMail,
                            Name = model.Name,
                            Surname = model.Surname,
                            Password = EncryptionHelper.Encrypt(model.Password),
                            BirthDate = model.BirthDate,
                            PhoneNumber = model.PhoneNumber,
                            CitizenshipNo = model.CitizenshipNo,
                            City = String.IsNullOrEmpty(model.City) ? "" : model.City,
                            School = String.IsNullOrEmpty(model.School) ? "" : model.School,
                            Department = String.IsNullOrEmpty(model.Department) ? "" : model.Department,
                            Job = String.IsNullOrEmpty(model.Job) ? "" : model.Job,
                            IsFirm = model.IsFirm,
                            IsCvUploaded = false,
                            IsEmailVerified = false,
                            IsApproved = Convert.ToInt32(UserApproveEnum.WaitingToApprove),
                            IsAdmin = false,
                            IsActive = true,
                            RegisterDate = DateTime.Now,
                            LastEditDate = DateTime.Now.AddYears(-10),
                            LastEditBy = "00000000-0000-0000-0000-000000000000"
                        };

                        var callbackUrl = Url.Action("VerifyEmail", "Account", new { userId = guid }, protocol: Request.Url.Scheme);

                        bool emailSend = HomeController.SendEMail(model.EMail, "E-postanızı yandaki linke tıklayarak onaylayabilirsiniz. " + callbackUrl, "Üyelik Onayı");

                        if (!emailSend)
                        {
                            ViewBag.Message = "E-posta doğrulamada bir sorun oluştu. Lütfen aşağıdaki linke tıklayarak tekrar deneyin.";
                            ViewBag.Link = callbackUrl;
                        }

                        if (newUser.IsFirm)
                        {
                            db.User.Add(newUser);
                            db.SaveChanges();
                            ViewBag.Message = "Kayıt işleminiz başarıyla gerçekleştirilerek sistem yöneticilerine onaya gönderilmiştir.";
                            return PartialView("_Approvement");
                        }

                        UserController.SetInfos(model);
                        return PartialView("_UploadCv");
                    }
                    else
                    {
                        ViewBag.Message = "T.C. Kimlik Numaranızı Hatalı Girdiniz";
                        return PartialView("_InvalidCitizenshipNo");
                    }
                }
                else
                {
                    return PartialView("_AlreadyRegistered");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Kayıt Sırasında Bir Hata Oluştu. Lütfen Tüm Bilgilerinizi Girdiğinizden Emin Olun.";
                return PartialView("_InvalidCitizenshipNo");
            }
        }

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(string email)
        {
            var user = db.User.FirstOrDefault(a => a.EMail == email);
            if (user == null)
            {
                ViewBag.Message = "E-Postanızı Hatalı Girdiniz";
                return View();
            }

            byte[] resetCode = new byte[32];
            var rngCrypto = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rngCrypto.GetBytes(resetCode);
            string code = Convert.ToBase64String(resetCode); //Aslında kullanılmıyor ama link dolu görünsün :)

            var callbackUrl = Url.Action("ResetPassword", "Account", new { code, userId = user.Id }, protocol: Request.Url.Scheme);
            HomeController.SendEMail(user.EMail, "Şifrenizi yandaki linke tıklayarak değiştirebilirsiniz. " + callbackUrl, "Şifre Sıfırlama");
            user.Password = EncryptionHelper.Encrypt(Guid.NewGuid().ToString()); // Şifreyi rastgele bir şey yaptı.
            db.SaveChanges();
            ViewBag.Message = "Şifre Sıfırlama Kodu Gönderilmiştir";
            return View();
        }

        [AllowAnonymous]
        public ActionResult ResetPassword(string userId)
        {
            return userId == null ? View("Error") : View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(string email, string password)
        {
            var user = db.User.FirstOrDefault(a => a.EMail == email);
            if (user == null)
            {
                ViewBag.Message = "E-Postanızı Hatalı Girdiniz";
                return View();
            }
            user.Password = EncryptionHelper.Encrypt(password);
            db.SaveChanges();
            ViewBag.Message = "Şifreniz Değiştirilmiştir";
            return View();
        }

        [AllowAnonymous]
        public ActionResult VerifyEmail(string userId)
        {
            var user = db.User.FirstOrDefault(a => a.Id == userId);
            user.IsEmailVerified = true;
            db.SaveChanges();
            return View();
        }
    }
}