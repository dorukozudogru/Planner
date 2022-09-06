using Planner.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Planner
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError() is HttpException ? Server.GetLastError() as HttpException : Server.GetLastError();
            if (exception != null)
            {
                var url = System.Web.HttpContext.Current.Request.RawUrl.ToString();
                var menuAdi = url + " - " + exception.Message;
                if (exception is HttpException)
                {
                    int errorCode = (exception as HttpException).GetHttpCode();
                    if (errorCode == 400)
                    {
                        Server.ClearError();
                        Response.Redirect("~/Error/Error400");
                    }
                    if (errorCode == 403)
                    {
                        Server.ClearError();
                        Response.Redirect("~/Error/Error403");
                    }
                    if (errorCode == 404)
                    {
                        Server.ClearError();
                        Response.Redirect("~/Error/Index");
                    }
                    if (errorCode == 500)
                    {
                        Server.ClearError();
                        Response.Redirect("~/Error/Error500");
                    }
                    if (errorCode == 503)
                    {
                        Server.ClearError();
                        Response.Redirect("~/Error/Error503");
                    }
                    else
                    {
                        Server.ClearError();
                        Response.Redirect("~/Error/Index");
                    }
                }
                else
                {
                    Response.Redirect("~/Error/Index");
                }
            }

        }

        void Application_EndRequest(object sender, System.EventArgs e)
        {
            if (Response.StatusCode == 401)
            {
                var returnUrl = System.Web.HttpContext.Current.Request.RawUrl.ToString();
                Response.ClearContent();
                Response.Redirect("~/Account/Login?returnUrl=" + returnUrl);
            }
        }
    }
}
