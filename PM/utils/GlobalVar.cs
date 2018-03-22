using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using PM.utils.Crawler;
using System.Threading.Tasks;

namespace PM.utils
{
    static class GlobalVar
    {

        static public BaseCrawler crawler;
        public struct Colpage
        {
            static public System.Windows.Forms.Timer PARtm = new System.Windows.Forms.Timer();
            static public AutoResetEvent autoEvent = new AutoResetEvent(false);
            static public int[] CrawlerInnerCounts = new int[2] { 0, 0 };
            static public System.Windows.Forms.Timer CaseCounttm = new System.Windows.Forms.Timer();
            static public string colLogTBtext = "";
            static public Boolean suspend = false;
            static public Boolean isStart = false;
            static public Boolean cancel = false;
            static public Boolean tminit = false;
            static public int round_num = 0;
        }

        public struct Retpage
        {

            public static DateTime StartDateTime = DateTime.Today;
            public static DateTime EndDateTime = DateTime.Today;
            public static int Page = 1;
            public static int Pagecount = 0;
            public static Boolean IsCheck = false;

  
        }

        public struct Stcpage
        {
            public static DateTime StartDateTime = DateTime.Today.AddDays(-15);
            public static DateTime EndDateTime = DateTime.Today;
  
 

        }
    }
}
