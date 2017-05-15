using Booking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Classes;
using System.Net;
using System.Data.Entity;

namespace Booking.Controllers
{
    public class AdminAccountController : BaseController
    {
        //
        // GET: /AdminAccount/
        private DB_BOOKINGEntities db = new DB_BOOKINGEntities();
        public ActionResult Index()
        {
            //if (!UserManager.Authenticated)
            //{
            //    return RedirectToAction("Login", "Admin");
            //}
            var users = db.ACCOUNTs;
            return View(users.ToList());

        }

        // GET: /User/Details/5
        public ActionResult Details(decimal? id)
        {
            if ((id + "").Trim() == "")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (!ValidInput.validDecimal(id + ""))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ACCOUNT user = db.ACCOUNTs.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: /User/Create
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")] // This is for output cache false
        public ActionResult Create()
        {
            //if (!UserManager.Authenticated)
            //{
            //    return RedirectToAction("Login", "Admin");
            //}
            ViewBag.ROLE_ID = new SelectList(db.ROLEs, "ROLE_ID", "ROLE_NAME");
            return View();
        }

        // POST: /User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")] // This is for output cache false
        public ActionResult Create([Bind(Include = "USER_ID,USER_NAME,USER_FULL_NAME,USER_PASSWORD,USER_VALID_ADMIN,USER_IS_ADMIN,USER_CREATEDATE,USER_CREATEBY,USER_ALLOW_CATEGORY,USER_ALLOW_ARTICLE,USER_ALLOW_MEDIA,USER_ALLOW_USER,USER_ALLOW_HOTEL,USER_ALLOW_ROOM,USER_ALLOW_MEMBER,ROLE_ID,USER_ACTIVED")] ACCOUNT user)
        {
            if (ModelState.IsValid)
            {
                bool valid = true;
                if (user.USER_NAME + "" == "")
                {
                    AddError("error", "Chưa Nhập Tên đăng nhập.");
                    valid = false;
                }
                if (user.USER_PASSWORD + "" == "")
                {
                    AddError("error", "Chưa chọn Mật khẩu.");
                    valid = false;
                }
                else
                {
                    string confirm_pass = Request.Form["CONFIRM_PASS"] + "";
                    if (confirm_pass != user.USER_PASSWORD)
                    {
                        AddError("error", "Mật khẩu xác nhận không chính xác.");
                        valid = false;
                    }
                }
                if (user.USER_FULL_NAME + "" == "")
                {
                    AddError("error", "Chưa Nhập Họ tên.");
                    valid = false;
                }
                if (user.ROLE_ID + "" == "")
                {
                    AddError("error", "Chưa chọn Kiểu tài khoản.");
                    valid = false;
                }
                var checkExist = db.ACCOUNTs.Where(u => u.USER_NAME == user.USER_NAME).ToList();
                if (checkExist.Any())
                {
                    AddError("error", "Tên Đăng nhập này đã tồn tại. Vui lòng chọn Tên Đăng nhập khác.");
                    valid = false;
                }
                if (valid)
                {
                    decimal maxId = 0;
                    if (db.ACCOUNTs.Count() > 0) maxId = db.ACCOUNTs.Max(u => u.USER_ID);
                    user.USER_ID = maxId + 1;
                    string ps = Security.EncryptSha1(Security.EncryptMd5(user.USER_PASSWORD).ToLower());
                    user.USER_PASSWORD = ps;
                    if (user.ROLE_ID == 1)
                    {
                        user.USER_VALID_ADMIN = Security.EncryptMd5(user.USER_IS_ADMIN + "&" + user.USER_ID).ToLower();
                        user.USER_IS_ADMIN = true;
                    }
                    user.USER_CREATEBY = UserManager.GetUserId;
                    user.USER_CREATEDATE = DateTime.Now;

                    db.ACCOUNTs.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            ViewBag.ROLE_ID = new SelectList(db.ROLEs, "ROLE_ID", "ROLE_NAME", user.ROLE_ID);
            return View(user);
        }

        // GET: /User/Edit/5
        public ActionResult Edit(decimal? id)
        {
            //if (!UserManager.Authenticated)
            //{
            //    return RedirectToAction("Login", "Admin");
            //}
            if ((id + "").Trim() == "")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (!ValidInput.validDecimal(id + ""))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ACCOUNT user = db.ACCOUNTs.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserCreatedBy = db.ACCOUNTs.Where(u => u.USER_ID == user.USER_CREATEBY).ToList();
            ViewBag.ROLE_ID = new SelectList(db.ROLEs, "ROLE_ID", "ROLE_NAME", user.ROLE_ID);
            return View(user);
        }

        // POST: /User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "USER_ID,USER_NAME,USER_FULL_NAME,USER_PASSWORD,USER_VALID_ADMIN,USER_IS_ADMIN,USER_CREATEDATE,USER_CREATEBY,USER_ALLOW_CATEGORY,USER_ALLOW_ARTICLE,USER_ALLOW_MEDIA,USER_ALLOW_USER,USER_ALLOW_HOTEL,USER_ALLOW_ROOM,USER_ALLOW_MEMBER,ROLE_ID,USER_ACTIVED")] ACCOUNT user)
        {
            ViewBag.AllowChangePass = (Request.Form["AllowChangePass"] == "on" ? true : false).ToString().ToLower();
            if (ModelState.IsValid)
            {
                bool valid = true;
                if (user.USER_NAME + "" == "")
                {
                    AddError("error", "Chưa Nhập Tên đăng nhập.");
                    valid = false;
                }
                if (user.USER_PASSWORD + "" == "")
                {
                    AddError("error", "Chưa chọn Mật khẩu.");
                    valid = false;
                }
                if (user.USER_FULL_NAME + "" == "")
                {
                    AddError("error", "Chưa Nhập Họ tên.");
                    valid = false;
                }
                if (user.ROLE_ID + "" == "")
                {
                    AddError("error", "Chưa chọn Vai trò.");
                    valid = false;
                }
                if (valid)
                {
                    db.Entry(user).State = EntityState.Modified;
                    if (Request.Form["AllowChangePass"] == "on" ? true : false)
                    {
                        string ps = Security.EncryptSha1(Security.EncryptMd5(user.USER_PASSWORD).ToLower());
                        user.USER_PASSWORD = ps;
                        db.Entry(user).Property("USER_PASSWORD").IsModified = true;
                    }
                    else
                    {
                        db.Entry(user).Property("USER_PASSWORD").IsModified = false;
                    }
                    user.USER_VALID_ADMIN = Security.EncryptMd5(user.USER_IS_ADMIN + "&" + user.USER_ID).ToLower();
                    user.USER_CREATEBY = UserManager.GetUserId;
                    db.SaveChanges();

                    ViewBag.UserCreatedBy = db.ACCOUNTs.Where(u => u.USER_ID == UserManager.GetUserId).ToList();

                    return RedirectToAction("Index");
                }
            }
            ViewBag.UserCreatedBy = db.ACCOUNTs.Where(u => u.USER_ID == user.USER_CREATEBY).ToList();
            ViewBag.ROLE_ID = new SelectList(db.ROLEs, "ROLE_ID", "ROLE_NAME", user.ROLE_ID);
            return View(user);
        }

        // GET: /User/Delete/5
        public ActionResult Delete(decimal id)
        {
            //if (!UserManager.Authenticated)
            //{
            //    return RedirectToAction("Login", "Admin");
            //}
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ACCOUNT user = db.ACCOUNTs.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: /User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            ACCOUNT user = db.ACCOUNTs.Find(id);
            db.ACCOUNTs.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}