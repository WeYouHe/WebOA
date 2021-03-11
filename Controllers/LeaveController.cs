using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebOA.Models;

namespace WebOA.Controllers
{
    /// <summary>
    /// 请假管理
    /// </summary>
    public class LeaveController : BaseController
    {
        WebOA.Models.OAWebEntities db = new Models.OAWebEntities();
        // GET: Leave
        [HttpGet]
        public ActionResult LeaveList()
        {
            return View();

        }
        [HttpPost]
        public ActionResult LeaveList(string LEAVE_TYPE,  int Page = 1, int PageSize = 10)
        {
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;
            var query = db.Leaves.Where(x => x.LEAVE_NAME==User.Identity.Name);
            if (!string.IsNullOrEmpty(LEAVE_TYPE))
            {
                query = query.Where(x => x.LEAVE_TYPE.ToString() == LEAVE_TYPE);
            }
            int total = query.Count();
            var list = query.OrderBy(x => x.ID).Skip((Page - 1) * PageSize).Take(PageSize).ToList();
            var str = PagerDate.PagedData<Leave>(list, total);
            return Content(str);
        }
        [HttpGet]
        public ActionResult LeaveAdd()
        {
            var depId = db.Users.Where(x => x.NAME == User.Identity.Name).FirstOrDefault();
            var depts = db.Departments.Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.DepNAME,Selected= x.ID== (depId.DEPAR_ID) }).ToList();
            ViewBag.DEPART_ID = depts;
            return View();
        }
        [HttpPost]
        public ActionResult LeaveAdd(UILeave model)
        {
            Leave em = new Leave();
            em.LEAVE_NAME = model.LEAVE_NAME;
            em.START_DATE = model.START_DATE;
            em.END_DATE = model.END_DATE;
            em.DEPART_ID = model.DEPART_ID;
            em.LEAVE_TYPE = model.LEAVE_TYPE;
            em.REASONS = model.REASONS;
            db.Leaves.Add(em);
            db.SaveChanges();
            return Json(new UIResult(true, "申请成功"));
        }
        [HttpGet]
        public ActionResult LeavePass()
        {
            var depts = db.Departments.Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.DepNAME }).ToList();
            ViewBag.Depts = depts;
            return View();
        }
        [HttpPost]
        public ActionResult LeavePass(string DEPART_ID, string LEAVE_TYPE, string LEAVE_NAME, int Page = 1, int PageSize = 10)
        {
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;
            var query = db.Leaves.Where(x => true);
            if (!string.IsNullOrEmpty(LEAVE_NAME))
            {
                query = db.Leaves.Where(x => x.LEAVE_NAME.Contains(LEAVE_NAME));
            }
            if (!string.IsNullOrEmpty(DEPART_ID))
            {
                query = query.Where(x => x.DEPART_ID.ToString() == DEPART_ID);
            }
            if (!string.IsNullOrEmpty(LEAVE_TYPE))
            {
                query = query.Where(x => x.LEAVE_TYPE.ToString() == LEAVE_TYPE);
            }
            int total = query.Count();
            var list = query.OrderBy(x => x.ID).Skip((Page - 1) * PageSize).Take(PageSize).ToList();
            var str = PagerDate.PagedData<Leave>(list, total);
            return Content(str);
        }
        [HttpPost]
        public ActionResult Dels(int id)
        {
            try
            {
                int i = db.Database.ExecuteSqlCommand("Delete Leave where ID =" + id + "");
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
            var Check_date = DateTime.Now.ToString();
            int i = db.Database.ExecuteSqlCommand("update Leave set CHECK_FLAG=1,CHECK_NAME='" + User.Identity.Name + "',CHECK_DATE='" + Check_date + "'  where Id in(" + ids + ")");
            if (i <= 0)
            {
                return Json(new UIResult(false, "批量审核通过失败"));
            }
            return Json(new UIResult(true, ""));
        }
        [HttpPost]
        public ActionResult PubNO(string ids)
        {
            var Check_date = DateTime.Now.ToString();
            int i = db.Database.ExecuteSqlCommand("update Leave set CHECK_FLAG=2,CHECK_NAME='" + User.Identity.Name + "',CHECK_DATE='" + Check_date + "' where Id in(" + ids + ")");
            if (i <= 0)
            {
                return Json(new UIResult(false, "批量审核未通过失败"));
            }
            return Json(new UIResult(true, ""));
        }
        [HttpPost]
        public ActionResult StatusPassYes(int id)
        {
            var Check_date=DateTime.Now.ToString();
            int i = db.Database.ExecuteSqlCommand("update Leave set CHECK_FLAG=1,CHECK_NAME='"+User.Identity.Name+"',CHECK_DATE='"+ Check_date + "' where Id =" + id +"");
            if (i <= 0)
            {
                return Json(new UIResult(false, "审核通过失败"));
            }
            return Json(new UIResult(true, ""));
        }
        [HttpPost]
        public ActionResult StatusPassNo(int id)
        {
            var Check_date = DateTime.Now.ToString();
            int i = db.Database.ExecuteSqlCommand("update Leave set CHECK_FLAG=2,CHECK_NAME='" + User.Identity.Name + "',CHECK_DATE='" + Check_date + "' where Id =" + id +"");
            if (i <= 0)
            {
                return Json(new UIResult(false, "审核未通过失败"));
            }
            return Json(new UIResult(true, ""));
        }
    }
}