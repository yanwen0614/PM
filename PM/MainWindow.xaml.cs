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
using JiebaNet.Segmenter.PosSeg;
using PM.utils;
using MahApps.Metro.Controls;
using System.Reflection;

namespace PM
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        static View.lblcol Lblcol;

        public MainWindow()
        {
            InitializeComponent();
            ChangePage(lblstc);
            Lblcol = new View.lblcol();
        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ChangePage(sender);
        }


        private void ChangePage(object sender)
        {
            ChangeSelectBackgroud(sender as Label);
            Navigate((sender as Label).Name.ToString());
        }

        private void Navigate(string path)
        {
            string uri = "PM.View." + path;

            Type type = Type.GetType(uri);
            if (type != null)
            {
                //实例化Page页
                if (uri == "PM.View.lblcol")
                    this.frame.Content = Lblcol as Page;
                else {
                object obj = type.Assembly.CreateInstance(uri);
                Page control = obj as Page;
                this.frame.Content = control;
                }

            }
        }

        private void ChangeSelectBackgroud(Label label)
        {
            this.lblret.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Transparent"));
            this.lblmap.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Transparent"));
            this.lblstc.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Transparent"));
            this.lblset.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Transparent"));
            this.lblcol.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Transparent"));
            label.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFAAAAAA"));
        }

        ~MainWindow() {
            if (GlobalVar.crawler != null)
            GlobalVar.crawler.QuitDrvier();
        }
    }
}


