using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Classes
{
    public class Utililies
    {
        public DateTime FirstDayOfMonth(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1);
        }
        public DateTime LastDayOfMonth(DateTime dateTime)
        {
            DateTime firstDayOfTheMonth = new DateTime(dateTime.Year, dateTime.Month, 1);
            return firstDayOfTheMonth.AddMonths(1).AddDays(-1);
        }

        public static bool GetStatus(object input)
        {
            if (input == null) return false;
            if (input.ToString().ToLower() == "true") return true;
            else if (input.ToString().ToLower() == "false") return false;
            else if (input.ToString().ToLower() == "1") return true;
            else if (input.ToString().ToLower() == "0") return false;
            return false;
        }

        public static string GetStatusIcon(object input)
        {
            if (GetStatus(input))
            {
                return "<img src='../../../Content/Admin/images/bullet_tick.png' />";
            }
            else return "<img src='../../../Content/Admin/images/bullet_stop.png' />";
            
        }

        public static string GetCurrentUrl()
        {
            return HttpContext.Current.Request.Url.AbsoluteUri;
        }
    }
}