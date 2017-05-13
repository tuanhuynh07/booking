using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Classes
{
    public class ValidInput
    {
        public static bool validUSZipCode(string yournumber)
        {
            string pattern = @"^\d{5}$";
            Match match = Regex.Match(yournumber, pattern, RegexOptions.IgnoreCase);

            if (match.Success)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// valid your email enter
        /// </summary>
        /// <param name="email"></param>
        /// <returns>true or false</returns>
        public static bool validEmail(string email)
        {
            //string pattern = @"^[a-z][a-z|0-9|]*([_][a-z|0-9]+)*([.][a-z|" +
            //   @"0-9]+([_][a-z|0-9]+)*)?@[a-z][a-z|0-9|]*\.([a-z]" +
            //   @"[a-z|0-9]*(\.[a-z][a-z|0-9]*)?)$";
            string pattern = @"^[a-z][a-z|0-9|]*([-][a-z|0-9]+)*([_][a-z|0-9]+)*([.][a-z|" +
               @"0-9]+([_][a-z|0-9]+)*)?@[a-z][a-z|0-9|]*\.([a-z]" +
               @"[a-z|0-9]*(\.[a-z][a-z|0-9]*)?)$";
            Match match = Regex.Match(email, pattern, RegexOptions.IgnoreCase);

            if (match.Success) return true;
            return false;
        }
        public static bool validPositiveNumber(string yournumber)
        {
            string pattern = @"^[0-9]{1,}$";
            Match match = Regex.Match(yournumber, pattern, RegexOptions.IgnoreCase);

            if (match.Success) return true;
            return false;

        }
        public static bool validNumber(string yournumber)
        {
            string pattern = @"^-[0-9]+$|^[0-9]+$";//@"^-[0-9]+$|^-[0-9]*[.]*[5]+$|^[0-9]+$|^[0-9]*[.]*[5]+$"; //@"^[+-]?[0-9][0-9]{1,}$";
            Match match = Regex.Match(yournumber, pattern, RegexOptions.IgnoreCase);

            if (match.Success) return true;
            return false;

        }

        public static bool validPositiveDecimal(string yournumber)
        {
            string pattern = @"^[0-9]*\.?[0-9]{1,}$";
            Match match = Regex.Match(yournumber, pattern, RegexOptions.IgnoreCase);

            if (match.Success) return true;
            return false;

        }
        public static bool validDecimal(string yournumber)
        {
            string pattern = @"^[+-]?[0-9]*\.?[0-9]{1,}$";
            Match match = Regex.Match(yournumber, pattern, RegexOptions.IgnoreCase);

            if (match.Success) return true;
            return false;

        }
        public static bool validDate(string yourdate)
        {
            string pattern = @"^\d{1,2}\/\d{1,2}\/\d{4}$";
            Match match = Regex.Match(yourdate, pattern, RegexOptions.IgnoreCase);

            if (match.Success) return true;
            return false;

        }

        public static Int32 ConvertToInt(string yournumber, int returnnumber)
        {
            if (yournumber == null) return returnnumber;

            string pattern = @"^[+-]?[0-9]*\.?[0-9]{1,}$";
            Match match = Regex.Match(yournumber, pattern, RegexOptions.IgnoreCase);

            if (match.Success)
            {
                try
                {
                    return Convert.ToInt32(yournumber);
                }
                catch (Exception)
                {
                    return returnnumber;
                }
            }
            return returnnumber;
        }
        public static decimal ConvertToDecimal(string yournumber, decimal returnnumber)
        {
            if (yournumber == null) return returnnumber;
            string pattern = @"^[+-]?[0-9]*\.?[0-9]{1,}$";
            Match match = Regex.Match(yournumber, pattern, RegexOptions.IgnoreCase);

            if (match.Success)
            {
                try
                {
                    return Convert.ToDecimal(yournumber);
                }
                catch (Exception)
                {
                    return returnnumber;
                }
            }
            return returnnumber;
        }

        public static double ConvertToDouble(string yournumber, double returnNumber)
        {
            string pattern = @"^[0-9|.]";
            Match match = Regex.Match(yournumber, pattern, RegexOptions.IgnoreCase);

            if (match.Success)
            {
                return Convert.ToDouble(yournumber);
            }
            return returnNumber;
        }

        public static string ConvertDateToString(DateTime yourdatetime, string yourtype)
        {
            return yourdatetime.ToString(yourtype);

        }

        public static bool ValidUsername(string yourstring)
        {
            if (Regex.IsMatch(yourstring,
                               @"^[a-zA-Z0-9_-]*$"))
            {
                return true;
            }
            return false;

        }

    }
}