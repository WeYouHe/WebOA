using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebOA.Models;

namespace WebOA.Controllers
{
    public class DeptmentController : BaseController
    {
        // GET: Deptment
        WebOA.Models.OAWebEntities db = new Models.OAWebEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DeptmentList(string name, int page = 1, int limit = 10)
        {
            try
            {
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                var query = db.Departments.Where(x => 1 == 1);
                if (!string.IsNullOrEmpty(name))
                {
                    query = db.Departments.Where(x => x.DepNAME.Contains(name));
                }

                var list = query.OrderBy(x => x.ID).Skip((page - 1) * limit).Take(limit).ToList();
                int total = query.Count();
                var str = PagerDate.PagedData<Department>(list, total);
                return Content(str);
            }
            catch (Exception ex)
            {

                return Json(new UIResult(false, ex.Message));
            }

        }
        [HttpPost]
        public ActionResult Dels(int id)
        {
            try
            {
                int i = db.Database.ExecuteSqlCommand("Delete Department where ID =" + id + "");
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


        [HttpGet]

        public ActionResult DeptAdd()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(string name)
        {
            try
            {
                Department em = new Department();
                em.DepNAME = name;

                db.Departments.Add(em);
                db.SaveChanges();
                return Json(new UIResult(true, "添加成功"));
            }
            catch (Exception ex)
            {

                return Json(new UIResult(true, ex.Message));
            }
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DeptUpdate(int id)
        {
            var us = db.Departments.Where(x => x.ID == id).FirstOrDefault();

            return View(us);
        }
        public ActionResult Update(int id, string name)
        {
            try
            {
                Department d = db.Departments.Where(x => x.ID == id).FirstOrDefault();

                d.DepNAME = name;
                db.SaveChanges();
            }
            catch (Exception ex)
            {

                return Json(new
                {
                    success = false,
                    message = ex.Message
                });
            }

            return Json(new
            {
                success = true,
                message = "修改成功"
            });
        }
    }
}