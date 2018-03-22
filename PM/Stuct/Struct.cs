using System;
using System.Collections.Generic;

namespace PM.Struct
{


    public class CaseStruct {
        public String Title { set; get; }
        public String Content { set; get; }
        public String WebUrl { set; get; }
        public DateTime PublishTime { set; get; }
        public DateTime CollectTime { set; get; }
        public Double Lat { set; get; }
        public Double Lon { set; get; }
        public String Address { set; get; }
        public String Websource { set; get; }
        public Int16 Casetype { set; get; }
        protected static String sqlstr = "INSERT INTO `Case` (`Title`, `Content`, `WebUrl`, `PublishTime`, `CollectTime`, `Casetype`, `Websource`,`Address`, `Coorrdinates`, `Checked`) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}','{7}', GeomFromText('{8}'), 0)";
        public CaseStruct() {
            Title = ""; Content = ""; WebUrl = ""; PublishTime = DateTime.MinValue; ;
            Casetype = 0; Websource = ""; Address = ""; Lat = 0; Lon = 0;
        }

        public string GetSql() {
            string coordinate = string.Format("POINT({0} {1})", Lat, Lon);
            CollectTime = DateTime.Now;
            string str = string.Format(sqlstr, Title, Content.Trim().Replace("'","|"), WebUrl, PublishTime, CollectTime, Casetype, Websource, Address, coordinate);
            return str;
        }

    }

    public class CasePlace
    {
        String StrCase { set; get; }
        String StrProvince { set; get; }
        String StrCity { set; get; }
        String StrDistrict { set; get; }
        public CasePlace(String strCase, String strProvince, String strCity,
                String strDistrict)
        {
            this.StrCase = strCase;
            this.StrProvince = strProvince;
            this.StrCity = strCity;
            this.StrDistrict = strDistrict;
        }
       
    }





       


    public class LanAndLon
    {
        public double lat;
        public double lon;
        public String strPlace;
        public int level;
        public LanAndLon()
        {
            lat = -1;
            lon = -1;
            strPlace = "";
            level = 999;
        }
        public LanAndLon(double t, double n, String pname,int level)
        {
            lat = t;
            lon = n;
            strPlace = pname;
            this.level = level;
        }
    }

    public class UrlsCustomStruct
    {
        public int id;
        public String webName;
        public String webUrl;
        public int rulesCollect;//1 为XPath；2为正则表达式，3位关键字；
        public String contentCollect;
    }




    public class UrlsSystem
    {
        public static int samplingInterval = 5;//时间间隔

        public static Boolean bCneb = false;//国家应急广播
        public static Boolean bsinaChina = false;//新浪中国

        public static Boolean bifeng = false;
        public static Boolean bnews163 = false;
        public static Boolean bpeople = false;
        public static Boolean bsouhu = false;
        public static Boolean btianyabbs = false;
        public static Boolean bweixingongzhong = false;
        public static Boolean bxinhuanet = false;

        public static Boolean bsinaBlog = false;//新浪微博

        public static String databasename = "LocationInfor";
        public static String msqlname = "root";
        public static String msqlpassword = "1234";
        public static int msqlport = 3306;
        public static String characterEncoding = "utf8";
        public static String useSSL = "true";

        public static double EditDistanceLimit = 0.85;

        public static String access_token = "2.00XafWZD0WfkMW15817e92b20lNfmC";
    }
    
    
}
