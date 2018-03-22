using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace PM.utils.Crawler
{
    public class SinachinaCrawler : BaseCrawler
    {
        public SinachinaCrawler(int samplingInterval,Boolean isvisible) : base("http://news.sina.com.cn/china/", samplingInterval, isvisible)
        {
            Websource = "新浪新闻";
        }

        protected override string TimeFormat(string Timestr)
        {
            Timestr.Replace('年','-');
            Timestr.Replace('月', '-');
            Timestr.Replace('日', ' ');
            return Timestr+":00";
        }

        public override void GetContent()
        {
            string[] htmlarg = new string[5] { "blk122", "a", "//*[@id=\"top_bar\"]/div/div[2]/span", "//*[@id=\"yc_con_txt\"]/p", "utf-8" };
            GetCaseContent(htmlarg);
        }
    }
}
