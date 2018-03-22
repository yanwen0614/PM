using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Configuration;
using PM.utils.Crawler;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Threading;
using log4net;
using System.Reflection;
using PM.utils;




namespace PM.View
{
    /// <summary>
    /// lblcol.xaml 的交互逻辑
    /// </summary>
    public partial class lblcol : Page
    {

        private Boolean IsOpenSetting = false;
        private int interval = 2;
        int ativiate_crawler_num = 0;
        Boolean visible;
        Boolean suspend;
        ILog log; 
        int round_num = 0;
        int onecrawlercasecount = 0;

        public lblcol()
        {
            InitializeComponent();
            InitializeCheckBoxs();
            round_num = GlobalVar.Colpage.round_num;
            log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            GlobalVar.Colpage.PARtm.Interval = 1;
            GlobalVar.Colpage.CaseCounttm.Interval = 5;
            if (!GlobalVar.Colpage.tminit)
            {
                GlobalVar.Colpage.PARtm.Tick += new EventHandler(Tm_Tick);
                GlobalVar.Colpage.CaseCounttm.Tick += new EventHandler(CaseCounttm_Tick);
                GlobalVar.Colpage.tminit = true;
            }
            LogTB.Text = GlobalVar.Colpage.colLogTBtext;
            suspend = GlobalVar.Colpage.suspend;
            if (suspend)
                suspendbtn.Content = "继续";           
            if (GlobalVar.Colpage.isStart)
                StartCrawl.IsEnabled = false;
            visible = Isvisible.IsChecked.Value;
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            GlobalVar.Colpage.suspend = suspend;
            GlobalVar.Colpage.round_num = round_num;
        }


        void Tm_Tick(object sender, EventArgs e)
        {
            GlobalVar.Colpage.autoEvent.Set();//通知阻塞的线程继续执行  
        }


        void CaseCounttm_Tick(object sender, EventArgs e)
        {
            if (GlobalVar.Colpage.CrawlerInnerCounts[1]> onecrawlercasecount)
            {
                onecrawlercasecount = GlobalVar.Colpage.CrawlerInnerCounts[1];
                Addstr2LogTB("\t  已采集"+ onecrawlercasecount+ "项，共"+ GlobalVar.Colpage.CrawlerInnerCounts[0]+"项");
                LogTB.DataContext += " ";
            }
        }

        private void InitializeCheckBoxs() {
            ativiate_crawler_num=+InitializeCheckBox(lbl1);
            ativiate_crawler_num = +InitializeCheckBox(lbl2);
            ativiate_crawler_num = +InitializeCheckBox(lbl3);
            ativiate_crawler_num = +InitializeCheckBox(lbl4);
            ativiate_crawler_num = +InitializeCheckBox(lbl5);
            ativiate_crawler_num = +InitializeCheckBox(lbl6);
            ativiate_crawler_num = +InitializeCheckBox(lbl7);
            ativiate_crawler_num = +InitializeCheckBox(lbl8);
            InitializeCheckBox(Isvisible);
        }

        private int InitializeCheckBox(CheckBox checkBox)
        {
            if (Convert.ToBoolean(ConfigurationManager.AppSettings[checkBox.Tag.ToString()]))
            {
                checkBox.IsChecked = true;
                return 1;
            }
            checkBox.IsChecked = false;
            return 0;
            
        }
        public delegate void NextRoundDelegate();
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GlobalVar.Colpage.cancel = false;
            GlobalVar.Colpage.isStart = true;
            (sender as Button).Dispatcher.BeginInvoke(DispatcherPriority.Normal,new NextRoundDelegate(Crawl));
            (sender as Button).IsEnabled = false;
            Addstr2LogTB("开始采集");
        }

        private void Crawl()
        {
            round_num++;
            Console.WriteLine("outer "+round_num);
            log.Info("第"+ round_num + "轮");
            Addstr2LogTB("开始第" + round_num + "轮：");
            visible = Isvisible.IsChecked.Value;
            GlobalVar.Colpage.PARtm.Start();
            GlobalVar.Colpage.CaseCounttm.Start();
            Task task = new Task(() => { CrawlTask(StartCrawl); });
            task.Start();
        }

        private void Addstr2LogTB(string line) {
            LogTB.Dispatcher.BeginInvoke(DispatcherPriority.Render, new NextRoundDelegate(() => {
                GlobalVar.Colpage.colLogTBtext += (line+"\n"); 
                LogTB.Text += (line + "\n");
                scrollviewer.ScrollToEnd();
            }));
        }

