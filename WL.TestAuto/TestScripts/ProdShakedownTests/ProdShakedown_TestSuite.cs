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
    public class ProdShakedown_TestSuite : AutomationCore
    {
        public string xmlDataFile = projectDirectory + "\\TestScripts\\PayrollTests\\ProcessGroupDetails.xml";

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
    }
}
