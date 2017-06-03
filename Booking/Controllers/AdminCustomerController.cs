using Booking.Models;
using Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace Booking.Controllers
{
    public class AdminCustomerController : Controller
    {
        private DB_BOOKINGEntities db = new DB_BOOKINGEntities();
        // GET: /AdminCustomer/
        public ActionResult Index(string currentFilter, string searchString, int? page)
        {
            if (!UserManager.Authenticated || !UserManager.RoleController("AdminCustomer"))
            {
                return RedirectToAction("Index", "Admin");
            }
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;

            List<BOOKING_ROOM> articles = new List<BOOKING_ROOM>();
            foreach (BOOKING_ROOM a in db.BOOKING_ROOM.OrderByDescending(x => x.DATE_BOOKING).ToList())
            {
                var ls = new BOOKING_ROOM();
                ls.BOOKING_CODE = a.BOOKING_CODE;
                ls.CUSTOMER_ID = a.CUSTOMER_ID;
                ls.CUSTOMER_FULLNAME = a.CUSTOMER_FULLNAME;
                ls.DATE_BEGIN = a.DATE_BEGIN;
                ls.DATE_BOOKING = a.DATE_BOOKING;
                ls.DATE_FINISH = a.DATE_FINISH;
                ls.NUMBER_NIGHT = a.NUMBER_NIGHT;
                ls.NUMBER_ROOM = a.NUMBER_ROOM;
                ls.PAY_CODE = a.PAY_CODE;
                ls.PAY_DATE = a.PAY_DATE;
                ls.PAY_INFORMATION = a.PAY_INFORMATION;
                if(a.PAY_STATUS.Value)
                {
                    string strscript = Security.EncryptMd5(a.PAY_STATUS.Value + "PAY" + a.BOOKING_CODE).ToLower();
                    if (a.PAY_STATUS_SCRIPT == strscript) a.PAY_STATUS = true;
                    else a.PAY_STATUS = false;
                }
                else a.PAY_STATUS = false;
                ls.PAY_LIMIT = a.PAY_LIMIT;
                ls.PAY_STATUS = a.PAY_STATUS;
                ls.PAY_TYPE = a.PAY_TYPE;
                ls.CANCEL_BOOKING = a.CANCEL_BOOKING;
                if (a.CANCEL_BOOKING.HasValue)
                {
                    if(a.CANCEL_BOOKING.Value)
                    {
                        string strscript = Security.EncryptMd5(a.CANCEL_BOOKING.Value + "cancel" + a.BOOKING_CODE).ToLower();
                        if (a.CANCEL_BOOKING_SCRIPT == strscript) ls.CANCEL_BOOKING = true;
                        else ls.CANCEL_BOOKING = false;
                    }
                    else ls.CANCEL_BOOKING = false;
                    
                }
                else ls.CANCEL_BOOKING = false;
                ls.CANCEL_BOOKING_DATE = a.CANCEL_BOOKING_DATE;
                ls.CANCEL_BOOKING_PERSON = a.CANCEL_BOOKING_PERSON;
                ls.CANCEL_BOOKING_SCRIPT = a.CANCEL_BOOKING_SCRIPT;
                articles.Add(ls);
            }
            if (!String.IsNullOrEmpty(searchString))
            {
                articles = db.BOOKING_ROOM.Where(x => x.BOOKING_CODE.Contains(searchString) || x.CUSTOMER_FULLNAME.Contains(searchString)).ToList();
            }
            int pageSize = 15;
            int currentPage = (page ?? 1);
            ViewBag.currentPage = currentPage;
            ViewBag.pageSize = pageSize;
            return View(articles.ToPagedList(currentPage, pageSize));

        }
        public ActionResult Information(string currentFilter, string searchString, int? page)
        {
            if (!UserManager.Authenticated || !UserManager.RoleController("AdminCustomer"))
            {
                return RedirectToAction("Index", "Admin");
            }
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;
            List<CUSTOMER> articles = new List<CUSTOMER>();            
            if (!String.IsNullOrEmpty(searchString))
            {
                articles = db.CUSTOMERs.Where(x => x.FIRST_NAME.Contains(searchString) || x.LAST_NAME.Contains(searchString)).ToList();
            }
            else
            {
                articles = db.CUSTOMERs.OrderBy(x => x.FIRST_NAME).ToList();
            }
            int pageSize = 15;
            int currentPage = (page ?? 1);
            ViewBag.currentPage = currentPage;
            ViewBag.pageSize = pageSize;
            return View(articles.ToPagedList(currentPage, pageSize));
        }
        public ActionResult InformationDetail(decimal id)
        {
            if (!UserManager.Authenticated || !UserManager.RoleController("AdminCustomer"))
            {
                return RedirectToAction("Index", "Admin");
            }           
            var articles = db.CUSTOMERs.Where(x => x.CUSTOMER_ID==id);  
            if(articles!=null && articles.Count()>0)
            {
                return View(articles.First());
            }
            else return RedirectToAction("Infomation");
        }
        public ActionResult Member(string currentFilter, string searchString, int? page)
        {
            if (!UserManager.Authenticated || !UserManager.RoleController("AdminCustomer"))
            {
                return RedirectToAction("Index", "Admin");
            }
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;
            List<MEMBER> articles = new List<MEMBER>();
            if (!String.IsNullOrEmpty(searchString))
            {
                articles = db.MEMBERs.Where(x => x.MEMBER_USERNAME.Contains(searchString) || x.FIRST_NAME.Contains(searchString)).ToList();
            }
            else
            {
                articles = db.MEMBERs.OrderBy(x => x.MEMBER_USERNAME).ToList();
            }
            int pageSize = 15;
            int currentPage = (page ?? 1);
            ViewBag.currentPage = currentPage;
            ViewBag.pageSize = pageSize;
            return View(articles.ToPagedList(currentPage, pageSize));
        }
        public ActionResult Comment(string currentFilter, string searchString, int? page)
        {
            if (!UserManager.Authenticated || !UserManager.RoleController("AdminCustomer"))
            {
                return RedirectToAction("Index", "Admin");
            }
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;
            List<COMMENT> articles = new List<COMMENT>();
            if (!String.IsNullOrEmpty(searchString))
            {
                articles = db.COMMENTs.Where(x => x.MESSEGER.Contains(searchString)).ToList();
            }
            else
            {
                articles = db.COMMENTs.OrderByDescending(x => x.CREATE_DATE).ToList();
            }
            int pageSize = 15;
            int currentPage = (page ?? 1);
            ViewBag.currentPage = currentPage;
            ViewBag.pageSize = pageSize;
            return View(articles.ToPagedList(currentPage, pageSize));
        }
        public ActionResult RoomBooking(string id)
        {
            if (!UserManager.Authenticated || !UserManager.RoleController("AdminCustomer"))
            {
                return RedirectToAction("Index", "Admin");
            }
            ViewBag.bookroomcode = id;
            var a = db.BOOKING_ROOM_DETAIL.Where(m=>m.BOOKING_CODE==id).OrderByDescending(x => x.CUSTOMER_ID).ToList();
            if (a != null && a.Count() > 0) return View(a);
            else return View(new List<BOOKING_ROOM_DETAIL>());
        }
        public ActionResult CancelBooking(String id)
        {
            if (!UserManager.Authenticated || !UserManager.RoleController("AdminCustomer"))
            {
                return RedirectToAction("Index", "Admin");
            }
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            BOOKING_ROOM article = db.BOOKING_ROOM.Find(id);
            if (article == null)
            {
                TempData["message_fail"] = "Lỗi: Không tìm thấy dữ liệu!";
                return RedirectToAction("Index");
            }
            article.CANCEL_BOOKING = true;
            article.CANCEL_BOOKING_DATE = DateTime.Now;
            article.CANCEL_BOOKING_PERSON = UserManager.GetUserName;
            article.CANCEL_BOOKING_SCRIPT = Security.EncryptMd5(true+"cancel" + article.BOOKING_CODE).ToLower();
            db.SaveChanges();
            TempData["message_success"] = "Hủy đăng ký thành công";
            return RedirectToAction("Index");
        }

        public ActionResult ActivePay(String id)
        {
            if (!UserManager.Authenticated || !UserManager.RoleController("AdminCustomer"))
            {
                return RedirectToAction("Index", "Admin");
            }
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                BOOKING_ROOM article = db.BOOKING_ROOM.Find(id);
                if (article != null)
                {
                    ViewBag.ctId = article.BOOKING_CODE;
                    return View(article);
                }
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ActivePay(BOOKING_ROOM booking,String id)
        {
            if (!UserManager.Authenticated || !UserManager.RoleController("AdminCustomer"))
            {
                return RedirectToAction("Index", "Admin");
            }
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                #region validate
                if (booking.PAY_CODE + "" == "")
                {
                    TempData["articleNameError"] = "Phải nhập mã thanh toán";
                    return View(booking);
                }
                if (booking.PAY_INFORMATION + "" == "")
                {
                    TempData["articleNameError"] = "Phải nhập thông tin thanh toán";
                    return View(booking);
                }
                #endregion               
                BOOKING_ROOM article = db.BOOKING_ROOM.Find(id);
                if(article==null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    article.PAY_CODE = booking.PAY_CODE;
                    article.PAY_DATE = booking.PAY_DATE;
                    article.PAY_INFORMATION = booking.PAY_INFORMATION+" (Được xác nhận bởi tài khoản hệ thống: "+ UserManager.GetUserName+")";
                    article.PAY_STATUS = true;
                    article.PAY_STATUS_SCRIPT = Security.EncryptMd5(true + "PAY" + article.BOOKING_CODE).ToLower();
                    db.SaveChanges();
                    return View("Index");
                }
            }           
        }
	}
}