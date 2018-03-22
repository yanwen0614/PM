using Microsoft.VisualStudio.TestTools.UnitTesting;
using PM.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PM.utils.Tests
{
    [TestClass()]
    public class MysqlTests
    {
        public Mysql mysql = Mysql.GetInstance();
        [TestMethod()]
        public void ExistTest()
        {
            Assert.IsTrue(mysql.Exist("SELECT * FROM test  WHERE id = 1"));//WHERE `id` = '1'
        }

        [TestMethod()]
        public void CreattableTest()
        {
           // Assert.Fail();
        }

        [TestMethod()]
        public void UpdateTest()
        {
           // Assert.Fail();
        }

        [TestMethod()]
        public void QueryTest()
        {
           // mysql.Query("SELECT * FROM test");
        }
    }
}