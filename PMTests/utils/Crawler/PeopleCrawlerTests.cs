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
    public class PeopleCrawlerTests
    {
        [TestMethod()]
        public void GetContentTest()
        {
            PeopleCrawler crawler = new PeopleCrawler(1, false);
            crawler.GetContent();
        }
    }
}