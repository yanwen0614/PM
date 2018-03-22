using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;

namespace PM.utils.Crawler
{
    public class PeopleCrawler : BaseCrawler
    {
        public PeopleCrawler(int samplingInterval,Boolean isvisible) : base("http://www.people.com.cn/", samplingInterval,isvisible)
        {
            Websource = "人民网";
        }

        protected override By ByWay(string str)
        {
            return By.XPath(str);
        }

        public override void GetContent()
        {
            string[] htmlarg = new string[5] { "//*[@id=\"rmw_a\"]", "a", "/html/body/div[4]/div/div[1]/text()", "//*[@id=\"rwb_zw\"]/p", "GBK" };
            GetCaseContent(htmlarg);
        }

        protected override string TimeFormat(string Timestr)
        {
            Timestr.Replace('年', '-');
            Timestr.Replace('月', '-');
            Timestr.Replace('日', ' ');
            return Timestr + ":00";
        }
    }
}

