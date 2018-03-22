using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PM.utils.NLP;

namespace PM.utils.Bayes
{
    /**
    * 训练集管理器
    */
    public class TrainingDataManager 
    {
        private static Mysql DBconn = Mysql.GetInstance();
        private static List<String>[] trainingSet = new List<string>[4];


        public TrainingDataManager() {

        }

        private void LoadModel() {

        }

        private void LoadTrainSet() {
            for (int i = 0; i < 4; i++)
            {
                trainingSet[i] = new List<string>();
                String strWordCase = "select * from keytitletable where Lable=" + (i+1);
                DataTable rs = DBconn.Query(strWordCase);
                foreach (DataRow item in rs.Rows)
                    trainingSet[i].Add(item.Field<object>("KeyTitle").ToString());
            }
        }

 
        private int GetAllSampleCounts() {
            int count = 0;
            foreach (var item in trainingSet)
            {
                count += item.Count;
            }
            return count;
        }

        private Dictionary<String, int> MergeDict(Dictionary<String, int> dict1, Dictionary<String, int> dict2) {
            foreach (var item in dict2)
            {
                if (dict1.ContainsKey(item.Key))
                    dict1[item.Key] += item.Value;
                else
                    dict1.Add(item.Key,item.Value);
            }
            return dict1;
        }


        private Dictionary<String, int> CalcuCaseWordsFrequen(int casetpye) {
            Dictionary<String, int> WordsFrequency = new Dictionary<String, int>();
            foreach (var item in trainingSet[casetpye])
            {
                Dictionary<String, int> SentanceWordsFrequency = BaseNLP.SentanceTF(item);
                WordsFrequency = MergeDict(WordsFrequency, SentanceWordsFrequency);
            }
            return WordsFrequency;
        }

        private Dictionary<String, int> CalcuAllWordsFrequen(Dictionary<String, int>[] list) {
            Dictionary<String, int> WordsFrequency = new Dictionary<String, int>();
            foreach (var item in list)
                WordsFrequency = MergeDict(WordsFrequency,item);
            return WordsFrequency;
        }

        private Dictionary<String, double[]> CalcuProbability(Dictionary<String, int> AllWordsFrequency, Dictionary<String, int>[] CaseWordsFrequency)
        {
            HashSet<String> wordhashSet = new HashSet<String>();
            for (int i = 0; i < 4; i++)
            {
                var k = CaseWordsFrequency[i].Keys;
                wordhashSet.UnionWith(k);
            }
            Dictionary<String, double[]> WordProbability = new Dictionary<String, double[]>();
            foreach (var item in wordhashSet)
            {
                double[] pro = new double[4];
                for (int i = 0; i < 4; i++)
                {
                    try
                    {
                        pro[i] = Convert.ToDouble(CaseWordsFrequency[i][item] + 1) / (AllWordsFrequency[item] + 4);
                    }
                    catch (KeyNotFoundException)
                    {
                        pro[i] = Convert.ToDouble(1) / (AllWordsFrequency[item] + 4);
                    }
                    
                }
                WordProbability.Add(item, pro); //add one to smooth
            }

            return WordProbability;
        }

        public void TrainModel() {
            LoadTrainSet();
            Dictionary<String, int>[] CaseWordsFrequenArray = new Dictionary<String, int>[4];
            for (int i = 0; i < 4; i++)
                CaseWordsFrequenArray[i] = CalcuCaseWordsFrequen(i);
            var AllWordsFrequen = CalcuAllWordsFrequen(CaseWordsFrequenArray);
            Dictionary<String, double[]> CaseWordsProbability = new Dictionary<String, double[]>();
            CaseWordsProbability= CalcuProbability(AllWordsFrequen, CaseWordsFrequenArray);
            WriteProbability2DB(CaseWordsProbability);
        }

        private void WriteProbability2DB(Dictionary<String, double[]> caseWordsProbability)
        {
            DBconn.Delete(" DELETE FROM BayesClassifier");
            String Insert = "INSERT INTO BayesClassifier (word,ProbabilityOf1,ProbabilityOf2,ProbabilityOf3,ProbabilityOf4)  VALUES ('{0}', {1},{2},{3},{4})";
            foreach (var item in caseWordsProbability)
            {
                String sql =  string.Format(Insert, item.Key, item.Value[0], item.Value[1], item.Value[2], item.Value[3]);
                DBconn.Update(sql);
            }
        }
    }
}
