using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebOA.Models;

namespace WebOA.Controllers
{
    /// <summary>
    /// 工作计划
    /// </summary>
    public class WorkPlanController : BaseController
    {
        WebOA.Models.OAWebEntities db = new Models.OAWebEntities();
        // GET: WorkPlan
        [HttpGet]
        public ActionResult Project()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Project( string Title, string Status, int page, int limit)
        {
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;
            var query = db.Plants.Where(x =>x.CARRY_USER==User.Identity.Name.ToString());
            if (!string.IsNullOrEmpty(Title))
            {
                query = query.Where(x => x.PLANT_NAME.Contains(Title));
            }
            if (!string.IsNullOrEmpty(Status))
            {
                query = query.Where(x => x.IN_FLAG.ToString()== Status);
            }
            var list = query.OrderBy(x => x.ID).Skip((page - 1) * limit).Take(limit).ToList();
            int total = query.Count();
            var str = PagerDate.PagedData<Plant>(list, total);
            return Content(str);
        }
        [HttpGet]
        public ActionResult WorkAdd()
        {
            return View();
        }
        [HttpPost]
        public ActionResult WorkAdd(Plant Work)
        {
            try
            {
                Plant em = new Plant();
                string d = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                em.CREA_DATE = DateTime.Parse(d);
                em.PLANT_NAME = Work.PLANT_NAME;
                em.CARRY_USER = Work.CARRY_USER;
                em.CREA_USER = Work.CREA_USER;
                em.PLANT_INFO = Work.PLANT_INFO;
                db.Plants.Add(em);
                db.SaveChanges();
                return Json(new UIResult(true, "添加成功"));
            }
            catch (Exception ex)
            {

                return Json(new UIResult(false, ex.Message));
            }
         
        }
        [HttpPost]
        public ActionResult PubStart(string ids)
        {
            var Start_date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            int i = db.Database.ExecuteSqlCommand("update Plant set IN_FLAG=1,START_DATE='"+ DateTime.Parse(Start_date) + "' where ID in (" + ids + ")");
            if (i <= 0)
            {
                return Json(new UIResult(false, "批量开始失败"));
            }
            return Json(new UIResult(true, ""));
        }
        [HttpPost]
        public ActionResult PubEnd(string ids)
        {
            var END_DATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            int i = db.Database.ExecuteSqlCommand("update Plant set IN_FLAG=2,START_DATE='" + DateTime.Parse(END_DATE) + "' where ID in (" + ids + ")");
            if (i <= 0)
            {
                return Json(new UIResult(false, "批量结束失败"));
            }
            return Json(new UIResult(true, ""));
        }
        [HttpPost]
        public ActionResult StartPlant(int id)
        {
            var Start_date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            int i = db.Database.ExecuteSqlCommand("update Plant set IN_FLAG=1 ,END_DATE='" + DateTime.Parse(Start_date) + "' where ID =" + id + "");
            if (i <= 0)
            {
                return Json(new UIResult(false, "计划开始失败"));
            }
            return Json(new UIResult(true, ""));
        }
        [HttpPost]
        public ActionResult EndStart(int id)
        {
            var End_date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            int i = db.Database.ExecuteSqlCommand("update Plant set IN_FLAG='2',END_DATE='" + DateTime.Parse(End_date) + "' where Id =" + id + "");
            if (i <= 0)
            {
                return Json(new UIResult(false, "计划结束失败"));
            }
            return Json(new UIResult(true, ""));
        }
    }

}