using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace PM.utils.Crawler
{
    public class WeixingongzhongCrawler : BaseCrawler
    {
        public WeixingongzhongCrawler(int samplingInterval,Boolean isvisible) : base("http://weixin.sogou.com/", samplingInterval, isvisible)
        {
            Websource = "微信文章";
        }

        protected override string TimeFormat(string Timestr)
        {
            return Timestr + " 00:00:00";
        }

        public override void GetContent()
        {
            string[] htmlarg = new string[5] { "news-list", "h3", "//*[@id=\"post-date\"]", "//*[@id=\"js_content\"]/p", "utf-8" };
            GetCaseContent(htmlarg);
        }

        /// <summary>  
        /// 时间戳转为C#格式时间  
        /// </summary>  
        /// <param name="timeStamp">Unix时间戳格式</param>  
        /// <returns>C#格式时间</returns>  
        public static DateTime GetTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }
    }
}