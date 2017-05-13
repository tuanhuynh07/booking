using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;

namespace Classes
{
    public class Log : Page
    {
        private string sLogFormat;
        private string sErrorTime;
        public Log()
        {
            sLogFormat = DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ==> ";

            string sYear = DateTime.Now.Year.ToString();
            string sMonth = DateTime.Now.Month.ToString().Length == 1 ? "0" + DateTime.Now.Month : DateTime.Now.Month + "";
            string sDay = DateTime.Now.Day.ToString().Length == 1 ? "0" + DateTime.Now.Day : DateTime.Now.Day + "";
            sErrorTime = sYear + "_" + sMonth + "_" + sDay;
        }
        public void WriteLog(string sPathName, string sErrMsg)
        {
            StreamWriter sw = new StreamWriter(sPathName + "logs.txt", true);
            sw.WriteLine(sLogFormat + " " + sErrMsg);
            sw.Flush();
            sw.Close();
        }
        public void ErrorLogGlobal(string sPathName, string sErrMsg, string ext)
        {
            string temp = sLogFormat.Replace(" ", "");
            string bookMark = temp.Substring(0, temp.IndexOf('='));

            StreamWriter sw = new StreamWriter(sPathName + sErrorTime + "." + ext, true);
            sw.WriteLine("<a name='" + bookMark + "'>" + sLogFormat + "</a>" + sErrMsg);
            sw.Flush();
            sw.Close();

            //em.SendMailCertValid("nguyentrongthanh@duytan.edu.vn", "MyDTU", "MyDTU send errors", sErrMsg + GenUrlToErrorFile(sPathName + sErrorTime + "." + ext, bookMark));
        }
        private string GenUrlToErrorFile(string path, string bookMark)
        {
            //Response.Write("<br/> " + HttpContext.Current.Request.Url.Host);
            //Response.Write("<br/> " + HttpContext.Current.Request.Url.Authority);
            //Response.Write("<br/> " + HttpContext.Current.Request.Url.AbsolutePath);
            //Response.Write("<br/> " + HttpContext.Current.Request.ApplicationPath);
            //Response.Write("<br/> " + HttpContext.Current.Request.Url.AbsoluteUri);
            //Response.Write("<br/> " + HttpContext.Current.Request.Url.PathAndQuery);
            //==>
            //localhost
            //localhost:60527
            //WebSite1test/Default2.aspx
            //WebSite1test
            //http://localhost:60527/WebSite1test/Default2.aspx?QueryString1=1&QuerrString2=2
            //WebSite1test/Default2.aspx?QueryString1=1&QuerrString2=2 

            int idx = path.IndexOf("Logs", System.StringComparison.Ordinal);
            int idx2 = HttpContext.Current.Request.Url.AbsoluteUri.IndexOf("://", System.StringComparison.Ordinal);
            string protocol = HttpContext.Current.Request.Url.AbsoluteUri.Substring(0, idx2);

            string absolutePath = path.Substring(idx, path.Length - idx).Replace("\\", "/");
            string link = HttpContext.Current.Request.Url.Authority + "/" + absolutePath + "#" + bookMark;
            return "<a href='" + protocol + "://" + link + "'>" + "Click here to view error detail: " + link + "</a>";
        }        
        public static string GetValueByKeyName(string keyName)
        {
            var configMap = new ExeConfigurationFileMap();
            configMap.ExeConfigFilename = HttpContext.Current.Server.MapPath("~/portal.config");
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
            KeyValueConfigurationCollection appSettings = config.AppSettings.Settings;

            if (appSettings.Count > 0)
            {
                return appSettings[keyName].Value;
            }
            return "";
        }        
    }
}
