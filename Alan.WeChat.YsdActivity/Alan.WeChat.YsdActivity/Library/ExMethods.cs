using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeChat.YsdActivity.Library.ExUtils
{
    public static class ExMethods
    {
        private static IDictionary<string, int> ChineseNumbers { get; set; }
        static ExMethods()
        {
            var basic = new[] { "零", "一", "二", "三", "四", "五", "六", "七", "八", "九" };

            IDictionary<string, int> matches = new Dictionary<string, int>();

            foreach (var no in Enumerable.Range(0, 10))
            {
                matches.Add(basic[no], no);
            }
            foreach (int no in Enumerable.Range(10, 10))
            {

                var number = no.ToString();

                var first = "";
                var last = basic[int.Parse(number[1].ToString())];
                if (last == basic[0]) last = "";
                matches.Add(String.Format("{0}十{1}", first, last), no);
            }
            foreach (int no in Enumerable.Range(20, 100 - 20))
            {
                var number = no.ToString();

                var first = basic[int.Parse(number[0].ToString())];
                var last = basic[int.Parse(number[1].ToString())];
                if (last == basic[0]) last = "";
                matches.Add(String.Format("{0}十{1}", first, last), no);
            }
            ChineseNumbers = matches;
        }
        public static string ToUnicode(this char charValue)
        {
            return String.Format("\\u{0}", ((int)charValue).ToString("x"));
        }
        public static string ToUnicodes(this string text)
        {
            return String.Join("", text.ToArray().Select(ToUnicode));
        }
        public static int ConvertChineseNumberToInt(this string chineseNumber)
        {
            if (String.IsNullOrWhiteSpace(chineseNumber)) return -1;
            if (ChineseNumbers.ContainsKey(chineseNumber)) return ChineseNumbers[chineseNumber];
            return -1;

        }
    }
}