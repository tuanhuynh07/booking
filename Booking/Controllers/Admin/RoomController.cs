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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Booking.Controllers.Admin
{
    public class RoomController : Controller
    {
        private DB_BOOKINGEntities db = new DB_BOOKINGEntities();

        // GET: /Room/
        public async Task<ActionResult> Index()
        {
            var rooms = db.ROOMs.Include(r => r.HOTEL).Include(r => r.ROOM_TYPE);
            return View(await rooms.ToListAsync());
        }

        // GET: /Room/Details/5
        public async Task<ActionResult> Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ROOM room = await db.ROOMs.FindAsync(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        // GET: /Room/Create
        public ActionResult Create()
        {
            ViewBag.Language = db.LANGUAGEs;
            ViewBag.HOTEL_ID = new SelectList(db.HOTELs, "HOTEL_ID", "HOTEL_NAME");
            ViewBag.ROOM_TYPE_ID = new SelectList(db.ROOM_TYPE, "ROOM_TYPE_ID", "ROOM_TYPE_NAME");
            return View();
        }

        // POST: /Room/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Exclude = "")] ROOM room)
        {
            ViewBag.Language = db.LANGUAGEs;
            if (ModelState.IsValid)
            {
                room.ROOM_ID = (db.ROOMs.Max(h => h.ROOM_ID) ?? 0) + 1;
                var maxTRID = (db.TRANSLATION_ROOM.Max(k => k.ID) ?? 0);
                #region //Lưu ảnh
                String thumbImagePath = null;
                var thumbImage = Request.Files["ROOM_IMAGE"];
                if (thumbImage != null && thumbImage.ContentLength > 0)
                {
                    String fileName = GetNewFileName(thumbImage.FileName);
                    thumbImagePath = "/Upload/images/" + fileName;
                    thumbImage.SaveAs(Path.Combine(Server.MapPath(thumbImagePath)));
                    //resize kích thước (149x112)
                    Image image = Image.FromStream(thumbImage.InputStream);
                    String thumbImagePath1 = "/Upload/images/thumbs/" + fileName;
                    Image thumb = ResizeImage(image, 149, 112, false);
                    thumb.Save(Path.Combine(Server.MapPath(thumbImagePath1)));
                }
                room.ROOM_IMAGE = thumbImagePath;
                #endregion
                room.TRANSLATION_ROOM = new List<TRANSLATION_ROOM>();
                var count = 1;
                foreach (var item in ViewBag.Language)
                {
                    if (count != item.LANGUAGE_ID)
                    {
                        room.TRANSLATION_ROOM.Add(new TRANSLATION_ROOM { ID = ++maxTRID, LANGUAGE_ID = item.LANGUAGE_ID, ROOM_ID = (decimal)room.ROOM_ID, ROOM_NAME = Request.Form["TRANSLATION_ROOM[" + item.LANGUAGE_ID + "].ROOM_NAME"] });
                    }
                }
                db.ROOMs.Add(room);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.HOTEL_ID = new SelectList(db.HOTELs, "HOTEL_ID", "HOTEL_NAME", room.HOTEL_ID);
            ViewBag.ROOM_TYPE_ID = new SelectList(db.ROOM_TYPE, "ROOM_TYPE_ID", "ROOM_TYPE_NAME", room.ROOM_TYPE_ID);
            return View(room);
        }


        // GET: /Room/Edit/5
        public async Task<ActionResult> Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ROOM room = await db.ROOMs.Where(h => h.ROOM_ID == id).Include(x => x.TRANSLATION_ROOM).FirstOrDefaultAsync();
            if (room == null)
            {
                return HttpNotFound();
            }
            ViewBag.Language = db.LANGUAGEs;
            ViewBag.HOTEL_ID = new SelectList(db.HOTELs, "HOTEL_ID", "HOTEL_NAME", room.HOTEL_ID);
            ViewBag.ROOM_TYPE_ID = new SelectList(db.ROOM_TYPE, "ROOM_TYPE_ID", "ROOM_TYPE_NAME", room.ROOM_TYPE_ID);
            return View(room);
        }

        // POST: /Room/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Exclude = "")] ROOM room)
        {
            if (ModelState.IsValid)
            {
                #region //Lưu ảnh
                String thumbImagePath = null;
                var thumbImage = Request.Files["ROOM_IMAGE"];
                if (thumbImage != null && thumbImage.ContentLength > 0)
                {
                    String fileName = GetNewFileName(thumbImage.FileName);
                    thumbImagePath = "/Upload/images/" + fileName;
                    thumbImage.SaveAs(Path.Combine(Server.MapPath(thumbImagePath)));
                    //resize kích thước (149x112)
                    Image image = Image.FromStream(thumbImage.InputStream);
                    String thumbImagePath1 = "/Upload/images/thumbs/" + fileName;
                    Image thumb = ResizeImage(image, 149, 112, false);
                    thumb.Save(Path.Combine(Server.MapPath(thumbImagePath1)));
                }
                room.ROOM_IMAGE = thumbImagePath;
                #endregion
                foreach (var item in room.TRANSLATION_ROOM)
                {
                    db.Entry(item).State = EntityState.Modified;
                }
                db.Entry(room).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.HOTEL_ID = new SelectList(db.HOTELs, "HOTEL_ID", "HOTEL_NAME", room.HOTEL_ID);
            ViewBag.ROOM_TYPE_ID = new SelectList(db.ROOM_TYPE, "ROOM_TYPE_ID", "ROOM_TYPE_NAME", room.ROOM_TYPE_ID);
            return View(room);
        }

        // GET: /Room/Delete/5
        public async Task<ActionResult> Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ROOM room = await db.ROOMs.FindAsync(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        // POST: /Room/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(decimal id)
        {
            ROOM room = await db.ROOMs.Where(x => x.ROOM_ID == id).Include(h => h.TRANSLATION_ROOM).FirstOrDefaultAsync();
            foreach (var item in room.TRANSLATION_ROOM.ToList())
            {
                db.TRANSLATION_ROOM.Remove(item);
            }
            db.ROOMs.Remove(room);
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
    }
}
