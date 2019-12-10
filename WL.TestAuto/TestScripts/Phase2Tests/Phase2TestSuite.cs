using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WL.TestAuto
{
    [TestClass]
    public class Phase2TestSuite : AutomationCore
    {
        [TestMethod]
        public void ZHRX_CAN_Payroll_Reports_001000_Run_payroll_for_available_Pay_Period()
        {
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string screen = data.GetTestData("Payroll_Screen");
            string chequeDate = data.GetTestData("Cheque_Date");
            if (chequeDate == "CURR_DATE")
            {
                chequeDate = DateTime.Now.ToString("MM/dd/yyyy");
            }

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Payroll process Screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll(screen);

            //Calcualte payroll
            Pages.Payroll.Fn_Calculate_Payroll_In_Payroll_Process("", "", chequeDate);
        }

        [TestMethod]
        public void ZHRX_CAN_Payroll_001000_Create_Payroll_Processing_Group()
        {
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string screen = data.GetTestData("Setup_Screen");
            string country = data.GetTestData("Country");
            string payFrequency = data.GetTestData("Pay_Frequency");
            string processGroup = data.GetTestData("Process_Group");
            string engDesc = data.GetTestData("English_Desc");
            string frenchDesc = data.GetTestData("French_Desc");
            string period = data.GetTestData("Period");
            string startDate = data.GetTestData("Start_Date");
            string cutoffDate = data.GetTestData("Cutoff_Date");

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Payroll Processing Group in Setup
            Pages.Setup.Fn_Navigate_Through_Setup(screen);

            //Create Payroll processing group
            Pages.Setup.Fn_Create_Payroll_Processing_Group(country, payFrequency, processGroup, engDesc, frenchDesc, period, startDate, cutoffDate);
        }

        [TestMethod]
        public void ZHRX_CAN_Payroll_001200_Calculate_Payroll()
        {
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string screen = data.GetTestData("Payroll_Screen");
            string processGrp = data.GetTestData("Process_Group");
            string runType = data.GetTestData("Run_Type");
            string chequeDate = data.GetTestData("Cheque_Date");
            if (chequeDate == "CURR_DATE")
            {
                chequeDate = DateTime.Now.ToString("MM/dd/yyyy");
            }

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Payroll process Screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll(screen);

            //Calcualte payroll
            Pages.Payroll.Fn_Calculate_Payroll_In_Payroll_Process(processGrp, runType, chequeDate);
        }

        [TestMethod]
        public void ZHRX_CAN_Payroll_001300_Verify_Standard_Reports_Payroll_Funding()
        {
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string screen = data.GetTestData("Payroll_Screen");
            string reportType = data.GetTestData("Report_Type");
            string reportName = data.GetTestData("Report_Name");
            string reportFormat = data.GetTestData("Report_Format");
            string headerSPName = data.GetTestData("ReportHeader_SP");
            string SPParams = data.GetTestData("SP_Params");
            string headerToVerify = data.GetTestData("ReportHeader_Val");

            string reportSectionSP = data.GetTestData("ReportSection_SP");
            string getColNames = data.GetTestData("Get_ColumnNames");
            string reportGrandTotalSP = data.GetTestData("ReportTotal_SP");

            string processGrp = data.GetTestData("Process_Group");
            string runType = data.GetTestData("Run_Type");
            string chequeDate = data.GetTestData("Cheque_Date");
            if (chequeDate == "CURR_DATE")
            {
                chequeDate = DateTime.Now.ToString("MM/dd/yyyy");
            }
            string connection = @"Data Source=" + "dataSource".AppSettings() + ";Initial Catalog=" + "dbMainName".AppSettings() + ";Integrated Security=SSPI;User ID=" + "dbUser".AppSettings() + ";Password=" + "dbPwd".AppSettings() + ""; ;

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Payroll process Screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll(screen);

            //Calculate Payroll if not available before verifying report
            if(!Pages.Payroll.Fn_GetPayrollRunStatus().Equals("Calculation Processed"))
            {
                //Calcualte payroll
                if(!Pages.Payroll.Fn_Calculate_Payroll_In_Payroll_Process(processGrp, runType, chequeDate))
                {
                    Assert.Fail("Calculate payroll failed");
                }
            }

            //Verify Standard Payroll Funding report
            if(Pages.Payroll.Fn_Verify_Reports_In_Payroll_ProcessTable(reportType, reportName, reportFormat))
            {
                test.Pass("Standard Payroll Funding report opened and verified Successfully");
            }
            else
            {
                test.Fail("Failed to verify Standard Payroll Funding report");
            }

            test.Info("Verify Standard Payroll Funding report Header");
            //Verify Standard Payroll Funding report from DB
            if(Pages.Home.Fn_Verify_ReportsData_In_DataBase(headerSPName, SPParams, headerToVerify))
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
                    test.Pass("Section: "+ section + " found in report Payroll Funding");

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

            if(actTotal > 0.0)
            {
                test.Info("Actual Total of all section amounts : $" + actTotal);
                grandTotal = Convert.ToDouble(Pages.Home.Fn_Get_ReportsData_From_DataBase(reportGrandTotalSP, SPParams, "total")[0]);

                if(actTotal.Equals(grandTotal))
                {
                    test.Pass("Total of all section amounts is matching with the Grand Total : $" + grandTotal);
                }
                else
                {
                    test.Fail("Total of all section amounts is not matching with the Grand Total : $" + grandTotal);
                }
            }
            
        }


    }
}
