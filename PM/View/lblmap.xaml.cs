using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CefSharp.Wpf;
using log4net;

namespace PM.View
{
    /// <summary>
    /// PageFunction1.xaml 的交互逻辑
    /// </summary>
    public partial class lblmap : Page
    {
        DateTime StartDateTime;
        DateTime EndDateTime;
        string szTmp = "http://127.0.0.1:5000/";
        // ILog log;
        public lblmap()
        {
            InitializeComponent();
            browser.Address = szTmp;
            StartDate.SelectedDate = DateTime.Today;
            EndDate.SelectedDate = DateTime.Today;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string url = szTmp +
                "query?" + "StartDate=" + StartDateTime.ToString() +
                            "&EndDate=" + EndDateTime.AddSeconds(86399).ToString();
            browser.Address = url;
        }

        private void StartDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            StartDateTime = StartDate.SelectedDate.Value;
        }

        private void EndDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            EndDateTime = EndDate.SelectedDate.Value;
        }

        //private void GetQuery_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    Query.Margin =(new Thickness(0,0,0,0));
        //    GetQuery.Visibility = Visibility.Hidden;
        //    CloseQuery.Visibility = Visibility.Visible;

        //}

        //private void CloseQuery_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    Query.Margin = (new Thickness(0, -40, 0, 0));
        //    GetQuery.Visibility = Visibility.Visible;
        //    CloseQuery.Visibility = Visibility.Hidden;
        //}
    }
}
