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
    public class XinhuanetCrawlerTests
    {
        [TestMethod()]
        public void GetContentTest()
        {
            XinhuanetCrawler XinhuanetCrawler = new XinhuanetCrawler(1, true);
            XinhuanetCrawler.GetContent();
        }
    }
}