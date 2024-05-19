using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WorkZen.CustomFilters.AuthenticationFilters
{
    public class RedirectUnAuthUser: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (string.IsNullOrEmpty(Convert.ToString(filterContext.HttpContext.Session["email"])))
            {
                filterContext.Result = new RedirectResult("~/Auth/Login");
            }
            base.OnActionExecuting(filterContext);
        }
    }
}