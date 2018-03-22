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
    public class CnebCrawlerTests
    {
        [TestMethod()]
        public void GetContentTest()
        {
            CnebCrawler crawler = new CnebCrawler(1,false);
            crawler.GetContent();
        }
    }
}