using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebOA.Models;

namespace WebOA.Controllers
{
    /// <summary>
    /// 权限管理
    /// </summary>
    public class PermissionController : BaseController
    {
        WebOA.Models.OAWebEntities db = new Models.OAWebEntities();
        // GET: Permission
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult RolepermissonList()
        {
            var RolType = db.Roles.Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.RoleName }).ToList();
            ViewBag.RolType = RolType;
            return View();
        }
        [HttpPost]
        public ActionResult RolepermissonList( string RolType, int Page, int limit)
        {
            string sqldb = "select DISTINCT (b.ActionTitle),b.ID ,b.href,RoleName,c.Title ,b.Update_time from Role a, ControllerAction b,Controller c where b.ParentID <> 0 and a.ID = b.RoleID and b.ControllID = c.ID and a.ID like '"+RolType+"%'";
            var sqllist = db.Database.SqlQuery<PersonView>(sqldb).ToList();
            int total = sqllist.Count();
            var list = sqllist.OrderBy(x => x.ID).Skip((Page - 1) * limit).Take(limit).ToList();
            var str = PagerDate.PagedData<PersonView>(list, total);
            return Content(str);
        }
        [HttpGet]
        public ActionResult RoleEditt(int id)
        {
            return View();
        }
        [HttpGet]
        public ActionResult PermisTree()
        {
            return View();
        }
        //[HttpPost]
        //public ActionResult RoleEditt(string name, string RolType, string status, int Page, int limit)
        //{

        //    string sql = "SELECT DISTINCT(NAME),(SELECT STUFF((SELECT DISTINCT ', ' + CAST(Title AS VARCHAR(20))FROM(SELECT Title  FROM  View_CRM b where a.ROLEID = b.ROLEID) A FOR XML PATH('') ), 1 , 1 , '')) as Title,RoleName,RoleDesc,STATUS FROM View_CRM a where ROLEID like '" + RolType + "'+'%' and NAME like '" + name + "'+'%' and STATUS like '" + status + "'+'%'  ";
        //    var sqllist = db.Database.SqlQuery<PersonView>(sql).ToList();
        //    decimal total = sqllist.Count();
        //    var list = sqllist.OrderBy(x => x.NAME).Skip((Page - 1) * limit).Take(limit).ToList();

        //    return Json(new
        //    {
        //        code = 0,
        //        msg = "",
        //        count = total,
        //        data = list
        //    });
        //}
    }
}