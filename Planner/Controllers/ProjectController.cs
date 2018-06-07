using Planner.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using Planner.Enums;
using Planner.Helpers;
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

                    foreach (var item in db.UserProject)
                    {
                        int upId = item.Id;
                        upModel = db.UserProject.First(z => z.Id == upId);
                        pModel = db.Projects.First(z => z.Id == upModel.ProjectId);
                        userModel = db.User.First(z => z.Id == upModel.UserId);

                        vwLstUp.Add(new vwUsersProjects { UserId = userModel.Id, UserName = userModel.Name, UserSurname = userModel.Surname, UserEMail = userModel.EMail, UserCitizenshipNo = userModel.CitizenshipNo, ProjectId = pModel.Id, ProjectName = pModel.Name, FileName = pModel.FileName, IsApproved = pModel.IsApproved, IsSupported = pModel.IsSupported, ProjectDescription = pModel.Description, SupportRequest = pModel.SupportRequest });

                    }
                    vwLstUp.ToList();
                    return View(vwLstUp);
                }
                catch (Exception ex)
                {
                    List<vwUsersProjects> vwLstUp = new List<vwUsersProjects>();
                    vwLstUp.ToList();
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

                foreach (var item in db.UserProject)
                {
                    int upId = item.Id;
                    upModel = db.UserProject.First(z => z.Id == upId);
                    pModel = db.Projects.First(z => z.Id == upModel.ProjectId);
                    userModel = db.User.First(z => z.Id == upModel.UserId);
                    if (pModel.IsApproved == Convert.ToInt32(ProjectApproveEnum.Approved) && pModel.IsSupported != Convert.ToInt32(ProjectSupportEnum.Supported))
                    {
                        vwLstUp.Add(new vwUsersProjects { UserId = userModel.Id, UserName = userModel.Name, UserSurname = userModel.Surname, UserEMail = userModel.EMail, UserCitizenshipNo = userModel.CitizenshipNo, ProjectId = pModel.Id, ProjectName = pModel.Name, FileName = pModel.FileName, IsApproved = pModel.IsApproved, IsSupported = pModel.IsSupported, ProjectDescription = pModel.Description, SupportRequest = pModel.SupportRequest });
                    }
                }
                vwLstUp.ToList();
                return View(vwLstUp);
            }
            catch (Exception ex)
            {
                List<vwUsersProjects> vwLstUp = new List<vwUsersProjects>();
                vwLstUp.ToList();
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

                foreach (var item in db.UserProject)
                {
                    int upId = item.Id;
                    upModel = db.UserProject.First(z => z.Id == upId);
                    pModel = db.Projects.First(z => z.Id == upModel.ProjectId);
                    userModel = db.User.First(z => z.Id == upModel.UserId);
                    if (pModel.IsSupported == Convert.ToInt32(ProjectSupportEnum.Supported) && pModel.IsApproved == Convert.ToInt32(ProjectApproveEnum.Approved))
                    {
                        vwLstUp.Add(new vwUsersProjects { UserId = userModel.Id, UserName = userModel.Name, UserSurname = userModel.Surname, UserEMail = userModel.EMail, UserCitizenshipNo = userModel.CitizenshipNo, ProjectId = pModel.Id, ProjectName = pModel.Name, FileName = pModel.FileName, IsApproved = pModel.IsApproved, IsSupported = pModel.IsSupported, ProjectDescription = pModel.Description, SupportRequest = pModel.SupportRequest });
                    }
                }
                vwLstUp.ToList();
                return View(vwLstUp);
            }
            catch (Exception ex)
            {
                List<vwUsersProjects> vwLstUp = new List<vwUsersProjects>();
                vwLstUp.ToList();
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

                    foreach (var item in db.UserProject)
                    {
                        int upId = item.Id;
                        upModel = db.UserProject.First(z => z.Id == upId);
                        pModel = db.Projects.First(z => z.Id == upModel.ProjectId);
                        userModel = db.User.First(z => z.Id == upModel.UserId);
                        if (pModel.IsApproved == Convert.ToInt32(ProjectApproveEnum.WaitingToApprove))
                        {
                            vwLstUp.Add(new vwUsersProjects { UserId = userModel.Id, UserName = userModel.Name, UserSurname = userModel.Surname, UserEMail = userModel.EMail, UserCitizenshipNo = userModel.CitizenshipNo, ProjectId = pModel.Id, ProjectName = pModel.Name, FileName = pModel.FileName, IsApproved = pModel.IsApproved, IsSupported = pModel.IsSupported, ProjectDescription = pModel.Description, SupportRequest = pModel.SupportRequest });
                        }
                    }
                    vwLstUp.ToList();
                    return View(vwLstUp);
                }
                catch (Exception ex)
                {
                    List<vwUsersProjects> vwLstUp = new List<vwUsersProjects>();
                    vwLstUp.ToList();
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

                foreach (var item in db.UserProject)
                {
                    int upId = item.Id;
                    upModel = db.UserProject.First(z => z.Id == upId);
                    pModel = db.Projects.First(z => z.Id == upModel.ProjectId);
                    userModel = db.User.First(z => z.Id == upModel.UserId);
                    if (pModel.IsSupported == Convert.ToInt32(ProjectSupportEnum.WaitingToSupport))
                    {
                        vwLstUp.Add(new vwUsersProjects { UserId = userModel.Id, UserName = userModel.Name, UserSurname = userModel.Surname, UserEMail = userModel.EMail, UserCitizenshipNo = userModel.CitizenshipNo, ProjectId = pModel.Id, ProjectName = pModel.Name, FileName = pModel.FileName, IsApproved = pModel.IsApproved, IsSupported = pModel.IsSupported, ProjectDescription = pModel.Description, SupportRequest = pModel.SupportRequest });
                    }
                }
                vwLstUp.ToList();
                return View(vwLstUp);
            }
            catch (Exception ex)
            {
                List<vwUsersProjects> vwLstUp = new List<vwUsersProjects>();
                vwLstUp.ToList();
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
                HomeController.SendEMail(umodel.EMail, "Projeniz Onaylanmıştır. Destek başvurusu yapmak için sisteme giriş yapınız.");
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
                HomeController.SendEMail(umodel.EMail, "Projeniz Onaylanmamıştır.");
                return RedirectToAction("MessageShow", "Home", new { messageBody = "Proje Reddedilmiştir", returnUrl = Request.UrlReferrer.AbsoluteUri });
            }
            catch (Exception ex)
            {
                return RedirectToAction("MessageShow", "Home", new { messageBody = "Proje Reddedilirken Bir Hata Oluştu", returnUrl = Request.UrlReferrer.AbsoluteUri });
            }
        }

        public ActionResult SupportProject(string Id)
        {
            try
            {
                Project pmodel = new Project();
                UserProject upmodel = new UserProject();
                pmodel = db.Projects.FirstOrDefault(m => m.Id == Id);
                upmodel = db.UserProject.FirstOrDefault(m => m.ProjectId == Id);
                if (pmodel.IsApproved == Convert.ToInt32(ProjectApproveEnum.Approved))
                {
                    pmodel.IsSupported = Convert.ToInt32(ProjectSupportEnum.Supported);
                    upmodel.IsSupported = Convert.ToInt32(ProjectSupportEnum.Supported);
                    pmodel.IsApproveChanged = Convert.ToInt32(ProjectTypeChangeEnum.ChangeAsSupported);
                    upmodel.IsApproveChanged = Convert.ToInt32(ProjectTypeChangeEnum.ChangeAsSupported);
                    pmodel.LastEditDate = DateTime.Now;
                    pmodel.LastEditBy = Convert.ToString(Session["UserId"]);
                    db.SaveChanges();
                    User umodel = new User();
                    umodel = db.User.FirstOrDefault(m => m.Id == upmodel.UserId);
                    HomeController.SendEMail(umodel.EMail, "Projeniz Destek Onayından Geçmiştir.");
                    return RedirectToAction("MessageShow", "Home", new { messageBody = "Proje Desteklenmiştir", returnUrl = Request.UrlReferrer.AbsoluteUri });
                }
                else
                {
                    return RedirectToAction("MessageShow", "Home", new { messageBody = "Desteklenecek Proje Bulunmamaktadır.", returnUrl = Request.UrlReferrer.AbsoluteUri });
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("MessageShow", "Home", new { messageBody = "Proje Desteklenirken Bir Hata Oluştu", returnUrl = Request.UrlReferrer.AbsoluteUri });
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
                HomeController.SendEMail(umodel.EMail, "Projeniz Destek Sürecinde Onaylanmamıştır.");
                return RedirectToAction("MessageShow", "Home", new { messageBody = "Proje Desteği Reddedilmiştir", returnUrl = Request.UrlReferrer.AbsoluteUri });
            }
            catch (Exception ex)
            {
                return RedirectToAction("MessageShow", "Home", new { messageBody = "Proje Desteği Reddedilirken Bir Hata Oluştu", returnUrl = Request.UrlReferrer.AbsoluteUri });
            }
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

        public ActionResult SupportToProject(string Id, string supportRequirements, string supportValue)
        {
            try
            {
                UserProject upModel = db.UserProject.FirstOrDefault(a => a.ProjectId == Id);
                User uModel = db.User.FirstOrDefault(a => a.Id == upModel.UserId);
                SupportedProjects sProjects = new SupportedProjects();

                sProjects.SupportDate = DateTime.Now;
                sProjects.SupportedProject = Id;
                sProjects.Supporter = Session["UserId"].ToString();
                sProjects.SupportRequirements = supportRequirements;
                sProjects.SupportValue = supportValue;
                db.SupportedProjects.Add(sProjects);
                db.SaveChanges();
                HomeController.SendEMail(uModel.EMail, "Projenize Destek Verilmiştir. Detaylı Bilgi İçin Sisteminizi Kontrol Ediniz.");
                return RedirectToAction("MessageShow", "Home", new { messageBody = "Destek Bilgileriniz İletilmiştir. Sorumlu Kişiler Bilgilendirilecektir.", returnUrl = Request.UrlReferrer.AbsoluteUri });
            }
            catch (Exception ex)
            {
                return RedirectToAction("MessageShow", "Home", new { messageBody = "Hata Oluştu. Lütfen Tekrar Deneyin.", returnUrl = Request.UrlReferrer.AbsoluteUri });
            }
        }
        #endregion

        #region Edit Project
        public ActionResult Edit(int? id)
        {
            Project project = db.Projects.Find(id);
            //vwUsersProjects vwUp = db.vwUsersProjects.Find(id);

            if (id == null || project == null)
            {
                return View("UserMenu");
            }

            //if (vwUp.UserId != Convert.ToInt32(Session["UserId"]))
            //{
            //    return RedirectToAction("UserMenu");
            //}

            else
            {
                return View(project);
            }
        }

        [HttpPost]
        public ActionResult EditProject(HttpPostedFileBase fileProject2, Project project)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string sessionCitizenship = Convert.ToString(Session["UserCitizenshipNo"]);
                    //For Project
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
                ViewBag.Message = "Proje Başarıyla Güncellendi";
                return View(project);
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Güncelleme Sırasında Bir Hata Oluştu";
                return View();
            }
        }
        #endregion
    }
}