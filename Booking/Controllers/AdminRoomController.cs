using Booking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Booking.Controllers
{
    public class AdminRoomController : Controller
    {
        private DB_BOOKINGEntities db = new DB_BOOKINGEntities();
        
        public ActionResult Index()
        {
            return View();
        }
	}
}