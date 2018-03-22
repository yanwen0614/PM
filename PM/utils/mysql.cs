using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace PM.utils
{
    public class Mysql
    {
        private MySqlConnection Connection = null;


        private static Mysql uniqueInstance;

        public static String creattableAly = "create table IF NOT EXISTS totalCount " + "(ID int AUTO_INCREMENT primary key,"
                + "Date varchar(255)," + "UrlId int," + "newItems int," + "caseItems int," + "case1 int,"
                + "case2 int," + "case3 int," + "case4 int," + "case1Title TEXT," + "case2Title TEXT," + "case3Title TEXT," + "case4Title TEXT)";
        public static String creattablesql = "create table IF NOT EXISTS %s " + "(ID int AUTO_INCREMENT primary key," +
                "Title varchar(255)," + "content varchar(255)," + "Url varchar(255)," + "Date varchar(255),"
                + "Place varchar(255))";


        private void Dbconn()
        {
            Connection = new MySqlConnection(GetDBconnstr());
        }

        private void Open()
        {
            if (Connection.State != ConnectionState.Open)
                Connection.Open();
        }

        private String GetDBconnstr()
        {
            MySqlConnectionStringBuilder M_str_sqlcon = new MySqlConnectionStringBuilder
            {
                Database = Properties.Settings.Default.数据库名称,
                Password = Properties.Settings.Default.数据库密码,
                Port = 3306,
                UserID = Properties.Settings.Default.数据库用户,
                Server = Properties.Settings.Default.数据库地址,
                CharacterSet = "utf8mb4",
            
            };
            return M_str_sqlcon.ToString();
        }

        private Mysql()
        {
            Dbconn();
        }

        public static Mysql GetInstance()
        {
            // 如果类的实例不存在则创建，否则直接返回
            if (uniqueInstance == null)
            {
                uniqueInstance = new Mysql();
            }
            return uniqueInstance;
        }

        public Boolean Exist(String Str_Sqlstr)
        {
            Open();
            MySqlDataReader reader = null;
            using (MySqlCommand cmd = new MySqlCommand(Str_Sqlstr, Connection))
            {
                reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                Connection.Close();
                return true;
            }
            Connection.Close();
            return false;
            }
        }
        public void Dbclose()
        {
            Connection.Close();
            Connection.Dispose();
        }

        //  SELECT column_name,column_name FROM table_name [WHERE Clause]
        public DataTable Query(String Str_Sqlstr)
        {
            Open();
            MySqlDataReader reader = null;
            DataTable dataTable = new DataTable();
            using (MySqlCommand cmd = new MySqlCommand(Str_Sqlstr, Connection))
            {

                reader = cmd.ExecuteReader();   
                dataTable.Load(reader);
            }
            return dataTable;

        }
        // DELETE FROM runoob_tbl where  
        public Boolean Delete(String Str_Sqlstr)
        {
            Open();
            MySqlDataReader reader = null;
            
            using (MySqlCommand cmd = new MySqlCommand(Str_Sqlstr, Connection))
            {
                reader = cmd.ExecuteReader();
                if (reader.RecordsAffected > 0)
                {
                    Connection.Close();
                    return true;
                }
                Connection.Close();
                return false;
            }
        }


        public void Creattable(String Str_Sqlstr)
        {

            Open();
            using (MySqlCommand cmd = new MySqlCommand(Str_Sqlstr, Connection))
            {
                cmd.ExecuteNonQuery();
            }
            Connection.Close();
        }

        //return affected rows
        public int Update(String Str_Sqlstr)
        {
            Open();
            int state;
            using (MySqlCommand cmd = new MySqlCommand(Str_Sqlstr, Connection))
            {
                state= cmd.ExecuteNonQuery();
            }
            return state;
        }

    }	
}
