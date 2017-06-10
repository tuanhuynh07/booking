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
using System.IO;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace Booking.Controllers.Admin
{
    public class HotelController : Controller
    {
        private DB_BOOKINGEntities db = new DB_BOOKINGEntities();

        // GET: /Hotel/
        public async Task<ActionResult> Index()
        {
            return View(await db.HOTELs.ToListAsync());
        }

        // GET: /Hotel/Details/5
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

        // GET: /Hotel/Create
        public ActionResult Create()
        {
            ViewBag.Language = db.LANGUAGEs;
            return View();
        }

        // POST: /Hotel/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Exclude = "")] HOTEL hotel)
        {
            ViewBag.Language = db.LANGUAGEs;
            if (ModelState.IsValid)
            {
                hotel.HOTEL_ID = (db.HOTELs.Max(h => h.HOTEL_ID) ?? 0) + 1;
                var maxTRID = (db.TRANSLATION_HOTEL.Max(k => k.ID) ?? 0);
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
                hotel.TRANSLATION_HOTEL = new List<TRANSLATION_HOTEL>();
                var count = 1;
                foreach (var item in ViewBag.Language)
                {
                    if (count != item.LANGUAGE_ID)
                    {
                        TRANSLATION_HOTEL a = new TRANSLATION_HOTEL();
                        a.ID = ++maxTRID;
                        a.HOTEL_ID = (decimal)hotel.HOTEL_ID;
                        a.LANGUAGE_ID = item.LANGUAGE_ID;
                        a.NAME = Request.Form["TRANSLATION_HOTEL[" + item.LANGUAGE_ID + "].NAME"];
                        a.ADDRESS = Request.Form["TRANSLATION_HOTEL[" + item.LANGUAGE_ID + "].ADDRESS"];
                        a.BRIEF = Request.Form["TRANSLATION_HOTEL[" + item.LANGUAGE_ID + "].BRIEF"];
                        a.DESCRIPTION = Request.Form["TRANSLATION_HOTEL[" + item.LANGUAGE_ID + "].DESCRIPTION"];
                        hotel.TRANSLATION_HOTEL.Add(a);
                    }
                }

                db.HOTELs.Add(hotel);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Language = db.LANGUAGEs;
            return View(hotel);
        }

        // GET: /Hotel/Edit/5
        public async Task<ActionResult> Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HOTEL hotel = await db.HOTELs.Where(h => h.HOTEL_ID == id).Include(x => x.TRANSLATION_HOTEL).FirstOrDefaultAsync(); ;
            if (hotel == null)
            {
                return HttpNotFound();
            }
            ViewBag.Language = db.LANGUAGEs;
            return View(hotel);
        }

        // POST: /Hotel/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Exclude = "")] HOTEL hotel)
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
                #endregion
                foreach (var item in hotel.TRANSLATION_HOTEL)
                {
                    db.Entry(item).State = EntityState.Modified;
                }

                db.Entry(hotel).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(hotel);
        }

        // GET: /Hotel/Delete/5
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

        // POST: /Hotel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(decimal id)
        {
            HOTEL hotel = await db.HOTELs.Where(x => x.HOTEL_ID == id).Include(h => h.TRANSLATION_HOTEL).FirstOrDefaultAsync();
            foreach (var item in hotel.TRANSLATION_HOTEL.ToList())
            {
                db.TRANSLATION_HOTEL.Remove(item);
            }
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
