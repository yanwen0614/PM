using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using PM.Struct;

namespace PM.utils.Crawler
{
    public class CnebCrawler : BaseCrawler
    {
        public CnebCrawler(int samplingInterval, Boolean isvisible) :base("http://www.cneb.gov.cn/guoneinews/", samplingInterval, isvisible)
        {
            Websource = "国家应急广播";

        }

        protected override By ByWay(string str)
        {
            return By.XPath(str);
        }
        

        public override void GetContent()
        {
            int numlimt = 10;
            string[] bak_xpath = new string[] { "", "" };
            string[] htmlarg = new string[5] { "//*[@id=\"ullist\"]", "li", "", "", "utf-8" };
            GetCaseContent(htmlarg, numlimt, bak_xpath);
           // GetOneCase(htmlarg);
        }

        protected override Boolean GetOneCase(string[] htmlarg, IWebElement item, params string[] bak_xpath)
        {
            CaseStruct caseStruct = new CaseStruct
            {
                Websource = Websource
            };
            String Title = item.FindElement(By.TagName("h2")).Text;

            if (IsTitleExist(Title))
                return false;
            Int16 casetype = Judgement(Title);
            caseStruct.Title = Title;
            try
            {
                caseStruct.WebUrl = item.FindElement(By.TagName("a")).GetAttribute("href");
            }
            catch (Exception)
            {
                caseStruct.WebUrl = item.GetAttribute("href");
            }
            caseStruct.Content = item.FindElement(By.TagName("p")).Text;
            caseStruct.PublishTime = Convert.ToDateTime(item.FindElement(By.TagName("i")).Text + ":00");
            UpdateDB(Title, casetype, caseStruct);
            return true;
        }
    }
}
