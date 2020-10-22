using System;
using System.Net;
using System.Threading;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.PageObjects;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using System.Data;
using iText.Kernel.Pdf.Annot;

namespace WL.TestAuto
{
    [TestClass]
    public class YearEnd_TestSuite : AutomationCore
    {
        public string xmlDataFile = projectDirectory + "\\TestScripts\\PayrollTests\\ProcessGroupDetails.xml";

        ///TEST-191
        ///YearEndStatDeductionException_select
        [TestMethod]
        public void TEST_191_WL_CAN_YE_001000_Stat_Deduction_Exception()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string options = data.GetTestData("Admin_Options");
            string reportName = data.GetTestData("Report_Name");
            string reportFormat = data.GetTestData("Report_Format");

            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");
            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), false);
            string SPParams = data.GetTestData("SP_Params").Replace("yyyy", (DateTime.Now.Year - 1).ToString()).Replace("reportDB", "dbReportName".AppSettings());
            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Year End>Year End screen in Admin
            Pages.Admin.Fn_Navigate_Through_Admin(options);

            //Verify Stat Deduction Exception report
            if (Pages.Admin.Fn_Verify_Record_Displayed_In_YearEndTable())
            {
                if(Pages.Admin.Fn_Verify_Reports_In_YearEndTable(reportName, reportFormat))
                {
                    test.Pass("Stat Deduction Exception report opened and verified Successfully");
                }
                else
                {
                    GenericMethods.CaptureScreenshot();
                    test.Fail("Failed to verify Stat Deduction Exception report");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                GenericMethods.CaptureScreenshot();
                test.Fail("No record found in Table");
                Assert.Fail("Failed to verify Report data");
            }

            test.Info("Verify Stat Deduction Exception report data exists");
            //Verify Stat Deduction Exception report from DB
            if (GlobalDB.IsStoredProcedureExists(connection, reportSPName))
            {
                if (Pages.Home.Fn_Verify_ReportsData_In_DataBase(reportSPName, SPParams, dataToVerify))
                {
                    test.Pass("Stat Deduction Exception report data verified successfully");
                }
                else
                {
                    test.Fail("Failed to verify Stat Deduction Exception report data");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                test.Fail("Stored Procedure not found for Stat Deduction Exception report verification");
                Assert.Fail("Failed to verify Report data");
            }
        }

        ///TEST-192
        ///NameAddressReportException_select        
        [TestMethod]
        public void TEST_192_WL_CAN_YE_001100_Exception_Report()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string options = data.GetTestData("Admin_Options");
            string reportName = data.GetTestData("Report_Name");
            string reportFormat = data.GetTestData("Report_Format");

            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");
            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), false);
            string SPParams = data.GetTestData("SP_Params").Replace("yyyy", (DateTime.Now.Year - 1).ToString()).Replace("reportDB", "dbReportName".AppSettings());
            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Year End>Year End screen in Admin
            Pages.Admin.Fn_Navigate_Through_Admin(options);

            //Verify Exception report
            if (Pages.Admin.Fn_Verify_Record_Displayed_In_YearEndTable())
            {
                if (Pages.Admin.Fn_Verify_Reports_In_YearEndTable(reportName, reportFormat))
                {
                    test.Pass("Exception report opened and verified Successfully");
                }
                else
                {
                    GenericMethods.CaptureScreenshot();
                    test.Fail("Failed to verify Exception report");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                GenericMethods.CaptureScreenshot();
                test.Fail("No record found in Table");
                Assert.Fail("Failed to verify Report data");
            }

            test.Info("Verify Exception report data exists");
            //Verify Exception report from DB
            if (GlobalDB.IsStoredProcedureExists(connection, reportSPName))
            {
                if (Pages.Home.Fn_Verify_ReportsData_In_DataBase(reportSPName, SPParams, dataToVerify))
                {
                    test.Pass("Exception report data verified successfully");
                }
                else
                {
                    test.Fail("Failed to verify Exception report data");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                test.Fail("Stored Procedure not found for Exception report verification");
                Assert.Fail("Failed to verify Report data");
            }
        }

        ///TEST-193
        ///YearEndWcbReport_select
        ///YearEndWcbReportGrandTotals_select
        [TestMethod]
        public void TEST_193_WL_CAN_YE_001200_WCB()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string options = data.GetTestData("Admin_Options");
            string reportName = data.GetTestData("Report_Name");
            string reportFormat = data.GetTestData("Report_Format");

            string excelName = data.GetTestData("ExcelReport_Name");

            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");
            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), false);
            string SPParams = data.GetTestData("SP_Params").Replace("yyyy", (DateTime.Now.Year - 1).ToString()).Replace("reportDB", "dbReportName".AppSettings());
            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Year End>Year End screen in Admin
            Pages.Admin.Fn_Navigate_Through_Admin(options);

            //Verify WCB report
            if (Pages.Admin.Fn_Verify_Record_Displayed_In_YearEndTable())
            {
                if (Pages.Admin.Fn_Verify_Reports_In_YearEndTable(reportName, reportFormat, excelName))
                {
                    test.Pass("WCB report opened and verified Successfully");
                }
                else
                {
                    GenericMethods.CaptureScreenshot();
                    test.Fail("Failed to verify WCB report");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                GenericMethods.CaptureScreenshot();
                test.Fail("No record found in Table");
                Assert.Fail("Failed to verify Report data");
            }

            test.Info("Verify WCB report data exists");
            //Verify WCB report from DB
            if (GlobalDB.IsStoredProcedureExists(connection, reportSPName))
            {
                if (Pages.Home.Fn_Verify_ReportsData_In_DataBase(reportSPName, SPParams, dataToVerify))
                {
                    test.Pass("WCB report data verified successfully");
                }
                else
                {
                    test.Fail("Failed to verify WCB report data");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                test.Fail("Stored Procedure not found for WCB report verification");
                Assert.Fail("Failed to verify Report data");
            }
        }

        ///TEST-194
        ///YearEndBoxTotals_select
        [TestMethod]
        public void TEST_194_WL_CAN_YE_001300_YE_Form_Employee_Summary()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string options = data.GetTestData("Admin_Options");
            string reportName = data.GetTestData("Report_Name");
            string reportFormat = data.GetTestData("Report_Format");

            string excelName = data.GetTestData("ExcelReport_Name");

            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");
            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), false);
            string SPParams = data.GetTestData("SP_Params").Replace("yyyy", (DateTime.Now.Year - 1).ToString()).Replace("reportDB", "dbReportName".AppSettings());
            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Year End>Year End screen in Admin
            Pages.Admin.Fn_Navigate_Through_Admin(options);

            //Verify YE Form Employee Summary report
            if (Pages.Admin.Fn_Verify_Record_Displayed_In_YearEndTable())
            {
                if (Pages.Admin.Fn_Verify_Reports_In_YearEndTable(reportName, reportFormat, excelName))
                {
                    test.Pass("YE Form Employee Summary report opened and verified Successfully");
                }
                else
                {
                    GenericMethods.CaptureScreenshot();
                    test.Fail("Failed to verify YE Form Employee Summary report");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                GenericMethods.CaptureScreenshot();
                test.Fail("No record found in Table");
                Assert.Fail("Failed to verify Report data");
            }

            test.Info("Verify YE Form Employee Summary report data exists");
            //Verify YE Form Employee Summary report from DB
            if (GlobalDB.IsStoredProcedureExists(connection, reportSPName))
            {
                if (Pages.Home.Fn_Verify_ReportsData_In_DataBase(reportSPName, SPParams, dataToVerify))
                {
                    test.Pass("YE Form Employee Summary report data verified successfully");
                }
                else
                {
                    test.Fail("Failed to verify YE Form Employee Summary report data");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                test.Fail("Stored Procedure not found for YE Form Employee Summary report verification");
                Assert.Fail("Failed to verify Report data");
            }
        }

        ///TEST-196
        ///EHTEarnings_select
        [TestMethod]
        public void TEST_196_WL_CAN_YE_001400_Health_Tax_Summary()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string options = data.GetTestData("Admin_Options");
            string reportName = data.GetTestData("Report_Name");
            string reportFormat = data.GetTestData("Report_Format");

            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");
            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), false);
            string SPParams = data.GetTestData("SP_Params").Replace("yyyy", (DateTime.Now.Year - 1).ToString()).Replace("reportDB", "dbReportName".AppSettings());
            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Year End>Year End screen in Admin
            Pages.Admin.Fn_Navigate_Through_Admin(options);

            //Verify Health Tax Summary report
            if (Pages.Admin.Fn_Verify_Record_Displayed_In_YearEndTable())
            {
                if (Pages.Admin.Fn_Verify_Reports_In_YearEndTable(reportName, reportFormat))
                {
                    test.Pass("Health Tax Summary report opened and verified Successfully");
                }
                else
                {
                    GenericMethods.CaptureScreenshot();
                    test.Fail("Failed to verify Health Tax Summary report");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                GenericMethods.CaptureScreenshot();
                test.Fail("No record found in Table");
                Assert.Fail("Failed to verify Report data");
            }

            test.Info("Verify Health Tax Summary report data exists");
            //Verify Health Tax Summary report from DB
            if (GlobalDB.IsStoredProcedureExists(connection, reportSPName))
            {
                if (Pages.Home.Fn_Verify_ReportsData_In_DataBase(reportSPName, SPParams, dataToVerify))
                {
                    test.Pass("Health Tax Summary report data verified successfully");
                }
                else
                {
                    test.Fail("Failed to verify Health Tax Summary report data");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                test.Fail("Stored Procedure not found for Health Tax Summary report verification");
                Assert.Fail("Failed to verify Report data");
            }
        }

        ///TEST-197
        ///R1YearEndDetail_select
        [TestMethod]
        public void TEST_197_WL_CAN_YE_001500_Year_End_R1_Detail()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string options = data.GetTestData("Admin_Options");
            string reportName = data.GetTestData("Report_Name");
            string reportFormat = data.GetTestData("Report_Format");

            string excelName = data.GetTestData("ExcelReport_Name");

            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");
            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), false);
            string SPParams = data.GetTestData("SP_Params").Replace("yyyy", (DateTime.Now.Year - 1).ToString()).Replace("reportDB", "dbReportName".AppSettings());
            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Year End>Year End screen in Admin
            Pages.Admin.Fn_Navigate_Through_Admin(options);

            //Verify Year End R1 Detail report
            if (Pages.Admin.Fn_Verify_Record_Displayed_In_YearEndTable())
            {
                if (Pages.Admin.Fn_Verify_Reports_In_YearEndTable(reportName, reportFormat, excelName))
                {
                    test.Pass("Year End R1 Detail report opened and verified Successfully");
                }
                else
                {
                    GenericMethods.CaptureScreenshot();
                    test.Fail("Failed to verify Year End R1 Detail report");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                GenericMethods.CaptureScreenshot();
                test.Fail("No record found in Table");
                Assert.Fail("Failed to verify Report data");
            }

            test.Info("Verify Year End R1 Detail report data exists");
            //Verify Year End R1 Detail report from DB
            if (GlobalDB.IsStoredProcedureExists(connection, reportSPName))
            {
                if (Pages.Home.Fn_Verify_ReportsData_In_DataBase(reportSPName, SPParams, dataToVerify))
                {
                    test.Pass("Year End R1 Detail report data verified successfully");
                }
                else
                {
                    test.Fail("Failed to verify Year End R1 Detail report data");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                test.Fail("Stored Procedure not found for Year End R1 Detail report verification");
                Assert.Fail("Failed to verify Report data");
            }
        }

        ///TEST-198
        ///T4YearEndDetail_select
        [TestMethod]
        public void TEST_198_WL_CAN_YE_001600_Year_End_T4_Detail()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string options = data.GetTestData("Admin_Options");
            string reportName = data.GetTestData("Report_Name");
            string reportFormat = data.GetTestData("Report_Format");

            string excelName = data.GetTestData("ExcelReport_Name");

            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");
            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), false);
            string SPParams = data.GetTestData("SP_Params").Replace("yyyy", (DateTime.Now.Year - 1).ToString()).Replace("reportDB", "dbReportName".AppSettings());
            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Year End>Year End screen in Admin
            Pages.Admin.Fn_Navigate_Through_Admin(options);

            //Verify Year End T4 Detail report
            if (Pages.Admin.Fn_Verify_Record_Displayed_In_YearEndTable())
            {
                if (Pages.Admin.Fn_Verify_Reports_In_YearEndTable(reportName, reportFormat, excelName))
                {
                    test.Pass("Year End T4 Detail report opened and verified Successfully");
                }
                else
                {
                    GenericMethods.CaptureScreenshot();
                    test.Fail("Failed to verify Year End T4 Detail report");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                GenericMethods.CaptureScreenshot();
                test.Fail("No record found in Table");
                Assert.Fail("Failed to verify Report data");
            }

            test.Info("Verify Year End T4 Detail report data exists");
            //Verify Year End T4 Detail report from DB
            if (GlobalDB.IsStoredProcedureExists(connection, reportSPName))
            {
                if (Pages.Home.Fn_Verify_ReportsData_In_DataBase(reportSPName, SPParams, dataToVerify))
                {
                    test.Pass("Year End T4 Detail report data verified successfully");
                }
                else
                {
                    test.Fail("Failed to verify Year End T4 Detail report data");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                test.Fail("Stored Procedure not found for Year End T4 Detail report verification");
                Assert.Fail("Failed to verify Report data");
            }
        }

        ///TEST-199
        ///T4AYearEndDetail_select
        [TestMethod]
        public void TEST_199_WL_CAN_YE_001700_Year_End_T4A_Detail()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string options = data.GetTestData("Admin_Options");
            string reportName = data.GetTestData("Report_Name");
            string reportFormat = data.GetTestData("Report_Format");

            string excelName = data.GetTestData("ExcelReport_Name");

            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");
            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), false);
            string SPParams = data.GetTestData("SP_Params").Replace("yyyy", (DateTime.Now.Year - 1).ToString()).Replace("reportDB", "dbReportName".AppSettings());
            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Year End>Year End screen in Admin
            Pages.Admin.Fn_Navigate_Through_Admin(options);

            //Verify Year End T4A Detail report
            if (Pages.Admin.Fn_Verify_Record_Displayed_In_YearEndTable())
            {
                if (Pages.Admin.Fn_Verify_Reports_In_YearEndTable(reportName, reportFormat, excelName))
                {
                    test.Pass("Year End T4A Detail report opened and verified Successfully");
                }
                else
                {
                    GenericMethods.CaptureScreenshot();
                    test.Fail("Failed to verify Year End T4A Detail report");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                GenericMethods.CaptureScreenshot();
                test.Fail("No record found in Table");
                Assert.Fail("Failed to verify Report data");
            }

            test.Info("Verify Year End T4A Detail report data exists");
            //Verify Year End T4A Detail report from DB
            if (GlobalDB.IsStoredProcedureExists(connection, reportSPName))
            {
                if (Pages.Home.Fn_Verify_ReportsData_In_DataBase(reportSPName, SPParams, dataToVerify))
                {
                    test.Pass("Year End T4A Detail report data verified successfully");
                }
                else
                {
                    test.Fail("Failed to verify Year End T4A Detail report data");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                test.Fail("Stored Procedure not found for Year End T4A Detail report verification");
                Assert.Fail("Failed to verify Report data");
            }
        }

        ///TEST-200
        ///YearEndAdjustmentReport_select
        [TestMethod]
        public void TEST_200_WL_CAN_YE_001800_Adjustment_Audit()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string options = data.GetTestData("Admin_Options");
            string reportName = data.GetTestData("Report_Name");
            string reportFormat = data.GetTestData("Report_Format");

            string excelName = data.GetTestData("ExcelReport_Name");

            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");
            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), false);
            string SPParams = data.GetTestData("SP_Params").Replace("yyyy", (DateTime.Now.Year - 1).ToString()).Replace("reportDB", "dbReportName".AppSettings());
            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Year End>Year End screen in Admin
            Pages.Admin.Fn_Navigate_Through_Admin(options);

            //Verify Year End Adjustments Audit report
            if (Pages.Admin.Fn_Verify_Record_Displayed_In_YearEndTable())
            {
                if (Pages.Admin.Fn_Verify_Reports_In_YearEndTable(reportName, reportFormat, excelName))
                {
                    test.Pass("Year End Adjustments Audit report opened and verified Successfully");
                }
                else
                {
                    GenericMethods.CaptureScreenshot();
                    test.Fail("Failed to verify Year End Adjustments Audit report");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                GenericMethods.CaptureScreenshot();
                test.Fail("No record found in Table");
                Assert.Fail("Failed to verify Report data");
            }

            test.Info("Verify Year End Adjustments Audit report data exists");
            //Verify Year End Adjustments Audit report from DB
            if (GlobalDB.IsStoredProcedureExists(connection, reportSPName))
            {
                if (Pages.Home.Fn_Verify_ReportsData_In_DataBase(reportSPName, SPParams, dataToVerify))
                {
                    test.Pass("Year End Adjustments Audit report data verified successfully");
                }
                else
                {
                    test.Fail("Failed to verify Year End Adjustments Audit report data");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                test.Fail("Stored Procedure not found for Year End Adjustments Audit report verification");
                Assert.Fail("Failed to verify Report data");
            }
        }

        ///TEST-250
        ///YearEndAdjustmentsAuditReport_select
        [TestMethod]
        public void TEST_250_WL_CAN_YE_001900_Adjustments_Audit_Detail()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string options = data.GetTestData("Admin_Options");
            string reportName = data.GetTestData("Report_Name");
            string reportFormat = data.GetTestData("Report_Format");

            string excelName = data.GetTestData("ExcelReport_Name");

            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");
            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), false);
            string SPParams = data.GetTestData("SP_Params").Replace("yyyy", (DateTime.Now.Year - 1).ToString()).Replace("reportDB", "dbReportName".AppSettings());
            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Year End>Year End screen in Admin
            Pages.Admin.Fn_Navigate_Through_Admin(options);

            //Verify Year End Adjustments Audit Detail report
            if (Pages.Admin.Fn_Verify_Record_Displayed_In_YearEndTable())
            {
                if (Pages.Admin.Fn_Verify_Reports_In_YearEndTable(reportName, reportFormat, excelName))
                {
                    test.Pass("Year End Adjustments Audit Detail report opened and verified Successfully");
                }
                else
                {
                    GenericMethods.CaptureScreenshot();
                    test.Fail("Failed to verify Year End Adjustments Audit Detail report");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                GenericMethods.CaptureScreenshot();
                test.Fail("No record found in Table");
                Assert.Fail("Failed to verify Report data");
            }

            test.Info("Verify Year End Adjustments Audit Detail report data exists");
            //Verify Year End Adjustments Audit Detail report from DB
            if (GlobalDB.IsStoredProcedureExists(connection, reportSPName))
            {
                if (Pages.Home.Fn_Verify_ReportsData_In_DataBase(reportSPName, SPParams, dataToVerify))
                {
                    test.Pass("Year End Adjustments Audit Detail report data verified successfully");
                }
                else
                {
                    test.Fail("Failed to verify Year End Adjustments Audit Detail report data");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                test.Fail("Stored Procedure not found for Year End Adjustments Audit Detail report verification");
                Assert.Fail("Failed to verify Report data");
            }
        }

        ///TEST-256
        ///YearEndAdjustmentsAuditReport_select
        [TestMethod]
        public void TEST_256_WL_CAN_YE_002000_Year_End_NR4_Detail()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string options = data.GetTestData("Admin_Options");
            string reportName = data.GetTestData("Report_Name");
            string reportFormat = data.GetTestData("Report_Format");

            string excelName = data.GetTestData("ExcelReport_Name");

            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");
            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), false);
            string SPParams = data.GetTestData("SP_Params").Replace("yyyy", (DateTime.Now.Year - 1).ToString()).Replace("reportDB", "dbReportName".AppSettings());
            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Year End>Year End screen in Admin
            Pages.Admin.Fn_Navigate_Through_Admin(options);

            //Verify Year End NR4 Detail report
            if (Pages.Admin.Fn_Verify_Record_Displayed_In_YearEndTable())
            {
                if (Pages.Admin.Fn_Verify_Reports_In_YearEndTable(reportName, reportFormat, excelName))
                {
                    test.Pass("Year End NR4 Detail report opened and verified Successfully");
                }
                else
                {
                    GenericMethods.CaptureScreenshot();
                    test.Fail("Failed to verify Year End NR4 Detail report");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                GenericMethods.CaptureScreenshot();
                test.Fail("No record found in Table");
                Assert.Fail("Failed to verify Report data");
            }

            test.Info("Verify Year End NR4 Detail report data exists");
            //Verify Year End NR4 Detail report from DB
            if (GlobalDB.IsStoredProcedureExists(connection, reportSPName))
            {
                if (Pages.Home.Fn_Verify_ReportsData_In_DataBase(reportSPName, SPParams, dataToVerify))
                {
                    test.Pass("Year End NR4 Detail report data verified successfully");
                }
                else
                {
                    test.Fail("Failed to verify Year End NR4 Detail report data");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                test.Fail("Stored Procedure not found for Year End NR4 Detail report verification");
                Assert.Fail("Failed to verify Report data");
            }
        }

        ///TEST-257
        [TestMethod]
        public void TEST_257_WL_CAN_YE_002100_Year_End_Adjustment_Form_NR4()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string options = data.GetTestData("Admin_Options");
            string yearYE = (DateTime.Now.Year - 1).ToString();
            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroup");

            //string reportName = data.GetTestData("Report_Name");
            string reportFormat = data.GetTestData("Report_Format");
            //string excelName = data.GetTestData("ExcelReport_Name");

            //string reportSPName = data.GetTestData("Report_SP");
            //string dataToVerify = data.GetTestData("Report_Val");
            //string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), false);
            //string SPParams = data.GetTestData("SP_Params").Replace("yyyy", (DateTime.Now.Year - 1).ToString()).Replace("reportDB", "dbReportName".AppSettings());
            #endregion
            
            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Year End>Year End Adjustments screen in Admin
            Pages.Admin.Fn_Navigate_Through_Admin(options);

            //Search for records with form type in YE Adjustments
            Pages.Admin.Fn_Search_Records_In_YE_Adjustments(yearYE, "Semi-Monthly", reportFormat);

            //Verify Report in YE Adjustments
            if (Pages.Admin.Fn_Verify_FormType_In_YE_Adjustments())
            {
                test.Pass(reportFormat + " Form verified successfully in Year End Adjustment");
            }
            else
            {
                test.Fail("Failed to verify Form " + reportFormat + " in Year End Adjustment");
                Assert.Fail();
            }


        }

        ///TEST-258
        [TestMethod]
        public void TEST_258_WL_CAN_YE_002200_Year_End_Adjustment_Form_R1()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string options = data.GetTestData("Admin_Options");
            string yearYE = (DateTime.Now.Year - 1).ToString();
            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroup");
            string reportFormat = data.GetTestData("Report_Format");
            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Year End>Year End Adjustments screen in Admin
            Pages.Admin.Fn_Navigate_Through_Admin(options);

            //Search for records with form type in YE Adjustments
            Pages.Admin.Fn_Search_Records_In_YE_Adjustments(yearYE, "Semi-Monthly", reportFormat);

            //Verify Report in YE Adjustments
            if (Pages.Admin.Fn_Verify_FormType_In_YE_Adjustments())
            {
                test.Pass(reportFormat + " Form verified successfully in Year End Adjustment");
            }
            else
            {
                test.Fail("Failed to verify Form " + reportFormat + " in Year End Adjustment");
                Assert.Fail();
            }

        }

        ///TEST-259
        [TestMethod]
        public void TEST_259_WL_CAN_YE_002300_Year_End_Adjustment_Form_R2()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string options = data.GetTestData("Admin_Options");
            string yearYE = (DateTime.Now.Year - 1).ToString();
            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroup");
            string reportFormat = data.GetTestData("Report_Format");
            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Year End>Year End Adjustments screen in Admin
            Pages.Admin.Fn_Navigate_Through_Admin(options);

            //Search for records with form type in YE Adjustments
            Pages.Admin.Fn_Search_Records_In_YE_Adjustments(yearYE, "Semi-Monthly", reportFormat);

            //Verify Report in YE Adjustments
            if (Pages.Admin.Fn_Verify_FormType_In_YE_Adjustments())
            {
                test.Pass(reportFormat + " Form verified successfully in Year End Adjustment");
            }
            else
            {
                test.Fail("Failed to verify Form " + reportFormat + " in Year End Adjustment");
                Assert.Fail();
            }

        }

        ///TEST-260
        [TestMethod]
        public void TEST_260_WL_CAN_YE_002400_Year_End_Adjustment_Form_T4()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string options = data.GetTestData("Admin_Options");
            string yearYE = (DateTime.Now.Year - 1).ToString();
            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroup");
            string reportFormat = data.GetTestData("Report_Format");
            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Year End>Year End Adjustments screen in Admin
            Pages.Admin.Fn_Navigate_Through_Admin(options);

            //Search for records with form type in YE Adjustments
            Pages.Admin.Fn_Search_Records_In_YE_Adjustments(yearYE, "Semi-Monthly", reportFormat);

            //Verify Report in YE Adjustments
            if(Pages.Admin.Fn_Verify_FormType_In_YE_Adjustments())
            {
                test.Pass(reportFormat + " Form verified successfully in Year End Adjustment");
            }
            else
            {
                test.Fail("Failed to verify Form " + reportFormat + " in Year End Adjustment");
                Assert.Fail();
            }

        }

        ///TEST-261
        [TestMethod]
        public void TEST_261_WL_CAN_YE_002500_Year_End_Adjustment_Form_T4A()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string options = data.GetTestData("Admin_Options");
            string yearYE = (DateTime.Now.Year - 1).ToString();
            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroup");
            string reportFormat = data.GetTestData("Report_Format");
            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Year End>Year End Adjustments screen in Admin
            Pages.Admin.Fn_Navigate_Through_Admin(options);

            //Search for records with form type in YE Adjustments
            Pages.Admin.Fn_Search_Records_In_YE_Adjustments(yearYE, "Semi-Monthly", reportFormat);

            //Verify Report in YE Adjustments
            if (Pages.Admin.Fn_Verify_FormType_In_YE_Adjustments())
            {
                test.Pass(reportFormat + " Form verified successfully in Year End Adjustment");
            }
            else
            {
                test.Fail("Failed to verify Form " + reportFormat + " in Year End Adjustment");
                Assert.Fail();
            }

        }

        ///TEST-262
        [TestMethod]
        public void TEST_262_WL_CAN_YE_002600_Year_End_Adjustment_Form_T4ARCA()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string options = data.GetTestData("Admin_Options");
            string yearYE = (DateTime.Now.Year - 1).ToString();
            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroup");
            string reportFormat = data.GetTestData("Report_Format");
            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Year End>Year End Adjustments screen in Admin
            Pages.Admin.Fn_Navigate_Through_Admin(options);

            //Search for records with form type in YE Adjustments
            Pages.Admin.Fn_Search_Records_In_YE_Adjustments(yearYE, "Semi-Monthly", reportFormat);

            //Verify Report in YE Adjustments
            if (Pages.Admin.Fn_Verify_FormType_In_YE_Adjustments())
            {
                test.Pass(reportFormat + " Form verified successfully in Year End Adjustment");
            }
            else
            {
                test.Fail("Failed to verify Form " + reportFormat + " in Year End Adjustment");
                Assert.Fail();
            }

        }

        ///TEST-263
        [TestMethod]
        public void TEST_263_WL_CAN_YE_002700_Year_End_Adjustment_Form_T4RSP()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string options = data.GetTestData("Admin_Options");
            string yearYE = (DateTime.Now.Year - 1).ToString();
            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroup");
            string reportFormat = data.GetTestData("Report_Format");
            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Year End>Year End Adjustments screen in Admin
            Pages.Admin.Fn_Navigate_Through_Admin(options);

            //Search for records with form type in YE Adjustments
            Pages.Admin.Fn_Search_Records_In_YE_Adjustments(yearYE, processGrp, reportFormat);

            //Verify Report in YE Adjustments
            if (Pages.Admin.Fn_Verify_FormType_In_YE_Adjustments())
            {
                test.Pass(reportFormat + " Form verified successfully in Year End Adjustment");
            }
            else
            {
                test.Fail("Failed to verify Form " + reportFormat + " in Year End Adjustment");
                Assert.Fail();
            }

        }

        ///TEST-229
        ///T4MassPrint_report
        [TestMethod]
        public void TEST_229_WL_CAN_YE_002800_YEMass_T4_Print()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string options = data.GetTestData("Admin_Options");
            string reportName = data.GetTestData("Report_Name");

            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");
            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), false);
            string SPParams = data.GetTestData("SP_Params").Replace("yyyy", (DateTime.Now.Year - 1).ToString()).Replace("reportDB", "dbReportName".AppSettings());
            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Year End>Year End screen in Admin
            Pages.Admin.Fn_Navigate_Through_Admin(options);

            //Verify Year End Mass T4 Print report
            if (Pages.Admin.Fn_Verify_Record_Displayed_In_YearEndTable())
            {
                if (Pages.Admin.Fn_Verify_MassPrint_In_YearEndTable(reportName))
                {
                    test.Pass("Year End "+reportName+" report opened and verified Successfully");
                }
                else
                {
                    GenericMethods.CaptureScreenshot();
                    test.Fail("Failed to verify Year End "+reportName+" report");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                GenericMethods.CaptureScreenshot();
                test.Fail("No record found in Table");
                Assert.Fail("Failed to verify Report data");
            }

            test.Info("Verify Year End Mass T4 Print report data exists");
            //Verify Year End Mass T4 Print report from DB
            if (GlobalDB.IsStoredProcedureExists(connection, reportSPName))
            {
                if (Pages.Home.Fn_Verify_ReportsData_In_DataBase(reportSPName, SPParams, dataToVerify))
                {
                    test.Pass("Year End Mass T4 Print report data verified successfully");
                }
                else
                {
                    test.Fail("Failed to verify Year End Mass T4 Print report data");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                test.Fail("Stored Procedure not found for Year End Mass T4 Print report verification");
                Assert.Fail("Failed to verify Report data");
            }
        }

        ///TEST-230
        ///exec R1MassPrint_report @databaseName=N'qaauto',@year=N'2019',@excludeFromYearEndMassFile=N'False',@excludeFromYearEndMassPrint=N'True'
        [TestMethod]
        public void TEST_230_WL_CAN_YE_002900_YEMass_R1_Print()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string options = data.GetTestData("Admin_Options");
            string reportName = data.GetTestData("Report_Name");

            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");
            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), false);
            string SPParams = data.GetTestData("SP_Params").Replace("yyyy", (DateTime.Now.Year - 1).ToString()).Replace("reportDB", "dbReportName".AppSettings());
            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Year End>Year End screen in Admin
            Pages.Admin.Fn_Navigate_Through_Admin(options);

            //Verify Year End Mass R1 Print report
            if (Pages.Admin.Fn_Verify_Record_Displayed_In_YearEndTable())
            {
                if (Pages.Admin.Fn_Verify_MassPrint_In_YearEndTable(reportName))
                {
                    test.Pass("Year End " + reportName + " report opened and verified Successfully");
                }
                else
                {
                    GenericMethods.CaptureScreenshot();
                    test.Fail("Failed to verify Year End " + reportName + " report");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                GenericMethods.CaptureScreenshot();
                test.Fail("No record found in Table");
                Assert.Fail("Failed to verify Report data");
            }

            test.Info("Verify Year End Mass R1 Print report data exists");
            //Verify Year End Mass R1 Print report from DB
            if (GlobalDB.IsStoredProcedureExists(connection, reportSPName))
            {
                if (Pages.Home.Fn_Verify_ReportsData_In_DataBase(reportSPName, SPParams, dataToVerify))
                {
                    test.Pass("Year End Mass R1 Print report data verified successfully");
                }
                else
                {
                    test.Fail("Failed to verify Year End Mass R1 Print report data");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                test.Fail("Stored Procedure not found for Year End Mass R1 Print report verification");
                Assert.Fail("Failed to verify Report data");
            }
        }

        ///TEST-231
        ///T4AMassPrint_report
        [TestMethod]
        public void TEST_231_WL_CAN_YE_003000_YEMass_T4A_Print()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string options = data.GetTestData("Admin_Options");
            string reportName = data.GetTestData("Report_Name");

            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");
            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), false);
            string SPParams = data.GetTestData("SP_Params").Replace("yyyy", (DateTime.Now.Year - 1).ToString()).Replace("reportDB", "dbReportName".AppSettings());
            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Year End>Year End screen in Admin
            Pages.Admin.Fn_Navigate_Through_Admin(options);

            //Verify Year End Mass T4A Print report
            if (Pages.Admin.Fn_Verify_Record_Displayed_In_YearEndTable())
            {
                if (Pages.Admin.Fn_Verify_MassPrint_In_YearEndTable(reportName))
                {
                    test.Pass("Year End " + reportName + " report opened and verified Successfully");
                }
                else
                {
                    GenericMethods.CaptureScreenshot();
                    test.Fail("Failed to verify Year End " + reportName + " report");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                GenericMethods.CaptureScreenshot();
                test.Fail("No record found in Table");
                Assert.Fail("Failed to verify Report data");
            }

            test.Info("Verify Year End Mass T4A Print report data exists");
            //Verify Year End Mass T4A Print report from DB
            if (GlobalDB.IsStoredProcedureExists(connection, reportSPName))
            {
                if (Pages.Home.Fn_Verify_ReportsData_In_DataBase(reportSPName, SPParams, dataToVerify))
                {
                    test.Pass("Year End Mass T4A Print report data verified successfully");
                }
                else
                {
                    test.Fail("Failed to verify Year End Mass T4A Print report data");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                test.Fail("Stored Procedure not found for Year End Mass T4A Print report verification");
                Assert.Fail("Failed to verify Report data");
            }
        }

        ///TEST-232
        ///T4AMassPrint_report
        [TestMethod]
        public void TEST_232_WL_CAN_YE_003100_YEMass_R2_Print()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string options = data.GetTestData("Admin_Options");
            string reportName = data.GetTestData("Report_Name");

            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");
            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), false);
            string SPParams = data.GetTestData("SP_Params").Replace("yyyy", (DateTime.Now.Year - 1).ToString()).Replace("reportDB", "dbReportName".AppSettings());
            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Year End>Year End screen in Admin
            Pages.Admin.Fn_Navigate_Through_Admin(options);

            //Verify Year End Mass R2 Print report
            if (Pages.Admin.Fn_Verify_Record_Displayed_In_YearEndTable())
            {
                if (Pages.Admin.Fn_Verify_MassPrint_In_YearEndTable(reportName))
                {
                    test.Pass("Year End " + reportName + " report opened and verified Successfully");
                }
                else
                {
                    GenericMethods.CaptureScreenshot();
                    test.Fail("Failed to verify Year End " + reportName + " report");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                GenericMethods.CaptureScreenshot();
                test.Fail("No record found in Table");
                Assert.Fail("Failed to verify Report data");
            }

            test.Info("Verify Year End Mass R2 Print report data exists");
            //Verify Year End Mass R2 Print report from DB
            if (GlobalDB.IsStoredProcedureExists(connection, reportSPName))
            {
                if (Pages.Home.Fn_Verify_ReportsData_In_DataBase(reportSPName, SPParams, dataToVerify))
                {
                    test.Pass("Year End Mass R2 Print report data verified successfully");
                }
                else
                {
                    test.Fail("Failed to verify Year End Mass R2 Print report data");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                test.Fail("Stored Procedure not found for Year End Mass R2 Print report verification");
                Assert.Fail("Failed to verify Report data");
            }
        }

        ///TEST-233
        [TestMethod]
        public void TEST_233_WL_CAN_YE_003200_YEMass_File_All()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string options = data.GetTestData("Admin_Options");
            string reportName = data.GetTestData("Report_Name"); //MGH_CA_GIBI_Yearend_Forms_yyyyMMdd.zip

            //string reportSPName = data.GetTestData("Report_SP");
            //string dataToVerify = data.GetTestData("Report_Val");
            //string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), false);
            //string SPParams = data.GetTestData("SP_Params").Replace("yyyy", (DateTime.Now.Year - 1).ToString()).Replace("reportDB", "dbReportName".AppSettings());
            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Year End>Year End screen in Admin
            Pages.Admin.Fn_Navigate_Through_Admin(options);

            //Verify Year End Mass All download
            if (Pages.Admin.Fn_Verify_Record_Displayed_In_YearEndTable())
            {
                if (Pages.Admin.Fn_Verify_MassFileAll_In_YearEndTable(reportName))
                {
                    test.Pass("Year End Mass File All report verified Successfully");
                }
                else
                {
                    GenericMethods.CaptureScreenshot();
                    test.Fail("Failed to verify Year End Mass File All report");
                }
            }
            else
            {
                GenericMethods.CaptureScreenshot();
                test.Fail("No record found in Table");
            }

            //test.Info("Verify Year End Mass R2 Print report data exists");
            ////Verify Year End Mass R2 Print report from DB
            //if (GlobalDB.IsStoredProcedureExists(connection, reportSPName))
            //{
            //    if (Pages.Home.Fn_Verify_ReportsData_In_DataBase(reportSPName, SPParams, dataToVerify))
            //    {
            //        test.Pass("Year End Mass R2 Print report data verified successfully");
            //    }
            //    else
            //    {
            //        test.Fail("Failed to verify Year End Mass R2 Print report data");
            //        Assert.Fail("Failed to verify Report data");
            //    }
            //}
            //else
            //{
            //    test.Fail("Stored Procedure not found for Year End Mass R2 Print report verification");
            //    Assert.Fail("Failed to verify Report data");
            //}
        }

        ///TEST-234
        ///exec YearEndT4_select @databaseName=N'qaauto',@year=2019,@businessNumberId=1,@getOriginals=1
        [TestMethod]
        public void TEST_234_WL_CAN_YE_003300_YET4_Export()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string options = data.GetTestData("Admin_Options");
            string reportName = data.GetTestData("Report_Name");
            string downloadFileName = data.GetTestData("Download_FileName");
            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");
            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), false);
            string SPParams = data.GetTestData("SP_Params").Replace("yyyy", (DateTime.Now.Year - 1).ToString()).Replace("reportDB", "dbReportName".AppSettings());
            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Year End>Year End screen in Admin
            Pages.Admin.Fn_Navigate_Through_Admin(options);

            //Verify Year End T4 Export
            if (Pages.Admin.Fn_Verify_Record_Displayed_In_YearEndTable())
            {
                if (Pages.Admin.Fn_Verify_Export_In_YearEndTable(reportName, downloadFileName))
                {
                    test.Pass("Year End " + reportName + " verified Successfully");
                }
                else
                {
                    GenericMethods.CaptureScreenshot();
                    test.Fail("Failed to verify Year End " + reportName);
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                GenericMethods.CaptureScreenshot();
                test.Fail("No record found in Table");
                Assert.Fail("Failed to verify Report data");
            }

            test.Info("Verify Year End T4 Export data exists");
            //Verify Year End T4 Export from DB
            if (GlobalDB.IsStoredProcedureExists(connection, reportSPName))
            {
                if (Pages.Home.Fn_Verify_ReportsData_In_DataBase(reportSPName, SPParams, dataToVerify))
                {
                    test.Pass("Year End T4 Export data verified successfully");
                }
                else
                {
                    test.Fail("Failed to verify Year End T4 Export data");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                test.Fail("Stored Procedure not found for Year End T4 Export verification");
                Assert.Fail("Failed to verify Report data");
            }
        }

        ///TEST-235
        ///Run in Manual QA
        ///exec YearEndR1_select @year=2019,@getOriginals=0,@getAmended=1
        [TestMethod]
        public void TEST_235_WL_CAN_YE_003400_YER1_Export()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string options = data.GetTestData("Admin_Options");
            string reportName = data.GetTestData("Report_Name");
            string downloadFileName = data.GetTestData("Download_FileName");
            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");
            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), false);
            string SPParams = data.GetTestData("SP_Params").Replace("yyyy", (DateTime.Now.Year - 1).ToString()).Replace("reportDB", "dbReportName".AppSettings());
            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Year End>Year End screen in Admin
            Pages.Admin.Fn_Navigate_Through_Admin(options);

            //Verify Year End R1 Export
            if (Pages.Admin.Fn_Verify_Record_Displayed_In_YearEndTable())
            {
                if (Pages.Admin.Fn_Verify_Export_In_YearEndTable(reportName, downloadFileName))
                {
                    test.Pass("Year End " + reportName + " verified Successfully");
                }
                else
                {
                    GenericMethods.CaptureScreenshot();
                    test.Fail("Failed to verify Year End " + reportName);
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                GenericMethods.CaptureScreenshot();
                test.Fail("No record found in Table");
                Assert.Fail("Failed to verify Report data");
            }

            test.Info("Verify Year End R1 Export data exists");
            //Verify Year End R1 Export from DB
            if (GlobalDB.IsStoredProcedureExists(connection, reportSPName))
            {
                if (Pages.Home.Fn_Verify_ReportsData_In_DataBase(reportSPName, SPParams, dataToVerify))
                {
                    test.Pass("Year End R1 Export data verified successfully");
                }
                else
                {
                    test.Fail("Failed to verify Year End R1 Export data");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                test.Fail("Stored Procedure not found for Year End R1 Export verification");
                Assert.Fail("Failed to verify Report data");
            }
        }

        ///TEST-236
        ///exec YearEndT4A_select @databaseName=N'qaauto',@year=2019,@businessNumberId=1,@getOriginals=1
        [TestMethod]
        public void TEST_236_WL_CAN_YE_003500_YET4A_Export()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string options = data.GetTestData("Admin_Options");
            string reportName = data.GetTestData("Report_Name");
            string downloadFileName = data.GetTestData("Download_FileName");
            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");
            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), false);
            string SPParams = data.GetTestData("SP_Params").Replace("yyyy", (DateTime.Now.Year - 1).ToString()).Replace("reportDB", "dbReportName".AppSettings());
            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Year End>Year End screen in Admin
            Pages.Admin.Fn_Navigate_Through_Admin(options);

            //Verify Year End T4A Export
            if (Pages.Admin.Fn_Verify_Record_Displayed_In_YearEndTable())
            {
                if (Pages.Admin.Fn_Verify_Export_In_YearEndTable(reportName, downloadFileName))
                {
                    test.Pass("Year End " + reportName + " verified Successfully");
                }
                else
                {
                    GenericMethods.CaptureScreenshot();
                    test.Fail("Failed to verify Year End " + reportName);
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                GenericMethods.CaptureScreenshot();
                test.Fail("No record found in Table");
                Assert.Fail("Failed to verify Report data");
            }

            test.Info("Verify Year End T4A Export data exists");
            //Verify Year End T4A Export from DB
            if (GlobalDB.IsStoredProcedureExists(connection, reportSPName))
            {
                if (Pages.Home.Fn_Verify_ReportsData_In_DataBase(reportSPName, SPParams, dataToVerify))
                {
                    test.Pass("Year End T4A Export data verified successfully");
                }
                else
                {
                    test.Fail("Failed to verify Year End T4A Export data");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                test.Fail("Stored Procedure not found for Year End T4A Export verification");
                Assert.Fail("Failed to verify Report data");
            }
        }

        ///TEST-240
        ///exec T4SummaryBusinessTaxNumber_select @databaseName=N'qaauto',@employerNumber=N'REDUCED'
        ///exec T4BoxSums_select @databaseName=N'qaauto',@employerNumber=N'REDUCED',@year=N'2019',@revision='0'
        [TestMethod]
        public void TEST_240_WL_CAN_YE_003900_YET4_Summary_Print()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string options = data.GetTestData("Admin_Options");
            string reportName = data.GetTestData("Report_Name");
            string reportSPName = data.GetTestData("Report_SP");
            string reportSPName2 = data.GetTestData("Report_SP2");
            string dataToVerify = data.GetTestData("Report_Val");
            string dataToVerify2 = data.GetTestData("Report_Val2");
            string SPParams = data.GetTestData("SP_Params").Replace("yyyy", (DateTime.Now.Year - 1).ToString()).Replace("reportDB", "dbReportName".AppSettings());
            string SPParams2 = data.GetTestData("SP_Params2").Replace("yyyy", (DateTime.Now.Year - 1).ToString()).Replace("reportDB", "dbReportName".AppSettings());
            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), false);
            
            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Year End>Year End screen in Admin
            Pages.Admin.Fn_Navigate_Through_Admin(options);

            //Verify Year End T4 Summary Print
            if (Pages.Admin.Fn_Verify_Record_Displayed_In_YearEndTable())
            {
                if (Pages.Admin.Fn_Verify_Summary_In_YearEndTable(reportName))
                {
                    test.Pass("Year End " + reportName + " verified Successfully");
                }
                else
                {
                    GenericMethods.CaptureScreenshot();
                    test.Fail("Failed to verify Year End " + reportName);
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                GenericMethods.CaptureScreenshot();
                test.Fail("No record found in Table");
                Assert.Fail("Failed to verify Report data");
            }

            test.Info("Verify Year End T4 Summary Print data exists");
            //Verify Year End T4 Summary Print Business Tax Number from DB
            if (GlobalDB.IsStoredProcedureExists(connection, reportSPName))
            {
                if (Pages.Home.Fn_Verify_ReportsData_In_DataBase(reportSPName, SPParams, dataToVerify))
                {
                    test.Pass("Year End T4 Summary Print data Business Tax Number verified successfully");
                }
                else
                {
                    test.Fail("Failed to verify Year End T4 Summary Print data Business Tax Number");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                test.Fail("Stored Procedure not found for Year End T4 Summary Print verification");
                Assert.Fail("Failed to verify Report data");
            }

            //Verify Year End T4 Summary Print contains Non Zero values from DB
            if (GlobalDB.IsStoredProcedureExists(connection, reportSPName2))
            {
                if (Pages.Home.Fn_Verify_ReportsData_In_DataBase(reportSPName2, SPParams2, dataToVerify2))
                {
                    test.Pass("Year End T4 Summary Print data contains Non Zero values verified successfully");
                }
                else
                {
                    test.Fail("Failed to verify Year End T4 Summary Print data contains Non Zero values");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                test.Fail("Stored Procedure not found for Year End T4 Summary Print verification");
                Assert.Fail("Failed to verify Report data");
            }
        }

        ///TEST-241
        ///exec R1SummaryOfSourceDeductionsR2BoxJ_select @databaseName=N'qaauto',@year=N'2019',@revision=N'0'
        ///exec R1QCBusinessNumber_select @databaseName=N'qaauto'
        [TestMethod]
        public void TEST_241_WL_CAN_YE_004000_YER1_Summary_Ded()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string options = data.GetTestData("Admin_Options");
            string reportName = data.GetTestData("Report_Name");
            string reportSPName = data.GetTestData("Report_SP");
            string reportSPName2 = data.GetTestData("Report_SP2");
            string dataToVerify = data.GetTestData("Report_Val");
            string dataToVerify2 = data.GetTestData("Report_Val2");
            string SPParams = data.GetTestData("SP_Params").Replace("yyyy", (DateTime.Now.Year - 1).ToString()).Replace("reportDB", "dbReportName".AppSettings());
            string SPParams2 = data.GetTestData("SP_Params2").Replace("yyyy", (DateTime.Now.Year - 1).ToString()).Replace("reportDB", "dbReportName".AppSettings());
            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), false);
            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Year End>Year End screen in Admin
            Pages.Admin.Fn_Navigate_Through_Admin(options);

            //Verify Year End R1 Summary Ded
            if (Pages.Admin.Fn_Verify_Record_Displayed_In_YearEndTable())
            {
                if (Pages.Admin.Fn_Verify_Summary_In_YearEndTable(reportName))
                {
                    test.Pass("Year End " + reportName + " verified Successfully");
                }
                else
                {
                    GenericMethods.CaptureScreenshot();
                    test.Fail("Failed to verify Year End " + reportName);
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                GenericMethods.CaptureScreenshot();
                test.Fail("No record found in Table");
                Assert.Fail("Failed to verify Report data");
            }

            test.Info("Verify Year End R1 Summary Ded data exists");
            //Verify Year End R1 Summary Ded Business Tax Number from DB
            if (GlobalDB.IsStoredProcedureExists(connection, reportSPName))
            {
                if (Pages.Home.Fn_Verify_ReportsData_In_DataBase(reportSPName, SPParams, dataToVerify))
                {
                    test.Pass("Year End R1 Summary Ded data Business Tax Number verified successfully");
                }
                else
                {
                    test.Fail("Failed to verify Year End R1 Summary Ded data Business Tax Number");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                test.Fail("Stored Procedure not found for Year End R1 Summary Ded verification");
                Assert.Fail("Failed to verify Report data");
            }

            //Verify Year End R1 Summary Ded contains Non Zero values from DB
            if (GlobalDB.IsStoredProcedureExists(connection, reportSPName2))
            {
                if (Pages.Home.Fn_Verify_ReportsData_In_DataBase(reportSPName2, SPParams2, dataToVerify2))
                {
                    test.Pass("Year End R1 Summary Ded data contains Non Zero values verified successfully");
                }
                else
                {
                    test.Fail("Failed to verify Year End R1 Summary Ded data contains Non Zero values");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                test.Fail("Stored Procedure not found for Year End R1 Summary Ded verification");
                Assert.Fail("Failed to verify Report data");
            }
        }

        ///TEST-242
        ///exec T4ASummaryReportBoxes_select @databaseName=N'qaauto',@year=N'2019',@employerNumber=N'REDUCED',@revision=N'0'
        ///exec T4ASummaryEmployerNameandTaxNumber_select @databaseName=N'qaauto',@employerNumber=N'REDUCED'
        [TestMethod]
        public void TEST_242_WL_CAN_YE_004100_YET4A_Summary()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string options = data.GetTestData("Admin_Options");
            string reportName = data.GetTestData("Report_Name");
            string reportSPName = data.GetTestData("Report_SP");
            string reportSPName2 = data.GetTestData("Report_SP2");
            string dataToVerify = data.GetTestData("Report_Val");
            string dataToVerify2 = data.GetTestData("Report_Val2");
            string SPParams = data.GetTestData("SP_Params").Replace("yyyy", (DateTime.Now.Year - 1).ToString()).Replace("reportDB", "dbReportName".AppSettings());
            string SPParams2 = data.GetTestData("SP_Params2").Replace("yyyy", (DateTime.Now.Year - 1).ToString()).Replace("reportDB", "dbReportName".AppSettings());
            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), false);
            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Year End>Year End screen in Admin
            Pages.Admin.Fn_Navigate_Through_Admin(options);

            //Verify Year End T4A Summary
            if (Pages.Admin.Fn_Verify_Record_Displayed_In_YearEndTable())
            {
                if (Pages.Admin.Fn_Verify_Summary_In_YearEndTable(reportName))
                {
                    test.Pass("Year End " + reportName + " verified Successfully");
                }
                else
                {
                    GenericMethods.CaptureScreenshot();
                    test.Fail("Failed to verify Year End " + reportName);
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                GenericMethods.CaptureScreenshot();
                test.Fail("No record found in Table");
                Assert.Fail("Failed to verify Report data");
            }

            test.Info("Verify Year End T4A Summary data exists");
            //Verify Year End T4A Summary Business Tax Number from DB
            if (GlobalDB.IsStoredProcedureExists(connection, reportSPName))
            {
                if (Pages.Home.Fn_Verify_ReportsData_In_DataBase(reportSPName, SPParams, dataToVerify))
                {
                    test.Pass("Year End T4A Summary data Business Tax Number verified successfully");
                }
                else
                {
                    test.Fail("Failed to verify Year End T4A Summary data Business Tax Number");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                test.Fail("Stored Procedure not found for Year End T4A Summary verification");
                Assert.Fail("Failed to verify Report data");
            }

            //Verify Year End T4A Summary contains Non Zero values from DB
            if (GlobalDB.IsStoredProcedureExists(connection, reportSPName2))
            {
                if (Pages.Home.Fn_Verify_ReportsData_In_DataBase(reportSPName2, SPParams2, dataToVerify2))
                {
                    test.Pass("Year End T4A Summary data contains Non Zero values verified successfully");
                }
                else
                {
                    test.Fail("Failed to verify Year End T4A Summary data contains Non Zero values");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                test.Fail("Stored Procedure not found for Year End T4A Summary verification");
                Assert.Fail("Failed to verify Report data");
            }
        }


    }
}
