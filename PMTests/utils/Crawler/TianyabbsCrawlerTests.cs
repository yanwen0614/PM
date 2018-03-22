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
    public class TianyabbsCrawlerTests
    {
        [TestMethod()]
        public void GetContentTest()
        {
            TianyabbsCrawler news163Crawler = new TianyabbsCrawler(1, false);
            news163Crawler.GetContent();
        }
    }
}