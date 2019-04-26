using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MD.PersianDateTime;

namespace PezeshkPlus.Models.Extension
{
    public static class Extension
    {
        public static string ToPersian(this DateTime datetime)
        {
            TimeSpan a = DateTime.Now - datetime;
            double b = a.TotalDays;
            int c = a.Days;
            PersianDateTime persianDateTime = new PersianDateTime(datetime);

            return persianDateTime.ToLongDateString();
        }
        public static string ToDaysAgo(this DateTime datetime)
        {
            TimeSpan different = DateTime.Now - datetime;
            int differentDays = different.Days;

            if (differentDays == 0)
                return string.Format("امروز");
            if(differentDays == 1)
                return string.Format("دیروز");

            return string.Format("{0} روز پیش", differentDays);
        }
    }
}