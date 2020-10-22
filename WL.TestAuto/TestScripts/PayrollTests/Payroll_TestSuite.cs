using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.Threading;
using Org.BouncyCastle.Asn1.Cmp;

namespace WL.TestAuto
{
    [TestClass]
    public class Payroll_TestSuite : AutomationCore
    {
        private static string xmlDataFile = projectDirectory + "\\TestScripts\\PayrollTests\\ProcessGroupDetails.xml";

        //********************** Payroll screen Reports verification ***********************************//

        [TestMethod]
        public void WL_CAN_Payroll_001000_Create_Payroll_Processing_Group()
        {
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string screen = data.GetTestData("Setup_Screen");
            /*string country = data.GetTestData("Country");
            string payFrequency = data.GetTestData("Pay_Frequency");
            string processGroup = data.GetTestData("Process_Group");
            string engDesc = data.GetTestData("English_Desc");
            string frenchDesc = data.GetTestData("French_Desc");
            string period = data.GetTestData("Period");
            string startDate = data.GetTestData("Start_Date");
            string cutoffDate = data.GetTestData("Cutoff_Date");*/


            string country = GenericMethods.GetXMLNodeValue(xmlDataFile, "Country");
            string payFrequency = GenericMethods.GetXMLNodeValue(xmlDataFile, "PayFrequency");
            string processGroup = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroupName");
            string engDesc = GenericMethods.GetXMLNodeValue(xmlDataFile, "EnglishDesc");
            string frenchDesc = GenericMethods.GetXMLNodeValue(xmlDataFile, "FrenchDesc");
            string period = GenericMethods.GetXMLNodeValue(xmlDataFile, "Period");
            string startDate = GenericMethods.GetXMLNodeValue(xmlDataFile, "StartDate");
            string cutoffDate = GenericMethods.GetXMLNodeValue(xmlDataFile, "CutOffDate");

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Payroll Processing Group in Setup
            Pages.Setup.Fn_Navigate_Through_Setup(screen);

            //Create Payroll processing group
            if (!Pages.Setup.Fn_Create_Payroll_Processing_Group(country, payFrequency, processGroup, engDesc, frenchDesc, period, startDate, cutoffDate))
            {
                GenericMethods.CaptureScreenshot();
                Assert.Fail("Failed to create Payroll Procesing Group");
                
            }
        }

        #region New Hire
        private string EmpNum = Utilities.Random_Number(100001, 9999999).ToString(); // data.GetTestData("Emp_Number");
        private string FirstName = "Derek"; //data.GetTestData("First_Name");
        private string LastName = "Shepard"; // data.GetTestData("Last_Name");
        private string SIN = SinGenerator.GetValidSin(); //data.GetTestData("SIN");
        //private string NewSIN = "754420891"; //https://www.fakenamegenerator.com/social-insurance-number.php
        private string NewSIN = SinGenerator.GetValidSin();
        private string NewFName = "Sandra";
        private string NewLName = "Robert";

        [TestMethod]
        //26.	HR Screen - Hire
        public void WL_CAN_NEW_HIRE_001000_Hire_an_Employee()
        {
            //Update Edit and Terminate case with Emp_Number
            string update_sql_hire = "UPDATE dbo.WL_CAN_NEW_HIRE_001000_Hire_an_Employee SET Emp_Number = " + EmpNum + " WHERE TestCaseName = 'WL_CAN_NEW_HIRE_001000_Hire_an_Employee';";
            string update_sql_edit = "UPDATE dbo.WL_CAN_NEW_HIRE_001100_Edit_an_Employee_record SET Emp_Number = " + EmpNum + " WHERE TestCaseName = 'WL_CAN_NEW_HIRE_001100_Edit_an_Employee_record';";
            string update_sql_term = "UPDATE dbo.WL_CAN_NEW_HIRE_001200_Terminate_an_Employee_record SET Emp_Number = " + EmpNum + " WHERE TestCaseName = 'WL_CAN_NEW_HIRE_001200_Terminate_an_Employee_record';";

            string DBServer = "dataSource".AppSettings();
            string DBName = "dbName".AppSettings();
            string DBuserName = "dbUser".AppSettings();
            string DBpassword = "dbPwd".AppSettings();
            string connection = GlobalDB.CreateConnectionString(DBServer, DBName, DBuserName, DBpassword, false);
            if (GlobalDB.ExecuteNonSQLQuery(update_sql_edit, connection) > 0 && GlobalDB.ExecuteNonSQLQuery(update_sql_term, connection) > 0 && GlobalDB.ExecuteNonSQLQuery(update_sql_hire, connection) > 0)
            {
                test.Info("Employee Number updated successfully for Hire, Edit and Terminate Employee");
            }

            string lang = data.GetTestData("User_Language");
            string userProfile = data.GetTestData("User_Profile");
            string hrScreen1 = data.GetTestData("HR_Screen1");
            string hrScreen2 = data.GetTestData("HR_Screen2");
            string payScreen = data.GetTestData("Payroll_Screen");
            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroup");
            string empState = data.GetTestData("Emp_State");
            string empDetails = data.GetTestData("Verify_Emp_Details");

            string suffix = DateTime.Now.ToString("HHmm");
            //EmpNum = (Convert.ToInt32(EmpNum) + 1).ToString();
            FirstName += suffix;
            LastName += suffix;

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(lang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProfile);

            //Navigate to Employee to search employee
            Pages.HR.Fn_NavigateThroughHumanResources(hrScreen1);
            if (Pages.HR.Fn_Search_Employee_Exists(EmpNum, FirstName, LastName, "", false))
            {
                test.Pass("Employee Does not Exist");
            }
            else
            {
                test.Pass("Employee already Exists");
                EndTest();
            }

            //Navigate to Add Wizard Scren in Human Resources
            Pages.HR.Fn_NavigateThroughHumanResources(hrScreen2);
            Thread.Sleep(5000);
            if (Pages.HR.Fn_Hire_Employee(EmpNum, FirstName, LastName, SIN, processGrp))
            {
                //Verify Employee hired Successfully
                Pages.Payroll.Fn_NavigateThroughPayroll(payScreen);
                if (Pages.HR.Fn_Search_Employee_Exists(EmpNum, FirstName, LastName, "", true))
                {
                    test.Pass("Employee Hired Successfully : " + FirstName + " " + LastName);
                }
                else
                {
                    test.Fail("Employee Hire failed/unable to find hired employee : " + FirstName + " " + LastName);
                    GenericMethods.CaptureScreenshot();
                    Assert.Fail("Failed to Hire Employee");
                }
            }
            else
            {
                GenericMethods.CaptureScreenshot();
                Assert.Fail("Failed to Hire Employee");
            }

            Thread.Sleep(5000);

            if (!Pages.Payroll.Fn_Verify_Employee_Details_In_Payroll_Position(empState, empDetails))
            {
                GenericMethods.CaptureScreenshot();
                Assert.Fail();
            }

        }

        #endregion

        [TestMethod]
        public void WL_CAN_Payroll_001200_Calculate_Payroll()
        {
            string userLang = data.GetTestData("User_Language");
            string userProf = "userProfile".AppSettings();
            string screen = data.GetTestData("Payroll_Screen");

            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroup");
            string runType = GenericMethods.GetXMLNodeValue(xmlDataFile, "RunType");
            string chequeDate = GenericMethods.GetXMLNodeValue(xmlDataFile, "ChequeDate");
            if (chequeDate == "CURR_DATE")
            {
                chequeDate = DateTime.Now.AddDays(1).ToString("M/d/yyyy"); // Selects the next date
                //chequeDate = "6/5/2020";
            }

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Payroll process Screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll(screen);

            //Calcualte payroll
            if (!Pages.Payroll.Fn_Calculate_Payroll_In_Payroll_Process(processGrp, runType, chequeDate))
            {
                GenericMethods.CaptureScreenshot();
                Assert.Fail("Calculate payroll failed");
                
            }

        }

        [TestMethod]
        public void WL_CAN_Payroll_001300_Verify_Standard_Reports_Payroll_Funding()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = "userProfile".AppSettings();
            string screen = data.GetTestData("Payroll_Screen");
            string reportType = data.GetTestData("Report_Type");
            string reportName = data.GetTestData("Report_Name");
            string reportFormat = data.GetTestData("Report_Format");
            string headerSPName = data.GetTestData("ReportHeader_SP");
            string headerToVerify = data.GetTestData("ReportHeader_Val");

