using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Planner.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.ReturnUrl = Request.UrlReferrer.AbsoluteUri;
            return View();
        }

        public ActionResult Error400()
        {
            ViewBag.ReturnUrl = Request.UrlReferrer.AbsoluteUri;
            return View();
        }

        public ActionResult Error403()
        {
            ViewBag.ReturnUrl = Request.UrlReferrer.AbsoluteUri;
            return View();
        }

        public ActionResult Error500()
        {
            ViewBag.ReturnUrl = Request.UrlReferrer.AbsoluteUri;
            return View();
        }

        public ActionResult Error503()
        {
            ViewBag.ReturnUrl = Request.UrlReferrer.AbsoluteUri;
            return View();
        }
    }
}