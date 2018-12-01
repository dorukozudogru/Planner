using Planner.Enums;
using Planner.Helpers;
using Planner.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;

namespace Planner.Controllers
{
    [UserAuthorize]
    public class UserController : Controller
    {
        private DBContext db = new DBContext();
        static string callbackUrl = "";
        static string userEmail = "";

        public ActionResult UserIndex()
        {
            if (Convert.ToBoolean(Session["UserIsAdmin"]) != false)
            {
                return View(db.User.ToList());
            }
            return RedirectToAction("UserMenu");
        }

        public ActionResult UserCVIndex()
        {
            try
            {
                User userModel = new User();
                List<User> lstUser = new List<User>();

                UserCV ucModel = new UserCV();
                List<vwUsersCVs> vwLstUc = new List<vwUsersCVs>();

                foreach (var item in db.UserCV)
                {
                    int ucId = item.Id;
                    ucModel = db.UserCV.First(z => z.Id == ucId);
                    userModel = db.User.First(z => z.Id == ucModel.UserId);
                    if (true)
                    {
                        vwLstUc.Add(new vwUsersCVs { UserId = userModel.Id, EMail = userModel.EMail, Name = userModel.Name, Surname = userModel.Surname, CitizenshipNo = userModel.CitizenshipNo, UserCVId = ucModel.Id, FileName = ucModel.FileName, FilePath = ucModel.FilePath });
                    }
                }
                return View(vwLstUc);
            }
            catch (Exception ex)
            {
                List<vwUsersCVs> vwLstUc = new List<vwUsersCVs>();
                return View(vwLstUc);
            }
        }

        public ActionResult UserCV(string loggedUserId)
        {
            try
            {
                User userModel = new User();
                List<User> lstUser = new List<User>();

                UserCV ucModel = new UserCV();
                List<vwUsersCVs> vwLstUc = new List<vwUsersCVs>();

                ucModel = db.UserCV.FirstOrDefault(z => z.UserId == loggedUserId);
                userModel = db.User.FirstOrDefault(z => z.Id == loggedUserId);

                vwLstUc.Add(new vwUsersCVs { UserId = userModel.Id, EMail = userModel.EMail, Name = userModel.Name, Surname = userModel.Surname, CitizenshipNo = userModel.CitizenshipNo, UserCVId = ucModel.Id, FileName = ucModel.FileName, FilePath = ucModel.FilePath });
                return View(vwLstUc);
            }
            catch (Exception)
            {
                List<vwUsersCVs> vwLstUp = new List<vwUsersCVs>();
                return View(vwLstUp);
            }
        }

        public ActionResult ChangeUserToAdmin(string Id)
        {
            User userModel = db.User.FirstOrDefault(a => a.Id == Id);
            userModel.IsAdmin = true;
            db.SaveChanges();
            return RedirectToAction("MessageShow", "Home", new { messageBody = "Kullanıcı Admin Olarak Ayarlanmıştır.", returnUrl = Request.UrlReferrer.AbsoluteUri });
        }