        private void CrawlTask(Button sender)
        {

            ConfigurationManager.RefreshSection("appSettings");
            Object[] param = new object[2];
            param[0] = interval;
            param[1] = visible;
            for (int i = 0; i < ConfigurationManager.AppSettings.Count - 1; i++)
            {
                // 监听取消与暂停
                if (GlobalVar.Colpage.cancel)
                {
                    Console.WriteLine("Abort mission success!");
                    Addstr2LogTB("取消完毕");
                    sender.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new NextRoundDelegate(()=> {
                        (sender as Button).IsEnabled = true;
                    })); 
                    return;
                }
                GlobalVar.Colpage.autoEvent.WaitOne();

                if (Convert.ToBoolean(ConfigurationManager.AppSettings[i]))
                {
                    Type type = Type.GetType("PM.utils.Crawler." + ConfigurationManager.AppSettings.Keys[i]);
                    GlobalVar.crawler = Activator.CreateInstance(type, param) as BaseCrawler;
                    Addstr2LogTB("\t采集 " + GlobalVar.crawler.Websource + " 中....");
                    GlobalVar.crawler.GetContent();
                    if (onecrawlercasecount == 0)
                    {
                        Addstr2LogTB("\t" + GlobalVar.crawler.Websource + "请求超时");
                    }
                    else
                    {
                        onecrawlercasecount = 0;
                        Addstr2LogTB("\t采集 " + GlobalVar.crawler.Websource + " 完成....");
                    }
                }
                Thread.Sleep(1000);
                //Console.WriteLine("  crawl" + i);
            }
            //System.Threading.Thread.Sleep(1);
            Console.WriteLine("finish");
            Thread.Sleep(1000*60*20);
            sender.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new NextRoundDelegate(Crawl));
        }


        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = "";
            string lasttext;
            try
            {
                lasttext = (sender as TextBox).Text.Remove((sender as TextBox).Text.Length - 1);
            }
            catch (Exception)
            {
                lasttext = "0";
            }
            
            if (!((sender as TextBox).Text == ""))
                text = (sender as TextBox).Text;
            else
                text = 0.ToString();
            try
            {
                interval = Convert.ToInt16((sender as TextBox).Text);
            }
            catch (Exception)
            {
                (sender as TextBox).Text = lasttext;
            }
             (sender as TextBox).SelectionStart = (sender as TextBox).Text.Length;

        }

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (IsOpenSetting)
            {
                R.Margin = (new Thickness(0, 0, -160, 0));
                L.Width = L.ActualWidth + R.Width;
                L.HorizontalAlignment = HorizontalAlignment.Stretch;
                IsOpenSetting = false;
                return;
            }
            R.Margin = (new Thickness(0, 0, 0, 0));
            L.Width = L.ActualWidth - R.Width;
            L.HorizontalAlignment = HorizontalAlignment.Left;
            IsOpenSetting = true;
        }



        private void Label_MouseDown_all(object sender, MouseButtonEventArgs e)
        {
            lbl1.IsChecked = true;
            lbl2.IsChecked = true;
            lbl3.IsChecked = true;
            lbl4.IsChecked = true;
            lbl5.IsChecked = true;
            lbl6.IsChecked = true;
            lbl7.IsChecked = true;
            lbl8.IsChecked = true;

        }

        private void Label_MouseDown_reverse(object sender, MouseButtonEventArgs e)
        {
            ChangeChecked(lbl1);
            ChangeChecked(lbl2);
            ChangeChecked(lbl3);
            ChangeChecked(lbl4);
            ChangeChecked(lbl5);
            ChangeChecked(lbl6);
            ChangeChecked(lbl7);
            ChangeChecked(lbl8);
        }

        private void ChangeChecked(CheckBox checkBox)
        {
            if (checkBox.IsChecked.Value)
            {
                checkBox.IsChecked = false;
                return;
            }
            checkBox.IsChecked = true;
        }

        private void lblUnchecked(object sender, RoutedEventArgs e)
        {
            Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            cfa.AppSettings.Settings[(sender as CheckBox).Tag.ToString()].Value = "false";
            ativiate_crawler_num = -1;
            cfa.Save();
            
        }

        private void lblChecked(object sender, RoutedEventArgs e)
        {
            Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            cfa.AppSettings.Settings[(sender as CheckBox).Tag.ToString()].Value = "true";
            ativiate_crawler_num = +1;
            cfa.Save();
        }

        private void Button_Click_suspend(object sender, RoutedEventArgs e)
        {
            if (suspend)
            {
                GlobalVar.Colpage.PARtm.Start();
                suspend = false;
                (sender as Button).Content = "暂停";
                Addstr2LogTB("\t采集继续");
                return;
            }
            suspend = true;
            GlobalVar.Colpage.PARtm.Stop();
            (sender as Button).Content = "继续";
            Addstr2LogTB("\t采集暂停");
        }

        private void Button_Click_cancel(object sender, RoutedEventArgs e)
        {
            GlobalVar.Colpage.cancel = true;
            Console.WriteLine("取消中");
            Addstr2LogTB("\t采集取消中");
            //  log.Info("测试而已！");
        }

        private void LogTB_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            
        }

    }
}
