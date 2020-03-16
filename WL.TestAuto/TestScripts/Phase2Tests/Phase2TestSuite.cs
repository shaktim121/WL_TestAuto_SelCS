using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.Threading;

namespace WL.TestAuto
{
    [TestClass]
    public class Phase2TestSuite : AutomationCore
    {
        public string xmlDataFile = projectDirectory + "\\TestScripts\\Phase2Tests\\ProcessGroupDetails.xml";

        //********************** Payroll screen Reports verification ***********************************//

        [TestMethod]
        public void ZHRX_CAN_Payroll_001000_ReportServer_Verify_Each_Report_Has_DSNPath()
        {
            string sql = data.GetTestData("StoredProcedure");
            string reportNames = data.GetTestData("ReportNames");
            string criteria = data.GetTestData("SearchCriteria");
            string DBServer = "dataSource".AppSettings();
            string DBName = "ReportServer";

            string connection = GlobalDB.CreateConnectionString(DBServer, DBName, "", "", true);
            bool flag = true;

            foreach (string report in reportNames.Split(';'))
            {
                string sqlNew = sql.Replace(criteria, report);
                DataSet reports = GlobalDB.ExecuteSQLQuery(sqlNew, connection);

                if (reports.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in reports.Tables[0].Rows)
                    {
                        if (row["DSNPath"].ToString() == null)
                        {
                            test.Fail(row["ReportName"].ToString() + " has emptry DSN Path for Search Criteria : " + report);
                            flag = false;
                        }
                    }
                }
                else
                {
                    test.Fail("Query returned no records for Search Criteria : " + report);
                    flag = false;
                }

                if (flag)
                {
                    test.Pass("DSN Path is not Null for Search Criteria : " + report);
                }
                else
                {
                    Assert.Fail("Test Failed");
                }
            }


        }

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
                Assert.Fail("Failed to create Payroll Procesing Group");
            }
        }

        [TestMethod]
        public void WL_CAN_Payroll_001200_Calculate_Payroll()
        {
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string screen = data.GetTestData("Payroll_Screen");

            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroup");
            string runType = GenericMethods.GetXMLNodeValue(xmlDataFile, "RunType");
            string chequeDate = GenericMethods.GetXMLNodeValue(xmlDataFile, "ChequeDate");
            if (chequeDate == "CURR_DATE")
            {
                chequeDate = DateTime.Now.ToString("M/d/yyyy");
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
                Assert.Fail("Calculate payroll failed");
            }

        }

        [TestMethod]
        public void ZHRX_CAN_Payroll_001300_Verify_Standard_Reports_Payroll_Funding()
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
            string chequeDate = GenericMethods.GetXMLNodeValue(xmlDataFile, "ChequeDate");
            if (chequeDate == "CURR_DATE")
            {
                chequeDate = DateTime.Now.ToString("M/d/yyyy");
            }
            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), true);

            string payProcessID = GlobalDB.ExecuteSQLQuery("lastPayProcessIDQuery".AppSettings(), connection).GetTestData("payroll_process_id").ToString();
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

            //Verify Standard Payroll Funding report
            if (Pages.Payroll.Fn_Verify_Reports_In_Payroll_ProcessTable(reportType, reportName, reportFormat))
            {
                test.Pass("Standard Payroll Funding report opened and verified Successfully");
            }
            else
            {
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


        //Incomplete
        //PayRegister usp_WL_REPORT_PayrollMasterPaycodeTotals (Find paycode 1 and 3 and deduct to get Netpay in [usp_WL_REPORT_PayrollMasterPaycodeSummaryTotals])
        public void ZHRX_CAN_Payroll_001400_Verify_Standard_Reports_Register_Detail_Register_Summary()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string screen = data.GetTestData("Payroll_Screen");

            string processGrp = data.GetTestData("Process_Group");
            string runType = data.GetTestData("Run_Type");
            string chequeDate = data.GetTestData("Cheque_Date");
            if (chequeDate == "CURR_DATE")
            {
                chequeDate = DateTime.Now.ToString("M/d/yyyy");
            }

            string regDetailSPName = data.GetTestData("RegDetail_Report_SP");
            string regSummarySPName = data.GetTestData("RegSummary_Report_SP");

            string reportType = data.GetTestData("Report_Type");
            string reportNames = data.GetTestData("Report_Name");
            string reportFormat = data.GetTestData("Report_Format");

            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), true);

            string payProcessID = GlobalDB.ExecuteSQLQuery("lastPayProcessIDQuery".AppSettings(), connection).GetTestData("payroll_process_id").ToString();
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
                test.Fail("Failed to verify Register Detail Funding report");
            }


            string refColNames = data.GetTestData("Ref_ColumnNames");
            string refColVals = data.GetTestData("Ref_ColumnValues");

            string NetPayCurrent = Pages.Home.Fn_Get_ReportsData_From_DataBase(regDetailSPName, SPParams, "current_dollars", refColNames, refColVals);
            if (NetPayCurrent != "")
            {
                test.Pass("Register Details Net Pay Current Amount : " + NetPayCurrent);
            }
            else
            {
                test.Fail("Invalid Net Pay Current amount value");
            }
            string NetPayMTD = Pages.Home.Fn_Get_ReportsData_From_DataBase(regDetailSPName, SPParams, "mtd_dollars", refColNames, refColVals);
            if (NetPayMTD != "")
            {
                test.Pass("Register Details Net Pay MTD Amount : " + NetPayMTD);
            }
            else
            {
                test.Fail("Invalid Net Pay MTD amount value");
            }
            string NetPayYTD = Pages.Home.Fn_Get_ReportsData_From_DataBase(regDetailSPName, SPParams, "ytd_dollars", refColNames, refColVals);
            if (NetPayYTD != "")
            {
                test.Pass("Register Details Net Pay YTD Amount : " + NetPayYTD);
            }
            else
            {
                test.Fail("Invalid Net Pay YTD amount value");
            }

            Thread.Sleep(3000);

            //Verify Standard Register Summary report
            if (Pages.Payroll.Fn_Verify_Reports_In_Payroll_ProcessTable(reportType, reportNames.Split(';')[1], reportFormat))
            {
                test.Pass("Standard Register Summary report opened and verified Successfully");
            }
            else
            {
                test.Fail("Failed to verify Register Summary Funding report");
            }








        }

        [TestMethod]
        public void ZHRX_CAN_Payroll_001600_Verify_Standard_Reports_Changes()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string screen = data.GetTestData("Payroll_Screen");


            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroup");
            string runType = GenericMethods.GetXMLNodeValue(xmlDataFile, "RunType");
            string chequeDate = GenericMethods.GetXMLNodeValue(xmlDataFile, "ChequeDate");
            if (chequeDate == "CURR_DATE")
            {
                chequeDate = DateTime.Now.ToString("M/d/yyyy");
            }

            string reportType = data.GetTestData("Report_Type");
            string reportName = data.GetTestData("Report_Name");
            string reportFormat = data.GetTestData("Report_Format");

            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");

            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), true);

            string payProcessID = GlobalDB.ExecuteSQLQuery("lastPayProcessIDQuery".AppSettings(), connection).GetTestData("payroll_process_id").ToString();
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

            //Verify Standard Changes report
            if (Pages.Payroll.Fn_Verify_Reports_In_Payroll_ProcessTable(reportType, reportName, reportFormat))
            {
                test.Pass("Standard Changes report opened and verified Successfully");
            }
            else
            {
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
        public void ZHRX_CAN_Payroll_001700_Verify_Standard_Reports_Compensation_List()
        {
            #region Data Variable
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string screen = data.GetTestData("Payroll_Screen");


            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroup");
            string runType = GenericMethods.GetXMLNodeValue(xmlDataFile, "RunType");
            string chequeDate = GenericMethods.GetXMLNodeValue(xmlDataFile, "ChequeDate");
            if (chequeDate == "CURR_DATE")
            {
                chequeDate = DateTime.Now.ToString("M/d/yyyy");
            }

            string reportType = data.GetTestData("Report_Type");
            string reportName = data.GetTestData("Report_Name");
            string reportFormat = data.GetTestData("Report_Format");

            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");

            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), true);

            string payProcessID = GlobalDB.ExecuteSQLQuery("lastPayProcessIDQuery".AppSettings(), connection).GetTestData("payroll_process_id").ToString();
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

            //Verify Standard Compensation List report
            if (Pages.Payroll.Fn_Verify_Reports_In_Payroll_ProcessTable(reportType, reportName, reportFormat))
            {
                test.Pass("Standard Compensation List report opened and verified Successfully");
            }
            else
            {
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
        public void ZHRX_CAN_Payroll_001800_Verify_Standard_Reports_Current_Vs_Prior()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string screen = data.GetTestData("Payroll_Screen");


            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroup");
            string runType = GenericMethods.GetXMLNodeValue(xmlDataFile, "RunType");
            string chequeDate = GenericMethods.GetXMLNodeValue(xmlDataFile, "ChequeDate");
            if (chequeDate == "CURR_DATE")
            {
                chequeDate = DateTime.Now.ToString("M/d/yyyy");
            }

            string reportType = data.GetTestData("Report_Type");
            string reportName = data.GetTestData("Report_Name");
            string reportFormat = data.GetTestData("Report_Format");

            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");

            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), true);

            string payProcessID = GlobalDB.ExecuteSQLQuery("lastPayProcessIDQuery".AppSettings(), connection).GetTestData("payroll_process_id").ToString();
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

            //Verify Standard Current Vs Prior report
            if (Pages.Payroll.Fn_Verify_Reports_In_Payroll_ProcessTable(reportType, reportName, reportFormat))
            {
                test.Pass("Standard Current Vs Prior report opened and verified Successfully");
            }
            else
            {
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
        public void ZHRX_CAN_Payroll_001900_Verify_Standard_Reports_Net_Current_Vs_Prior()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string screen = data.GetTestData("Payroll_Screen");


            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroup");
            string runType = GenericMethods.GetXMLNodeValue(xmlDataFile, "RunType");
            string chequeDate = GenericMethods.GetXMLNodeValue(xmlDataFile, "ChequeDate");
            if (chequeDate == "CURR_DATE")
            {
                chequeDate = DateTime.Now.ToString("M/d/yyyy");
            }

            string reportType = data.GetTestData("Report_Type");
            string reportName = data.GetTestData("Report_Name");
            string reportFormat = data.GetTestData("Report_Format");

            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");

            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), true);

            string payProcessID = GlobalDB.ExecuteSQLQuery("lastPayProcessIDQuery".AppSettings(), connection).GetTestData("payroll_process_id").ToString();
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

            //Verify Standard Current Vs Prior Net report
            if (Pages.Payroll.Fn_Verify_Reports_In_Payroll_ProcessTable(reportType, reportName, reportFormat))
            {
                test.Pass("Standard Current Vs Prior Net report opened and verified Successfully");
            }
            else
            {
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
        public void ZHRX_CAN_Payroll_002000_Verify_Standard_Reports_Current_Vs_Prior_Period_Variances()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string screen = data.GetTestData("Payroll_Screen");


            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroup");
            string runType = GenericMethods.GetXMLNodeValue(xmlDataFile, "RunType");
            string chequeDate = GenericMethods.GetXMLNodeValue(xmlDataFile, "ChequeDate");
            if (chequeDate == "CURR_DATE")
            {
                chequeDate = DateTime.Now.ToString("M/d/yyyy");
            }

            string reportType = data.GetTestData("Report_Type");
            string reportName = data.GetTestData("Report_Name");
            string reportFormat = data.GetTestData("Report_Format");

            string reportSPName = data.GetTestData("Report_SP"); //usp_WL_REPORT_CurrentVsPriorPeriodVariancesDetail
            string dataToVerify = data.GetTestData("Report_Val");

            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), true);

            string payProcessID = GlobalDB.ExecuteSQLQuery("lastPayProcessIDQuery".AppSettings(), connection).GetTestData("payroll_process_id").ToString();
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

            //Verify Standard Current Vs Prior Period Variances report
            if (Pages.Payroll.Fn_Verify_Reports_In_Payroll_ProcessTable(reportType, reportName, reportFormat))
            {
                test.Pass("Standard Current Vs Prior Period Variances report opened and verified Successfully");
            }
            else
            {
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
        public void ZHRX_CAN_Payroll_002100_Verify_Standard_Reports_Garnishment()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string screen = data.GetTestData("Payroll_Screen");


            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroup");
            string runType = GenericMethods.GetXMLNodeValue(xmlDataFile, "RunType");
            string chequeDate = GenericMethods.GetXMLNodeValue(xmlDataFile, "ChequeDate");
            if (chequeDate == "CURR_DATE")
            {
                chequeDate = DateTime.Now.ToString("M/d/yyyy");
            }

            string reportType = data.GetTestData("Report_Type");
            string reportName = data.GetTestData("Report_Name");
            string reportFormat = data.GetTestData("Report_Format");

            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");

            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), true);

            string payProcessID = GlobalDB.ExecuteSQLQuery("lastPayProcessIDQuery".AppSettings(), connection).GetTestData("payroll_process_id").ToString();
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

            //Verify Standard Garnishment report
            if (Pages.Payroll.Fn_Verify_Reports_In_Payroll_ProcessTable(reportType, reportName, reportFormat))
            {
                test.Pass("Standard Garnishment report opened and verified Successfully");
            }
            else
            {
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
        public void ZHRX_CAN_Payroll_002200_Verify_Standard_Reports_Pay_Exception()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string screen = data.GetTestData("Payroll_Screen");


            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroup");
            string runType = GenericMethods.GetXMLNodeValue(xmlDataFile, "RunType");
            string chequeDate = GenericMethods.GetXMLNodeValue(xmlDataFile, "ChequeDate");
            if (chequeDate == "CURR_DATE")
            {
                chequeDate = DateTime.Now.ToString("M/d/yyyy");
            }

            string reportType = data.GetTestData("Report_Type");
            string reportName = data.GetTestData("Report_Name");
            string reportFormat = data.GetTestData("Report_Format");

            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");

            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), true);

            string payProcessID = GlobalDB.ExecuteSQLQuery("lastPayProcessIDQuery".AppSettings(), connection).GetTestData("payroll_process_id").ToString();
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

            //Verify Standard Pay Exception report
            if (Pages.Payroll.Fn_Verify_Reports_In_Payroll_ProcessTable(reportType, reportName, reportFormat))
            {
                test.Pass("Standard Pay Exception report opened and verified Successfully");
            }
            else
            {
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
        public void ZHRX_CAN_Payroll_002300_Verify_Standard_Reports_Gross_Vs_Salary()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string screen = data.GetTestData("Payroll_Screen");


            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroup");
            string runType = GenericMethods.GetXMLNodeValue(xmlDataFile, "RunType");
            string chequeDate = GenericMethods.GetXMLNodeValue(xmlDataFile, "ChequeDate");
            if (chequeDate == "CURR_DATE")
            {
                chequeDate = DateTime.Now.ToString("M/d/yyyy");
            }

            string reportType = data.GetTestData("Report_Type");
            string reportName = data.GetTestData("Report_Name");
            string reportFormat = data.GetTestData("Report_Format");

            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");

            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), true);

            string payProcessID = GlobalDB.ExecuteSQLQuery("lastPayProcessIDQuery".AppSettings(), connection).GetTestData("payroll_process_id").ToString();
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

            //Verify Standard Gross Vs Salary report
            if (Pages.Payroll.Fn_Verify_Reports_In_Payroll_ProcessTable(reportType, reportName, reportFormat))
            {
                test.Pass("Standard Gross Vs Salary report opened and verified Successfully");
            }
            else
            {
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
        public void ZHRX_CAN_Payroll_002400_Verify_Standard_Reports_PD7A()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string screen = data.GetTestData("Payroll_Screen");


            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroup");
            string runType = GenericMethods.GetXMLNodeValue(xmlDataFile, "RunType");
            string chequeDate = GenericMethods.GetXMLNodeValue(xmlDataFile, "ChequeDate");
            if (chequeDate == "CURR_DATE")
            {
                chequeDate = DateTime.Now.ToString("M/d/yyyy");
            }

            string reportType = data.GetTestData("Report_Type");
            string reportName = data.GetTestData("Report_Name");
            string reportFormat = data.GetTestData("Report_Format");

            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");

            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), true);

            string payProcessID = GlobalDB.ExecuteSQLQuery("lastPayProcessIDQuery".AppSettings(), connection).GetTestData("payroll_process_id").ToString();
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

            //Verify Standard PD7A report
            if (Pages.Payroll.Fn_Verify_Reports_In_Payroll_ProcessTable(reportType, reportName, reportFormat))
            {
                test.Pass("Standard PD7A report opened and verified Successfully");
            }
            else
            {
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
        public void ZHRX_CAN_Payroll_002500_Verify_Standard_Reports_YTD_Bonus_Commission()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string screen = data.GetTestData("Payroll_Screen");


            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroup");
            string runType = GenericMethods.GetXMLNodeValue(xmlDataFile, "RunType");
            string chequeDate = GenericMethods.GetXMLNodeValue(xmlDataFile, "ChequeDate");
            if (chequeDate == "CURR_DATE")
            {
                chequeDate = DateTime.Now.ToString("M/d/yyyy");
            }

            string reportType = data.GetTestData("Report_Type");
            string reportName = data.GetTestData("Report_Name");
            string reportFormat = data.GetTestData("Report_Format");

            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");

            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), true);

            string payProcessID = GlobalDB.ExecuteSQLQuery("lastPayProcessIDQuery".AppSettings(), connection).GetTestData("payroll_process_id").ToString();
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

            //Verify Standard YTD Bonus Commission report
            if (Pages.Payroll.Fn_Verify_Reports_In_Payroll_ProcessTable(reportType, reportName, reportFormat))
            {
                test.Pass("Standard YTD Bonus Commission report opened and verified Successfully");
            }
            else
            {
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
        public void ZHRX_CAN_Payroll_002600_Verify_Standard_Reports_WCB()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string screen = data.GetTestData("Payroll_Screen");


            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroup");
            string runType = GenericMethods.GetXMLNodeValue(xmlDataFile, "RunType");
            string chequeDate = GenericMethods.GetXMLNodeValue(xmlDataFile, "ChequeDate");
            if (chequeDate == "CURR_DATE")
            {
                chequeDate = DateTime.Now.ToString("M/d/yyyy");
            }

            string reportType = data.GetTestData("Report_Type");
            string reportName = data.GetTestData("Report_Name");
            string reportFormat = data.GetTestData("Report_Format");

            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");

            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), true);

            string payProcessID = GlobalDB.ExecuteSQLQuery("lastPayProcessIDQuery".AppSettings(), connection).GetTestData("payroll_process_id").ToString();
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

            //Verify Standard WCB report
            if (Pages.Payroll.Fn_Verify_Reports_In_Payroll_ProcessTable(reportType, reportName, reportFormat))
            {
                test.Pass("Standard WCB report opened and verified Successfully");
            }
            else
            {
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
        public void ZHRX_CAN_Payroll_002700_Verify_Standard_Reports_Detail_Earn_Ben_Dedn()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string screen = data.GetTestData("Payroll_Screen");


            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroup");
            string runType = GenericMethods.GetXMLNodeValue(xmlDataFile, "RunType");
            string chequeDate = GenericMethods.GetXMLNodeValue(xmlDataFile, "ChequeDate");
            if (chequeDate == "CURR_DATE")
            {
                chequeDate = DateTime.Now.ToString("M/d/yyyy");
            }

            string reportType = data.GetTestData("Report_Type");
            string reportName = data.GetTestData("Report_Name");
            string reportFormat = data.GetTestData("Report_Format");

            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");

            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), true);

            string payProcessID = GlobalDB.ExecuteSQLQuery("lastPayProcessIDQuery".AppSettings(), connection).GetTestData("payroll_process_id").ToString();
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

            //Verify Standard Detail Earn/Ben/Dedn report
            if (Pages.Payroll.Fn_Verify_Reports_In_Payroll_ProcessTable(reportType, reportName, reportFormat))
            {
                test.Pass("Standard Detail Earn/Ben/Dedn report opened and verified Successfully");
            }
            else
            {
                test.Fail("Failed to verify Standard Detail Earn/Ben/Dedn report");
                Assert.Fail("Failed to verify Report data");
            }

            test.Info("Verify Standard Detail Earn/Ben/Dedn report data exists");
            //Verify Standard Detail Earn/Ben/Dedn report from DB
            if (GlobalDB.IsStoredProcedureExists(connection, reportSPName))
            {
                if (Pages.Home.Fn_Verify_ReportsData_In_DataBase(reportSPName, SPParams, dataToVerify))
                {
                    test.Pass("Standard Detail Earn/Ben/Dedn report data verified successfully");
                }
                else
                {
                    test.Fail("Failed to verify Standard Detail Earn/Ben/Dedn report data");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                test.Fail("Stored Procedure not found for Standard Detail Earn/Ben/Dedn report verification");
                Assert.Fail("Failed to verify Report data");
            }
        }

        //GlEmployeeDetails_select
        [TestMethod]
        public void ZHRX_CAN_Payroll_002800_Verify_Standard_Reports_GL_Employee_Details()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string screen = data.GetTestData("Payroll_Screen");


            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroup");
            string runType = GenericMethods.GetXMLNodeValue(xmlDataFile, "RunType");
            string chequeDate = GenericMethods.GetXMLNodeValue(xmlDataFile, "ChequeDate");
            if (chequeDate == "CURR_DATE")
            {
                chequeDate = DateTime.Now.ToString("M/d/yyyy");
            }

            string reportType = data.GetTestData("Report_Type");
            string reportName = data.GetTestData("Report_Name");
            string reportFormat = data.GetTestData("Report_Format");

            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");

            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), true);

            string payProcessID = GlobalDB.ExecuteSQLQuery("lastPayProcessIDQuery".AppSettings(), connection).GetTestData("payroll_process_id").ToString();
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

            //Verify Standard GL Employee Details report
            if (Pages.Payroll.Fn_Verify_Reports_In_Payroll_ProcessTable(reportType, reportName, reportFormat))
            {
                test.Pass("Standard GL Employee Details report opened and verified Successfully");
            }
            else
            {
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
        public void ZHRX_CAN_Payroll_002900_Verify_Standard_Reports_Vacation_Report()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string screen = data.GetTestData("Payroll_Screen");


            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroup");
            string runType = GenericMethods.GetXMLNodeValue(xmlDataFile, "RunType");
            string chequeDate = GenericMethods.GetXMLNodeValue(xmlDataFile, "ChequeDate");
            if (chequeDate == "CURR_DATE")
            {
                chequeDate = DateTime.Now.ToString("M/d/yyyy");
            }

            string reportType = data.GetTestData("Report_Type");
            string reportName = data.GetTestData("Report_Name");
            string reportFormat = data.GetTestData("Report_Format");

            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");

            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), true);

            string payProcessID = GlobalDB.ExecuteSQLQuery("lastPayProcessIDQuery".AppSettings(), connection).GetTestData("payroll_process_id").ToString();
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

            //Verify Standard Vacation report
            if (Pages.Payroll.Fn_Verify_Reports_In_Payroll_ProcessTable(reportType, reportName, reportFormat))
            {
                test.Pass("Standard Vacation report opened and verified Successfully");
            }
            else
            {
                test.Fail("Failed to verify Standard Vacation report");
                Assert.Fail("Failed to verify Report data");
            }

            test.Info("Verify Standard Vacation report data exists");
            //Verify Standard Vacation report from DB
            if (GlobalDB.IsStoredProcedureExists(connection, reportSPName))
            {
                if (Pages.Home.Fn_Verify_ReportsData_In_DataBase(reportSPName, SPParams, dataToVerify))
                {
                    test.Pass("Standard Vacation report data verified successfully");
                }
                else
                {
                    test.Fail("Failed to verify Standard Vacation report data");
                    Assert.Fail("Failed to verify Report data");
                }
            }
            else
            {
                test.Fail("Stored Procedure not found for Standard Vacation report verification");
                Assert.Fail("Failed to verify Report data");
            }
        }

        //usp_WL_REPORT_PensionReportDetail
        [TestMethod]
        public void ZHRX_CAN_Payroll_003000_Verify_Custom_Reports_Pension()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string screen = data.GetTestData("Payroll_Screen");


            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroup");
            string runType = GenericMethods.GetXMLNodeValue(xmlDataFile, "RunType");
            string chequeDate = GenericMethods.GetXMLNodeValue(xmlDataFile, "ChequeDate");
            if (chequeDate == "CURR_DATE")
            {
                chequeDate = DateTime.Now.ToString("M/d/yyyy");
            }

            string reportType = data.GetTestData("Report_Type");
            string reportName = data.GetTestData("Report_Name");
            string reportFormat = data.GetTestData("Report_Format");

            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");

            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), true);

            string payProcessID = GlobalDB.ExecuteSQLQuery("lastPayProcessIDQuery".AppSettings(), connection).GetTestData("payroll_process_id").ToString();
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

            //Verify Custom Pension report
            if (Pages.Payroll.Fn_Verify_Reports_In_Payroll_ProcessTable(reportType, reportName, reportFormat))
            {
                test.Pass("Custom Pension report opened and verified Successfully");
            }
            else
            {
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
        public void ZHRX_CAN_Payroll_003100_Verify_Custom_Reports_Pension_Demographics()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string screen = data.GetTestData("Payroll_Screen");


            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroup");
            string runType = GenericMethods.GetXMLNodeValue(xmlDataFile, "RunType");
            string chequeDate = GenericMethods.GetXMLNodeValue(xmlDataFile, "ChequeDate");
            if (chequeDate == "CURR_DATE")
            {
                chequeDate = DateTime.Now.ToString("M/d/yyyy");
            }

            string reportType = data.GetTestData("Report_Type");
            string reportName = data.GetTestData("Report_Name");
            string reportFormat = data.GetTestData("Report_Format");

            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");

            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), true);

            string payProcessID = GlobalDB.ExecuteSQLQuery("lastPayProcessIDQuery".AppSettings(), connection).GetTestData("payroll_process_id").ToString();
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

            //Verify Custom Pension Demographics report
            if (Pages.Payroll.Fn_Verify_Reports_In_Payroll_ProcessTable(reportType, reportName, reportFormat))
            {
                test.Pass("Custom Pension Demographics report opened and verified Successfully");
            }
            else
            {
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
        public void ZHRX_CAN_Payroll_003200_Verify_Custom_Reports_Current_Vs_Prior_TwoPeriod_Variances()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string screen = data.GetTestData("Payroll_Screen");


            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroup");
            string runType = GenericMethods.GetXMLNodeValue(xmlDataFile, "RunType");
            string chequeDate = GenericMethods.GetXMLNodeValue(xmlDataFile, "ChequeDate");
            if (chequeDate == "CURR_DATE")
            {
                chequeDate = DateTime.Now.ToString("M/d/yyyy");
            }

            string reportType = data.GetTestData("Report_Type");
            string reportName = data.GetTestData("Report_Name");
            string reportFormat = data.GetTestData("Report_Format");

            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");

            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), true);

            string payProcessID = GlobalDB.ExecuteSQLQuery("lastPayProcessIDQuery".AppSettings(), connection).GetTestData("payroll_process_id").ToString();
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

            //Verify Custom Current Vs Prior Two Period Variances report
            if (Pages.Payroll.Fn_Verify_Reports_In_Payroll_ProcessTable(reportType, reportName, reportFormat))
            {
                test.Pass("Custom Current Vs Prior Two Period Variances report opened and verified Successfully");
            }
            else
            {
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
        public void ZHRX_CAN_Payroll_003300_Undo_Calc_Payroll()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
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
                if (!Pages.Payroll.Fn_Undo_Payroll_Calculation())
                {
                    Assert.Fail("Undo Payroll failed");
                }
            }
        }

        [TestMethod]
        public void ZHRX_CAN_Payroll_003500_Rerun_Calculate_Payroll_Post_the_Payroll()
        {
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string screen = data.GetTestData("Payroll_Screen");

            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroup");
            string runType = GenericMethods.GetXMLNodeValue(xmlDataFile, "RunType");
            string chequeDate = GenericMethods.GetXMLNodeValue(xmlDataFile, "ChequeDate");
            if (chequeDate == "CURR_DATE")
            {
                chequeDate = DateTime.Now.ToString("M/d/yyyy");
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
                Assert.Fail("Calculate payroll failed");
            }

            //Post Calculated Payroll
            if (!Pages.Payroll.Fn_Post_Calculated_Payroll_In_Payroll_Process())
            {
                Assert.Fail("Failed to Post payroll");
            }
        }

        //********************** Payments screen Reports verification ***********************************//

        [TestMethod]
        public void ZHRX_CAN_Payments_003700_Verify_Standard_Reports_Payroll_Funding()
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
            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), true);

            string payProcessID = GlobalDB.ExecuteSQLQuery("lastPostedPayProcessIDQuery".AppSettings(), connection).GetTestData("payroll_process_id").ToString();
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
        public void ZHRX_CAN_Payments_003800_Verify_Standard_Reports_Fed_Prov_Remit_Summary()
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

            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), true);

            string payProcessID = GlobalDB.ExecuteSQLQuery("lastPostedPayProcessIDQuery".AppSettings(), connection).GetTestData("payroll_process_id").ToString();
            string SPParams = data.GetTestData("SP_Params").Replace("ppid", payProcessID).Replace("yyyy", DateTime.Now.Year.ToString()).Replace("reportDB", "dbReportName".AppSettings());
            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Payroll process Screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll(screen);

            //Verify Standard Fed/Prov. Remit Summary report
            if (Pages.Payroll.Fn_Verify_Reports_In_Payroll_PaymentsTable(processGrp, runType, reportType, reportName, reportFormat))
            {
                test.Pass("Standard Fed/Prov. Remit Summary report opened and verified Successfully");
            }
            else
            {
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
        public void ZHRX_CAN_Payments_003900_Verify_Standard_Reports_Prov_Health_Summary()
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

            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), true);

            string payProcessID = GlobalDB.ExecuteSQLQuery("lastPostedPayProcessIDQuery".AppSettings(), connection).GetTestData("payroll_process_id").ToString();
            string SPParams = data.GetTestData("SP_Params").Replace("ppid", payProcessID).Replace("yyyy", DateTime.Now.Year.ToString()).Replace("reportDB", "dbReportName".AppSettings());
            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Payroll process Screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll(screen);

            //Verify Standard Prov. Health Summary report
            if (Pages.Payroll.Fn_Verify_Reports_In_Payroll_PaymentsTable(processGrp, runType, reportType, reportName, reportFormat))
            {
                test.Pass("Standard Prov. Health Summary report opened and verified Successfully");
            }
            else
            {
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
        public void ZHRX_CAN_Payments_004000_Verify_Standard_Reports_Pre_YE_T4A_Detail()
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

            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), true);

            string payProcessID = GlobalDB.ExecuteSQLQuery("lastPostedPayProcessIDQuery".AppSettings(), connection).GetTestData("payroll_process_id").ToString();
            string SPParams = data.GetTestData("SP_Params").Replace("ppid", payProcessID).Replace("yyyy", DateTime.Now.Year.ToString()).Replace("reportDB", "dbReportName".AppSettings());
            #endregion

            //Change User language after Log In
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Payroll process Screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll(screen);

            //Verify Standard Pre Y-E T4A Detail report
            if (Pages.Payroll.Fn_Verify_Reports_In_Payroll_PaymentsTable(processGrp, runType, reportType, reportName, reportFormat))
            {
                test.Pass("Standard Pre Y-E T4A Detail report opened and verified Successfully");
            }
            else
            {
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
        public void ZHRX_CAN_Payments_004100_Verify_Standard_Reports_Pre_YE_T4_Detail()
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

            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), true);

            string payProcessID = GlobalDB.ExecuteSQLQuery("lastPostedPayProcessIDQuery".AppSettings(), connection).GetTestData("payroll_process_id").ToString();
            string SPParams = data.GetTestData("SP_Params").Replace("ppid", payProcessID).Replace("yyyy", DateTime.Now.Year.ToString()).Replace("reportDB", "dbReportName".AppSettings());
            #endregion

            //Change User language after Log In
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Payroll process Screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll(screen);

            //Verify Standard Pre Y-E T4 Detail report
            if (Pages.Payroll.Fn_Verify_Reports_In_Payroll_PaymentsTable(processGrp, runType, reportType, reportName, reportFormat))
            {
                test.Pass("Standard Pre Y-E T4 Detail report opened and verified Successfully");
            }
            else
            {
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
        public void ZHRX_CAN_Payments_004200_Verify_Standard_Reports_Pre_YE_R1_Detail()
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

            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), true);

            string payProcessID = GlobalDB.ExecuteSQLQuery("lastPostedPayProcessIDQuery".AppSettings(), connection).GetTestData("payroll_process_id").ToString();
            string SPParams = data.GetTestData("SP_Params").Replace("ppid", payProcessID).Replace("yyyy", DateTime.Now.Year.ToString()).Replace("reportDB", "dbReportName".AppSettings());
            #endregion

            //Change User language after Log In
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Payroll process Screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll(screen);

            //Verify Standard Pre Y-E R1 Detail report
            if (Pages.Payroll.Fn_Verify_Reports_In_Payroll_PaymentsTable(processGrp, runType, reportType, reportName, reportFormat))
            {
                test.Pass("Standard Pre Y-E R1 Detail report opened and verified Successfully");
            }
            else
            {
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
        public void ZHRX_CAN_Payments_004300_Verify_Standard_Reports_PIER_Pre_YE()
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

            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), true);

            string payProcessID = GlobalDB.ExecuteSQLQuery("lastPostedPayProcessIDQuery".AppSettings(), connection).GetTestData("payroll_process_id").ToString();
            string SPParams = data.GetTestData("SP_Params").Replace("ppid", payProcessID).Replace("yyyy", DateTime.Now.Year.ToString()).Replace("reportDB", "dbReportName".AppSettings());
            #endregion

            //Change User language after Log In
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Payroll process Screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll(screen);

            //Verify Standard PIER Pre-YE report
            if (Pages.Payroll.Fn_Verify_Reports_In_Payroll_PaymentsTable(processGrp, runType, reportType, reportName, reportFormat))
            {
                test.Pass("Standard PIER Pre-YE report opened and verified Successfully");
            }
            else
            {
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
        public void WL_CAN_Payments_003700_Verify_Standard_Reports_Payment_List()
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

            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), true);

            string payProcessID = GlobalDB.ExecuteSQLQuery("lastPostedPayProcessIDQuery".AppSettings(), connection).GetTestData("payroll_process_id").ToString();
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

            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");

            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), true);

            string payProcessID = GlobalDB.ExecuteSQLQuery("lastPostedPayProcessIDQuery".AppSettings(), connection).GetTestData("payroll_process_id").ToString();
            string SPParams = data.GetTestData("SP_Params").Replace("ppid", payProcessID).Replace("yyyy", DateTime.Now.Year.ToString()).Replace("reportDB", "dbReportName".AppSettings());
            #endregion

            //Change User language after Log In
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Payroll process Screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll(screen);

            //Verify Standard Vacation Report
            if (Pages.Payroll.Fn_Verify_Reports_In_Payroll_PaymentsTable(processGrp, runType, reportType, reportName, reportFormat))
            {
                test.Pass("Standard Vacation Report opened and verified Successfully");
            }
            else
            {
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

            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), true);

            string payProcessID = GlobalDB.ExecuteSQLQuery("lastPostedPayProcessIDQuery".AppSettings(), connection).GetTestData("payroll_process_id").ToString();
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

            string reportSPName = data.GetTestData("Report_SP");
            string dataToVerify = data.GetTestData("Report_Val");

            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), true);

            string payProcessID = GlobalDB.ExecuteSQLQuery("lastPostedPayProcessIDQuery".AppSettings(), connection).GetTestData("payroll_process_id").ToString();
            string SPParams = data.GetTestData("SP_Params").Replace("ppid", payProcessID).Replace("yyyy", DateTime.Now.Year.ToString()).Replace("reportDB", "dbReportName".AppSettings());
            #endregion

            //Change User language after Log In
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Payroll process Screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll(screen);

            //Verify Custom Pension Demographics Report
            if (Pages.Payroll.Fn_Verify_Reports_In_Payroll_PaymentsTable(processGrp, runType, reportType, reportName, reportFormat))
            {
                test.Pass("Custom Pension Demographics Report opened and verified Successfully");
            }
            else
            {
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

        //****** blocked due to no SP in DB
        public void WL_CAN_Payments_004700_Verify_Custom_Reports_Pre_Year_End_Detail()
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

            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), true);

            string payProcessID = GlobalDB.ExecuteSQLQuery("lastPostedPayProcessIDQuery".AppSettings(), connection).GetTestData("payroll_process_id").ToString();
            string SPParams = data.GetTestData("SP_Params").Replace("ppid", payProcessID).Replace("yyyy", DateTime.Now.Year.ToString()).Replace("reportDB", "dbReportName".AppSettings());
            #endregion

            //Change User language after Log In
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Payroll process Screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll(screen);

            //Verify Custom Pension Demographics Report
            if (Pages.Payroll.Fn_Verify_Reports_In_Payroll_PaymentsTable(processGrp, runType, reportType, reportName, reportFormat))
            {
                test.Pass("Custom Pension Demographics Report opened and verified Successfully");
            }
            else
            {
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

            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbReportName".AppSettings(), "dbUser".AppSettings(), "dbPwd".AppSettings(), true);
            string payProcessID = GlobalDB.ExecuteSQLQuery("lastPostedPayProcessIDQuery".AppSettings(), connection).GetTestData("payroll_process_id").ToString();

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
            if (!Pages.Payroll.Fn_Save_Mass_Payslip_File_And_Verify(processGrp, runType, payslipPath, fileName))
            {
                Assert.Fail("Failed to Save Mass Payslip File");
            }
        }

    }
}
