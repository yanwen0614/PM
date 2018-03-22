using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using PM.utils.TFIDF;

namespace PM.utils.check
{

    public class IsAccident 
    {


        public static List<List<String>> keyWordCass = new List<List<String>>();
        public static List<List<String>> keyTitleCass = new List<List<String>>();
        private static Mysql DBconn = Mysql.GetInstance();
        public  Boolean isAccResu = false;// 是否是突发事件的最终结果
        public  String str;
        public  int stronWordClass = 0;

        public static void Addkw2case(int casetype) {
            String strWordCase = "select * from KeyWordTable where Lable="+ casetype;
            String strTitleCase = "select * from KeyTitleTable where Lable=" + casetype;
            DataTable rs = DBconn.Query(strWordCase);
            foreach (DataRow item in rs.Rows)
                keyWordCass[casetype].Add(item.Field<object>("KeyWord").ToString());
            rs = DBconn.Query(strTitleCase);
            foreach (DataRow item in rs.Rows)
                keyWordCass[casetype].Add(item.Field<object>("KeyTitle").ToString());
        }


        public IsAccident(string title)
        {
            Initiallise();
            str = title;
        }

        public static void Initiallise() {
            for (int i = 0; i < 4; i++)
            {
                Addkw2case(i);
            }
        }

        public  void ByKeyWrod()
        {
            
        }

        public  Boolean GetIsAccident()
        {

            ByKeyWrod();
            return isAccResu;

        }

        private int JuedgBykeyword(List<String> keyWordCass)
        {
            int kwcount = 0;
            for (int i = 0; i < keyWordCass.Count; i++)
            {
                if (str.Contains(keyWordCass[i]))
                {
                    kwcount += 1;
                    isAccResu = true;
                }
            }
            return kwcount;
        }

        public double JuedgBytitle(List<String> keyTitleCass,double EditDistanceLimit)
        {
            for (int i = 0; i < keyTitleCass.Count; i++)
            {
                Console.WriteLine("判别中。");
                if (isAccResu)
                    return 0;
                double similar =  EditDistance.Similar(str, keyTitleCass[i]);
                if ((similar) > EditDistanceLimit)
                    return similar;
                
            }
            return 0;
        }


        public void ByInterest(int casetype)
        {
            CalcuDegree calcu = new CalcuDegree(casetype);
            if (isAccResu)
                return;
            double tmp = calcu.Tfidf(str);
            //getP gp = new getP();

            //if (tmp > gp.getP())
            //{
            //    stronWordClass = 0;
            //    isAccResu = true;
            //}
        }






    }
}
