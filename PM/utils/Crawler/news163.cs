using HtmlAgilityPack;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace PM.utils.Crawler
{
    public class News163Crawler : BaseCrawler
    {
        public News163Crawler(int samplingInterval, Boolean isvisible) : base("http://news.163.com/", samplingInterval, isvisible)
        {
            Websource = "网易新闻";
        }

        protected override string TimeFormat(string Timestr)
        {
            return Timestr.Split('来')[0];
        }

        public override void GetContent()
        {
            string[] htmlarg = new string[5] { "mod_top_news2","a" ,"//*[@id=\"epContentLeft\"]/div[1]", "//*[@id=\"endText\"]/p", "GBK" };
            GetCaseContent(htmlarg);
        }

    }
}