        #region Views
        public ActionResult UserMenu(string loggedUserId)
        {
            if (Convert.ToString(Session["UserId"]) != "")
            {
                if (Convert.ToBoolean(Session["UserIsAdmin"]))
                {
                    return RedirectToAction("AdminUserMenu", "User");
                }
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public ActionResult AdminUserMenu()
        {
            if (!Convert.ToBoolean(Session["UserIsAdmin"]))
            {
                return RedirectToAction("UserMenu", "User");
            }
            else
            {
                try
                {
                    User userModel = new User();
                    List<User> lstUser = new List<User>();

                    Project pModel = new Project();
                    List<Project> lstProject = new List<Project>();

                    UserProject upModel = new UserProject();
                    List<vwUsersProjects> vwLstUp = new List<vwUsersProjects>();

                    bool userAuthorized = false;

                    foreach (var item in db.UserProject)
                    {
                        string _tempUserId = Session["UserId"].ToString();
                        ProjectUserAuthorize authorize = db.ProjectUserAuthorize.FirstOrDefault(a => a.ProjectId == item.ProjectId);
                        if (authorize == null) //Authorize proje için null gelmişse herkes görebilmeli
                        {
                            int upId = item.Id;
                            upModel = db.UserProject.First(z => z.Id == upId);
                            pModel = db.Projects.First(z => z.Id == upModel.ProjectId);
                            userModel = db.User.First(z => z.Id == upModel.UserId);
                            vwLstUp.Add(new vwUsersProjects
                            {
                                UserId = userModel.Id,
                                UserName = userModel.Name,
                                UserSurname = userModel.Surname,
                                UserEMail = userModel.EMail,
                                UserCitizenshipNo = userModel.CitizenshipNo,
                                ProjectId = pModel.Id,
                                ProjectName = pModel.Name,
                                FileName = pModel.FileName,
                                IsApproved = pModel.IsApproved,
                                IsSupported = pModel.IsSupported,
                                ProjectDescription = pModel.Description,
                                ProjectCreationDate = pModel.CreationDate
                            });
                        }
                        else //Authorize proje için null değilse kimlerin görebildiğine bakılmalı
                        {
                            authorize = db.ProjectUserAuthorize.FirstOrDefault(a => a.AuthorizedUserId == _tempUserId && a.ProjectId == item.ProjectId);
                            if (authorize == null)
                                userAuthorized = false;
                            else if (authorize != null)
                                userAuthorized = true;

                            if (userAuthorized)
                            {
                                int upId = item.Id;
                                upModel = db.UserProject.First(z => z.Id == upId);
                                pModel = db.Projects.First(z => z.Id == upModel.ProjectId);
                                userModel = db.User.First(z => z.Id == upModel.UserId);
                                vwLstUp.Add(new vwUsersProjects
                                {
                                    UserId = userModel.Id,
                                    UserName = userModel.Name,
                                    UserSurname = userModel.Surname,
                                    UserEMail = userModel.EMail,
                                    UserCitizenshipNo = userModel.CitizenshipNo,
                                    ProjectId = pModel.Id,
                                    ProjectName = pModel.Name,
                                    FileName = pModel.FileName,
                                    IsApproved = pModel.IsApproved,
                                    IsSupported = pModel.IsSupported,
                                    ProjectDescription = pModel.Description,
                                    ProjectCreationDate = pModel.CreationDate
                                });
                            }
                        }
                    }
                    vwLstUp = vwLstUp.OrderByDescending(x => x.ProjectCreationDate).ToList();
                    vwLstUp.RemoveRange(5, vwLstUp.Count - 5);
                    return View(vwLstUp);
                }
                catch (Exception ex)
                {
                    List<vwUsersProjects> vwLstUp = new List<vwUsersProjects>();
                    return View(vwLstUp);
                }
            }
        }

        public ActionResult UploadUserCV()
        {
            return View();
        }
        #endregion

        #region Approvement
        public ActionResult ApproveUser(string id)
        {
            if (id == null || id == "")
            {
                return RedirectToAction("MessageShow", "Home", new { messageBody = "Kullanıcı Id'si Bulunamadı", returnUrl = Request.UrlReferrer.AbsoluteUri });
            }

            var user = db.User.SingleOrDefault(m => m.Id == id);
            if (user == null)
            {
                return RedirectToAction("MessageShow", "Home", new { messageBody = "Kullanıcı Bulunamadı", returnUrl = Request.UrlReferrer.AbsoluteUri });
            }

            if (user != null)
            {
                user.IsApproved = Convert.ToInt32(UserApproveEnum.Approved);
                user.LastEditDate = DateTime.Now;
                user.LastEditBy = Convert.ToString(Session["UserId"]);
                db.SaveChanges();
                try
                {
                    HomeController.SendEMail(user.EMail, "Üyeliğiniz Onaylanmıştır.", "Üyelik Onayı");
                    return RedirectToAction("MessageShow", "Home", new { messageBody = "Kullanıcı Onaylanmıştır", returnUrl = Request.UrlReferrer.AbsoluteUri });
                }
                catch (Exception ex)
                {
                    return RedirectToAction("MessageShow", "Home", new { messageBody = "Kullanıcı Onaylanmıştır Fakat E-Posta Gönderiminde Bir Hata Oluşmuştur", returnUrl = Request.UrlReferrer.AbsoluteUri });
                }
            }
            return View();
        }

        public ActionResult DeclineUser(string id)
        {
            if (id == null || id == "")
            {
                return RedirectToAction("MessageShow", "Home", new { messageBody = "Kullanıcı Id'si Bulunamadı", returnUrl = Request.UrlReferrer.AbsoluteUri });
            }

            var user = db.User.SingleOrDefault(m => m.Id == id);
            if (user == null)
            {
                return RedirectToAction("MessageShow", "Home", new { messageBody = "Kullanıcı Bulunamadı", returnUrl = Request.UrlReferrer.AbsoluteUri });
            }
            if (user != null)
            {
                user.IsApproved = Convert.ToInt32(UserApproveEnum.NotApproved);
                user.LastEditDate = DateTime.Now;
                user.LastEditBy = Convert.ToString(Session["UserId"]);
                db.SaveChanges();
                try
                {
                    HomeController.SendEMail(user.EMail, "Üyeliğiniz Reddedilmiştir.", "Üyelik Onayı");
                    return RedirectToAction("MessageShow", "Home", new { messageBody = "Kullanıcı Hesabı Reddedilmiştir", returnUrl = Request.UrlReferrer.AbsoluteUri });
                }
                catch (Exception ex)
                {
                    return RedirectToAction("MessageShow", "Home", new { messageBody = "Kullanıcı Hesabı Reddedilmiştir Fakat E-Posta Gönderiminde Bir Hata Oluşmuştur", returnUrl = Request.UrlReferrer.AbsoluteUri });
                }
            }
            return View();
        }

        public ActionResult BlockUser(string id)
        {
            if (id == null || id == "")
            {
                return RedirectToAction("MessageShow", "Home", new { messageBody = "Kullanıcı Id'si Bulunamadı", returnUrl = Request.UrlReferrer.AbsoluteUri });
            }

            var user = db.User.SingleOrDefault(m => m.Id == id);
            if (user == null)
            {
                return RedirectToAction("MessageShow", "Home", new { messageBody = "Kullanıcı Bulunamadı", returnUrl = Request.UrlReferrer.AbsoluteUri });
            }

            if (user != null)
            {
                user.IsApproved = Convert.ToInt32(UserApproveEnum.Blocked);
                user.LastEditDate = DateTime.Now;
                user.LastEditBy = Convert.ToString(Session["UserId"]);
                db.SaveChanges();
                try
                {
                    HomeController.SendEMail(user.EMail, "Üyeliğiniz Dondurulmuştur.", "Üyelik Onayı");
                    return RedirectToAction("MessageShow", "Home", new { messageBody = "Kullanıcı Hesabı Engellenmiştir", returnUrl = Request.UrlReferrer.AbsoluteUri });
                }
                catch (Exception ex)
                {
                    return RedirectToAction("MessageShow", "Home", new { messageBody = "Kullanıcı Hesabı Engellenmiştir Fakat E-Posta Gönderiminde Bir Hata Oluşmuştur", returnUrl = Request.UrlReferrer.AbsoluteUri });
                }
            }
            return View();
        }

        public ActionResult ApproveAfterBlockUser(string id)
        {
            if (id == null || id == "")
            {
                return RedirectToAction("MessageShow", "Home", new { messageBody = "Kullanıcı Id'si Bulunamadı", returnUrl = Request.UrlReferrer.AbsoluteUri });
            }

            var user = db.User.SingleOrDefault(m => m.Id == id);
            if (user == null)
            {
                return RedirectToAction("MessageShow", "Home", new { messageBody = "Kullanıcı Bulunamadı", returnUrl = Request.UrlReferrer.AbsoluteUri });
            }

            if (user != null)
            {
                user.IsApproved = Convert.ToInt32(UserApproveEnum.ApproveAfterBlock);
                user.LastEditDate = DateTime.Now;
                user.LastEditBy = Convert.ToString(Session["UserId"]);
                db.SaveChanges();
                try
                {
                    HomeController.SendEMail(user.EMail, "Üyeliğiniz Yeniden Aktif Edilmiştir.", "Üyelik Onayı");
                    return RedirectToAction("MessageShow", "Home", new { messageBody = "Kullanıcı Hesabı Yeniden Aktif Edilmiştir", returnUrl = Request.UrlReferrer.AbsoluteUri });
                }
                catch (Exception ex)
                {
                    return RedirectToAction("MessageShow", "Home", new { messageBody = "Kullanıcı Hesabı Yeniden Aktif Edilmiştir Fakat E-Posta Gönderiminde Bir Hata Oluşmuştur", returnUrl = Request.UrlReferrer.AbsoluteUri });
                }
            }
            return View();
        }
        #endregion

        #region ListUsers (Yapılmadı - Kullanıcıları Admin Ekranında Gruplama)
        public ActionResult ApprovedUsers()
        {
            if (Convert.ToBoolean(Session["UserIsAdmin"]) != false)
            {
                try
                {
                    User userModel = new User();
                    List<User> lstUser = new List<User>();

                    foreach (var item in db.User)
                    {
                        if (item.IsApproved == Convert.ToInt32(UserApproveEnum.Approved))
                        {
                            lstUser.Add(new User
                            {
                                BirthDate = item.BirthDate,
                                CitizenshipNo = item.CitizenshipNo,
                                EMail = item.EMail,
                                Id = item.Id,
                                IsAdmin = item.IsAdmin,
                                IsApproved = item.IsApproved,
                                IsCvUploaded = item.IsCvUploaded,
                                LastEditBy = item.LastEditBy,
                                LastEditDate = item.LastEditDate,
                                Name = item.Name,
                                Password = item.Password,
                                RegisterDate = item.RegisterDate,
                                Surname = item.Surname
                            });
                        }
                    }
                    lstUser.ToList();
                    return View(lstUser);
                }
                catch (Exception ex)
                {
                    List<User> lstUser = new List<User>();
                    lstUser.ToList();
                    return View(lstUser);
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        #endregion

        #region Edit User (Yapılmadı)
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    User user = db.User.Find(id);
        //    if (user == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(user);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,EMail,Name,Surname,Password,BirthDate,CitizenshipNo,IsApproved")] User user)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(user).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(user);
        //}
        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult UserProjectIndex(string loggedUserId)
        {
            try
            {
                if (loggedUserId == Convert.ToString(Session["UserId"]))
                {
                    User userModel = db.User.First(a => a.Id == loggedUserId);
                    Project pModel = new Project();
                    List<Project> lstUp = new List<Project>();
                    foreach (var item in db.UserProject)
                    {
                        if (userModel.Id == item.UserId && item.IsApproveChanged != Convert.ToInt32(ProjectTypeChangeEnum.ChangeAsNotApproved))
                        {
                            string pId = item.ProjectId;
                            pModel = db.Projects.First(z => z.Id == pId);
                            lstUp.Add(new Project { Id = pModel.Id, Name = pModel.Name, Description = pModel.Description, FileName = pModel.FileName, IsApproved = pModel.IsApproved, IsSupported = pModel.IsSupported });
                        }
                    }
                    return View(lstUp);
                }
                else
                {
                    RedirectToAction("UserMenu");
                }
                return View();
            }
            catch (Exception ex)
            {
                List<Project> lstNull = new List<Project>();
                return View(lstNull);
            }
        }

        public ActionResult UserRejectedProjectIndex(string loggedUserId)
        {
            try
            {
                if (loggedUserId == Convert.ToString(Session["UserId"]))
                {
                    User userModel = db.User.First(a => a.Id == loggedUserId);
                    Project pModel = new Project();
                    RejectedProjects rProjects = new RejectedProjects();
                    List<vwRejectedProjects> lstUp = new List<vwRejectedProjects>();
                    foreach (var item in db.UserProject)
                    {
                        if (userModel.Id == item.UserId && item.IsApproveChanged == Convert.ToInt32(ProjectTypeChangeEnum.ChangeAsNotApproved))
                        {
                            string pId = item.ProjectId;
                            pModel = db.Projects.First(z => z.Id == pId);
                            rProjects = db.RejectedProjects.FirstOrDefault(z => z.RejectedProjectId == pModel.Id);
                            lstUp.Add(new vwRejectedProjects { ProjectId = pModel.Id, ProjectName = pModel.Name, ProjectDescription = pModel.Description, RejectCause = rProjects.RejectCause, RejectDate = rProjects.RejectDate.Date });
                        }
                    }
                    return View(lstUp);
                }
                else
                {
                    RedirectToAction("UserMenu");
                }
                return View();
            }
            catch (Exception ex)
            {
                List<Project> lstNull = new List<Project>();
                return View(lstNull);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult UploadUserCV(HttpPostedFileBase fileCV)
        {
            try
            {
                if (fileCV.ContentType != "application/pdf")
                {
                    ViewBag.Message = "Lütfen PDF Dosyası Yükleyin";
                    return View();
                }
                else if (fileCV.ContentLength > 0 && fileCV.ContentType == "application/pdf")
                {
                    string citizenshipNo = Convert.ToString(Session["UserCitizenshipNo"]);

                    var fileName = Path.GetFileName(fileCV.FileName);
                    var path = Path.Combine(Server.MapPath("~/Files/CV"), (citizenshipNo + "_CV_" + fileName));
                    fileCV.SaveAs(path);

                    UserCV cv = new UserCV();

                    var userControl = db.User.SingleOrDefault(m => m.CitizenshipNo == citizenshipNo);

                    if (userControl.IsCvUploaded != false)
                    {
                        cv = db.UserCV.SingleOrDefault(m => m.UserId == userControl.Id);
                        cv.FileName = fileName;
                        cv.FilePath = path;
                        cv.CreationDate = DateTime.Now;
                        db.SaveChanges();
                        return RedirectToAction("MessageShow", "Home", new { messageBody = "CV Başarıyla Güncellendi", returnUrl = Request.UrlReferrer.AbsoluteUri });
                    }
                    else
                    {
                        cv.UserId = userControl.Id;
                        cv.FileName = fileName;
                        cv.FilePath = path;
                        cv.CreationDate = DateTime.Now;
                        db.UserCV.Add(cv);

                        var user = db.User.SingleOrDefault(m => m.Id == cv.UserId);
                        user.IsCvUploaded = true;
                        db.SaveChanges();
                        ViewBag.Message = "CV Başarıyla Yüklendi";
                        Session["UserIsCvUploaded"] = true;
                        userEmail = user.EMail;

                        callbackUrl = Url.Action("VerifyEmail", "Account", new { userId = user.Id }, protocol: Request.Url.Scheme);
                        try
                        {
                            HomeController.SendEMail(user.EMail, "E-postanızı yandaki linke tıklayarak onaylayabilirsiniz. " + callbackUrl, "Üyelik Onayı");
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                if (ex.ToString().Contains("SMTP"))
                {
                    ViewBag.Message = "Kullanıcınız Başarıyla Oluşturulmuştur. Fakat Onay E-Postası Gönderimi Sırasında Bir Hata Oluştu.";
                    ViewBag.ReturnUrl = "/User/ResendEmail";
                }
                else
                {
                    ViewBag.Message = "Kullanıcınız Başarıyla Oluşturulmuştur. Fakat CV Yükleme Sırasında Bir Hata Oluştu. Lütfen Yönetici Onayından Sonra CV'nizi Yükleyiniz.";
                    ViewBag.ReturnUrl = Request.UrlReferrer.AbsoluteUri;
                }
                return PartialView("_UploadUserCV");
            }
        }

        public ActionResult Download(int? Id)
        {
            try
            {
                UserCV ucmodel = db.UserCV.FirstOrDefault(m => m.Id == Id);
                byte[] fileBytes = System.IO.File.ReadAllBytes(ucmodel.FilePath);
                var file = ucmodel.FilePath;
                var cd = new ContentDisposition
                {
                    Inline = true,
                    FileName = ucmodel.FileName
                };
                Response.AddHeader("Content-Disposition", cd.ToString());
                //return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, pmodel.FileName); //For Download
                return File(fileBytes, ".pdf"); // For New Tab
            }
            catch (Exception)
            {
                return RedirectToAction("MessageShow", "Home", new { messageBody = "Dosya Bulunamadı", returnUrl = Request.UrlReferrer.AbsoluteUri });
            }
        }

        public ActionResult UserProfile(string loggedUserId)
        {
            try
            {
                User userModel = db.User.FirstOrDefault(z => z.Id == loggedUserId);
                return View(userModel);
            }
            catch (Exception ex)
            {
                return RedirectToAction("MessageShow", "Home", new { messageBody = "Beklenmeyen Bir Hata Oluştu", returnUrl = Request.UrlReferrer.AbsoluteUri });
            }
        }

        [HttpPost]
        public ActionResult EditProfile(User user)
        {
            if (HomeController.ControlCitizenshipNo(user.CitizenshipNo))
            {
                User model = db.User.FirstOrDefault(a => a.EMail == user.EMail);
                model.Name = user.Name;
                model.Surname = user.Surname;
                model.EMail = user.EMail;
                model.Password = HomeController.Encrypt(user.Password);
                model.CitizenshipNo = user.CitizenshipNo;
                model.BirthDate = user.BirthDate;
                model.City = user.City;
                model.PhoneNumber = user.PhoneNumber;
                model.School = user.School;
                model.Department = user.Department;
                model.Job = user.Job;
                model.LastEditBy = user.Id;
                model.LastEditDate = DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("MessageShow", "Home", new { messageBody = "Bilgileriniz Güncellenmiştir", returnUrl = Request.UrlReferrer.AbsoluteUri });
            }
            else
            {
                return RedirectToAction("MessageShow", "Home", new { messageBody = "T.C. Kimlik Numaranızı Doğru Girdiğinizden Emin Olun", returnUrl = Request.UrlReferrer.AbsoluteUri });
            }
        }


        [AllowAnonymous]
        public ActionResult ResendEmail()
        {
            try
            {
                if (!string.IsNullOrEmpty(userEmail))
                {
                    HomeController.SendEMail(userEmail, "E-postanızı yandaki linke tıklayarak onaylayabilirsiniz. " + callbackUrl, "Üyelik Onayı");
                }
                else
                {
                    HomeController.SendEMail(Session["ResendEmail"].ToString(), "E-postanızı yandaki linke tıklayarak onaylayabilirsiniz. " + Session["ConfirmationUrl"].ToString(), "Üyelik Onayı");
                }
                ViewBag.Message = "Onaylama E-Postası Başarıyla Gönderildi.";
                ViewBag.ReturnUrl = "/Account/Login";
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Onay E-Postası Gönderimi Sırasında Bir Hata Oluştu.";
                ViewBag.ReturnUrl = "/User/ResendEmail";
                ViewBag.Message2 = "/Home";
            }
            return PartialView("_UploadUserCV");
        }

        public ActionResult AssignUserRole()
        {

            return View();
        }
    }
}