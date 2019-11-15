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

namespace WL.TestAuto
{
    [TestClass]
    public class SmokeTestSuite : AutomationCore
    {
        [AssemblyInitialize]
        public static void StartClass(TestContext context)
        {
            #region Report Initialize
            //htmlReporter.LoadConfig(projectDirectory + "\\" + @"Extent-Config.xml");
            htmlReporter.Config.DocumentTitle = ConfigurationManager.AppSettings["TestName"].ToString() + " Report";
            htmlReporter.Config.ReportName = ConfigurationManager.AppSettings["TestName"].ToString();
            
            extent.AttachReporter(htmlReporter);

            extent.AddSystemInfo("OS : ", Environment.OSVersion.ToString());
            extent.AddSystemInfo("Host Name : ", Dns.GetHostName());
            extent.AddSystemInfo("Browser : ", ConfigurationManager.AppSettings["browser"]);
            #endregion
        }

        [TestMethod]
        //1.	Human Resources - Employee Screen
        public void ZHRX_CAN_HR_UI_000100_Verify_Human_Resources_Employee_Screen()
        {
            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage("English");

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile("Administrator");

            //Navigate to Employee Screen in Human Resources
            Pages.HR.Fn_NavigateThroughHumanResources("Employee");

            //Verify all fields in Employee Screen
            if (Pages.HR.Fn_Verify_Fields_In_HR_Screens("Employee Number;Last Name;First Name;SIN;Organizational Unit", "Clear;Search", "Process Group", "Active;Benefited Leave;Leave;Terminated", "Biographical;Position;Employment;Terminations", "Employee Number;Last Name;First Name;Status;Process Group;Organizational Unit", ""))
            {
                test.Log(Status.Pass, "All fields in Employee screen verified successfully");
            }
            else
            {
                test.Fail("Failed to verify all fields in Employee screen");
            }

            //Search Employee records present
            if (Pages.HR.Fn_Verify_Record_Displayed_In_EmployeeTable())
            {
                test.Log(Status.Pass, "Verified Employee records displayed");
            }
            else
            {
                test.Fail("Failed to verify Employee record displayed");
            }
        }

