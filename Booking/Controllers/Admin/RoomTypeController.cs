using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Booking.Models;

namespace Booking.Controllers.Admin
{
    public class RoomTypeController : Controller
    {
        private DB_BOOKINGEntities db = new DB_BOOKINGEntities();

        // GET: /RoomType/
        public async Task<ActionResult> Index()
        {
            return View(await db.ROOM_TYPE.ToListAsync());
        }

        // GET: /RoomType/Details/5
        public async Task<ActionResult> Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ROOM_TYPE room_type = await db.ROOM_TYPE.FindAsync(id);
            if (room_type == null)
            {
                return HttpNotFound();
            }
            return View(room_type);
        }

        // GET: /RoomType/Create
        public ActionResult Create()
        {
            ViewBag.Language = db.LANGUAGEs;
            return View();
        }

        // POST: /RoomType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Exclude="")] ROOM_TYPE room_type)
        {
            ViewBag.Language = db.LANGUAGEs;
            if (ModelState.IsValid)
            {
                room_type.ROOM_TYPE_ID = (db.ROOM_TYPE.Max(h => h.ROOM_TYPE_ID) ?? 0) + 1;
                var maxTRID = (db.TRANSLATION_ROOM_TYPE.Max(k => k.ID) ?? 0);
                room_type.TRANSLATION_ROOM_TYPE = new List<TRANSLATION_ROOM_TYPE>();
                var count = 1;
                foreach (var item in ViewBag.Language)
                {
                    if (count != item.LANGUAGE_ID)
                    {
                        room_type.TRANSLATION_ROOM_TYPE.Add(new TRANSLATION_ROOM_TYPE { ID = ++maxTRID, LANGUAGE_ID = item.LANGUAGE_ID, ROOM_TYPE_ID = room_type.ROOM_TYPE_ID, ROOM_TYPE_NAME = Request.Form["TRANSLATION_ROOM_TYPE[" + item.LANGUAGE_ID + "].ROOM_TYPE_NAME"] });
                    }
                }
                db.ROOM_TYPE.Add(room_type);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            
            return View(room_type);
        }

        // GET: /RoomType/Edit/5
        public async Task<ActionResult> Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ROOM_TYPE room_type = await db.ROOM_TYPE.Where(x=>x.ROOM_TYPE_ID==id).Include(y=>y.TRANSLATION_ROOM_TYPE).FirstOrDefaultAsync();
            if (room_type == null)
            {
                return HttpNotFound();
            }
            ViewBag.Language = db.LANGUAGEs;
            return View(room_type);
        }

        // POST: /RoomType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Exclude="")] ROOM_TYPE room_type)
        {
            if (ModelState.IsValid)
            {
                foreach (var item in room_type.TRANSLATION_ROOM_TYPE)
                {
                    db.Entry(item).State = EntityState.Modified;
                }
                db.Entry(room_type).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Language = db.LANGUAGEs;
            return View(room_type);
        }

        // GET: /RoomType/Delete/5
        public async Task<ActionResult> Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ROOM_TYPE room_type = await db.ROOM_TYPE.FindAsync(id);
            if (room_type == null)
            {
                return HttpNotFound();
            }
            return View(room_type);
        }

        // POST: /RoomType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(decimal id)
        {
            ROOM_TYPE room_type = await db.ROOM_TYPE.Where(x => x.ROOM_TYPE_ID == id).Include(h => h.TRANSLATION_ROOM_TYPE).FirstOrDefaultAsync();
            foreach (var item in room_type.TRANSLATION_ROOM_TYPE.ToList())
            {
                db.TRANSLATION_ROOM_TYPE.Remove(item);
            }
            db.ROOM_TYPE.Remove(room_type);
            await db.SaveChangesAsync();
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
