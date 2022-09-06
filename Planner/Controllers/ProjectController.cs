using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using Planner.Enums;
using Planner.Helpers;
using System.Net.Mime;
using Planner.DataAccess;
using Planner.ViewModels;

namespace Planner.Controllers
{
    [UserAuthorize]
    public class ProjectController : Controller
    {
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
                        var authorize = BasicRepository<ProjectUserAuthorize>.FirstOrDefault("WHERE ProjectId = @0", item.ProjectId);
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
                                Description = pModel.Description,
                                FileName = pModel.FileName,
                                CreationDate = pModel.CreationDate,
                                IsApproved = pModel.IsApproved,
                                IsSupported = pModel.IsSupported
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
                                    Description = pModel.Description,
                                    FileName = pModel.FileName,
                                    CreationDate = pModel.CreationDate,
                                    IsApproved = pModel.IsApproved,
                                    IsSupported = pModel.IsSupported
                                });
                            }
                        }
                    }
                    return View(vwLstUp);
                }
                catch (Exception ex)
                {
                    List<vwUsersProject> vwLstUp = new List<vwUsersProject>();
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
                Users userModel = new Users();
                List<Users> lstUser = new List<Users>();

                Project pModel = new Project();
                List<Project> lstProject = new List<Project>();

                UserProject upModel = new UserProject();
                List<vwUsersProject> vwLstUp = new List<vwUsersProject>();

                bool userAuthorized = true;

                foreach (var item in BasicRepository<UserProject>.Fetch())
                {
                    if (item.IsApproved == Convert.ToInt32(ProjectApproveEnum.Approved) &&
                        item.IsSupported == Convert.ToInt32(ProjectSupportEnum.NotSupported))
                    {
                        string _tempUserId = Session["UserId"].ToString();
                        var authorize = BasicRepository<ProjectUserAuthorize>.FirstOrDefault("WHERE ProjectId = @0", item.ProjectId);
                        if (authorize == null)
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
                                Description = pModel.Description,
                                FileName = pModel.FileName,
                                CreationDate = pModel.CreationDate,
                                IsApproved = pModel.IsApproved,
                                IsSupported = pModel.IsSupported
                            });
                        }
                        else
                        {
                            authorize = BasicRepository<ProjectUserAuthorize>.FirstOrDefault("WHERE AuthorizedUserId = @0 AND ProjectId = @1", _tempUserId, item.ProjectId);
                            if (authorize == null)
                                userAuthorized = false;
                            else if (authorize != null)
                                userAuthorized = true;

                            if (userAuthorized)
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
                                    Description = pModel.Description,
                                    FileName = pModel.FileName,
                                    CreationDate = pModel.CreationDate,
                                    IsApproved = pModel.IsApproved,
                                    IsSupported = pModel.IsSupported
                                });
                            }
                        }
                    }
                }
                return View(vwLstUp);
            }
            catch (Exception ex)
            {
                List<vwUsersProject> vwLstUp = new List<vwUsersProject>();
                return View(vwLstUp);
            }
        }

        public ActionResult SupportedProjects()
        {
            try
            {
                Users userModel = new Users();
                List<Users> lstUser = new List<Users>();

                Project pModel = new Project();
                List<Project> lstProject = new List<Project>();

                UserProject upModel = new UserProject();
                List<vwUsersProject> vwLstUp = new List<vwUsersProject>();

                bool userAuthorized = true;

                foreach (var item in BasicRepository<UserProject>.Fetch())
                {
                    if (item.IsSupported == Convert.ToInt32(ProjectSupportEnum.Supported) &&
                        item.IsApproved == Convert.ToInt32(ProjectApproveEnum.Approved) &&
                        item.IsSupported != Convert.ToInt32(ProjectSupportEnum.Closed))
                    {
                        string _tempUserId = Session["UserId"].ToString();
                        var authorize = BasicRepository<ProjectUserAuthorize>.FirstOrDefault("WHERE ProjectId = @0", item.ProjectId);
                        if (authorize == null)
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
                                Description = pModel.Description,
                                FileName = pModel.FileName,
                                CreationDate = pModel.CreationDate,
                                IsApproved = pModel.IsApproved,
                                IsSupported = pModel.IsSupported
                            });
                        }
                        else
                        {
                            authorize = BasicRepository<ProjectUserAuthorize>.FirstOrDefault("WHERE AuthorizedUserId = @0 AND ProjectId = @1", _tempUserId, item.ProjectId);
                            if (authorize == null)
                                userAuthorized = false;
                            else if (authorize != null)
                                userAuthorized = true;

                            if (userAuthorized)
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
                                    Description = pModel.Description,
                                    FileName = pModel.FileName,
                                    CreationDate = pModel.CreationDate,
                                    IsApproved = pModel.IsApproved,
                                    IsSupported = pModel.IsSupported
                                });
                            }
                        }
                    }
                }
                return View(vwLstUp);
            }
            catch (Exception ex)
            {
                List<vwUsersProject> vwLstUp = new List<vwUsersProject>();
                return View(vwLstUp);
            }
        }

        public ActionResult WaitingToApprove()
        {
            if (Convert.ToBoolean(Session["UserIsAdmin"]) != false)
            {
                try
                {
                    Users userModel = new Users();
                    List<Users> lstUser = new List<Users>();

                    Project pModel = new Project();
                    List<Project> lstProject = new List<Project>();

                    UserProject upModel = new UserProject();
                    List<vwUsersProject> vwLstUp = new List<vwUsersProject>();

                    bool userAuthorized = true;

                    foreach (var item in BasicRepository<UserProject>.Fetch())
                    {
                        if (item.IsApproved == Convert.ToInt32(ProjectApproveEnum.WaitingToApprove))
                        {
                            string _tempUserId = Session["UserId"].ToString();
                            var authorize = BasicRepository<ProjectUserAuthorize>.FirstOrDefault("WHERE ProjectId = @0", item.ProjectId);
                            if (authorize == null)
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
                                    Description = pModel.Description,
                                    FileName = pModel.FileName,
                                    CreationDate = pModel.CreationDate,
                                    IsApproved = pModel.IsApproved,
                                    IsSupported = pModel.IsSupported
                                });
                            }
                            else
                            {
                                authorize = BasicRepository<ProjectUserAuthorize>.FirstOrDefault("WHERE AuthorizedUserId = @0 AND ProjectId = @1", _tempUserId, item.ProjectId);
                                if (authorize == null)
                                    userAuthorized = false;
                                else if (authorize != null)
                                    userAuthorized = true;
                                if (userAuthorized)
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
                                        Description = pModel.Description,
                                        FileName = pModel.FileName,
                                        CreationDate = pModel.CreationDate,
                                        IsApproved = pModel.IsApproved,
                                        IsSupported = pModel.IsSupported
                                    });
                                }
                            }
                        }
                    }
                    return View(vwLstUp);
                }
                catch (Exception ex)
                {
                    List<vwUsersProject> vwLstUp = new List<vwUsersProject>();
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
                Users userModel = new Users();
                List<Users> lstUser = new List<Users>();

                Project pModel = new Project();
                List<Project> lstProject = new List<Project>();

                UserProject upModel = new UserProject();
                List<vwUsersProject> vwLstUp = new List<vwUsersProject>();

                bool userAuthorized = true;

                foreach (var item in BasicRepository<UserProject>.Fetch())
                {
                    if (item.IsSupported == Convert.ToInt32(ProjectSupportEnum.WaitingToSupport) &&
                        item.IsSupported != Convert.ToInt32(ProjectSupportEnum.Closed))
                    {
                        string _tempUserId = Session["UserId"].ToString();
                        // Eğer daha önce desteklemişse burada göremez.
                        if (BasicRepository<SupportedProject>.FirstOrDefault("WHERE SupporterUserId = @0", _tempUserId) == null)
                        {
                            var authorize = BasicRepository<ProjectUserAuthorize>.FirstOrDefault("WHERE ProjectId = @0", item.ProjectId);
                            if (authorize == null)
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
                                    Description = pModel.Description,
                                    FileName = pModel.FileName,
                                    CreationDate = pModel.CreationDate,
                                    IsApproved = pModel.IsApproved,
                                    IsSupported = pModel.IsSupported,
                                    SupportRequest = pModel.SupportRequest
                                });
                            }
                            else
                            {
                                authorize = BasicRepository<ProjectUserAuthorize>.FirstOrDefault("WHERE AuthorizedUserId = @0 AND ProjectId = @1", _tempUserId, item.ProjectId);
                                if (authorize == null)
                                    userAuthorized = false;
                                else if (authorize != null)
                                    userAuthorized = true;

                                if (userAuthorized)
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
                                        Description = pModel.Description,
                                        FileName = pModel.FileName,
                                        CreationDate = pModel.CreationDate,
                                        IsApproved = pModel.IsApproved,
                                        IsSupported = pModel.IsSupported,
                                        SupportRequest = pModel.SupportRequest
                                    });
                                }
                            }
                        }
                    }
                }
                return View(vwLstUp);
            }
            catch (Exception ex)
            {
                List<vwUsersProject> vwLstUp = new List<vwUsersProject>();
                return View(vwLstUp);
            }
        }

        public ActionResult ClosedProjects()
        {
            try
            {
                Users userModel = new Users();
                List<Users> lstUser = new List<Users>();

                Project pModel = new Project();
                List<Project> lstProject = new List<Project>();

                UserProject upModel = new UserProject();
                List<vwUsersProject> vwLstUp = new List<vwUsersProject>();

                bool userAuthorized = true;

                foreach (var item in BasicRepository<UserProject>.Fetch())
                {
                    if (item.IsSupported == Convert.ToInt32(ProjectSupportEnum.Closed) &&
                        item.IsApproved == Convert.ToInt32(ProjectApproveEnum.Approved))
                    {
                        string _tempUserId = Session["UserId"].ToString();
                        var authorize = BasicRepository<ProjectUserAuthorize>.FirstOrDefault("WHERE ProjectId = @0", item.ProjectId);
                        if (authorize == null)
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
                                Description = pModel.Description,
                                FileName = pModel.FileName,
                                CreationDate = pModel.CreationDate,
                                IsApproved = pModel.IsApproved,
                                IsSupported = pModel.IsSupported
                            });
                        }
                        else
                        {
                            authorize = BasicRepository<ProjectUserAuthorize>.FirstOrDefault("WHERE AuthorizedUserId = @0 AND ProjectId = @1", _tempUserId, item.ProjectId);
                            if (authorize == null)
                                userAuthorized = false;
                            else if (authorize != null)
                                userAuthorized = true;

                            if (userAuthorized)
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
                                    Description = pModel.Description,
                                    FileName = pModel.FileName,
                                    CreationDate = pModel.CreationDate,
                                    IsApproved = pModel.IsApproved,
                                    IsSupported = pModel.IsSupported
                                });
                            }
                        }
                    }
                }
                return View(vwLstUp);
            }
            catch (Exception ex)
            {
                List<vwUsersProject> vwLstUp = new List<vwUsersProject>();
                return View(vwLstUp);
            }
        }

        public ActionResult SupportedProjectsByMe()
        {
            try
            {
                string _tempUserId = Session["UserId"].ToString();
                var supportedProjects = BasicRepository<vwUsersProject>.Fetch("WHERE SupporterUserId = @0", _tempUserId);

                Users userModel = new Users();
                Project project = new Project();

                UserProject userProject = new UserProject();
                List<vwUsersProject> vwLstUp = new List<vwUsersProject>();

                bool userAuthorized = true;

                foreach (var item in supportedProjects)
                {
                    var authorize = BasicRepository<ProjectUserAuthorize>.FirstOrDefault("WHERE ProjectId = @0", item.ProjectId);
                    if (authorize == null)
                    {
                        userProject = BasicRepository<UserProject>.FirstOrDefault("WHERE ProjectId = @0", item.ProjectId);
                        project = BasicRepository<Project>.FirstOrDefault("WHERE Id = @0", userProject.ProjectId);
                        userModel = BasicRepository<Users>.FirstOrDefault("WHERE Id = @0", userProject.UserId);
                        vwLstUp.Add(new vwUsersProject
                        {
                            Id = userModel.Id,
                            Name = userModel.Name,
                            Surname = userModel.Surname,
                            EMail = userModel.EMail,
                            CitizenshipNo = userModel.CitizenshipNo,
                            ProjectId = project.Id,
                            ProjectName = project.Name,
                            Description = project.Description,
                            FileName = project.FileName,
                            CreationDate = project.CreationDate,
                            IsApproved = project.IsApproved,
                            IsSupported = project.IsSupported,
                            SupportDate = item.SupportDate,
                            SupportRequest = item.SupportRequest,
                            SupportRequirements = item.SupportRequirements,
                            SupportValue = item.SupportValue
                        });
                    }
                    else
                    {
                        authorize = BasicRepository<ProjectUserAuthorize>.FirstOrDefault("WHERE AuthorizedUserId = @0 AND ProjectId = @1", _tempUserId, item.ProjectId);
                        if (authorize == null)
                            userAuthorized = false;
                        else if (authorize != null)
                            userAuthorized = true;

                        if (userAuthorized)
                        {
                            userProject = BasicRepository<UserProject>.FirstOrDefault("WHERE ProjectId = @0", item.ProjectId);
                            project = BasicRepository<Project>.FirstOrDefault("WHERE Id = @0", userProject.ProjectId);
                            userModel = BasicRepository<Users>.FirstOrDefault("WHERE Id = @0", userProject.UserId);
                            vwLstUp.Add(new vwUsersProject
                            {
                                Id = userModel.Id,
                                Name = userModel.Name,
                                Surname = userModel.Surname,
                                EMail = userModel.EMail,
                                CitizenshipNo = userModel.CitizenshipNo,
                                ProjectId = project.Id,
                                ProjectName = project.Name,
                                Description = project.Description,
                                FileName = project.FileName,
                                CreationDate = project.CreationDate,
                                IsApproved = project.IsApproved,
                                IsSupported = project.IsSupported
                            });
                        }
                    }
                }
                return View(vwLstUp);
            }
            catch (Exception ex)
            {
                List<vwUsersProject> vwLstUp = new List<vwUsersProject>();
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
                pmodel = BasicRepository<Project>.FirstOrDefault("WHERE Id = @0", Id);
                upmodel = BasicRepository<UserProject>.FirstOrDefault("WHERE ProjectId = @0", Id);
                pmodel.IsApproved = Convert.ToInt32(ProjectApproveEnum.Approved);
                upmodel.IsApproved = Convert.ToInt32(ProjectApproveEnum.Approved);
                pmodel.IsApproveChanged = Convert.ToInt32(ProjectTypeChangeEnum.ChangedAsApproved);
                upmodel.IsApproveChanged = Convert.ToInt32(ProjectTypeChangeEnum.ChangedAsApproved);
                pmodel.LastEditDate = DateTime.Now;
                pmodel.LastEditBy = Convert.ToString(Session["UserId"]);
                BasicRepository<Project>.Update(pmodel);
                BasicRepository<UserProject>.Update(upmodel);
                var umodel = BasicRepository<Users>.FirstOrDefault("WHERE Id = @0", upmodel.UserId);
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

        public ActionResult RejectProject(string Id, string declineCause)
        {
            try
            {
                Project pmodel = new Project();
                UserProject upmodel = new UserProject();
                RejectedProject rProjects = new RejectedProject();

                pmodel = BasicRepository<Project>.FirstOrDefault("WHERE Id = @0", Id);
                upmodel = BasicRepository<UserProject>.FirstOrDefault("WHERE ProjectId = @0", Id);
                pmodel.IsApproved = Convert.ToInt32(ProjectApproveEnum.NotApproved);
                upmodel.IsApproved = Convert.ToInt32(ProjectApproveEnum.NotApproved);
                pmodel.IsApproveChanged = Convert.ToInt32(ProjectTypeChangeEnum.ChangedAsNotApproved);
                upmodel.IsApproveChanged = Convert.ToInt32(ProjectTypeChangeEnum.ChangedAsNotApproved);
                pmodel.LastEditDate = DateTime.Now;
                pmodel.LastEditBy = Convert.ToString(Session["UserId"]);

                rProjects.RejectedProjectId = Id;
                rProjects.RejectUserId = Convert.ToString(Session["UserId"]);
                rProjects.RejectCause = declineCause;
                rProjects.RejectDate = DateTime.Now;

                BasicRepository<RejectedProject>.Insert(rProjects);
                BasicRepository<Project>.Update(pmodel);
                BasicRepository<UserProject>.Update(upmodel);

                var umodel = BasicRepository<Users>.FirstOrDefault("WHERE Id = @0", upmodel.UserId);
                try
                {
                    HomeController.SendEMail(umodel.EMail, "Projeniz Onaylanmamıştır. Reddedilme Sebebi: '" + declineCause + "'", "Proje Onayı");
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
                var upModel = BasicRepository<UserProject>.FirstOrDefault("WHERE ProjectId = @0", Id);
                var uModel = BasicRepository<Users>.FirstOrDefault("WHERE Id = @0", upModel.UserId);

                var pModel = BasicRepository<Project>.FirstOrDefault("WHERE Id = @0", upModel.ProjectId);
                SupportedProject sProjects = new SupportedProject();

                if (pModel.IsApproved == Convert.ToInt32(ProjectApproveEnum.Approved))
                {
                    pModel.IsApproveChanged = Convert.ToInt32(ProjectTypeChangeEnum.ChangedAsSupported);
                    upModel.IsApproveChanged = Convert.ToInt32(ProjectTypeChangeEnum.ChangedAsSupported);
                    pModel.LastEditDate = DateTime.Now;
                    pModel.LastEditBy = Convert.ToString(Session["UserId"]);

                    sProjects.SupportDate = DateTime.Now;
                    sProjects.SupportedProjectId = Id;
                    sProjects.SupporterUserId = Session["UserId"].ToString();
                    sProjects.SupportRequirements = supportRequirements;
                    sProjects.SupportValue = supportValue;

                    BasicRepository<SupportedProject>.Insert(sProjects);
                    BasicRepository<Project>.Update(pModel);
                    BasicRepository<UserProject>.Update(upModel);

                    try
                    {
                        HomeController.SendEMail(uModel.EMail, "Projenize Destek Verilmiştir. Detaylı Bilgi İçin Sisteminizi Kontrol Ediniz.", "Proje Desteği");
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction("MessageShow", "Home", new { messageBody = "Destek Bilgileriniz İletilmiştir", returnUrl = Request.UrlReferrer.AbsoluteUri });
                    }
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
                var pmodel = BasicRepository<Project>.FirstOrDefault("WHERE Id = @0", Id);
                var upmodel = BasicRepository<UserProject>.FirstOrDefault("WHERE ProjectId = @0", Id);
                pmodel.IsApproved = Convert.ToInt32(ProjectSupportEnum.NotSupported);
                upmodel.IsApproved = Convert.ToInt32(ProjectSupportEnum.NotSupported);
                pmodel.IsApproveChanged = Convert.ToInt32(ProjectTypeChangeEnum.ChangedAsNotSupported);
                upmodel.IsApproveChanged = Convert.ToInt32(ProjectTypeChangeEnum.ChangedAsNotSupported);
                pmodel.LastEditDate = DateTime.Now;
                pmodel.LastEditBy = Convert.ToString(Session["UserId"]);
                BasicRepository<Project>.Update(pmodel);
                BasicRepository<UserProject>.Update(upmodel);

                var umodel = BasicRepository<Users>.FirstOrDefault("WHERE Id = @0", upmodel.UserId);
                try
                {
                    HomeController.SendEMail(umodel.EMail, "Projeniz Destek Sürecinde Onaylanmamıştır.", "Proje Desteği");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("MessageShow", "Home", new { messageBody = "Proje Desteği Reddedilmiştir Fakat Kullanıcıya E-Posta Gönderiminde Hata Oluşmuştur", returnUrl = Request.UrlReferrer.AbsoluteUri });
                }
                return RedirectToAction("MessageShow", "Home", new { messageBody = "Proje Desteği Reddedilmiştir", returnUrl = Request.UrlReferrer.AbsoluteUri });
            }
            catch (Exception ex)
            {
                return RedirectToAction("MessageShow", "Home", new { messageBody = "Proje Desteği Reddedilirken Bir Hata Oluştu", returnUrl = Request.UrlReferrer.AbsoluteUri });
            }
        }

        public ActionResult CloseSupportedProject(string Id)
        {
            var project = BasicRepository<Project>.FirstOrDefault("WHERE Id = @0", Id);
            var userProject = BasicRepository<UserProject>.FirstOrDefault("WHERE ProjectId = @0", Id);
            var user = BasicRepository<Users>.FirstOrDefault("WHERE Id = @0", userProject.UserId);

            project.IsSupported = Convert.ToInt32(ProjectSupportEnum.Closed);
            userProject.IsSupported = Convert.ToInt32(ProjectSupportEnum.Closed);

            project.IsApproveChanged = Convert.ToInt32(ProjectTypeChangeEnum.ChangedAsClosed);
            userProject.IsApproveChanged = Convert.ToInt32(ProjectTypeChangeEnum.ChangedAsClosed);

            BasicRepository<Project>.Update(project);
            BasicRepository<UserProject>.Update(userProject);

            try
            {
                HomeController.SendEMail(user.EMail, "Projenizin Destek Başvurusu Kapatılmıştır.", "Proje Desteği");
            }
            catch (Exception ex)
            {
                return RedirectToAction("MessageShow", "Home", new { messageBody = "Proje Desteği Kapatılmıştır Fakat Kullanıcıya E-Posta Gönderiminde Hata Oluşmuştur", returnUrl = Request.UrlReferrer.AbsoluteUri });
            }
            return RedirectToAction("MessageShow", "Home", new { messageBody = "Proje Desteği Kapatılmıştır", returnUrl = Request.UrlReferrer.AbsoluteUri });
        }
        #endregion

        public virtual ActionResult Download(string Id)
        {
            try
            {
                var pmodel = BasicRepository<Project>.FirstOrDefault("WHERE Id = @0", Id);
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
                return RedirectToAction("MessageShow", "Home", new { messageBody = "Projeniz Yüklenmiştir. Lütfen Projenizi Kontrol Ettikten Sonra Onaya Gönderiniz.", returnUrl = "/User/UserMenu" });
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
                BasicRepository<Project>.Insert(project);

                up.ProjectId = project.Id;
                up.UserId = Convert.ToString(Session["UserId"]);
                up.IsApproved = Convert.ToInt32(ProjectApproveEnum.NotApproved);
                up.IsSupported = Convert.ToInt32(ProjectSupportEnum.NotSupported);
                up.IsApproveChanged = Convert.ToInt32(ProjectTypeChangeEnum.NotChanged);
                BasicRepository<UserProject>.Insert(up);
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
                var projects = BasicRepository<Project>.FirstOrDefault("WHERE Id = @0", Id);
                var ups = BasicRepository<UserProject>.FirstOrDefault("WHERE ProjectId = @0", Id);
                ups.IsApproved = Convert.ToInt32(ProjectApproveEnum.WaitingToApprove);
                projects.IsApproved = Convert.ToInt32(ProjectApproveEnum.WaitingToApprove);
                projects.LastEditDate = DateTime.Now;
                projects.LastEditBy = Convert.ToString(Session["UserId"]);
                BasicRepository<Project>.Update(projects);
                BasicRepository<UserProject>.Update(ups);
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
                var projects = BasicRepository<Project>.FirstOrDefault("WHERE Id = @0", Id);
                var ups = BasicRepository<UserProject>.FirstOrDefault("WHERE ProjectId = @0", Id);
                if (projects.IsApproved == Convert.ToInt32(ProjectApproveEnum.Approved))
                {
                    ups.SupportRequest = supportRequest;
                    ups.IsSupported = Convert.ToInt32(ProjectSupportEnum.WaitingToSupport);
                    projects.SupportRequest = supportRequest;
                    projects.IsSupported = Convert.ToInt32(ProjectSupportEnum.WaitingToSupport);
                    projects.LastEditDate = DateTime.Now;
                    projects.LastEditBy = Convert.ToString(Session["UserId"]);
                    BasicRepository<Project>.Update(projects);
                    BasicRepository<UserProject>.Update(ups);
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
            var project = BasicRepository<Project>.FirstOrDefault("WHERE Id = @0", id);
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

                        var _tempProject = BasicRepository<Project>.FirstOrDefault("WHERE Id = @0", project.Id);

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

                        BasicRepository<Project>.Update(_tempProject);
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
                List<Users> User = new List<Users>();

                foreach (var item in BasicRepository<Users>.Fetch("WHERE IsActive = @0", true))
                {
                    var userAuthorize = BasicRepository<ProjectUserAuthorize>.FirstOrDefault("WHERE ProjectId = @0 AND AuthorizedUserId = @1", id, item.Id);
                    if (userAuthorize == null)
                    {
                        User.Add(new Users
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
                List<Users> lstUser = new List<Users>();
                return View(lstUser);
            }
        }

        public ActionResult ProjectAuthorizedUser(string id)
        {
            try
            {
                Session["ProjectId"] = id;
                List<vwAuthorizedUserProject> authorizes = new List<vwAuthorizedUserProject>();
                foreach (var item in BasicRepository<Users>.Fetch())
                {
                    var userAuthorize = BasicRepository<ProjectUserAuthorize>.FirstOrDefault("WHERE ProjectId = @0 AND AuthorizedUserId = @1", id, item.Id);
                    if (userAuthorize != null)
                    {
                        authorizes.Add(new vwAuthorizedUserProject
                        {
                            ProjectId = userAuthorize.ProjectId,
                            UserId = item.Id,
                            UserName = item.Name,
                            UserSurname = item.Surname,
                            UserEMail = item.EMail,
                            UserIsAdmin = item.IsAdmin.ToString()
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
        public ActionResult AddAuthorizeToUserForProject(List<string> users)
        {
            try
            {
                string _projectId = Session["ProjectId"].ToString();
                string _sessionUserId = Session["UserId"].ToString();
                ProjectUserAuthorize userAuthorize = new ProjectUserAuthorize();
                foreach (var item in users)
                {
                    userAuthorize.AuthorizedUserId = item;
                    userAuthorize.ProjectId = _projectId;
                    userAuthorize.LastEditBy = _sessionUserId;
                    userAuthorize.LastEditDate = DateTime.Now;
                    BasicRepository<ProjectUserAuthorize>.Insert(userAuthorize);
                }
                var adminUsers = BasicRepository<Users>.Fetch("WHERE IsAdmin = @0", true);
                foreach (var item in adminUsers) //Tüm Adminleri de Yetkili Yap
                {
                    userAuthorize.AuthorizedUserId = item.Id;
                    userAuthorize.ProjectId = _projectId;
                    userAuthorize.LastEditBy = _sessionUserId;
                    userAuthorize.LastEditDate = DateTime.Now;
                    BasicRepository<ProjectUserAuthorize>.Insert(userAuthorize);
                }
                return RedirectToAction("MessageShow", "Home", new { messageBody = "Projeyi Görmeye Yetkili Kullanıcılar Ayarlanmıştır.", returnUrl = "/Project/WaitingToApprove" });
            }
            catch (Exception ex)
            {
                return RedirectToAction("MessageShow", "Home", new { messageBody = "Hata Oluştu. Lütfen Tekrar Deneyin.", returnUrl = Request.UrlReferrer.AbsoluteUri });
            }
        }

        [HttpPost]
        public ActionResult RemoveAuthorizeToUserForProject(List<string> Users)
        {
            try
            {
                if (!Users.Contains(Session["UserId"].ToString()))
                {
                    string _projectId = Session["ProjectId"].ToString();
                    var userAuthorize = BasicRepository<ProjectUserAuthorize>.Fetch("WHERE ProjectId = @0", _projectId);
                    foreach (var item in userAuthorize)
                    {
                        if (Users.Contains(item.AuthorizedUserId))
                        {
                            BasicRepository<ProjectUserAuthorize>.Delete(item);
                        }
                    }
                    return RedirectToAction("MessageShow", "Home", new { messageBody = "Projeyi Görmeye Yetkili Kullanıcılar Düzenlenmiştir.", returnUrl = "/Project/WaitingToApprove" });
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

        public ActionResult SupportList(string projectId)
        {
            var supportList = BasicRepository<SupportedProject>.Fetch("WHERE SupportedProjectId = @0", projectId);
            List<SupportListViewModel> supportListVM = new List<SupportListViewModel>();

            foreach (var item in supportList)
            {
                var supporterUser = BasicRepository<Users>.FirstOrDefault("WHERE Id = @0", item.SupporterUserId);
                supportListVM.Add(new SupportListViewModel
                {
                    Name = supporterUser.Name,
                    Surname = supporterUser.Surname,
                    SupportDate = item.SupportDate,
                    SupportRequirements = item.SupportRequirements,
                    SupportValue = item.SupportValue
                });
            }
            ViewBag.PreviousUrl = Request.UrlReferrer.AbsoluteUri;
            return View(supportListVM);
        }
    }
}