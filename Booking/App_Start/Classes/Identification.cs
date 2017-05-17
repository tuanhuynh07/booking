using System;
using System.Data;
using System.Configuration;
//using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
//using System.Xml.Linq;


namespace Classes
{
    public sealed class Identification : Page
    {        
        private static string UserData = "";
        private static Identification _manager;
        private Identification()
        {
            UserData = Security.DecryptStringCbc(HttpContext.Current.User.Identity.Name + "", "system");
        }
        public static Identification GetInstance()
        {
            lock (typeof(Identification))
            {
                _manager = new Identification();
            }
            return _manager;
        }
        public bool Authenticated
        {
            get
            {
                return HttpContext.Current.User.Identity.IsAuthenticated;
            }
        }
        public bool IsAuthenticated
        {
            get { return this.Authenticated; }
        }
        public string GetUserName()
        {
            var data = UserData.Split(';');
            if (data.Length > 0)
            {
                return data[0];
            }
            return "";
        }
        public decimal GetUserId()
        {
            var data = UserData.Split(';');
            if (data.Length > 1)
            {
                return decimal.Parse(data[1]);
            }
            return -1;
        }
    }
}
