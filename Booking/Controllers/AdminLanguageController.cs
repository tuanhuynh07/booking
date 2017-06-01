using Booking.Models;
using Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Booking.Controllers
{
    public class AdminLanguageController : Controller
    {
        private DB_BOOKINGEntities db = new DB_BOOKINGEntities();
        // GET: /Language/
        public ActionResult Index()
        {
            if (!UserManager.Authenticated || !UserManager.RoleController("AdminLanguage"))
            {
                return RedirectToAction("Index", "Admin");
            }
            List<LANGUAGE> lc = new List<LANGUAGE>();
            foreach (LANGUAGE it in db.LANGUAGEs.ToList())
            {
                var ls = new LANGUAGE();
                ls.LANGUAGE_ACTIVE = it.LANGUAGE_ACTIVE;
                ls.LANGUAGE_CODE = it.LANGUAGE_CODE;
                ls.LANGUAGE_ID = it.LANGUAGE_ID;
                ls.LANGUAGE_IS_PRIMARY = it.LANGUAGE_IS_PRIMARY;
                ls.LANGUAGE_NAME = it.LANGUAGE_NAME;
                lc.Add(ls);
            }
            ViewBag.ListOfCategory = lc;
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            if (!UserManager.Authenticated || !UserManager.RoleController("AdminLanguage"))
            {
                return RedirectToAction("Index", "Admin");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Create([Bind(Include = "LANGUAGE_ACTIVE,LANGUAGE_CODE,LANGUAGE_ID,LANGUAGE_IS_PRIMARY,LANGUAGE_NAME")] LANGUAGE category)
        {
            if (!UserManager.Authenticated || !UserManager.RoleController("AdminLanguage"))
            {
                return RedirectToAction("Index", "Admin");
            }            
            if (category.LANGUAGE_CODE + "" == "")
            {
                TempData["categogyNameError"] = "Mã ngôn ngữ không được bỏ trống";
                return View(category);
            }
            if (category.LANGUAGE_NAME + "" == "")
            {
                TempData["categogyNameError"] = "Tên ngôn ngữ không được bỏ trống";
                return View(category);
            } 
            if(category.LANGUAGE_IS_PRIMARY.Value)
            {
                if(db.LANGUAGEs.Where(m=>m.LANGUAGE_IS_PRIMARY.Value).Count()>0)
                {
                    TempData["categogyNameError"] = "Đã tồn tại ngôn ngữ chính trong hệ thống, ngôn ngữ này không được thiết lập là ngôn ngôn ngữ chính.";
                    return View(category);
                }
            }
            if (ModelState.IsValid)
            {
                //Tìm max id
                decimal maxId = 0;
                try
                {
                    maxId = db.LANGUAGEs.Max(x => x.LANGUAGE_ID);
                }
                catch (Exception)
                {
                }
                //tìm alias
                
                category.LANGUAGE_ID = maxId + 1;
                String code = category.LANGUAGE_CODE.Trim();
                String title = category.LANGUAGE_NAME.Trim();
                category.LANGUAGE_CODE = code;
                category.LANGUAGE_NAME = title;                               
                db.LANGUAGEs.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        [HttpGet]
        public ActionResult Edit(decimal id)
        {
            if (!UserManager.Authenticated || !UserManager.RoleController("AdminLanguage"))
            {
                return RedirectToAction("Index", "Admin");
            }
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            LANGUAGE category = db.LANGUAGEs.Where(x => x.LANGUAGE_ID == id).FirstOrDefault();
            if (category == null)
            {
                return RedirectToAction("Index");
            }
            return View(category);
        }
        [HttpPost]
        public ActionResult Edit(LANGUAGE category, decimal id)
        {
            if (!UserManager.Authenticated || !UserManager.RoleController("AdminLanguage"))
            {
                return RedirectToAction("Index", "Admin");
            }
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return RedirectToAction("Index");
                }
                LANGUAGE category_old = db.LANGUAGEs.Where(x => x.LANGUAGE_ID == id).FirstOrDefault();
                if (category_old == null)
                {
                    return RedirectToAction("Index");
                }
                if (category.LANGUAGE_CODE + "" == "")
                {
                    TempData["categogyNameError"] = "Mã ngôn ngữ không được bỏ trống";
                    return View(category);
                }
                if (category.LANGUAGE_NAME + "" == "")
                {
                    TempData["categogyNameError"] = "Tên ngôn ngữ không được bỏ trống";
                    return View(category);
                }
                if (category.LANGUAGE_IS_PRIMARY.Value)
                {
                    if (db.LANGUAGEs.Where(m => m.LANGUAGE_IS_PRIMARY.Value && m.LANGUAGE_ID!= id).Count() > 0)
                    {
                        TempData["categogyNameError"] = "Đã tồn tại ngôn ngữ chính trong hệ thống, ngôn ngữ này không được thiết lập là ngôn ngôn ngữ chính.";
                        return View(category);
                    }
                }
                category_old.LANGUAGE_ACTIVE = category.LANGUAGE_ACTIVE;
                category_old.LANGUAGE_CODE = category.LANGUAGE_CODE.Trim();
                category_old.LANGUAGE_NAME = category.LANGUAGE_NAME.Trim();
                category_old.LANGUAGE_IS_PRIMARY = category.LANGUAGE_IS_PRIMARY;                
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        [HttpGet]
        public ActionResult Delete(decimal id)
        {
            if (!UserManager.Authenticated || !UserManager.RoleController("AdminLanguage"))
            {
                return RedirectToAction("Index", "Admin");
            }
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            LANGUAGE category = db.LANGUAGEs.Where(x => x.LANGUAGE_ID == id).FirstOrDefault();
            if (category == null)
            {
                TempData["message_fail"] = "Không thể xóa ngôn ngữ này. Không tìm thấy!";
                return RedirectToAction("Index");
            }
            if (category.LANGUAGE_IS_PRIMARY.HasValue && category.LANGUAGE_IS_PRIMARY.Value)
            {
                TempData["message_fail"] = "Không thể xóa ngôn ngữ này. Đây là ngôn ngữ chính.";
                return RedirectToAction("Index");
            }
            if(category.TRANSLATION_ARTICLE.Count()>0 || category.TRANSLATION_CATEGORY.Count()>0 || category.TRANSLATION_CUSTOMER.Count()>0 ||
                category.TRANSLATION_HOTEL.Count()>0 || category.TRANSLATION_ROOM.Count()>0 || category.TRANSLATIONs.Count()>0)
            {
                TempData["message_fail"] = "Không thể xóa ngôn ngữ này. Ngôn ngữ này đã được sử dụng.";
                return RedirectToAction("Index");
            }
            db.LANGUAGEs.Remove(category);
            db.SaveChanges();
            TempData["message_success"] = "Xóa thành công!";
            return RedirectToAction("Index");
        }
	}
}