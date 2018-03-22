using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace PM.utils.Crawler
{
    public class TianyabbsCrawler : BaseCrawler
    {
        public TianyabbsCrawler(int samplingInterval, Boolean isvisible) : base("http://bbs.tianya.cn/hotArticle.jsp", samplingInterval, isvisible)
        {
            Websource = "天涯论坛";
        }

        protected override string TimeFormat(string Timestr)
        {
            return Timestr.Replace("时间：","");
        }

        public override void GetContent()
        {
            string[] htmlarg = new string[5] { "mt5", "a", "//*[@id=\"post_head\"]/div[2]/div[2]/span[2]", "//*[@id=\"bd\"]/div[4]/div[1]/div/div[2]/div[1]", "utf-8" };
            GetCaseContent(htmlarg);
        }

        public new void GetCaseContent(string[] htmlarg)
        {
            IReadOnlyList<IWebElement> elementsEle = GetElements(htmlarg);

            int i = 1;
            foreach (var item in elementsEle)
            {
                if (i % 2 == 0)
                {
                    i++;
                    continue;
                }
                GetOneCase(htmlarg, item);
            }
            QuitDrvier();
            Console.WriteLine("采集成功");
        }
    }
}