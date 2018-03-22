using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using PM.Struct;

namespace PM.utils.Crawler
{
    public class XinhuanetCrawler : BaseCrawler
    {
        public XinhuanetCrawler(int samplingInterval,Boolean isvisible) : base("http://www.xinhuanet.com/politics/", samplingInterval, isvisible)
        {
            Websource = "新华网";
        }

        public override void GetContent()
        {
            string[] htmlarg = new string[5] { "dataList", "h3", "/html/body/div[2]/div[3]/div/div[2]/span[1]", "//*[@id=\"p-detail\"]/p", "utf-8" };
            GetCaseContent(htmlarg);
        }

    }
}
