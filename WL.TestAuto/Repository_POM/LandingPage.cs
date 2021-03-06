﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace WL.TestAuto
{
    public class LandingPage : AutomationCore
    {
        private IWebDriver driver;

        #region Common Object Collection

        [FindsBy(How = How.XPath, Using = "*//td[@class='rwWindowContent rwExternalContent rwLoading']")]
        private IWebElement Win_PDFReportLoad { get; set; }

        [FindsBy(How = How.XPath, Using = "*//td[@class='rwWindowContent rwExternalContent']")]
        private IWebElement Win_PDFReport { get; set; }

        [FindsBy(How = How.XPath, Using = "*//iframe[contains(@name,'RegisterReportWindow') or contains(@name,'PayrollProcessWindow') or contains(@name,'YearEndWindows') or contains(@name,'YearEndReportWindows')]")]
        private IWebElement Frame_PDFReport { get; set; }

        [FindsBy(How = How.XPath, Using = "*//a[@class='rwCloseButton' and @title='Close']")]
        private IWebElement Btn_CloseX { get; set; }

        #endregion

        #region Home Page Object Collection
        [FindsBy(How = How.Id, Using = "lblLoggedInValue")]
        private IWebElement Lbl_user { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_ProfileGroup_ProfileGroup_Input")]
        private IWebElement Drpdwn_UserGroup { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_ProfileGroup_ProfileGroup_DropDown")]
        private IWebElement List_UserGroup { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()=\"Home\" or text()=\"Maison\"]")]
        private IWebElement Menu_Home { get; set; }

        [FindsBy(How = How.XPath, Using = "*//a[@id='ChangeLanguage']")]
        private IWebElement Link_Language { get; set; }
        

        private bool IsAt()
        {
            return Browsers.Title.Contains("WorkLinks");
        }
        #endregion

        #region Constructor
        public LandingPage()
        {
            driver = Browsers.GetDriver;
        }
        #endregion

        #region Landing Page reusable Methods
        //Gets the signed in user name
        public String GetSignInUser()
        {
            try
            {
                Assert.IsTrue(IsAt());
                Lbl_user.Highlight();
            }
            catch (Exception ex)
            {
                test.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                EndTest();
                //throw new Exception(ex.Message);
            }
            return Lbl_user.Text.ToString();
        }

        public bool Fn_ChangeUserLanguage(string language)
        {
            Boolean cnt = false;
            Boolean flag = false;
            try
            {
                if(Link_Language.Exists(10))
                {
                    string lng = Link_Language.Text;
                    if(lng.Equals(language))
                    {
                        Link_Language.Click();
                        cnt = true;
                        test.Pass("User Language changed to :" + language);
                        flag = true;
                    }
                    if(!cnt)
                    {
                        test.Pass("User Language already in :" + language);
                        flag = true;
                    }
                }
                else
                {
                    test.Fail("Unable to change Language");
                    flag = false;
                }
                
            }
            catch (Exception ex)
            {
                test.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                EndTest();
                //throw new Exception(ex.Message);
            }
            if(flag)
            {
                test.Pass("Language selected successfully");
            }
            else
            {
                test.Fail("Language selection failed");
            }
            return flag;
        }

        //Selects the Signed in user profile as Administrator or Others
        public bool Fn_SelectUserProfile(string Option)
        {
            Boolean flag = false;
            try
            {
                flag = Drpdwn_UserGroup.SelectValueFromDropDown(Option);
                if (flag)
                {
                    test.Pass("User Type selected : " + Option);
                }
                else
                {
                    test.Fail("Failed to select User Type: " + Option);
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                test.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                //EndTest();
                throw new Exception(ex.Message);
            }
            
            return flag;
        }

        //Navigate to Home Screen
        public bool Fn_NavigateToHomeScreen()
        {
            Boolean flag = false;
            try
            {
                if (Menu_Home.Exists(30))
                {
                    Menu_Home.Highlight();
                    Menu_Home.Click();
                    IWebElement Link_Home = driver.FindElement(By.XPath("*//a[contains(text(),'Home') or contains(text(),'Maison')]"));
                    if (Link_Home.Exists(20))
                    {
                        flag = true;
                    }
                    else
                    {
                        flag= false;
                    }
                }
                else
                {
                    test.Fail("Unabe to find 'Home' Menu");
                    flag = false;
                }
            }
            catch (Exception ex)
            {
                test.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                EndTest();
                //throw new Exception(ex.Message);
            }
            return flag;
        }

        //Verify records in a column in UI table
        public bool Fn_VerifyRecordsInSingleColumn(IWebElement table, string columnName, string colValueString)
        {
            Boolean flag = false;
            int colNum, rowCount, finalCount = 0;
            IReadOnlyList<IWebElement> rows;
            string[] colVals;
            try
            {
                if (table.Exists(10))
                {
                    colNum = table.GetColumnNumber(columnName);
                    rows = table.FindElements(By.XPath("./tbody/tr"));
                    rowCount = table.FindElements(By.XPath("./tbody/tr")).Count;
                    colVals = colValueString.Split(';');

                    if (rowCount > 0)
                    {
                        foreach (string colVal in colVals)
                        {
                            int valCount = 0;
                            for (int i = 0; i < rowCount; i++)
                            {
                                if (rows[i].FindElements(By.TagName("td"))[colNum].Text.ToLower().Equals(colVal.ToLower()))
                                {
                                    rows[i].FindElements(By.TagName("td"))[colNum].Highlight();
                                    valCount++;
                                    test.Pass("Value : " + colVal + " found in Column : " + columnName);
                                    break;
                                }
                            }
                            if (valCount == 0)
                            {
                                test.Fail("Value : " + colVal + " Not found in Column : " + columnName);
                                flag = false;
                            }
                            else
                            {
                                finalCount++;
                            }
                        }

                        if (finalCount == colVals.Length)
                        {
                            test.Pass("All records found in Column : " + columnName);
                            flag = true;
                        }
                        else
                        {
                            test.Fail("Few or all of the records are missing in Column : " + columnName);
                            flag = false;
                        }
                    }
                    else
                    {
                        test.Fail("No rows present in table");
                        flag = false;
                    }
                }
                else
                {
                    test.Fail("Required table does not Exist");
                    flag = false;
                }
            }
            catch (Exception ex)
            {
                test.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                //EndTest();
                throw new Exception(ex.Message);
            }
            return flag;
        }

        //Verify Payroll reports from DB Stored Proc
        //spParams format - param1:val1;param2:val2;etc
        //dataToVerify format - col1:val1;col2:val2;etc
        public bool Fn_Verify_ReportsData_In_DataBase(string storedProc, string spParams, string dataToVerify, [Optional] string connection)
        {
            bool flag = false;
            DataSet ds;

            try
            {
                if(connection=="" || connection == null)
                {
                    connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), false);
                }

                ds = GlobalDB.ExecuteStoredProc(storedProc, spParams, connection);
                test.Info("Stored Procedure : " + storedProc);
                test.Info("SP Parameters : " + spParams);
                test.Info("Connection parameters: " + connection);

                if (dataToVerify.ToLower().Contains("data exists"))
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        test.Pass("Data Exists in the Report and is not empty");
                        flag = true;
                    }
                    else
                    {
                        test.Fail("No data present or Report is empty");
                    }
                }
                else
                {
                    string[] data = dataToVerify.Split(';');
                    flag = true;
                    foreach (string dt in data)
                    {
                        int cnt = 0;

                        foreach (DataRow dRow in ds.Tables[0].Rows)
                        {
                            cnt++;
                            string aval = Convert.ToString(dRow[dt.Split(':')[0]]);
                            string eval = dt.Split(':')[1];
                            
                            if(eval.ToLower().Contains("non zero"))
                            {
                                if(aval!="")
                                {
                                    int a = (int)Convert.ToDouble(aval);
                                    if (!a.Equals(0))
                                    {
                                        test.Pass("column : " + dt.Split(':')[0] + " has Non Zero value in the report DB");
                                        break;
                                    }
                                    else
                                    {
                                        test.Fail("column: " + dt.Split(':')[0] + " has Zero value in the report DB");
                                        flag = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    test.Fail("column: " + dt.Split(':')[0] + " has Zero value in the report DB");
                                    flag = false;
                                    break;
                                }
                                
                            }

                            else if (aval.Contains(eval))
                            {
                                test.Pass(eval + " : found in the report DB under column : "+ dt.Split(':')[0]);
                                break;
                            }

                            if (cnt == ds.Tables[0].Rows.Count)
                            {
                                test.Fail(eval + ": not found in the report DB under column : " + dt.Split(':')[0]);
                                flag = false;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                test.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                //EndTest();
                throw new Exception(ex.Message);
            }
            return flag;
        }

        //Verify report columns in DB through SP
        //colNames: col1;col2;col3;etc
        public bool Fn_Verify_ReportHeaders_In_DataBase(string storedProc, string spParams, string colNames)
        {
            bool flag = true;
            DataSet ds;
            try
            {
                string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), true);
                ds = GlobalDB.ExecuteStoredProc(storedProc, spParams, connection);
                foreach(string col in colNames.Split(';'))
                {
                    if (ds.Tables[0].Columns.Contains(col))
                    {
                        test.Pass("Column Name : " + col + " found in Report DB");
                    }
                    else
                    {
                        test.Fail("Column Name : " + col + " not found in Report DB");
                        flag = false;
                    }
                }
            }
            catch (Exception ex)
            {
                test.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                //EndTest();
                throw new Exception(ex.Message);
            }
            return flag;
        }

        //Gets all the values from the specified column from report DB and returns a List
        public List<string> Fn_Get_ReportsData_From_DataBase(string storedProc, string spParams, string colName)
        {
            List<string> listVals = new List<string>();
            DataSet ds;
            string colval;
            try
            {
                string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), true);
                ds = GlobalDB.ExecuteStoredProc(storedProc, spParams, connection);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach(DataRow row in ds.Tables[0].Rows)
                    {
                        colval = row[colName].ToString();
                        listVals.Add(colval);
                    }
                }
                else
                {
                    listVals.Add("");
                }   
            }
            catch (Exception ex)
            {
                test.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                //EndTest();
                throw new Exception(ex.Message);
            }
            return listVals;
        }

        //Get data from specified column and row number
        public string Fn_Get_ReportsData_From_DataBase(string storedProc, string spParams, string colName, int row)
        {
            DataSet ds;
            string colval = "";
            try
            {
                string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), true);
                ds = GlobalDB.ExecuteStoredProc(storedProc, spParams, connection);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    colval = ds.Tables[0].Rows[row][colName].ToString();
                    
                }
            }
            catch (Exception ex)
            {
                test.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                //EndTest();
                throw new Exception(ex.Message);
            }
            return colval;
        }

        //Get data from specified column with reference to reference column and value
        public string Fn_Get_ReportsData_From_DataBase(string storedProc, string spParams, string colName, string refCol, string refVal)
        {
            DataSet ds;
            string colval = "";
            try
            {
                string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), true);
                ds = GlobalDB.ExecuteStoredProc(storedProc, spParams, connection);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach(DataRow row in ds.Tables[0].Rows)
                    {
                        bool flag = true;
                        for(int i = 0; i<refCol.Split(';').Length; i++)
                        {
                            if(!row[refCol.Split(';')[i]].ToString().Equals(refVal.Split(';')[i]))
                            {
                                flag = false;
                            }
                        }
                        if(flag)
                        {
                            colval = row[colName].ToString();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                test.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                //EndTest();
                throw new Exception(ex.Message);
            }
            return colval;
        }


        #endregion

        #region Common Methods for any page

        //Verify PDF window opened
        public bool Fn_Verify_PDF_Opened_In_Window()
        {
            bool flag = false;
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            try
            {
                if (Win_PDFReportLoad.Exists(5))
                {
                    if (wait.Until(driver => Win_PDFReport.Exists(45)))
                    {
                        flag = true;
                    }
                }

                if (Win_PDFReport.Exists(10))
                {
                    string frameName = Win_PDFReport.FindElements(By.XPath(".//iframe")).FirstOrDefault().GetAttribute("name");
                    driver.SwitchTo().Frame(frameName);
                    if (driver.FindElements(By.XPath("*//embed[@type='application/pdf']")).Count > 0)
                    {
                        test.Pass("PDF report opened successfully");
                        flag = true;
                    }
                    else
                    {
                        test.Fail("Failed to verify PDF Report");
                        GenericMethods.CaptureScreenshot();
                        flag = false;
                    }
                    driver.SwitchTo().DefaultContent();
                    Btn_CloseX.Click();
                }
            }
            catch (Exception ex)
            {
                test.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                GenericMethods.CaptureScreenshot();
                throw new Exception(ex.Message);
            }
            return flag;
        }

        //Verify Downloaded Excel
        public bool Fn_Save_And_Verify_Excel(string downloadFileName="")
        {
            bool flag = false;
            try
            {

                if (GenericMethods.WaitForFileExists(downloadsFolder, downloadFileName + "*.xls*", 60))
                {
                    test.Pass("Excel report download successful");
                    flag = true;
                }
                else if (GenericMethods.SaveFileFromDialog(downloadsFolder, downloadFileName + ".xlsx", 30))
                {
                    GenericMethods.CaptureScreenshot();
                    test.Pass("Excel report download successful");
                    flag = true;
                }
                else
                {
                    GenericMethods.CaptureScreenshot();
                    test.Fail("Failed to verify Excel report download");
                }
            }
            catch (Exception ex)
            {
                test.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                GenericMethods.CaptureScreenshot();
                throw new Exception(ex.Message);
            }
            return flag;
            
        }


        #endregion


    }
}
