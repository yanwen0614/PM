using Microsoft.VisualStudio.TestTools.UnitTesting;
using PM.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PM.Struct;
using JiebaNet.Segmenter.PosSeg;

namespace PM.utils.Tests
{
    [TestClass()]
    public class GetLanAndLonTests
    {
        [TestMethod()]
        public void GetLantiAndLonTest()
        {
            string title = "新疆克孜勒苏州乌恰县发生3.2级地震 震源深度8千米";
            var res = GetLanAndLon.GetLatAndLonByTitle(title);
            LanAndLon ll = new LanAndLon(39.719310, 75.259228, "新疆维吾尔自治区克孜勒苏柯尔克孜自治州乌恰县", 3);
            Assert.AreEqual(res.lat, ll.lat);
            Assert.AreEqual(res.lon, ll.lon);
            Assert.AreEqual(res.strPlace, ll.strPlace);
        }

        [TestMethod()]
        public void GetLantiAndLontiTest()
        {
            LanAndLon res = GetLanAndLon.GetLatAndLonByWord("北京");
            LanAndLon ll = new LanAndLon(39.904030, 116.407526, "北京市",1);
            Assert.AreEqual(res.lat, ll.lat);
            Assert.AreEqual(res.lon, ll.lon);
            Assert.AreEqual(res.strPlace, ll.strPlace);
        }
    }
}