            string reportSectionSP = data.GetTestData("ReportSection_SP");
            string getColNames = data.GetTestData("Get_ColumnNames");
            string reportGrandTotalSP = data.GetTestData("ReportTotal_SP");


            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroup");
            string runType = GenericMethods.GetXMLNodeValue(xmlDataFile, "RunType");
            string chequeDate = GenericMethods.GetXMLNodeValue(xmlDataFile, "ChequeDate");
            if (chequeDate == "CURR_DATE")
            {
                chequeDate = DateTime.Now.AddDays(1).ToString("M/d/yyyy");
            }
            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), false);

            string queryLastPPID = "lastPayProcessIDQuery".AppSettings().Replace("ppcode", GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroupName"));
            string payProcessID = GlobalDB.ExecuteSQLQuery(queryLastPPID, connection).GetTestData("payroll_process_id").ToString();
            string SPParams = data.GetTestData("SP_Params").Replace("ppid", payProcessID).Replace("yyyy", DateTime.Now.Year.ToString()).Replace("reportDB", "dbReportName".AppSettings());
            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Payroll process Screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll(screen);

            //Calculate Payroll if not available before verifying report
            if (!Pages.Payroll.Fn_Verify_Payroll_Run_In_Payroll_Process(processGrp, runType, "Calculation Processed"))
            {
                test.Info("Payroll not calculated, hence calculating payroll");
                //Calcualte payroll
                if (!Pages.Payroll.Fn_Calculate_Payroll_In_Payroll_Process(processGrp, runType, chequeDate))
                {
                    GenericMethods.CaptureScreenshot();
                    Assert.Fail("Calculate payroll failed");
                }
            }

            //Verify Standard Payroll Funding report
            if (Pages.Payroll.Fn_Verify_Reports_In_Payroll_ProcessTable(reportType, reportName, reportFormat))
            {
                test.Pass("Standard Payroll Funding report opened and verified Successfully");
            }
            else
            {
                GenericMethods.CaptureScreenshot();
                test.Fail("Failed to verify Standard Payroll Funding report");
                Assert.Fail("Failed to verify Report data");
            }

            test.Info("Verify Standard Payroll Funding report Header");
            //Verify Standard Payroll Funding report from DB
            if (Pages.Home.Fn_Verify_ReportsData_In_DataBase(headerSPName, SPParams, headerToVerify))
            {
                test.Pass("Standard Payroll Funding report header verified successfully");
            }
            else
            {
                test.Fail("Failed to verify Standard Payroll Funding report header");
                Assert.Fail();
            }

            //Verify Sections in the report Exist
            test.Info("Verify Standard Payroll Funding report Section Names and get Amount");
            string[] sectionSPNames = reportSectionSP.Split(';');
            double actTotal = 0.0;
            double grandTotal;
            for (int i = 0; i < sectionSPNames.Length; i++)
            {
                string section = sectionSPNames[i].Split(':')[0];
                string spName = sectionSPNames[i].Split(':')[1];

                if (GlobalDB.IsStoredProcedureExists(connection, spName))
                {
                    test.Pass("Section: " + section + " found in report Payroll Funding");

                    List<string> outputVals = Pages.Home.Fn_Get_ReportsData_From_DataBase(spName, SPParams, getColNames.Split(';')[i]);
                    foreach (string outputVal in outputVals)
                    {
                        if (outputVal != "")
                        {
                            test.Info("Amount under Section : " + section + " is : $" + outputVal);
                            actTotal += Convert.ToDouble(outputVal);
                        }
                        else
                        {
                            test.Info("Amount under Section : " + section + " is : $0.00");
                        }
                    }
                }
                else
                {
                    test.Fail("Section: " + section + " not found in report Payroll Funding");
                }
            }

            if (actTotal > 0.0)
            {
                actTotal = Math.Round(actTotal, 2);
                test.Info("Actual Total of all section amounts : $" + actTotal);
                grandTotal = Convert.ToDouble(Pages.Home.Fn_Get_ReportsData_From_DataBase(reportGrandTotalSP, SPParams, "total")[0]);

                if (actTotal.Equals(grandTotal))
                {
                    test.Pass("Total of all section amounts is matching with the Grand Total : $" + grandTotal);
                }
                else
                {
                    test.Fail("Total of all section amounts is not matching with the Grand Total : $" + grandTotal);
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                test.Fail("Failed to total all sections Amounts in Report");
                Assert.Fail("Failed to verify Report data");
            }

        }

        //NetPay = Income - Deduction (Benefit not included)
        //get netpay in [usp_WL_REPORT_PayrollMasterPaycodeSummaryTotals] for orglevel = 1
        //get total of income from usp_WL_REPORT_PayrollMasterPaycodeTotals (Paycode 1)
        //get total of deduction from usp_WL_REPORT_PayrollMasterPaycodeTotals (Paycode 3)
        //PayRegister usp_WL_REPORT_PayrollMasterPaycodeTotals (Find paycode 1 and 3 and deduct to get Netpay in [usp_WL_REPORT_PayrollMasterPaycodeSummaryTotals])
        [TestMethod]
        public void WL_CAN_Payroll_001400_Verify_Standard_Reports_Register_Detail_Register_Summary()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = "userProfile".AppSettings();
            string screen = data.GetTestData("Payroll_Screen");

            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroup");
            string runType = GenericMethods.GetXMLNodeValue(xmlDataFile, "RunType");
            string chequeDate = GenericMethods.GetXMLNodeValue(xmlDataFile, "ChequeDate");
            if (chequeDate == "CURR_DATE")
            {
                chequeDate = DateTime.Now.AddDays(1).ToString("M/d/yyyy");
            }

            string reportSPName = data.GetTestData("Report_SP");

            string reportType = data.GetTestData("Report_Type");
            string reportNames = data.GetTestData("Report_Name");
            string reportFormat = data.GetTestData("Report_Format");

            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), false);
            string queryLastPPID = "lastPayProcessIDQuery".AppSettings().Replace("ppcode", GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroupName"));
            string payProcessID = GlobalDB.ExecuteSQLQuery(queryLastPPID, connection).GetTestData("payroll_process_id").ToString();
            string SPParams = data.GetTestData("SP_Params").Replace("ppid", payProcessID).Replace("yyyy", DateTime.Now.Year.ToString()).Replace("reportDB", "dbReportName".AppSettings());
            #endregion


            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Payroll process Screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll(screen);

            //Calculate Payroll if not available before verifying report
            if (!Pages.Payroll.Fn_Verify_Payroll_Run_In_Payroll_Process(processGrp, runType, "Calculation Processed"))
            {
                test.Info("Payroll not calculated, hence calculating payroll");
                //Calcualte payroll
                if (!Pages.Payroll.Fn_Calculate_Payroll_In_Payroll_Process(processGrp, runType, chequeDate))
                {
                    Assert.Fail("Calculate payroll failed");
                }
            }

            //Verify Standard Register Detail report
            if (Pages.Payroll.Fn_Verify_Reports_In_Payroll_ProcessTable(reportType, reportNames.Split(';')[0], reportFormat))
            {
                test.Pass("Standard Register Detail report opened and verified Successfully");
            }
            else
            {
                GenericMethods.CaptureScreenshot();
                test.Fail("Failed to verify Standard Register Detail report");
                Assert.Fail("Failed to verify Report data");
            }

            DataSet ds;
            double rdCurrentIncome = 0, rdMTDIncome = 0, rdYTDIncome = 0;
            double rdCurrentDed = 0, rdMTDDed = 0, rdYTDDed = 0;
            double rdNetPayCurrent = 0, rdNetPayMTD = 0, rdNetPayYTD = 0;
            double rsNetPayCurrent = 0, rsNetPayMTD = 0, rsNetPayYTD = 0;

            #region Register Detail DB

            //Verify Standard Register Detail report from DB
            if (GlobalDB.IsStoredProcedureExists(connection, reportSPName.Split(';')[0]))
            {
                ds = GlobalDB.ExecuteStoredProc(reportSPName.Split(';')[0], SPParams, connection);

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    if (row["report_group_code_paycode_type_cd"].ToString() == "1")
                    {
                        rdCurrentIncome += Convert.ToDouble(row["currentamt"].ToString());
                        rdMTDIncome += Convert.ToDouble(row["mtd"].ToString());
                        rdYTDIncome += Convert.ToDouble(row["ytd"].ToString());
                    }
                    else if (row["report_group_code_paycode_type_cd"].ToString() == "3")
                    {
                        rdCurrentDed += Convert.ToDouble(row["currentamt"].ToString());
                        rdMTDDed += Convert.ToDouble(row["mtd"].ToString());
                        rdYTDDed += Convert.ToDouble(row["ytd"].ToString());
                    }
                }

                rdNetPayCurrent = rdCurrentIncome - rdCurrentDed;
                rdNetPayMTD = rdMTDIncome - rdMTDDed;
                rdNetPayYTD = rdYTDIncome - rdYTDDed;

                test.Info("Register Detail Net Pay Current amount : " + rdNetPayCurrent);
                test.Info("Register Detail Net Pay MTD : " + rdNetPayMTD);
                test.Info("Register Detail Net Pay YTD : " + rdNetPayYTD);

                if (rdNetPayCurrent != 0 && rdNetPayMTD != 0 && rdNetPayYTD != 0)
                {
                    test.Pass("Standard Register Detail report data verified successfully");
                }
                else
                {
                    test.Fail("Failed to verify Standard Register Detail report data");
                    Assert.Fail("Failed to verify Report data");
                }
            }

            #endregion

            //Navigate to Payroll process Screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll(screen);

            //Verify Standard Register Summary report
            if (Pages.Payroll.Fn_Verify_Reports_In_Payroll_ProcessTable(reportType, reportNames.Split(';')[1], reportFormat))
            {
                test.Pass("Standard Register Summary report opened and verified Successfully");
            }
            else
            {
                GenericMethods.CaptureScreenshot();
                test.Fail("Failed to verify Standard Register Summary report");
                Assert.Fail("Failed to verify Report data");
            }

            #region Register Summary DB

            //Verify Standard Register Summary report from DB
            if (GlobalDB.IsStoredProcedureExists(connection, reportSPName.Split(';')[1]))
            {
                ds = GlobalDB.ExecuteStoredProc(reportSPName.Split(';')[1], SPParams, connection);

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    if (row["paycode_type"].ToString() == "Net Pay" && row["org_level"].ToString() == "1")
                    {
                        rsNetPayCurrent += Convert.ToDouble(row["current_dollars"].ToString());
                        rsNetPayMTD += Convert.ToDouble(row["mtd_dollars"].ToString());
                        rsNetPayYTD += Convert.ToDouble(row["ytd_dollars"].ToString());
                    }
                }

                test.Info("Register Summary Net Pay Current amount : " + rsNetPayCurrent);
                test.Info("Register Summary Net Pay MTD : " + rsNetPayMTD);
                test.Info("Register Summary Net Pay YTD : " + rsNetPayYTD);

                if (rsNetPayCurrent != 0 && rsNetPayMTD != 0 && rsNetPayYTD != 0)
                {
                    test.Pass("Standard Register Summary report data verified successfully");
                }
                else
                {
                    test.Fail("Failed to verify Standard Register Summary report data");
                    Assert.Fail("Failed to verify Report data");
                }
            }

            #endregion

            if (Math.Round(rdNetPayCurrent, 2).Equals(Math.Round(rsNetPayCurrent, 2)))
            {
                test.Pass("Net Pay Current amount is same in both Register Detail and Summary report");
            }
            else
            {
                test.Fail("Net Pay Current amount is not same in Register Detail and Summary report");
            }

            if (Math.Round(rdNetPayMTD, 2).Equals(Math.Round(rsNetPayMTD, 2)))
            {
                test.Pass("Net Pay MTD is same in both Register Detail and Summary report");
            }
            else
            {
                test.Fail("Net Pay MTD is not same in Register Detail and Summary report");
            }

            if (Math.Round(rdNetPayYTD, 2).Equals(Math.Round(rsNetPayYTD, 2)))
            {
                test.Pass("Net Pay YTD is same in both Register Detail and Summary report");
            }
            else
            {
                test.Fail("Net Pay YTD is not same in Register Detail and Summary report");
            }

        }

        [TestMethod]
        public void WL_CAN_Payroll_001600_Verify_Standard_Reports_Changes()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = "userProfile".AppSettings();
            string screen = data.GetTestData("Payroll_Screen");


            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroup");
            string runType = GenericMethods.GetXMLNodeValue(xmlDataFile, "RunType");
            string chequeDate = GenericMethods.GetXMLNodeValue(xmlDataFile, "ChequeDate");
            if (chequeDate == "CURR_DATE")
            {
                chequeDate = DateTime.Now.AddDays(1).ToString("M/d/yyyy");
            }

            string reportType = data.GetTestData("Report_Type");
            string reportName = data.GetTestData("Report_Name");
            string reportFormat = data.GetTestData("Report_Format");

            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");

            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), false);
            string queryLastPPID = "lastPayProcessIDQuery".AppSettings().Replace("ppcode", GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroupName"));
            string payProcessID = GlobalDB.ExecuteSQLQuery(queryLastPPID, connection).GetTestData("payroll_process_id").ToString();
            string SPParams = data.GetTestData("SP_Params").Replace("ppid", payProcessID).Replace("yyyy", DateTime.Now.Year.ToString()).Replace("reportDB", "dbReportName".AppSettings());
            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Payroll process Screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll(screen);

            //Calculate Payroll if not available before verifying report
            if (!Pages.Payroll.Fn_Verify_Payroll_Run_In_Payroll_Process(processGrp, runType, "Calculation Processed"))
            {
                test.Info("Payroll not calculated, hence calculating payroll");
                //Calcualte payroll
                if (!Pages.Payroll.Fn_Calculate_Payroll_In_Payroll_Process(processGrp, runType, chequeDate))
                {
                    GenericMethods.CaptureScreenshot();
                    Assert.Fail("Calculate payroll failed");
                }
            }

            //Verify Standard Changes report
            if (Pages.Payroll.Fn_Verify_Reports_In_Payroll_ProcessTable(reportType, reportName, reportFormat))
            {
                test.Pass("Standard Changes report opened and verified Successfully");
            }
            else
            {
                GenericMethods.CaptureScreenshot();
                test.Fail("Failed to verify Standard Changes report");
                Assert.Fail("Failed to verify Report data");
            }

            test.Info("Verify Standard Changes report data exists");
            //Verify Standard Changes report from DB
            if (GlobalDB.IsStoredProcedureExists(connection, reportSPName))
            {
                if (Pages.Home.Fn_Verify_ReportsData_In_DataBase(reportSPName, SPParams, dataToVerify))
                {
                    test.Pass("Standard Changes report data verified successfully");
                }
                else
                {
                    test.Fail("Failed to verify Standard Changes report data");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                test.Fail("Stored Procedure not found for Standard Changes report verification");
                Assert.Fail("Failed to verify Report data");
            }


        }

        [TestMethod]
        public void WL_CAN_Payroll_001700_Verify_Standard_Reports_Compensation_List()
        {
            #region Data Variable
            string userLang = data.GetTestData("User_Language");
            string userProf = "userProfile".AppSettings();
            string screen = data.GetTestData("Payroll_Screen");


            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroup");
            string runType = GenericMethods.GetXMLNodeValue(xmlDataFile, "RunType");
            string chequeDate = GenericMethods.GetXMLNodeValue(xmlDataFile, "ChequeDate");
            if (chequeDate == "CURR_DATE")
            {
                chequeDate = DateTime.Now.AddDays(1).ToString("M/d/yyyy");
            }

            string reportType = data.GetTestData("Report_Type");
            string reportName = data.GetTestData("Report_Name");
            string reportFormat = data.GetTestData("Report_Format");

            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");

            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), false);
            string queryLastPPID = "lastPayProcessIDQuery".AppSettings().Replace("ppcode", GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroupName"));
            string payProcessID = GlobalDB.ExecuteSQLQuery(queryLastPPID, connection).GetTestData("payroll_process_id").ToString();
            string SPParams = data.GetTestData("SP_Params").Replace("ppid", payProcessID).Replace("yyyy", DateTime.Now.Year.ToString()).Replace("reportDB", "dbReportName".AppSettings());
            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Payroll process Screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll(screen);

            //Calculate Payroll if not available before verifying report
            if (!Pages.Payroll.Fn_Verify_Payroll_Run_In_Payroll_Process(processGrp, runType, "Calculation Processed"))
            {
                test.Info("Payroll not calculated, hence calculating payroll");
                //Calcualte payroll
                if (!Pages.Payroll.Fn_Calculate_Payroll_In_Payroll_Process(processGrp, runType, chequeDate))
                {
                    GenericMethods.CaptureScreenshot();
                    Assert.Fail("Calculate payroll failed");
                }
            }

            //Verify Standard Compensation List report
            if (Pages.Payroll.Fn_Verify_Reports_In_Payroll_ProcessTable(reportType, reportName, reportFormat))
            {
                test.Pass("Standard Compensation List report opened and verified Successfully");
            }
            else
            {
                GenericMethods.CaptureScreenshot();
                test.Fail("Failed to verify Standard Compensation List report");
                Assert.Fail("Failed to verify Report data");
            }

            test.Info("Verify Standard Compensation List report data exists");
            //Verify Standard Compensation List report from DB
            if (GlobalDB.IsStoredProcedureExists(connection, reportSPName))
            {
                if (Pages.Home.Fn_Verify_ReportsData_In_DataBase(reportSPName, SPParams, dataToVerify))
                {
                    test.Pass("Standard Compensation List report data verified successfully");
                }
                else
                {
                    test.Fail("Failed to verify Standard Compensation List report data");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                test.Fail("Stored Procedure not found for Standard Compensation List report verification");
                Assert.Fail("Failed to verify Report data");
            }


        }

        [TestMethod]
        public void WL_CAN_Payroll_001800_Verify_Standard_Reports_Current_Vs_Prior()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = "userProfile".AppSettings();
            string screen = data.GetTestData("Payroll_Screen");


            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroup");
            string runType = GenericMethods.GetXMLNodeValue(xmlDataFile, "RunType");
            string chequeDate = GenericMethods.GetXMLNodeValue(xmlDataFile, "ChequeDate");
            if (chequeDate == "CURR_DATE")
            {
                chequeDate = DateTime.Now.AddDays(1).ToString("M/d/yyyy");
            }

            string reportType = data.GetTestData("Report_Type");
            string reportName = data.GetTestData("Report_Name");
            string reportFormat = data.GetTestData("Report_Format");

            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");

            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), false);
            string queryLastPPID = "lastPayProcessIDQuery".AppSettings().Replace("ppcode", GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroupName"));
            string payProcessID = GlobalDB.ExecuteSQLQuery(queryLastPPID, connection).GetTestData("payroll_process_id").ToString();
            string SPParams = data.GetTestData("SP_Params").Replace("ppid", payProcessID).Replace("yyyy", DateTime.Now.Year.ToString()).Replace("reportDB", "dbReportName".AppSettings());
            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Payroll process Screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll(screen);

            //Calculate Payroll if not available before verifying report
            if (!Pages.Payroll.Fn_Verify_Payroll_Run_In_Payroll_Process(processGrp, runType, "Calculation Processed"))
            {
                test.Info("Payroll not calculated, hence calculating payroll");
                //Calcualte payroll
                if (!Pages.Payroll.Fn_Calculate_Payroll_In_Payroll_Process(processGrp, runType, chequeDate))
                {
                    GenericMethods.CaptureScreenshot();
                    Assert.Fail("Calculate payroll failed");
                }
            }

            //Verify Standard Current Vs Prior report
            if (Pages.Payroll.Fn_Verify_Reports_In_Payroll_ProcessTable(reportType, reportName, reportFormat))
            {
                test.Pass("Standard Current Vs Prior report opened and verified Successfully");
            }
            else
            {
                GenericMethods.CaptureScreenshot();
                test.Fail("Failed to verify Standard Current Vs Prior report");
                Assert.Fail("Failed to verify Report data");
            }

            test.Info("Verify Standard Current Vs Prior report data exists");
            //Verify Standard Current Vs Prior report from DB
            if (GlobalDB.IsStoredProcedureExists(connection, reportSPName))
            {
                if (Pages.Home.Fn_Verify_ReportsData_In_DataBase(reportSPName, SPParams, dataToVerify))
                {
                    test.Pass("Standard Current Vs Prior report data verified successfully");
                }
                else
                {
                    test.Fail("Failed to verify Standard Current Vs Prior report data");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                test.Fail("Stored Procedure not found for Current Vs Prior report verification");
                Assert.Fail("Failed to verify Report data");
            }


        }

        [TestMethod]
        public void WL_CAN_Payroll_001900_Verify_Standard_Reports_Net_Current_Vs_Prior()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = "userProfile".AppSettings();
            string screen = data.GetTestData("Payroll_Screen");


            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroup");
            string runType = GenericMethods.GetXMLNodeValue(xmlDataFile, "RunType");
            string chequeDate = GenericMethods.GetXMLNodeValue(xmlDataFile, "ChequeDate");
            if (chequeDate == "CURR_DATE")
            {
                chequeDate = DateTime.Now.AddDays(1).ToString("M/d/yyyy");
            }

            string reportType = data.GetTestData("Report_Type");
            string reportName = data.GetTestData("Report_Name");
            string reportFormat = data.GetTestData("Report_Format");

            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");

            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), false);
            string queryLastPPID = "lastPayProcessIDQuery".AppSettings().Replace("ppcode", GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroupName"));
            string payProcessID = GlobalDB.ExecuteSQLQuery(queryLastPPID, connection).GetTestData("payroll_process_id").ToString();
            string SPParams = data.GetTestData("SP_Params").Replace("ppid", payProcessID).Replace("yyyy", DateTime.Now.Year.ToString()).Replace("reportDB", "dbReportName".AppSettings());
            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Payroll process Screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll(screen);

            //Calculate Payroll if not available before verifying report
            if (!Pages.Payroll.Fn_Verify_Payroll_Run_In_Payroll_Process(processGrp, runType, "Calculation Processed"))
            {
                test.Info("Payroll not calculated, hence calculating payroll");
                //Calcualte payroll
                if (!Pages.Payroll.Fn_Calculate_Payroll_In_Payroll_Process(processGrp, runType, chequeDate))
                {
                    GenericMethods.CaptureScreenshot();
                    Assert.Fail("Calculate payroll failed");
                }
            }

            //Verify Standard Current Vs Prior Net report
            if (Pages.Payroll.Fn_Verify_Reports_In_Payroll_ProcessTable(reportType, reportName, reportFormat))
            {
                test.Pass("Standard Current Vs Prior Net report opened and verified Successfully");
            }
            else
            {
                GenericMethods.CaptureScreenshot();
                test.Fail("Failed to verify Standard Current Vs Prior Net report");
                Assert.Fail("Failed to verify Report data");
            }

            test.Info("Verify Standard Current Vs Prior Net report data exists");
            //Verify Standard Current Vs Prior Net report from DB
            if (GlobalDB.IsStoredProcedureExists(connection, reportSPName))
            {
                if (Pages.Home.Fn_Verify_ReportsData_In_DataBase(reportSPName, SPParams, dataToVerify))
                {
                    test.Pass("Standard Current Vs Prior Net report data verified successfully");
                }
                else
                {
                    test.Fail("Failed to verify Standard Current Vs Prior Net report data");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                test.Fail("Stored Procedure not found for Current Vs Prior Net report verification");
                Assert.Fail("Failed to verify Report data");
            }
        }

        [TestMethod]
        public void WL_CAN_Payroll_002000_Verify_Standard_Reports_Current_Vs_Prior_Period_Variances()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = "userProfile".AppSettings();
            string screen = data.GetTestData("Payroll_Screen");


            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroup");
            string runType = GenericMethods.GetXMLNodeValue(xmlDataFile, "RunType");
            string chequeDate = GenericMethods.GetXMLNodeValue(xmlDataFile, "ChequeDate");
            if (chequeDate == "CURR_DATE")
            {
                chequeDate = DateTime.Now.AddDays(1).ToString("M/d/yyyy");
            }

            string reportType = data.GetTestData("Report_Type");
            string reportName = data.GetTestData("Report_Name");
            string reportFormat = data.GetTestData("Report_Format");

            string reportSPName = data.GetTestData("Report_SP"); //usp_WL_REPORT_CurrentVsPriorPeriodVariancesDetail
            string dataToVerify = data.GetTestData("Report_Val");

            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), false);
            string queryLastPPID = "lastPayProcessIDQuery".AppSettings().Replace("ppcode", GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroupName"));
            string payProcessID = GlobalDB.ExecuteSQLQuery(queryLastPPID, connection).GetTestData("payroll_process_id").ToString();
            string SPParams = data.GetTestData("SP_Params").Replace("ppid", payProcessID).Replace("yyyy", DateTime.Now.Year.ToString()).Replace("reportDB", "dbReportName".AppSettings());

            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Payroll process Screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll(screen);

            //Calculate Payroll if not available before verifying report
            if (!Pages.Payroll.Fn_Verify_Payroll_Run_In_Payroll_Process(processGrp, runType, "Calculation Processed"))
            {
                test.Info("Payroll not calculated, hence calculating payroll");
                //Calcualte payroll
                if (!Pages.Payroll.Fn_Calculate_Payroll_In_Payroll_Process(processGrp, runType, chequeDate))
                {
                    GenericMethods.CaptureScreenshot();
                    Assert.Fail("Calculate payroll failed");
                }
            }

            //Verify Standard Current Vs Prior Period Variances report
            if (Pages.Payroll.Fn_Verify_Reports_In_Payroll_ProcessTable(reportType, reportName, reportFormat))
            {
                test.Pass("Standard Current Vs Prior Period Variances report opened and verified Successfully");
            }
            else
            {
                GenericMethods.CaptureScreenshot();
                test.Fail("Failed to verify Standard Current Vs Prior Period Variances report");
                Assert.Fail("Failed to verify Report data");
            }

            test.Info("Verify Standard Current Vs Prior Period Variances report data exists");
            //Verify Standard Current Vs Prior Period Variances report from DB
            if (GlobalDB.IsStoredProcedureExists(connection, reportSPName))
            {
                if (Pages.Home.Fn_Verify_ReportsData_In_DataBase(reportSPName, SPParams, dataToVerify))
                {
                    test.Pass("Standard Current Vs Prior Period Variances report data verified successfully");
                }
                else
                {
                    test.Fail("Failed to verify Standard Current Vs Prior Period Variances report data");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                test.Fail("Stored Procedure not found for Standard Current Vs Prior Period Variances report verification");
                Assert.Fail("Failed to verify Report data");
            }
        }

        [TestMethod]
        public void WL_CAN_Payroll_002100_Verify_Standard_Reports_Garnishment()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = "userProfile".AppSettings();
            string screen = data.GetTestData("Payroll_Screen");


            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroup");
            string runType = GenericMethods.GetXMLNodeValue(xmlDataFile, "RunType");
            string chequeDate = GenericMethods.GetXMLNodeValue(xmlDataFile, "ChequeDate");
            if (chequeDate == "CURR_DATE")
            {
                chequeDate = DateTime.Now.AddDays(1).ToString("M/d/yyyy");
            }

            string reportType = data.GetTestData("Report_Type");
            string reportName = data.GetTestData("Report_Name");
            string reportFormat = data.GetTestData("Report_Format");

            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");

            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), false);
            string queryLastPPID = "lastPayProcessIDQuery".AppSettings().Replace("ppcode", GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroupName"));
            string payProcessID = GlobalDB.ExecuteSQLQuery(queryLastPPID, connection).GetTestData("payroll_process_id").ToString();
            string SPParams = data.GetTestData("SP_Params").Replace("ppid", payProcessID).Replace("yyyy", DateTime.Now.Year.ToString()).Replace("reportDB", "dbReportName".AppSettings());
            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Payroll process Screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll(screen);

            //Calculate Payroll if not available before verifying report
            if (!Pages.Payroll.Fn_Verify_Payroll_Run_In_Payroll_Process(processGrp, runType, "Calculation Processed"))
            {
                test.Info("Payroll not calculated, hence calculating payroll");
                //Calcualte payroll
                if (!Pages.Payroll.Fn_Calculate_Payroll_In_Payroll_Process(processGrp, runType, chequeDate))
                {
                    GenericMethods.CaptureScreenshot();
                    Assert.Fail("Calculate payroll failed");
                }
            }

            //Verify Standard Garnishment report
            if (Pages.Payroll.Fn_Verify_Reports_In_Payroll_ProcessTable(reportType, reportName, reportFormat))
            {
                test.Pass("Standard Garnishment report opened and verified Successfully");
            }
            else
            {
                GenericMethods.CaptureScreenshot();
                test.Fail("Failed to verify Standard Garnishment report");
                Assert.Fail("Failed to verify Report data");
            }

            test.Info("Verify Standard Garnishment report data exists");
            //Verify Standard Garnishment report from DB
            if (GlobalDB.IsStoredProcedureExists(connection, reportSPName))
            {
                if (Pages.Home.Fn_Verify_ReportsData_In_DataBase(reportSPName, SPParams, dataToVerify))
                {
                    test.Pass("Standard Garnishment report data verified successfully");
                }
                else
                {
                    test.Fail("Failed to verify Standard Garnishment report data");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                test.Fail("Stored Procedure not found for Standard Garnishment report verification");
                Assert.Fail("Failed to verify Report data");
            }
        }

        [TestMethod]
        public void WL_CAN_Payroll_002200_Verify_Standard_Reports_Pay_Exception()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = "userProfile".AppSettings();
            string screen = data.GetTestData("Payroll_Screen");


            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroup");
            string runType = GenericMethods.GetXMLNodeValue(xmlDataFile, "RunType");
            string chequeDate = GenericMethods.GetXMLNodeValue(xmlDataFile, "ChequeDate");
            if (chequeDate == "CURR_DATE")
            {
                chequeDate = DateTime.Now.AddDays(1).ToString("M/d/yyyy");
            }

            string reportType = data.GetTestData("Report_Type");
            string reportName = data.GetTestData("Report_Name");
            string reportFormat = data.GetTestData("Report_Format");

            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");

            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), false);
            string queryLastPPID = "lastPayProcessIDQuery".AppSettings().Replace("ppcode", GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroupName"));
            string payProcessID = GlobalDB.ExecuteSQLQuery(queryLastPPID, connection).GetTestData("payroll_process_id").ToString();
            string SPParams = data.GetTestData("SP_Params").Replace("ppid", payProcessID).Replace("yyyy", DateTime.Now.Year.ToString()).Replace("reportDB", "dbReportName".AppSettings());
            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Payroll process Screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll(screen);

            //Calculate Payroll if not available before verifying report
            if (!Pages.Payroll.Fn_Verify_Payroll_Run_In_Payroll_Process(processGrp, runType, "Calculation Processed"))
            {
                test.Info("Payroll not calculated, hence calculating payroll");
                //Calcualte payroll
                if (!Pages.Payroll.Fn_Calculate_Payroll_In_Payroll_Process(processGrp, runType, chequeDate))
                {
                    GenericMethods.CaptureScreenshot();
                    Assert.Fail("Calculate payroll failed");
                }
            }

            //Verify Standard Pay Exception report
            if (Pages.Payroll.Fn_Verify_Reports_In_Payroll_ProcessTable(reportType, reportName, reportFormat))
            {
                test.Pass("Standard Pay Exception report opened and verified Successfully");
            }
            else
            {
                GenericMethods.CaptureScreenshot();
                test.Fail("Failed to verify Standard Pay Exception report");
                Assert.Fail("Failed to verify Report data");
            }

            test.Info("Verify Standard Pay Exception report data exists");
            //Verify Standard Pay Exception report from DB
            if (GlobalDB.IsStoredProcedureExists(connection, reportSPName))
            {
                if (Pages.Home.Fn_Verify_ReportsData_In_DataBase(reportSPName, SPParams, dataToVerify))
                {
                    test.Pass("Standard Pay Exception report data verified successfully");
                }
                else
                {
                    test.Fail("Failed to verify Standard Pay Exception report data");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                test.Fail("Stored Procedure not found for Standard Pay Exception report verification");
                Assert.Fail("Failed to verify Report data");
            }
        }

        [TestMethod]
        public void WL_CAN_Payroll_002300_Verify_Standard_Reports_Gross_Vs_Salary()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = "userProfile".AppSettings();
            string screen = data.GetTestData("Payroll_Screen");


            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroup");
            string runType = GenericMethods.GetXMLNodeValue(xmlDataFile, "RunType");
            string chequeDate = GenericMethods.GetXMLNodeValue(xmlDataFile, "ChequeDate");
            if (chequeDate == "CURR_DATE")
            {
                chequeDate = DateTime.Now.AddDays(1).ToString("M/d/yyyy");
            }

            string reportType = data.GetTestData("Report_Type");
            string reportName = data.GetTestData("Report_Name");
            string reportFormat = data.GetTestData("Report_Format");

            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");

            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), false);
            string queryLastPPID = "lastPayProcessIDQuery".AppSettings().Replace("ppcode", GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroupName"));
            string payProcessID = GlobalDB.ExecuteSQLQuery(queryLastPPID, connection).GetTestData("payroll_process_id").ToString();
            string SPParams = data.GetTestData("SP_Params").Replace("ppid", payProcessID).Replace("yyyy", DateTime.Now.Year.ToString()).Replace("reportDB", "dbReportName".AppSettings());
            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Payroll process Screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll(screen);

            //Calculate Payroll if not available before verifying report
            if (!Pages.Payroll.Fn_Verify_Payroll_Run_In_Payroll_Process(processGrp, runType, "Calculation Processed"))
            {
                test.Info("Payroll not calculated, hence calculating payroll");
                //Calcualte payroll
                if (!Pages.Payroll.Fn_Calculate_Payroll_In_Payroll_Process(processGrp, runType, chequeDate))
                {
                    GenericMethods.CaptureScreenshot();
                    Assert.Fail("Calculate payroll failed");
                }
            }

            //Verify Standard Gross Vs Salary report
            if (Pages.Payroll.Fn_Verify_Reports_In_Payroll_ProcessTable(reportType, reportName, reportFormat))
            {
                test.Pass("Standard Gross Vs Salary report opened and verified Successfully");
            }
            else
            {
                GenericMethods.CaptureScreenshot();
                test.Fail("Failed to verify Standard Gross Vs Salary report");
                Assert.Fail("Failed to verify Report data");
            }

            test.Info("Verify Standard Gross Vs Salary report data exists");
            //Verify Standard Gross Vs Salary report from DB
            if (GlobalDB.IsStoredProcedureExists(connection, reportSPName))
            {
                if (Pages.Home.Fn_Verify_ReportsData_In_DataBase(reportSPName, SPParams, dataToVerify))
                {
                    test.Pass("Standard Gross Vs Salary report data verified successfully");
                }
                else
                {
                    test.Fail("Failed to verify Standard Gross Vs Salary report data");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                test.Fail("Stored Procedure not found for Standard Gross Vs Salary report verification");
                Assert.Fail("Failed to verify Report data");
            }
        }

        [TestMethod]
        public void WL_CAN_Payroll_002400_Verify_Standard_Reports_PD7A()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = "userProfile".AppSettings();
            string screen = data.GetTestData("Payroll_Screen");


            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroup");
            string runType = GenericMethods.GetXMLNodeValue(xmlDataFile, "RunType");
            string chequeDate = GenericMethods.GetXMLNodeValue(xmlDataFile, "ChequeDate");
            if (chequeDate == "CURR_DATE")
            {
                chequeDate = DateTime.Now.AddDays(1).ToString("M/d/yyyy");
            }

            string reportType = data.GetTestData("Report_Type");
            string reportName = data.GetTestData("Report_Name");
            string reportFormat = data.GetTestData("Report_Format");

            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");

            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), false);
            string queryLastPPID = "lastPayProcessIDQuery".AppSettings().Replace("ppcode", GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroupName"));
            string payProcessID = GlobalDB.ExecuteSQLQuery(queryLastPPID, connection).GetTestData("payroll_process_id").ToString();
            string SPParams = data.GetTestData("SP_Params").Replace("ppid", payProcessID).Replace("yyyy", DateTime.Now.Year.ToString()).Replace("reportDB", "dbReportName".AppSettings());
            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Payroll process Screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll(screen);

            //Calculate Payroll if not available before verifying report
            if (!Pages.Payroll.Fn_Verify_Payroll_Run_In_Payroll_Process(processGrp, runType, "Calculation Processed"))
            {
                test.Info("Payroll not calculated, hence calculating payroll");
                //Calcualte payroll
                if (!Pages.Payroll.Fn_Calculate_Payroll_In_Payroll_Process(processGrp, runType, chequeDate))
                {
                    GenericMethods.CaptureScreenshot();
                    Assert.Fail("Calculate payroll failed");
                }
            }

            //Verify Standard PD7A report
            if (Pages.Payroll.Fn_Verify_Reports_In_Payroll_ProcessTable(reportType, reportName, reportFormat))
            {
                test.Pass("Standard PD7A report opened and verified Successfully");
            }
            else
            {
                GenericMethods.CaptureScreenshot();
                test.Fail("Failed to verify Standard PD7A report");
                Assert.Fail("Failed to verify Report data");
            }

            test.Info("Verify Standard PD7A report data exists");
            //Verify Standard PD7A report from DB
            if (GlobalDB.IsStoredProcedureExists(connection, reportSPName))
            {
                if (Pages.Home.Fn_Verify_ReportsData_In_DataBase(reportSPName, SPParams, dataToVerify))
                {
                    test.Pass("Standard PD7A report data verified successfully");
                }
                else
                {
                    test.Fail("Failed to verify Standard PD7A report data");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                test.Fail("Stored Procedure not found for Standard PD7A report verification");
                Assert.Fail("Failed to verify Report data");
            }
        }

        [TestMethod]
        public void WL_CAN_Payroll_002500_Verify_Standard_Reports_YTD_Bonus_Commission()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = "userProfile".AppSettings();
            string screen = data.GetTestData("Payroll_Screen");


            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroup");
            string runType = GenericMethods.GetXMLNodeValue(xmlDataFile, "RunType");
            string chequeDate = GenericMethods.GetXMLNodeValue(xmlDataFile, "ChequeDate");
            if (chequeDate == "CURR_DATE")
            {
                chequeDate = DateTime.Now.AddDays(1).ToString("M/d/yyyy");
            }

            string reportType = data.GetTestData("Report_Type");
            string reportName = data.GetTestData("Report_Name");
            string reportFormat = data.GetTestData("Report_Format");

            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");

            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), false);
            string queryLastPPID = "lastPayProcessIDQuery".AppSettings().Replace("ppcode", GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroupName"));
            string payProcessID = GlobalDB.ExecuteSQLQuery(queryLastPPID, connection).GetTestData("payroll_process_id").ToString();
            string SPParams = data.GetTestData("SP_Params").Replace("ppid", payProcessID).Replace("yyyy", DateTime.Now.Year.ToString()).Replace("reportDB", "dbReportName".AppSettings());
            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Payroll process Screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll(screen);

            //Calculate Payroll if not available before verifying report
            if (!Pages.Payroll.Fn_Verify_Payroll_Run_In_Payroll_Process(processGrp, runType, "Calculation Processed"))
            {
                test.Info("Payroll not calculated, hence calculating payroll");
                //Calcualte payroll
                if (!Pages.Payroll.Fn_Calculate_Payroll_In_Payroll_Process(processGrp, runType, chequeDate))
                {
                    GenericMethods.CaptureScreenshot();
                    Assert.Fail("Calculate payroll failed");
                }
            }

            //Verify Standard YTD Bonus Commission report
            if (Pages.Payroll.Fn_Verify_Reports_In_Payroll_ProcessTable(reportType, reportName, reportFormat))
            {
                test.Pass("Standard YTD Bonus Commission report opened and verified Successfully");
            }
            else
            {
                GenericMethods.CaptureScreenshot();
                test.Fail("Failed to verify Standard YTD Bonus Commission report");
                Assert.Fail("Failed to verify Report data");
            }

            test.Info("Verify Standard YTD Bonus Commission report data exists");
            //Verify Standard YTD Bonus Commission report from DB
            if (GlobalDB.IsStoredProcedureExists(connection, reportSPName))
            {
                if (Pages.Home.Fn_Verify_ReportsData_In_DataBase(reportSPName, SPParams, dataToVerify))
                {
                    test.Pass("Standard YTD Bonus Commission report data verified successfully");
                }
                else
                {
                    test.Fail("Failed to verify Standard YTD Bonus Commission report data");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                test.Fail("Stored Procedure not found for Standard YTD Bonus Commission report verification");
                Assert.Fail("Failed to verify Report data");
            }
        }

        //WCBEmployeeData_select
        [TestMethod]
        public void WL_CAN_Payroll_002600_Verify_Standard_Reports_WCB()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = "userProfile".AppSettings();
            string screen = data.GetTestData("Payroll_Screen");


            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroup");
            string runType = GenericMethods.GetXMLNodeValue(xmlDataFile, "RunType");
            string chequeDate = GenericMethods.GetXMLNodeValue(xmlDataFile, "ChequeDate");
            if (chequeDate == "CURR_DATE")
            {
                chequeDate = DateTime.Now.AddDays(1).ToString("M/d/yyyy");
            }

            string reportType = data.GetTestData("Report_Type");
            string reportName = data.GetTestData("Report_Name");
            string reportFormat = data.GetTestData("Report_Format");
            string excelName = data.GetTestData("ExcelReport_Name");

            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");

            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), false);
            string queryLastPPID = "lastPayProcessIDQuery".AppSettings().Replace("ppcode", GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroupName"));
            string payProcessID = GlobalDB.ExecuteSQLQuery(queryLastPPID, connection).GetTestData("payroll_process_id").ToString();
            string SPParams = data.GetTestData("SP_Params").Replace("ppid", payProcessID).Replace("yyyy", DateTime.Now.Year.ToString()).Replace("reportDB", "dbReportName".AppSettings());
            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Payroll process Screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll(screen);

            //Calculate Payroll if not available before verifying report
            if (!Pages.Payroll.Fn_Verify_Payroll_Run_In_Payroll_Process(processGrp, runType, "Calculation Processed"))
            {
                test.Info("Payroll not calculated, hence calculating payroll");
                //Calcualte payroll
                if (!Pages.Payroll.Fn_Calculate_Payroll_In_Payroll_Process(processGrp, runType, chequeDate))
                {
                    GenericMethods.CaptureScreenshot();
                    Assert.Fail("Calculate payroll failed");
                }
            }

            //Verify Standard WCB report
            if (Pages.Payroll.Fn_Verify_Reports_In_Payroll_ProcessTable(reportType, reportName, reportFormat, excelName))
            {
                test.Pass("Standard WCB report opened and verified Successfully");
            }
            else
            {
                GenericMethods.CaptureScreenshot();
                test.Fail("Failed to verify Standard WCB report");
                Assert.Fail("Failed to verify Report data");
            }

            test.Info("Verify Standard WCB report data exists");
            //Verify Standard WCB report from DB
            if (GlobalDB.IsStoredProcedureExists(connection, reportSPName))
            {
                if (Pages.Home.Fn_Verify_ReportsData_In_DataBase(reportSPName, SPParams, dataToVerify))
                {
                    test.Pass("Standard WCB report data verified successfully");
                }
                else
                {
                    test.Fail("Failed to verify Standard WCB report data");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                test.Fail("Stored Procedure not found for Standard WCB report verification");
                Assert.Fail("Failed to verify Report data");
            }
        }

        //[PaycodeDetailReport_select]
        [TestMethod]
        public void WL_CAN_Payroll_002700_Verify_Standard_Reports_Payroll_Details()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = "userProfile".AppSettings();
            string screen = data.GetTestData("Payroll_Screen");


            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroup");
            string runType = GenericMethods.GetXMLNodeValue(xmlDataFile, "RunType");
            string chequeDate = GenericMethods.GetXMLNodeValue(xmlDataFile, "ChequeDate");
            if (chequeDate == "CURR_DATE")
            {
                chequeDate = DateTime.Now.AddDays(1).ToString("M/d/yyyy");
            }

            string reportType = data.GetTestData("Report_Type");
            string reportName = data.GetTestData("Report_Name");
            string reportFormat = data.GetTestData("Report_Format");
            string excelName = data.GetTestData("ExcelReport_Name");

            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");

            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), false);
            string queryLastPPID = "lastPayProcessIDQuery".AppSettings().Replace("ppcode", GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroupName"));
            string payProcessID = GlobalDB.ExecuteSQLQuery(queryLastPPID, connection).GetTestData("payroll_process_id").ToString();
            string SPParams = data.GetTestData("SP_Params").Replace("ppid", payProcessID).Replace("yyyy", DateTime.Now.Year.ToString()).Replace("reportDB", "dbReportName".AppSettings());
            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Payroll process Screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll(screen);

            //Calculate Payroll if not available before verifying report
            if (!Pages.Payroll.Fn_Verify_Payroll_Run_In_Payroll_Process(processGrp, runType, "Calculation Processed"))
            {
                test.Info("Payroll not calculated, hence calculating payroll");
                //Calcualte payroll
                if (!Pages.Payroll.Fn_Calculate_Payroll_In_Payroll_Process(processGrp, runType, chequeDate))
                {
                    GenericMethods.CaptureScreenshot();
                    Assert.Fail("Calculate payroll failed");
                }
            }

            //Verify Standard Payroll Details Report
            if (Pages.Payroll.Fn_Verify_Reports_In_Payroll_ProcessTable(reportType, reportName, reportFormat, excelName))
            {
                test.Pass("Standard Payroll Details Report opened and verified Successfully");
            }
            else
            {
                GenericMethods.CaptureScreenshot();
                test.Fail("Failed to verify Standard Payroll Details Report");
                Assert.Fail("Failed to verify Report data");
            }

            test.Info("Verify Standard Payroll Details Report data exists");
            //Verify Standard Payroll Details Report from DB
            if (GlobalDB.IsStoredProcedureExists(connection, reportSPName))
            {
                if (Pages.Home.Fn_Verify_ReportsData_In_DataBase(reportSPName, SPParams, dataToVerify))
                {
                    test.Pass("Standard Payroll Details Report data verified successfully");
                }
                else
                {
                    test.Fail("Failed to verify Standard Payroll Details Report data");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                test.Fail("Stored Procedure not found for Standard Payroll Details Report verification");
                Assert.Fail("Failed to verify Report data");
            }
        }

        //GlEmployeeDetails_select
        [TestMethod]
        public void WL_CAN_Payroll_002800_Verify_Standard_Reports_GL_Employee_Details()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = "userProfile".AppSettings();
            string screen = data.GetTestData("Payroll_Screen");


            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroup");
            string runType = GenericMethods.GetXMLNodeValue(xmlDataFile, "RunType");
            string chequeDate = GenericMethods.GetXMLNodeValue(xmlDataFile, "ChequeDate");
            if (chequeDate == "CURR_DATE")
            {
                chequeDate = DateTime.Now.AddDays(1).ToString("M/d/yyyy");
            }

            string reportType = data.GetTestData("Report_Type");
            string reportName = data.GetTestData("Report_Name");
            string reportFormat = data.GetTestData("Report_Format");
            string excelName = data.GetTestData("ExcelReport_Name");

            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");

            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), false);
            string queryLastPPID = "lastPayProcessIDQuery".AppSettings().Replace("ppcode", GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroupName"));
            string payProcessID = GlobalDB.ExecuteSQLQuery(queryLastPPID, connection).GetTestData("payroll_process_id").ToString();
            string SPParams = data.GetTestData("SP_Params").Replace("ppid", payProcessID).Replace("yyyy", DateTime.Now.Year.ToString()).Replace("reportDB", "dbReportName".AppSettings());
            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Payroll process Screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll(screen);

            //Calculate Payroll if not available before verifying report
            if (!Pages.Payroll.Fn_Verify_Payroll_Run_In_Payroll_Process(processGrp, runType, "Calculation Processed"))
            {
                test.Info("Payroll not calculated, hence calculating payroll");
                //Calcualte payroll
                if (!Pages.Payroll.Fn_Calculate_Payroll_In_Payroll_Process(processGrp, runType, chequeDate))
                {
                    GenericMethods.CaptureScreenshot();
                    Assert.Fail("Calculate payroll failed");
                }
            }

            //Verify Standard GL Employee Details report
            if (Pages.Payroll.Fn_Verify_Reports_In_Payroll_ProcessTable(reportType, reportName, reportFormat, excelName))
            {
                test.Pass("Standard GL Employee Details report opened and verified Successfully");
            }
            else
            {
                GenericMethods.CaptureScreenshot();
                test.Fail("Failed to verify Standard GL Employee Details report");
                Assert.Fail("Failed to verify Report data");
            }

            test.Info("Verify Standard GL Employee Details report data exists");
            //Verify Standard GL Employee Details report from DB
            if (GlobalDB.IsStoredProcedureExists(connection, reportSPName))
            {
                if (Pages.Home.Fn_Verify_ReportsData_In_DataBase(reportSPName, SPParams, dataToVerify))
                {
                    test.Pass("Standard GL Employee Details report data verified successfully");
                }
                else
                {
                    test.Fail("Failed to verify Standard GL Employee Details report data");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                test.Fail("Stored Procedure not found for Standard GL Employee Details report verification");
                Assert.Fail("Failed to verify Report data");
            }
        }

        //VacationReport_select
        [TestMethod]
        public void WL_CAN_Payroll_002900_Verify_Custom_Reports_Vacation_Report()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = "userProfile".AppSettings();
            string screen = data.GetTestData("Payroll_Screen");


            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroup");
            string runType = GenericMethods.GetXMLNodeValue(xmlDataFile, "RunType");
            string chequeDate = GenericMethods.GetXMLNodeValue(xmlDataFile, "ChequeDate");
            if (chequeDate == "CURR_DATE")
            {
                chequeDate = DateTime.Now.AddDays(1).ToString("M/d/yyyy");
            }

            string reportType = data.GetTestData("Report_Type");
            string reportName = data.GetTestData("Report_Name");
            string reportFormat = data.GetTestData("Report_Format");
            string excelName = data.GetTestData("ExcelReport_Name");

            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");

            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), false);
            string queryLastPPID = "lastPayProcessIDQuery".AppSettings().Replace("ppcode", GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroupName"));
            string payProcessID = GlobalDB.ExecuteSQLQuery(queryLastPPID, connection).GetTestData("payroll_process_id").ToString();
            string SPParams = data.GetTestData("SP_Params").Replace("ppid", payProcessID).Replace("yyyy", DateTime.Now.Year.ToString()).Replace("reportDB", "dbReportName".AppSettings());
            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Payroll process Screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll(screen);

            //Calculate Payroll if not available before verifying report
            if (!Pages.Payroll.Fn_Verify_Payroll_Run_In_Payroll_Process(processGrp, runType, "Calculation Processed"))
            {
                test.Info("Payroll not calculated, hence calculating payroll");
                //Calcualte payroll
                if (!Pages.Payroll.Fn_Calculate_Payroll_In_Payroll_Process(processGrp, runType, chequeDate))
                {
                    GenericMethods.CaptureScreenshot();
                    Assert.Fail("Calculate payroll failed");
                }
            }

            //Verify Custom Vacation report
            if (Pages.Payroll.Fn_Verify_Reports_In_Payroll_ProcessTable(reportType, reportName, reportFormat, excelName))
            {
                test.Pass("Custom Vacation report opened and verified Successfully");
            }
            else
            {
                GenericMethods.CaptureScreenshot();
                test.Fail("Failed to verify Custom Vacation report");
                Assert.Fail("Failed to verify Report data");
            }

            test.Info("Verify Custom Vacation report data exists");
            //Verify Custom Vacation report from DB
            if (GlobalDB.IsStoredProcedureExists(connection, reportSPName))
            {
                if (Pages.Home.Fn_Verify_ReportsData_In_DataBase(reportSPName, SPParams, dataToVerify))
                {
                    test.Pass("Custom Vacation report data verified successfully");
                }
                else
                {
                    test.Fail("Failed to verify Custom Vacation report data");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                test.Fail("Custom Procedure not found for Standard Vacation report verification");
                Assert.Fail("Failed to verify Report data");
            }
        }

        //usp_WL_REPORT_PensionReportDetail
        [TestMethod]
        public void WL_CAN_Payroll_003000_Verify_Custom_Reports_Pension()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = "userProfile".AppSettings();
            string screen = data.GetTestData("Payroll_Screen");


            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroup");
            string runType = GenericMethods.GetXMLNodeValue(xmlDataFile, "RunType");
            string chequeDate = GenericMethods.GetXMLNodeValue(xmlDataFile, "ChequeDate");
            if (chequeDate == "CURR_DATE")
            {
                chequeDate = DateTime.Now.AddDays(1).ToString("M/d/yyyy");
            }

            string reportType = data.GetTestData("Report_Type");
            string reportName = data.GetTestData("Report_Name");
            string reportFormat = data.GetTestData("Report_Format");

            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");

            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), false);
            string queryLastPPID = "lastPayProcessIDQuery".AppSettings().Replace("ppcode", GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroupName"));
            string payProcessID = GlobalDB.ExecuteSQLQuery(queryLastPPID, connection).GetTestData("payroll_process_id").ToString();
            string SPParams = data.GetTestData("SP_Params").Replace("ppid", payProcessID).Replace("yyyy", DateTime.Now.Year.ToString()).Replace("reportDB", "dbReportName".AppSettings());
            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Payroll process Screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll(screen);

            //Calculate Payroll if not available before verifying report
            if (!Pages.Payroll.Fn_Verify_Payroll_Run_In_Payroll_Process(processGrp, runType, "Calculation Processed"))
            {
                test.Info("Payroll not calculated, hence calculating payroll");
                //Calcualte payroll
                if (!Pages.Payroll.Fn_Calculate_Payroll_In_Payroll_Process(processGrp, runType, chequeDate))
                {
                    GenericMethods.CaptureScreenshot();
                    Assert.Fail("Calculate payroll failed");
                }
            }

            //Verify Custom Pension report
            if (Pages.Payroll.Fn_Verify_Reports_In_Payroll_ProcessTable(reportType, reportName, reportFormat))
            {
                test.Pass("Custom Pension report opened and verified Successfully");
            }
            else
            {
                GenericMethods.CaptureScreenshot();
                test.Fail("Failed to verify Custom Pension report");
                Assert.Fail("Failed to verify Report data");
            }

            test.Info("Verify Custom Pension report data exists");
            //Verify Custom Pension report from DB
            if (GlobalDB.IsStoredProcedureExists(connection, reportSPName))
            {
                if (Pages.Home.Fn_Verify_ReportsData_In_DataBase(reportSPName, SPParams, dataToVerify))
                {
                    test.Pass("Custom Pension report data verified successfully");
                }
                else
                {
                    test.Fail("Failed to verify Custom Pension report data");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                test.Fail("Stored Procedure not found for Custom Pension report verification");
                Assert.Fail("Failed to verify Report data");
            }
        }

        //PensionExport_select
        [TestMethod]
        public void WL_CAN_Payroll_003100_Verify_Custom_Reports_Pension_Demographics()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = "userProfile".AppSettings();
            string screen = data.GetTestData("Payroll_Screen");


            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroup");
            string runType = GenericMethods.GetXMLNodeValue(xmlDataFile, "RunType");
            string chequeDate = GenericMethods.GetXMLNodeValue(xmlDataFile, "ChequeDate");
            if (chequeDate == "CURR_DATE")
            {
                chequeDate = DateTime.Now.AddDays(1).ToString("M/d/yyyy");
            }

            string reportType = data.GetTestData("Report_Type");
            string reportName = data.GetTestData("Report_Name");
            string reportFormat = data.GetTestData("Report_Format");
            string excelName = data.GetTestData("ExcelReport_Name");

            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");

            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), false);
            string queryLastPPID = "lastPayProcessIDQuery".AppSettings().Replace("ppcode", GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroupName"));
            string payProcessID = GlobalDB.ExecuteSQLQuery(queryLastPPID, connection).GetTestData("payroll_process_id").ToString();
            string SPParams = data.GetTestData("SP_Params").Replace("ppid", payProcessID).Replace("yyyy", DateTime.Now.Year.ToString()).Replace("reportDB", "dbReportName".AppSettings());
            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Payroll process Screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll(screen);

            //Calculate Payroll if not available before verifying report
            if (!Pages.Payroll.Fn_Verify_Payroll_Run_In_Payroll_Process(processGrp, runType, "Calculation Processed"))
            {
                test.Info("Payroll not calculated, hence calculating payroll");
                //Calcualte payroll
                if (!Pages.Payroll.Fn_Calculate_Payroll_In_Payroll_Process(processGrp, runType, chequeDate))
                {
                    GenericMethods.CaptureScreenshot();
                    Assert.Fail("Calculate payroll failed");
                }
            }

            //Verify Custom Pension Demographics report
            if (Pages.Payroll.Fn_Verify_Reports_In_Payroll_ProcessTable(reportType, reportName, reportFormat, excelName))
            {
                test.Pass("Custom Pension Demographics report opened and verified Successfully");
            }
            else
            {
                GenericMethods.CaptureScreenshot();
                test.Fail("Failed to verify Custom Pension Demographics report");
                Assert.Fail("Failed to verify Report data");
            }

            test.Info("Verify Custom Pension Demographics report data exists");
            //Verify Custom Pension Demographics report from DB
            if (GlobalDB.IsStoredProcedureExists(connection, reportSPName))
            {
                if (Pages.Home.Fn_Verify_ReportsData_In_DataBase(reportSPName, SPParams, dataToVerify))
                {
                    test.Pass("Custom Pension Demographics report data verified successfully");
                }
                else
                {
                    test.Fail("Failed to verify Custom Pension Demographics report data");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                test.Fail("Stored Procedure not found for Custom Pension Demographics report verification");
                Assert.Fail("Failed to verify Report data");
            }
        }

        //CurrentVsPriorTwoPeriodVariancesDetail
        [TestMethod]
        public void WL_CAN_Payroll_003200_Verify_Custom_Reports_Current_Vs_Prior_TwoPeriod_Variances()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = "userProfile".AppSettings();
            string screen = data.GetTestData("Payroll_Screen");


            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroup");
            string runType = GenericMethods.GetXMLNodeValue(xmlDataFile, "RunType");
            string chequeDate = GenericMethods.GetXMLNodeValue(xmlDataFile, "ChequeDate");
            if (chequeDate == "CURR_DATE")
            {
                chequeDate = DateTime.Now.AddDays(1).ToString("M/d/yyyy");
            }

            string reportType = data.GetTestData("Report_Type");
            string reportName = data.GetTestData("Report_Name");
            string reportFormat = data.GetTestData("Report_Format");

            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");

            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), false);
            string queryLastPPID = "lastPayProcessIDQuery".AppSettings().Replace("ppcode", GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroupName"));
            string payProcessID = GlobalDB.ExecuteSQLQuery(queryLastPPID, connection).GetTestData("payroll_process_id").ToString();
            string SPParams = data.GetTestData("SP_Params").Replace("ppid", payProcessID).Replace("yyyy", DateTime.Now.Year.ToString()).Replace("reportDB", "dbReportName".AppSettings());
            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Payroll process Screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll(screen);

            //Calculate Payroll if not available before verifying report
            if (!Pages.Payroll.Fn_Verify_Payroll_Run_In_Payroll_Process(processGrp, runType, "Calculation Processed"))
            {
                test.Info("Payroll not calculated, hence calculating payroll");
                //Calcualte payroll
                if (!Pages.Payroll.Fn_Calculate_Payroll_In_Payroll_Process(processGrp, runType, chequeDate))
                {
                    GenericMethods.CaptureScreenshot();
                    Assert.Fail("Calculate payroll failed");
                }
            }

            //Verify Custom Current Vs Prior Two Period Variances report
            if (Pages.Payroll.Fn_Verify_Reports_In_Payroll_ProcessTable(reportType, reportName, reportFormat))
            {
                test.Pass("Custom Current Vs Prior Two Period Variances report opened and verified Successfully");
            }
            else
            {
                GenericMethods.CaptureScreenshot();
                test.Fail("Failed to verify Custom Current Vs Prior Two Period Variances report");
                Assert.Fail("Failed to verify Report data");
            }

            test.Info("Verify Custom Current Vs Prior Two Period Variances report data exists");
            //Verify Custom Current Vs Prior Two Period Variances report from DB
            if (GlobalDB.IsStoredProcedureExists(connection, reportSPName))
            {
                if (Pages.Home.Fn_Verify_ReportsData_In_DataBase(reportSPName, SPParams, dataToVerify))
                {
                    test.Pass("Custom Current Vs Prior Two Period Variances report data verified successfully");
                }
                else
                {
                    test.Fail("Failed to verify Custom Current Vs Prior Two Period Variances report data");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                test.Fail("Stored Procedure not found for Custom Current Vs Prior Two Period Variances report verification");
                Assert.Fail("Failed to verify Report data");
            }
        }

        [TestMethod]
        public void WL_CAN_Payroll_003300_Undo_Calc_Payroll()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = "userProfile".AppSettings();
            string screen = data.GetTestData("Payroll_Screen");


            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroup");
            string runType = GenericMethods.GetXMLNodeValue(xmlDataFile, "RunType");
            string chequeDate = GenericMethods.GetXMLNodeValue(xmlDataFile, "ChequeDate");
            string runStatus = data.GetTestData("Run_Status");
            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Payroll process Screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll(screen);

            //Undo Payroll if already calculated
            if (Pages.Payroll.Fn_Verify_Payroll_Run_In_Payroll_Process(processGrp, runType, runStatus))
            {
                test.Info("Payroll already calculated, hence undoing payroll calculation");
                //Calcualte payroll
                if (!Pages.Payroll.Fn_Undo_Payroll_Calculation(processGrp))
                {
                    GenericMethods.CaptureScreenshot();
                    Assert.Fail("Undo Payroll failed");
                }
            }
        }

        [TestMethod]
        public void WL_CAN_Payroll_003500_Rerun_Calculate_Payroll_Post_the_Payroll()
        {
            string userLang = data.GetTestData("User_Language");
            string userProf = "userProfile".AppSettings();
            string screen = data.GetTestData("Payroll_Screen");

            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroup");
            string runType = GenericMethods.GetXMLNodeValue(xmlDataFile, "RunType");
            string chequeDate = GenericMethods.GetXMLNodeValue(xmlDataFile, "ChequeDate");
            if (chequeDate == "CURR_DATE")
            {
                chequeDate = DateTime.Now.AddDays(1).ToString("M/d/yyyy");
            }

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Payroll process Screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll(screen);

            //Calcualte payroll
            if (!Pages.Payroll.Fn_Calculate_Payroll_In_Payroll_Process(processGrp, runType, chequeDate))
            {
                GenericMethods.CaptureScreenshot();
                Assert.Fail("Calculate payroll failed");
            }

            //Post Calculated Payroll
            if (!Pages.Payroll.Fn_Post_Calculated_Payroll_In_Payroll_Process())
            {
                GenericMethods.CaptureScreenshot();
                Assert.Fail("Failed to Post payroll");
            }
        }

        //TEST-188
        //WageTypeCatalogJamaica_select
        [TestMethod]
        public void WL_CAN_Payroll_006100_Wage_Type_Catalog()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            //string userProf = data.GetTestData("User_Profile");
            string userProf = "userProfile".AppSettings();
            string reports = data.GetTestData("Report");
            string reportName= data.GetTestData("DownloadFileName");

            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");

            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), false);
            string SPParams = data.GetTestData("SP_Params").Replace("yyyy", DateTime.Now.Year.ToString()).Replace("reportDB", "dbReportName".AppSettings());
            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Export Report
            GenericMethods.DeleteFilesFromDirectory(downloadsFolder, reportName);
            if (!Pages.Payroll.Fn_Navigate_To_Reports_In_Payroll(reports))
            {
                Assert.Fail();
            }
            
            if (GenericMethods.WaitForFileExists(downloadsFolder, reportName))
            {
                test.Pass("Report Wage type catalog download successful");
            }
            else
            {
                test.Fail("Report Wage type catalog download failed");
                Assert.Fail();
            }

            //Verify Wage type catalog report from DB
            test.Info("Verify Wage type catalog report data exists");
            if (GlobalDB.IsStoredProcedureExists(connection, reportSPName))
            {
                if (Pages.Home.Fn_Verify_ReportsData_In_DataBase(reportSPName, SPParams, dataToVerify))
                {
                    test.Pass("Wage type catalog report data verified successfully");
                }
                else
                {
                    test.Fail("Failed to verify Wage type catalog report data");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                test.Fail("Stored Procedure not found for Wage type catalog report verification");
                Assert.Fail("Failed to verify Report data");
            }


        }

        //TEST-189
        [TestMethod]
        public void WL_CAN_Payroll_006200_Paycode_Year_End_Mapping_Report()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            //string userProf = data.GetTestData("User_Profile");
            string userProf = "userProfile".AppSettings();
            string reports = data.GetTestData("Report");

            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            if (!Pages.Payroll.Fn_Navigate_To_Reports_In_Payroll(reports))
            {
                Assert.Fail();
            }

            GenericMethods.DeleteFilesFromDirectory(downloadsFolder, "PaycodeYearEndMapping");

            if (!Pages.HR.Fn_Click_View_Report_And_Verify_PDF_Loaded(reports))
            {
                Assert.Fail();
            }
            else
            {
                Pages.Payroll.ClickExportReport("Excel");
            }

            if (GenericMethods.WaitForFileExists(downloadsFolder, "PaycodeYearEndMapping"))
            {
                test.Pass("Report Paycode Year End Mapping Report download successful");
            }
            else
            {
                test.Fail("Report Paycode Year End Mapping Report download failed");
                Assert.Fail();
            }

        }

        //TEST-190
        [TestMethod]
        public void WL_CAN_Payroll_006300_Non_Stat_Deduction()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            //string userProf = data.GetTestData("User_Profile");
            string userProf = "userProfile".AppSettings();
            string reports = data.GetTestData("Report");
            string date = data.GetTestData("Date");
            string vendor = data.GetTestData("Vendor");

            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            if (!Pages.Payroll.Fn_Navigate_To_Reports_In_Payroll(reports))
            {
                Assert.Fail();
            }

            if(!Pages.Payroll.Fn_View_Verify_Payroll_Report_NonStatDed(date, vendor))
            {
                Assert.Fail();
            }
        }







    }
}
