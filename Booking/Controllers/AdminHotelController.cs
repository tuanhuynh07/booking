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
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace Booking.Controllers
{
    public class AdminHotelController : Controller
    {
        private DB_BOOKINGEntities db = new DB_BOOKINGEntities();

        // GET: /AdminHotel/
        public async Task<ActionResult> Index()
        {
            var hotels = db.HOTELs.Include(h => h.TRANSLATION_HOTEL).Include(h => h.TRANSLATION_HOTEL1).Include(h => h.TRANSLATION_HOTEL2).Include(h => h.TRANSLATION_HOTEL3);
            return View(await hotels.ToListAsync());
        }

        // GET: /AdminHotel/Details/5
        public async Task<ActionResult> Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HOTEL hotel = await db.HOTELs.FindAsync(id);
            if (hotel == null)
            {
                return HttpNotFound();
            }
            return View(hotel);
        }

        // GET: /AdminHotel/Create
        public ActionResult Create()
        {
            ViewBag.NAME_TRANSLATION_ID = new SelectList(db.TRANSLATION_HOTEL, "ID", "TEXT");
            ViewBag.ADDRESS_TRANSLATION_ID = new SelectList(db.TRANSLATION_HOTEL, "ID", "TEXT");
            ViewBag.DESCRIPTION_TRANSLATION_ID = new SelectList(db.TRANSLATION_HOTEL, "ID", "TEXT");
            ViewBag.BRIEF_TRANSLATION_ID = new SelectList(db.TRANSLATION_HOTEL, "ID", "TEXT");
            ViewBag.Language = db.LANGUAGEs;//new SelectList(db.LANGUAGEs, "LANGUAGE_ID", "LANGUAGE_NAME"); ;
            return View();
        }

        // POST: /AdminHotel/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Exclude = "HOTEL_ID,HOTEL_CREATEDATE")] HOTEL hotel)
        {
            if (ModelState.IsValid)
            {
                #region //Lưu ảnh
                String thumbImagePath = null;
                var thumbImage = Request.Files["HOTEL_IMAGE"];
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
                hotel.HOTEL_IMAGE = thumbImagePath;
                hotel.HOTEL_CREATEDATE = DateTime.Now;
                #endregion
                hotel.HOTEL_ID = (db.HOTELs.Max(h => h.HOTEL_ID) ?? 0) + 1;
                db.HOTELs.Add(hotel);
                var maxTRID = (db.HOTELs.Max(h => h.HOTEL_ID) ?? 0) + 1;
                hotel.TRANSLATION_HOTEL.ID = maxTRID;
                hotel.TRANSLATION_HOTEL.LANGUAGE_ID = 2;
                hotel.TRANSLATION_HOTEL1.ID = maxTRID + 1;
                hotel.TRANSLATION_HOTEL1.LANGUAGE_ID = 2;
                hotel.TRANSLATION_HOTEL2.ID = maxTRID + 2;
                hotel.TRANSLATION_HOTEL2.LANGUAGE_ID = 2;
                hotel.TRANSLATION_HOTEL3.ID = maxTRID + 3;
                hotel.TRANSLATION_HOTEL3.LANGUAGE_ID = 2;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.NAME_TRANSLATION_ID = new SelectList(db.TRANSLATION_HOTEL, "ID", "TEXT", hotel.NAME_TRANSLATION_ID);
            ViewBag.ADDRESS_TRANSLATION_ID = new SelectList(db.TRANSLATION_HOTEL, "ID", "TEXT", hotel.ADDRESS_TRANSLATION_ID);
            ViewBag.DESCRIPTION_TRANSLATION_ID = new SelectList(db.TRANSLATION_HOTEL, "ID", "TEXT", hotel.DESCRIPTION_TRANSLATION_ID);
            ViewBag.BRIEF_TRANSLATION_ID = new SelectList(db.TRANSLATION_HOTEL, "ID", "TEXT", hotel.BRIEF_TRANSLATION_ID);
            ViewBag.Language = db.LANGUAGEs;
            return View(hotel);
        }

        // GET: /AdminHotel/Edit/5
        public async Task<ActionResult> Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HOTEL hotel = await db.HOTELs.FindAsync(id);
            if (hotel == null)
            {
                return HttpNotFound();
            }
            ViewBag.NAME_TRANSLATION_ID = new SelectList(db.TRANSLATION_HOTEL, "ID", "TEXT", hotel.NAME_TRANSLATION_ID);
            ViewBag.ADDRESS_TRANSLATION_ID = new SelectList(db.TRANSLATION_HOTEL, "ID", "TEXT", hotel.ADDRESS_TRANSLATION_ID);
            ViewBag.DESCRIPTION_TRANSLATION_ID = new SelectList(db.TRANSLATION_HOTEL, "ID", "TEXT", hotel.DESCRIPTION_TRANSLATION_ID);
            ViewBag.BRIEF_TRANSLATION_ID = new SelectList(db.TRANSLATION_HOTEL, "ID", "TEXT", hotel.BRIEF_TRANSLATION_ID);
            return View(hotel);
        }

        // POST: /AdminHotel/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "HOTEL_ID,HOTEL_NAME,HOTEL_ADDRESS,HOTEL_CREATEDATE,HOTEL_DESCRIPTION,HOTEL_BRIEF,NUMBER_RATING,TOTAL_RATING,TOTAL_ROOM,PRICE_GENERAL,HOTEL_STAR,HOTEL_LEVEL,HOTEL_STATUS,HOTEL_IMAGE,MEDIA_ARRAY,HOTEL_CHECKIN,HOTEL_MAP,NAME_TRANSLATION_ID,ADDRESS_TRANSLATION_ID,DESCRIPTION_TRANSLATION_ID,BRIEF_TRANSLATION_ID,HOTEL_ALIAS")] HOTEL hotel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hotel).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.NAME_TRANSLATION_ID = new SelectList(db.TRANSLATION_HOTEL, "ID", "TEXT", hotel.NAME_TRANSLATION_ID);
            ViewBag.ADDRESS_TRANSLATION_ID = new SelectList(db.TRANSLATION_HOTEL, "ID", "TEXT", hotel.ADDRESS_TRANSLATION_ID);
            ViewBag.DESCRIPTION_TRANSLATION_ID = new SelectList(db.TRANSLATION_HOTEL, "ID", "TEXT", hotel.DESCRIPTION_TRANSLATION_ID);
            ViewBag.BRIEF_TRANSLATION_ID = new SelectList(db.TRANSLATION_HOTEL, "ID", "TEXT", hotel.BRIEF_TRANSLATION_ID);
            return View(hotel);
        }

        // GET: /AdminHotel/Delete/5
        public async Task<ActionResult> Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HOTEL hotel = await db.HOTELs.FindAsync(id);
            if (hotel == null)
            {
                return HttpNotFound();
            }
            return View(hotel);
        }

        // POST: /AdminHotel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(decimal id)
        {
            HOTEL hotel = await db.HOTELs.FindAsync(id);
            db.HOTELs.Remove(hotel);
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
