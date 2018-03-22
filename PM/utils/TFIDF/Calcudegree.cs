using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PM.utils;
using JiebaNet.Segmenter;
using JiebaNet.Segmenter.PosSeg;
using JiebaNet.Analyser;
using System.Data;
using PM.utils.NLP;


namespace PM.utils.TFIDF{
    public class CalcuDegree
    {

        protected static List<String> fileList = new List<String>();
        protected static Dictionary<String, Dictionary<String, float>> allTheTf1 = new Dictionary<String, Dictionary<String, float>>();
        protected static Dictionary<String, Dictionary<String, int>> allTheNormalTF1 = new Dictionary<String, Dictionary<String, int>>();
        protected static Mysql DBconn  = Mysql.GetInstance();
        protected static Boolean IsInit = false;
        protected int casetype;

        public CalcuDegree(int casetpye)
        {
            this.casetype = casetpye;
            InitDB();
            if (!IsInit)
            {
                NormalTFOfAll(fileList);
                TfOfAll(fileList);
                Tfidf();
            }
        }

        protected void InitDB()
        {
                
            String strWordCase = "select * from keytitletable where Lable="+ casetype;
            DataTable rs = DBconn.Query(strWordCase);
            foreach (DataRow item in rs.Rows)
                fileList.Add(item.Field<object>("KeyTitle").ToString());
        }

        protected static void StopDB()
        {
            DBconn.Dbclose();
        }

        protected List<Pair> CutWord(String file)
        {
            return BaseNLP.CutWord(file);
        }

        protected Dictionary<String, float> Tf(List<Pair> cutWordResult)
            {
                Dictionary<String, float> tf = new Dictionary<String, float>();//正规化
                Dictionary<String, int> tfNormal = NormalTF(cutWordResult);
                int wordNum = cutWordResult.Count;
                foreach (var word in tfNormal.Keys)
                {
                    tf.Add(word, (Convert.ToSingle(tfNormal[word])/ Convert.ToSingle(wordNum)));
                }
                return tf;
            }


        protected Dictionary<String, Dictionary<String, float>> TfOfAll(List<String> fileList)
        {
    	        foreach (String strFile in fileList) {
                    if (!allTheTf1.ContainsKey(strFile)){
                        Dictionary<String, float> dict = new Dictionary<String, float>();
                        dict = Tf(CutWord(strFile));
                        allTheTf1.Add(strFile, dict);
                    }
		        }
                return allTheTf1;
            }


        protected Dictionary<String, int> NormalTF(List<Pair> cutWordResult)
        {
            return BaseNLP.TF(cutWordResult);
        }

        protected Dictionary<String, Dictionary<String, int>> NormalTFOfAll(List<String> fileList)
        {
                foreach (String strFile in fileList) {
                if (!allTheNormalTF1.ContainsKey(strFile)){
                    Dictionary<String, int> dict = new Dictionary<String, int>();
                    dict = NormalTF(CutWord(strFile));
                    allTheNormalTF1.Add(strFile, dict);
                }
            }
                return allTheNormalTF1;
        }

        protected Dictionary<String, float> Idf(Dictionary<String, Dictionary<String, int>> tfInIdf) {
                //公式IDF＝log((1+|D|)/|Dt|)，其中|D|表示文档总数，|Dt|表示包含关键词t的文档数量。
            Dictionary<String, float> idf = new Dictionary<String, float>();
            List<String> located = new List<String>();
            Dictionary<String, uint> Dt = new Dictionary<String, uint>();
            float D = fileList.Count;//文档总数
            List<String> key = fileList;//存储各个文档名的List
            foreach (String file in fileList)
            {
                Dictionary<String, int> temp = tfInIdf[file];
                foreach (String word in temp.Keys)
                {
                    if (Dt.ContainsKey(word))
                        Dt[word] += 1;
                    else
                        Dt.Add(word, 1);
                }
            }
            foreach (var item in Dt)   //
                idf.Add(item.Key, -(float)Math.Log(item.Value/(1 + D), 10));
            return idf;
        }




        public double Tfidf(String str)
        {
            Dictionary<String, float> dict = Tf(CutWord(str));
            Dictionary<String, float> idf = Idf(allTheNormalTF1);
            var result = from pair in idf orderby pair.Value select pair;

            double sumTfIdf=0;
    	    foreach (String word in  dict.Keys)
                sumTfIdf += (idf[word]) * dict[word];
    	    return sumTfIdf;
        }

        protected Dictionary<String, Dictionary<String, float>> Tfidf() 
        {
            Dictionary<String, float> idf = Idf(allTheNormalTF1);
            Dictionary<String, float> Tfidf = new Dictionary<String, float>();
            Dictionary<String, Dictionary<String, float>> tf = allTheTf1;

            //foreach (String file in tf.Keys)
            //{
            //    Dictionary<String, float> singelFile = tf[file];
            //    foreach (String word in singelFile.Keys)
            //    {
            //        singelFile.Add(word, (idf[word]) * singelFile[word]);
            //    }
            //}
            return tf;
        }
    }
}
