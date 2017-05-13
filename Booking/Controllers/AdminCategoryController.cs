using Booking.Models;
using Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Booking.Controllers
{
    public class AdminCategoryController : Controller
    {
        private DB_BOOKINGEntities db = new DB_BOOKINGEntities();
        private RewriteUrl ru = new RewriteUrl();
        public ActionResult Index()
        {
            //if (!UserManager.Authenticated)
            //{
            //    return RedirectToAction("Login", "Admin");
            //}
            List<CATEGORY> lc = new List<CATEGORY>();
            foreach (CATEGORY it in db.CATEGORies.ToList())
            {
                var ls = new CATEGORY();
                ls.CATEGORY_ID = it.CATEGORY_ID;
                ls.CATEGORY_NAME = it.CATEGORY_NAME;
                ls.CATEGORY_ALIAS = it.CATEGORY_ALIAS;
                ls.CATEGORY_CREATEBY = it.CATEGORY_CREATEBY;
                ls.CATEGORY_CREATEDATE = it.CATEGORY_CREATEDATE;
                ls.CATEGORY_IS_SHOW_FOOTER = it.CATEGORY_IS_SHOW_FOOTER;
                ls.CATEGORY_IS_SHOW_MENU = it.CATEGORY_IS_SHOW_MENU;
                ls.CATEGORY_LINK = it.CATEGORY_LINK;
                ls.CATEGORY_ORDER = it.CATEGORY_ORDER;
                ls.CATEGORY_PARENT_ID = it.CATEGORY_PARENT_ID;
                lc.Add(ls);
            }
            ViewBag.ListOfCategory = lc;
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            //if (!UserManager.Authenticated)
            //{
            //    return RedirectToAction("Login", "Admin");
            //}
            BindDropDownCategory();
            return View();
        }

        [HttpPost]
        public ActionResult Create([Bind(Include = "CATEGORY_ISMEDIA,CATEGORY_ID,CATEGORY_NAME,CATEGORY_ORDER,CATEGORY_IS_SHOW_MENU,CATEGORY_IS_SHOW_FOOTER,CATEGORY_LINK,CATEGORY_ALIAS,CATEGORY_CREATEDATE,CATEGORY_CREATEBY,CATEGORY_PARENT_ID")] CATEGORY category)
        {
            //if (!UserManager.Authenticated)
            //{
            //    return RedirectToAction("Login", "Admin");
            //}
            BindDropDownCategory();
            if (category.CATEGORY_NAME + "" == "")
            {
                TempData["categogyNameError"] = "Tên Danh mục không được bỏ trống";
                return View(category);
            }
            var categoryImage = Request.Files["CATEGORY_IMAGE"];
            if (categoryImage.ContentLength == 0)
            {
                TempData["categogyImageError"] = "Ảnh đại điện không được bỏ trống";
                return View(category);
            }
            if (categoryImage != null && categoryImage.ContentLength > 0)
            {
                if (Path.GetExtension(categoryImage.FileName).ToLower() != ".jpg"
                        && Path.GetExtension(categoryImage.FileName).ToLower() != ".png"
                        && Path.GetExtension(categoryImage.FileName).ToLower() != ".gif"
                        && Path.GetExtension(categoryImage.FileName).ToLower() != ".jpeg")
                {
                    TempData["categogyImageError"] = "Ảnh đại điện phải là kiểu ảnh .jpg .png .gif .jpeg";
                    return View(category);
                }
            }
            if (ModelState.IsValid)
            {
                //Tìm max id
                decimal maxId = 0;
                try
                {
                    maxId = db.CATEGORies.Max(x => x.CATEGORY_ID);
                }
                catch (Exception)
                {
                }
                //tìm alias
                String alias = ru.SlugLink(category.CATEGORY_NAME.Trim());
                String title = category.CATEGORY_NAME.Trim();
                int i = db.CATEGORies.Where(x => x.CATEGORY_NAME.Trim() == category.CATEGORY_NAME.Trim()).Count();
                if (i > 0)
                {
                    //alias = alias + "-" + i;
                    //title = title + " ("+i+")";
                    TempData["categogyNameError"] = "Tên Danh mục đã tồn tại!";
                    return View(category);
                }
                //lưu ảnh
                String categoryImagePath = null;
                if (categoryImage != null && categoryImage.ContentLength > 0)
                {
                    categoryImagePath = "/upload/images/" + GetNewFileName(categoryImage.FileName);
                    categoryImage.SaveAs(Path.Combine(Server.MapPath(categoryImagePath)));
                }
                category.CATEGORY_ID = maxId + 1;
                category.CATEGORY_IMAGE = categoryImagePath;
                if (category.CATEGORY_PARENT_ID == 0) category.CATEGORY_PARENT_ID = null;
                category.CATEGORY_CREATEDATE = DateTime.Now;
                category.CATEGORY_CREATEBY = UserManager.GetUserName;
                category.CATEGORY_ALIAS = alias;
                category.CATEGORY_NAME = title;
                db.CATEGORies.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        [HttpGet]
        public ActionResult Edit(String id)
        {
            //if (!UserManager.Authenticated)
            //{
            //    return RedirectToAction("Login", "Admin");
            //}
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            CATEGORY category = db.CATEGORies.Where(x => x.CATEGORY_ALIAS == id).FirstOrDefault();
            if (category == null)
            {
                return RedirectToAction("Index");
            }
            BindDropDownCategoryForEdit(category.CATEGORY_ID);
            return View(category);
        }
        [HttpPost]
        public ActionResult Edit(CATEGORY category, String id)
        {
            //if (!UserManager.Authenticated)
            //{
            //    return RedirectToAction("Login", "Admin");
            //}
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return RedirectToAction("Index");
                }
                CATEGORY category_old = db.CATEGORies.Where(x => x.CATEGORY_ALIAS == id).FirstOrDefault();
                if (category_old == null)
                {
                    return RedirectToAction("Index");
                }
                BindDropDownCategoryForEdit(category_old.CATEGORY_ID);
                if (category.CATEGORY_NAME + "" == "")
                {
                    TempData["categogyNameError"] = "Tên Danh mục không được bỏ trống";
                    return View(category);
                }
                var categoryImage = Request.Files["CATEGORY_IMAGE"];
                if (category.CATEGORY_PARENT_ID == 0)
                {
                    category_old.CATEGORY_PARENT_ID = null;
                }
                else
                {
                    category_old.CATEGORY_PARENT_ID = category.CATEGORY_PARENT_ID;
                }
                //tìm alias
                if (category_old.CATEGORY_NAME.Trim() != category.CATEGORY_NAME.Trim())
                {
                    String title = category.CATEGORY_NAME.Trim();
                    String alias = ru.SlugLink(category.CATEGORY_NAME.Trim());
                    int i = db.CATEGORies.Where(x => x.CATEGORY_NAME.Trim() == category.CATEGORY_NAME.Trim()).Count();
                    if (i > 0)
                    {
                        //alias = alias + "-" + i;
                        //title = title + " ("+i+")";
                        TempData["categogyNameError"] = "Tên Danh mục đã tồn tại!";
                        return View(category);
                    }
                    category_old.CATEGORY_NAME = title;
                    category_old.CATEGORY_ALIAS = alias;
                    //cập nhật lại category alias cho các bài viết.
                    //kiểm tra có category con không
                    var category_child = db.CATEGORies.Where(x => x.CATEGORY_PARENT_ID == category_old.CATEGORY_ID).ToList();
                    if (category_child != null && category_child.Count() > 0)
                    {
                        foreach (var c in category_child)
                        {
                            var articles_with_child_category = db.ARTICLEs.Where(x => x.CATEGORY_ID == c.CATEGORY_ID).ToList();
                            if (articles_with_child_category != null && articles_with_child_category.Count() > 0)
                            {
                                foreach (var b in articles_with_child_category)
                                {
                                    b.CATEGORY_ALIAS = alias + "/" + c.CATEGORY_ALIAS;
                                }
                            }
                        }
                    }
                    var articles = db.ARTICLEs.Where(x => x.CATEGORY_ID == category_old.CATEGORY_ID).ToList();
                    if (articles != null && articles.Count() > 0)
                    {
                        foreach (var a in articles)
                        {
                            if (category_old.CATEGORY_PARENT_ID + "" != "")
                            {
                                var category_dad = db.CATEGORies.Where(x => x.CATEGORY_ID == category_old.CATEGORY_PARENT_ID).FirstOrDefault();
                                if (category_dad != null)
                                {
                                    a.CATEGORY_NAME = title;
                                    a.CATEGORY_ALIAS = category_dad.CATEGORY_ALIAS + "/" + alias;
                                }
                            }
                            else
                            {
                                a.CATEGORY_NAME = title;
                                a.CATEGORY_ALIAS = alias;
                            }
                        }
                    }
                }
                String imagePath = category_old.CATEGORY_IMAGE;
                if (categoryImage != null && categoryImage.ContentLength > 0)
                {
                    //Xóa file cũ
                    deleteFile(Path.Combine(Server.MapPath(imagePath)));
                    //Lưu file
                    imagePath = "/upload/images/" + GetNewFileName(categoryImage.FileName);
                    categoryImage.SaveAs(Path.Combine(Server.MapPath(imagePath)));
                }
                category_old.CATEGORY_IMAGE = imagePath;
                category_old.CATEGORY_ISMEDIA = category.CATEGORY_ISMEDIA;
                category_old.CATEGORY_NAME = category.CATEGORY_NAME;
                category_old.CATEGORY_ORDER = category.CATEGORY_ORDER;
                category_old.CATEGORY_IS_SHOW_MENU = category.CATEGORY_IS_SHOW_MENU;
                category_old.CATEGORY_IS_SHOW_FOOTER = category.CATEGORY_IS_SHOW_FOOTER;
                category_old.CATEGORY_LINK = category.CATEGORY_LINK;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        [HttpGet]
        public ActionResult Delete(String id)
        {
            //if (!UserManager.Authenticated)
            //{
            //    return RedirectToAction("Login", "Admin");
            //}
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            CATEGORY category = db.CATEGORies.Where(x => x.CATEGORY_ALIAS == id).FirstOrDefault();
            if (category == null)
            {
                TempData["message_fail"] = "Không thể xóa danh mục này. Không tìm thấy danh mục!";
                return RedirectToAction("Index");
            }
            ARTICLE article = db.ARTICLEs.Where(x => x.CATEGORY_ID == category.CATEGORY_ID).FirstOrDefault();
            if (article != null)
            {
                TempData["message_fail"] = "Không thể xóa danh mục này. Xóa bài viết của danh mục trước!";
                return RedirectToAction("Index");
            }
            Medium media = db.MEDIA.Where(x => x.CATEGORY_ID == category.CATEGORY_ID).FirstOrDefault();
            if (media != null)
            {
                TempData["message_fail"] = "Không thể xóa danh mục này. Xóa media của danh mục trước!";
                return RedirectToAction("Index");
            }
            int categoryChilCount = db.CATEGORies.Where(x => x.CATEGORY_PARENT_ID == category.CATEGORY_ID).Count();
            if (categoryChilCount > 0)
            {
                TempData["message_fail"] = "Không thể xóa danh mục này. Xóa danh mục con trước!";
                return RedirectToAction("Index");
            }
            //var role = db.CATEGORies.Include(x => x.ROLEs);
            //if (role != null)
            //{
            //    TempData["message_fail"] = "Không thể xóa danh mục này. Xóa quyền của danh mục trước!";
            //    return RedirectToAction("Index");
            //}
            var categoryImage = category.CATEGORY_IMAGE;
            if (categoryImage + "" != "")
            {
                //Xóa file cũ
                deleteFile(Path.Combine(Server.MapPath(categoryImage)));
            }
            db.CATEGORies.Remove(category);
            db.SaveChanges();
            TempData["message_success"] = "Xóa thành công!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult DisplayOnMenu()
        {
            decimal id = decimal.Parse(Request["id"].ToString());
            CATEGORY category = db.CATEGORies.Find(id);
            if (category != null)
            {
                if ((bool)category.CATEGORY_IS_SHOW_MENU)
                {
                    category.CATEGORY_IS_SHOW_MENU = false;
                    db.SaveChanges();
                    return Json(new { message = 0 });
                }
                else
                {
                    category.CATEGORY_IS_SHOW_MENU = true;
                    db.SaveChanges();
                    return Json(new { message = 1 });
                }
            }
            return null;
        }

        [HttpPost]
        public JsonResult DisplayOnMenuFooter()
        {
            decimal id = decimal.Parse(Request["id"].ToString());
            CATEGORY category = db.CATEGORies.Find(id);
            if (category != null)
            {
                if ((bool)category.CATEGORY_IS_SHOW_FOOTER)
                {
                    category.CATEGORY_IS_SHOW_FOOTER = false;
                    db.SaveChanges();
                    return Json(new { message = 0 });
                }
                else
                {
                    category.CATEGORY_IS_SHOW_FOOTER = true;
                    db.SaveChanges();
                    return Json(new { message = 1 });
                }
            }
            return null;
        }

        [HttpPost]
        public JsonResult deleteImage()
        {
            String id = Request["id"].ToString();
            if (id + "" != "")
            {
                decimal ctId = decimal.Parse(id);
                CATEGORY category = db.CATEGORies.Find(ctId);
                if (category != null)
                {
                    String fileImagePath = Path.Combine(Server.MapPath(category.CATEGORY_IMAGE));
                    if (deleteFile(fileImagePath))
                    {
                        //Xóa trong csdl
                        category.CATEGORY_IMAGE = null;
                        db.SaveChanges();
                        return Json(new { message = "Xóa thành công" });
                    }
                }
            }
            return null;
        }

        public void BindDropDownCategory()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "---Chọn Danh mục---", Value = "0" });
            var cat = (from c in db.CATEGORies select c).ToArray();
            for (int i = 0; i < cat.Length; i++)
            {
                list.Add(new SelectListItem
                {
                    Text = cat[i].CATEGORY_NAME,
                    Value = cat[i].CATEGORY_ID + ""
                });
            }
            ViewBag.BindDropDownCategory = list;
        }
        public void BindDropDownCategoryForEdit(decimal cateId)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "-Chọn Danh mục-", Value = "0" });
            var cat = (from c in db.CATEGORies where c.CATEGORY_ID != cateId select c).ToArray();
            for (int i = 0; i < cat.Length; i++)
            {
                list.Add(new SelectListItem
                {
                    Text = cat[i].CATEGORY_NAME,
                    Value = cat[i].CATEGORY_ID + ""
                });
            }
            ViewBag.BindDropDownCategory = list;
        }
        public string GetNewFileName(string a)
        {
            string fileName = "";
            if (a.Length > 0)
            {
                Random rnd = new Random();
                DateTime date = DateTime.Now;
                fileName = date.Day + "-" + date.Month + "-" + date.Year + "-" + date.Hour + "-" + date.Minute + "-" + date.Second + "-" + rnd.Next(1, 100) + Path.GetExtension(a);
            }
            return fileName;
        }
        public bool deleteFile(String path)
        {
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
                return true;
            }
            return false;
        }
	}
}