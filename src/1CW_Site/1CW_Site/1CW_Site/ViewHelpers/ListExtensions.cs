using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _1CW_Site.ViewHelpers
{
    public class ListExtensions
    {
        public static int IndexOf(IEnumerable source, object value)
        {
            int index = 0;
            foreach (object item in source)
            {
                if (item.Equals(value)) return index;
                index++;
            }
            return -1;
        }


        public static string GetRowClass(IEnumerable list, object item, string evenClass, string oddClass = "")
        {
            int index = IndexOf(list, item);
            return index % 2 == 0 ? evenClass : oddClass;
        }
    }
}