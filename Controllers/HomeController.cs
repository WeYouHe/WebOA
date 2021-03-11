using log4net;
using log4net.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebOA.Filters;
using WebOA.Models;
using WebOA;

namespace WebOA.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        WebOA.Models.OAWebEntities db = new Models.OAWebEntities();
        // GET: Home
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login ()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login (UILoginData data)
        {
            try
            {
                var UserName = db.Users.Where(x => x.NAME == data.NAME).FirstOrDefault();
                if (UserName != null)
                {
                    if (data.password == UserName.PASSWORD)
                    {
                        if (UserName.STATUS == 0)
                        {
                            FormsAuthentication.SetAuthCookie(data.NAME, false);
                            db.Database.ExecuteSqlCommand("update  Users set LOGINS=LOGINS+1 where NAME='" + data.NAME + "'");

                            return Json(new UIResult(true, "登录成功"));
                        }
                        else
                        {
                            return Json(new UIResult(false, "账号已被禁用"));
                        }

                    }
                    else
                    {
                        return Json(new UIResult(false, "密码错误"));
                    }

                }
                else
                {
                    return Json(new UIResult(false, "账号不存在"));
                }
            }
            catch (Exception ex)
            {

                return Json(new UIResult(false, ex.Message  ));
            }


        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ChangePwd ()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePwd(UIChangPwd obj)
        {

            try
            {
                if (obj.newpass != obj.repass)
                {
                    return Json(new UIResult(false, "两次输入密码不一致"));
                }
                var u = db.Users.Where(x => x.NAME.ToString() == User.Identity.Name).FirstOrDefault();
                if (u != null)
                {
                    if (u.PASSWORD == obj.oldpass)
                    {
                        u.PASSWORD = obj.newpass;
                        db.SaveChanges();
                        return Json(new UIResult(true, "修改成功"));

                    }
                    else
                    {
                        return Json(new UIResult(false, "原始密码不正确"));
                    }
                }
                else
                {
                    return Json(new UIResult(false, "此用户不存在"));
                }
            }
            catch (Exception ex)
            {

                return Json(new UIResult(false, ex.Message));
            }

            
        }
        [HttpGet]
        public ActionResult Index ()
        {
            var UserName = User.Identity.Name;
            var UserImg = db.Users.Where(x => x.NAME == UserName).FirstOrDefault();
            ViewBag.data = db.View_CRM.Where(x => x.NAME == UserName).ToList();
            return View(UserImg);
        }


        [HttpGet]
        public ActionResult WelCome ()
        {
            var UserName = User.Identity.Name;
            var UserImg = db.Users.Where(x => x.NAME == UserName).FirstOrDefault();
            var UserCnt = db.Users.Where(x => x.STATUS == 0).Count();
            var PlantCnt = db.Plants.Where(x => x.CARRY_USER == UserName).Count();
            var TimeCnt = db.OverTimes.Where(x => x.NAME == UserName).Count();
            var LeaveCnt = db.Leaves.Where(x => x.LEAVE_NAME == UserName).Count();
            ViewBag.UserCnt = UserCnt;
            ViewBag.PlantCnt = PlantCnt;
            ViewBag.TimeCnt = TimeCnt;
            ViewBag.LeaveCnt = LeaveCnt;
            return View();
        }
        [HttpPost]
        public ActionResult WelCome (int Page = 1, int PageSize = 10)
        {

            var query = db.Plants.Where(x => x.CARRY_USER == User.Identity.Name&&x.IN_FLAG==0);
            var total = query.Count();
            var list = query.OrderBy(x=>true).Skip((Page - 1) * PageSize).Take(PageSize).ToList();
            var str = PagerDate.PagedData<Plant>(list, total);
            return Content(str);
        }

        [HttpGet]
        public ActionResult SingOut ()
        {
            FormsAuthentication.SignOut();
            return Redirect("/Home/Login/");

        }
        /// <summary>
        /// 验证码生成
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public FileResult VerificationCode ()
        {

            return File(new VerificationCode().GetVerifyCode(), @"image/Gif");
        }
        /// <summary>
        /// 我的信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult MyInfo()
        {
            var quer = db.Users.Where(x => x.NAME == User.Identity.Name).FirstOrDefault();
            return View(quer);
        }
        [HttpPost]
        public ActionResult MyInfo(User model)
        {
            try
            {
                var us = db.Users.Where(x => x.NAME == User.Identity.Name).FirstOrDefault();
                us.PHONE = model.PHONE;
                us.PORTRAIT = model.PORTRAIT;
                us.ADDRESS = model.ADDRESS;
                us.EMAIL = model.EMAIL;
                us.SEX = model.SEX;
                db.SaveChanges();
                return Json(new UIResult(true, "修改成功"));
            }
            catch (Exception ex)
            {
                return Json(new UIResult(false, "修改失败" + ex));
            }
        }
    }
}