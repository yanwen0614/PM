using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using JiebaNet.Segmenter.PosSeg;
using PM.Struct;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PM.utils
{
    public class GetLanAndLon
    {
        private static String url = "http://restapi.amap.com/v3/geocode/geo?key=8325164e247e15eea68b59e89200988b&address=";
        private static String[] temArr = { "国家", "省", "市", "区县", "开发区", "乡镇", "村庄", " 热点商圈", "兴趣点", "门牌号", "单元号", "道路", "道路交叉路口", "公交站台、地铁站", "未知" };
        private static List<String> levellist = new List<String>(temArr);
        public static LanAndLon GetLatAndLonByTitle(string title)
        {
            List<LanAndLon> lll = new List<LanAndLon>();
            PosSegmenter PosSeg = new PosSegmenter();
            IEnumerable<Pair>res_pair =  PosSeg.Cut(title);
            //Console.WriteLine(res_pair.ToString());
            foreach (Pair item in res_pair)
            {
                if (item.Flag == "ns")
                {
                    lll.Add(GetLatAndLonByWord(item.Word));
                }
            }
            lll.OrderBy(ll => ll.level);
            if (lll.Count == 0)
                return new LanAndLon();
            return lll.Last();
        }

        public static LanAndLon GetLatAndLonByWord(string Word)
        {
            
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url+Word);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            JObject jo = (JObject)JsonConvert.DeserializeObject(retString);
            JToken geocodes = jo["geocodes"];
            //if (jopos.LongCount() == 0)
            //{
            //    string suggestion = jo["suggestion"]["cities"][0]["name"].ToString();
            //    return GetLantiAndLonti(suggestion);
            //}
            try
            {
                String[] laandlon = geocodes[0]["location"].ToString().Split(',');
                string name = geocodes[0]["formatted_address"].ToString();
                int level = levellist.IndexOf(geocodes[0]["level"].ToString());
                LanAndLon ll = new LanAndLon(Convert.ToDouble(laandlon[1]), Convert.ToDouble(laandlon[0]), name, level);
                return ll;
            }
            catch (Exception)
            {
                return new LanAndLon();
            }
            
        }
    }
    
}
