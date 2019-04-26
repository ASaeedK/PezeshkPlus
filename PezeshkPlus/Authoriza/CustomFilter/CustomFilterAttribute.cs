using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PezeshkPlus.Models;

namespace PezeshkPlus.Authoriza.CustomFilter
{
    public class CustomFilterAttribute : ActionFilterAttribute
    {
        //public bool IsLogin;
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if(filterContext.HttpContext.Request.Cookies["AuthID"] != null && filterContext.HttpContext.Session["AuthID"] != null)
            {
                if (filterContext.HttpContext.Request.Cookies["AuthID"].Value == filterContext.HttpContext.Session["AuthID"].ToString())
                {
                    string authID = filterContext.HttpContext.Session["AuthID"].ToString();
                    if (filterContext.HttpContext.Session[authID + "IP"].ToString() == filterContext.HttpContext.Request.UserHostAddress)
                    {
                        try
                        {
                            filterContext.Controller.ViewBag.IsLogin = true;

                            PezeshkPlusEntities db = new PezeshkPlusEntities();
                            List<USP_SEL_NameAndRole_Result> NameAndRole = db.USP_SEL_NameAndRole(filterContext.HttpContext.Session[authID].ToString()).ToList();

                            if (NameAndRole[0].Role == 0)
                                filterContext.Controller.ViewBag.Role = "Admin";
                            else
                                filterContext.Controller.ViewBag.Role = "Doctor";
                            filterContext.Controller.ViewBag.Name = NameAndRole[0].LastName;
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                    }
                    else
                    {
                        filterContext.Controller.ViewBag.IsLogin = false;
                    }
                }
                else
                {
                    filterContext.Controller.ViewBag.IsLogin = false;
                }
            }
            else
            {
                filterContext.Controller.ViewBag.IsLogin = false;
            }
            
            base.OnActionExecuting(filterContext);
        }
    }
}