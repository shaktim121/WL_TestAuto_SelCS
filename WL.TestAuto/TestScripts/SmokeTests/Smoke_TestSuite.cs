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

namespace WL.TestAuto
{
    [TestClass]
    public class Smoke_TestSuite : AutomationCore
    {
        public string xmlDataFile = projectDirectory + "\\TestScripts\\PayrollTests\\ProcessGroupDetails.xml";

        [TestMethod]
        //TEST-172
        public void WL_CAN_Reports_001000_Verify_Each_Report_Has_DSNPath()
        {
            string sql = data.GetTestData("StoredProcedure");
            //string reportNames = data.GetTestData("ReportNames");
            //string criteria = data.GetTestData("SearchCriteria");
            string DBServer = "dataSource".AppSettings();
            string DBName = "ReportServer";
            string connection = GlobalDB.CreateConnectionString(DBServer, DBName, "", "", false);

            int failCount = 0;

            //Fetch all report details through SQL query
            DataSet reports = GlobalDB.ExecuteSQLQuery(sql, connection);
            if (reports != null && reports.Tables[0].Rows.Count > 0)
            {
                test.Info("Total reports found : " + reports.Tables[0].Rows.Count);
                foreach (DataRow row in reports.Tables[0].Rows)
                {
                    if (row["DSNPath"].ToString() == "" || row["DSNName"].ToString() == "")
                    {
                        test.Fail("DSN Path or DSN Name is empty for Report : " + row["ReportName"].ToString() + " in Directory : " + row["directory"].ToString());
                        failCount++;
                    }
                    else
                    {
                        test.Pass("DSN Path exist for Report : " + row["ReportName"].ToString() + " in Directory : " + row["directory"].ToString());
                    }
                }

            }
            else
            {
                test.Fail("Query returned no records");
                Assert.Fail("Test Failed");
            }

            if (failCount == 0)
            {
                test.Pass("No records found with Null DSN Path");
            }
            else
            {
                test.Fail(failCount + " : records found with Null DSN Path");
                Assert.Fail("Test Failed");
            }

        }

        [TestMethod]
        //1.	Human Resources - Employee Screen - TEST-2
        public void WL_CAN_HR_UI_000100_Verify_Human_Resources_Employee_Screen()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string screen = data.GetTestData("HR_Screen");

            string textFields = data.GetTestData("Verify_TextFields");
            string buttons = data.GetTestData("Verify_Buttons");
            string drpDowns = data.GetTestData("Verify_DropDowns");
            string checkBoxes = data.GetTestData("Verify_CheckBoxes");
            string toolBars = data.GetTestData("Verify_ToolBars");
            string tableColumns = data.GetTestData("Verify_TableColumns");
            string labelFields = data.GetTestData("Verify_LabelFields");
            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Employee Screen in Human Resources
            Pages.HR.Fn_NavigateThroughHumanResources(screen);

            //Verify all fields in Employee Screen
            if (Pages.HR.Fn_Verify_Fields_In_HR_Screens(textFields, buttons, drpDowns, checkBoxes, toolBars, tableColumns, labelFields))
            {
                test.Log(Status.Pass, "All fields in Employee screen verified successfully");
            }
            else
            {
                test.Fail("Failed to verify all fields in Employee screen");
                Assert.Fail();
            }

            //Search Employee records present
            if (Pages.HR.Fn_Verify_Record_Displayed_In_EmployeeTable())
            {
                test.Log(Status.Pass, "Verified Employee records displayed");
            }
            else
            {
                test.Fail("Failed to verify Employee record displayed");
                Assert.Fail();
            }
        }

        [TestMethod]
        //2.	HR Screen - Open Screens in English - TEST-3
        public void WL_CAN_HR_UI_000200_Verify_Human_Resources_HR_Screen_Open_Screens_In_English()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string screen1 = data.GetTestData("HR_Screen1");
            string screen2 = data.GetTestData("HR_Screen2");
            string reportNames = data.GetTestData("Report_List");

            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Employee Screen in Human Resources
            Pages.HR.Fn_NavigateThroughHumanResources(screen1);

            //Navigate to Add Wizard Screen in Human Resources
            Pages.HR.Fn_NavigateThroughHumanResources(screen2);

            //string reportNames = "Phone List;Birthday List;Name and Address Listing;Anniversary Listing;Emergency Contact List;Employee Listing;Licenses/Certificates;Terminated Employees;Pay Rate Listing;Status Code Report;Year End File and Print Exclusions;Probation Report;Diversity Report;Paycode Report;Employee Detail Audit Report;HR Changes Audit Report;Skill Report;Stat Holiday;Attrition Report";

