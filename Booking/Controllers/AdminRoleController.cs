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
            //if (!UserManager.Authenticated)
            //{
            //    return RedirectToAction("Login", "Admin");
            //}
            return View(db.ROLEs.ToList());
        }

        // GET: /Role/Details/5
        public ActionResult Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ROLE role = db.ROLEs.Find(id);

            //var allCategoriesList = db.CATEGORies.ToList();
            //ViewBag.CategoriesSelected = db.ROLEs.Include(i => i.CATEGORies).Where(i => i.ROLE_ID == id).ToList();
            //ViewBag.CategoriesAll = allCategoriesList;
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }
        //private ICollection<ROLE_CATEGORY_RELATIONSHIP> PopulateCoursesData(decimal rID)
        //{
        //    var rc = db.ROLEs.Include(i => i.CATEGORies).Where(i => i.ROLE_ID == rID).ToList().Select(cc => cc.CATEGORies);
        //    var viewModel = new List<ROLE_CATEGORY_RELATIONSHIP>();
        //    foreach (CATEGORY item in rc.Single())
        //    {
        //        viewModel.Add(new ROLE_CATEGORY_RELATIONSHIP
        //        {
        //            ROLE_ID = rID,
        //            CATEGORY_ID = item.CATEGORY_ID,
        //            //CATEGORY_NAME = item.CATEGORY_NAME
        //        });
        //    }
        //    return viewModel;
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(string ROLE_ID, string hidCateSelected)
        {
            if (ModelState.IsValid)
            {
                decimal roleId = decimal.Parse(ROLE_ID);
                //ROLE_CATEGORY_RELATIONSHIP rc = new ROLE_CATEGORY_RELATIONSHIP();
                //rc.CATEGORY_ID = roleId;
                //rc.CATEGORY_ID = 9;
                //db.ROLE_CATEGORY_RELATIONSHIPs.Add(rc);
                //db.SaveChanges();
                //ViewBag.CategoriesSelected = db.ROLEs.Include(i => i.CATEGORies).Where(i => i.ROLE_ID == roleId).ToList();
                //return RedirectToAction("Index");
                //db.SaveChanges();
            }

            return RedirectToAction("Details");
        }

        // GET: /Role/Create
        public ActionResult Create()
        {
            if (!UserManager.Authenticated)
            {
                return RedirectToAction("Login", "Admin");
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
                    AddError("error", "Chưa Nhập Tên vai trò.");
                    valid = false;
                }
                if (valid)
                {
                    db.ROLEs.Add(role);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(role);
        }

        // GET: /Role/Edit/5
        public ActionResult Edit(decimal id)
        {
            if (!UserManager.Authenticated)
            {
                return RedirectToAction("Login", "Admin");
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
                    AddError("error", "Chưa Nhập Tên vai trò.");
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
            if (!UserManager.Authenticated)
            {
                return RedirectToAction("Login", "Admin");
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