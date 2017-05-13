using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Classes
{
    public class RewriteUrl
    {
        public string SlugLink(String content)
        {
            if (content.Length > 0)
            {
                content = content.ToLower();
                content = content.Replace(",", "");
                content = content.Replace("#", "");
                content = content.Replace("@", "");
                content = content.Replace("$", "");
                content = content.Replace("%", "");
                content = content.Replace("^", "");
                content = content.Replace("(", "");
                content = content.Replace(")", "");
                content = content.Replace("_", "");
                content = content.Replace("=", "");
                content = content.Replace(">", "");
                content = content.Replace("<", "");
                content = content.Replace("?", "");
                content = content.Replace("|", "");
                content = content.Replace("[", "");
                content = content.Replace("]", "");
                content = content.Replace("{", "");
                content = content.Replace("}", "");
                content = content.Replace("`", "");
                content = content.Replace(" ", "-");
                content = content.Replace("!", "");
                content = content.Replace("\"", "");
                content = content.Replace("'", "");
                content = content.Replace("&", "-");
                content = content.Replace("&quot;", "");
                content = content.Replace(";", "");
                content = content.Replace(".", "");
                content = content.Replace("/", "");
                content = content.Replace(":", "");
                content = content.Replace("+", "");
                content = content.Replace("*", "");
                content = content.Replace("&amp;", "");
                content = content.Replace("?", "");
                content = content.Replace("æ", "ae");
                content = content.Replace("ø", "oe");
                content = content.Replace("å", "aa");
                content = content.Replace("á", "a");
                content = content.Replace("à", "a");
                content = content.Replace("ả", "a");
                content = content.Replace("ã", "a");
                content = content.Replace("ạ", "a");
                content = content.Replace("ă", "a");
                content = content.Replace("ắ", "a");
                content = content.Replace("ằ", "a");
                content = content.Replace("ẳ", "a");
                content = content.Replace("ẵ", "a");
                content = content.Replace("ặ", "a");
                content = content.Replace("â", "a");
                content = content.Replace("ấ", "a");
                content = content.Replace("ầ", "a");
                content = content.Replace("ẩ", "a");
                content = content.Replace("ẫ", "a");
                content = content.Replace("ậ", "a");
                content = content.Replace("đ", "d");
                content = content.Replace("é", "e");
                content = content.Replace("è", "e");
                content = content.Replace("ẻ", "e");
                content = content.Replace("ẽ", "e");
                content = content.Replace("ẹ", "e");
                content = content.Replace("ê", "e");
                content = content.Replace("ế", "e");
                content = content.Replace("ề", "e");
                content = content.Replace("ệ", "e");
                content = content.Replace("ễ", "e");
                content = content.Replace("ể", "e");
                content = content.Replace("í", "i");
                content = content.Replace("ì", "i");
                content = content.Replace("ỉ", "i");
                content = content.Replace("ĩ", "i");
                content = content.Replace("ị", "i");
                content = content.Replace("ó", "o");
                content = content.Replace("ò", "o");
                content = content.Replace("ỏ", "o");
                content = content.Replace("õ", "o");
                content = content.Replace("ọ", "o");
                content = content.Replace("ô", "o");
                content = content.Replace("ố", "o");
                content = content.Replace("ồ", "o");
                content = content.Replace("ổ", "o");
                content = content.Replace("ỗ", "o");
                content = content.Replace("ộ", "o");
                content = content.Replace("ơ", "o");
                content = content.Replace("ớ", "o");
                content = content.Replace("ờ", "o");
                content = content.Replace("ở", "o");
                content = content.Replace("ỡ", "o");
                content = content.Replace("ợ", "o");
                content = content.Replace("ú", "u");
                content = content.Replace("ù", "u");
                content = content.Replace("ũ", "u");
                content = content.Replace("ủ", "u");
                content = content.Replace("ụ", "u");
                content = content.Replace("ư", "u");
                content = content.Replace("ứ", "u");
                content = content.Replace("ừ", "u");
                content = content.Replace("ữ", "u");
                content = content.Replace("ử", "u");
                content = content.Replace("ự", "u");
                content = content.Replace("ý", "y");
                content = content.Replace("ỳ", "y");
                content = content.Replace("ỷ", "y");
                content = content.Replace("ỹ", "y");
                content = content.Replace("ỵ", "y");
                content = Regex.Replace(content, @"-+", @"-");
            }
            return content;
        }
    }
}