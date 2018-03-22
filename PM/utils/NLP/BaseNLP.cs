using JiebaNet.Segmenter.PosSeg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JiebaNet.Analyser;
using System.Data;

namespace PM.utils.NLP
{
    public class BaseNLP
    {
        protected static ISet<string> StopWords { get; set; }
        protected static HashSet<string> PosFilter = new HashSet<string> { "nsf", "ns", "mp", "m","x" };
        private static Mysql DBconn = Mysql.GetInstance();

        public static List<Pair> CutWord(String sentance)
        {
            PosSegmenter PosSeg = new PosSegmenter();
            IEnumerable<Pair> res_pair = PosSeg.Cut(sentance);
            return res_pair.ToList();
        }

        public static Dictionary<String, int> TF(List<Pair> cutWordResult) {
            SetStopWords();
            Dictionary<String, int> tfNormal = new Dictionary<String, int>();//没有正规化
            int wordNum = cutWordResult.Count;
            foreach (Pair item in cutWordResult)
            {
                if (PosFilter.Contains(item.Flag) || StopWords.Contains(item.Word))
                    continue;
                if (tfNormal.ContainsKey(item.Word))
                    tfNormal[item.Word] += 1;
                else
                    tfNormal.Add(item.Word, 1);
            }
            return tfNormal;
        }

        public static Dictionary<String, int> SentanceTF(String sentance)
        {
            return TF(CutWord(sentance));
        }

        private static void SetStopWords()
        {
            try
            {
               var aa = StopWords.Count;
            }
            catch (Exception)
            {
                StopWords = new HashSet<string>();
                String strWordCase = "select * from StopWords";
                DataTable rs = DBconn.Query(strWordCase);
                foreach (DataRow item in rs.Rows)
                    StopWords.Add(item.Field<object>("word").ToString());
            }

        }

        public static List<string> DropStopWords(List<Pair> CutWordResult) {
            SetStopWords();
            List<string> rs = new List<string>();
            foreach (Pair item in CutWordResult)
            {
                if (PosFilter.Contains(item.Flag) || StopWords.Contains(item.Word))
                    continue;
                rs.Add(item.Word);
            }
            return rs;
        }

    }
}
