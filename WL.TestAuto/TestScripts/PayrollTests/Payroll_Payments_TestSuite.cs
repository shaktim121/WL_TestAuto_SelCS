using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WL.TestAuto
{
    [TestClass]
    public class Payroll_Payments_TestSuite : AutomationCore
    {
        private static string xmlDataFile = projectDirectory + "\\TestScripts\\PayrollTests\\ProcessGroupDetails.xml";

        //********************** Payments screen Reports verification ***********************************//

        ///NetPay = Income - Deduction (Benefit not included)
        ///get netpay in [usp_WL_REPORT_PayrollMasterPaycodeSummaryTotals] for orglevel = 1
        ///get total of income from usp_WL_REPORT_PayrollMasterPaycodeTotals (Paycode 1)
        ///get total of deduction from usp_WL_REPORT_PayrollMasterPaycodeTotals (Paycode 3)
        ///PayRegister usp_WL_REPORT_PayrollMasterPaycodeTotals (Find paycode 1 and 3 and deduct to get Netpay in [usp_WL_REPORT_PayrollMasterPaycodeSummaryTotals])
        [TestMethod]
        public void WL_CAN_Payments_003600_Verify_Employee_changes_in_Standard_Reports_Register_Detail_Register_Summary()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string screen = data.GetTestData("Payroll_Screen");

            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroup");
            string runType = GenericMethods.GetXMLNodeValue(xmlDataFile, "RunType");

            string reportType = data.GetTestData("Report_Type");
            string reportName = data.GetTestData("Report_Name");
            string reportFormat = data.GetTestData("Report_Format");

            string reportSPName = data.GetTestData("Report_SP");

            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), false);

            string queryLastPPID = "lastPostedPayProcessIDQuery".AppSettings().Replace("ppcode", GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroupName"));
            string payProcessID = GlobalDB.ExecuteSQLQuery(queryLastPPID, connection).GetTestData("payroll_process_id").ToString();
            string SPParams = data.GetTestData("SP_Params").Replace("ppid", payProcessID).Replace("yyyy", DateTime.Now.Year.ToString()).Replace("reportDB", "dbReportName".AppSettings());

            #endregion


            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Payroll process Screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll(screen);

            //Verify Standard Register Detail report
            if (Pages.Payroll.Fn_Verify_Reports_In_Payroll_PaymentsTable(processGrp, runType, reportType, reportName.Split(';')[0], reportFormat))
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
            if (Pages.Payroll.Fn_Verify_Reports_In_Payroll_PaymentsTable(processGrp, runType, reportType, reportName.Split(';')[1], reportFormat))
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
        public void WL_CAN_Payments_003700_Verify_Standard_Reports_Payroll_Funding()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
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
            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), false);

            string queryLastPPID = "lastPostedPayProcessIDQuery".AppSettings().Replace("ppcode", GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroupName"));
            string payProcessID = GlobalDB.ExecuteSQLQuery(queryLastPPID, connection).GetTestData("payroll_process_id").ToString();
            string SPParams = data.GetTestData("SP_Params").Replace("ppid", payProcessID).Replace("yyyy", DateTime.Now.Year.ToString()).Replace("reportDB", "dbReportName".AppSettings());
            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Payroll process Screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll(screen);

            //Verify Standard Payroll Funding report
            if (Pages.Payroll.Fn_Verify_Reports_In_Payroll_PaymentsTable(processGrp, runType, reportType, reportName, reportFormat))
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

        //FederalRemittanceSummaryReport_select
        [TestMethod]
        public void WL_CAN_Payments_003800_Verify_Standard_Reports_Fed_Prov_Remit_Summary()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string screen = data.GetTestData("Payroll_Screen");

            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroup");
            string runType = GenericMethods.GetXMLNodeValue(xmlDataFile, "RunType");

            string reportType = data.GetTestData("Report_Type");
            string reportName = data.GetTestData("Report_Name");
            string reportFormat = data.GetTestData("Report_Format");
            string excelName = data.GetTestData("ExcelReport_Name");

            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");

            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), false);

            string queryLastPPID = "lastPostedPayProcessIDQuery".AppSettings().Replace("ppcode", GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroupName"));
            string payProcessID = GlobalDB.ExecuteSQLQuery(queryLastPPID, connection).GetTestData("payroll_process_id").ToString();
            string SPParams = data.GetTestData("SP_Params").Replace("ppid", payProcessID).Replace("yyyy", DateTime.Now.Year.ToString()).Replace("reportDB", "dbReportName".AppSettings());
            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Payroll process Screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll(screen);

            //Verify Standard Fed/Prov. Remit Summary report
            if (Pages.Payroll.Fn_Verify_Reports_In_Payroll_PaymentsTable(processGrp, runType, reportType, reportName, reportFormat, excelName))
            {
                test.Pass("Standard Fed/Prov. Remit Summary report opened and verified Successfully");
            }
            else
            {
                GenericMethods.CaptureScreenshot();
                test.Fail("Failed to verify Standard Fed/Prov. Remit Summary report");
                Assert.Fail("Failed to verify Report data");
            }

            test.Info("Verify Standard Fed/Prov. Remit Summary report data exists");
            //Verify Standard Fed/Prov. Remit Summary report from DB
            if (GlobalDB.IsStoredProcedureExists(connection, reportSPName))
            {
                if (Pages.Home.Fn_Verify_ReportsData_In_DataBase(reportSPName, SPParams, dataToVerify))
                {
                    test.Pass("Standard Fed/Prov. Remit Summary report data verified successfully");
                }
                else
                {
                    test.Fail("Failed to verify Standard Fed/Prov. Remit Summary report data");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                test.Fail("Stored Procedure not found for Standard Fed/Prov. Remit Summary report verification");
                Assert.Fail("Failed to verify Report data");
            }
        }

        //ProvincialHealthSummaryReport_select
        [TestMethod]
        public void WL_CAN_Payments_003900_Verify_Standard_Reports_Prov_Health_Summary()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string screen = data.GetTestData("Payroll_Screen");

            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroup");
            string runType = GenericMethods.GetXMLNodeValue(xmlDataFile, "RunType");

            string reportType = data.GetTestData("Report_Type");
            string reportName = data.GetTestData("Report_Name");
            string reportFormat = data.GetTestData("Report_Format");
            string excelName = data.GetTestData("ExcelReport_Name");

            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");

            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), false);

            string queryLastPPID = "lastPostedPayProcessIDQuery".AppSettings().Replace("ppcode", GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroupName"));
            string payProcessID = GlobalDB.ExecuteSQLQuery(queryLastPPID, connection).GetTestData("payroll_process_id").ToString();
            string SPParams = data.GetTestData("SP_Params").Replace("ppid", payProcessID).Replace("yyyy", DateTime.Now.Year.ToString()).Replace("reportDB", "dbReportName".AppSettings());
            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Payroll process Screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll(screen);

            //Verify Standard Prov. Health Summary report
            if (Pages.Payroll.Fn_Verify_Reports_In_Payroll_PaymentsTable(processGrp, runType, reportType, reportName, reportFormat, excelName))
            {
                test.Pass("Standard Prov. Health Summary report opened and verified Successfully");
            }
            else
            {
                GenericMethods.CaptureScreenshot();
                test.Fail("Failed to verify Standard Prov. Health Summary report");
                Assert.Fail("Failed to verify Report data");
            }

            test.Info("Verify Standard Prov. Health Summary report data exists");
            //Verify Standard Prov. Health Summary report from DB
            if (GlobalDB.IsStoredProcedureExists(connection, reportSPName))
            {
                if (Pages.Home.Fn_Verify_ReportsData_In_DataBase(reportSPName, SPParams, dataToVerify))
                {
                    test.Pass("Standard Prov. Health Summary report data verified successfully");
                }
                else
                {
                    test.Fail("Failed to verify Standard Prov. Health Summary report data");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                test.Fail("Stored Procedure not found for Standard Prov. Health Summary report verification");
                Assert.Fail("Failed to verify Report data");
            }
        }

        //PreYearEndT4aDetail_select
        [TestMethod]
        public void WL_CAN_Payments_004000_Verify_Standard_Reports_Pre_YE_T4A_Detail()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string screen = data.GetTestData("Payroll_Screen");

            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroup");
            string runType = GenericMethods.GetXMLNodeValue(xmlDataFile, "RunType");

            string reportType = data.GetTestData("Report_Type");
            string reportName = data.GetTestData("Report_Name");
            string reportFormat = data.GetTestData("Report_Format");
            string excelName = data.GetTestData("ExcelReport_Name");

            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");

            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), false);

            string queryLastPPID = "lastPostedPayProcessIDQuery".AppSettings().Replace("ppcode", GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroupName"));
            string payProcessID = GlobalDB.ExecuteSQLQuery(queryLastPPID, connection).GetTestData("payroll_process_id").ToString();
            string SPParams = data.GetTestData("SP_Params").Replace("ppid", payProcessID).Replace("yyyy", DateTime.Now.Year.ToString()).Replace("reportDB", "dbReportName".AppSettings());
            #endregion

            //Change User language after Log In
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Payroll process Screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll(screen);

            //Verify Standard Pre Y-E T4A Detail report
            if (Pages.Payroll.Fn_Verify_Reports_In_Payroll_PaymentsTable(processGrp, runType, reportType, reportName, reportFormat, excelName))
            {
                test.Pass("Standard Pre Y-E T4A Detail report opened and verified Successfully");
            }
            else
            {
                GenericMethods.CaptureScreenshot();
                test.Fail("Failed to verify Standard Pre Y-E T4A Detail report");
                Assert.Fail("Failed to verify Report data");
            }

            //Verify Standard Pre Y-E T4A Detail report from DB
            test.Info("Verify Standard Pre Y-E T4A Detail report data exists");
            if (GlobalDB.IsStoredProcedureExists(connection, reportSPName))
            {
                if (Pages.Home.Fn_Verify_ReportsData_In_DataBase(reportSPName, SPParams, dataToVerify))
                {
                    test.Pass("Standard Pre Y-E T4A Detail report data verified successfully");
                }
                else
                {
                    test.Fail("Failed to verify Standard Pre Y-E T4A Detail report data");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                test.Fail("Stored Procedure not found for Standard Pre Y-E T4A Detail report verification");
                Assert.Fail("Failed to verify Report data");
            }
        }

        //PreYearEndT4Detail_select
        [TestMethod]
        public void WL_CAN_Payments_004100_Verify_Standard_Reports_Pre_YE_T4_Detail()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string screen = data.GetTestData("Payroll_Screen");

            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroup");
            string runType = GenericMethods.GetXMLNodeValue(xmlDataFile, "RunType");

            string reportType = data.GetTestData("Report_Type");
            string reportName = data.GetTestData("Report_Name");
            string reportFormat = data.GetTestData("Report_Format");
            string excelName = data.GetTestData("ExcelReport_Name");

            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");

            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), false);

            string queryLastPPID = "lastPostedPayProcessIDQuery".AppSettings().Replace("ppcode", GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroupName"));
            string payProcessID = GlobalDB.ExecuteSQLQuery(queryLastPPID, connection).GetTestData("payroll_process_id").ToString();
            string SPParams = data.GetTestData("SP_Params").Replace("ppid", payProcessID).Replace("yyyy", DateTime.Now.Year.ToString()).Replace("reportDB", "dbReportName".AppSettings());
            #endregion

            //Change User language after Log In
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Payroll process Screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll(screen);

            //Verify Standard Pre Y-E T4 Detail report
            if (Pages.Payroll.Fn_Verify_Reports_In_Payroll_PaymentsTable(processGrp, runType, reportType, reportName, reportFormat, excelName))
            {
                test.Pass("Standard Pre Y-E T4 Detail report opened and verified Successfully");
            }
            else
            {
                GenericMethods.CaptureScreenshot();
                test.Fail("Failed to verify Standard Pre Y-E T4 Detail report");
                Assert.Fail("Failed to verify Report data");
            }

            //Verify Standard Pre Y-E T4 Detail report from DB
            test.Info("Verify Standard Pre Y-E T4 Detail report data exists");
            if (GlobalDB.IsStoredProcedureExists(connection, reportSPName))
            {
                if (Pages.Home.Fn_Verify_ReportsData_In_DataBase(reportSPName, SPParams, dataToVerify))
                {
                    test.Pass("Standard Pre Y-E T4 Detail report data verified successfully");
                }
                else
                {
                    test.Fail("Failed to verify Standard Pre Y-E T4 Detail report data");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                test.Fail("Stored Procedure not found for Standard Pre Y-E T4 Detail report verification");
                Assert.Fail("Failed to verify Report data");
            }
        }

        //PreYearEndR1Detail_select
        [TestMethod]
        public void WL_CAN_Payments_004200_Verify_Standard_Reports_Pre_YE_R1_Detail()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string screen = data.GetTestData("Payroll_Screen");

            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroup");
            string runType = GenericMethods.GetXMLNodeValue(xmlDataFile, "RunType");

            string reportType = data.GetTestData("Report_Type");
            string reportName = data.GetTestData("Report_Name");
            string reportFormat = data.GetTestData("Report_Format");
            string excelName = data.GetTestData("ExcelReport_Name");

            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");

            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), false);

            string queryLastPPID = "lastPostedPayProcessIDQuery".AppSettings().Replace("ppcode", GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroupName"));
            string payProcessID = GlobalDB.ExecuteSQLQuery(queryLastPPID, connection).GetTestData("payroll_process_id").ToString();
            string SPParams = data.GetTestData("SP_Params").Replace("ppid", payProcessID).Replace("yyyy", DateTime.Now.Year.ToString()).Replace("reportDB", "dbReportName".AppSettings());
            #endregion

            //Change User language after Log In
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Payroll process Screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll(screen);

            //Verify Standard Pre Y-E R1 Detail report
            if (Pages.Payroll.Fn_Verify_Reports_In_Payroll_PaymentsTable(processGrp, runType, reportType, reportName, reportFormat, excelName))
            {
                test.Pass("Standard Pre Y-E R1 Detail report opened and verified Successfully");
            }
            else
            {
                GenericMethods.CaptureScreenshot();
                test.Fail("Failed to verify Standard Pre Y-E R1 Detail report");
                Assert.Fail("Failed to verify Report data");
            }

            //Verify Standard Pre Y-E R1 Detail report from DB
            test.Info("Verify Standard Pre Y-E R1 Detail report data exists");
            if (GlobalDB.IsStoredProcedureExists(connection, reportSPName))
            {
                if (Pages.Home.Fn_Verify_ReportsData_In_DataBase(reportSPName, SPParams, dataToVerify))
                {
                    test.Pass("Standard Pre Y-E R1 Detail report data verified successfully");
                }
                else
                {
                    test.Fail("Failed to verify Standard Pre Y-E R1 Detail report data");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                test.Fail("Stored Procedure not found for Standard Pre Y-E R1 Detail report verification");
                Assert.Fail("Failed to verify Report data");
            }
        }

        //PIERPreYearEndReport_select
        [TestMethod]
        public void WL_CAN_Payments_004300_Verify_Standard_Reports_PIER_Pre_YE()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string screen = data.GetTestData("Payroll_Screen");

            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroup");
            string runType = GenericMethods.GetXMLNodeValue(xmlDataFile, "RunType");

            string reportType = data.GetTestData("Report_Type");
            string reportName = data.GetTestData("Report_Name");
            string reportFormat = data.GetTestData("Report_Format");
            string excelName = data.GetTestData("ExcelReport_Name");

            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");

            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), false);

            string queryLastPPID = "lastPostedPayProcessIDQuery".AppSettings().Replace("ppcode", GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroupName"));
            string payProcessID = GlobalDB.ExecuteSQLQuery(queryLastPPID, connection).GetTestData("payroll_process_id").ToString();
            string SPParams = data.GetTestData("SP_Params").Replace("ppid", payProcessID).Replace("yyyy", DateTime.Now.Year.ToString()).Replace("reportDB", "dbReportName".AppSettings());
            #endregion

            //Change User language after Log In
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Payroll process Screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll(screen);

            //Verify Standard PIER Pre-YE report
            if (Pages.Payroll.Fn_Verify_Reports_In_Payroll_PaymentsTable(processGrp, runType, reportType, reportName, reportFormat, excelName))
            {
                test.Pass("Standard PIER Pre-YE report opened and verified Successfully");
            }
            else
            {
                GenericMethods.CaptureScreenshot();
                test.Fail("Failed to verify Standard PIER Pre-YE report");
                Assert.Fail("Failed to verify Report data");
            }

            //Verify Standard PIER Pre-YE report from DB
            test.Info("Verify Standard PIER Pre-YE report data exists");
            if (GlobalDB.IsStoredProcedureExists(connection, reportSPName))
            {
                if (Pages.Home.Fn_Verify_ReportsData_In_DataBase(reportSPName, SPParams, dataToVerify))
                {
                    test.Pass("Standard PIER Pre-YE report data verified successfully");
                }
                else
                {
                    test.Fail("Failed to verify Standard PIER Pre-YE report data");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                test.Fail("Stored Procedure not found for Standard PIER Pre-YE report verification");
                Assert.Fail("Failed to verify Report data");
            }
        }

        [TestMethod]
        public void WL_CAN_Payments_005100_Verify_Standard_Reports_Payment_List()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string screen = data.GetTestData("Payroll_Screen");

            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroup");
            string runType = GenericMethods.GetXMLNodeValue(xmlDataFile, "RunType");

            string reportType = data.GetTestData("Report_Type");
            string reportName = data.GetTestData("Report_Name");
            string reportFormat = data.GetTestData("Report_Format");

            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");

            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), false);

            string queryLastPPID = "lastPostedPayProcessIDQuery".AppSettings().Replace("ppcode", GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroupName"));
            string payProcessID = GlobalDB.ExecuteSQLQuery(queryLastPPID, connection).GetTestData("payroll_process_id").ToString();
            string SPParams = data.GetTestData("SP_Params").Replace("ppid", payProcessID).Replace("yyyy", DateTime.Now.Year.ToString()).Replace("reportDB", "dbReportName".AppSettings());
            #endregion

            //Change User language after Log In
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Payroll process Screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll(screen);

            //Verify Standard Payment List report
            if (Pages.Payroll.Fn_Verify_Reports_In_Payroll_PaymentsTable(processGrp, runType, reportType, reportName, reportFormat))
            {
                test.Pass("Standard Payment List report opened and verified Successfully");
            }
            else
            {
                GenericMethods.CaptureScreenshot();
                test.Fail("Failed to verify Standard Payment List report");
                Assert.Fail("Failed to verify Report data");
            }

            //Verify Standard Payment List report from DB
            test.Info("Verify Standard Payment List report data exists");
            if (GlobalDB.IsStoredProcedureExists(connection, reportSPName))
            {
                if (Pages.Home.Fn_Verify_ReportsData_In_DataBase(reportSPName, SPParams, dataToVerify))
                {
                    test.Pass("Standard Payment List report data verified successfully");
                }
                else
                {
                    test.Fail("Failed to verify Standard Payment List report data");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                test.Fail("Stored Procedure not found for Standard Payment List report verification");
                Assert.Fail("Failed to verify Report data");
            }
        }

        [TestMethod]
        public void WL_CAN_Payments_004400_Verify_Standard_Reports_Vacation_Report()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string screen = data.GetTestData("Payroll_Screen");

            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroup");
            string runType = GenericMethods.GetXMLNodeValue(xmlDataFile, "RunType");

            string reportType = data.GetTestData("Report_Type");
            string reportName = data.GetTestData("Report_Name");
            string reportFormat = data.GetTestData("Report_Format");
            string excelName = data.GetTestData("ExcelReport_Name");

            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");

            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), false);

            string queryLastPPID = "lastPostedPayProcessIDQuery".AppSettings().Replace("ppcode", GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroupName"));
            string payProcessID = GlobalDB.ExecuteSQLQuery(queryLastPPID, connection).GetTestData("payroll_process_id").ToString();
            string SPParams = data.GetTestData("SP_Params").Replace("ppid", payProcessID).Replace("yyyy", DateTime.Now.Year.ToString()).Replace("reportDB", "dbReportName".AppSettings());
            #endregion

            //Change User language after Log In
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Payroll process Screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll(screen);

            //Verify Standard Vacation Report
            if (Pages.Payroll.Fn_Verify_Reports_In_Payroll_PaymentsTable(processGrp, runType, reportType, reportName, reportFormat, excelName))
            {
                test.Pass("Standard Vacation Report opened and verified Successfully");
            }
            else
            {
                GenericMethods.CaptureScreenshot();
                test.Fail("Failed to verify Standard Vacation Report");
                Assert.Fail("Failed to verify Report data");
            }

            //Verify Standard Vacation Report from DB
            test.Info("Verify Standard Vacation Report data exists");
            if (GlobalDB.IsStoredProcedureExists(connection, reportSPName))
            {
                if (Pages.Home.Fn_Verify_ReportsData_In_DataBase(reportSPName, SPParams, dataToVerify))
                {
                    test.Pass("Standard Vacation Report data verified successfully");
                }
                else
                {
                    test.Fail("Failed to verify Standard Vacation Report data");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                test.Fail("Stored Procedure not found for Standard Vacation Report verification");
                Assert.Fail("Failed to verify Report data");
            }
        }

        [TestMethod]
        public void WL_CAN_Payments_004500_Verify_Custom_Reports_Pension()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string screen = data.GetTestData("Payroll_Screen");

            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroup");
            string runType = GenericMethods.GetXMLNodeValue(xmlDataFile, "RunType");

            string reportType = data.GetTestData("Report_Type");
            string reportName = data.GetTestData("Report_Name");
            string reportFormat = data.GetTestData("Report_Format");

            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");

            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), false);

            string queryLastPPID = "lastPostedPayProcessIDQuery".AppSettings().Replace("ppcode", GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroupName"));
            string payProcessID = GlobalDB.ExecuteSQLQuery(queryLastPPID, connection).GetTestData("payroll_process_id").ToString();
            string SPParams = data.GetTestData("SP_Params").Replace("ppid", payProcessID).Replace("yyyy", DateTime.Now.Year.ToString()).Replace("reportDB", "dbReportName".AppSettings());
            #endregion

            //Change User language after Log In
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Payroll process Screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll(screen);

            //Verify Custom Pension Report
            if (Pages.Payroll.Fn_Verify_Reports_In_Payroll_PaymentsTable(processGrp, runType, reportType, reportName, reportFormat))
            {
                test.Pass("Custom Pension Report opened and verified Successfully");
            }
            else
            {
                GenericMethods.CaptureScreenshot();
                test.Fail("Failed to verify Custom Pension Report");
                Assert.Fail("Failed to verify Report data");
            }

            //Verify Custom Pension Report from DB
            test.Info("Verify Custom Pension Report data exists");
            if (GlobalDB.IsStoredProcedureExists(connection, reportSPName))
            {
                if (Pages.Home.Fn_Verify_ReportsData_In_DataBase(reportSPName, SPParams, dataToVerify))
                {
                    test.Pass("Custom Pension Report data verified successfully");
                }
                else
                {
                    test.Fail("Failed to verify Custom Pension Report data");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                test.Fail("Stored Procedure not found for Custom Pension Report verification");
                Assert.Fail("Failed to verify Report data");
            }
        }

        [TestMethod]
        public void WL_CAN_Payments_004600_Verify_Custom_Reports_Pension_Demographics()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string screen = data.GetTestData("Payroll_Screen");

            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroup");
            string runType = GenericMethods.GetXMLNodeValue(xmlDataFile, "RunType");

            string reportType = data.GetTestData("Report_Type");
            string reportName = data.GetTestData("Report_Name");
            string reportFormat = data.GetTestData("Report_Format");
            string excelName = data.GetTestData("ExcelReport_Name");

            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");

            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), false);

            string queryLastPPID = "lastPostedPayProcessIDQuery".AppSettings().Replace("ppcode", GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroupName"));
            string payProcessID = GlobalDB.ExecuteSQLQuery(queryLastPPID, connection).GetTestData("payroll_process_id").ToString();
            string SPParams = data.GetTestData("SP_Params").Replace("ppid", payProcessID).Replace("yyyy", DateTime.Now.Year.ToString()).Replace("reportDB", "dbReportName".AppSettings());
            #endregion

            //Change User language after Log In
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Payroll process Screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll(screen);

            //Verify Custom Pension Demographics Report
            if (Pages.Payroll.Fn_Verify_Reports_In_Payroll_PaymentsTable(processGrp, runType, reportType, reportName, reportFormat, excelName))
            {
                test.Pass("Custom Pension Demographics Report opened and verified Successfully");
            }
            else
            {
                GenericMethods.CaptureScreenshot();
                test.Fail("Failed to verify Custom Pension Demographics Report");
                Assert.Fail("Failed to verify Report data");
            }

            //Verify Custom Pension Demographics Report from DB
            test.Info("Verify Custom Pension Demographics Report data exists");
            if (GlobalDB.IsStoredProcedureExists(connection, reportSPName))
            {
                if (Pages.Home.Fn_Verify_ReportsData_In_DataBase(reportSPName, SPParams, dataToVerify))
                {
                    test.Pass("Custom Pension Demographics Report data verified successfully");
                }
                else
                {
                    test.Fail("Failed to verify Custom Pension Demographics Report data");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                test.Fail("Stored Procedure not found for Custom Pension Demographics Report verification");
                Assert.Fail("Failed to verify Report data");
            }
        }

        [TestMethod]
        public void WL_CAN_Payments_004800_Verify_Mass_Payslip_File()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string screen = data.GetTestData("Payroll_Screen");

            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroup");
            string runType = GenericMethods.GetXMLNodeValue(xmlDataFile, "RunType");

            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), false);
            string queryLastPPID = "lastPostedPayProcessIDQuery".AppSettings().Replace("ppcode", GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroupName"));
            string payProcessID = GlobalDB.ExecuteSQLQuery(queryLastPPID, connection).GetTestData("payroll_process_id").ToString();

            string payslipPath = @"C:\Users\" + Environment.UserName + "\\Downloads\\";
            //string fileName = "MassPaySlip_"+ DateTime.Now.ToString() + ".zip";
            string fileName = DateTime.Now.ToString("yyyyMMdd") + "-" + payProcessID + ".zip";

            #endregion

            //Change User language after Log In
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Payroll process Screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll(screen);

            //Select latest payroll process group
            if (!Pages.Payroll.Fn_Search_Select_Latest_ProcessGroup(processGrp, runType))
            {
                GenericMethods.CaptureScreenshot();
                test.Fail("Failed to select payroll process group record");
                Assert.Fail("Failed to select payroll process group record");
            }

            //Save Mass payslip file
            if (!Pages.Payroll.Fn_Save_Mass_Payslip_File_And_Verify(payslipPath, fileName))
            {
                GenericMethods.CaptureScreenshot();
                Assert.Fail("Failed to Save Mass Payslip File");
            }
        }

        [TestMethod]
        public void WL_CAN_Payments_004900_Verify_the_Export_Options()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string screen = data.GetTestData("Payroll_Screen");

            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroup");
            string runType = GenericMethods.GetXMLNodeValue(xmlDataFile, "RunType");

            string exportOptions = data.GetTestData("Export_Options");

            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), false);
            string queryLastPPID = "lastPostedPayProcessIDQuery".AppSettings().Replace("ppcode", GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroupName"));
            string payProcessID = GlobalDB.ExecuteSQLQuery(queryLastPPID, connection).GetTestData("payroll_process_id").ToString();

            string payslipPath = @"C:\Users\" + Environment.UserName + "\\Downloads\\";
            //string fileName = "MassPaySlip_"+ DateTime.Now.ToString() + ".zip";
            string fileName = DateTime.Now.ToString("yyyyMMdd") + "-" + payProcessID + ".zip";
            #endregion

            //Change User language after Log In
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Payroll process Screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll(screen);

            //Select latest payroll process group
            if (!Pages.Payroll.Fn_Search_Select_Latest_ProcessGroup(processGrp, runType))
            {
                GenericMethods.CaptureScreenshot();
                test.Fail("Failed to select payroll process group record");
                Assert.Fail("Failed to select payroll process group record");
            }

            //verify Export Options
            if (!Pages.Payroll.Fn_Verify_Export_Options_In_Payments(exportOptions))
            {
                GenericMethods.CaptureScreenshot();
                Assert.Fail("Failed to Verify Export Options");
            }

        }

        [TestMethod]
        public void WL_CAN_Payments_005000_Verify_Employee_Payslip()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string screen = data.GetTestData("Payroll_Screen");
            string employeeStatus = data.GetTestData("Employee_Status");

            #endregion

            //Change User language after Log In
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Employee Screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll(screen);

            //Search Employees with status checkbox
            Pages.HR.Fn_Search_Employees_With_Status(employeeStatus);

            //Select latest payperiod and verify payslip for the employee
            Pages.Payroll.Fn_Verify_Employee_Payslips_Displayed(1);

            Thread.Sleep(5000);
            //Search Employees with status checkbox
            Pages.HR.Fn_Search_Employees_With_Status(employeeStatus);

            //Select last run payperiod and verify payslip for the employee
            Pages.Payroll.Fn_Verify_Employee_Payslips_Displayed(2);
        }

        //TEST-180
        //[QuebecRemittanceSummaryReport_select]
        [TestMethod]
        public void WL_CAN_Payments_005300_QC_Remittance_Summary()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string screen = data.GetTestData("Payroll_Screen");

            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroup");
            string runType = GenericMethods.GetXMLNodeValue(xmlDataFile, "RunType");

            string reportType = data.GetTestData("Report_Type");
            string reportName = data.GetTestData("Report_Name");
            string reportFormat = data.GetTestData("Report_Format");
            string excelName = data.GetTestData("ExcelReport_Name");

            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");

            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), false);

            string queryLastPPID = "lastPostedPayProcessIDQuery".AppSettings().Replace("ppcode", GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroupName"));
            string payProcessID = GlobalDB.ExecuteSQLQuery(queryLastPPID, connection).GetTestData("payroll_process_id").ToString();
            string SPParams = data.GetTestData("SP_Params").Replace("ppid", payProcessID).Replace("yyyy", DateTime.Now.Year.ToString()).Replace("reportDB", "dbReportName".AppSettings());
            #endregion

            //Change User language after Log In
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Payroll process Screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll(screen);

            //Verify Standard QC Remittance Summary report
            if (Pages.Payroll.Fn_Verify_Reports_In_Payroll_PaymentsTable(processGrp, runType, reportType, reportName, reportFormat, excelName))
            {
                test.Pass("Standard QC Remittance Summary report opened and verified Successfully");
            }
            else
            {
                GenericMethods.CaptureScreenshot();
                test.Fail("Failed to verify Standard QC Remittance Summary report");
                Assert.Fail("Failed to verify Report data");
            }

            //Verify Standard QC Remittance Summary report from DB
            test.Info("Verify Standard QC Remittance Summary report data exists");
            if (GlobalDB.IsStoredProcedureExists(connection, reportSPName))
            {
                if (Pages.Home.Fn_Verify_ReportsData_In_DataBase(reportSPName, SPParams, dataToVerify))
                {
                    test.Pass("Standard QC Remittance Summary report data verified successfully");
                }
                else
                {
                    test.Fail("Failed to verify Standard QC Remittance Summary report data");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                test.Fail("Stored Procedure not found for Standard QC Remittance Summary report verification");
                Assert.Fail("Failed to verify Report data");
            }
        }
    }
}
