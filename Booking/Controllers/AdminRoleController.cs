using Booking.Models;
using Classes;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Booking.Controllers
{
    public class AdminRoleController : BaseController
    {
        private DB_BOOKINGEntities db = new DB_BOOKINGEntities();
        public ActionResult Index()
        {
            if (!UserManager.Authenticated || !UserManager.RoleController("AdminRole"))
            {
                return RedirectToAction("Index", "Admin");
            }
            return View(db.ROLEs.ToList());
        }

        // GET: /Role/Details/5
        public ActionResult Permissions(decimal id)
        {
            if (!UserManager.Authenticated || !UserManager.RoleController("AdminRole"))
            {
                return RedirectToAction("Index", "Admin");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ROLE role = db.ROLEs.Find(id);
            var allCategoriesList = db.CATEGORies.ToList();
            ViewBag.CategoriesAll = allCategoriesList;
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Permissions(string ROLE_ID)
        {
            decimal roleId = decimal.Parse(ROLE_ID);
            var getRole = db.ROLEs.Where(i => i.ROLE_ID == roleId).FirstOrDefault();
            if (ModelState.IsValid)
            {               
                var allCategoriesList = db.CATEGORies.ToList();
                foreach(var item in allCategoriesList)
                {
                    string idCat = Request.Form[item.CATEGORY_ID+""] + "";
                    if(idCat!="false")
                    {
                        if (!getRole.CATEGORies.Where(m => m.CATEGORY_ID == item.CATEGORY_ID).Any())
                        {
                            getRole.CATEGORies.Add(item);
                        }
                    }
                    else
                    {
                        if (getRole.CATEGORies.Where(m => m.CATEGORY_ID == item.CATEGORY_ID).Any())
                        {
                            getRole.CATEGORies.Remove(item);
                        }
                    }
                    db.SaveChanges();
                }                
                return RedirectToAction("Index");                                
            }
            return View(getRole);
        }

        // GET: /Role/Create
        public ActionResult Create()
        {
            if (!UserManager.Authenticated || !UserManager.RoleController("AdminRole"))
            {
                return RedirectToAction("Index", "Admin");
            }
            return View();
        }

        // POST: /Role/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ROLE_ID,ROLE_NAME,ROLE_ALLOW_ADD,ROLE_ALLOW_UPDATE,ROLE_ALLOW_DELETE,ROLE_ALLOW_VIEW")] ROLE role)
        {
            if (ModelState.IsValid)
            {
                bool valid = true;
                if (role.ROLE_NAME + "" == "")
                {
                    AddError("error", "Chưa Nhập Tên kiểu.");
                    valid = false;
                }
                if (valid)
                {
                    db.ROLEs.Add(role);
                    db.SaveChanges();
                    //update Danh mục
                    var allCategoriesList = db.CATEGORies.ToList();
                    foreach (var item in allCategoriesList)
                    {
                        role.CATEGORies.Add(item);
                    }
                    db.SaveChanges();
                    //end
                    return RedirectToAction("Index");
                }
            }

            return View(role);
        }

        // GET: /Role/Edit/5
        public ActionResult Edit(decimal id)
        {
            if (!UserManager.Authenticated || !UserManager.RoleController("AdminRole"))
            {
                return RedirectToAction("Index", "Admin");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ROLE role = db.ROLEs.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        // POST: /Role/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ROLE_ID,ROLE_NAME,ROLE_ALLOW_ADD,ROLE_ALLOW_UPDATE,ROLE_ALLOW_DELETE,ROLE_ALLOW_VIEW")] ROLE role)
        {
            if (ModelState.IsValid)
            {
                bool valid = true;
                if (role.ROLE_NAME + "" == "")
                {
                    AddError("error", "Chưa Nhập Tên kiểu.");
                    valid = false;
                }
                if (valid)
                {
                    db.Entry(role).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(role);
        }

        // GET: /Role/Delete/5
        public ActionResult Delete(decimal id)
        {
            if (!UserManager.Authenticated || !UserManager.RoleController("AdminRole"))
            {
                return RedirectToAction("Index", "Admin");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ROLE role = db.ROLEs.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        // POST: /Role/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            ROLE role = db.ROLEs.Find(id);
            db.ROLEs.Remove(role);
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