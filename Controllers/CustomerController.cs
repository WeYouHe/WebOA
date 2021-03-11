using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebOA.Models;

namespace WebOA.Controllers
{
    public class CustomerController : BaseController
    {
        WebOA.Models.OAWebEntities db = new Models.OAWebEntities();
        // GET: Customer
        [HttpGet]
        public ActionResult CustomerList()
        {
            var depts = db.C_Type.Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.VOCATION }).ToList();
            ViewBag.Depts = depts;
            return View();
        }
        [HttpPost]
        public ActionResult CustomerList(string Name, string CType, int Page = 1, int PageSize = 10)
        {
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;
            var query = db.Clients.Where(x => x.CLIENT_USER==User.Identity.Name);
            if (!string.IsNullOrEmpty(Name))
            {
                query = db.Clients.Where(x => x.CLIENT_NAME.Contains(Name));
            }
            if (!string.IsNullOrEmpty(CType))
            {
                query = query.Where(x => x.CLIENT_TYPE_ID.ToString() == CType);
            }
            int total = query.Count();
            var list = query.OrderBy(x => x.ID).Skip((Page - 1) * PageSize).Take(PageSize).ToList();
            var str = PagerDate.PagedData<Client>(list, total);
            return Content(str);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Dels(int id)
        {
            try
            {
                int i = db.Database.ExecuteSqlCommand("Delete Client where ID =" + id + "");
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
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Del(string ids)
        {
            int i = db.Database.ExecuteSqlCommand("Delete Client where ID in(" + ids + ")");
            if (i <= 0)
            {
                return Json(new UIResult(false, "批量删除失败"));
            }
            return Json(new UIResult(true, ""));
        }
        /// <summary>
        /// 客户信息修改
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CustomerEdit(int ID)
        {
            var us = db.Clients.Where(x => x.ID == ID).FirstOrDefault();
            var depts = db.C_Type.Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.VOCATION, Selected = x.ID == (us.CLIENT_TYPE_ID)  }).ToList();
            ViewBag.CLIENT_TYPE_ID = depts;
            var model = db.Clients.FirstOrDefault(x => x.ID == ID);
            return View(model);
        }

        //[HttpPost]
        //public ActionResult CustomerEdit(UICustomer model)
        //{
        //    try
        //    {
        //        var us = db.Clients.Where(x => x.CLIENT_NAME == model.CLIENT_NAME).FirstOrDefault();
        //        us.CLIENT_NAME = model.CLIENT_NAME;
        //        us.CLIENT_VOCATION = model.CLIENT_VOCATION;
        //        us.CLIENT_CY = model.CLIENT_CY;
        //        us.CLIENT_ADDRESS = model.CLIENT_ADDRESS;
        //        us.CLIENT_TYPE_ID = model.CLIENT_TYPE_ID;
        //        us.CLIENT_BANK = model.CLIENT_BANK;
        //        us.CLIENT_BANKNUM = model.CLIENT_BANKNUM;
        //        us.CLIENT_USER = model.CLIENT_USER;
        //        db.SaveChanges();
        //        return Json(new UIResult(true, "修改成功"));
        //    }
        //    catch (Exception ex)
        //    {

        //        return Json(new UIResult(false, "修改失败"+ex));
        //    }

        //}
        [HttpPost]
        public ActionResult CustomerEdit(UICustomer model)
        {
            try
            {
                var us = db.Clients.Where(x => x.ID == model.ID).FirstOrDefault();
                us.CLIENT_NAME = model.CLIENT_NAME;
                us.CLIENT_VOCATION = model.CLIENT_VOCATION;
                us.CLIENT_CY = model.CLIENT_CY;
                us.CLIENT_ADDRESS = model.CLIENT_ADDRESS;
                us.CLIENT_TYPE_ID = model.CLIENT_TYPE_ID;
                us.CLIENT_BANK = model.CLIENT_BANK;
                us.CLIENT_BANKNUM = model.CLIENT_BANKNUM;
                us.CLIENT_USER = model.CLIENT_USER;
                db.SaveChanges();
                return Json(new UIResult(true, "修改成功"));
            }
            catch (Exception ex)
            {

                return Json(new UIResult(false, "修改失败" + ex));
            }

        }
        [HttpGet]
        public ActionResult CustomerAdd()
        {
            var depts = db.C_Type.Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.VOCATION }).ToList();
            ViewBag.Depts = depts;
            return View();
        }
        [HttpPost]
        public ActionResult CustomerAdd(UICustomer model)
        {
            Client em = new Client();
            em.CLIENT_NAME = model.CLIENT_NAME;
            em.CLIENT_TYPE_ID = model.CLIENT_TYPE_ID;
            em.CLIENT_CY = model.CLIENT_CY;
            em.CLIENT_ADDRESS = model.CLIENT_ADDRESS;
            em.CLIENT_BANK = model.CLIENT_BANK;
            em.CLIENT_VOCATION = model.CLIENT_VOCATION;
            em.CLIENT_BANKNUM = model.CLIENT_BANKNUM;
            em.CLIENT_USER = User.Identity.Name;
            db.Clients.Add(em);
            db.SaveChanges();
            return Json(new UIResult(true, "添加成功"));
        }
    }
}