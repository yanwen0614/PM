using Microsoft.VisualStudio.TestTools.UnitTesting;
using PM.utils.Crawler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PM.utils.Crawler.Tests
{
    [TestClass()]
    public class News163CrawlerTests
    {
        [TestMethod()]
        public void GetCaseTest()
        {
            News163Crawler news163Crawler = new News163Crawler(1, false);
        //    news163Crawler.GetCase("http://news.163.com/18/0228/10/DBNO36NO000189FH.html","//*[@id=\"epContentLeft\"]/div[1]", "//*[@id=\"endText\"]/p", "GBK");

        }

        [TestMethod()]
        public void GetContentTest()
        {
            News163Crawler news163Crawler = new News163Crawler(1, false);
            news163Crawler.GetContent();
        }
    }
}