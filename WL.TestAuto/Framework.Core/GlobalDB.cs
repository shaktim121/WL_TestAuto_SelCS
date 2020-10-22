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
        public static string dataSource = string.Empty;
        public static string dbName = string.Empty;
        public static string dbUser = string.Empty;
        public static string dbPwd = string.Empty;
        public static string connectionString = string.Empty;

        //Function to create sql connection
        public static string CreateConnectionString(string DBSrv, string DB, string user, string pwd, bool winAuth)
        {
            string cnn;
            try
            {
                if (winAuth)
                {
                    cnn = @"Data Source=" + DBSrv + ";Initial Catalog=" + DB + ";Integrated Security=SSPI;";
                }
                else if(user!="" && pwd!="")
                {
                    cnn = @"Data Source=" + DBSrv + ";Initial Catalog=" + DB + ";User ID=" + user + ";Password=" + pwd + "";
                }
                else
                {
                    throw new Exception("User Name and Password are required for SQL Server Authentication");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " " + ex.StackTrace);
            }
            return cnn;
        }

        //Connect to Automation DB and Return the SQLConnection
        public static SqlConnection DBConnect(string dataSource, string dbName, string dbUser, string dbPwd, bool winAuth)
        {
            SqlConnection cnn;
            try
            {
                connectionString = CreateConnectionString(dataSource, dbName, dbUser, dbPwd, winAuth);
                cnn = new SqlConnection(connectionString);
                cnn.Open();
            }
            catch (Exception ex)
            {
                throw new Exception("DB Connection Failed: " + ex.Message + " " + ex.StackTrace);
            }
            return cnn;
        }

        public static SqlConnection DBConnect(string connectionString)
        {
            SqlConnection cnn;
            try
            {
                cnn = new SqlConnection(connectionString);
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

        //Function to execute any SQL Query
        public static DataSet ExecuteSQLQuery(string sqlQuery, string connectionString)
        {
            DataSet data = new DataSet();
            SqlDataAdapter adapter;
            SqlConnection cnn;
            SqlCommand command;

            string sql;

            try
            {
                cnn = new SqlConnection(connectionString);
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

        //Function to update using sql query
        public static int ExecuteNonSQLQuery(string sqlQuery, string connectionString)
        {
            SqlConnection cnn;
            SqlCommand command;
            string sql; int rows;

            try
            {
                cnn = DBConnect(connectionString);
                sql = sqlQuery;
                using (command = cnn.CreateCommand())
                {
                    command.CommandText = sql;
                    rows = command.ExecuteNonQuery();
                }
                command.Dispose();
                cnn.DBDispose();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " " + ex.StackTrace);
            }
            return rows;
        }

        //Execute StoredProc
        //DataSet ds = GlobalDB.ExecuteStoredProc("EmployeeAnniversaryListing_report", "@databaseName:WLAT;@securityRoleId:1001;@securityUserId:58;@languageCode:EN");
        public static DataSet ExecuteStoredProc(string storedProc, string parameters, string connectionString)
        {
            DataSet data = new DataSet();
            SqlDataAdapter adapter;
            SqlConnection cnn;
            SqlCommand command;
            string[] parameter = parameters.Split(';');

            try
            {
                cnn = new SqlConnection(connectionString);
                command = new SqlCommand(storedProc, cnn);
                command.CommandType = CommandType.StoredProcedure;

                foreach (string str in parameter)
                {
                    if(str.Split(':')[1].ToString().ToLower()=="null")
                    {
                        command.Parameters.Add(new SqlParameter(str.Split(':')[0], DBNull.Value));
                    }
                    else
                    {
                        command.Parameters.Add(new SqlParameter(str.Split(':')[0], str.Split(':')[1]));
                    }
                    
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
                string chkSql = "select * from sys.objects where type_desc = 'SQL_STORED_PROCEDURE' AND name = '" + storedProc + "'";
                if (ExecuteSQLQuery(chkSql, cnn).Tables[0].Rows.Count > 0)
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

        //Function to load all the table data together
        public static DataSet LoadTestData(this string testName)
        {
            DataSet data = new DataSet();
            SqlDataAdapter adapter;
            SqlConnection cnn;
            SqlCommand command;
            string sql;
            dataSource = "dataSource".AppSettings();
            dbName = "dbName".AppSettings();
            dbUser = "dbUser".AppSettings();
            dbPwd = "dbPwd".AppSettings();

            try
            {
                cnn = DBConnect(dataSource, dbName, dbUser, dbPwd, false);
                sql = "select * from [" + "dbName".AppSettings() + "].[dbo].[" + testName + "];";
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




    }
}
