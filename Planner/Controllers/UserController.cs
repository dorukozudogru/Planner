﻿using Planner.Enums;
using Planner.Helpers;
using Planner.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;

namespace Planner.Controllers
{
    //[UserAuthorize]
    public class UserController : Controller
    {
        private DBContext db = new DBContext();

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
                vwLstUc.ToList();
                return View(vwLstUc);
            }
            catch (Exception ex)
            {
                List<vwUsersProjects> vwLstUp = new List<vwUsersProjects>();
                vwLstUp.ToList();
                return View(vwLstUp);
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
                vwLstUc.ToList();

                return View(vwLstUc);
            }
            catch (Exception)
            {
                List<vwUsersCVs> vwLstUp = new List<vwUsersCVs>();
                vwLstUp.ToList();
                return View(vwLstUp);
            }
        }

        public ActionResult ChangeUserToAdmin(string Id, string returnUrl)
        {
            User userModel = db.User.FirstOrDefault(a => a.Id == Id);
            userModel.IsAdmin = true;
            db.SaveChanges();
            return RedirectToAction("MessageShow", "Home", new { messageBody = "Kullanıcı Admin Olarak Ayarlanmıştır.", returnUrl });
        }

        #region Views
        public ActionResult UserMenu(int? loggedUserId)
        {
            if (Convert.ToString(Session["UserId"]) != "")
            {
                if (Convert.ToBoolean(Session["UserIsAdmin"]) == true)
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

        [AllowAnonymous]
        public ActionResult Rejected()
        {
            return View();
        }

        public ActionResult AdminUserMenu()
        {
            return View();
        }

        public ActionResult UploadUserCV()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult LogInApprovement()
        {
            return View();
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        #endregion

        #region Approvement
        public ActionResult ApproveUser(string id, string returnUrl)
        {
            if (id == null || id == "")
            {
                return RedirectToAction("MessageShow", "Home", new { messageBody = "Kullanıcı Id'si Bulunamadı", returnUrl });
            }

            var user = db.User.SingleOrDefault(m => m.Id == id);
            if (user == null)
            {
                return RedirectToAction("MessageShow", "Home", new { messageBody = "Kullanıcı Bulunamadı", returnUrl });
            }

            if (user != null)
            {
                user.IsApproved = Convert.ToInt32(UserApproveEnum.Approved);
                user.LastEditDate = DateTime.Now;
                user.LastEditBy = Convert.ToInt32(Session["UserId"]);
                db.SaveChanges();
                try
                {
                    HomeController.SendEMail(user.EMail, "Üyeliğiniz Onaylanmıştır.");
                    return RedirectToAction("MessageShow", "Home", new { messageBody = "Kullanıcı Onaylanmıştır", returnUrl });
                }
                catch (Exception ex)
                {
                    return RedirectToAction("MessageShow", "Home", new { messageBody = "Kullanıcı Onaylanmıştır Fakat E-Posta Gönderiminde Bir Hata Oluşmuştur", returnUrl });
                }
            }
            return View();
        }

        public ActionResult DeclineUser(string id, string returnUrl)
        {
            if (id == null || id == "")
            {
                return RedirectToAction("MessageShow", "Home", new { messageBody = "Kullanıcı Id'si Bulunamadı", returnUrl });
            }

            var user = db.User.SingleOrDefault(m => m.Id == id);
            if (user == null)
            {
                return RedirectToAction("MessageShow", "Home", new { messageBody = "Kullanıcı Bulunamadı", returnUrl });
            }
            if (user != null)
            {
                user.IsApproved = Convert.ToInt32(UserApproveEnum.NotApproved);
                user.LastEditDate = DateTime.Now;
                user.LastEditBy = Convert.ToInt32(Session["UserId"]);
                db.SaveChanges();
                try
                {
                    HomeController.SendEMail(user.EMail, "Üyeliğiniz Reddedilmiştir.");
                    return RedirectToAction("MessageShow", "Home", new { messageBody = "Kullanıcı Hesabı Reddedilmiştir", returnUrl });
                }
                catch (Exception ex)
                {
                    return RedirectToAction("MessageShow", "Home", new { messageBody = "Kullanıcı Hesabı Reddedilmiştir Fakat E-Posta Gönderiminde Bir Hata Oluşmuştur", returnUrl });
                }
            }
            return View();
        }

        public ActionResult BlockUser(string id, string returnUrl)
        {
            if (id == null || id == "")
            {
                return RedirectToAction("MessageShow", "Home", new { messageBody = "Kullanıcı Id'si Bulunamadı", returnUrl });
            }

            var user = db.User.SingleOrDefault(m => m.Id == id);
            if (user == null)
            {
                return RedirectToAction("MessageShow", "Home", new { messageBody = "Kullanıcı Bulunamadı", returnUrl });
            }

            if (user != null)
            {
                user.IsApproved = Convert.ToInt32(UserApproveEnum.Blocked);
                user.LastEditDate = DateTime.Now;
                user.LastEditBy = Convert.ToInt32(Session["UserId"]);
                db.SaveChanges();
                try
                {
                    HomeController.SendEMail(user.EMail, "Üyeliğiniz Dondurulmuştur.");
                    return RedirectToAction("MessageShow", "Home", new { messageBody = "Kullanıcı Hesabı Engellenmiştir", returnUrl });
                }
                catch (Exception ex)
                {
                    return RedirectToAction("MessageShow", "Home", new { messageBody = "Kullanıcı Hesabı Engellenmiştir Fakat E-Posta Gönderiminde Bir Hata Oluşmuştur", Request.UrlReferrer.AbsoluteUri });
                }
            }
            return View();
        }

        public ActionResult ApproveAfterBlockUser(string id, string returnUrl)
        {
            if (id == null || id == "")
            {
                return RedirectToAction("MessageShow", "Home", new { messageBody = "Kullanıcı Id'si Bulunamadı", returnUrl });
            }

            var user = db.User.SingleOrDefault(m => m.Id == id);
            if (user == null)
            {
                return RedirectToAction("MessageShow", "Home", new { messageBody = "Kullanıcı Bulunamadı", returnUrl });
            }

            if (user != null)
            {
                user.IsApproved = Convert.ToInt32(UserApproveEnum.ApproveAfterBlock);
                user.LastEditDate = DateTime.Now;
                user.LastEditBy = Convert.ToInt32(Session["UserId"]);
                db.SaveChanges();
                try
                {
                    HomeController.SendEMail(user.EMail, "Üyeliğiniz Yeniden Aktif Edilmiştir.");
                    return RedirectToAction("MessageShow", "Home", new { messageBody = "Kullanıcı Hesabı Yeniden Aktif Edilmiştir", returnUrl });
                }
                catch (Exception ex)
                {
                    return RedirectToAction("MessageShow", "Home", new { messageBody = "Kullanıcı Hesabı Yeniden Aktif Edilmiştir Fakat E-Posta Gönderiminde Bir Hata Oluşmuştur", returnUrl });
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
                        if (userModel.Id == item.UserId)
                        {
                            string pId = item.ProjectId;
                            pModel = db.Projects.First(z => z.Id == pId);
                            lstUp.Add(new Project { Id = pModel.Id, Name = pModel.Name, Description = pModel.Description, FileName = pModel.FileName, IsApproved = pModel.IsApproved, IsSupported = pModel.IsSupported });
                        }
                    }
                    lstUp.ToList();
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
                var lstNull = "";
                lstNull.ToList();
                return View(lstNull);
            }
        }

        [HttpPost]
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
                    string sessionCitizenship = Convert.ToString(Session["UserCitizenshipNo"]);

                    var fileName = Path.GetFileName(fileCV.FileName);
                    var path = Path.Combine(Server.MapPath("~/Files/CV"), (sessionCitizenship + "_CV_" + fileName));
                    fileCV.SaveAs(path);

                    User u = new User();
                    UserCV cv = new UserCV();

                    var userControl = db.User.SingleOrDefault(m => m.CitizenshipNo == sessionCitizenship);

                    if (userControl.IsCvUploaded != false)
                    {
                        cv = db.UserCV.SingleOrDefault(m => m.UserId == userControl.Id);
                        cv.FileName = fileName;
                        cv.FilePath = path;
                        cv.CreationDate = DateTime.Now;
                    }
                    else
                    {
                        cv.UserId = Convert.ToString(Session["UserId"]);
                        cv.FileName = fileName;
                        cv.FilePath = path;
                        cv.CreationDate = DateTime.Now;
                        db.UserCV.Add(cv);

                        var user = db.User.SingleOrDefault(m => m.Id == cv.UserId);
                        user.IsCvUploaded = true;
                    }
                    db.SaveChanges();
                }
                ViewBag.Message = "CV Başarıyla Yüklendi";
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Message = "CV Yükleme Sırasında Bir Hata Oluştu";
                return View();
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
                string returnUrl = "";
                return RedirectToAction("MessageShow", "Home", new { messageBody = "Dosya Bulunamadı", returnUrl });
            }
        }
    }
}