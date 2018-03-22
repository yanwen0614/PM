using log4net;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using System;
using System.Collections.Generic;
using PM.Struct;
using System.Windows;
using PM.utils.Bayes;
using PM.utils.check;
using HtmlAgilityPack;
using System.Net;
using System.IO;
using System.Text;
using OpenQA.Selenium.Chrome;
using System.Runtime.InteropServices;

namespace PM.utils.Crawler
{
    public class BaseCrawler
    {
        protected IWebDriver driver = null;
        protected String webBrowserPath = "";
        protected String Url { get; set; }
        protected int crawMethod = -1;
        protected String crawContent = null;
        protected int samplingInterval = 2;
        protected ILog logInfo;
        protected Mysql DB = null;
        public string Websource;
        Boolean Visible;
        protected HttpWebRequest Webconnection;

        // 查找窗口句柄  
        [DllImport("user32.dll")]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        // 显示/隐藏窗口  
        [DllImport("user32.dll", EntryPoint = "ShowWindow")]
        static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);


        protected HtmlDocument InitWebconnection(string Url,string EncodingMethod)
        {
            Webconnection = (HttpWebRequest)WebRequest.Create(Url);
            Webconnection.Method = "GET";
            Webconnection.ContentType = "text/html;charset="+ EncodingMethod;
            Webconnection.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Sa";
            Webconnection.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
            HttpWebResponse response = (HttpWebResponse)Webconnection.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding(EncodingMethod));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(retString);
            return htmlDocument;
        }



        protected void InitLog()
        {       
            logInfo = LogManager.GetLogger(GetSubClassname());
        }

        protected virtual Type GetSubClassname()
        {
            return GetType();
        }

        protected BaseCrawler(String url, int samplingInterval, Boolean Isvisible)
        {
            this.Url = url;
            this.samplingInterval = samplingInterval;
            InitLog();
            Visible = Isvisible;
            DB = Mysql.GetInstance();
        }



        public void QuitDrvier()
        {
            if (null != driver)
                driver.Quit();
        }

        protected void WaitForSecond()
        {

        }

        public virtual  void GetContent()
        {
            throw new NotImplementedException();
        }

        protected virtual string TimeFormat(string Timestr) {
            return Timestr;
        }

        protected virtual By ByWay(string str)
        {
            return By.ClassName(str);
        }

        // htmlarg 为网页上配对的标记值
        public void GetCaseContent(string[] htmlarg, params string[] bak_xpath)
        {
            GetCaseContent(htmlarg, 0, bak_xpath);
        }

        public void GetCaseContent(string[] htmlarg, int numlimt , params string[] bak_xpath)
        {
            string wdsname;
            if (Visible)
            {
                driver = new ChromeDriver();
                wdsname = System.AppDomain.CurrentDomain.BaseDirectory+ "chromedriver.exe";
            }
            else
            {
                driver = new PhantomJSDriver();
                wdsname = System.AppDomain.CurrentDomain.BaseDirectory + "PhantomJS.exe";
            }
            IntPtr handle = FindWindow(null, wdsname);
            ShowWindow(handle, 0);

            IReadOnlyList<IWebElement> elementsEle = GetElements(htmlarg);

            if (elementsEle == null)
            {
                Console.WriteLine("请求超时");
                QuitDrvier();
                return;
            }
            if (numlimt == 0)
            {
                GlobalVar.Colpage.CrawlerInnerCounts = new int[2] { elementsEle.Count, 0 };
                foreach (var item in elementsEle)
                {
                    GlobalVar.Colpage.CrawlerInnerCounts[1]++;
                    GlobalVar.Colpage.autoEvent.WaitOne();
                    GetOneCase(htmlarg, item, bak_xpath);
                }
            }
            else
            {

                GlobalVar.Colpage.CrawlerInnerCounts = new int[2] { numlimt, 0 };

                for (int i = 0; i < numlimt; i++)
                {
                    GlobalVar.Colpage.CrawlerInnerCounts[1]++;
                    GlobalVar.Colpage.autoEvent.WaitOne();
                    GetOneCase(htmlarg, elementsEle[i], bak_xpath);
                }
            }
            QuitDrvier();
            Console.WriteLine("采集成功");
            GlobalVar.Colpage.CrawlerInnerCounts = new int[2] { 0, 0 };
            logInfo.Info("");
        }

        protected virtual Boolean GetOneCase(string[] htmlarg, IWebElement item, params string[] bak_xpath)
        {
            CaseStruct caseStruct = new CaseStruct
            {
                Websource = Websource
            };
            String Title = item.Text;
            if (Title.Length < 5)
                return false;
            if (IsTitleExist(Title))
                return false;
            Int16 casetype = Judgement(Title);
            //if (casetype == 0)
            //{
            //    UpdateTrainSet(Title);
            //    return;
            //}
            caseStruct.Title = Title;
            try
            {
                caseStruct.WebUrl = item.FindElement(By.TagName("a")).GetAttribute("href");
            }
            catch (Exception)
            {
                caseStruct.WebUrl = item.GetAttribute("href");
            }

            List<string>[] comboxXpath= ComboxXpath(htmlarg[2], htmlarg[3], bak_xpath);

            object[] TimeAndContent = GetCase(caseStruct.WebUrl, comboxXpath[0].ToArray(), comboxXpath[1].ToArray(), htmlarg[4]);
            caseStruct.Content = TimeAndContent[1] as string;
            caseStruct.PublishTime = Convert.ToDateTime(TimeAndContent[0]);
            UpdateDB(Title, casetype, caseStruct);
            return true;
        }

        protected List<string>[] ComboxXpath(string t, string c, string[] bak_xpath)
        {
            List<string>[] list = new List<string>[2] { new List<string>(),new List<string>()};
            list[0].Add(t);
            list[1].Add(c);
            for (int i = 0; i < bak_xpath.Length; i++)
            {
                if (i % 2 == 0)
                    list[0].Add(bak_xpath[i]);
                else
                    list[1].Add(bak_xpath[i]);
            }
            return list;
        }

        public object[] GetCase(string webUrl, string[] xpathtime, string[] xpathcontent, string EncodingMethod) //对于只有标题的case需要点进去
        {
            HtmlDocument htmlDocument = InitWebconnection(webUrl, EncodingMethod);
            HtmlNode htmlNode = htmlDocument.DocumentNode;
            HtmlNode timenode;
            HtmlNodeCollection contentnodes;
            DateTime time = Convert.ToDateTime(("1970-1-1 00:00:00")); ;
            string content = "";
            foreach (var item in xpathtime)
            {
                try
                {
                    timenode = htmlNode.SelectNodes(item)[0];
                    time = Convert.ToDateTime(TimeFormat(timenode.InnerText));
                    break;
                }
                catch (Exception e)
                {

                    Console.WriteLine(time+e.Message.ToString());
                    logInfo.Debug(e.ToString());
                    
                }
            }
            if (time ==DateTime.Parse("1970-1-1 00:00:00"))
                logInfo.Error("时间未找到 当前url： " + webUrl);

            foreach (var item in xpathcontent)
            {
                try
                {
                    contentnodes = htmlNode.SelectNodes(item);
                    foreach (var p in contentnodes)
                        if (p.InnerText.Length < 2000)
                            content = content + p.InnerText;
                    break;
                }
                catch (Exception e)
                {
                    logInfo.Debug(e.ToString());
                }
            }
            if (content == "")
                logInfo.Error("内容未找到 当前url： " + webUrl);
            object[] ret = new object[2] { time, content };
            return ret;
        }



        protected IReadOnlyList<IWebElement> GetElements(string[] htmlarg)
        {
            int count = 0;
            int OutTime = 0;
            IWebElement elementsLoca;
            IReadOnlyList<IWebElement> elementsEle = null;
     //       GlobalVar.autoEvent.WaitOne(); 
            do
            {
                try
                {
                    driver.Navigate().GoToUrl(Url);
                    elementsLoca = driver.FindElement(ByWay(htmlarg[0]));
                    elementsEle = elementsLoca.FindElements(By.TagName(htmlarg[1]));
                    count = elementsEle.Count;
                }
                catch (WebDriverTimeoutException)
                {
                    OutTime++;
                    if (OutTime > 3) break;
                }
                catch (NoSuchElementException e) {
                    logInfo.Error(e.ToString());
                    logInfo.Error( Url);
                }
            }
            while (count == 0);
            return elementsEle;
        }

        protected Boolean IsTitleExist(String title) {
            string sqlCheck = "select 1 from `HashTitle` where `HashTitle` = " + title.GetHashCode() + ";";
            return DB.Exist(sqlCheck);
        }

        //return affected rows
        protected int InsertTitle2Hashtable(String title)
        {
            string sqlCheck =string.Format("INSERT INTO `HashTitle` (`HashTitle`, `CollectionTime`) VALUES('{0}', NOW())",title.GetHashCode());
            return DB.Update(sqlCheck);

        }

        protected void UpdateDB(string Title, short casetype, CaseStruct caseStruct)
        {
            var ll = GetLanAndLon.GetLatAndLonByTitle(Title);
            caseStruct.Casetype = casetype;
            caseStruct.Address = ll.strPlace;
            caseStruct.Lat = ll.lat;
            caseStruct.Lon = ll.lon;//数据加载至casestruct
            DB.Update(caseStruct.GetSql());
            InsertTitle2Hashtable(caseStruct.Title);
            UpdateTrainSet(Title, casetype);
        }

        protected LanAndLon GetLocation(string Title)
        {
            LanAndLon ll = null;
            ll = GetLanAndLon.GetLatAndLonByTitle(Title);
            String sqlKeyTitleTable = "insert into LocationPOI (Place,Lontitude,Latitude) " +
                " values ('" + ll.strPlace + "'," + ll.lon + "," + ll.lat + ")";
            DB.Update(sqlKeyTitleTable);
            return ll;
        }

        /*是突发的 */
        protected void UpdateTrainSet(string Title, Int16 casetype)
        {
            String sqlKeyTitleTable = "insert into KeyTitleTable_add (KeyTitle,Lable)values  ('{0}', {1})";
            sqlKeyTitleTable = string.Format(sqlKeyTitleTable, Title, casetype);
            DB.Update(sqlKeyTitleTable);
        }

        /*不是突发的 */
        protected void UpdateTrainSet(string Title)
        {
            String sqlKeyTitleTable = "insert into nonetitletable (KeyTitle,Lable)values  ('{0}', {1})";
            sqlKeyTitleTable = string.Format(sqlKeyTitleTable, Title,0);
            InsertTitle2Hashtable(Title);
            DB.Update(sqlKeyTitleTable);
        }

        protected Int16 Judgement(String Title)
        {
            //IsAccident isAccident = new IsAccident(Title);
            //if (isAccident.isAccResu)
            //    return 0;
            //每判断一个类别，插入一个类别
            // BayesClassifier classifier = new BayesClassifier();// 构造Bayes分类器
            Int16 result = BayesClassifier.Classify(Title);// 进行分类
            return result;
        }


      
    }
}