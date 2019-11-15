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
        public static SqlConnection DBConnect(string dataSource, string dbName, string dbUser, string dbPwd)
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

        public static void DBDispose(SqlConnection cnn)
        {
            try
            {
                cnn.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to Close DB Connection: " + ex.Message + " " + ex.StackTrace);
            }
        }

        public static string GetTestData(this string testName, string key_name)
        {
            string data = null;

            SqlConnection cnn;
            SqlCommand command;
            SqlDataReader dataReader;
            string dataSource = "dataSource".AppSettings();
            string dbName = "dbName".AppSettings();
            string dbUser = "dbUser".AppSettings();
            string dbPwd = "dbPwd".AppSettings();

            string sql;

            try
            {
                cnn = DBConnect(dataSource, dbName, dbUser, dbPwd);
                sql = "select TOP (1) ["+key_name+"] from ["+dbName+"].[dbo].["+testName+"];";
                command = new SqlCommand(sql, cnn);

                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    data = dataReader.GetValue(0).ToString();
                    break;
                }

                dataReader.Close();
                command.Dispose();
                DBDispose(cnn);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " " + ex.StackTrace);
            }
            return data;
        }
    }
}
