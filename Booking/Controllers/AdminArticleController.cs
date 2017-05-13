using Booking.Models;
using Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Globalization;

namespace Booking.Controllers
{
    public class AdminArticleController : Controller
    {
        private DB_BOOKINGEntities db = new DB_BOOKINGEntities();
        private RewriteUrl ru = new RewriteUrl();
        string strUrl = System.Web.HttpContext.Current.Request.Url.AbsoluteUri;
        string strPath = System.Web.HttpContext.Current.Request.ApplicationPath;
        string host = System.Web.HttpContext.Current.Request.Url.Host;


        public ActionResult Index()
        {
            //if (!UserManager.Authenticated)
            //{
            //    return RedirectToAction("Login", "Admin");
            //}
            string urlHome = "/";if (strPath != "/")
            {
                urlHome = strPath + urlHome;
            }
            int start = 0;
            String rs = "<ul class='nav nav-stacked'>";
            var categorys = db.CATEGORies.Where(x => x.CATEGORY_ISMEDIA == false).Where(x => x.CATEGORY_PARENT_ID == null).ToList();
            if (categorys != null && categorys.Count() > 0)
            {
                foreach (var item in categorys)
                {
                    int aCount = db.ARTICLEs.Where(x => x.CATEGORY_ID == item.CATEGORY_ID).Count();
                    rs += "<li style='clear:both;'><a href='" + urlHome + "adminarticle/articelincategory/" + item.CATEGORY_ID + "' class='col-md-11'>" + item.CATEGORY_NAME + "<span title='Có " + aCount + " bài viết' class='pull-right badge bg-blue'>" + aCount + "</span></a><a class='col-md-1' title='Thêm bài viết' href='" + urlHome + "adminarticle/create/" + item.CATEGORY_ID + "'><i class='fa fa-plus' style='color:green;'></i></a></li>";
                    GetChildCategory(item.CATEGORY_ID, ref rs, start);
                }
            }
            rs += "</ul>";
            ViewBag.ListOfCategory = rs;
            return View();
        }

        public ActionResult ArticelInCategory(String id, string currentFilter, string searchString, int? page)
        {
            //if (!UserManager.Authenticated)
            //{
            //    return RedirectToAction("Login", "Admin");
            //}
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;
            ViewBag.Id = id;
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            //Lấy danh sách bài viết thuộc danh mục
            else
            {
                decimal cateogryId = decimal.Parse(id);
                CATEGORY category = db.CATEGORies.Where(x => x.CATEGORY_ID == cateogryId).FirstOrDefault();
                if (category == null)
                {
                    return RedirectToAction("Index");
                }
                ViewBag.categoryName = category.CATEGORY_NAME;
                List<ARTICLE> articles = new List<ARTICLE>();
                foreach (ARTICLE a in db.ARTICLEs.Where(x => x.CATEGORY_ID == cateogryId).OrderByDescending(x => x.ARTICLE_CREATEDATE).ToList())
                {
                    var ls = new ARTICLE();
                    ls.ARTICLE_ID = a.ARTICLE_ID;
                    ls.ARTICLE_TITLE = a.ARTICLE_TITLE;
                    ls.ARTICLE_BRIEF = a.ARTICLE_BRIEF;
                    ls.ARTICLE_ALIAS = a.ARTICLE_ALIAS;
                    ls.ARTICLE_DISABLE = a.ARTICLE_DISABLE;
                    articles.Add(ls);
                }
                if (!String.IsNullOrEmpty(searchString))
                {
                    articles = db.ARTICLEs.Where(x => x.ARTICLE_TITLE.Contains(searchString)
                                                    || x.ARTICLE_BRIEF.Contains(searchString)).ToList();
                }
                int pageSize = 15;
                int currentPage = (page ?? 1);
                ViewBag.currentPage = currentPage;
                ViewBag.pageSize = pageSize;
                return View(articles.ToPagedList(currentPage, pageSize));
            }
        }

