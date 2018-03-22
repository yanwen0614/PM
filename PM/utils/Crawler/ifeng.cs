using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using PM.Struct;
using log4net;
using System.Windows.Controls;

namespace PM.utils.Crawler
{
    public class IfengCrawler : BaseCrawler
    {
        public IfengCrawler(int samplingInterval, Boolean isvisible) : base("http://www.ifeng.com/", samplingInterval,  isvisible)
        {
            Websource = "凤凰网";
           // InitLog();
        }

        protected override string TimeFormat(string Timestr)
        {
            Timestr.Replace('年', '-');
            Timestr.Replace('月', '-');
            Timestr.Replace('日', ' ');
            return Timestr;
        }

        public void logtest() {
            logInfo.Info("sssss");
        }

        //*[@id="main_content"]/p[1] //*[@id="main_content"]/p[1]
        public override void GetContent()
        {
            string[] bak_xpath = new string[] { "/html/body/div[3]/div[1]/p/span[1]", "//*[@id=\"main_content\"]/p" };
            string[] htmlarg = new string[5] { "FNewMTopLis", "a", "//*[@id=\"artical_sth\"]/p/span[1]", "//*[@id=\"yc_con_txt\"]/p", "utf-8" };
            GetCaseContent(htmlarg, bak_xpath);
        }
    }
}
