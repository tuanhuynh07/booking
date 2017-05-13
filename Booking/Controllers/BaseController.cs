using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Booking.Controllers
{
    public class BaseController : Controller
    {
        private int _countErrer = 0;
        public void AddError(string key, string message)
        {
            ModelState.AddModelError(key, message);
            _countErrer++;
            ViewBag.CountError = _countErrer;
        }
    }
}