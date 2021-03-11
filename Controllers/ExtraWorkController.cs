using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebOA.Models;

namespace WebOA.Controllers
{   /// <summary>
    /// 加班管理
    /// </summary>
    public class ExtraWorkController : BaseController
    {
        WebOA.Models.OAWebEntities db = new Models.OAWebEntities();
        // GET: ExtraWork
        [HttpGet]
        public ActionResult ExtraWorkList()
        {
            var depts = db.Departments.Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.DepNAME }).ToList();
            ViewBag.Depts = depts;
            return View();
        }
        [HttpPost]
        public ActionResult ExtraWorkList(string CHECK_FLAG, int Page, int limit)
        {
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;
            var query = db.OverTimes.Where(x => x.NAME == User.Identity.Name);
            if (!string.IsNullOrWhiteSpace(CHECK_FLAG))
            {
                query = query.Where(x => x.CHECK_FLAG.ToString() == CHECK_FLAG);
            }
            int total = query.Count();
            var list = query.OrderBy(x => x.ID).Skip((Page - 1) * limit).Take(limit).ToList();
            var str = PagerDate.PagedData<OverTime>(list, total);
            return Content(str);
        }
        /// <summary>
        /// 加班申请
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ExtraWorkAdd()
        {
            var us = db.Users.Where(x => x.NAME == User.Identity.Name).FirstOrDefault();
            var depts = db.Departments.Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.DepNAME, Selected = x.ID == (us.DEPAR_ID) }).ToList();
            ViewBag.DEPART_ID = depts;
            return View();
        }
        [HttpPost]
        public ActionResult ExtraWorkAdd(UIExtraWork model)
        {
            OverTime em = new OverTime();
            em.START_DATE = model.START_DATE;
            em.END_DATE = model.END_DATE;
            em.DEPART_ID = model.DEPART_ID;
            em.NAME = User.Identity.Name;
            em.REASONS = model.REASONS;
            db.OverTimes.Add(em);
            var i= db.SaveChanges();
            if (i>0)
            {
                return Json(new UIResult(true, "加班申请成功"));
            }
            else
            {
                return Json(new UIResult(false, "加班申请失败"));
            }
            
        }
        [HttpGet]
        public ActionResult ExtraWorkPass()
        {
            var depts = db.Departments.Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.DepNAME }).ToList();
            ViewBag.Depts = depts;
            return View();
        }
        [HttpPost]
        public ActionResult ExtraWorkPass(string NAME, string DEPART_ID, string CHECK_FLAG, int Page = 1, int PageSize = 10)
        {
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;
            var query = db.OverTimes.Where(x => x.ID>0);
            if (!string.IsNullOrEmpty(NAME))
            {
                query = db.OverTimes.Where(x => x.NAME.Contains(NAME));
            }
            if (!string.IsNullOrEmpty(DEPART_ID))
            {
                query = query.Where(x => x.DEPART_ID.ToString() == DEPART_ID);
            }
            if (!string.IsNullOrEmpty(CHECK_FLAG))
            {
                query = query.Where(x => x.CHECK_FLAG.ToString() == CHECK_FLAG);
            }
            int total = query.Count();
            var list = query.OrderBy(x => x.ID).Skip((Page - 1) * PageSize).Take(PageSize).ToList();
            var str = PagerDate.PagedData<OverTime>(list, total);
            return Content(str);
        }

        [HttpPost]
        public ActionResult Dels(int id)
        {
            try
            {
                int i = db.Database.ExecuteSqlCommand("Delete OverTime where ID =" + id + "");
                if (i <= 0)
                {
                    return Json(new UIResult(false, "删除失败"));
                }
                return Json(new UIResult(true, ""));
            }
            catch (Exception ex)
            {

                return Json(new UIResult(false, "删除失败" + ex.Message));
            }
        }

        [HttpPost]
        public ActionResult Pub(string ids)
        {
            int i = db.Database.ExecuteSqlCommand("update OverTime set CHECK_FLAG='1' where ID in(" + ids + ")");
            if (i <= 0)
            {
                return Json(new UIResult(false, "批量审核通过失败"));
            }
            return Json(new UIResult(true, ""));
        }
        [HttpPost]
        public ActionResult PubNO(string ids)
        {
            int i = db.Database.ExecuteSqlCommand("update OverTime set CHECK_FLAG='2' where ID in(" + ids + ")");
            if (i <= 0)
            {
                return Json(new UIResult(false, "批量审核未通过失败"));
            }
            return Json(new UIResult(true, ""));
        }
        [HttpPost]
        public ActionResult WorkPassYes(int id)
        {
            var Check_date = DateTime.Now.ToString();
            int i = db.Database.ExecuteSqlCommand("update OverTime set CHECK_FLAG=1,CHECK_NAME='" + User.Identity.Name + "',CHECK_DATE='" + Check_date + "' where Id =" + id + "");
            if (i <= 0)
            {
                return Json(new UIResult(false, "审核通过失败"));
            }
            return Json(new UIResult(true, ""));
        }
        [HttpPost]
        public ActionResult WorkPassNo(int id)
        {
            var Check_date = DateTime.Now.ToString();
            int i = db.Database.ExecuteSqlCommand("update OverTime set CHECK_FLAG=2,CHECK_NAME='" + User.Identity.Name + "',CHECK_DATE='" + Check_date + "' where Id =" + id + "");
            if (i <= 0)
            {
                return Json(new UIResult(false, "审核未通过失败"));
            }
            return Json(new UIResult(true, ""));
        }
    }

}