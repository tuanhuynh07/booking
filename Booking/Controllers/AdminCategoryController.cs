using Booking.Models;
using Classes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
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
        private Int32 thumb_height=Convert.ToInt32(ConfigurationManager.AppSettings["Thumb_height"]+"");
        private Int32 thumb_width = Convert.ToInt32(ConfigurationManager.AppSettings["Thumb_width"] + "");
        private Int32 medium_height = Convert.ToInt32(ConfigurationManager.AppSettings["Medium_height"] + "");
        private Int32 medium_width = Convert.ToInt32(ConfigurationManager.AppSettings["Medium_width"] + "");
        public ActionResult Index()
        {
            if (!UserManager.Authenticated || !UserManager.RoleController("AdminCategory"))
            {
                return RedirectToAction("Index", "Admin");
            }
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
            if (!UserManager.Authenticated || !UserManager.RoleController("AdminCategory"))
            {
                return RedirectToAction("Index", "Admin");
            }
            BindDropDownCategory();
            BindListLanguage();
            return View();
        }

        [HttpPost]
        public ActionResult Create([Bind(Include = "CATEGORY_ISMEDIA,CATEGORY_ID,CATEGORY_NAME,CATEGORY_ORDER,CATEGORY_IS_SHOW_MENU,CATEGORY_IS_SHOW_FOOTER,CATEGORY_LINK,CATEGORY_ALIAS,CATEGORY_CREATEDATE,CATEGORY_CREATEBY,CATEGORY_PARENT_ID,NAME_TRANSLATION_ID")] CATEGORY category)
        {
            if (!UserManager.Authenticated || !UserManager.RoleController("AdminCategory"))
            {
                return RedirectToAction("Index", "Admin");
            }
            BindDropDownCategory();
            BindListLanguage();
            if (category.CATEGORY_NAME + "" == "")
            {
                TempData["categogyNameError"] = "Tên Danh mục không được bỏ trống";
                return View(category);
            }
            var categoryImage = Request.Files["CATEGORY_IMAGE"];            
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
                int i = db.CATEGORies.Where(x => x.CATEGORY_ALIAS == alias).Count();
                if (i > 0)
                {
                    alias = alias + (maxId + 1);
                }
                //lưu ảnh
                String categoryImagePath = null;
                string fileName = GetNewFileName(categoryImage.FileName);
                SaveImageCrop(categoryImage, fileName, "Thumb", ref categoryImagePath, thumb_height, thumb_width);
                SaveImageCrop(categoryImage, fileName, "Medium", ref categoryImagePath, medium_height, medium_width);                      
                category.CATEGORY_ID = maxId + 1;
                category.CATEGORY_IMAGE = categoryImagePath;
                if (category.CATEGORY_PARENT_ID == 0) category.CATEGORY_PARENT_ID = null;
                category.CATEGORY_CREATEDATE = DateTime.Now;
                category.CATEGORY_CREATEBY = UserManager.GetUserName;
                category.CATEGORY_ALIAS = alias;
                category.CATEGORY_NAME = title;
                if (category.CATEGORY_ORDER == null) category.CATEGORY_ORDER = 0;
                var listLanguage = db.LANGUAGEs.Where(m => m.LANGUAGE_IS_PRIMARY != true).ToArray();
                if (listLanguage.Count() > 0)
                {
                    decimal maxIdTran = 0;
                    try
                    {
                        maxIdTran = db.TRANSLATION_CATEGORY.Max(x => x.ID);
                    }
                    catch (Exception)
                    {
                    }
                    foreach (var item in listLanguage)
                    {
                        string name = Request.Form["CATEGORY_NAME_" + item.LANGUAGE_ID] + "";
                        if (name != "")
                        {
                            TRANSLATION_CATEGORY tran = new TRANSLATION_CATEGORY();                            
                            tran.ID = maxIdTran + 1;
                            tran.LANGUAGE_ID = item.LANGUAGE_ID;
                            tran.TEXT = name;
                            category.NAME_TRANSLATION_ID = maxIdTran + 1;
                            db.TRANSLATION_CATEGORY.Add(tran);
                            db.SaveChanges();
                        }
                    }
                }
                db.CATEGORies.Add(category);
                db.SaveChanges();
                
                return RedirectToAction("Index");
            }
            return View(category);
        }

        [HttpGet]
        public ActionResult Edit(String id)
        {
            if (!UserManager.Authenticated || !UserManager.RoleController("AdminCategory"))
            {
                return RedirectToAction("Index", "Admin");
            }
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
            BindListLanguage();
            return View(category);
        }
        [HttpPost]
        public ActionResult Edit(CATEGORY category, String id)
        {
            if (!UserManager.Authenticated || !UserManager.RoleController("AdminCategory"))
            {
                return RedirectToAction("Index", "Admin");
            }
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
                BindListLanguage();
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
                String title = category.CATEGORY_NAME.Trim();
                String alias = ru.SlugLink(category.CATEGORY_NAME.Trim());
                if (category_old.CATEGORY_ALIAS!= alias)
                {
                    int i = db.CATEGORies.Where(x => x.CATEGORY_ALIAS == alias).Count();
                    if (i > 0)
                    {
                        alias = alias + category.CATEGORY_ID;
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
                    deleteFile(imagePath);                    
                    //Lưu file
                    string fileName = GetNewFileName(categoryImage.FileName);
                    SaveImageCrop(categoryImage,fileName,"Thumb", ref imagePath, thumb_height, thumb_width);
                    SaveImageCrop(categoryImage,fileName,"Medium", ref imagePath, medium_height, medium_width);
                }
                else
                {
                    if (Request.Form["deleteImg"] != null && Request.Form["deleteImg"] + "" != "false")
                    {
                        deleteFile(imagePath);
                        imagePath = "";
                    }
                }
                category_old.CATEGORY_IMAGE = imagePath;
                category_old.CATEGORY_ISMEDIA = category.CATEGORY_ISMEDIA;
                category_old.CATEGORY_NAME = category.CATEGORY_NAME;
                category_old.CATEGORY_ORDER = category.CATEGORY_ORDER;
                category_old.CATEGORY_IS_SHOW_MENU = category.CATEGORY_IS_SHOW_MENU;
                category_old.CATEGORY_IS_SHOW_FOOTER = category.CATEGORY_IS_SHOW_FOOTER;
                category_old.CATEGORY_LINK = category.CATEGORY_LINK;
                var listLanguage = db.LANGUAGEs.Where(m => m.LANGUAGE_IS_PRIMARY != true).ToArray();
                if (category.NAME_TRANSLATION_ID != null)
                {
                    foreach (var item in listLanguage)
                    {
                        string name = Request.Form["CATEGORY_NAME_" + item.LANGUAGE_ID] + "";
                        var gettran = db.TRANSLATION_CATEGORY.Where(m => m.LANGUAGE_ID == item.LANGUAGE_ID && m.ID == category.NAME_TRANSLATION_ID);
                        if (gettran!=null && gettran.Count()>0)
                        {
                            if (name != "")
                            {
                                gettran.First().TEXT = name;
                                db.SaveChanges();
                            }
                            else
                            {
                                db.TRANSLATION_CATEGORY.Remove(gettran.First());
                                db.SaveChanges();
                            }
                        }
                        else
                        {
                            if(name!="")
                            {
                                TRANSLATION_CATEGORY tran = new TRANSLATION_CATEGORY();                                
                                tran.ID = category.NAME_TRANSLATION_ID.Value;
                                tran.LANGUAGE_ID = item.LANGUAGE_ID;
                                tran.TEXT = name;
                                db.TRANSLATION_CATEGORY.Add(tran);
                                db.SaveChanges();
                            }
                        }
                    }
                }
                else
                {
                    TRANSLATION_CATEGORY tran = new TRANSLATION_CATEGORY();
                    decimal maxIdTran = 0;
                    try
                    {
                        maxIdTran = db.TRANSLATION_CATEGORY.Max(x => x.ID);
                    }
                    catch (Exception)
                    {
                    }
                    foreach (var item in listLanguage)
                    {
                        string name = Request.Form["CATEGORY_NAME_" + item.LANGUAGE_ID] + "";
                        if (name != "")
                        {
                            tran.ID = maxIdTran + 1;
                            tran.LANGUAGE_ID = item.LANGUAGE_ID;
                            tran.TEXT = name;
                            category.NAME_TRANSLATION_ID = maxIdTran + 1;
                            db.TRANSLATION_CATEGORY.Add(tran);
                            db.SaveChanges();
                        }
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        [HttpGet]
        public ActionResult Delete(String id)
        {
            if (!UserManager.Authenticated || !UserManager.RoleController("AdminCategory"))
            {
                return RedirectToAction("Index", "Admin");
            }
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
            if(category.NAME_TRANSLATION_ID!=null)
            {
                var gettran = db.TRANSLATION_CATEGORY.Where(m => m.ID == category.NAME_TRANSLATION_ID);
                db.TRANSLATION_CATEGORY.RemoveRange(gettran);
                db.SaveChanges();
            }
            var categoryImage = category.CATEGORY_IMAGE;
            if (categoryImage + "" != "")
            {
                //Xóa file cũ
                deleteFile(categoryImage);
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
        public void BindListLanguage()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            var getlist = db.LANGUAGEs.Where(m => m.LANGUAGE_IS_PRIMARY != true).ToArray();
            foreach(var item in getlist)
            {
                list.Add(new SelectListItem
                {
                    Text = item.LANGUAGE_NAME+"",
                    Value = item.LANGUAGE_ID+""
                });
            }
            ViewBag.BindListLanguage = list;
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
                fileName =Path.GetFileNameWithoutExtension(a)+"-"+DateTime.Now.Ticks + Path.GetExtension(a);
            }
            return fileName;
        }
        public bool deleteFile(String path)
        {
            if (path + "" != "")
            {
                int startIndex = path.LastIndexOf("/") + 1;
                int endIndex = path.Length;
                string filename = path.Substring(startIndex, (endIndex - startIndex));
                string path1 = Path.Combine(Server.MapPath("/upload/images/" + filename));
                string path2 = Path.Combine(Server.MapPath("/upload/images/Thumb/" + filename));
                string path3 = Path.Combine(Server.MapPath("/upload/images/Medium/" + filename));
                try
                {
                    if (System.IO.File.Exists(path1)) System.IO.File.Delete(path1);
                    if (System.IO.File.Exists(path2)) System.IO.File.Delete(path2);
                    if (System.IO.File.Exists(path3)) System.IO.File.Delete(path3);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else return true;
        }
        public void SaveImageCrop(HttpPostedFileBase img,string filename,string folder,ref string path_default,int height,int width)
        {
            if (img != null && img.ContentLength > 0)
            {
                var originalDirectory = new DirectoryInfo(string.Format("{0}Upload\\images\\", Server.MapPath(@"\")));
                path_default = "/upload/images/" + filename;
                img.SaveAs(Path.Combine(Server.MapPath(path_default)));//Lưu thư mục chính
                string pathString = System.IO.Path.Combine(originalDirectory.ToString(), folder);
                bool isExists = System.IO.Directory.Exists(pathString);
                if (!isExists) System.IO.Directory.CreateDirectory(pathString);
                var path = string.Format("{0}\\{1}", pathString, filename);
                var newPath = ImageTool.GetNewPathForDupes(path, ref filename);
                //Lưu hình ảnh Resize từ file sử dụng file.InputStream
                ImageTool.SaveCroppedImage(Image.FromStream(img.InputStream), width, height, newPath);
                path_default = "/upload/images/Thumb/" + filename;
            }
        }
	}
}