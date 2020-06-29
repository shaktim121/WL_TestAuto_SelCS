using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.Threading;

namespace WL.TestAuto
{
    [TestClass]
    public class CustomReports_TestSuite : AutomationCore
    {
        private static string xmlDataFile = projectDirectory + "\\TestScripts\\PayrollTests\\ProcessGroupDetails.xml";

        ///TEST-179
        ///[CanadaRevenueAgencyBondAmount_select]
        [TestMethod]
        public void WL_CAN_Payments_005200_CSB()
        {
            #region Data Variables
            string customURL = "STA1220url".AppSettings();
            string LogIn_user = "STA1220user".AppSettings();
            string LogIn_pwd = "STA1220pwd".AppSettings();

            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string screen = data.GetTestData("Payroll_Screen");

            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroupsta1220");
            string runType = GenericMethods.GetXMLNodeValue(xmlDataFile, "RunType");

            string reportType = data.GetTestData("Report_Type");
            string reportName = data.GetTestData("Report_Name");
            string reportFormat = data.GetTestData("Report_Format");

            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");

            string connection = GlobalDB.CreateConnectionString("dataSourceST".AppSettings(), "dbReportNameSTA1220".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), false);

            string queryLastPPID = "lastPostedPayProcessIDQuery".AppSettings().Replace("ppcode", GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroupNamesta1220"));
            string payProcessID = GlobalDB.ExecuteSQLQuery(queryLastPPID, connection).GetTestData("payroll_process_id").ToString();
            string SPParams = data.GetTestData("SP_Params").Replace("ppid", payProcessID).Replace("yyyy", DateTime.Now.Year.ToString()).Replace("reportDB", "dbReportNameSTA1220".AppSettings());
            #endregion

            //Navigate to URL
            Browsers.Goto(customURL);
            
            //LogIn to Application
            //Pages.LogIn.Fn_LogInToApplication(LogIn_user, LogIn_pwd);

            //Change User language after Log In
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Payroll process Screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll(screen);

            //Verify Standard CSB report
            if (Pages.Payroll.Fn_Verify_Reports_In_Payroll_PaymentsTable(processGrp, runType, reportType, reportName, reportFormat))
            {
                test.Pass("Standard CSB report opened and verified Successfully");
            }
            else
            {
                GenericMethods.CaptureScreenshot();
                test.Fail("Failed to verify Standard CSB report");
                Assert.Fail("Failed to verify Report data");
            }

            //Verify Standard CSB report from DB
            //test.Info("Verify Standard CSB report data exists");
            //if (GlobalDB.IsStoredProcedureExists(connection, reportSPName))
            //{
            //    if (Pages.Home.Fn_Verify_ReportsData_In_DataBase(reportSPName, SPParams, dataToVerify))
            //    {
            //        test.Pass("Standard CSB report data verified successfully");
            //    }
            //    else
            //    {
            //        test.Fail("Failed to verify Standard CSB report data");
            //        Assert.Fail("Failed to verify Report data");
            //    }
            //}
            //else
            //{
            //    test.Fail("Stored Procedure not found for Standard CSB report verification");
            //    Assert.Fail("Failed to verify Report data");
            //}

        }

        ///TEST-181
        ///BenefitArrears_Select
        [TestMethod]
        public void WL_CAN_Payments_005400_Arrears()
        {
            #region Data Variables
            string customURL = "STKR1010url".AppSettings();
            string LogIn_user = "STKR1010user".AppSettings();
            string LogIn_pwd = "STKR1010pwd".AppSettings();

            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string screen = data.GetTestData("Payroll_Screen");

            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroupstkr1010");
            string runType = GenericMethods.GetXMLNodeValue(xmlDataFile, "RunType");

            string reportType = data.GetTestData("Report_Type");
            string reportName = data.GetTestData("Report_Name");
            string reportFormat = data.GetTestData("Report_Format");
            string excelName = data.GetTestData("ExcelReport_Name");

            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");

            string connection = GlobalDB.CreateConnectionString("dataSourceST".AppSettings(), "dbReportNameSTKR1010".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), false);

