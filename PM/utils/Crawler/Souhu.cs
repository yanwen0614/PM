using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace PM.utils.Crawler
{
    public class SouhuCrawler : BaseCrawler
    {
        public SouhuCrawler(int samplingInterval,Boolean isvisible) : base("http://news.sohu.com/", samplingInterval, isvisible)
        {
            Websource = "搜狐新闻";
        }

        protected override string TimeFormat(string Timestr)
        {
            return Timestr + ":00";
        }

        public override void GetContent()
        {
            string[] htmlarg = new string[5] { "focus-news", "a", "//*[@id=\"news-time\"]", "//*[@id=\"mp-editor\"]/p", "utf-8" };
            GetCaseContent(htmlarg);
        }
    }
}