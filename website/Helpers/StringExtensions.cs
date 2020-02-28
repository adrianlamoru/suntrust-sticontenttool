using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace st1001.website.Helpers
{
    public static class StringExtensions
    {
        
        public static string Excerpt(this string s, int length)
        {
            if (!String.IsNullOrEmpty(s))
            {
                if (s.Length > length){
                    return string.Format("{0}...", s.Substring(0, length));
                }
                return s;
            }
            return "";
        }
    }
}