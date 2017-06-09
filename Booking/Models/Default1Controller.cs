using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Booking.Models
{
    public class Default1Controller : Controller
    {
        private DB_BOOKINGEntities db = new DB_BOOKINGEntities();

        // GET: /Default1/
        public async Task<ActionResult> Index()
        {
            var translation_hotel = db.TRANSLATION_HOTEL.Include(t => t.HOTEL).Include(t => t.LANGUAGE);
            return View(await translation_hotel.ToListAsync());
        }

        // GET: /Default1/Details/5
        public async Task<ActionResult> Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TRANSLATION_HOTEL translation_hotel = await db.TRANSLATION_HOTEL.FindAsync(id);
            if (translation_hotel == null)
            {
                return HttpNotFound();
            }
            return View(translation_hotel);
        }

        // GET: /Default1/Create
        public ActionResult Create()
        {
            ViewBag.HOTEL_ID = new SelectList(db.HOTELs, "HOTEL_ID", "HOTEL_NAME");
            ViewBag.LANGUAGE_ID = new SelectList(db.LANGUAGEs, "LANGUAGE_ID", "LANGUAGE_NAME");
            return View();
        }

        // POST: /Default1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="ID,LANGUAGE_ID,NAME,HOTEL_ID,BRIEF,ADDRESS,DESCRIPTION")] TRANSLATION_HOTEL translation_hotel)
        {
            if (ModelState.IsValid)
            {
                db.TRANSLATION_HOTEL.Add(translation_hotel);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.HOTEL_ID = new SelectList(db.HOTELs, "HOTEL_ID", "HOTEL_NAME", translation_hotel.HOTEL_ID);
            ViewBag.LANGUAGE_ID = new SelectList(db.LANGUAGEs, "LANGUAGE_ID", "LANGUAGE_NAME", translation_hotel.LANGUAGE_ID);
            return View(translation_hotel);
        }

        // GET: /Default1/Edit/5
        public async Task<ActionResult> Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TRANSLATION_HOTEL translation_hotel = await db.TRANSLATION_HOTEL.FindAsync(id);
            if (translation_hotel == null)
            {
                return HttpNotFound();
            }
            ViewBag.HOTEL_ID = new SelectList(db.HOTELs, "HOTEL_ID", "HOTEL_NAME", translation_hotel.HOTEL_ID);
            ViewBag.LANGUAGE_ID = new SelectList(db.LANGUAGEs, "LANGUAGE_ID", "LANGUAGE_NAME", translation_hotel.LANGUAGE_ID);
            return View(translation_hotel);
        }

        // POST: /Default1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="ID,LANGUAGE_ID,NAME,HOTEL_ID,BRIEF,ADDRESS,DESCRIPTION")] TRANSLATION_HOTEL translation_hotel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(translation_hotel).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.HOTEL_ID = new SelectList(db.HOTELs, "HOTEL_ID", "HOTEL_NAME", translation_hotel.HOTEL_ID);
            ViewBag.LANGUAGE_ID = new SelectList(db.LANGUAGEs, "LANGUAGE_ID", "LANGUAGE_NAME", translation_hotel.LANGUAGE_ID);
            return View(translation_hotel);
        }

        // GET: /Default1/Delete/5
        public async Task<ActionResult> Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TRANSLATION_HOTEL translation_hotel = await db.TRANSLATION_HOTEL.FindAsync(id);
            if (translation_hotel == null)
            {
                return HttpNotFound();
            }
            return View(translation_hotel);
        }

        // POST: /Default1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(decimal id)
        {
            TRANSLATION_HOTEL translation_hotel = await db.TRANSLATION_HOTEL.FindAsync(id);
            db.TRANSLATION_HOTEL.Remove(translation_hotel);
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
