using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Globalization;

namespace cisApp.library
{
    public static class DateTimeUtil
    {
        private static string[] formats = {"M/d/yyyy h:mm:ss tt", "M/d/yyyy h:mm tt",
                     "MM/dd/yyyy hh:mm:ss", "M/d/yyyy h:mm:ss",
                     "M/d/yyyy hh:mm tt", "M/d/yyyy hh tt",
                     "M/d/yyyy h:mm", "M/d/yyyy h:mm",
                     "MM/dd/yyyy hh:mm", "M/dd/yyyy hh:mm",
                     "ddd, dd MMM yyyy HH:mm:ss zzz", "ddd, dd MMM yyyy H:mm:ss zzz",
                     "yyyy-MM-dd HH:mm:ss", "yyyy-MM-dd H:mm:ss",
                      "yyyy-MM-dd"};
        private static List<MonthPair> MonthList = new List<MonthPair>() { 
            new MonthPair(){ id="01", th_full="มกราคม", th_abbr="ม.ค.", en_full="JANUARY", en_abbr="Jan." },
            new MonthPair(){ id="02", th_full="กุมภาพันธ์", th_abbr="ก.พ.", en_full="FEBRUARY", en_abbr="Feb." },
            new MonthPair(){ id="03", th_full="มีนาคม", th_abbr="มี.ค.", en_full="MARCH", en_abbr="Mar." },
            new MonthPair(){ id="04", th_full="เมษายน", th_abbr="เม.ย.", en_full="APRIL", en_abbr="Apr." },
            new MonthPair(){ id="05", th_full="พฤษภาคม", th_abbr="พ.ค.", en_full="MAY", en_abbr="May" },
            new MonthPair(){ id="06", th_full="มิถุนายน", th_abbr="มิ.ย.", en_full="JUNE", en_abbr="Jun." },
            new MonthPair(){ id="07", th_full="กรกฎาคม", th_abbr="ก.ค.", en_full="JULY", en_abbr="Jul." },
            new MonthPair(){ id="08", th_full="สิงหาคม", th_abbr="ส.ค.", en_full="AUGUST", en_abbr="Aug." },
            new MonthPair(){ id="09", th_full="กันยายน", th_abbr="ก.ย.", en_full="SEPTEMBER", en_abbr="Sep." },
            new MonthPair(){ id="10", th_full="ตุลาคม", th_abbr="ต.ค.", en_full="OCTOBER", en_abbr="Oct." },
            new MonthPair(){ id="11", th_full="พฤศจิกายน", th_abbr="พ.ย.", en_full="NOVEMBER", en_abbr="Nov." },
            new MonthPair(){ id="12", th_full="ธันวาคม", th_abbr="ธ.ค.", en_full="DECEMBER", en_abbr="Dec." }
        };
        public enum Language
        {
            TH,
            EN
        }
        public enum DateTimeFormat
        {
            FULL,
            ABBR
        }
        private static List<MonthObject> GetMonthFormat(DateTimeFormat format = DateTimeFormat.ABBR, Language lan = Language.TH)
        {
            switch (format)
            {
                case DateTimeFormat.FULL: 
                    return lan == Language.TH ? MonthList.Select(o => new MonthObject { id = o.id, text = o.th_full }).ToList() : MonthList.Select(o => new MonthObject { id = o.id, text = o.en_full }).ToList();
                case DateTimeFormat.ABBR:
                    return lan == Language.TH ? MonthList.Select(o => new MonthObject { id = o.id, text = o.th_abbr }).ToList() : MonthList.Select(o => new MonthObject { id = o.id, text = o.en_abbr }).ToList();
                default:
                    return lan == Language.TH ? MonthList.Select(o => new MonthObject { id = o.id, text = o.th_abbr }).ToList() : MonthList.Select(o => new MonthObject { id = o.id, text = o.en_abbr }).ToList();
            }
        }
        public static string ToStringFormat(this DateTime? dt, DateTimeFormat format = DateTimeFormat.ABBR, Language lan = Language.TH)
        {
            if (dt == null) return "";
            var dtList = GetMonthFormat(format, lan);
            var year = dt?.Year > 2500 ? dt?.Year : dt?.Year + 543;
            var text = String.Format("{0} {1} {2}", dt?.Day.ToString("00"), dtList.Where(o => o.id == dt?.Month.ToString("00")).FirstOrDefault().text, year);
            return text;
        }
        public static DateTime? ToDateTimeFormat(this string dt)
        {
            if (dt == null) return null;
            DateTime dateValue;
            if (DateTime.TryParseExact(dt, formats,
                                    new CultureInfo("en-US"),
                                    DateTimeStyles.None,
                                    out dateValue))
            {
                return dateValue;
            }
            return null; 
        }

        public class MonthObject
        {
            public string id { get; set; }
            public string text { get; set; } 
        }
        private class MonthPair
        {
            public string id { get; set; }
            public string th_full { get; set; }
            public string th_abbr { get; set; }
            public string en_full { get; set; }
            public string en_abbr { get; set; }
        }
    }
    
    
}
