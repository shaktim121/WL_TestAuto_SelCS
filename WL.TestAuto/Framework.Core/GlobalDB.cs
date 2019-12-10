using System;
using System.Configuration;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WL.TestAuto
{
    public static class GlobalDB
    {
        public static string dataSource = "dataSource".AppSettings();
        public static string dbName = "dbName".AppSettings();
        public static string dbUser = "dbUser".AppSettings();
        public static string dbPwd = "dbPwd".AppSettings();

        //Connect to Automation DB and Return the SQLConnection
        public static SqlConnection DBConnect()
        {
            string connectionString;
            SqlConnection cnn;
            //connectionString = @"Data Source=cowlqa01.island.local;Initial Catalog=testautomationio;Integrated Security=SSPI;User ID=ISLAND\sahus;Password=wyPDw*2678*y";
            connectionString = @"Data Source="+dataSource+";Initial Catalog="+dbName+";Integrated Security=SSPI;User ID="+dbUser+";Password="+dbPwd+"";
            cnn = new SqlConnection(connectionString);
            try
            {
                cnn.Open();
            }
            catch (Exception ex)
            {
                throw new Exception("DB Connection Failed: " + ex.Message + " " + ex.StackTrace);
            }
            return cnn;
        }

        //Disposes the SQLConnection
        public static void DBDispose(this SqlConnection cnn)
        {
            try
            {
                cnn.Close();
                cnn.Dispose();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to Close DB Connection: " + ex.Message + " " + ex.StackTrace);
            }
        }

        //Function to load all the table data together
        public static DataSet LoadTestData(this string testName)
        {
            DataSet data = new DataSet();
            SqlDataAdapter adapter;
            SqlConnection cnn;
            SqlCommand command;
            SqlDataReader dataReader;

            string sql;

            try
            {
                cnn = DBConnect();
                sql = "select * from [" + dbName + "].[dbo].[" + testName + "];";
                //sql = "select TOP (1) ["+key_name+"] from ["+dbName+"].[dbo].["+testName+"];";
                using (command = cnn.CreateCommand())
                {
                    command.CommandText = sql;
                    adapter = new SqlDataAdapter(command);
                    adapter.Fill(data);
                }

                //command = new SqlCommand(sql, cnn);
                //dataReader = command.ExecuteReader();
                /*while (dataReader.Read())
                {
                    data = dataReader.GetValue(0).ToString();
                    break;
                }*/

                //dataReader.Close();
                adapter.Dispose();
                command.Dispose();
                cnn.DBDispose();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " " + ex.StackTrace);
            }
            return data;
        }

        //Function to fetch individual test data value from a dataSet
        public static string GetTestData(this DataSet ds, string key)
        {
            string sval;
            try
            {
                sval = Convert.ToString(ds.Tables[0].Rows[0][key]);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " " + ex.StackTrace);
            }
            return sval;
        }

        //Function to execute any SQL Query
        public static DataSet ExecuteSQLQuery(string sqlQuery, string cnnString)
        {
            DataSet data = new DataSet();
            SqlDataAdapter adapter;
            SqlConnection cnn;
            SqlCommand command;

            string sql;

            try
            {
                cnn = new SqlConnection(cnnString);
                sql = sqlQuery;
                using (command = cnn.CreateCommand())
                {
                    command.CommandText = sql;
                    adapter = new SqlDataAdapter(command);
                    adapter.Fill(data);
                }

                adapter.Dispose();
                command.Dispose();
                cnn.DBDispose();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " " + ex.StackTrace);
            }
            return data;
        }

        //Execute StoredProc
        //DataSet ds = GlobalDB.ExecuteStoredProc("EmployeeAnniversaryListing_report", "@databaseName:WLAT;@securityRoleId:1001;@securityUserId:58;@languageCode:EN");
        public static DataSet ExecuteStoredProc(string storedProc, string parameters)
        {
            DataSet data = new DataSet();
            SqlDataAdapter adapter;
            SqlConnection cnn;
            SqlCommand command;
            string[] parameter = parameters.Split(';') ;

            string dataSource = "dataSource".AppSettings();
            string dbName = "dbMainName".AppSettings();
            string dbUser = "dbUser".AppSettings();
            string dbPwd = "dbPwd".AppSettings();

            try
            {
                string connectionString = @"Data Source=" + dataSource + ";Initial Catalog=" + dbName + ";Integrated Security=SSPI;User ID=" + dbUser + ";Password=" + dbPwd + "";
                cnn = new SqlConnection(connectionString);
                command = new SqlCommand(storedProc, cnn);
                command.CommandType = CommandType.StoredProcedure;
                
                foreach(string str in parameter)
                {
                    command.Parameters.Add(new SqlParameter(str.Split(':')[0], str.Split(':')[1]));
                }
                
                adapter = new SqlDataAdapter(command);
                adapter.Fill(data);


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " " + ex.StackTrace);
            }

            return data;
        }

        //Verify if Stored Proc Exists
        public static bool IsStoredProcedureExists(string cnn, string storedProc)
        {
            bool flag = false;

            try
            {
                string chkSql = "select * from sys.objects where type_desc = 'SQL_STORED_PROCEDURE' AND name = '"+storedProc+"'";
                if(ExecuteSQLQuery(chkSql, cnn).Tables[0].Rows.Count > 0)
                {
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " " + ex.StackTrace);
            }

            return flag;
        }
    }
}
