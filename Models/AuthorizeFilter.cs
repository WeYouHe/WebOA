using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebOA.Models;

namespace WebOA.Models
{
    public class AuthorizeFilter : AuthorizeAttribute
    {
        public override void OnAuthorization (AuthorizationContext filterContext)
        {

        }


        protected override void HandleUnauthorizedRequest (AuthorizationContext filterContext)
        {

            int i = filterContext.HttpContext.Response.StatusCode;//false返回200
                                                                  // filterContext.Result = new ViewResult { ViewName = View };
            if (filterContext.HttpContext.Response.StatusCode == 403)
            {
                filterContext.HttpContext.Response.Write("<Script lanuage='javascript'>alert('您没有此操作权限')</Script>");
                //filterContext.HttpContext.Response.End();//此方法导致整个页面空白
                filterContext.Result = new EmptyResult();//此方法导致整个页面空白

            }

        }
    }
}