﻿using System;
using System.Linq;
using System.Web.Mvc;
using Planner.Models;
using Planner.Enums;

namespace Planner.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private DBContext db = new DBContext();

        #region Hallolanlar
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string encPass = HomeController.Encrypt(model.Password);
            var loginUser = db.User.Where(a => a.EMail == model.EMail && a.Password == encPass).FirstOrDefault();
            var loginUserCV = db.UserCV.FirstOrDefault(a => a.UserId == loginUser.Id);

            if (loginUser != null)
            {
                if (loginUser.IsApproved == Convert.ToInt32(UserApproveEnum.WaitingToApprove))
                {
                    return RedirectToAction("LogInApprovement", "User");
                }
                else if (loginUser.IsApproved == Convert.ToInt32(UserApproveEnum.Blocked) || loginUser.IsApproved == Convert.ToInt32(UserApproveEnum.NotApproved))
                {
                    return RedirectToAction("Rejected", "User");
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
            if (ModelState.IsValid)
            {
                //KPS Entegrasyonu
                //int birthYear = user.BirthDate.Year;
                //KPS_Servis.KPSPublicSoapClient ws = new KPS_Servis.KPSPublicSoapClient();
                //bool? confirmCitizenshipNo = ws.TCKimlikNoDogrula(Convert.ToInt64(user.CitizenshipNo), (user.Name).ToUpper(), (user.Surname).ToUpper(), birthYear);
                //if (confirmCitizenshipNo == true)
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
                bool controlCitizenshipno = HomeController.ControlCitizenshipNo(model.CitizenshipNo);
                if (controlCitizenshipno)
                {
                    model.Password = HomeController.Encrypt(model.Password);
                    model.IsCvUploaded = false;
                    model.IsApproved = Convert.ToInt32(UserApproveEnum.WaitingToApprove);
                    model.IsAdmin = false;
                    model.RegisterDate = DateTime.Now;
                    model.LastEditDate = Convert.ToDateTime("1753-01-01");
                    model.LastEditBy = -1;
                    db.User.Add(model);
                    db.SaveChanges();
                    return PartialView("_Approvement");
                }
                else
                {
                    ViewBag.Message = "Gecersiz T.C. Kimlik Numarası";
                }
            }
            return View(model);
        }
        #endregion

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        // BURAYI YAP
        // POST: /Account/ForgotPassword
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = db.User.Where(a => a.EMail == model.Email).FirstOrDefault();
        //        if (user == null)
        //        {
        //            // Don't reveal that the user does not exist or is not confirmed
        //            return View("ForgotPasswordConfirmation");
        //        }

        //        string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id.ToString());
        //        var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
        //        await UserManager.SendEmailAsync(user.Id.ToString(), "Reset Password",
        //           "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
        //        TempData["ViewBagLink"] = callbackUrl;
        //        return RedirectToAction("ForgotPasswordConfirmation", "Account");
        //    }

        //    // If we got this far, something failed, redisplay form
        //    return View(model);
        //}

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            ViewBag.Link = TempData["ViewBagLink"];
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        ////
        //// POST: /Account/ResetPassword
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }

        //    var user = await UserManager.FindByNameAsync(model.Email);
        //    if (user == null)
        //    {
        //        // Don't reveal that the user does not exist
        //        return RedirectToAction("ResetPasswordConfirmation", "Account");
        //    }
        //    var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
        //    if (result.Succeeded)
        //    {
        //        return RedirectToAction("ResetPasswordConfirmation", "Account");
        //    }
        //    AddErrors(result);
        //    return View();
        //}

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }
    }
}