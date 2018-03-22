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
    public class SouhuCrawlerTests
    {
        [TestMethod()]
        public void GetContentTest()
        {
            SouhuCrawler souhuCrawler = new SouhuCrawler(1, false);
            souhuCrawler.GetContent();
        }
    }
}