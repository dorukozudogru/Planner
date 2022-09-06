using Planner.Enums;
using Planner.Helpers;
using Planner.DataAccess;
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
        static string callbackUrl = "";
        static string userEmail = "";

        public ActionResult UserIndex()
        {
            if (Convert.ToBoolean(Session["UserIsAdmin"]) != false)
            {
                return View(BasicRepository<Users>.Fetch());
            }
            return RedirectToAction("UserMenu");
        }

        public ActionResult UserCVIndex()
        {
            try
            {
                Users userModel = new Users();
                List<Users> lstUser = new List<Users>();

                UserCV ucModel = new UserCV();
                List<vwUsersCV> vwLstUc = new List<vwUsersCV>();

                foreach (var item in BasicRepository<UserCV>.Fetch())
                {
                    ucModel = BasicRepository<UserCV>.FirstOrDefault("WHERE Id = @0", item.Id);
                    userModel = BasicRepository<Users>.FirstOrDefault("WHERE Id = @0", ucModel.UserId);
                    if (Convert.ToBoolean(Session["UserIsAdmin"]) != false)
                    {
                        vwLstUc.Add(new vwUsersCV { UserId = userModel.Id, EMail = userModel.EMail, Name = userModel.Name, Surname = userModel.Surname, CitizenshipNo = userModel.CitizenshipNo, UserCVId = ucModel.Id, FileName = ucModel.FileName, FilePath = ucModel.FilePath });
                    }
                }
                return View(vwLstUc);
            }
            catch (Exception ex)
            {
                List<vwUsersCV> vwLstUc = new List<vwUsersCV>();
                return View(vwLstUc);
            }
        }

        public ActionResult UserCV(string loggedUserId)
        {
            try
            {
                Users userModel = new Users();
                List<Users> lstUser = new List<Users>();

                UserCV ucModel = new UserCV();
                List<vwUsersCV> vwLstUc = new List<vwUsersCV>();

                ucModel = BasicRepository<UserCV>.FirstOrDefault("WHERE UserId = @0", loggedUserId);
                userModel = BasicRepository<Users>.FirstOrDefault("WHERE Id = @0", loggedUserId);

                vwLstUc.Add(new vwUsersCV { UserId = userModel.Id, EMail = userModel.EMail, Name = userModel.Name, Surname = userModel.Surname, CitizenshipNo = userModel.CitizenshipNo, UserCVId = ucModel.Id, FileName = ucModel.FileName, FilePath = ucModel.FilePath });
                return View(vwLstUc);
            }
            catch (Exception)
            {
                List<vwUsersCV> vwLstUp = new List<vwUsersCV>();
                return View(vwLstUp);
            }
        }

        public ActionResult AssignUserAsAdmin(string Id)
        {
            Users userModel = BasicRepository<Users>.FirstOrDefault("WHERE Id = @0", Id);
            userModel.IsAdmin = true;
            BasicRepository<Users>.Update(userModel);
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
                    Users userModel = new Users();
                    List<Users> lstUser = new List<Users>();

                    Project pModel = new Project();
                    List<Project> lstProject = new List<Project>();

                    UserProject upModel = new UserProject();
                    List<vwUsersProject> vwLstUp = new List<vwUsersProject>();

                    bool userAuthorized = false;

                    foreach (var item in BasicRepository<UserProject>.Fetch())
                    {
                        string _tempUserId = Session["UserId"].ToString();
                        ProjectUserAuthorize authorize = BasicRepository<ProjectUserAuthorize>.FirstOrDefault("WHERE ProjectId = @0", item.ProjectId);
                        if (authorize == null) //Authorize proje için null gelmişse herkes görebilmeli
                        {
                            int upId = item.Id;
                            upModel = BasicRepository<UserProject>.FirstOrDefault("WHERE Id = @0", upId);
                            pModel = BasicRepository<Project>.FirstOrDefault("WHERE Id = @0", upModel.ProjectId);
                            userModel = BasicRepository<Users>.FirstOrDefault("WHERE Id = @0", upModel.UserId);
                            vwLstUp.Add(new vwUsersProject
                            {
                                Id = userModel.Id,
                                Name = userModel.Name,
                                Surname = userModel.Surname,
                                EMail = userModel.EMail,
                                CitizenshipNo = userModel.CitizenshipNo,
                                ProjectId = pModel.Id,
                                ProjectName = pModel.Name,
                                FileName = pModel.FileName,
                                IsApproved = pModel.IsApproved,
                                IsSupported = pModel.IsSupported,
                                Description = pModel.Description,
                                CreationDate = pModel.CreationDate
                            });
                        }
                        else //Authorize proje için null değilse kimlerin görebildiğine bakılmalı
                        {
                            authorize = BasicRepository<ProjectUserAuthorize>.FirstOrDefault("WHERE AuthorizedUserId = @0 AND ProjectId = @1", _tempUserId, item.ProjectId);
                            if (authorize == null)
                                userAuthorized = false;
                            else if (authorize != null)
                                userAuthorized = true;

                            if (userAuthorized)
                            {
                                upModel = BasicRepository<UserProject>.FirstOrDefault("WHERE Id = @0", item.Id);
                                pModel = BasicRepository<Project>.FirstOrDefault("WHERE Id = @0", upModel.ProjectId);
                                userModel = BasicRepository<Users>.FirstOrDefault("WHERE Id = @0", upModel.UserId);
                                vwLstUp.Add(new vwUsersProject
                                {
                                    Id = userModel.Id,
                                    Name = userModel.Name,
                                    Surname = userModel.Surname,
                                    EMail = userModel.EMail,
                                    CitizenshipNo = userModel.CitizenshipNo,
                                    ProjectId = pModel.Id,
                                    ProjectName = pModel.Name,
                                    FileName = pModel.FileName,
                                    IsApproved = pModel.IsApproved,
                                    IsSupported = pModel.IsSupported,
                                    Description = pModel.Description,
                                    CreationDate = pModel.CreationDate
                                });
                            }
                        }
                    }
                    vwLstUp = vwLstUp.OrderByDescending(x => x.CreationDate).ToList();
                    if (vwLstUp.Count > 5)
                    {
                        vwLstUp.RemoveRange(5, vwLstUp.Count - 5);
                    }
                    return View(vwLstUp);
                }
                catch (Exception ex)
                {
                    List<vwUsersProject> vwLstUp = new List<vwUsersProject>();
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

            var user = BasicRepository<Users>.FirstOrDefault("WHERE Id = @0", id);
            if (user == null)
            {
                return RedirectToAction("MessageShow", "Home", new { messageBody = "Kullanıcı Bulunamadı", returnUrl = Request.UrlReferrer.AbsoluteUri });
            }

            if (user != null)
            {
                user.IsApproved = Convert.ToInt32(UserApproveEnum.Approved);
                user.LastEditDate = DateTime.Now;
                user.LastEditBy = Convert.ToString(Session["UserId"]);
                BasicRepository<Users>.Update(user);
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

            var user = BasicRepository<Users>.FirstOrDefault("WHERE Id = @0", id);
            if (user == null)
            {
                return RedirectToAction("MessageShow", "Home", new { messageBody = "Kullanıcı Bulunamadı", returnUrl = Request.UrlReferrer.AbsoluteUri });
            }
            if (user != null)
            {
                user.IsApproved = Convert.ToInt32(UserApproveEnum.NotApproved);
                user.LastEditDate = DateTime.Now;
                user.LastEditBy = Convert.ToString(Session["UserId"]);
                BasicRepository<Users>.Update(user);
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

            var user = BasicRepository<Users>.FirstOrDefault("WHERE Id = @0", id);
            if (user == null)
            {
                return RedirectToAction("MessageShow", "Home", new { messageBody = "Kullanıcı Bulunamadı", returnUrl = Request.UrlReferrer.AbsoluteUri });
            }

            if (user != null)
            {
                user.IsApproved = Convert.ToInt32(UserApproveEnum.Blocked);
                user.LastEditDate = DateTime.Now;
                user.LastEditBy = Convert.ToString(Session["UserId"]);
                BasicRepository<Users>.Update(user);
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

            var user = BasicRepository<Users>.FirstOrDefault("WHERE Id = @0", id);
            if (user == null)
            {
                return RedirectToAction("MessageShow", "Home", new { messageBody = "Kullanıcı Bulunamadı", returnUrl = Request.UrlReferrer.AbsoluteUri });
            }

            if (user != null)
            {
                user.IsApproved = Convert.ToInt32(UserApproveEnum.ApproveAfterBlock);
                user.LastEditDate = DateTime.Now;
                user.LastEditBy = Convert.ToString(Session["UserId"]);
                BasicRepository<Users>.Update(user);
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
                    Users userModel = new Users();
                    List<Users> lstUser = new List<Users>();

                    foreach (var item in BasicRepository<Users>.Fetch())
                    {
                        if (item.IsApproved == Convert.ToInt32(UserApproveEnum.Approved))
                        {
                            lstUser.Add(new Users
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
                    List<Users> lstUser = new List<Users>();
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
                Planner.DataAccess.DBContextDB.GetInstance().Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult UserProjectIndex(string loggedUserId)
        {
            try
            {
                if (loggedUserId == Convert.ToString(Session["UserId"]))
                {
                    Users userModel = BasicRepository<Users>.FirstOrDefault("WHERE Id = @0", loggedUserId);
                    Project pModel = new Project();
                    List<Project> lstUp = new List<Project>();
                    foreach (var item in BasicRepository<UserProject>.Fetch())
                    {
                        if (userModel.Id == item.UserId && item.IsApproveChanged != Convert.ToInt32(ProjectTypeChangeEnum.ChangedAsNotApproved))
                        {
                            pModel = BasicRepository<Project>.FirstOrDefault("WHERE Id = @0", item.ProjectId);
                            lstUp.Add(new Project { Id = pModel.Id, Name = pModel.Name, Description = pModel.Description, FileName = pModel.FileName, IsApproved = pModel.IsApproved, IsSupported = pModel.IsSupported, CreationDate = pModel.CreationDate });
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
                    Users userModel = BasicRepository<Users>.FirstOrDefault("WHERE Id = @0", loggedUserId);
                    Project pModel = new Project();
                    RejectedProject rProjects = new RejectedProject();
                    List<vwRejectedProjects> lstUp = new List<vwRejectedProjects>();
                    foreach (var item in BasicRepository<UserProject>.Fetch())
                    {
                        if (userModel.Id == item.UserId && item.IsApproveChanged == Convert.ToInt32(ProjectTypeChangeEnum.ChangedAsNotApproved))
                        {
                            pModel = BasicRepository<Project>.FirstOrDefault("WHERE Id = @0", item.ProjectId);
                            rProjects = BasicRepository<RejectedProject>.FirstOrDefault("WHERE RejectedProjectId = @0", pModel.Id);
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

                    var userControl = BasicRepository<Users>.FirstOrDefault("WHERE CitizenshipNo = @0", citizenshipNo);

                    if (userControl.IsCvUploaded != false)
                    {
                        cv = BasicRepository<UserCV>.FirstOrDefault("WHERE UserId = @0", userControl.Id);
                        cv.FileName = fileName;
                        cv.FilePath = path;
                        cv.CreationDate = DateTime.Now;
                        BasicRepository<UserCV>.Update(cv);
                        return RedirectToAction("MessageShow", "Home", new { messageBody = "CV Başarıyla Güncellendi", returnUrl = Request.UrlReferrer.AbsoluteUri });
                    }
                    else
                    {
                        cv.UserId = userControl.Id;
                        cv.FileName = fileName;
                        cv.FilePath = path;
                        cv.CreationDate = DateTime.Now;
                        BasicRepository<UserCV>.Insert(cv);

                        var user = BasicRepository<Users>.FirstOrDefault("WHERE Id = @0", cv.UserId);
                        user.IsCvUploaded = true;
                        BasicRepository<Users>.Update(user);
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
                UserCV ucmodel = BasicRepository<UserCV>.FirstOrDefault("WHERE Id = @0", Id);
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
                Users userModel = BasicRepository<Users>.FirstOrDefault("WHERE Id = @0", loggedUserId);
                return View(userModel);
            }
            catch (Exception ex)
            {
                return RedirectToAction("MessageShow", "Home", new { messageBody = "Beklenmeyen Bir Hata Oluştu", returnUrl = Request.UrlReferrer.AbsoluteUri });
            }
        }

        [HttpPost]
        public ActionResult EditProfile(Users user)
        {
            if (HomeController.ControlCitizenshipNo(user.CitizenshipNo))
            {
                Users model = BasicRepository<Users>.FirstOrDefault("WHERE EMail = @0", user.EMail);
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
                BasicRepository<Users>.Update(model);
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