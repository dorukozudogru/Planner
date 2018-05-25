using Planner.Models;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Planner.Helpers
{
    public class UserAuthorizeAttribute : AuthorizeAttribute
    {
        public new readonly string Users;

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return UserSession.IsAuthorized();
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                    {
                        {"action","UnAuthorized"},
                        {"controller","Error"}
                    });
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }
    }
}