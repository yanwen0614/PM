using Microsoft.VisualStudio.TestTools.UnitTesting;
using PM.utils.TFIDF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PM.utils.TFIDF.Tests
{
    [TestClass()]
    public class CalcuDegreeTests
    {
        [TestMethod()]
        public void MainTest()
        {
            CalcuDegree calcu = new CalcuDegree(1);

            Console.WriteLine(calcu.Tfidf("病毒性肝炎有哪几种类型"));
        }
    }
}