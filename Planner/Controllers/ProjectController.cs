using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using Planner.Enums;
using Planner.Helpers;
using Planner.Models;
using System.Net.Mime;

namespace Planner.Controllers
{
    [UserAuthorize]
    public class ProjectController : Controller
    {
        DBContext db = new DBContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateProject()
        {
            return View();
        }

        #region ListProjects
        public ActionResult AllProjects()
        {
            if (Convert.ToBoolean(Session["UserIsAdmin"]) != false)
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
                                ProjectDescription = pModel.Description
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
                                    ProjectDescription = pModel.Description
                                });
                            }
                        }
                    }
                    return View(vwLstUp);
                }
                catch (Exception ex)
                {
                    List<vwUsersProjects> vwLstUp = new List<vwUsersProjects>();
                    return View(vwLstUp);
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public ActionResult ApprovedProjects()
        {
            try
            {
                User userModel = new User();
                List<User> lstUser = new List<User>();

                Project pModel = new Project();
                List<Project> lstProject = new List<Project>();

                UserProject upModel = new UserProject();
                List<vwUsersProjects> vwLstUp = new List<vwUsersProjects>();

                bool userAuthorized = true;

                foreach (var item in db.UserProject)
                {
                    string _tempUserId = Session["UserId"].ToString();
                    ProjectUserAuthorize authorize = db.ProjectUserAuthorize.FirstOrDefault(a => a.ProjectId == item.ProjectId);
                    if (authorize == null)
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
                            ProjectDescription = pModel.Description
                        });
                    }
                    else
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
                            if (pModel.IsApproved == Convert.ToInt32(ProjectApproveEnum.Approved) && pModel.IsSupported == Convert.ToInt32(ProjectSupportEnum.NotSupported))
                            {
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
                                    ProjectDescription = pModel.Description
                                });
                            }
                        }
                    }
                }
                return View(vwLstUp);
            }
            catch (Exception ex)
            {
                List<vwUsersProjects> vwLstUp = new List<vwUsersProjects>();
                return View(vwLstUp);
            }
        }

        public ActionResult SupportedProjects()
        {
            try
            {
                User userModel = new User();
                List<User> lstUser = new List<User>();

                Project pModel = new Project();
                List<Project> lstProject = new List<Project>();

                UserProject upModel = new UserProject();
                List<vwUsersProjects> vwLstUp = new List<vwUsersProjects>();

                bool userAuthorized = true;

                foreach (var item in db.UserProject)
                {
                    string _tempUserId = Session["UserId"].ToString();
                    ProjectUserAuthorize authorize = db.ProjectUserAuthorize.FirstOrDefault(a => a.ProjectId == item.ProjectId);
                    if (authorize == null)
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
                            ProjectDescription = pModel.Description
                        });
                    }
                    else
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
                            if (pModel.IsSupported == Convert.ToInt32(ProjectSupportEnum.Supported) && pModel.IsApproved == Convert.ToInt32(ProjectApproveEnum.Approved))
                            {
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
                                    ProjectDescription = pModel.Description
                                });
                            }
                        }
                    }
                }
                return View(vwLstUp);
            }
            catch (Exception ex)
            {
                List<vwUsersProjects> vwLstUp = new List<vwUsersProjects>();
                return View(vwLstUp);
            }
        }

        public ActionResult WaitingForApprove()
        {
            if (Convert.ToBoolean(Session["UserIsAdmin"]) != false)
            {
                try
                {
                    User userModel = new User();
                    List<User> lstUser = new List<User>();

                    Project pModel = new Project();
                    List<Project> lstProject = new List<Project>();

                    UserProject upModel = new UserProject();
                    List<vwUsersProjects> vwLstUp = new List<vwUsersProjects>();

                    bool userAuthorized = true;

                    foreach (var item in db.UserProject)
                    {
                        string _tempUserId = Session["UserId"].ToString();
                        ProjectUserAuthorize authorize = db.ProjectUserAuthorize.FirstOrDefault(a => a.ProjectId == item.ProjectId);
                        if (authorize == null)
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
                                ProjectDescription = pModel.Description
                            });
                        }
                        else
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
                                if (pModel.IsApproved == Convert.ToInt32(ProjectApproveEnum.WaitingToApprove))
                                {
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
                                        ProjectDescription = pModel.Description
                                    });
                                }
                            }
                        }
                    }
                    return View(vwLstUp);
                }
                catch (Exception ex)
                {
                    List<vwUsersProjects> vwLstUp = new List<vwUsersProjects>();
                    return View(vwLstUp);
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public ActionResult WaitingForSupport()
        {
            try
            {
                User userModel = new User();
                List<User> lstUser = new List<User>();

                Project pModel = new Project();
                List<Project> lstProject = new List<Project>();

                UserProject upModel = new UserProject();
                List<vwUsersProjects> vwLstUp = new List<vwUsersProjects>();

                bool userAuthorized = true;

                foreach (var item in db.UserProject)
                {
                    string _tempUserId = Session["UserId"].ToString();
                    ProjectUserAuthorize authorize = db.ProjectUserAuthorize.FirstOrDefault(a => a.ProjectId == item.ProjectId);
                    if (authorize == null)
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
                            ProjectDescription = pModel.Description
                        });
                    }
                    else
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
                            if (pModel.IsSupported == Convert.ToInt32(ProjectSupportEnum.WaitingToSupport) && pModel.IsSupported != Convert.ToInt32(ProjectSupportEnum.Closed))
                            {
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
                                    ProjectDescription = pModel.Description
                                });
                            }
                        }
                    }
                }
                return View(vwLstUp);
            }
            catch (Exception ex)
            {
                List<vwUsersProjects> vwLstUp = new List<vwUsersProjects>();
                return View(vwLstUp);
            }
        }
        #endregion

        #region Approve&SupportProjects
        public ActionResult ApproveProject(string Id)
        {
            try
            {
                Project pmodel = new Project();
                UserProject upmodel = new UserProject();
                pmodel = db.Projects.FirstOrDefault(m => m.Id == Id);
                upmodel = db.UserProject.FirstOrDefault(m => m.ProjectId == Id);
                pmodel.IsApproved = Convert.ToInt32(ProjectApproveEnum.Approved);
                upmodel.IsApproved = Convert.ToInt32(ProjectApproveEnum.Approved);
                pmodel.IsApproveChanged = Convert.ToInt32(ProjectTypeChangeEnum.ChangeAsApproved);
                upmodel.IsApproveChanged = Convert.ToInt32(ProjectTypeChangeEnum.ChangeAsApproved);
                pmodel.LastEditDate = DateTime.Now;
                pmodel.LastEditBy = Convert.ToString(Session["UserId"]);

                db.SaveChanges();
                User umodel = new User();
                umodel = db.User.FirstOrDefault(m => m.Id == upmodel.UserId);
                try
                {
                    HomeController.SendEMail(umodel.EMail, "Projeniz Onaylanmıştır. Destek başvurusu yapmak için sisteme giriş yapınız.", "Proje Onayı");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("MessageShow", "Home", new { messageBody = "Proje Onaylanmıştır Fakat E-Posta Gönderiminde Bir Hata Oluşmuştur", returnUrl = Request.UrlReferrer.AbsoluteUri });
                }
                return RedirectToAction("MessageShow", "Home", new { messageBody = "Proje Onaylanmıştır", returnUrl = Request.UrlReferrer.AbsoluteUri });
            }
            catch (Exception ex)
            {
                return RedirectToAction("MessageShow", "Home", new { messageBody = "Proje Onaylanırken Bir Hata Oluştu", returnUrl = Request.UrlReferrer.AbsoluteUri });
            }
        }

        public ActionResult DeclineProject(string Id)
        {
            try
            {
                Project pmodel = new Project();
                UserProject upmodel = new UserProject();
                pmodel = db.Projects.FirstOrDefault(m => m.Id == Id);
                upmodel = db.UserProject.FirstOrDefault(m => m.ProjectId == Id);
                pmodel.IsApproved = Convert.ToInt32(ProjectApproveEnum.NotApproved);
                upmodel.IsApproved = Convert.ToInt32(ProjectApproveEnum.NotApproved);
                pmodel.IsApproveChanged = Convert.ToInt32(ProjectTypeChangeEnum.ChangeAsNotApproved);
                upmodel.IsApproveChanged = Convert.ToInt32(ProjectTypeChangeEnum.ChangeAsNotApproved);
                pmodel.LastEditDate = DateTime.Now;
                pmodel.LastEditBy = Convert.ToString(Session["UserId"]);
                db.SaveChanges();
                User umodel = new User();
                umodel = db.User.FirstOrDefault(m => m.Id == upmodel.UserId);
                try
                {
                    HomeController.SendEMail(umodel.EMail, "Projeniz Onaylanmamıştır.", "Proje Onayı");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("MessageShow", "Home", new { messageBody = "Proje Reddedilmiştir Fakat E-Posta Gönderiminde Bir Hata Oluşmuştur", returnUrl = Request.UrlReferrer.AbsoluteUri });
                }
                return RedirectToAction("MessageShow", "Home", new { messageBody = "Proje Reddedilmiştir", returnUrl = Request.UrlReferrer.AbsoluteUri });
            }
            catch (Exception ex)
            {
                return RedirectToAction("MessageShow", "Home", new { messageBody = "Proje Reddedilirken Bir Hata Oluştu", returnUrl = Request.UrlReferrer.AbsoluteUri });
            }
        }

        public ActionResult SupportToProject(string Id, string supportRequirements, string supportValue)
        {
            try
            {
                UserProject upModel = db.UserProject.FirstOrDefault(a => a.ProjectId == Id);
                User uModel = db.User.FirstOrDefault(a => a.Id == upModel.UserId);
                Project pModel = db.Projects.FirstOrDefault(a => a.Id == upModel.ProjectId);
                SupportedProjects sProjects = new SupportedProjects();

                if (pModel.IsApproved == Convert.ToInt32(ProjectApproveEnum.Approved))
                {
                    pModel.IsApproveChanged = Convert.ToInt32(ProjectTypeChangeEnum.ChangeAsSupported);
                    upModel.IsApproveChanged = Convert.ToInt32(ProjectTypeChangeEnum.ChangeAsSupported);
                    pModel.LastEditDate = DateTime.Now;
                    pModel.LastEditBy = Convert.ToString(Session["UserId"]);

                    sProjects.SupportDate = DateTime.Now;
                    sProjects.SupportedProjectId = Id;
                    sProjects.SupporterUserId = Session["UserId"].ToString();
                    sProjects.SupportRequirements = supportRequirements;
                    sProjects.SupportValue = supportValue;
                    db.SupportedProjects.Add(sProjects);
                    db.SaveChanges();
                    HomeController.SendEMail(uModel.EMail, "Projenize Destek Verilmiştir. Detaylı Bilgi İçin Sisteminizi Kontrol Ediniz.", "Proje Desteği");
                    return RedirectToAction("MessageShow", "Home", new { messageBody = "Destek Bilgileriniz İletilmiştir. Sorumlu Kişiler Bilgilendirilecektir.", returnUrl = Request.UrlReferrer.AbsoluteUri });
                }
                else
                {
                    return RedirectToAction("MessageShow", "Home", new { messageBody = "Projenin (Bir Önceki Adım Olarak) Onaylandığına Emin Olun.", returnUrl = Request.UrlReferrer.AbsoluteUri });
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("MessageShow", "Home", new { messageBody = "Hata Oluştu. Lütfen Tekrar Deneyin.", returnUrl = Request.UrlReferrer.AbsoluteUri });
            }
        }

        public ActionResult DeclineSupportProject(string Id)
        {
            try
            {
                Project pmodel = new Project();
                UserProject upmodel = new UserProject();
                pmodel = db.Projects.FirstOrDefault(m => m.Id == Id);
                upmodel = db.UserProject.FirstOrDefault(m => m.ProjectId == Id);
                pmodel.IsApproved = Convert.ToInt32(ProjectSupportEnum.NotSupported);
                upmodel.IsApproved = Convert.ToInt32(ProjectSupportEnum.NotSupported);
                pmodel.IsApproveChanged = Convert.ToInt32(ProjectTypeChangeEnum.ChangeAsNotSupported);
                upmodel.IsApproveChanged = Convert.ToInt32(ProjectTypeChangeEnum.ChangeAsNotSupported);
                pmodel.LastEditDate = DateTime.Now;
                pmodel.LastEditBy = Convert.ToString(Session["UserId"]);
                db.SaveChanges();
                User umodel = new User();
                umodel = db.User.FirstOrDefault(m => m.Id == upmodel.UserId);
                HomeController.SendEMail(umodel.EMail, "Projeniz Destek Sürecinde Onaylanmamıştır.", "Proje Desteği");
                return RedirectToAction("MessageShow", "Home", new { messageBody = "Proje Desteği Reddedilmiştir", returnUrl = Request.UrlReferrer.AbsoluteUri });
            }
            catch (Exception ex)
            {
                return RedirectToAction("MessageShow", "Home", new { messageBody = "Proje Desteği Reddedilirken Bir Hata Oluştu", returnUrl = Request.UrlReferrer.AbsoluteUri });
            }
        }

        public ActionResult CloseSupportedProject(string Id)
        {
            Project project = db.Projects.FirstOrDefault(a => a.Id == Id);
            UserProject userProject = db.UserProject.FirstOrDefault(a => a.ProjectId == Id);
            User user = db.User.FirstOrDefault(a => a.Id == userProject.UserId);

            //project.IsSupported = Convert.ToInt32(ProjectSupportEnum.Supported);
            //userProject.IsSupported = Convert.ToInt32(ProjectSupportEnum.Supported);
            project.IsSupported = Convert.ToInt32(ProjectSupportEnum.Closed);
            userProject.IsSupported = Convert.ToInt32(ProjectSupportEnum.Closed);
            db.SaveChanges();
            HomeController.SendEMail(user.EMail, "Projenizin Destek Başvurusu Kapatılmıştır.", "Proje Desteği");
            return RedirectToAction("MessageShow", "Home", new { messageBody = "Proje Desteği Kapatılmıştır", returnUrl = Request.UrlReferrer.AbsoluteUri });
        }
        #endregion

        public virtual ActionResult Download(string Id)
        {
            try
            {
                Project pmodel = db.Projects.FirstOrDefault(m => m.Id == Id);
                byte[] fileBytes = System.IO.File.ReadAllBytes(pmodel.FilePath);
                var file = pmodel.FilePath;
                var cd = new ContentDisposition
                {
                    Inline = true,
                    FileName = pmodel.Name
                };
                Response.AddHeader("Content-Disposition", cd.ToString());
                //return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, pmodel.FileName); //For Download
                return File(fileBytes, pmodel.FileExtension); // For New Tab
            }
            catch (Exception)
            {
                return RedirectToAction("MessageShow", "Home", new { messageBody = "Dosya Bulunamadı", returnUrl = Request.UrlReferrer.AbsoluteUri });
            }
        }

        #region CreatingProject
        [HttpPost]
        public ActionResult UploadProject(HttpPostedFileBase fileProject, Project project)
        {
            try
            {
                if (fileProject.ContentType != "application/pdf")
                {
                    return RedirectToAction("Shared", "Error");
                }
                else if (fileProject.ContentLength > 0 && fileProject.ContentType == "application/pdf")
                {
                    string sessionCitizenship = Convert.ToString(Session["UserCitizenshipNo"]);
                    //For Project
                    var fileName = Path.GetFileName(fileProject.FileName);
                    var path = Path.Combine(Server.MapPath("~/Files/Project"), (sessionCitizenship + "_Proje_" + fileName));
                    fileProject.SaveAs(path);

                    project.FileName = fileName;
                    project.FileExtension = fileProject.ContentType;
                    project.FileSize = Convert.ToInt64(fileProject.ContentLength);
                    project.FilePath = path;
                    CreateProject(project);
                }
                return RedirectToAction("MessageShow", "Home", new { messageBody = "Projeniz Yüklenmiştir. Lütfen Projenizi Kontrol Ettikten Sonra Onaya Gönderiniz.", returnUrl = Request.UrlReferrer.AbsoluteUri });
            }
            catch (Exception ex)
            {
                return RedirectToAction("MessageShow", "Home", new { messageBody = "Yükleme Sırasında Bir Hata Oluştu", returnUrl = Request.UrlReferrer.AbsoluteUri });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateProject([Bind(Include = "Id,Name,Description,FileName,FilePath,FileSize,FileExtension,IsApproved,IsSupported,IsApproveChanged")] Project project)
        {
            UserProject up = new UserProject();
            try
            {
                project.Id = Guid.NewGuid().ToString();
                project.IsApproved = Convert.ToInt32(ProjectApproveEnum.NotApproved);
                project.IsSupported = Convert.ToInt32(ProjectSupportEnum.NotSupported);
                project.IsApproveChanged = Convert.ToInt32(ProjectTypeChangeEnum.NotChanged);
                project.CreationDate = DateTime.Now;
                project.LastEditDate = Convert.ToDateTime("1753-01-01");
                project.LastEditBy = "00000000-0000-0000-0000-000000000000";
                db.Projects.Add(project);
                db.SaveChanges();
                up.ProjectId = project.Id;
                up.UserId = Convert.ToString(Session["UserId"]);
                up.IsApproved = Convert.ToInt32(ProjectApproveEnum.NotApproved);
                up.IsSupported = Convert.ToInt32(ProjectSupportEnum.NotSupported);
                up.IsApproveChanged = Convert.ToInt32(ProjectTypeChangeEnum.NotChanged);
                db.UserProject.Add(up);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return RedirectToAction("MessageShow", "Home", new { messageBody = "Projeniz Kayıt Edilirken Bir Hata Oluştu", returnUrl = Request.UrlReferrer.AbsoluteUri });
            }
            return RedirectToAction("MessageShow", "Home", new { messageBody = "Projeniz Yüklenmiştir. Lütfen Projenizi Kontrol Ettikten Sonra Onaya Gönderiniz.", returnUrl = Request.UrlReferrer.AbsoluteUri });
        }
        #endregion

        #region SendToAdminForApprove&Support
        public ActionResult SendProjectToApprove(string Id)
        {
            try
            {
                Project projects = db.Projects.SingleOrDefault(m => m.Id == Id);
                UserProject ups = db.UserProject.SingleOrDefault(m => m.ProjectId == Id);
                ups.IsApproved = Convert.ToInt32(ProjectApproveEnum.WaitingToApprove);
                projects.IsApproved = Convert.ToInt32(ProjectApproveEnum.WaitingToApprove);
                projects.LastEditDate = DateTime.Now;
                projects.LastEditBy = Convert.ToString(Session["UserId"]);
                db.SaveChanges();
                return RedirectToAction("MessageShow", "Home", new { messageBody = "Proje Onaya Gönderilmiştir", returnUrl = Request.UrlReferrer.AbsoluteUri });
            }
            catch (Exception ex)
            {
                return RedirectToAction("MessageShow", "Home", new { messageBody = "Hata Oluştu. Lütfen Tekrar Deneyin", returnUrl = Request.UrlReferrer.AbsoluteUri });
            }
        }

        public ActionResult SendProjectToSupport(string Id, string supportRequest)
        {
            try
            {
                Project projects = db.Projects.SingleOrDefault(m => m.Id == Id);
                UserProject ups = db.UserProject.SingleOrDefault(m => m.ProjectId == Id);
                if (projects.IsApproved == Convert.ToInt32(ProjectApproveEnum.Approved))
                {
                    ups.SupportRequest = supportRequest;
                    ups.IsSupported = Convert.ToInt32(ProjectSupportEnum.WaitingToSupport);
                    projects.SupportRequest = supportRequest;
                    projects.IsSupported = Convert.ToInt32(ProjectSupportEnum.WaitingToSupport);
                    projects.LastEditDate = DateTime.Now;
                    projects.LastEditBy = Convert.ToString(Session["UserId"]);
                    db.SaveChanges();
                }
                return RedirectToAction("MessageShow", "Home", new { messageBody = "Proje Destek Onayına Gönderilmiştir", returnUrl = Request.UrlReferrer.AbsoluteUri });
            }
            catch (Exception ex)
            {
                return RedirectToAction("MessageShow", "Home", new { messageBody = "Hata Oluştu. Lütfen Tekrar Deneyin", returnUrl = Request.UrlReferrer.AbsoluteUri });
            }
        }
        #endregion

        #region Edit Project
        public ActionResult Edit(string id)
        {
            ViewBag.ReturnUrl = Request.UrlReferrer.AbsoluteUri;
            Project project = db.Projects.Find(id);

            if (id == null || project == null)
            {
                return View("UserMenu");
            }

            else
            {
                return View(project);
            }
        }

        [HttpPost]
        public ActionResult EditProject(HttpPostedFileBase fileProject2, Project project)
        {
            if (fileProject2 != null)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        string sessionCitizenship = Convert.ToString(Session["UserCitizenshipNo"]);

                        var fileName = Path.GetFileName(fileProject2.FileName);
                        var path = Path.Combine(Server.MapPath("~/Files/Project"), (sessionCitizenship + "_Güncel_Proje_" + fileName));
                        fileProject2.SaveAs(path);

                        var _tempProject = db.Projects.FirstOrDefault(m => m.Id == project.Id);

                        _tempProject.Name = project.Name;
                        _tempProject.Description = project.Description;
                        _tempProject.FileName = fileName;
                        _tempProject.FilePath = path;
                        _tempProject.FileSize = Convert.ToInt64(fileProject2.ContentLength);
                        _tempProject.FileExtension = fileProject2.ContentType;
                        _tempProject.IsApproved = _tempProject.IsApproved;
                        _tempProject.IsSupported = _tempProject.IsSupported;
                        _tempProject.IsApproveChanged = _tempProject.IsApproveChanged;
                        _tempProject.CreationDate = _tempProject.CreationDate;
                        _tempProject.LastEditBy = Convert.ToString(Session["UserId"]);
                        _tempProject.LastEditDate = DateTime.Now;

                        db.SaveChanges();
                    }
                    return RedirectToAction("MessageShow", "Home", new { messageBody = "Proje Başarıyla Güncellendi", returnUrl = Request.UrlReferrer.AbsoluteUri });
                }
                catch (Exception ex)
                {
                    return RedirectToAction("MessageShow", "Home", new { messageBody = "Güncelleme Sırasında Bir Hata Oluştu", returnUrl = Request.UrlReferrer.AbsoluteUri });
                }
            }
            else
            {
                return RedirectToAction("MessageShow", "Home", new { messageBody = "Lütfen Proje Dosyanızı Yükleyiniz.", returnUrl = Request.UrlReferrer.AbsoluteUri });
            }
        }
        #endregion

        #region ProjectUserAuthorization
        public ActionResult GiveAuthorizeToUserForProject(string id)
        {
            try
            {
                Session["ProjectId"] = id;
                User userModel = new User();
                List<User> User = new List<User>();

                foreach (var item in db.User)
                {
                    ProjectUserAuthorize userAuthorize = db.ProjectUserAuthorize.FirstOrDefault(a => a.ProjectId == id && a.AuthorizedUserId == item.Id);
                    if (userAuthorize == null)
                    {
                        User.Add(new User
                        {
                            BirthDate = item.BirthDate,
                            CitizenshipNo = item.CitizenshipNo,
                            City = item.City,
                            Department = item.Department,
                            EMail = item.EMail,
                            Id = item.Id,
                            IsActive = item.IsActive,
                            IsAdmin = item.IsAdmin,
                            IsApproved = item.IsApproved,
                            IsCvUploaded = item.IsCvUploaded,
                            Job = item.Job,
                            LastEditBy = item.LastEditBy,
                            LastEditDate = item.LastEditDate,
                            Name = item.Name,
                            Password = item.Password,
                            PhoneNumber = item.PhoneNumber,
                            RegisterDate = item.RegisterDate,
                            School = item.School,
                            Surname = item.Surname
                        });
                    }
                }
                return View(User);
            }
            catch (Exception ex)
            {
                List<User> lstUser = new List<User>();
                return View(lstUser);
            }
        }

        public ActionResult ProjectAuthorizedUser(string id)
        {
            try
            {
                Session["ProjectId"] = id;
                List<vwAuthorizedUserProject> authorizes = new List<vwAuthorizedUserProject>();
                foreach (var item in db.User)
                {
                    ProjectUserAuthorize userAuthorize = db.ProjectUserAuthorize.FirstOrDefault(a => a.ProjectId == id && a.AuthorizedUserId == item.Id);
                    if (userAuthorize != null)
                    {
                        authorizes.Add(new vwAuthorizedUserProject
                        {
                            ProjectId = userAuthorize.ProjectId,
                            UserEMail = item.EMail,
                            UserId = item.Id,
                            UserIsAdmin = item.IsAdmin,
                            UserName = item.Name,
                            UserSurname = item.Surname
                        });
                    }
                }
                return View(authorizes);
            }
            catch (Exception ex)
            {
                List<vwAuthorizedUserProject> authorizes = new List<vwAuthorizedUserProject>();
                return View(authorizes);
            }
        }

        [HttpPost]
        public ActionResult AddAuthorizeToUserForProject(List<string> User)
        {
            try
            {
                string _projectId = Session["ProjectId"].ToString();
                string _sessionUserId = Session["UserId"].ToString();
                ProjectUserAuthorize userAuthorize = new ProjectUserAuthorize();
                foreach (var item in User)
                {
                    userAuthorize.AuthorizedUserId = item;
                    userAuthorize.ProjectId = _projectId;
                    userAuthorize.LastEditBy = _sessionUserId;
                    userAuthorize.LastEditDate = DateTime.Now;
                    db.ProjectUserAuthorize.Add(userAuthorize);
                    db.SaveChanges();
                }

                //İşlemi Yapan Kişi Kendini Eklemezse
                var result = db.ProjectUserAuthorize.Where(a => a.AuthorizedUserId == _sessionUserId);
                if (result == null)
                {
                    userAuthorize.AuthorizedUserId = Session["UserId"].ToString();
                    userAuthorize.ProjectId = _projectId;
                    userAuthorize.LastEditBy = _sessionUserId;
                    userAuthorize.LastEditDate = DateTime.Now;
                    db.ProjectUserAuthorize.Add(userAuthorize);
                    db.SaveChanges();
                }
                //
                return RedirectToAction("MessageShow", "Home", new { messageBody = "Projeyi Görmeye Yetkili Kullanıcılar Ayarlanmıştır.", returnUrl = "/Project/WaitingForApprove" });
            }
            catch (Exception ex)
            {
                return RedirectToAction("MessageShow", "Home", new { messageBody = "Hata Oluştu. Lütfen Tekrar Deneyin.", returnUrl = Request.UrlReferrer.AbsoluteUri });
            }
        }

        [HttpPost]
        public ActionResult RemoveAuthorizeToUserForProject(List<string> User)
        {
            try
            {
                if (!User.Contains(Session["UserId"].ToString()))
                {
                    string _projectId = Session["ProjectId"].ToString();
                    List<ProjectUserAuthorize> userAuthorize = db.ProjectUserAuthorize.Where(a => a.ProjectId == _projectId).ToList();

                    foreach (var item in userAuthorize)
                    {
                        if (!User.Contains(item.AuthorizedUserId))
                        {
                            db.ProjectUserAuthorize.Remove(item);
                            db.SaveChanges();
                        }
                    }
                    return RedirectToAction("MessageShow", "Home", new { messageBody = "Projeyi Görmeye Yetkili Kullanıcılar Düzenlenmiştir.", returnUrl = "/Project/WaitingForApprove" });
                }
                else
                {
                    return RedirectToAction("MessageShow", "Home", new { messageBody = "Projeyi Görme Yetkisini Kendinizden Alamazsınız!", returnUrl = Request.UrlReferrer.AbsoluteUri });
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("MessageShow", "Home", new { messageBody = "Hata Oluştu. Lütfen Tekrar Deneyin.", returnUrl = Request.UrlReferrer.AbsoluteUri });
            }
        }
        #endregion
    }
}