using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
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
using log4net;
using PM.utils;
using PM.Struct;
using static PM.utils.GlobalVar.Retpage;
using static PM.utils.GlobalVar;

namespace PM.View
{
    /// <summary>
    /// lblret.xaml 的交互逻辑
    /// </summary>
    public partial class lblret : Page
    {

        protected Mysql DB = null;
        ILog log;
        DataTable dataTable = null;
        string DataGridsql = "SELECT `case`.id,  `case`.Title, `case`.Content,  `case`.WebUrl, `case`.PublishTime, `case`.CollectTime, `case`.Casetype, `case`.Websource, astext(`case`.Coorrdinates), `case`.Address, `case`.Checked FROM `case` WHERE `case`.Casetype IN(1,2,3,4) AND `case`.CollectTime BETWEEN '{0}' AND '{1}' ORDER BY `case`.PublishTime DESC {2}";
        string numlimt = "LIMIT {0}, {1}";
        public int page = 1;
        public int pagecount = 0;
        int onepagecount = 30;
        public lblret()
        {
            InitializeComponent();
            log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            DB = Mysql.GetInstance();
            StartDate.SelectedDate = StartDateTime;
            EndDate.SelectedDate = EndDateTime;
            page = Retpage.Page;
            pagecount = Retpage.Pagecount;
            CheckTag.IsChecked = IsCheck;
            if (pagecount != 0)
                CaseDataQuery(page);
            else
                CaseDataQuery();

        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            StartDateTime = StartDate.SelectedDate.Value ;
            EndDateTime = EndDate.SelectedDate.Value;
            IsCheck = CheckTag.IsChecked.Value;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CaseDataQuery();
        }

        string DataGridsqltunnel(string DataGridsql) {
            if (CheckTag.IsChecked.Value)
                return DataGridsql.Replace("(1,2,3,4)", "(0,1,2,3,4)");
            return DataGridsql;

        }

        private void CaseDataQuery()
        {
            String str = String.Format(DataGridsqltunnel(DataGridsql), StartDateTime.ToString(), EndDateTime.AddSeconds(86399).ToString(), "");
            dataTable = DB.Query(str);
            pagecount = (int)Math.Ceiling((float)(dataTable.Rows.Count) / 50);
            CaseDataQuery(page);
            checkpagebtn();

        }

        private void CaseDataQuery(int page) {
            string _ = String.Format(numlimt, (page - 1) * onepagecount, (page) * onepagecount);
            String str = String.Format(DataGridsqltunnel(DataGridsql), StartDateTime.ToString(), EndDateTime.AddSeconds(86399).ToString(), _);
            dataTable = DB.Query(str);
            DataView.ItemsSource = dataTable.DefaultView;
            pagestate.Content = page + "/" + pagecount;
            
        }

        private void StartDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            StartDateTime = StartDate.SelectedDate.Value;
        }

        private void EndDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            EndDateTime = EndDate.SelectedDate.Value;
        }

        private void DataView_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            DataRow row = (e.Row.Item as DataRowView).Row;
            string fieldname = e.Column.SortMemberPath;
            if (e.EditingElement.GetType() == (new ComboBox()).GetType())
                UpdateRow(fieldname, row, ((System.Windows.Controls.Primitives.Selector)e.EditingElement).SelectedItem);
            else
            {
                int check = ((System.Windows.Controls.Primitives.ToggleButton)e.EditingElement).IsChecked.Value ? 1 : 0;
                UpdateRow(fieldname, row, check);
            }


            // e.Column.SortMemberPath
        }

        int UpdateRow(string fieldname, DataRow Row, object value) {
            string sqlstr = string.Format("UPDATE `case` SET `case`.{0}='{1}' WHERE id='{2}'", fieldname, value, Row.ItemArray[0]);
            return DB.Update(sqlstr);
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            DataView.IsReadOnly = false;
            DataView.Columns[0].Visibility = Visibility.Visible;
            DataView.FrozenColumnCount = 1;
            CaseDataQuery();

        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            DataView.IsReadOnly = true;
            DataView.Columns[0].Visibility = Visibility.Hidden;
            DataView.FrozenColumnCount = 0;
            CaseDataQuery();
        }

        private void lastpage_Click(object sender, RoutedEventArgs e)
        {
            page--;
            checkpagebtn();
            CaseDataQuery( page);
        }

        private void nextpage_Click(object sender, RoutedEventArgs e)
        {
            page++;
            checkpagebtn();
            CaseDataQuery( page);
        }

        void checkpagebtn() {
            lastpage.IsEnabled = true;
            nextpage.IsEnabled = true;
            if (page==1)
                lastpage.IsEnabled = false;
            if (page==pagecount)
            {
                nextpage.IsEnabled = false;
            }
        }


    }
}