        [TestMethod]
        //2.	HR Screen - Open Screens in English **reports
        public void ZHRX_CAN_HR_UI_000200_Verify_Human_Resources_HR_Screen_Open_Screens_In_English()
        {
            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage("English");

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile("Administrator");

            //Navigate to Employee Scren in Human Resources
            Pages.HR.Fn_NavigateThroughHumanResources("Employee");

            //Navigate to Add Wizard Scren in Human Resources
            Pages.HR.Fn_NavigateThroughHumanResources("Add Wizard");

            //Navigate to Reports Screen in Human Resources
            if (Pages.HR.Fn_NavigateThroughHumanResources("Reports-Anniversary Listing"))
            {
                test.Log(Status.Pass, "Navigated to Reports-Anniversary Listing Screen");
            }
            else
            {
                test.Fail("Failed to navigate to Reports-Anniversary Listing Screen");
            }

            //Verify Anniversary Listing report
            if(Pages.HR.Fn_ViewAndVerify_HR_ReportDisplayedOnScreen("Anniversary Listing"))
            {
                test.Log(Status.Pass, "View and verify PDF report Aniversary Listing successful");
            }
            else
            {
                test.Fail("Failed to verify PDF report displayed : Anniversary Listing");
            }

            //Navigate to Home
            if(Pages.Home.Fn_NavigateToHomeScreen())
            {
                test.Log(Status.Pass, "Navigated to Home Screen");
            }
            else
            {
                test.Fail("Failed to navigate to Home screen");
            }

            //Navigate to Reports Screen in Human Resources
            if (Pages.HR.Fn_NavigateThroughHumanResources("Reports-Employee Listing"))
            {
                test.Log(Status.Pass, "Navigated to Reports-Employee Listing Screen");
            }
            else
            {
                test.Fail("Failed to navigate to Reports-Employee Listing Screen");
            }

            //Verify Employee Listing report
            if (Pages.HR.Fn_ViewAndVerify_HR_ReportDisplayedOnScreen("Employee Listing"))
            {
                test.Log(Status.Pass, "View and verify PDF report Employee Listing successful");
            }
            else
            {
                test.Fail("Failed to verify PDF report displayed : Employee Listing");
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

            //Navigate to Reports Screen in Human Resources
            if (Pages.HR.Fn_NavigateThroughHumanResources("Reports-Paycode Report"))
            {
                test.Log(Status.Pass, "Navigated to Reports-Paycode Report Screen");
            }
            else
            {
                test.Fail("Failed to navigate to Reports-Paycode Report Screen");
            }
        }

        [TestMethod]
        //3.	HR Screen - Open Screens in French **reports
        public void ZHRX_CAN_HR_UI_000300_HR_Screen_Open_Screens_In_French()
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
            if (Pages.HR.Fn_ViewAndVerify_HR_ReportDisplayedOnScreen("Anniversary Listing"))
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
            if (Pages.HR.Fn_ViewAndVerify_HR_ReportDisplayedOnScreen("Employee Listing"))
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
        //4.	Payroll Screen - Employee Screen
        public void ZHRX_CAN_HR_UI_000400_Payroll_Screen_Employee_Screen()
        {
            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage("English");

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile("Administrator");

            //Navigate to Employee Screen in Human Resources
            if (Pages.Payroll.Fn_NavigateThroughPayroll("Employee"))
            {
                test.Log(Status.Pass, "Navigated to Employee Screen under Payroll");
            }
            else
            {
                test.Fail("Failed to navigate to Employee Screen under Payroll");
            }

            //Verify all fields in Employee Screen
            Pages.Payroll.Fn_Verify_Fields_In_Payroll_Screens("Employee Number;Last Name;First Name;SIN;Organizational Unit", "Clear;Search", "Process Group", "Active;Benefited Leave;Leave;Terminated", "Biographical;Position;Employment;Terminations;Direct Deposit;Stat Deductions;Paycodes", "Employee Number;Last Name;First Name;Status;Process Group;Organizational Unit", "");

            //Search Employee records present
            if (Pages.HR.Fn_Verify_Record_Displayed_In_EmployeeTable())
            {
                test.Log(Status.Pass, "Verified Employee records displayed");
            }
            else
            {
                test.Fail("Failed to verify Employee record displayed");
            }
        }

        [TestMethod]
        //5.	Payroll Screen - Batch Screen
        public void ZHRX_CAN_HR_UI_000500_Payroll_Screen_Batch_Screen()
        {
            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage("English");

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile("Administrator");

            //Navigate to Batch Screen in Payroll
            if (Pages.Payroll.Fn_NavigateThroughPayroll("Batch"))
            {
                test.Log(Status.Pass, "Navigated to Batch Screen under Payroll");
            }
            else
            {
                test.Fail("Failed to navigate to Batch Screen under Payroll");
            }

            //Verify all fields in Batch Screen
            Pages.Payroll.Fn_Verify_Fields_In_Payroll_Screens("Batch Source;Description", "Clear;Search", "Process Group;Run Type;Approval Status", "Processed", "Add;Delete", "", "");
        }

        [TestMethod]
        //6.	Payroll Screen - Payroll Transaction
        public void ZHRX_CAN_HR_UI_000600_Payroll_Screen_Payroll_Transaction()
        {
            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage("English");

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile("Administrator");

            //Navigate to Payroll Transaction under Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll("Payroll Transaction");

            //To get control on the Iframe window of the page
            Browsers.GetDriver.SwitchTo().Frame("payrollException");

            //Verify all fields in Payroll Screen
            Pages.Payroll.Fn_Verify_Fields_In_Payroll_Screens("Minimum Work Hour;Maximum Lunch Minute;Start Date;End Date;Employee Number;Last Name", "Search;Clear", "", "", "", "Employee Number;Last Name;First Name;Date;Payable Hours;Non-Payable Hours;Total Payable Hours", "");

            //Verify records displayed in Payroll Transaction
            Pages.Payroll.Fn_Verify_Record_Displayed_In_PayrollTransactionTable();

            //To get control on the Iframe window of the page
            Browsers.GetDriver.SwitchTo().DefaultContent();

        }

        [TestMethod]
        //7.	Payroll Screen - Payroll Process
        public void ZHRX_CAN_HR_UI_000700_Payroll_Screen_PayrollProcess()
        {
            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage("English");

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile("Administrator");

            //Navigate to Payroll process Screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll("Payroll Process");

            //Verify all fields in Payroll Screen
            Pages.Payroll.Fn_Verify_Fields_In_Payroll_Screens("", "", "Process Group;Run Type", "", "Calculate;Undo Calc;Post;Reports;Details;Add;Delete;Export", "", "Status;Cheque Date;Start Date;Period End Date;Last Calc Time;Last Calc User");
        }

        [TestMethod]
        //8.	Payroll Screen - Payments
        public void ZHRX_CAN_HR_UI_000800_Payroll_Screen_Payments()
        {
            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage("English");

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile("Administrator");

            //Navigate to Payments screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll("Payments");

            //Verify all fields in Payroll Screen
            Pages.Payroll.Fn_Verify_Fields_In_Payroll_Screens("", "Clear;Search", "Process Group;Run Type", "", "Mass Payslip File;Mass Payslip Print;Reports", "", "");

            //Verify reports present in Payments UI
            Pages.Payroll.Fn_Verify_Reports_In_Payroll_PaymentsTable("Payroll Funding Jamaica;Register Detail;Register Summary;Changes;Compensation List;Current Vs Prior;Garnishment;Pay Exception;Jamaica Deduction Summary;Detail Earn/Ben/Dedn;GL Employee Details");
        }

        [TestMethod]
        //9.	Payroll Screen - Import
        public void ZHRX_CAN_HR_UI_000900_Payroll_Screen_Import()
        {
            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage("English");

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile("Administrator");

            //Navigate to Import screen in Payroll
            if (Pages.Payroll.Fn_NavigateThroughPayroll("Import"))
            {
                test.Log(Status.Pass, "Navigated to Import Screen under Payroll");
            }
            else
            {
                test.Fail("Failed to navigate to Import Screen under Payroll");
            }

            //verify Import Types in dropdown
            Pages.Payroll.Fn_Verify_ImportTypes_In_Payroll_Import("Address Import;Advantage Time File Import;Banking Import;Employee Import;Employment Import;Position Import;Standard paycode Import;Standard Payroll Adjustment Import;Standard Payroll Bonus Import;Standard Payroll Import;Statutory Deduction Import;Year End Adjustment Import;Year End Adjustment Paycode Import");
        }

        [TestMethod]
        //10.	Payroll Screen - Reports
        public void ZHRX_CAN_HR_UI_001000_Payroll_Screen_Reports()
        {
            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage("English");

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile("Administrator");

            //Export Report
            Pages.Payroll.Fn_Verify_And_Navigate_To_Reports_In_Payroll("Wage Type Catalog>Excel");
        }

        [TestMethod]
        //11.	Setup Screen - Policy **
        public void ZHRX_CAN_HR_UI_001100_Setup_Screen_Policy()
        {
            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage("English");

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile("Administrator");

            //Navigate to Policy screen in Setup
            Pages.Setup.Fn_Navigate_Through_Setup("Policy");

            //Verify all fields in Setup->Policy
            if (Pages.Setup.Fn_Verify_Fields_In_Setup_Screens("Description", "Clear;Search", "", "", "Details", "Description", ""))
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
        //12.	Setup Screen - Entitlement **
        public void ZHRX_CAN_HR_UI_001200_Setup_Screen_Entitlement()
        {
            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage("English");

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile("Administrator");

            //Navigate to Entitlement screen in Setup
            Pages.Setup.Fn_Navigate_Through_Setup("Entitlement");

            //Verify all fields in Setup->Entitlement
            if (Pages.Setup.Fn_Verify_Fields_In_Setup_Screens("Description", "Clear;Search", "", "", "Details", "Description", ""))
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
        public void ZHRX_CAN_HR_UI_001300_Setup_Screen_Vendor()
        {
            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage("English");

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile("Administrator");

            //Navigate to Vendor screen in Setup
            Pages.Setup.Fn_Navigate_Through_Setup("Vendor");

            //Verify all fields in Setup->Vendor
            if (Pages.Setup.Fn_Verify_Fields_In_Setup_Screens("", "", "", "", "Add", "", ""))
            {
                test.Pass("All fields in Vendor screen under Setup are verified successfully");
            }
            else
            {
                test.Fail("Failed to verify all fields in Vendor screen under Setup");
            }

            //Verify records exist in Vendor Table
            Pages.Setup.Fn_Verify_Record_Displayed_In_VendorTable();
        }

        [TestMethod]
        //14.	Setup Screen - Payroll Processing Group
        public void ZHRX_CAN_HR_UI_001400_Setup_Screen_Payroll_Processing_Group()
        {
            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage("English");

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile("Administrator");

            //Navigate to Payroll Processing Group screen in Setup
            Pages.Setup.Fn_Navigate_Through_Setup("Payroll Processing Group");

            //Verify all fields in Setup->Payroll Processing Group
            if (Pages.Setup.Fn_Verify_Fields_In_Setup_Screens("", "", "", "", "Add", "Country;Process Group", ""))
            {
                test.Pass("All fields in Payroll Processing Group screen under Setup are verified successfully");
            }
            else
            {
                test.Fail("Failed to verify all fields in Payroll Processing Group screen under Setup");
            }

            //Verify records exist in Payroll Processing Group Table
            Pages.Setup.Fn_Verify_Record_Displayed_In_PayProcessGroupTable();
        }

        [TestMethod]
        //15.	Setup Screen - Organizational Unit Level
        public void ZHRX_CAN_HR_UI_001500_Setup_Screen_Organizational_Unit_Level()
        {
            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage("English");

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile("Administrator");

            //Navigate to Organizational Unit Level screen in Setup
            Pages.Setup.Fn_Navigate_Through_Setup("Organizational Unit Level");

            //Verify records in table column
            Pages.Setup.Fn_Verify_Records_Of_Single_Column_In_OUL("English Desc.", "Company;Legal Entity;Business Unit;Department");
        }

        [TestMethod]
        //16.	Setup Screen - Wizard Tempate
        public void ZHRX_CAN_HR_UI_001600_Setup_Screen_Wizard_Template()
        {
            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage("English");

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile("Administrator");

            //Navigate to Wizard Tempate screen in Setup
            Pages.Setup.Fn_Navigate_Through_Setup("Wizard Template");

            //Verify records exist in Wizard Template Table
            Pages.Setup.Fn_Verify_Record_Displayed_In_WizardTable();
        }

        [TestMethod]
        //17.	Setup Screen - Paycode
        public void ZHRX_CAN_HR_UI_001700_Setup_Screen_Paycode()
        {
            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage("English");

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile("Administrator");

            //Navigate to Paycode screen in Setup
            Pages.Setup.Fn_Navigate_Through_Setup("Paycode>Global Employee Paycode");

            Thread.Sleep(3000);

            //Navigate to Paycode screen in Setup
            Pages.Setup.Fn_Navigate_Through_Setup("Paycode>Paycode Association");

            //Verify records exist in Paycode Association Tables
            Pages.Setup.Fn_Verify_Record_Displayed_In_PaycodeAssociationTable();

            Thread.Sleep(3000);

            //Navigate to Paycode screen in Setup
            Pages.Setup.Fn_Navigate_Through_Setup("Paycode>Paycode Maintenance");

            //Verify all fields in Setup->Paycode>Paycode Maintenance
            if (Pages.Setup.Fn_Verify_Fields_In_Setup_Screens("", "Refresh", "", "", "Add;Details;Delete;Export;Import;Reports", "Paycode;English Description;Paycode Type", ""))
            {
                test.Pass("All fields in Paycode>Paycode Maintenance screen under Setup are verified successfully");
            }
            else
            {
                test.Fail("Failed to verify all fields in Paycode>Paycode Maintenance screen under Setup");
            }

            //Verify records exist in Paycode Maintenance Table
            Pages.Setup.Fn_Verify_Record_Displayed_In_PaycodeMaintTable();
        }

        [TestMethod]
        //18.	Admin Screen - Code Maintenance
        public void ZHRX_CAN_HR_UI_001800_Admin_Code_Maintenance()
        {
            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage("English");

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile("Administrator");

            //Navigate to Code Maintenance screen in Admin
            Pages.Admin.Fn_Navigate_Through_Admin("Code Maintenance");

            //Verify code types listed in drop down Code Table
            Pages.Admin.Fn_Verify_CodeTypes_Dropdown_In_CodeMaint(2);
        }

        [TestMethod]
        //19.	Admin Screen - Code System
        public void ZHRX_CAN_HR_UI_001900_Admin_Code_System()
        {
            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage("English");

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile("Administrator");

            //Navigate to Code System screen in Admin
            Pages.Admin.Fn_Navigate_Through_Admin("Code System");

            //Verify records in Code System table
            Pages.Admin.Fn_Verify_Record_Displayed_In_CodeSystemTable();
        }

        [TestMethod]
        //20.	Admin Screen - Mandatory Field Editor
        public void ZHRX_CAN_HR_UI_002000_Admin_Mandatory_Field_Editor()
        {
            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage("English");

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile("Administrator");

            //Navigate to Mandatory Field Editor screen in Admin
            Pages.Admin.Fn_Navigate_Through_Admin("Mandatory Field Editor");

            //Verify records in Mandatory Field Editor table
            Pages.Admin.Fn_Verify_Record_Displayed_In_FieldEditorTable();
        }

        [TestMethod]
        //21.	Admin Screen - Language Editor
        public void ZHRX_CAN_HR_UI_002100_Admin_Language_Editor()
        {
            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage("English");

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile("Administrator");

            //Navigate to Language Editor screen in Admin
            Pages.Admin.Fn_Navigate_Through_Admin("Language Editor");

            //Verify all fields in Admin->Language Editor
            if (Pages.Admin.Fn_Verify_Fields_In_Admin_Screens("Form Description", "Clear;Search", "", "", "Details", "", ""))
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
        public void ZHRX_CAN_HR_UI_002200_Admin_Export_FTP()
        {
            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage("English");

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile("Administrator");

            //Navigate to Export FTP screen in Admin
            Pages.Admin.Fn_Navigate_Through_Admin("Export FTP");

            //Verify records in Export FTP table
            Pages.Admin.Fn_Verify_Record_Displayed_In_ExportFTPTable();
        }

        [TestMethod]
        //23.	Admin Screen - Custom Field Config
        public void ZHRX_CAN_HR_UI_002300_Admin_Custom_Field_Config()
        {
            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage("English");

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile("Administrator");

            //Navigate to Custom Field Config screen in Admin
            Pages.Admin.Fn_Navigate_Through_Admin("Custom Field Config");

            //Verify records in Custom Field Config table
            Pages.Admin.Fn_Verify_Record_Displayed_In_CustomFieldConfigTable();
        }

        [TestMethod]
        //24.	Admin Screen - Security
        public void ZHRX_CAN_HR_UI_002400_Admin_Security()
        {
            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage("English");

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile("Administrator");

            //Navigate to Security>User Admin screen in Admin
            Pages.Admin.Fn_Navigate_Through_Admin("Security>User Admin");

            Thread.Sleep(3000);

            //Navigate to Security>Role Editor screen in Admin
            Pages.Admin.Fn_Navigate_Through_Admin("Security>Role Editor");

            Thread.Sleep(3000);

            //Navigate to Security>Group Editor screen in Admin
            Pages.Admin.Fn_Navigate_Through_Admin("Security>Group Editor");

            Thread.Sleep(3000);

            //Navigate to Security>Security Categories screen in Admin
            Pages.Admin.Fn_Navigate_Through_Admin("Security>Security Categories");
        }

        [TestMethod]
        //25.	Help
        public void ZHRX_CAN_HR_UI_002500_Help()
        {
            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage("English");

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile("Administrator");

            //Verify options under Help
            Pages.Help.Fn_Verify_Options_Under_Help("PasswordChange;About;Cache Flush");
        }

        [TestMethod]
        //26.	HR Screen - Hire
        public void ZHRX_CAN_NEW_HIRE_001000_Hire_an_Employee()
        {
            string EmpNum = "99999";
            string FirstName = "Derek";
            string LastName = "Shepard";
            string SIN = "999999997";

            string suffix = DateTime.Now.ToString("HHmm");
            //EmpNum = (Convert.ToInt32(EmpNum) + 1).ToString();
            FirstName = FirstName + suffix;
            LastName = LastName + suffix;

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage("English");

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile("Administrator");

            //Navigate to Employee to search employee
            Pages.HR.Fn_NavigateThroughHumanResources("Employee");
            if(Pages.HR.Fn_Search_Employee_Exists(EmpNum, FirstName, LastName, "", false))
            {
                test.Pass("Employee Does not Exist");
            }
            else
            {
                test.Pass("Employee already Exists");
                EndTest();
            }

            //Navigate to Add Wizard Scren in Human Resources
            Pages.HR.Fn_NavigateThroughHumanResources("Add Wizard");
            Pages.HR.Fn_Hire_Employee(EmpNum, FirstName, LastName, SIN);

            Thread.Sleep(5000);

            //Verify Employee hired Successfully
            Pages.Payroll.Fn_NavigateThroughPayroll("Employee");
            Pages.HR.Fn_Search_Employee_Exists(EmpNum, FirstName, LastName, "", true);

            Pages.Payroll.Fn_Verify_Employee_Details_In_Payroll_Position("Active", "Annual Salary-60,000.00");

        }

        [TestMethod]
        //27.	HR Screen - Edit
        public void ZHRX_CAN_NEW_HIRE_001100_Edit_an_Employee_record()
        {
            string EmpNum = "99999";
            string FirstName = "Derek1443";
            string LastName = "Shepard1443";
            string SIN = "999999997";

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage("English");

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile("Administrator");

            //Navigate to Employee to search employee
            Pages.HR.Fn_NavigateThroughHumanResources("Employee");

            //Search Employee record
            if (!Pages.HR.Fn_Search_Employee_Exists(EmpNum, FirstName, LastName, "", true))
            {
                test.Fail("Employee does not exist");
                EndTest();
            }

            //Create method for Personal, Address and Phones in Biographical
            Pages.HR.Fn_Edit_Biographical_Details_Of_Employee(EmpNum, "Ms.", "Sandra", "Robert", "French", "1/1/1985", "Married", "Margaret", "Sandra", "Female", "Canada", "715259628", "Non-Smoker", "Aboriginal Person;Person with Disability;Senior Citizen;Visible Minority;Woman", "Bilingual", "Canadian", "789", "789", "Home Address", "Address Line 1", "Address Line 2", "A1A 1A1", "", "", "", "Work", "9990009999");

            FirstName = "Sandra";
            LastName = "Robert";
            SIN = "715259628";

            //Search Employee record
            if (!Pages.HR.Fn_Search_Employee_Exists(EmpNum, FirstName, LastName, "", true))
            {
                test.Fail("Employee does not exist");
                EndTest();
            }
            else
            {
                test.Pass("Employee details edited successfully");
            }


        }

        [TestMethod]
        //28.	HR Screen - Terminate Employee
        public void ZHRX_CAN_NEW_HIRE_001200_Terminate_an_Employee_record()
        {
            string EmpNum = "99998";
            string FirstName = "Derek2028";
            string LastName = "Shepard2028";
            string SIN = "999999997";

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage("English");

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile("Administrator");

            //Navigate to Employee to search employee
            Pages.HR.Fn_NavigateThroughHumanResources("Employee");

            //Search Employee record
            if(!Pages.HR.Fn_Search_Employee_Exists(EmpNum, FirstName, LastName, "", true))
            {
                test.Fail("Employee does not exist");
                EndTest();
            }

            Pages.HR.Fn_Terminate_Employee("04/15/2019", "04/14/2019", "Termination", "Resigned");

            //Navigate to Employee to search employee
            Pages.HR.Fn_NavigateThroughHumanResources("Employee");

            Pages.HR.Fn_Search_Employee_Exists(EmpNum, FirstName, LastName, "", true);

            if(Pages.Payroll.Fn_Verify_Employee_Details_In_Payroll_Position("Terminated", "Status-Terminated"))
            {
                test.Pass("Employee:" + FirstName + " " + LastName + " Terminated Successfully");
            }
            else
            {
                test.Fail("Failed to terminate Employee : " + FirstName + " " + LastName);
            }


        }


    }
}
