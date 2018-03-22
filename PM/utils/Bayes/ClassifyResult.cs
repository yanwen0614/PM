using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PM.utils.Bayes
{
    public class ClassifyResult
    {
        public List<float> probility;//分类的概率
        public Int16 classification;//分类
        public ClassifyResult(List<float> pro, Int16 classify)
        {
            this.probility = pro;
            this.classification = classify;
        }

        public int Compare(ClassifyResult r) => ClassifyResult.Compare(this, r);

        public static int Compare(ClassifyResult r1, ClassifyResult r2)
        {
            return r1.probility.Last().CompareTo(r2.probility.Last());
        }
    }
}
