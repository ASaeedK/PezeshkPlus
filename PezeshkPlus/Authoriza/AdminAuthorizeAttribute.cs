using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PezeshkPlus.Models;

namespace PezeshkPlus.Authoriza
{
    public class AdminAuthorizeAttribute : AuthorizeAttribute
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
                        PezeshkPlusEntities db = new PezeshkPlusEntities();
                        List<USP_SEL_NameAndRole_Result> NameAndRole = db.USP_SEL_NameAndRole(httpContext.Session[authID].ToString()).ToList();

                        if (NameAndRole[0].Role == 0)
                        {
                            List<bool?> adminType = db.USP_SEL_AdminType(httpContext.Session[authID].ToString()).ToList();
                            if (adminType[0] == false)
                                return true;
                        }
                        return false;
                    }
                    return false;
                }
                return false;
            }
            return false;
        }


        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult(@"~/Home");
        }
    }
}