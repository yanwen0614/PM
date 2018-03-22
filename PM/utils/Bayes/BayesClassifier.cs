using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PM.utils.NLP;
using JiebaNet.Segmenter.PosSeg;
using System.Data;

namespace PM.utils.Bayes
{
    public class BayesClassifier
    {
        protected static Mysql DBconn = Mysql.GetInstance();
        /**
        * 计算给定的文本属性向量X在给定的分类Cj中的类条件概率
        * <code>ClassConditionalProbability</code>连乘值
        * @param X 给定的文本属性向量
        * @param Cj 给定的类别
        * @return 分类条件概率连乘值，即<br>
        */
        static List<Single> CalcProd(List<String> X, int Cj)
        {
            float pro = 1f;
            string querysql = "SELECT {0} FROM BayesClassifier where word = '{1}'";
            List<Single> prolist = new List<float>();
            
            foreach (var item in X)
            {
                string query = string.Format(querysql,"ProbabilityOf" + (Cj + 1).ToString(), item);
                DataTable rs = DBconn.Query(query);
                if (rs.Rows.Count == 0)  //词库无该词
                    continue;
                var wordpro = Convert.ToSingle(rs.Rows[0]["ProbabilityOf" + (Cj + 1).ToString()].ToString());
                prolist.Add(wordpro);
                pro *= wordpro;
            }
            prolist.Add(pro);
            return prolist;
        }
        /**
        * 去掉停用词
        * @param text 给定的文本
        * @return 去停用词后结果
        */
        static List<String> DropStopWords(List<Pair> oldWords)
        {
           return BaseNLP.DropStopWords(oldWords);
        }

        static bool IsKeyWord(string word) {
            string querysql = "SELECT {0} FROM keywordtable where {0} = '{1}'";
            string query = string.Format(querysql, "KeyWord", word);
            return DBconn.Exist(query);
        }

        /**
        * 对给定的文本进行分类
        * @param text 给定的文本
        * @return 分类结果 1  2 3 4 0
        */
        public static Int16 Classify(String text)
        {
            var terms = BaseNLP.CutWord(text);//中文分词处理(分词后结果可能还包含有停用词）
            List<String> term = DropStopWords(terms);//去掉停用词，以免影响分类
            // String Classes = tdm.getTraningClassifications();//分类
            List<ClassifyResult> crs = new List<ClassifyResult>();//分类结果
            Int32 ValidWordNum = 0;
            //MI=new double[terms.length][Classes.length];
            for (Int16 i = 0; i < 4; i++)
            {
                List<float> probility = new List<float>();
                // String Ci = Classes[i];//第i个分类
                int Ci = i ;//第i个分类
                probility = CalcProd(term, Ci);//计算给定的文本属性向量terms在给定的分类Ci中的分类条件概率                              
                ClassifyResult cr = new ClassifyResult(probility,i);
                crs.Add(cr);
                ValidWordNum = probility.Count - 1;
            }
            
            if(!(Convert.ToDouble(ValidWordNum)/term.Count>0.8 && ValidWordNum>5))
                foreach (var item in term)
                {
                    if (!IsKeyWord(item))
                        return 0;
                }
            //对最后概率结果进行排序
            crs.Sort(ClassifyResult.Compare);
            //返回概率最大的分类
            return  ++crs[3].classification;//先加再return
        }
            //        String text = "内蒙古：人们感到地震后纷纷咨询地震情况";
            //        BayesClassifier classifier = new BayesClassifier();//构造Bayes分类器
            //        int  result = classifier.classify(text);//进行分类
            //        System.out.println("此项属于["+result+"]");

    }
}
