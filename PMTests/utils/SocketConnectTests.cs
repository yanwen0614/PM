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
    public class SocketConnectTests
    {
        [TestMethod()]
        public void ListenTest()
        {
            SocketConnect socketConnect = new SocketConnect("127.0.0.1", 9999);
            socketConnect.Listen();
        }
    }
}