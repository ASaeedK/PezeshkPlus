using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PezeshkPlus.Models;

namespace PezeshkPlus.Authoriza
{
    public class DoctorLoggedAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Request.Cookies["AuthID"] != null && httpContext.Session["AuthID"] != null)
            {
                if (httpContext.Request.Cookies["AuthID"].Value == httpContext.Session["AuthID"].ToString())
                {
                    string authID = httpContext.Session["AuthID"].ToString();
                    if (httpContext.Session[authID + "IP"].ToString() == httpContext.Request.UserHostAddress)
                    {
                            return false;
                    }
                    return true;
                }
                return true;
            }
            return true;
        }


        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult(@"~/Doctor/Panel");
        }
    }
}