            string queryLastPPID = "lastPostedPayProcessIDQuery".AppSettings().Replace("ppcode", GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroupNamestkr1010"));
            string payProcessID = GlobalDB.ExecuteSQLQuery(queryLastPPID, connection).GetTestData("payroll_process_id").ToString();
            string SPParams = data.GetTestData("SP_Params").Replace("ppid", payProcessID).Replace("yyyy", DateTime.Now.Year.ToString()).Replace("reportDB", "dbReportNameSTKR1010".AppSettings());
            #endregion

            //Navigate to URL
            Browsers.Goto(customURL);

            //LogIn to Application
            Pages.LogIn.Fn_LogInToApplication(LogIn_user, LogIn_pwd);

            //Change User language after Log In
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Payroll process Screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll(screen);

            //Verify Standard Arrears report
            if (Pages.Payroll.Fn_Verify_Reports_In_Payroll_PaymentsTable(processGrp, runType, reportType, reportName, reportFormat, excelName))
            {
                test.Pass("Standard Arrears report opened and verified Successfully");
            }
            else
            {
                GenericMethods.CaptureScreenshot();
                test.Fail("Failed to verify Standard Arrears report");
                Assert.Fail("Failed to verify Report data");
            }

            //Verify Standard Arrears report from DB
            test.Info("Verify Standard Arrears report data exists");
            if (GlobalDB.IsStoredProcedureExists(connection, reportSPName))
            {
                if (Pages.Home.Fn_Verify_ReportsData_In_DataBase(reportSPName, SPParams, dataToVerify, connection))
                {
                    test.Pass("Standard Arrears report data verified successfully");
                }
                else
                {
                    test.Fail("Failed to verify Standard Arrears report data");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                test.Fail("Stored Procedure not found for Standard Arrears report verification");
                Assert.Fail("Failed to verify Report data");
            }
        }

        ///TEST-182
        ///PreYearEndIncome_report
        [TestMethod]
        public void WL_CAN_Payments_005500_Research_and_Development()
        {
            #region Data Variables
            string customURL = "STKR1010url".AppSettings();
            string LogIn_user = "STKR1010user".AppSettings();
            string LogIn_pwd = "STKR1010pwd".AppSettings();

            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string screen = data.GetTestData("Payroll_Screen");

            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroupstkr1010");
            string runType = GenericMethods.GetXMLNodeValue(xmlDataFile, "RunType");

            string reportType = data.GetTestData("Report_Type");
            string reportName = data.GetTestData("Report_Name");
            string reportFormat = data.GetTestData("Report_Format");
            string excelName = data.GetTestData("ExcelReport_Name");

            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");

            string connection = GlobalDB.CreateConnectionString("dataSourceST".AppSettings(), "dbReportNameSTKR1010".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), false);

            string queryLastPPID = "lastPostedPayProcessIDQuery".AppSettings().Replace("ppcode", GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroupNamestkr1010"));
            string payProcessID = GlobalDB.ExecuteSQLQuery(queryLastPPID, connection).GetTestData("payroll_process_id").ToString();
            string SPParams = data.GetTestData("SP_Params").Replace("ppid", payProcessID).Replace("yyyy", DateTime.Now.Year.ToString()).Replace("reportDB", "dbReportNameSTKR1010".AppSettings());
            #endregion

            //Navigate to URL
            Browsers.Goto(customURL);

            //LogIn to Application
            //Pages.LogIn.Fn_LogInToApplication(LogIn_user, LogIn_pwd);

            //Change User language after Log In
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Payroll process Screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll(screen);

            //Verify Custom Research and Development report
            if (Pages.Payroll.Fn_Verify_Reports_In_Payroll_PaymentsTable(processGrp, runType, reportType, reportName, reportFormat, excelName))
            {
                test.Pass("Custom Research and Development report opened and verified Successfully");
            }
            else
            {
                GenericMethods.CaptureScreenshot();
                test.Fail("Failed to verify Custom Research and Development report");
                Assert.Fail("Failed to verify Report data");
            }

            //Verify Custom Research and Development report from DB
            test.Info("Verify Custom Research and Development report data exists");
            if (GlobalDB.IsStoredProcedureExists(connection, reportSPName))
            {
                if (Pages.Home.Fn_Verify_ReportsData_In_DataBase(reportSPName, SPParams, dataToVerify, connection))
                {
                    test.Pass("Custom Research and Development report data verified successfully");
                }
                else
                {
                    test.Fail("Failed to verify Custom Research and Development report data");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                test.Fail("Stored Procedure not found for Custom Research and Development report verification");
                Assert.Fail("Failed to verify Report data");
            }
        }

        ///TEST-183
        ///UnitedWayContributions_select
        [TestMethod]
        public void WL_CAN_Payments_005600_United_Way_Contributions()
        {
            #region Data Variables
            string customURL = "STA1220url".AppSettings();
            string LogIn_user = "STA1220user".AppSettings();
            string LogIn_pwd = "STA1220pwd".AppSettings();

            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string screen = data.GetTestData("Payroll_Screen");

            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroupsta1220");
            string runType = GenericMethods.GetXMLNodeValue(xmlDataFile, "RunType");

            string reportType = data.GetTestData("Report_Type");
            string reportName = data.GetTestData("Report_Name");
            string reportFormat = data.GetTestData("Report_Format");

            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");

            string connection = GlobalDB.CreateConnectionString("dataSourceST".AppSettings(), "dbReportNameSTA1220".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), false);

           
            string queryLastPPID = "lastPostedPayProcessIDQuery".AppSettings().Replace("ppcode", GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroupNamesta1220"));
            string payProcessID = GlobalDB.ExecuteSQLQuery(queryLastPPID, connection).GetTestData("payroll_process_id").ToString();
            string SPParams = data.GetTestData("SP_Params").Replace("ppid", payProcessID).Replace("yyyy", DateTime.Now.Year.ToString()).Replace("reportDB", "dbReportNameSTA1220".AppSettings());
            #endregion

            //Navigate to URL
            Browsers.Goto(customURL);

            //LogIn to Application
            //Pages.LogIn.Fn_LogInToApplication(LogIn_user, LogIn_pwd);

            //Change User language after Log In
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Payroll process Screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll(screen);

            //Verify Custom United Way Contributions report
            if (Pages.Payroll.Fn_Verify_Reports_In_Payroll_PaymentsTable(processGrp, runType, reportType, reportName, reportFormat))
            {
                test.Pass("Custom United Way Contributions report opened and verified Successfully");
            }
            else
            {
                GenericMethods.CaptureScreenshot();
                test.Fail("Failed to verify Custom United Way Contributions report");
                Assert.Fail("Failed to verify Report data");
            }

            //Verify Custom United Way Contributions report from DB
            test.Info("Verify Custom United Way Contributions report data exists");
            if (GlobalDB.IsStoredProcedureExists(connection, reportSPName))
            {
                if (Pages.Home.Fn_Verify_ReportsData_In_DataBase(reportSPName, SPParams, dataToVerify, connection))
                {
                    test.Pass("Custom United Way Contributions report data verified successfully");
                }
                else
                {
                    test.Fail("Failed to verify Custom United Way Contributions report data");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                test.Fail("Stored Procedure not found for Custom United Way Contributions report verification");
                Assert.Fail("Failed to verify Report data");
            }
        }

        ///TEST-185
        ///SrpCheckPensionCodesReachingPaycodeYearlyMax_select
        [TestMethod]
        public void WL_CAN_Payments_005800_SRP()
        {
            #region Data Variables
            string customURL = "STA1225url".AppSettings();
            string LogIn_user = "STA1225user".AppSettings();
            string LogIn_pwd = "STA1225pwd".AppSettings();

            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string screen = data.GetTestData("Payroll_Screen");

            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroupsta1225");
            string runType = GenericMethods.GetXMLNodeValue(xmlDataFile, "RunType");

            string reportType = data.GetTestData("Report_Type");
            string reportName = data.GetTestData("Report_Name");
            string reportFormat = data.GetTestData("Report_Format");

            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");

            string connection = GlobalDB.CreateConnectionString("dataSourceST".AppSettings(), "dbReportNameSTA1225".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), false);

            string queryLastPPID = "lastPostedPayProcessIDQuery".AppSettings().Replace("ppcode", GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroupNamesta1225"));
            string payProcessID = GlobalDB.ExecuteSQLQuery(queryLastPPID, connection).GetTestData("payroll_process_id").ToString();
            string SPParams = data.GetTestData("SP_Params").Replace("ppid", payProcessID).Replace("yyyy", DateTime.Now.Year.ToString()).Replace("reportDB", "dbReportNameSTA1225".AppSettings());
            #endregion

            //Navigate to URL
            Browsers.Goto(customURL);

            //LogIn to Application
            //Pages.LogIn.Fn_LogInToApplication(LogIn_user, LogIn_pwd);

            //Change User language after Log In
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Payroll process Screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll(screen);

            //Verify Custom SRP report
            if (Pages.Payroll.Fn_Verify_Reports_In_Payroll_PaymentsTable(processGrp, runType, reportType, reportName, reportFormat))
            {
                test.Pass("Custom SRP report opened and verified Successfully");
            }
            else
            {
                GenericMethods.CaptureScreenshot();
                test.Fail("Failed to verify Custom SRP report");
                Assert.Fail("Failed to verify Report data");
            }

            //Verify Custom SRP report from DB
            test.Info("Verify Custom SRP report data exists");
            if (GlobalDB.IsStoredProcedureExists(connection, reportSPName))
            {
                if (Pages.Home.Fn_Verify_ReportsData_In_DataBase(reportSPName, SPParams, dataToVerify, connection))
                {
                    test.Pass("Custom SRP report data verified successfully");
                }
                else
                {
                    test.Fail("Failed to verify Custom SRP report data");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                test.Fail("Stored Procedure not found for Custom SRP report verification");
                Assert.Fail("Failed to verify Report data");
            }
        }

        ///TEST-186
        ///A1235GlEmployeeDetails_select
        [TestMethod]
        public void WL_CAN_Payments_005900_GL_Detail()
        {
            #region Data Variables
            string customURL = "STA1235url".AppSettings();
            string LogIn_user = "STA1235user".AppSettings();
            string LogIn_pwd = "STA1235pwd".AppSettings();

            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string screen = data.GetTestData("Payroll_Screen");

            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroupsta1235");
            string runType = GenericMethods.GetXMLNodeValue(xmlDataFile, "RunType");

            string reportType = data.GetTestData("Report_Type");
            string reportName = data.GetTestData("Report_Name");
            string reportFormat = data.GetTestData("Report_Format");
            string excelName = data.GetTestData("ExcelReport_Name");

            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");

            string connection = GlobalDB.CreateConnectionString("dataSourceST".AppSettings(), "dbReportNameSTA1235".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), false);

            string queryLastPPID = "lastPostedPayProcessIDQuery".AppSettings().Replace("ppcode", GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroupNamesta1235"));
            string payProcessID = GlobalDB.ExecuteSQLQuery(queryLastPPID, connection).GetTestData("payroll_process_id").ToString();
            string SPParams = data.GetTestData("SP_Params").Replace("ppid", payProcessID).Replace("yyyy", DateTime.Now.Year.ToString()).Replace("reportDB", "dbReportNameSTA1235".AppSettings());
            #endregion

            //Navigate to URL
            Browsers.Goto(customURL);

            //LogIn to Application
            //Pages.LogIn.Fn_LogInToApplication(LogIn_user, LogIn_pwd);

            //Change User language after Log In
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Payroll process Screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll(screen);

            //Verify Custom GL Details report
            if (Pages.Payroll.Fn_Verify_Reports_In_Payroll_PaymentsTable(processGrp, runType, reportType, reportName, reportFormat, excelName))
            {
                test.Pass("Custom GL Details report opened and verified Successfully");
            }
            else
            {
                GenericMethods.CaptureScreenshot();
                test.Fail("Failed to verify Custom GL Details report");
                Assert.Fail("Failed to verify Report data");
            }

            //Verify Custom GL Details report from DB
            test.Info("Verify Custom GL Details report data exists");
            if (GlobalDB.IsStoredProcedureExists(connection, reportSPName))
            {
                if (Pages.Home.Fn_Verify_ReportsData_In_DataBase(reportSPName, SPParams, dataToVerify, connection))
                {
                    test.Pass("Custom GL Details report data verified successfully");
                }
                else
                {
                    test.Fail("Failed to verify Custom GL Details report data");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                test.Fail("Stored Procedure not found for Custom GL Details report verification");
                Assert.Fail("Failed to verify Report data");
            }

        }

        ///TEST-187
        ///CICPlusWeeklyExport_select
        [TestMethod]
        public void WL_CAN_Payments_006000_CIC_Weekly_Export()
        {
            #region Data Variables
            string customURL = "STA1225url".AppSettings();
            string LogIn_user = "STA1225user".AppSettings();
            string LogIn_pwd = "STA1225pwd".AppSettings();

            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string screen = data.GetTestData("Payroll_Screen");

            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroupsta1225");
            string runType = GenericMethods.GetXMLNodeValue(xmlDataFile, "RunType");
            
            string reportName = data.GetTestData("Report_Name");

            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");

            string connection = GlobalDB.CreateConnectionString("dataSourceST".AppSettings(), "dbReportNameSTA1225".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), false);

            string queryLastPPID = "lastPostedPayProcessIDQuery".AppSettings().Replace("ppcode", GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroupNamesta1225"));
            string payProcessID = GlobalDB.ExecuteSQLQuery(queryLastPPID, connection).GetTestData("payroll_process_id").ToString();
            string SPParams = data.GetTestData("SP_Params").Replace("ppid", payProcessID).Replace("yyyy", DateTime.Now.Year.ToString()).Replace("reportDB", "dbReportNameSTA1225".AppSettings());
            #endregion

            //Navigate to URL
            Browsers.Goto(customURL);

            //LogIn to Application
            //Pages.LogIn.Fn_LogInToApplication(LogIn_user, LogIn_pwd);

            //Change User language after Log In
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Payroll process Screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll(screen);

            //Verify CIC Weekly Export report
            if(Pages.Payroll.Fn_Search_Select_Latest_ProcessGroup(processGrp, runType))
            {
                if(!Pages.Payroll.Fn_Verify_Export_Options_In_Payments(reportName, "AuthCA*.txt"))
                {
                    GenericMethods.CaptureScreenshot();
                    test.Fail("Failed to verify CIC Weekly Export report");
                    Assert.Fail("Failed to verify Report data from UI");
                }
            }
            else
            {
                GenericMethods.CaptureScreenshot();
                test.Fail("Failed to select Payroll process group");
                Assert.Fail("Failed to select Payroll process group");
            }

            //Verify CIC Weekly Export from DB
            test.Info("Verify CIC Weekly Export data exists");
            if (GlobalDB.IsStoredProcedureExists(connection, reportSPName))
            {
                if (Pages.Home.Fn_Verify_ReportsData_In_DataBase(reportSPName, SPParams, dataToVerify, connection))
                {
                    test.Pass("CIC Weekly Export data verified successfully");
                }
                else
                {
                    test.Fail("Failed to verify CIC Weekly Export data");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                test.Fail("Stored Procedure not found for CIC Weekly Export verification");
                Assert.Fail("Failed to verify Report data");
            }
        }

    }

}
