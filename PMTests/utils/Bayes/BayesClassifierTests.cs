using Microsoft.VisualStudio.TestTools.UnitTesting;
using PM.utils.Bayes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PM.utils.Bayes.Tests
{
    [TestClass()]
    public class BayesClassifierTests
    {
        [TestMethod()]
        public void ClassifyTest()
        {
            String text = "雁塔王家村民房凌晨突发火灾 4人死亡13人受伤";
            int result = BayesClassifier.Classify(text);
            Console.WriteLine(result);
        }
    }
}