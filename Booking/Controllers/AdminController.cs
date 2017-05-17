using Booking.Models;
using Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Booking.Controllers
{
    public class AdminController : Controller
    {
        private DB_BOOKINGEntities db = new DB_BOOKINGEntities();
        public ActionResult Index()
        {
            if (UserManager.Authenticated)
            {
                    decimal clist = db.HOTELs.Count();
                    ViewBag.NumberOfHotels = clist;
                    var listHotels = db.HOTELs.Take(20).OrderByDescending(a => a.HOTEL_CREATEDATE).ToList();
                    ViewBag.listhotels = listHotels;
                    return View();
            }
            else
            {
                HttpCookie authCookie = System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                if (authCookie != null) authCookie.Expires = DateTime.Now.AddDays(-1);
                System.Web.Security.FormsAuthentication.SignOut();
                Session.Abandon();
                Session.Clear();
                Session.RemoveAll();
                return RedirectToAction("Login");
            }           
        }
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")] // This is for output cache false
        public ActionResult Login()
        {
            if (UserManager.Authenticated)
            {
                HttpCookie authCookie =
                    System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                if (authCookie != null) authCookie.Expires = DateTime.Now.AddDays(-1);
                System.Web.Security.FormsAuthentication.SignOut();
                Session.Abandon();
                Session.Clear();
                Session.RemoveAll();
            }
            return View();
        }
        // POST: /User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")] // This is for output cache false
        public ActionResult Login([Bind(Include = "USER_NAME,USER_PASSWORD")] ACCOUNT user)
        {
            if (ModelState.IsValid)
            {
                if (user.USER_NAME + "" == "" || user.USER_PASSWORD + "" == "")
                {
                    ModelState.AddModelError("error", "Nhập đầy đủ tài khoản và mật khẩu để đăng nhập.");
                }
                else
                {
                    string ps = Security.EncryptSha1(Security.EncryptMd5(user.USER_PASSWORD).ToLower());
                    var login = from u in db.ACCOUNTs
                                where u.USER_NAME == user.USER_NAME && u.USER_PASSWORD == ps && u.USER_ACTIVED == true
                                select u;
                    if (login.Any())
                    {
                        string ss = Security.EncryptSha1(Security.EncryptMd5(login.Single().USER_NAME + "#" + login.Single().USER_PASSWORD).ToLower());
                        Session["UsernameSystem"] = ss;
                        this.Session.Timeout = 60;

                        string data = Security.EncryptStringCbc(login.Single().USER_NAME + ";" + login.Single().USER_ID, "system");
                        HttpCookie authCookie = FormsAuthentication.GetAuthCookie(data, false);
                        var ticket = FormsAuthentication.Decrypt(authCookie.Value);

                        if (ticket != null)
                        {
                            var newTicket = new FormsAuthenticationTicket(ticket.Version, ticket.Name,
                                                                          ticket.IssueDate, ticket.Expiration,
                                                                          ticket.IsPersistent, "");

                            //Update the authCookie's Value to use the encrypted version of newTicket. 
                            authCookie.Value = FormsAuthentication.Encrypt(newTicket);
                        }
                        authCookie.Expires = DateTime.Now.AddMinutes(60);
                        Response.Cookies.Add(authCookie);

                        return RedirectToAction("Index", "Admin");
                    }
                    else
                    {
                        ModelState.AddModelError("error", "Tài khoản hoặc mật khẩu không chính xác.");
                    }
                }
            }
            return View();
        }
        public ActionResult Logout()
        {
            HttpCookie authCookie = System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null) authCookie.Expires = DateTime.Now.AddDays(-1);
            System.Web.Security.FormsAuthentication.SignOut();
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            return RedirectToAction("Login");
        }       
    }
}