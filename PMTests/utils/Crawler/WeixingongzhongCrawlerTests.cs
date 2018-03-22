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
    public class WeixingongzhongCrawlerTests
    {
        [TestMethod()]
        public void GetContentTest()
        {
            WeixingongzhongCrawler news163Crawler = new WeixingongzhongCrawler(1, false);
            news163Crawler.GetContent();
        }
    }
}