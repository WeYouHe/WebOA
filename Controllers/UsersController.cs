using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebOA.Models;

namespace WebOA.Controllers
{
    /// <summary>
    /// 用户管理
    /// </summary>
    public class UsersController : BaseController
    {
        WebOA.Models.OAWebEntities db = new Models.OAWebEntities();
        // GET: Users
        [HttpGet]
        public ActionResult UsersList()
        {
            var depts = db.Departments.Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.DepNAME }).ToList();
            ViewBag.DEPAR_ID = depts;
            return View();
        }
        [HttpPost]
        public ActionResult UsersList(string name,string RoleType,string DEPAR_ID,string StatusIS, int page=1,int limit = 10)
        {
            try
            {
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                var query = db.Users.Where(x => 1 == 1);
                if (!string.IsNullOrEmpty(name))
                {
                    query = db.Users.Where(x => x.NAME.Contains(name));
                }
                if (!string.IsNullOrEmpty(RoleType))
                {
                    query = db.Users.Where(x => x.ROLEID.ToString()==RoleType);
                }
                if (!string.IsNullOrEmpty(DEPAR_ID))
                {
                    query = db.Users.Where(x => x.DEPAR_ID.ToString() == DEPAR_ID);
                }
                if (!string.IsNullOrEmpty(StatusIS))
                {
                    query = db.Users.Where(x => x.STATUS.ToString() == StatusIS);
                }
                var list = query.OrderBy(x => x.Id).Skip((page - 1) * limit).Take(limit).ToList();
                int total = query.Count();
                var str = PagerDate.PagedData<User>(list, total);
                return Content(str);
            }
            catch (Exception ex)
            {

                return Json(new UIResult(false, ex.Message));
            }
        }
        [HttpGet]
        public ActionResult UsersAdd ()
        {
            var depts = db.Departments.Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.DepNAME }).ToList();
            ViewBag.DEPAR_ID = depts;
            return View();
        }
        [HttpPost]
        public ActionResult UsersAdd(User model)
        {
            string d = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //var  rol = 1;
            User em = new User();
            em.NAME = model.NAME;
            em.PHONE = model.PHONE.ToString();
            em.EMAIL = model.EMAIL;
            em.DEPAR_ID = model.DEPAR_ID;
            em.SEX = model.SEX;
            em.PASSWORD = model.PASSWORD; 
            em.ADDRESS = model.ADDRESS;
            em.CREATE_DATE = DateTime.Parse(d);
            em.UPDATE_DATE = DateTime.Parse(d);
            em.PORTRAIT = model.PORTRAIT;
            em.ROLEID = model.ROLEID;
            em.STATUS = model.STATUS;
            em.BIRTHDAY = model.BIRTHDAY;
            db.Users.Add(em);
            int i=db.SaveChanges();
            if (i > 0)
            {
                return Json(new
                {
                    success = true,
                    message = ""
                });
            }
            else
            {
                return Json(new
                {
                    success = false,
                    message = ""
                });
            }
            
        }
        [HttpGet]
        public ActionResult UerUpdate(int id)
        {
            User u = db.Users.Where(x => x.Id == id).FirstOrDefault();

            var depts = db.Departments.Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.DepNAME, Selected = x.ID == u.DEPAR_ID }).ToList();
            ViewBag.depts = depts;
            Models.User a = db.Users.Where(x => x.Id == id).FirstOrDefault();
            return View(a);
        }
        [HttpPost]
        public ActionResult Update(User user)
        {
            try
            {
                Models.User u = db.Users.Where(x => x.Id == user.Id).FirstOrDefault();
                u.EMAIL = user.EMAIL;
                u.PORTRAIT = user.PORTRAIT;
                u.NAME = user.NAME;
                u.SEX = u.SEX;
                u.PHONE = user.PHONE;
                u.ROLEID = user.ROLEID;
                db.SaveChanges();
                return Json(new UIResult(true, "成功"));
            }
            catch (Exception ex)
            {

                return Json(new UIResult(false, "失败"+ex.Message));
            }


        }
        public ActionResult UploadPhoto()
        {
            try
            {
                HttpPostedFileBase postFile = Request.Files["file"];
                if (postFile != null)
                {
                    string fileExt = postFile.FileName.Substring(postFile.FileName.LastIndexOf(".") + 1); //文件扩展名，不含“.”
                    string newFileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "." + fileExt; //随机生成新的文件名
                    string upLoadPath = "/Content/images/"; //上传目录相对路径
                    string fullUpLoadPath = Server.MapPath(upLoadPath);  //将路径转换成 物理路径
                    string newFilePath = upLoadPath + newFileName; //上传后的路径
                    postFile.SaveAs(fullUpLoadPath + newFileName);  //核心方法
                    return Json(new UIResult(true, "", "/Content/images/" + newFileName));
                }
                else
                {
                    return Json(new UIResult(false, "未检测到文件数据"));
                }
            }
            catch (Exception ex)
            {
                return Json(new UIResult(false, "上传失败:" + ex.Message));
            }
        }


        public ActionResult Upload()
        {
            try
            {
                HttpPostedFileBase pic = Request.Files["file"];
                if (pic != null)
                {
                    string filename = pic.FileName.Substring(pic.FileName.LastIndexOf(".") + 1);//扩展名
                    string newname = DateTime.Now.ToString("yyyyMMddHHmm") + "." + filename;//生成新文件
                    string xdpath = "/Content/images/";
                    string wlpath = Server.MapPath(xdpath);
                    string newpath = xdpath + newname;
                    pic.SaveAs(wlpath + newname);
                    return Json(new
                    {
                        code = 0,
                        message = "上传成功！",
                        count = 0,
                        data = xdpath + newname
                    });
                }
                else
                {
                    return Json(new
                    {
                        code = 0,
                        message = "上传失败！",
                        count = 0,
                        data = "上传失败"
                    });
                }
            }
            catch (Exception ex)
            {

                return Json(new
                {
                    success = false,
                    message = "上传失败:" + ex.Message
                });
            }
        }
        [HttpPost]
        public ActionResult Dels(int id)
        {
            try
            {
                int i = db.Database.ExecuteSqlCommand("Delete Users where ID =" + id + "");
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
            int i = db.Database.ExecuteSqlCommand("update Users set STATUS='0' where Id in(" + ids + ")");
            if (i <= 0)
            {
                return Json(new UIResult(false, "批量启用失败"));
            }
            return Json(new UIResult(true, ""));
        }
        [HttpPost]
        public ActionResult PubNO(string ids)
        {
            int i = db.Database.ExecuteSqlCommand("update Users set STATUS='1' where Id in(" + ids + ")");
            if (i <= 0)
            {
                return Json(new UIResult(false, "批量禁用失败"));
            }
            return Json(new UIResult(true, ""));
        }
        [HttpPost]
        public ActionResult PubDels(string ids)
        {
            int i = db.Database.ExecuteSqlCommand("Delete Users  where Id in(" + ids + ")");
            if (i <= 0)
            {
                return Json(new UIResult(false, "批量删除失败"));
            }
            return Json(new UIResult(true, ""));
        }
    }
}