        public ActionResult Create(String id)
        {
            //if (!UserManager.Authenticated)
            //{
            //    return RedirectToAction("Login", "Admin");
            //}
            ViewBag.Id = id;
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                decimal cateogryId = decimal.Parse(id);
                CATEGORY category = db.CATEGORies.Where(x => x.CATEGORY_ID == cateogryId).FirstOrDefault();
                if (category == null)
                {
                    return RedirectToAction("Index");
                }
                ViewBag.categoryName = category.CATEGORY_NAME;
            }
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(ARTICLE article, String id)
        {
            //if (!UserManager.Authenticated)
            //{
            //    return RedirectToAction("Login", "Admin");
            //}
            #region validate
            if (article.ARTICLE_TITLE + "" == "")
            {
                TempData["articleNameError"] = "Tiêu đề bài viết không được bỏ trống";
                return View(article);
            }
            if (Request.Files["ARTICLE_IMAGE_LARGE"].ContentLength == 0)
            {
                TempData["articleImageError"] = "Ảnh đại điện không được bỏ trống";
                return View(article);
            }
            if (Request.Files["ARTICLE_IMAGE_THUMB"].ContentLength == 0)
            {
                TempData["articleImageError"] = "Ảnh nhỏ không được bỏ trống";
                return View(article);
            }
            if (Request.Files["ARTICLE_IMAGE_LARGE"] != null && Request.Files["ARTICLE_IMAGE_LARGE"].ContentLength > 0)
            {
                if (Path.GetExtension(Request.Files["ARTICLE_IMAGE_LARGE"].FileName).ToLower() != ".jpg"
                        && Path.GetExtension(Request.Files["ARTICLE_IMAGE_LARGE"].FileName).ToLower() != ".png"
                        && Path.GetExtension(Request.Files["ARTICLE_IMAGE_LARGE"].FileName).ToLower() != ".gif"
                        && Path.GetExtension(Request.Files["ARTICLE_IMAGE_LARGE"].FileName).ToLower() != ".jpeg")
                {
                    TempData["articleImageError"] = "Ảnh đại điện phải là kiểu ảnh .jpg .png .gif .jpeg";
                    return View(article);
                }
            }
            if (Request.Files["ARTICLE_IMAGE_THUMB"] != null && Request.Files["ARTICLE_IMAGE_THUMB"].ContentLength > 0)
            {
                if (Path.GetExtension(Request.Files["ARTICLE_IMAGE_THUMB"].FileName).ToLower() != ".jpg"
                        && Path.GetExtension(Request.Files["ARTICLE_IMAGE_THUMB"].FileName).ToLower() != ".png"
                        && Path.GetExtension(Request.Files["ARTICLE_IMAGE_THUMB"].FileName).ToLower() != ".gif"
                        && Path.GetExtension(Request.Files["ARTICLE_IMAGE_THUMB"].FileName).ToLower() != ".jpeg")
                {
                    TempData["articleImageError"] = "Ảnh nhỏ phải là kiểu ảnh .jpg .png .gif .jpeg";
                    return View(article);
                }
            }
            if (article.ARTICLE_CONTENT + "" == "")
            {
                TempData["articleContentError"] = "Nội dung bài viết không được bỏ trống";
                return View(article);
            }
            #endregion
            ViewBag.Id = id;
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                decimal cateogryId = decimal.Parse(id);
                CATEGORY category = db.CATEGORies.Where(x => x.CATEGORY_ID == cateogryId).FirstOrDefault();
                if (category == null)
                {
                    return RedirectToAction("ArticelInCategory");
                }
                //Tìm max id
                decimal maxId = 0;
                try
                {
                    maxId = db.ARTICLEs.Max(x => x.ARTICLE_ID);
                }
                catch (Exception)
                {
                }
                //tìm alias
                String alias = ru.SlugLink(article.ARTICLE_TITLE.Trim());
                String title = article.ARTICLE_TITLE.Trim();
                int i = db.ARTICLEs.Where(x => x.CATEGORY_ID == cateogryId).Where(x => x.ARTICLE_TITLE.Trim() == article.ARTICLE_TITLE.Trim()).Count();
                if (i > 0)
                {
                    //alias = alias + "-" + i;
                    //title = title + " (" + i + ")";
                    TempData["articleNameError"] = "Tiêu đề này đã tồn tại!";
                    return View(article);
                }
                article.ARTICLE_ID = maxId + 1;
                #region //Lưu ảnh
                String largeImagePath = null;
                String thumbImagePath = null;
                var largeImage = Request.Files["ARTICLE_IMAGE_LARGE"];
                if (largeImage != null && largeImage.ContentLength > 0)
                {
                    String fileName = GetNewFileName(largeImage.FileName);
                    largeImagePath = "/upload/images/" + fileName;
                    largeImage.SaveAs(Path.Combine(Server.MapPath(largeImagePath)));
                    ////lưu image croped dùng cho jcrop.js
                    ////string imgCropped = Request["imgCropped"];
                    ////if (imgCropped + "" != "")
                    ////{
                    ////    byte[] bytes = Convert.FromBase64String(imgCropped.Split(',')[1]);
                    ////    thumbImagePath = "/upload/_thumbs/Images/" + GetNewFileName(largeImage.FileName);
                    ////    FileStream stream = new FileStream(Path.Combine(Server.MapPath(thumbImagePath)), System.IO.FileMode.Create);
                    ////    stream.Write(bytes, 0, bytes.Length);
                    ////    stream.Flush();
                    ////    stream.Close();
                    ////}
                    Image image = Image.FromStream(largeImage.InputStream);
                    String largeImagePath1 = "/upload/images1/" + fileName;
                    Image thumb = ResizeImage(image, 229, 172, false);
                    thumb.Save(Path.Combine(Server.MapPath(largeImagePath1)));
                }
                var thumbImage = Request.Files["ARTICLE_IMAGE_THUMB"];
                if (thumbImage != null && thumbImage.ContentLength > 0)
                {
                    String fileName = GetNewFileName(thumbImage.FileName);
                    thumbImagePath = "/upload/_thumbs/Images/" + fileName;
                    thumbImage.SaveAs(Path.Combine(Server.MapPath(thumbImagePath)));
                    //resize kích thước (149x112)
                    Image image = Image.FromStream(thumbImage.InputStream);
                    String thumbImagePath1 = "/upload/_thumbs/Images1/" + fileName;
                    Image thumb = ResizeImage(image, 149, 112, false);
                    thumb.Save(Path.Combine(Server.MapPath(thumbImagePath1)));
                }
                article.ARTICLE_IMAGE_LARGE = largeImagePath;
                article.ARTICLE_IMAGE_THUMB = thumbImagePath;
                #endregion
                #region //Lưu file
                String filePath1 = null;
                String fileName1 = null;
                String filePath2 = null;
                String fileName2 = null;
                var file1 = Request.Files["ARTICLE_FILE_NAME_1"];
                var file2 = Request.Files["ARTICLE_FILE_NAME_2"];
                if (file1 != null && file1.ContentLength > 0)
                {
                    fileName1 = file1.FileName;
                    filePath1 = "/upload/file/" + GetNewFileName(fileName1);
                    file1.SaveAs(Path.Combine(Server.MapPath(filePath1)));
                }
                if (file2 != null && file2.ContentLength > 0)
                {
                    fileName2 = file2.FileName;
                    filePath2 = "/upload/file/" + GetNewFileName(fileName2);
                    file2.SaveAs(Path.Combine(Server.MapPath(filePath2)));
                }
                article.ARTICLE_FILE_NAME_1 = fileName1;
                article.ARTICLE_FILE_PATH_1 = filePath1;
                article.ARTICLE_FILE_NAME_2 = fileName2;
                article.ARTICLE_FILE_PATH_2 = filePath2;
                #endregion
                article.ARTICLE_CONTENT = article.ARTICLE_CONTENT;
                article.ARTICLE_RATE_COUNT = 0;
                article.ARTICLE_RATE_SUM_STAR = 0;
                article.ARTICLE_VIEW_TOTAL = 0;
                String date = Request["ARTICLE_CREATEDATE"].ToString();
                if (date + "" == "")
                {
                    article.ARTICLE_CREATEDATE = DateTime.Now;
                }
                else
                {
                    date = date + " " + DateTime.Now.ToString("hh", CultureInfo.InvariantCulture) + ":" + DateTime.Now.ToString("mm", CultureInfo.InvariantCulture) + ":" + DateTime.Now.ToString("ss", CultureInfo.InvariantCulture) + " " + DateTime.Now.ToString("tt", CultureInfo.InvariantCulture);
                    article.ARTICLE_CREATEDATE = DateTime.ParseExact(date, "dd/MM/yyyy hh:mm:ss tt", null);
                }
                article.ARTICLE_CREATEBY = UserManager.GetUserName;
                article.CATEGORY_ID = cateogryId;
                article.CATEGORY_NAME = category.CATEGORY_NAME;
                article.ARTICLE_TITLE = title;
                article.ARTICLE_ALIAS = alias;
                String categoryAlias = "";
                GetCategoryAlias(cateogryId, ref categoryAlias);
                article.CATEGORY_ALIAS = categoryAlias;
                db.ARTICLEs.Add(article);
                db.SaveChanges();
                ViewBag.categoryName = category.CATEGORY_NAME;

                return RedirectToAction("articelincategory/" + cateogryId);
            }
        }

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
            decimal articleID = decimal.Parse(id);
            ARTICLE article = db.ARTICLEs.Find(articleID);
            if (article == null)
            {
                TempData["message_fail"] = "Không tìm thấy bài viết!";
                return RedirectToAction("Index");
            }
            //xóa ảnh
            deleteFile(Path.Combine(Server.MapPath(article.ARTICLE_IMAGE_LARGE)));
            if (article.ARTICLE_IMAGE_LARGE + "" != "")
            {
                deleteFile(Path.Combine(Server.MapPath((article.ARTICLE_IMAGE_LARGE).Replace("images", "images1"))));
            }
            deleteFile(Path.Combine(Server.MapPath(article.ARTICLE_IMAGE_THUMB)));
            if (article.ARTICLE_IMAGE_THUMB + "" != "")
            {
                deleteFile(Path.Combine(Server.MapPath((article.ARTICLE_IMAGE_THUMB).Replace("Images", "Images1"))));
            }
            //xóa file
            deleteFile(Path.Combine(Server.MapPath(article.ARTICLE_FILE_PATH_1)));
            deleteFile(Path.Combine(Server.MapPath(article.ARTICLE_FILE_PATH_2)));
            String ac = "articelincategory/" + article.CATEGORY_ID.ToString();
            db.ARTICLEs.Remove(article);
            db.SaveChanges();
            TempData["message_success"] = "Xóa thành công!";
            return RedirectToAction(ac);
        }

        public ActionResult Edit(String id)
        {
            //if (!UserManager.Authenticated)
            //{
            //    return RedirectToAction("Login", "Admin");
            //}
            if (id != null)
            {
                decimal articleId = decimal.Parse(id);
                ARTICLE article = db.ARTICLEs.Find(articleId);
                if (article != null)
                {
                    article.ARTICLE_CONTENT = HttpUtility.HtmlDecode(article.ARTICLE_CONTENT);
                    ViewBag.ctId = article.CATEGORY_ID;
                    ViewBag.categoryName = article.CATEGORY_NAME;
                    return View(article);
                }
            }
            return RedirectToAction("/articelincategory/" + id);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(ARTICLE article, String id)
        {
            //if (!UserManager.Authenticated)
            //{
            //    return RedirectToAction("Login", "Admin");
            //}
            #region validate
            if (article.ARTICLE_TITLE + "" == "")
            {
                TempData["articleNameError"] = "Tiêu đề bài viết không được bỏ trống";
                return View(article);
            }
            if (article.ARTICLE_CONTENT + "" == "")
            {
                TempData["articleContentError"] = "Nội dung bài viết không được bỏ trống";
                return View(article);
            }
            #endregion
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            decimal aId = decimal.Parse(id);
            ARTICLE article_old = db.ARTICLEs.Where(x => x.ARTICLE_ID == aId).FirstOrDefault();
            if (article_old == null)
            {
                return RedirectToAction("Index");
            }
            //tìm alias
            if (article_old.ARTICLE_TITLE.Trim() != article.ARTICLE_TITLE.Trim())
            {
                String title = article.ARTICLE_TITLE.Trim();
                String alias = ru.SlugLink(article.ARTICLE_TITLE.Trim());
                int i = db.ARTICLEs.Where(x => x.CATEGORY_ID == article_old.CATEGORY_ID).Where(x => x.ARTICLE_TITLE.Trim() == article.ARTICLE_TITLE.Trim()).Count();
                if (i > 0)
                {
                    //alias = alias + "-" + i;
                    //title = title + " ("+i+")";
                    TempData["articleNameError"] = "Tiêu đề này đã tồn tại!";
                    return View(article);
                }
                article_old.ARTICLE_TITLE = title;
                article_old.ARTICLE_ALIAS = alias;
            }
            String date = Request["ARTICLE_CREATEDATE"].ToString();
            if (date == "")
            {
                article_old.ARTICLE_CREATEDATE = DateTime.Now;
            }
            else
            {
                date = date + " " + DateTime.Now.ToString("hh", CultureInfo.InvariantCulture) + ":" + DateTime.Now.ToString("mm", CultureInfo.InvariantCulture) + ":" + DateTime.Now.ToString("ss", CultureInfo.InvariantCulture) + " " + DateTime.Now.ToString("tt", CultureInfo.InvariantCulture);
                article_old.ARTICLE_CREATEDATE = DateTime.ParseExact(date, "dd/MM/yyyy hh:mm:ss tt", null);
            }
            #region ảnh
            String largeImagePath = article_old.ARTICLE_IMAGE_LARGE;
            String thumbImagePath = article_old.ARTICLE_IMAGE_THUMB;
            var largeImage = Request.Files["ARTICLE_IMAGE_LARGE"];
            if (largeImage != null && largeImage.ContentLength > 0)
            {
                String fileName = GetNewFileName(largeImage.FileName);
                //xóa file cũ
                deleteFile(Path.Combine(Server.MapPath(largeImagePath)));
                if (largeImagePath + "" != "")
                {
                    deleteFile(Path.Combine(Server.MapPath(largeImagePath.Replace("images", "images1"))));
                }
                //lưu mới
                largeImagePath = "/upload/images/" + fileName;
                largeImage.SaveAs(Path.Combine(Server.MapPath(largeImagePath)));
                //////lưu image croped dùng Jcrop.js
                ////string imgCropped = Request["imgCropped"];
                ////if (imgCropped + "" != "")
                ////{
                ////    byte[] bytes = Convert.FromBase64String(imgCropped.Split(',')[1]);
                ////    thumbImagePath = "/upload/_thumbs/Images/" + GetNewFileName(largeImage.FileName);
                ////    FileStream stream = new FileStream(Path.Combine(Server.MapPath(thumbImagePath)), System.IO.FileMode.Create);
                ////    stream.Write(bytes, 0, bytes.Length);
                ////    stream.Flush();
                ////    stream.Close();
                ////}
                //resize kích thước (229x172)
                Image image = Image.FromStream(largeImage.InputStream);
                String largeImagePath1 = "/upload/images1/" + fileName;
                Image thumb = ResizeImage(image, 229, 172, false);
                thumb.Save(Path.Combine(Server.MapPath(largeImagePath1)));
            }
            var thumbImage = Request.Files["ARTICLE_IMAGE_THUMB"];
            if (thumbImage != null && thumbImage.ContentLength > 0)
            {
                String fileName = GetNewFileName(thumbImage.FileName);
                deleteFile(Path.Combine(Server.MapPath(thumbImagePath)));
                if (thumbImagePath + "" != "")
                {
                    deleteFile(Path.Combine(Server.MapPath(thumbImagePath.Replace("Images", "Images1"))));
                }
                thumbImagePath = "/upload/_thumbs/Images/" + fileName;
                thumbImage.SaveAs(Path.Combine(Server.MapPath(thumbImagePath)));
                //resize kích thước (1)
                Image image = Image.FromStream(thumbImage.InputStream);
                String thumbImagePath1 = "/upload/_thumbs/Images1/" + fileName;
                Image thumb = ResizeImage(image, 149, 112, false);
                thumb.Save(Path.Combine(Server.MapPath(thumbImagePath1)));
            }
            article_old.ARTICLE_IMAGE_LARGE = largeImagePath;
            article_old.ARTICLE_IMAGE_THUMB = thumbImagePath;
            #endregion
            #region file
            String filePath1 = article_old.ARTICLE_FILE_PATH_1;
            String fileName1 = article_old.ARTICLE_FILE_NAME_1;
            String filePath2 = article_old.ARTICLE_FILE_PATH_2;
            String fileName2 = article_old.ARTICLE_FILE_NAME_2;
            var file1 = Request.Files["ARTICLE_FILE_NAME_1"];
            var file2 = Request.Files["ARTICLE_FILE_NAME_2"];
            if (file1 != null && file1.ContentLength > 0)
            {
                //Xóa file cũ
                deleteFile(Path.Combine(Server.MapPath(filePath1)));
                //Lưu file
                fileName1 = file1.FileName;
                filePath1 = "/upload/file/" + GetNewFileName(fileName1);
                file1.SaveAs(Path.Combine(Server.MapPath(filePath1)));
            }
            if (file2 != null && file2.ContentLength > 0)
            {
                //Xóa file cũ
                deleteFile(Path.Combine(Server.MapPath(filePath2)));
                //Lưu file
                fileName2 = file2.FileName;
                filePath2 = "/upload/file/" + GetNewFileName(fileName2);
                file2.SaveAs(Path.Combine(Server.MapPath(filePath2)));
            }
            article_old.ARTICLE_FILE_NAME_1 = fileName1;
            article_old.ARTICLE_FILE_PATH_1 = filePath1;
            article_old.ARTICLE_FILE_NAME_2 = fileName2;
            article_old.ARTICLE_FILE_PATH_2 = filePath2;
            #endregion
            article_old.ARTICLE_TITLE = article.ARTICLE_TITLE;
            article_old.ARTICLE_BRIEF = article.ARTICLE_BRIEF;
            article_old.ARTICLE_CONTENT = article.ARTICLE_CONTENT;
            article_old.ARTICLE_TAGS = article.ARTICLE_TAGS;
            article_old.ARTICLE_IS_HOT = article.ARTICLE_IS_HOT;
            article_old.ARTICLE_DISABLE = article.ARTICLE_DISABLE;
            db.SaveChanges();
            return RedirectToAction("articelincategory/" + article_old.CATEGORY_ID);
        }

        [HttpPost]
        public JsonResult deleteImage()
        {
            String id = Request["id"].ToString();
            String isThumb = Request["isThumb"].ToString();
            if (id + "" != "")
            {
                decimal articleId = decimal.Parse(id);
                ARTICLE article = db.ARTICLEs.Find(articleId);
                if (article != null)
                {
                    String fileImagePath = Path.Combine(Server.MapPath(article.ARTICLE_IMAGE_LARGE));
                    String fileThumbPath = Path.Combine(Server.MapPath(article.ARTICLE_IMAGE_THUMB));
                    if (isThumb == "large")
                    {
                        if (deleteFile(fileImagePath))
                        {
                            //Xóa trong csdl
                            deleteFile(fileImagePath.Replace("images", "images1"));
                            article.ARTICLE_IMAGE_LARGE = null;
                            db.SaveChanges();
                            return Json(new { message = "Xóa thành công" });
                        }
                    }
                    if (isThumb == "thumb")
                    {
                        if (deleteFile(fileThumbPath))
                        {
                            //Xóa trong csdl
                            deleteFile(fileThumbPath.Replace("Images", "Images1"));
                            article.ARTICLE_IMAGE_THUMB = null;
                            db.SaveChanges();
                            return Json(new { message = "Xóa thành công" });
                        }
                    }
                }
            }
            return null;
        }

        [HttpPost]
        public JsonResult deleteFile()
        {
            String id = Request["id"].ToString();
            String intFile = Request["intFile"].ToString();
            if (id + "" != "")
            {
                decimal articleId = decimal.Parse(id);
                ARTICLE article = db.ARTICLEs.Find(articleId);
                if (article != null)
                {
                    String fileFilePath1 = Path.Combine(Server.MapPath(article.ARTICLE_FILE_PATH_1));
                    String fileFilePath2 = Path.Combine(Server.MapPath(article.ARTICLE_FILE_PATH_2));
                    if (int.Parse(intFile) == 1)
                    {
                        if (deleteFile(fileFilePath1))
                        {
                            //Xóa trong csdl
                            article.ARTICLE_FILE_PATH_1 = null;
                            article.ARTICLE_FILE_NAME_1 = null;
                            db.SaveChanges();
                            return Json(new { message = "Xóa thành công" });
                        }
                    }
                    if (int.Parse(intFile) == 2)
                    {
                        if (deleteFile(fileFilePath2))
                        {
                            //Xóa trong csdl
                            article.ARTICLE_FILE_PATH_2 = null;
                            article.ARTICLE_FILE_NAME_2 = null;
                            db.SaveChanges();
                            return Json(new { message = "Xóa thành công" });
                        }
                    }
                }
            }
            return null;
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

        public void GetChildCategory(decimal id, ref string rs, int start)
        {
            string urlHome = "/";
            if (strUrl.Contains("duytan.edu.vn"))
            {
                urlHome = "http://duytan.edu.vn/xagan/";
            }
            else if (strPath != "/")
            {
                urlHome = strPath + urlHome;
            }
            start = start + 1;
            string fa_font = "";
            for (int i = 0; i < start; i++)
            {
                fa_font += "- ";
            }
            var childCategory = db.CATEGORies.Where(x => x.CATEGORY_ISMEDIA == false).Where(x => x.CATEGORY_PARENT_ID == id).ToList();
            if (childCategory != null && childCategory.Count() > 0)
            {
                foreach (var item in childCategory)
                {
                    int aCount = db.ARTICLEs.Where(x => x.CATEGORY_ID == item.CATEGORY_ID).Count();
                    rs += "<li><a href='" + urlHome + "admin/article/articelincategory/" + item.CATEGORY_ID + "' class='col-md-11'>" + fa_font + " " + item.CATEGORY_NAME + "<span title='Có " + aCount + " bài viết' class='pull-right badge bg-blue'>" + aCount + "</span></a><a class='col-md-1' title='Thêm bài viết' href='" + urlHome + "admin/article/create/" + item.CATEGORY_ID + "'><i class='fa fa-plus' style='color:green;'></i></a></li>";
                    GetChildCategory(item.CATEGORY_ID, ref rs, start);
                }
            }
        }

        public void GetCategoryAlias(decimal id, ref string rs)
        {
            var category = db.CATEGORies.Find(id);
            if (category != null)
            {
                if (category.CATEGORY_PARENT_ID != null)
                {
                    decimal ctID = decimal.Parse(category.CATEGORY_PARENT_ID.ToString());
                    GetCategoryAlias(ctID, ref rs);
                    rs += "/";
                }
            }
            rs += category.CATEGORY_ALIAS;
        }

        public static Image ResizeImage(Image imgPhoto, int width, int height, bool needToFill)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)width / (float)sourceWidth);
            nPercentH = ((float)height / (float)sourceHeight);
            if (!needToFill)
            {
                if (nPercentH < nPercentW)
                {
                    nPercent = nPercentH;
                }
                else
                {
                    nPercent = nPercentW;
                }
            }
            else
            {
                if (nPercentH > nPercentW)
                {
                    nPercent = nPercentH;
                    destX = (int)Math.Round((width -
                        (sourceWidth * nPercent)) / 2);
                }
                else
                {
                    nPercent = nPercentW;
                    destY = (int)Math.Round((height -
                        (sourceHeight * nPercent)) / 2);
                }
            }

            if (nPercent > 1)
                nPercent = 1;

            int destWidth = (int)Math.Round(sourceWidth * nPercent);
            int destHeight = (int)Math.Round(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(
                destWidth <= width ? destWidth : width,
                destHeight < height ? destHeight : height,
                              PixelFormat.Format32bppRgb);
            //bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
            //                 imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.White);
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //InterpolationMode.HighQualityBicubic;
            grPhoto.CompositingQuality = CompositingQuality.HighQuality;
            grPhoto.SmoothingMode = SmoothingMode.HighQuality;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }

        [HttpPost]
        public JsonResult Active()
        {
            decimal id = decimal.Parse(Request["id"].ToString());
            ARTICLE article = db.ARTICLEs.Find(id);
            if (article != null)
            {
                if ((bool)article.ARTICLE_DISABLE)
                {
                    article.ARTICLE_DISABLE = false;
                    db.SaveChanges();
                    return Json(new { message = 0 });
                }
                else
                {
                    article.ARTICLE_DISABLE = true;
                    db.SaveChanges();
                    return Json(new { message = 1 });
                }
            }
            return null;
        }

        public ActionResult toolResizeImage()
        {
            String path = Server.MapPath("/upload/images/");
            DirectoryInfo d = new DirectoryInfo(path);
            FileInfo[] Files = d.GetFiles("*.*");
            foreach (FileInfo file in Files)
            {
                Image image = Image.FromFile(file.FullName);
                String thumbImagePath1 = file.FullName.Replace("images", "images1");
                Image thumb = ResizeImage(image, 229, 172, false);
                thumb.Save(Path.Combine(thumbImagePath1));
            }
            return RedirectToAction("Index");
        }
        public ActionResult toolResizeImageThumb()
        {
            String path = Server.MapPath("/upload/_thumbs/Images/");
            DirectoryInfo d = new DirectoryInfo(path);
            FileInfo[] Files = d.GetFiles("*.*");
            foreach (FileInfo file in Files)
            {
                Image image = Image.FromFile(file.FullName);
                String thumbImagePath1 = file.FullName.Replace("Images", "Images1");
                Image thumb = ResizeImage(image, 149, 112, false);
                thumb.Save(Path.Combine(thumbImagePath1));
            }
            return RedirectToAction("Index");
        }
	}
}