            int failCnt = 0;
            foreach (string report in reportNames.Split(';'))
            {
                //Navigate to Reports Screen in Human Resources
                if (Pages.HR.Fn_NavigateThroughHumanResources("Reports-" + report))
                {
                    test.Log(Status.Pass, "Navigated to Reports-" + report + " Screen");
                }
                else
                {
                    test.Fail("Failed to navigate to Reports-" + report + " Screen");
                }

                //Verify report
                if (Pages.HR.Fn_View_Verify_HR_ReportDisplayedOnScreen(report))
                {
                    test.Log(Status.Pass, "View and verify PDF report " + report + " successful");
                }
                else
                {
                    test.Fail("Failed to verify PDF report displayed : " + report);
                    failCnt++;
                }

                //Navigate to Home
                if (Pages.Home.Fn_NavigateToHomeScreen())
                {
                    test.Log(Status.Pass, "Navigated to Home Screen");
                }
                else
                {
                    test.Fail("Failed to navigate to Home screen");
                }
            }
            if (failCnt != 0)
            {
                Assert.Fail();
            }

        }

        [TestMethod]
        //3.	HR Screen - Open Screens in French **reports TEST-4
        public void WL_CAN_HR_UI_000300_HR_Screen_Open_Screens_In_French()
        {
            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage("Français");

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile("Administrateur");

            //Navigate to Employee Scren in Human Resources
            if (Pages.HR.Fn_NavigateThroughRessourcesHumaines("Employé"))
            {
                test.Log(Status.Pass, "Navigated to Employé Screen");
            }
            else
            {
                test.Fail("Failed to navigate to Employé Screen");
            }

            //Navigate to Add Wizard Scren in Human Resources
            if (Pages.HR.Fn_NavigateThroughRessourcesHumaines("Embauche"))
            {
                test.Log(Status.Pass, "Navigated to Embauche Screen");
            }
            else
            {
                test.Fail("Failed to navigate to Embauche Screen");
            }

            //Navigate to Reports Screen in Human Resources
            if (Pages.HR.Fn_NavigateThroughRessourcesHumaines("Reports-Anniversary Listing"))
            {
                test.Log(Status.Pass, "Navigated to Reports-Anniversary Listing Screen");
            }
            else
            {
                test.Fail("Failed to navigate to Reports-Anniversary Listing Screen");
            }

            //Verify Anniversary Listing report
            if (Pages.HR.Fn_View_Verify_HR_ReportDisplayedOnScreen("Anniversary Listing"))
            {
                test.Log(Status.Pass, "View and verify PDF report Aniversary Listing successful");
            }
            else
            {
                test.Fail("Failed to verify PDF report displayed : Anniversary Listing");
            }

            Thread.Sleep(2000);

            //Navigate to Home/Maison
            if (Pages.Home.Fn_NavigateToHomeScreen())
            {
                test.Log(Status.Pass, "Navigated to Home Screen");
            }
            else
            {
                test.Fail("Failed to navigate to Home screen");
            }

            Thread.Sleep(5000);

            //Navigate to Reports Screen in Human Resources
            if (Pages.HR.Fn_NavigateThroughRessourcesHumaines("Reports-Employee Listing"))
            {
                test.Log(Status.Pass, "Navigated to Reports-Employee Listing Screen");
            }
            else
            {
                test.Fail("Failed to navigate to Reports-Employee Listing Screen");
            }

            //Verify Employee Listing report
            if (Pages.HR.Fn_View_Verify_HR_ReportDisplayedOnScreen("Employee Listing"))
            {
                test.Log(Status.Pass, "View and verify PDF report Employee Listing successful");
            }
            else
            {
                test.Fail("Failed to verify PDF report displayed : Employee Listing");
            }

            Thread.Sleep(2000);

            //Navigate to Home/Maison
            if (Pages.Home.Fn_NavigateToHomeScreen())
            {
                test.Log(Status.Pass, "Navigated to Home Screen");
            }
            else
            {
                test.Fail("Failed to navigate to Home screen");
            }

            Thread.Sleep(5000);

            //Navigate to Reports Screen in Human Resources
            if (Pages.HR.Fn_NavigateThroughRessourcesHumaines("Reports-Paycode Report"))
            {
                test.Log(Status.Pass, "Navigated to Reports-Paycode Report Screen");
            }
            else
            {
                test.Fail("Failed to navigate to Reports-Paycode Report Screen");
            }
        }

        [TestMethod]
        //4.	Payroll Screen - Employee Screen TEST-5
        public void WL_CAN_HR_UI_000400_Payroll_Screen_Employee_Screen()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string screen = data.GetTestData("Payroll_Screen");

            string textFields = data.GetTestData("Verify_TextFields");
            string buttons = data.GetTestData("Verify_Buttons");
            string drpDowns = data.GetTestData("Verify_DropDowns");
            string checkBoxes = data.GetTestData("Verify_CheckBoxes");
            string toolBars = data.GetTestData("Verify_ToolBars");
            string tableColumns = data.GetTestData("Verify_TableColumns");
            string labelFields = data.GetTestData("Verify_LabelFields");
            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Employee Screen in Human Resources
            if (Pages.Payroll.Fn_NavigateThroughPayroll(screen))
            {
                test.Log(Status.Pass, "Navigated to Employee Screen under Payroll");
            }
            else
            {
                test.Fail("Failed to navigate to Employee Screen under Payroll");
                Assert.Fail();
            }

            //Verify all fields in Employee Screen
            Pages.Payroll.Fn_Verify_Fields_In_Payroll_Screens(textFields, buttons, drpDowns, checkBoxes, toolBars, tableColumns, labelFields);

            //Search Employee records present
            if (Pages.HR.Fn_Verify_Record_Displayed_In_EmployeeTable())
            {
                test.Log(Status.Pass, "Verified Employee records displayed");
            }
            else
            {
                test.Fail("Failed to verify Employee record displayed");
                Assert.Fail();
            }
        }

        [TestMethod]
        //5.	Payroll Screen - Batch Screen TEST-6
        public void WL_CAN_HR_UI_000500_Payroll_Screen_Batch_Screen()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string screen = data.GetTestData("Payroll_Screen");

            string textFields = data.GetTestData("Verify_TextFields");
            string buttons = data.GetTestData("Verify_Buttons");
            string drpDowns = data.GetTestData("Verify_DropDowns");
            string checkBoxes = data.GetTestData("Verify_CheckBoxes");
            string toolBars = data.GetTestData("Verify_ToolBars");
            string tableColumns = data.GetTestData("Verify_TableColumns");
            string labelFields = data.GetTestData("Verify_LabelFields");

            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Batch Screen in Payroll
            if (Pages.Payroll.Fn_NavigateThroughPayroll(screen))
            {
                test.Log(Status.Pass, "Navigated to Batch Screen under Payroll");
            }
            else
            {
                test.Fail("Failed to navigate to Batch Screen under Payroll");
                Assert.Fail();
            }

            //Verify all fields in Batch Screen
            if (!Pages.Payroll.Fn_Verify_Fields_In_Payroll_Screens(textFields, buttons, drpDowns, checkBoxes, toolBars, tableColumns, labelFields))
            {
                Assert.Fail();
            }
        }


        //6.	Payroll Screen - Payroll Transaction TEST-7
        public void WL_CAN_HR_UI_000600_Payroll_Screen_Payroll_Transaction()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string screen = data.GetTestData("Payroll_Screen");

            string textFields = data.GetTestData("Verify_TextFields");
            string buttons = data.GetTestData("Verify_Buttons");
            string drpDowns = data.GetTestData("Verify_DropDowns");
            string checkBoxes = data.GetTestData("Verify_CheckBoxes");
            string toolBars = data.GetTestData("Verify_ToolBars");
            string tableColumns = data.GetTestData("Verify_TableColumns");
            string labelFields = data.GetTestData("Verify_LabelFields");
            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Payroll Transaction under Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll(screen);

            //To get control on the Iframe window of the page
            Browsers.GetDriver.SwitchTo().Frame("payrollException");

            if (Browsers.GetDriver.FindElements(By.XPath(".//p[contains(text(),'HTTP Error 404')]")).Count > 0)
            {
                test.Fail("Failed to load Payroll Transaction page");
                Browsers.GetDriver.SwitchTo().DefaultContent();
                Assert.Fail();
            }

            //Verify all fields in Payroll Screen
            if (!Pages.Payroll.Fn_Verify_Fields_In_Payroll_Screens(textFields, buttons, drpDowns, checkBoxes, toolBars, tableColumns, labelFields))
            {
                Assert.Fail();
            }

            //Verify records displayed in Payroll Transaction
            if (!Pages.Payroll.Fn_Verify_Record_Displayed_In_PayrollTransactionTable())
            {
                Assert.Fail();
            }

            //To get control on the Iframe window of the page
            Browsers.GetDriver.SwitchTo().DefaultContent();

        }

        [TestMethod]
        //7.	Payroll Screen - Payroll Process TEST-8
        public void WL_CAN_HR_UI_000700_Payroll_Screen_PayrollProcess()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string screen = data.GetTestData("Payroll_Screen");
            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroup");

            string textFields = data.GetTestData("Verify_TextFields");
            string buttons = data.GetTestData("Verify_Buttons");
            string drpDowns = data.GetTestData("Verify_DropDowns");
            string checkBoxes = data.GetTestData("Verify_CheckBoxes");
            string toolBars = data.GetTestData("Verify_ToolBars");
            string tableColumns = data.GetTestData("Verify_TableColumns");
            string labelFields = data.GetTestData("Verify_LabelFields");

            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Payroll process Screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll(screen);

            if (!Pages.Payroll.Fn_Undo_Payroll_Calculation(processGrp))
            {
                Assert.Fail("Undo Payroll failed");
            }

            if (Pages.Payroll.DrpDwn_ProcessGrp.Exists())
            {
                Pages.Payroll.DrpDwn_ProcessGrp.SelectValueFromDropDown(processGrp);
            }

            //Verify all fields in Payroll Screen
            if (!Pages.Payroll.Fn_Verify_Fields_In_Payroll_Screens(textFields, buttons, drpDowns, checkBoxes, toolBars, tableColumns, labelFields))
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        //8.	Payroll Screen - Payments TEST-9
        //Fn_Verify_ReportsDisplayed_In_Payroll_PaymentsTable - need to be fixed
        public void WL_CAN_HR_UI_000800_Payroll_Screen_Payments()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string screen = data.GetTestData("Payroll_Screen");

            string textFields = data.GetTestData("Verify_TextFields");
            string buttons = data.GetTestData("Verify_Buttons");
            string drpDowns = data.GetTestData("Verify_DropDowns");
            string checkBoxes = data.GetTestData("Verify_CheckBoxes");
            string toolBars = data.GetTestData("Verify_ToolBars");
            string tableColumns = data.GetTestData("Verify_TableColumns");
            string labelFields = data.GetTestData("Verify_LabelFields");
            string reportList = data.GetTestData("Reports_List");

            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Payments screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll(screen);

            //Verify all fields in Payroll Screen
            if (!Pages.Payroll.Fn_Verify_Fields_In_Payroll_Screens(textFields, buttons, drpDowns, checkBoxes, toolBars, tableColumns, labelFields))
            {
                Assert.Fail();
            }

            //Verify reports present in Payments UI
            if (!Pages.Payroll.Fn_Verify_ReportsDisplayed_In_Payroll_PaymentsTable(reportList))
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        //9.	Payroll Screen - Import TEST-10
        public void WL_CAN_HR_UI_000900_Payroll_Screen_Import()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string screen = data.GetTestData("Payroll_Screen");
            string imports = data.GetTestData("ImportType_List");

            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Import screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll(screen);

            //verify Import Types in dropdown
            if (!Pages.Payroll.Fn_Verify_ImportTypesDisplayed_In_Payroll_Import(imports))
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        //10.	Payroll Screen - Reports TEST-11
        public void WL_CAN_HR_UI_001000_Payroll_Screen_Reports()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProfile = data.GetTestData("User_Profile");
            string reports = data.GetTestData("Report_List");
            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProfile);

            //Export Report
            if (!Pages.Payroll.Fn_Navigate_To_Reports_In_Payroll(reports))
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        //11.	Setup Screen - Policy
        public void WL_CAN_HR_UI_001100_Setup_Screen_Policy()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProfile = data.GetTestData("User_Profile");
            string screen = data.GetTestData("Setup_Screen");

            string textFields = data.GetTestData("Verify_TextFields");
            string buttons = data.GetTestData("Verify_Buttons");
            string drpDowns = data.GetTestData("Verify_DropDowns");
            string checkBoxes = data.GetTestData("Verify_CheckBoxes");
            string toolBars = data.GetTestData("Verify_ToolBars");
            string tableColumns = data.GetTestData("Verify_TableColumns");
            string labelFields = data.GetTestData("Verify_LabelFields");

            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProfile);

            //Navigate to Policy screen in Setup
            Pages.Setup.Fn_Navigate_Through_Setup(screen);

            //Verify all fields in Setup->Policy
            if (Pages.Setup.Fn_Verify_Fields_In_Setup_Screens(textFields, buttons, drpDowns, checkBoxes, toolBars, tableColumns, labelFields))
            {
                test.Pass("All fields in Policy screen under Setup are verified successfully");
            }
            else
            {
                test.Fail("Failed to verify all fields in Policy screen under Setup");
            }

            Pages.Setup.Fn_Verify_Record_Displayed_In_PolicyTable();
        }

        [TestMethod]
        //12.	Setup Screen - Entitlement
        public void WL_CAN_HR_UI_001200_Setup_Screen_Entitlement()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProfile = data.GetTestData("User_Profile");
            string screen = data.GetTestData("Setup_Screen");

            string textFields = data.GetTestData("Verify_TextFields");
            string buttons = data.GetTestData("Verify_Buttons");
            string drpDowns = data.GetTestData("Verify_DropDowns");
            string checkBoxes = data.GetTestData("Verify_CheckBoxes");
            string toolBars = data.GetTestData("Verify_ToolBars");
            string tableColumns = data.GetTestData("Verify_TableColumns");
            string labelFields = data.GetTestData("Verify_LabelFields");

            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProfile);

            //Navigate to Entitlement screen in Setup
            Pages.Setup.Fn_Navigate_Through_Setup(screen);

            //Verify all fields in Setup->Entitlement
            if (Pages.Setup.Fn_Verify_Fields_In_Setup_Screens(textFields, buttons, drpDowns, checkBoxes, toolBars, tableColumns, labelFields))
            {
                test.Pass("All fields in Entitlement screen under Setup are verified successfully");
            }
            else
            {
                test.Fail("Failed to verify all fields in Entitlement screen under Setup");
            }

            //Verify records exist in Entitlement Table
            Pages.Setup.Fn_Verify_Record_Displayed_In_EntitlementTable();
        }

        [TestMethod]
        //13.	Setup Screen - Vendor
        public void WL_CAN_HR_UI_001300_Setup_Screen_Vendor()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProfile = data.GetTestData("User_Profile");
            string screen = data.GetTestData("Setup_Screen");

            string textFields = data.GetTestData("Verify_TextFields");
            string buttons = data.GetTestData("Verify_Buttons");
            string drpDowns = data.GetTestData("Verify_DropDowns");
            string checkBoxes = data.GetTestData("Verify_CheckBoxes");
            string toolBars = data.GetTestData("Verify_ToolBars");
            string tableColumns = data.GetTestData("Verify_TableColumns");
            string labelFields = data.GetTestData("Verify_LabelFields");

            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProfile);

            //Navigate to Vendor screen in Setup
            Pages.Setup.Fn_Navigate_Through_Setup(screen);

            //Verify all fields in Setup->Vendor
            if (Pages.Setup.Fn_Verify_Fields_In_Setup_Screens(textFields, buttons, drpDowns, checkBoxes, toolBars, tableColumns, labelFields))
            {
                test.Pass("All fields in Vendor screen under Setup are verified successfully");
            }
            else
            {
                test.Fail("Failed to verify all fields in Vendor screen under Setup");
                Assert.Fail();
            }

            //Verify records exist in Vendor Table
            if (!Pages.Setup.Fn_Verify_Record_Displayed_In_VendorTable())
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        //14.	Setup Screen - Payroll Processing Group
        public void WL_CAN_HR_UI_001400_Setup_Screen_Payroll_Processing_Group()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProfile = data.GetTestData("User_Profile");
            string screen = data.GetTestData("Setup_Screen");

            string textFields = data.GetTestData("Verify_TextFields");
            string buttons = data.GetTestData("Verify_Buttons");
            string drpDowns = data.GetTestData("Verify_DropDowns");
            string checkBoxes = data.GetTestData("Verify_CheckBoxes");
            string toolBars = data.GetTestData("Verify_ToolBars");
            string tableColumns = data.GetTestData("Verify_TableColumns");
            string labelFields = data.GetTestData("Verify_LabelFields");

            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProfile);

            //Navigate to Payroll Processing Group screen in Setup
            Pages.Setup.Fn_Navigate_Through_Setup(screen);

            //Verify all fields in Setup->Payroll Processing Group
            if (Pages.Setup.Fn_Verify_Fields_In_Setup_Screens(textFields, buttons, drpDowns, checkBoxes, toolBars, tableColumns, labelFields))
            {
                test.Pass("All fields in Payroll Processing Group screen under Setup are verified successfully");
            }
            else
            {
                test.Fail("Failed to verify all fields in Payroll Processing Group screen under Setup");
                Assert.Fail();
            }

            //Verify records exist in Payroll Processing Group Table
            if (!Pages.Setup.Fn_Verify_Record_Displayed_In_PayProcessGroupTable())
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        //15.	Setup Screen - Organizational Unit Level
        public void WL_CAN_HR_UI_001500_Setup_Screen_Organizational_Unit_Level()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProfile = data.GetTestData("User_Profile");
            string screen = data.GetTestData("Setup_Screen");
            string colName = data.GetTestData("Column_Name");
            string valueList = data.GetTestData("Column_Values");
            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProfile);

            //Navigate to Organizational Unit Level screen in Setup
            if (!Pages.Setup.Fn_Navigate_Through_Setup(screen))
            {
                Assert.Fail("Failed to navigate to Setup Screen");
            }

            //Verify records in table column
            if (!Pages.Setup.Fn_Verify_Records_Of_Single_Column_In_OUL(colName, valueList))
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        //16.	Setup Screen - Wizard Tempate
        public void WL_CAN_HR_UI_001600_Setup_Screen_Wizard_Template()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProfile = data.GetTestData("User_Profile");
            string screen = data.GetTestData("Setup_Screen");
            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProfile);

            //Navigate to Wizard Tempate screen in Setup
            Pages.Setup.Fn_Navigate_Through_Setup(screen);

            //Verify records exist in Wizard Template Table
            if (!Pages.Setup.Fn_Verify_Record_Displayed_In_WizardTable())
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        //17.	Setup Screen - Paycode
        public void WL_CAN_HR_UI_001700_Setup_Screen_Paycode()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProfile = data.GetTestData("User_Profile");
            string screen1 = data.GetTestData("Setup_Screen1");
            string screen2 = data.GetTestData("Setup_Screen2");
            string screen3 = data.GetTestData("Setup_Screen3");

            string textFields = data.GetTestData("Verify_TextFields");
            string buttons = data.GetTestData("Verify_Buttons");
            string drpDowns = data.GetTestData("Verify_DropDowns");
            string checkBoxes = data.GetTestData("Verify_CheckBoxes");
            string toolBars = data.GetTestData("Verify_ToolBars");
            string tableColumns = data.GetTestData("Verify_TableColumns");
            string labelFields = data.GetTestData("Verify_LabelFields");

            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProfile);

            //Navigate to Paycode screen in Setup
            Pages.Setup.Fn_Navigate_Through_Setup(screen1);

            Thread.Sleep(3000);

            //Navigate to Paycode screen in Setup
            Pages.Setup.Fn_Navigate_Through_Setup(screen2);

            //Verify records exist in Paycode Association Tables
            Pages.Setup.Fn_Verify_Record_Displayed_In_PaycodeAssociationTable();

            Thread.Sleep(3000);

            //Navigate to Paycode screen in Setup
            Pages.Setup.Fn_Navigate_Through_Setup(screen3);

            //Verify all fields in Setup->Paycode>Paycode Maintenance
            if (Pages.Setup.Fn_Verify_Fields_In_Setup_Screens(textFields, buttons, drpDowns, checkBoxes, toolBars, tableColumns, labelFields))
            {
                test.Pass("All fields in " + screen3 + " screen under Setup are verified successfully");
            }
            else
            {
                test.Fail("Failed to verify all fields in " + screen3 + " screen under Setup");
                Assert.Fail();
            }

            //Verify records exist in Paycode Maintenance Table
            if (!Pages.Setup.Fn_Verify_Record_Displayed_In_PaycodeMaintTable())
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        //18.	Admin Screen - Code Maintenance
        public void WL_CAN_HR_UI_001800_Admin_Code_Maintenance()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProfile = data.GetTestData("User_Profile");
            string options = data.GetTestData("Admin_Options");
            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProfile);

            //Navigate to Code Maintenance screen in Admin
            Pages.Admin.Fn_Navigate_Through_Admin(options);

            //Verify code types listed in drop down Code Table
            if (!Pages.Admin.Fn_Verify_CodeTypes_Dropdown_In_CodeMaint(2))
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        //19.	Admin Screen - Code System
        public void WL_CAN_HR_UI_001900_Admin_Code_System()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProfile = data.GetTestData("User_Profile");
            string screen = data.GetTestData("Admin_Screen");

            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProfile);

            //Navigate to Code System screen in Admin
            Pages.Admin.Fn_Navigate_Through_Admin(screen);

            //Verify records in Code System table
            if (!Pages.Admin.Fn_Verify_Record_Displayed_In_CodeSystemTable())
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        //20.	Admin Screen - Mandatory Field Editor
        public void WL_CAN_HR_UI_002000_Admin_Mandatory_Field_Editor()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProfile = data.GetTestData("User_Profile");
            string screen = data.GetTestData("Admin_Screen");

            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProfile);

            //Navigate to Mandatory Field Editor screen in Admin
            Pages.Admin.Fn_Navigate_Through_Admin(screen);

            //Verify records in Mandatory Field Editor table
            if (!Pages.Admin.Fn_Verify_Record_Displayed_In_FieldEditorTable())
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        //21.	Admin Screen - Language Editor
        public void WL_CAN_HR_UI_002100_Admin_Language_Editor()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProfile = data.GetTestData("User_Profile");
            string screen = data.GetTestData("Admin_Screen");

            string textFields = data.GetTestData("Verify_TextFields");
            string buttons = data.GetTestData("Verify_Buttons");
            string drpDowns = data.GetTestData("Verify_DropDowns");
            string checkBoxes = data.GetTestData("Verify_CheckBoxes");
            string toolBars = data.GetTestData("Verify_ToolBars");
            string tableColumns = data.GetTestData("Verify_TableColumns");
            string labelFields = data.GetTestData("Verify_LabelFields");
            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProfile);

            //Navigate to Language Editor screen in Admin
            Pages.Admin.Fn_Navigate_Through_Admin(screen);

            //Verify all fields in Admin->Language Editor
            if (Pages.Admin.Fn_Verify_Fields_In_Admin_Screens(textFields, buttons, drpDowns, checkBoxes, toolBars, tableColumns, labelFields))
            {
                test.Pass("All fields in Language Editor screen under Admin are verified successfully");
            }
            else
            {
                test.Fail("Failed to verify all fields in Language Editor screen under Admin");
            }

            //Verify records in Language Editor table
            Pages.Admin.Fn_Verify_Record_Displayed_In_LanguageEditorTable();
        }

        [TestMethod]
        //22.	Admin Screen - Export FTP
        public void WL_CAN_HR_UI_002200_Admin_Export_FTP()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProfile = data.GetTestData("User_Profile");
            string screen = data.GetTestData("Admin_Screen");
            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProfile);

            //Navigate to Export FTP screen in Admin
            Pages.Admin.Fn_Navigate_Through_Admin(screen);

            //Verify records in Export FTP table
            if (!Pages.Admin.Fn_Verify_Record_Displayed_In_ExportFTPTable())
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        //23.	Admin Screen - Custom Field Config
        public void WL_CAN_HR_UI_002300_Admin_Custom_Field_Config()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string options = data.GetTestData("Admin_Options");
            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Custom Field Config screen in Admin
            Pages.Admin.Fn_Navigate_Through_Admin(options);

            //Verify records in Custom Field Config table
            if (!Pages.Admin.Fn_Verify_Record_Displayed_In_CustomFieldConfigTable())
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        //24.	Admin Screen - Security
        public void WL_CAN_HR_UI_002400_Admin_Security()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string[] options = data.GetTestData("Admin_Options").Split(';');
            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            bool status = true ;

            foreach(string option in options)
            {
                //Navigate to Security>User Admin, Role Editor, Group Editor, Security Categories screen in Admin
                if (!Pages.Admin.Fn_Navigate_Through_Admin(option))
                {
                    status = false;
                }

                Thread.Sleep(3000);
            }

            if (!status)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        //25.	Help
        public void WL_CAN_HR_UI_002500_Help()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string options = data.GetTestData("Help_Options");

            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Verify options under Help
            if (!Pages.Help.Fn_Verify_Options_Under_Help(options))
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        //26.  Emergency Contact Screen
        public void WL_CAN_HR_UI_002600_Validate_Emergency_Contacts_screen()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string screen = data.GetTestData("HR_Screen");
            string statusSearch = data.GetTestData("Search_Status");

            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            //Navigate to Employee Screen in Human Resources
            Pages.HR.Fn_NavigateThroughHumanResources(screen);

            //Search Active employees and select the first
            Pages.HR.Fn_Search_Employees_With_Status(statusSearch);

            //Verify Emergency contact screen for Update and Cancel button
            if(!Pages.HR.Fn_Verify_Employee_Emergency_Contact_Screen())
            {
                Assert.Fail("Failed to verify Employee Contacts screen");
            }

        }

        [TestMethod]
        //27. Verify Add Paycode screen
        public void WL_CAN_HR_UI_002700_Setup_Add_Income_Benefit_Deduction_Employer_in_Paycodes()
        {
            #region Data Variables
            string userLang = data.GetTestData("User_Language");
            string userProfile = data.GetTestData("User_Profile");
            string screen = data.GetTestData("Setup_Screen");
            string[] paycodes = data.GetTestData("PayCodeTypes").Split(';');

            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProfile);

            //Navigate to Paycode screen in Setup
            Pages.Setup.Fn_Navigate_Through_Setup(screen);

            Thread.Sleep(3000);

            foreach (string paycode in paycodes)
            {
                if (!Pages.Setup.Fn_Navigate_Verify_Add_Paycode_Screen(paycode))
                {
                    test.Fail("Failed to verify Add paycode screen : " + paycode);
                }
                else
                {
                    test.Pass("Successfully verified Add paycode screen : " + paycode);
                }

                Thread.Sleep(2000);
            }
        }


        //************************ Employee Hire Edit and Terminate ************************//
        #region Employee Hire Edit and Terminate

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

            string connection = GlobalDB.CreateConnectionString("dataSource".AppSettings(), "dbName".AppSettings(), "", "", false);
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

        [TestMethod]
        //27.	HR Screen - Edit
        public void WL_CAN_NEW_HIRE_001100_Edit_an_Employee_record()
        {
            string userlang = data.GetTestData("User_Language");
            string userProfile = data.GetTestData("User_Profile");
            string hrScreen = data.GetTestData("HR_Screen");

            string EmpNum = data.GetTestData("Emp_Number");
            /*string FirstName = "Derek1251";
            string LastName = "Shepard1251";
            string SIN = "999999997";*/

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userlang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProfile);

            //Navigate to Employee to search employee
            Pages.HR.Fn_NavigateThroughHumanResources(hrScreen);

            //Search Employee record
            if (!Pages.HR.Fn_Search_Employee_Exists(EmpNum, FirstName, LastName, "", true))
            {
                test.Fail("Employee does not exist");
                GenericMethods.CaptureScreenshot();
                Assert.Fail("Employee does not exist");
            }

            //Create method for Personal, Address and Phones in Biographical
            if (!Pages.HR.Fn_Edit_Biographical_Details_Of_Employee(EmpNum, "Ms.", NewFName, NewLName, "French", "1/1/1985", "Married", "Margaret", "Sandra", "Female", "Canada", NewSIN, "Non-Smoker", "Aboriginal Person;Person with Disability;Senior Citizen;Visible Minority;Woman", "Bilingual", "Canadian", "789", "789", "Home Address", "Address Line 1", "Address Line 2", "A1A 1A1", "", "", "", "Work", "9990009999"))
            {
                GenericMethods.CaptureScreenshot();
                Assert.Fail();
            }

            //Search Employee record
            if (!Pages.HR.Fn_Search_Employee_Exists(EmpNum, NewFName, NewLName, "", true))
            {
                test.Fail("Employee does not exist");
                GenericMethods.CaptureScreenshot();
                Assert.Fail();
            }
            else
            {
                test.Pass("Employee details edited successfully");
            }

        }

        [TestMethod]
        //28.	HR Screen - Terminate Employee
        public void WL_CAN_NEW_HIRE_001200_Terminate_an_Employee_record()
        {
            string EmpNum = data.GetTestData("Emp_Number");
            //string FirstName = "Sandra";
            //string LastName = "Robert";

            string userlang = data.GetTestData("User_Language");
            string userProfile = data.GetTestData("User_Profile");
            string hrScreen = data.GetTestData("HR_Screen1");

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userlang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProfile);

            //Navigate to Employee to search employee
            Pages.HR.Fn_NavigateThroughHumanResources(hrScreen);

            //Search Employee record
            if (!Pages.HR.Fn_Search_Employee_Exists(EmpNum, NewFName, NewLName, "", true))
            {
                test.Fail("Employee does not exist");
                GenericMethods.CaptureScreenshot();
                Assert.Fail();
            }

            if (!Pages.HR.Fn_Terminate_Employee("04/15/2019", "04/14/2019", "Termination", "Resigned"))
            {
                test.Fail("Employee Termination failed");
                GenericMethods.CaptureScreenshot();
                Assert.Fail();
            }

            //Navigate to Employee to search employee
            Pages.HR.Fn_NavigateThroughHumanResources(hrScreen);

            Pages.HR.Fn_Search_Employee_Exists(EmpNum, NewFName, NewLName, "", true);

            if (Pages.Payroll.Fn_Verify_Employee_Details_In_Payroll_Position("Terminated", "Status-Terminated"))
            {
                test.Pass("Employee:" + NewFName + " " + NewLName + " Terminated Successfully");
            }
            else
            {
                test.Fail("Failed to terminate Employee : " + NewFName + " " + NewLName);
                GenericMethods.CaptureScreenshot();
                Assert.Fail();
            }


        }

        #endregion


    }
}
