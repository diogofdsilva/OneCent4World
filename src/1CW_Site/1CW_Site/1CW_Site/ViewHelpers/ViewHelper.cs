using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;

namespace _1CW_Site.ViewHelpers
{
    public static class ViewHelper
    {
        public static IHtmlString Repeat<T>(IEnumerable<T> collection, Func<T, object> template)
        {
            StringBuilder sb = new StringBuilder();

            foreach (T t in collection)
            {
                sb.Append(template(t));
            }

            return new HtmlString(sb.ToString());
        }

        public static IHtmlString GetFilePath(string FileName)
        {
            return new HtmlString("Content/SolidarityLogos/" + FileName);
        }

        public static IEnumerable<string> GetONGsLogos(int nLogos, IEnumerable<string> logos)
        {
            List<string> list = logos.ToList<string>();
            List<string> returnList = new List<string>();
            Random r = new Random();
            int idx;

            for ( ; nLogos > 0; --nLogos)
            {
                idx = r.Next(list.Count);
                returnList.Add(list[idx]);
                list.RemoveAt(idx);
            }

            return returnList;
        }

    }
}