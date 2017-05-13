using System;
using System.Data;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace Classes
{
    public class Valid
    {
        public Valid() { }

        public static string RemovePotentialCharInput2(string s)
        {
            var sb = new StringBuilder(s);
            sb.Replace("&lt;", "");
            sb.Replace("&gt;", "");
            sb.Replace("&#39;", "");
            sb.Replace("&quot;", "");
            sb.Replace("=", "");
            sb.Replace("select", "");
            sb.Replace("delete", "");
            sb.Replace("insert", "");
            sb.Replace("update", "");
            sb.Replace("file", "");
            sb.Replace("src", "");
            sb.Replace("copy", "");
            sb.Replace("move", "");
            sb.Replace("replace", "");
            sb.Replace("html", "");
            sb.Replace("body", "");
            sb.Replace("input", "");
            sb.Replace("type", "");
            sb.Replace("alert", "");
            sb.Replace("script", "");
            sb.Replace("%", "");
            sb.Replace("prompt", "");
            sb.Replace("(", "");
            sb.Replace(")", "");
            sb.Replace("\\", "");
            sb.Replace("&amp;", "");
            sb.Replace("&", "");
            sb.Replace(">", "");
            sb.Replace("<", "");
            sb.Replace("'", "");
            sb.Replace("\"", "");

            return sb.ToString();
        }

        public static string RemovePotentialCharInput(string s)
        {
            var sb = new StringBuilder(HttpUtility.HtmlEncode((s + "").ToLower()));
            sb.Replace("&lt;", "");
            sb.Replace("&gt;", "");
            sb.Replace("&#39;", "");
            sb.Replace("&quot;", "");
            sb.Replace("=", "");
            sb.Replace("select", "");
            sb.Replace("delete", "");
            sb.Replace("insert", "");
            sb.Replace("update", "");
            sb.Replace("file", "");
            sb.Replace("src", "");
            sb.Replace("copy", "");
            sb.Replace("move", "");
            sb.Replace("replace", "");
            sb.Replace("html", "");
            sb.Replace("body", "");
            sb.Replace("input", "");
            sb.Replace("type", "");
            sb.Replace("alert", "");
            sb.Replace("script", "");
            sb.Replace("%", "");
            sb.Replace("prompt", "");
            sb.Replace("(", "");
            sb.Replace(")", "");
            sb.Replace("\\", "");
            sb.Replace("&amp;", "");
            sb.Replace("&", "");
            sb.Replace(">", "");
            sb.Replace("<", "");
            sb.Replace("'", "");
            sb.Replace("\"", "");

            return sb.ToString();
        }
        public static string RemovePotentialCharUrl2(string s)
        {
            var sb = new StringBuilder(HttpUtility.HtmlEncode((s + "")));
            sb.Replace("&lt;", "");//>
            sb.Replace("&gt;", "");//<
            sb.Replace("&#39;", "");//'
            sb.Replace("&quot;", "");//"
            sb.Replace("select", "");
            sb.Replace("delete", "");
            sb.Replace("insert", "");
            sb.Replace("update", "");
            sb.Replace("file", "");
            sb.Replace("src", "");
            sb.Replace("copy", "");
            sb.Replace("move", "");
            sb.Replace("replace", "");
            sb.Replace("html", "");
            sb.Replace("body", "");
            sb.Replace("input", "");
            sb.Replace("alert", "");
            //sb.Replace("script", "");
            sb.Replace("prompt", "");
            sb.Replace("(", "");
            sb.Replace(")", "");
            sb.Replace("\\", "");
            sb.Replace(">", "");
            sb.Replace("<", "");
            sb.Replace("'", "");
            sb.Replace("\"", "");
            sb.Replace("&amp;", "&");

            return sb.ToString();
        }

        public static string RemovePotentialCharUrl(string s)
        {
            var sb = new StringBuilder(HttpUtility.HtmlEncode((s + "").ToLower()));
            sb.Replace("&lt;", "");//>
            sb.Replace("&gt;", "");//<
            sb.Replace("&#39;", "");//'
            sb.Replace("&quot;", "");//"
            sb.Replace("select", "");
            sb.Replace("delete", "");
            sb.Replace("insert", "");
            sb.Replace("update", "");
            sb.Replace("file", "");
            sb.Replace("src", "");
            sb.Replace("copy", "");
            sb.Replace("move", "");
            sb.Replace("replace", "");
            sb.Replace("html", "");
            sb.Replace("body", "");
            sb.Replace("input", "");
            sb.Replace("alert", "");
            //sb.Replace("script", "");
            sb.Replace("prompt", "");
            sb.Replace("(", "");
            sb.Replace(")", "");
            sb.Replace("\\", "");
            sb.Replace(">", "");
            sb.Replace("<", "");
            sb.Replace("'", "");
            sb.Replace("\"", "");
            sb.Replace("&amp;", "&");

            return sb.ToString();
        }

        public static byte[] SaveFileFromUrl(string url)
        {
            byte[] content;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                WebResponse response = request.GetResponse();

                Stream stream = response.GetResponseStream();

                using (BinaryReader br = new BinaryReader(stream))
                {
                    content = br.ReadBytes(500000);
                    br.Close();
                }
                response.Close();
            }
            catch (Exception ex)
            {
                return null;
            }
            return content;

            //FileStream fs = new FileStream(fileName, FileMode.Create);
            //BinaryWriter bw = new BinaryWriter(fs);
            //try
            //{
            //    bw.Write(content);
            //}
            //finally
            //{
            //    fs.Close();
            //    bw.Close();
            //}
        }

        private static char[] vnchars = {
        'à', 'á', 'ả', 'ã', 'ạ',
        'ă', 'ằ', 'ắ', 'ẳ', 'ẵ', 'ặ',
        'â', 'ầ', 'ấ', 'ẩ', 'ẫ', 'ậ',
        'đ', 'è', 'é', 'ẻ', 'ẽ', 'ẹ',
        'ê', 'ề', 'ế', 'ể', 'ễ', 'ệ',
        'ì', 'í', 'ỉ', 'ĩ', 'ị',
        'ò', 'ó', 'ỏ', 'õ', 'ọ',
        'ô', 'ồ', 'ố', 'ổ', 'ỗ', 'ộ',
        'ơ', 'ờ', 'ớ', 'ở', 'ỡ', 'ợ',
        'ù', 'ú', 'ủ', 'ũ', 'ụ',
        'ư', 'ừ', 'ứ', 'ử', 'ữ', 'ự',
        'ỳ', 'ý', 'ỷ', 'ỹ', 'ỵ',
        'Ă', 'Â', 'Đ', 'Ê', 'Ô', 'Ơ', 'Ư'
        };

        private static char[] unichars = {
        'a', 'a', 'a', 'a', 'a',
        'a', 'a', 'a', 'a', 'a', 'a',
        'a', 'a', 'a', 'a', 'a', 'a',
        'd', 'e', 'e', 'e', 'e', 'e',
        'e', 'e', 'e', 'e', 'e', 'e',
        'i', 'i', 'i', 'i', 'i',
        'o', 'o', 'o', 'o', 'o',
        'o', 'o', 'o', 'o', 'o', 'o',
        'o', 'o', 'o', 'o', 'o', 'o',
        'u', 'u', 'u', 'u', 'u',
        'u', 'u', 'u', 'u', 'u', 'u',
        'y', 'y', 'y', 'y', 'y',
        'A', 'A', 'D', 'E', 'O', 'O', 'U'
        };
        static String[] mang_a = {
            "á","à","ả","ã","ạ",
            "ă","ắ","ằ","ẳ","ẵ","ặ",
            "â","ấ","ầ","ẩ","ẫ","ậ",
            "ó","ò","ỏ","õ","ọ",
            "ô","ố","ồ","ổ","ỗ","ộ",
            "ơ","ớ","ờ","ở","ỡ","ợ",
            "é","è","ẻ","ẽ","ẹ",
            "ê","ế","ề","ể","ễ","ệ",
            "ú","ù","ủ","ũ","ụ",
            "ư","ứ","ừ","ử","ữ","ự",
            "í","ì","ỉ","ĩ","ị",
            "ý","ỳ","ỷ","ỹ","ỵ",
            "Á","À","Ả","Ã","Ạ",
            "Ă","Ắ","Ằ","Ẳ","Ẵ","Ặ",
            "Â","Ấ","Ầ","Ả","Ã","Ậ",
            "Ó","Ò","Ỏ","Õ","Ọ",
            "Ô","Ố","Ồ","Ổ","Ỗ","Ộ",
            "Ơ","Ớ","Ờ","Ở","Ỡ","Ợ",
            "É","È","Ẻ","Ẽ","Ẹ",
            "Ê","Ế","Ề","Ể","Ễ","Ệ",
            "Ú","Ù","Ủ","Ũ","Ụ",
            "Ư","Ứ","Ừ","Ử","Ữ","Ự",
            "Í","Ì","Ỉ","Ĩ","Ị",
            "Ý","Ỳ","Ỷ","Ỹ","Ỵ",
            "́","̀","̉","̃","̣"
        };
        static String[] mang_b = {
            "a", "a", "a", "a", "a",
            "a", "a", "a", "a", "a", "a",
            "a", "a", "a", "a", "a", "a",
            "o", "o", "o", "o", "o",
            "o", "o", "o", "o", "o", "o",
            "o", "o", "o", "o", "o", "o",
            "e", "e", "e", "e", "e",
            "e", "e", "e", "e", "e", "e",
            "u", "u", "u", "u", "u", 
            "u", "u", "u", "u", "u", "u", 
            "i", "i", "i", "i", "i", 
            "y", "y", "y", "y", "y",
            "a", "a", "a", "a", "a",
            "a", "a", "a", "a", "a", "a",
            "a", "a", "a", "a", "a", "a",
            "o", "o", "o", "o", "o",
            "o", "o", "o", "o", "o", "o",
            "o", "o", "o", "o", "o", "o",
            "e", "e", "e", "e", "e",
            "e", "e", "e", "e", "e", "e",
            "u", "u", "u", "u", "u", 
            "u", "u", "u", "u", "u", "u", 
            "i", "i", "i", "i", "i", 
            "y", "y", "y", "y", "y",
            "","","","",""
        };
        public static String VnToUnicode(String value)
        {
            string sOuput = "";
            bool check = false;
            for (int i = 0; i < value.Length; i++)
            {
                for (int j = 0; j < vnchars.Length; j++)
                {
                    if (value[i] == vnchars[j])
                    {
                        sOuput += unichars[j];
                        check = true;
                        break;
                    }
                }
                if (!check)
                {
                    sOuput += value[i] + "";
                }
                check = false;
            }

            sOuput = ToUnicodeToHop(sOuput);
            return sOuput;
        }

        private static String ToUnicodeToHop(String value)
        {
            String sOuput = "";
            for (int i = 0; i < value.Length; i++)
            {
                bool check = false;
                for (int j = 0; j < mang_a.Length; j++)
                {
                    if ((value[i] + "").Equals(mang_a[j]))
                    {
                        sOuput += mang_b[j];
                        check = true;
                        break;
                    }
                }
                if (!check)
                {
                    sOuput += value[i] + "";
                }
                check = false;
            }
            return sOuput;
        }

        public static string ToFileSize(int source)
        {
            return ToFileSize(Convert.ToInt64(source));
        }

        public static string ToFileSize(long source)
        {
            const int byteConversion = 1024;
            double bytes = Convert.ToDouble(source);

            if (bytes >= Math.Pow(byteConversion, 3)) //GB Range
            {
                return string.Concat(Math.Round(bytes / Math.Pow(byteConversion, 3), 2), " GB");
            }
            else if (bytes >= Math.Pow(byteConversion, 2)) //MB Range
            {
                return string.Concat(Math.Round(bytes / Math.Pow(byteConversion, 2), 2), " MB");
            }
            else if (bytes >= byteConversion) //KB Range
            {
                return string.Concat(Math.Round(bytes / byteConversion, 2), " KB");
            }
            else //Bytes
            {
                return string.Concat(bytes, " Bytes");
            }
        }

        public static string GetDomain(string url, string regex)
        {
            url = url.Replace("http://", "");
            string rt = "";
            int l = url.IndexOf(regex);
            int f = 0;

            if (l > f)
            {
                rt = url.Substring(0, l);
            }
            else
            {
                rt = url.Substring(0, url.Length);
            }

            return rt;
        }
    }
}
