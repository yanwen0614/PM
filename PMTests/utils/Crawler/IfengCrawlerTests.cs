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
    public class IfengCrawlerTests
    {
        [TestMethod()]
        public void GetContentTest()
        {
            IfengCrawler crawler = new IfengCrawler(1, false);
      //      crawler.GetContent();
        }

        [TestMethod()]
        public void GetCaseTest()
        {
            IfengCrawler ifengCrawler = new IfengCrawler(1, false);

        }

        [TestMethod()]
        public void GetTypeTest()
        {
            
        }

        [TestMethod()]
        public void IfengCrawlerTest()
        {
            IfengCrawler ifengCrawler = new IfengCrawler(1, false);
            ifengCrawler.logtest();
            int aaa = 1;
            
        }
    }
}