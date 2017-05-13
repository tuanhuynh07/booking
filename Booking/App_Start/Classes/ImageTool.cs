using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Classes
{
    public class ImageTool
    {
        public ArrayList FetchLinksFromSource(string htmlSource)
        {
            ArrayList links = new ArrayList();
            string regexImgSrc = @"<img[^>]*?src\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>";
            MatchCollection matchesImgSrc = Regex.Matches(htmlSource, regexImgSrc, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            foreach (Match m in matchesImgSrc)
            {
                string href = m.Groups[1].Value;
                links.Add(href);
            }
            return links;
        }
        public string GetImageUrlFromContent(string source, ref string fimgname)
        {
            string temp = source;
            string s2 = "";
            if (temp.Length > 0)
            {
                int i = temp.IndexOf("<img");
                if (i >= 0)
                {
                    string s = temp.Substring(i, temp.Length - i);
                    int j = s.IndexOf(">");
                    if (j >= 0)
                    {
                        s2 = s.Substring(0, j);
                        int k = s2.IndexOf("src=");
                        if (k >= 0)
                        {
                            string s3 = s2.Substring(k + 5, s2.Length - (k + 5));
                            string ext = "";
                            int t = GetIndex(s3, ref ext);
                            if (t >= 0)
                            {
                                string sss = s3.Substring(0, t) + ext;
                                fimgname = Guid.NewGuid() + "." + ext;
                                return sss;
                            }
                        }
                    }
                }

            }
            return s2;
        }
        private static int GetIndex(string s, ref string ext)
        {
            string[] ss = new string[] { "png", "jpeg", "jpg", "gif", "tif", "bmp" };
            foreach (var s1 in ss)
            {
                int i = s.LastIndexOf(s1);
                if (i >= 0)
                {
                    ext = s1;
                    return i;
                }
            }
            return -1;
        }
    }
}