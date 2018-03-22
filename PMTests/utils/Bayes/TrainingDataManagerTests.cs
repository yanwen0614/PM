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
    public class TrainingDataManagerTests
    {
        [TestMethod()]
        public void TrainModel()
        {
            TrainingDataManager tdm = new TrainingDataManager();
            tdm.TrainModel();
        }
    }
}