using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebOA.Controllers
{
    public class BaseController : Controller
    {
        WebOA.Models.OAWebEntities db = new Models.OAWebEntities();
        // GET: Base
        protected override void OnActionExecuting (ActionExecutingContext filterContext)
        {
            if (checkLogin())// 判断是否登录
            {
                Redirect("/Home/Login/");
            }
            else
            {
                var status = db.Users.Where(x => x.NAME == User.Identity.Name).FirstOrDefault();
                if (status.STATUS==1)//判断账号是否被禁用
                {
                    Content("<script>alert('账号已被禁用，请联系管理员！');</script>");
                }
            }
            base.OnActionExecuting(filterContext);
        }

        /// <summary>
        /// 判断是否登录
        /// </summary>
        protected bool checkLogin ()
        {
            if (User.Identity.IsAuthenticated)
            {
                return false;
            }
            return true;
        }

    }
}