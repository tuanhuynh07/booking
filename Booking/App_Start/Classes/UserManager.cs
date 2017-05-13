using System;
using System.Data;
using System.Configuration;
using System.Data.Entity;
//using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Booking.Models;

//using System.Xml.Linq;


namespace Classes
{
    public static class UserManager
    {
        private static DB_BOOKINGEntities db = new DB_BOOKINGEntities();
        public static decimal totalOnlineUser = 25;
        //public static decimal totalHitcount = 0;
        public static bool Authenticated
        {
            get
            {
                Identification iden = Identification.GetInstance();
                return iden.IsAuthenticated;
            }
        }

        public static string GetUserName
        {
            get
            {
                Identification iden = Identification.GetInstance();
                return iden.GetUserName();
            }
        }
        public static decimal GetUserId
        {
            get
            {
                Identification iden = Identification.GetInstance();
                return iden.GetUserId();
            }
        }

        public static decimal OnlineUser
        {
            get { return totalOnlineUser; }            
        }
        public static void UpdateTotalHitCount()
        {
            try
            {
                //if (db.HitCounters == null) return;
                //HitCounter hc = db.HitCounters.Find(1);
                //if (hc != null)
                //{
                //    int cur = hc.SumNumber + "" == "" ? 0 : int.Parse(hc.SumNumber + "");
                //    db.Entry(hc).State = EntityState.Modified;
                //    db.Entry(hc).Property("SumNumber").IsModified = true;
                //    hc.SumNumber = cur + 1;
                //    db.SaveChanges();
                //}
            }
            catch { }
        }
        public static int GetTotalHitCount()
        {
            //if (db.HitCounters == null) return 1453589;
            //HitCounter hc = db.HitCounters.Find(1);
            //if (hc != null)
            //{
            //    int cur = hc.SumNumber + "" == "" ? 0 : int.Parse(hc.SumNumber + "");
            //    return cur;
            //}
            return 0;
        }
    }
}
