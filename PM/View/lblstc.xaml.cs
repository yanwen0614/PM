using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using PM.utils;
using System;
using System.Data;
using System.Windows.Controls;
using static PM.utils.GlobalVar.Stcpage;

namespace PM.View
{
    /// <summary>
    /// lblstc.xaml 的交互逻辑
    /// </summary>
    public partial class lblstc : Page
    {

        public SeriesCollection SeriesCollectionline { get; set; }
        public SeriesCollection SeriesCollectionpie { get; set; }
        public Func<double, string> XFormatter { get; set; }
        public Func<double, string> YFormatter { get; set; }
        public Func<ChartPoint, string> PointLabel { get; set; }
        string DataGridsql = "SELECT `case`.id, `case`.Checked FROM `case` WHERE `case`.Casetype= {0}  AND `case`.PublishTime BETWEEN '{1}' AND '{2}' ORDER BY `case`.PublishTime DESC";
        protected Mysql DB = null;
        int[] casenum = new int[4] { 0,0,0,0};
        
        public lblstc()
        {
            InitializeComponent();
            
            DB = Mysql.GetInstance();
            StartDate.SelectedDate = StartDateTime;
            EndDate.SelectedDate = EndDateTime;
            PointLabel = chartPoint =>
                string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);
            XFormatter = val => new DateTime((long)val).Date.ToShortDateString();
            YFormatter = val => val.ToString();
            Loadchart(); 

        }


        private void Page_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            StartDateTime =  StartDate.SelectedDate.Value ;
            EndDateTime= EndDate.SelectedDate.Value;
        }


        void Loadchart() {
             SeriesCollectionline = new SeriesCollection();
            SeriesCollectionpie = new SeriesCollection();
            casenum = new int[4] { 0, 0, 0, 0 };
            for (int i = 1; i < 5; i++)
            {
                StackedAreaSeries stackedareaSeries = new StackedAreaSeries
                {
                    Title = "第" + i + "类",
                    LineSmoothness = i
                };
                var Values = new ChartValues<DateTimePoint>();
                var days = (EndDateTime - StartDateTime).Days;
                DateTime queryDate = StartDateTime.AddDays(-1);
                for (int j = 0; j < days; j++)
                {
                    queryDate = queryDate.AddDays(1);
                    int num = GetCaseCount(queryDate.Date, queryDate.Date, i);
                    casenum[i - 1] += num;
                    Values.Add(new DateTimePoint(queryDate.Date, Convert.ToDouble(num)));
                }
                stackedareaSeries.Values = Values;
                SeriesCollectionline.Add(stackedareaSeries);
                PieSeries ps = new PieSeries();
                ps.Title = "第" + i + "类";
                ps.Values =new ChartValues<int>(new int[1] { casenum[i - 1] });
                ps.DataLabels = true;
                ps.LabelPoint = PointLabel;
                SeriesCollectionpie.Add(ps);
            }
            linechart.Series = SeriesCollectionline;
            piechart.Series = SeriesCollectionpie;

        }

        int GetCaseCount(DateTime StartDateTime, DateTime EndDateTime, int casrtype) {
            String str = String.Format((DataGridsql), casrtype, StartDateTime.ToString(), EndDateTime.AddSeconds(86399).ToString());
            DataTable dataTable = DB.Query(str);
            int num = dataTable.Rows.Count;
            return num;
        }





        private void StartDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            StartDateTime = StartDate.SelectedDate.Value;
        }

        private void EndDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            EndDateTime = EndDate.SelectedDate.Value;
        }

        private void Chart_OnDataClick(object sender, ChartPoint chartPoint)
        {
            var chart = (PieChart)chartPoint.ChartView;
            var selectedSeries = (PieSeries)chartPoint.SeriesView;
            if (selectedSeries.PushOut == 8)
            {
                selectedSeries.PushOut = 0;
                return;
            }
            selectedSeries.PushOut = 8;
            //clear selected slice.
            foreach (PieSeries series in chart.Series)
                series.PushOut = 0;
            selectedSeries.PushOut = 8;
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Loadchart();
        }

    }
}
