using AutoIt;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using Org.BouncyCastle.Crypto.Engines;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Keys = OpenQA.Selenium.Keys;

namespace WL.TestAuto
{
    public class PayrollPage : AutomationCore
    {
        private IWebDriver driver;

        #region Payroll Page Object Collection

        [FindsBy(How = How.XPath, Using = "*//div[@id='ctl00_MainMenu']//span[text()='Payroll']")]
        private IWebElement Menu_Payroll { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id=\"ctl00_MainMenu\"]/ul/li[3]/div")]
        private IWebElement Menu_SlidePayroll { get; set; }

        [FindsBy(How = How.XPath, Using = "*//a[text()='Batch']")]
        private IWebElement Link_Batch { get; set; }

        [FindsBy(How = How.XPath, Using = "*//a[text()='Payroll Transaction']")]
        private IWebElement Link_PayTransact { get; set; }

        [FindsBy(How = How.XPath, Using = "*//a[text()='Payroll Process']")]
        private IWebElement Link_PayProcess { get; set; }

        [FindsBy(How = How.XPath, Using = "*//a[text()='Payments']")]
        private IWebElement Link_Payments { get; set; }

        [FindsBy(How = How.XPath, Using = "*//a[text()='Import']")]
        private IWebElement Link_Import { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='ctl00_MainMenu']//span[text()='Payroll']//following::a/span[text()='Reports']")]
        private IWebElement SubMenu_Reports { get; set; }

        [FindsBy(How = How.XPath, Using = "*//a[text()=\"Employee\" or text()=\"Employé\"]")]
        private IWebElement Link_Employee { get; set; }

        [FindsBy(How = How.XPath, Using = "*//input[@value='View Report']")]
        private IWebElement Btn_ViewReport { get; set; }

        [FindsBy(How = How.XPath, Using = "*//input[@value='Search']")]
        private IWebElement Btn_Search { get; set; }

        [FindsBy(How = How.XPath, Using = "*//button[text()='Search']")]
        private IWebElement Btn_SearchMatCard { get; set; }

        [FindsBy(How = How.XPath, Using = "*//table[@id='ctl00_MainContent_PayrollProcessSearchControl_PayrollProcessSummaryGrid_ctl00']")]
        private IWebElement Tbl_PaymentsPayroll { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[contains(@class,'rmExpandDown') and text()='Reports']")]
        private IWebElement Tab_Reports { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Mass Payslip File']")]
        private IWebElement Tab_MassPayslipFile { get; set; }

        [FindsBy(How = How.XPath, Using = ".//span[@class='rtbText' and text()='Export']")]
        private IWebElement Tab_Export { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[contains(@class,'rmExpandRight') and text()='Standard']")]
        private IWebElement SubMenu_Standard { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[contains(@class,'rmExpandRight') and text()='Custom']")]
        private IWebElement SubMenu_Custom { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Import Type']/parent::*/parent::*//input[@id='ctl00_MainContent_ImportControl_ImportExportId_Field_Input']")]
        private IWebElement DrpDwn_ImportType { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_MainContent_ImportControl_ImportExportId_Field_DropDown")]
        private IWebElement List_ImportType { get; set; }

        [FindsBy(How = How.XPath, Using = "/html/body/app-root/app-payroll-exception/mat-card/div/table")]
        private IWebElement Tbl_PayrollTransact { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Annual Salary']/parent::*/parent::*//input[@type='text']")]
        private IWebElement Txt_EmpAnnualSal { get; set; }

        [FindsBy(How = How.XPath, Using = "*//table[@id='ctl00_ContentPlaceHolder1_EmployeePositionControl1_EmployeePositionGrid_ctl00']")]
        private IWebElement Tbl_EmployeePosition { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Position']")]
        private IWebElement Btn_EmpPosition { get; set; }

        [FindsBy(How = How.XPath, Using = "*//em[contains(text(),'Employee Position')]")]
        private IWebElement Lbl_EmployeePosition { get; set; }

        [FindsBy(How = How.XPath, Using = "*//iframe[contains(@name,'PositionWindows')]")]
        private IWebElement Frame_EmployeePosition { get; set; }

        [FindsBy(How = How.XPath, Using = "*//a[@class='rwCloseButton' and @title='Close']")]
        private IWebElement Btn_CloseX { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Undo Calc']")]
        private IWebElement Btn_UndoCalc { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Calculate']")]
        private IWebElement Btn_Calculate { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Post']")]
        private IWebElement Btn_Post { get; set; }

        [FindsBy(How = How.XPath, Using = "*//div[contains(@class,'RadComboBoxDropDown') and contains(@style,'display: block')]")]
        public IWebElement List_AllDrpDwn { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Process Group']/parent::*/parent::*//input[@type='text']")]
        public IWebElement DrpDwn_ProcessGrp { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Run Type']/parent::*/parent::*//input[@type='text']")]
        private IWebElement DrpDwn_RunType { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Cheque Date']/parent::*/parent::*//input[@class='riTextBox riEnabled']")]
        private IWebElement Txt_ChequeDate { get; set; }

        [FindsBy(How = How.XPath, Using = "*//a[@id='ctl00_MainContent_PayrollProcessControl1_PayrollProcessView_ChequeDate_Field_popupButton']")]
        private IWebElement Calendar_ChequeDate { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span['Status']/parent::*/parent::*//span[@id='MainContent_PayrollProcessControl1_PayrollProcessView_PayrollProcessStatusCode_Field']")]
        private IWebElement Lbl_PayrollStatus { get; set; }

        [FindsBy(How = How.XPath, Using = "*//td[@class='rwWindowContent rwExternalContent rwLoading']")]
        private IWebElement Win_PDFReportLoad { get; set; }

        [FindsBy(How = How.XPath, Using = "*//td[@class='rwWindowContent rwExternalContent']")]
        private IWebElement Win_PDFReport { get; set; }

        [FindsBy(How = How.XPath, Using = "*//iframe[contains(@name,'RegisterReportWindow') or contains(@name,'PayrollProcessWindow')]")]
        private IWebElement Frame_PDFReport { get; set; }

        [FindsBy(How = How.XPath, Using = "*//div[@class='rmScrollWrap rmGroup rmLevel1']//a[@class='rmBottomArrow']")]
        private IWebElement Btn_ScrollDownArrow { get; set; }

        [FindsBy(How = How.XPath, Using = "*//div[@class='rmScrollWrap rmGroup rmLevel1']//a[@class='rmTopArrow']")]
        private IWebElement Btn_ScrollUpArrow { get; set; }

        [FindsBy(How = How.XPath, Using = "//li[@class='rtbItem rtbBtn']//span[text()='Pay Slips']")]
        private IWebElement Btn_PaySlips { get; set; }

        [FindsBy(How = How.XPath, Using = "//em[contains(text(),'Pay Slips')]")]
        private IWebElement Lbl_PaySlips { get; set; }

        [FindsBy(How = How.XPath, Using = "*//iframe[contains(@name,'EmployeeWindows')]")]
        private IWebElement Frame_EmployeeWizard { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[contains(@id,'EmployeePaySlipControl_PayrollProcessSummaryGrid_GridData')]//table[@class='rgMasterTable rgClipCells rgClipCells']")]
        private IWebElement Table_EmployeePayrollPeriod { get; set; }

        [FindsBy(How = How.XPath, Using = "*//iframe[contains(@id,'_RadPane') and contains(@src, 'PDF')]")]
        private IWebElement PDFReportArea { get; set; }

        [FindsBy(How = How.XPath, Using = ".//span[text()='Start Date']//following::span[contains(@id,'StartDate_Field')]")]
        private IWebElement Lbl_PeriodStartDate { get; set; }

        [FindsBy(How = How.XPath, Using = ".//span[text()='Period End Date']//following::span[contains(@id,'CutoffDate_Field')]")]
        private IWebElement Lbl_PeriodEndDate { get; set; }

        [FindsBy(How = How.XPath, Using = ".//table[@id='ctl00_MainContent_RoeCreationSearchControl1_EmployeeSummaryGrid_ctl00']")]
        private IWebElement Tbl_ROECreationSearch { get; set; }

        [FindsBy(How = How.XPath, Using = ".//span[@class='rtbText' and text()='Export ROE']")]
        private IWebElement Tab_ExportROE { get; set; }
        
        [FindsBy(How = How.XPath, Using = ".//input[@id='ctl00_MainContent_NonStatDeductionsControl1_Date_Field_dateInput']")]
        private IWebElement Txt_DateNotStatDed { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Vendor']/parent::*/parent::*//input[@type='text']")]
        private IWebElement DrpDwn_Vendor { get; set; }

        [FindsBy(How = How.XPath, Using = "*//input[@value='Excel']")]
        private IWebElement Btn_ExportExcel { get; set; }

        [FindsBy(How = How.XPath, Using = "*//input[@value='PDF']")]
        private IWebElement Btn_ExportPDF { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Add']")]
        private IWebElement Btn_AddToTable { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Effective Date']/following::span/input[contains(@id,'EffectiveDate')]")]
        private IWebElement Txt_EffectiveDate { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Event/Action']/parent::*/parent::*//input[@type='text']")]
        private IWebElement DrpDwn_EventAction { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Regular/Temporary']/parent::*/parent::*//input[@type='text']")]
        private IWebElement DrpDwn_RegTemp { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Insert']")]
        private IWebElement Btn_InsertToTable { get; set; }


        #endregion

        #region Constructor
        public PayrollPage()
        {
            driver = Browsers.GetDriver;
        }
        #endregion

        #region Payroll Page reusable Methods

        //Naviate to Screens under Payroll
        public bool Fn_NavigateThroughPayroll(string Option)
        {
            Boolean flag = false;
            try
            {
                if (Option.Split('-')[0].ToLower().Equals("reports"))
                {
                    //Code for Reports - FORMAT: 'Reports-Report Name'
                    GenericMethods.SelectValueFromSlideDropDown(Menu_Payroll, "Payroll", Menu_SlidePayroll, Option.Split('-')[0], Option.Split('-')[1]);
                    IWebElement Link_subReport = driver.FindElement(By.XPath("*//a[text()='" + Option.Split('-')[1] + "']"));
                    if (Link_subReport.Exists(30))
                    {
                        Link_subReport.Highlight();
                        test.Pass("Verified 'Payroll -> Reports -> " + Option.Split('-')[1] + "' on page");
                        flag = true;
                    }
                    else
                    {
                        test.Fail("Failed to verify 'Payroll -> Reports -> " + Option.Split('-')[1] + "' on page");
                        GenericMethods.CaptureScreenshot();
                        flag = false;
                    }
                }

                if (Option != "")
                {
                    GenericMethods.SelectValueFromSlideDropDown(Menu_Payroll, "Payroll", Menu_SlidePayroll, Option);
                    if (driver.FindElement(By.XPath(".//a[text()='" + Option + "']")).Exists(30))
                    {
                        driver.FindElement(By.XPath(".//a[text()='" + Option + "']")).Highlight();
                        test.Pass("Verified 'Payroll -> " + Option + "' on page");
                        test.Pass("Navigated to " + Option + " Screen under Payroll");
                        flag = true;
                    }
                    else
                    {
                        test.Fail("Failed to verify 'Payroll -> " + Option + "' on page");
                        GenericMethods.CaptureScreenshot();
                        flag = false;
                    }
                }

            }
            catch (Exception ex)
            {
                test.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                GenericMethods.CaptureScreenshot();
                EndTest();
                //throw new Exception(ex.Message);
            }
            return flag;
        }

        //Verify fields under Payroll screens
        public bool Fn_Verify_Fields_In_Payroll_Screens(string textFields, string buttons, string drpDowns, string checkBoxes, string toolBars, string tableColumns, string labelFields)
        {
            Boolean flag = false;
            try
            {
                //Code for text field verification
                if (textFields.Length > 0)
                {
                    string[] text = textFields.Split(';');
                    foreach (string t in text)
                    {
                        IReadOnlyCollection<IWebElement> ele = driver.FindElements(By.XPath("*//input[@type='text' or @placeholder='" + t + "']/parent::*/parent::*//span[text()='" + t + "']"));
                        if (ele.Count > 0)
                        {
                            ele.FirstOrDefault().Highlight();
                            test.Pass("Text field: " + t + " found");
                            flag = true;
                        }
                        else
                        {
                            test.Fail("Failed to find Text Field: " + t);
                            flag = false;
                        }

                    }
                }

                //Code to verify buttons
                if (buttons.Length > 0)
                {
                    string[] btn = buttons.Split(';');
                    foreach (string b in btn)
                    {
                        IReadOnlyCollection<IWebElement> ele = driver.FindElements(By.XPath("*//input[(@type='button' or @type='submit') and @value=\"" + b + "\"]"));

                        if (ele.Count > 0)
                        {
                            ele.First().Highlight();
                            test.Pass("Button: " + b + " found");
                            flag = true;
                        }
                        else
                        {
                            ele = driver.FindElements(By.XPath("*//button[(@type='button' or @type='submit') and text()=\"" + b + "\"]"));

                            if (ele.Count > 0)
                            {
                                ele.First().Highlight();
                                test.Pass("Button: " + b + " found");
                                flag = true;
                            }
                            else
                            {
                                test.Fail("Failed to find Button: " + b);
                                flag = false;
                            }
                        }

                    }
                }

                //Code to verify checkboxes
                if (checkBoxes.Length > 0)
                {
                    string[] chk = checkBoxes.Split(';');
                    foreach (string ch in chk)
                    {
                        IReadOnlyCollection<IWebElement> ele = driver.FindElements(By.XPath("*//input[@type=\"checkbox\"]/parent::*/span[text()=\"" + ch + "\"]"));

                        if (ele.Count > 0)
                        {
                            ele.FirstOrDefault().Highlight();
                            test.Pass("Checkbox: " + ch + " found");
                            flag = true;
                        }
                        else
                        {
                            IReadOnlyCollection<IWebElement> chkAll = driver.FindElements(By.XPath("*//input[@type='checkbox' and contains(@id,'" + ch + "')]"));

                            if (chkAll.Count > 0)
                            {
                                chkAll.FirstOrDefault().Highlight();
                                test.Pass("Checkbox: " + ch + " found");
                                flag = true;
                            }
                        }

                        if (!flag)
                        {
                            test.Fail("Failed to find Checkbox: " + ch);
                        }
                    }
                }

                //Code to verify Dropdown or Comboboxes
                if (drpDowns.Length > 0)
                {
                    string[] drp = drpDowns.Split(';');
                    foreach (string dd in drp)
                    {
                        IReadOnlyCollection<IWebElement> ele = driver.FindElements(By.XPath("*//div[contains(@class,\"ComboBox\")]/parent::*/parent::*//span[text()=\"" + dd + "\"]"));
                        if (ele.Count > 0)
                        {
                            ele.FirstOrDefault().Highlight();
                            test.Pass("ComboBox: " + dd + " found");
                            flag = true;
                        }
                        else
                        {
                            test.Fail("Failed to find ComboBox: " + dd);
                            flag = false;
                        }
                    }
                }

                //Code to verify Toolbar or toolbar buttons
                if (toolBars.Length > 0)
                {
                    string[] toolBarL = toolBars.Split(';');
                    foreach (string tool in toolBarL)
                    {
                        IReadOnlyCollection<IWebElement> ele = driver.FindElements(By.XPath("*//div[contains(@class,\"ToolBar\")]//span[text()=\"" + tool + "\"]"));
                        if (ele.Count > 0)
                        {
                            ele.FirstOrDefault().Highlight();
                            test.Pass("ToolBar option: " + tool + " found");
                            flag = true;
                        }
                        else
                        {
                            test.Fail("Failed to find ToolBar option: " + tool);
                            flag = false;
                        }
                    }
                }

                //Code to verify Column Headers in Table
                if (tableColumns.Length > 0)
                {
                    string[] cols = tableColumns.Split(';');
                    foreach (string col in cols)
                    {
                        IReadOnlyCollection<IWebElement> ele = driver.FindElements(By.XPath("*//th/a[text()=\"" + col + "\"]"));

                        if (ele.Count > 0)
                        {
                            ele.FirstOrDefault().Highlight();
                            test.Pass("Column name: " + col + " found");
                            flag = true;
                        }
                        else
                        {
                            IReadOnlyCollection<IWebElement> ele1 = driver.FindElements(By.XPath("*//th[text()=\"" + col + "\"]"));
                            if (ele1.Count > 0)
                            {
                                ele1.FirstOrDefault().Highlight();
                                test.Pass("Column name: " + col + " found");
                                flag = true;
                            }
                            else
                            {
                                test.Fail("Failed to find Column name: " + col);
                                flag = false;
                            }

                        }
                    }
                }

                //Code to verify other Label fields
                if (labelFields.Length > 0)
                {
                    string[] labels = labelFields.Split(';');
                    foreach (string label in labels)
                    {
                        IReadOnlyCollection<IWebElement> ele = driver.FindElements(By.XPath("*//span[text()='" + label + "']"));
                        if (ele.Count > 0)
                        {
                            ele.FirstOrDefault().Highlight();
                            test.Pass("Label name: " + label + " found");
                            flag = true;
                        }
                        else
                        {
                            test.Fail("Failed to find Label name: " + label);
                            flag = false;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                test.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                GenericMethods.CaptureScreenshot();
                EndTest();
                //throw new Exception(ex.Message);
            }
            if (flag) test.Pass("All fields in Payroll Process screen under Payroll are verified successfully");
            else
            {
                test.Fail("Failed to verify all fields in Payroll Process screen under Payroll");
                GenericMethods.CaptureScreenshot();
            }

            return flag;
        }

        public bool Fn_Verify_ImportTypesDisplayed_In_Payroll_Import(string typeNames)
        {
            Boolean flag = false;
            try
            {
                if (DrpDwn_ImportType.Exists(5))
                {
                    DrpDwn_ImportType.Click();
                    Thread.Sleep(TimeSpan.FromSeconds(5));
                    var typeList = List_ImportType.FindElements(By.TagName("li"));
                    int failCnt = 0;
                    if (typeNames != "" && typeList.Count > 0)
                    {
                        string[] types = typeNames.Split(';');
                        foreach (string type in types)
                        {
                            int cnt = 0;
                            for (int t = 0; t < typeList.Count; t++)
                            {
                                if (type.ToLower().Equals(typeList[t].Text.ToLower()))
                                {
                                    cnt++;
                                    typeList[t].Highlight();
                                    test.Pass(type + " : Found");
                                    flag = true;
                                    break;
                                }
                            }
                            if (cnt == 0)
                            {
                                failCnt++;
                                test.Fail(type + " : Not Found");
                                flag = false;
                            }
                        }
                    }
                    else
                    {
                        test.Fail("No Types listed");
                        GenericMethods.CaptureScreenshot();
                        flag = false;
                    }

                    //if one type is not found return false
                    if (failCnt != 0)
                    {
                        GenericMethods.CaptureScreenshot(); flag = false;
                    }
                }
            }
            catch (Exception ex)
            {
                test.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                GenericMethods.CaptureScreenshot();
                EndTest();
                //throw new Exception(ex.Message);
            }

            if (flag) test.Pass("All Import Types available in UI");
            else test.Fail("Import Type verification in UI failed");
            return flag;

        }

        public bool Fn_Navigate_To_Reports_In_Payroll(string reportName)
        {
            Boolean flag = false;
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            try
            {
                if (reportName == "")
                {
                    test.Error("No report name specified");
                    GenericMethods.CaptureScreenshot();
                    return false;
                }

                string[] reportList = reportName.Split(';');
                foreach (string reportL in reportList)
                {

                    //report names Report1>subreport1>subsubreport...
                    string[] reportNav = reportL.Split('>');

                    if (Menu_Payroll.Exists(5))
                    {
                        Menu_Payroll.Click();
                        Thread.Sleep(3000);
                        if (SubMenu_Reports.Exists(5))
                        {
                            SubMenu_Reports.Click();
                            Thread.Sleep(3000);
                            if (reportNav[reportNav.Length - 1].ToString() == "Excel")
                            {
                                //Code to export report from menu in Excel
                                foreach (string report in reportNav)
                                {
                                    if (report.Equals("Excel"))
                                    {
                                        if (driver.FindElements(By.XPath("*//a/span[text()='" + report + "']")).Count > 0)
                                        {
                                            var element = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("*//a/span[text()='" + report + "']")));

                                            Actions action = new Actions(driver);
                                            action.MoveToElement(element).Perform();
                                            driver.FindElement(By.XPath("*//a/span[text()='" + report + "']")).Click();
                                            
                                            Thread.Sleep(2000);
                                            test.Pass("Exported " + reportNav[reportNav.Length - 1] + " report: " + reportNav[reportNav.Length - 2] + "from " + reportName);
                                            flag = true;
                                        }
                                        else
                                        {
                                            test.Fail("Failed to find or export report: " + reportNav[reportNav.Length - 2] + "from " + reportName);
                                            GenericMethods.CaptureScreenshot();
                                            flag = false;
                                        }
                                    }
                                    else
                                    {
                                        var element = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("*//span[text()='" + report + "']")));

                                        Actions action = new Actions(driver);
                                        action.MoveToElement(element).Perform();
                                        Thread.Sleep(2000);
                                    }

                                }
                            }
                            else
                            {
                                //Code to navigate to report page 
                                foreach (string report in reportNav)
                                {
                                    if (driver.FindElements(By.XPath("*//a/span[text()='" + report + "']")).Count > 0)
                                    {
                                        driver.FindElements(By.XPath("*//a/span[text()='" + report + "']"))[0].Click();
                                        Thread.Sleep(3000);
                                        if (report.Equals(reportNav[reportNav.Length - 1].ToString()))
                                        {
                                            Thread.Sleep(2000);
                                            if (driver.FindElements(By.XPath("*//a[text()='" + report + "']")).Count > 0)
                                            {
                                                driver.FindElements(By.XPath("*//a[text()='" + report + "']"))[0].Highlight();
                                                test.Pass("Successfully Navigated to report: " + report);
                                                flag = true;
                                            }
                                        }


                                    }
                                    else
                                    {
                                        test.Fail("Failed to navigate to report: " + report + "from " + reportName);
                                        GenericMethods.CaptureScreenshot();
                                        flag = false;
                                    }
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                test.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                GenericMethods.CaptureScreenshot();
                EndTest();
                //throw new Exception(ex.Message);
            }

            return flag;
        }

        public bool Fn_View_Verify_Payroll_Report_NonStatDed(string date, string vendor)
        {
            bool flag = false;
            try
            {
                if(date!="")
                {
                    Txt_DateNotStatDed.SetText(date);
                }

                if(DrpDwn_Vendor.SelectValueFromDropDown(vendor))
                {
                    GenericMethods.DeleteFilesFromDirectory(downloadsFolder, "NonStatDeductions");

                    flag = Pages.HR.Fn_Click_View_Report_And_Verify_PDF_Loaded("Non-Stat Deductions");
                    if (flag) ClickExportReport("Excel");

                    if (GenericMethods.WaitForFileExists(downloadsFolder, "NonStatDeductions"))
                    {
                        test.Pass("Report Non-Stat Deductions download successful");
                        flag = true;
                    }
                    else
                    {
                        test.Fail("Report Non-Stat Deductions download failed");
                        flag = false;
                    }
                }
                else
                {
                    test.Fail("Failed to load or verify Payroll report: Non-Stat Deductions");
                    GenericMethods.CaptureScreenshot();
                }

            }
            catch(Exception ex)
            {
                test.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                GenericMethods.CaptureScreenshot();
                throw new Exception(ex.Message);
            }
            return flag;
        }

        public void ClickExportReport(string reportFormat)
        {
            try
            {
                if(reportFormat.ToUpper().Equals("PDF"))
                {
                    Btn_ExportPDF.Click();
                }
                else
                {
                    Btn_ExportExcel.Click();
                }
                
            }
            catch (Exception ex)
            {
                test.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                GenericMethods.CaptureScreenshot();
                throw new Exception(ex.Message);
            }
            
        }

        public bool Fn_Verify_Record_Displayed_In_PayrollTransactionTable()
        {
            Boolean flag = false;
            try
            {
                Btn_SearchMatCard.Click();
                Thread.Sleep(10000);

                if (Tbl_PayrollTransact.Exists(10))
                {
                    if (Tbl_PayrollTransact.VerifyRecordDisplayedInTable(false))
                    {
                        flag = true;
                        test.Pass("Employees exists in table");
                    }
                    else
                    {
                        flag = false;
                        test.Fail("No Employee records displayed");
                        GenericMethods.CaptureScreenshot();
                    }
                }
                else
                {
                    test.Fail("Unable to find table Payroll Transaction");
                    GenericMethods.CaptureScreenshot();
                    flag = false;
                }
            }
            catch (Exception ex)
            {
                driver.SwitchTo().DefaultContent();
                test.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                GenericMethods.CaptureScreenshot();
                EndTest();
                //throw new Exception(ex.Message);
            }

            return flag;
        }

        public bool Fn_Add_Row_In_Payroll_Position(string empState, string eventAction, string regularTemp, [Optional] string effectiveDate)
        {
            bool flag = false;
            string latestEffDate;
            try
            {
                if (Btn_EmpPosition.Exists(10))
                {
                    Btn_EmpPosition.Click();
                    if (Lbl_EmployeePosition.Exists(15))
                    {
                        driver.SwitchTo().Frame(Frame_EmployeePosition);

                        if (Tbl_EmployeePosition.Exists() && Tbl_EmployeePosition.FindElements(By.XPath("./tbody/tr[1]/td[text()='" + empState + "']")).Count > 0)
                        {
                            Tbl_EmployeePosition.FindElement(By.XPath("./tbody/tr/td[text()='" + empState + "']")).Highlight();
                            Tbl_EmployeePosition.FindElement(By.XPath("./tbody/tr/td[text()='" + empState + "']")).Click();

                            latestEffDate = Tbl_EmployeePosition.FindElement(By.XPath("./tbody/tr[1]/td[2]")).Text;
                            Thread.Sleep(5000);
                            flag = true;
                        }
                        else
                        {
                            test.Fail("Latest row in position is not : " + empState);
                            driver.SwitchTo().DefaultContent();
                            Btn_CloseX.Click();
                            return false;
                        }

                        if(Btn_AddToTable.Exists(5))
                        {
                            Btn_AddToTable.Click();
                            Thread.Sleep(5000);
                            if (Convert.ToDateTime(latestEffDate).Date <= DateTime.Today)
                            {
                                effectiveDate = DateTime.Today.ToString("M/d/yyyy");
                                Txt_EffectiveDate.SetText(effectiveDate);
                               
                            }
                            else if(Convert.ToDateTime(latestEffDate).Date > DateTime.Today)
                            {
                                effectiveDate = Convert.ToDateTime(latestEffDate).AddDays(1).ToString("M/d/yyyy");
                                Txt_EffectiveDate.SetText(effectiveDate);
                            }
                            else
                            {
                                effectiveDate = Convert.ToDateTime(effectiveDate).ToString("M/d/yyyy");
                                Txt_EffectiveDate.SetText(effectiveDate);
                            }
                            Txt_EffectiveDate.SendKeys(Keys.Tab);
                            Thread.Sleep(5000);
                            DrpDwn_EventAction.SelectValueFromDropDown(eventAction);
                            Thread.Sleep(5000);
                            DrpDwn_RegTemp.SelectValueFromDropDown(regularTemp);
                            Thread.Sleep(5000);
                            Btn_InsertToTable.Click();
                            Thread.Sleep(10000);
                            flag = true;
                        }
                        else
                        {
                            test.Fail("Add row button is not available/Disabled");
                            driver.SwitchTo().DefaultContent();
                            Btn_CloseX.Click();
                            return false;
                        }

                        //code to verify new row added
                        if (Tbl_EmployeePosition.Exists() && Tbl_EmployeePosition.FindElements(By.XPath("./tbody/tr[1]/td[text()='" + effectiveDate + "']")).Count > 0)
                        {
                            Tbl_EmployeePosition.FindElement(By.XPath("./tbody/tr[1]/td[text()='" + effectiveDate + "']")).Highlight();
                            Tbl_EmployeePosition.FindElement(By.XPath("./tbody/tr[1]/td[text()='" + effectiveDate + "']")).Click();
                            Thread.Sleep(5000);
                            test.Pass("Employee position row with status : " + empState + " and effective date : "+effectiveDate+" added");
                            driver.SwitchTo().DefaultContent();
                            Btn_CloseX.Click();
                            flag = true;
                        }
                        else
                        {
                            test.Fail("Employee position row failed to add with effective date : " + effectiveDate);
                            driver.SwitchTo().DefaultContent();
                            Btn_CloseX.Click();
                            return false;
                        }

                    }
                    else
                    {
                        flag = false;
                    }

                }
                else
                {
                    flag = false;
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
        
        //Enter Employee details to verify in Field1-Value1;Field2-Value2,etc
        public bool Fn_Verify_Employee_Details_In_Payroll_Position(string empState, string arrayEmpDetails, [Optional] string effDate)
        {
            Boolean flag = false;
            try
            {
                if (Btn_EmpPosition.Exists(10))
                {
                    Btn_EmpPosition.Click();
                    if (Lbl_EmployeePosition.Exists(15))
                    {
                        flag = true;
                        driver.SwitchTo().Frame(Frame_EmployeePosition);

                        if (Tbl_EmployeePosition.Exists() && Tbl_EmployeePosition.FindElements(By.XPath("./tbody/tr/td[text()='" + empState + "']")).Count > 0)
                        {
                            Tbl_EmployeePosition.FindElement(By.XPath("./tbody/tr/td[text()='" + empState + "']")).Highlight();
                            Tbl_EmployeePosition.FindElement(By.XPath("./tbody/tr/td[text()='" + empState + "']")).Click();
                            Thread.Sleep(5000);
                            test.Pass("Employee position row with status : " + empState + " verified");
                        }
                        else
                        {
                            return false;
                        }

                        if (effDate != "" && effDate != null && effDate != string.Empty)
                        {
                            if (Tbl_EmployeePosition.Exists() && Tbl_EmployeePosition.FindElements(By.XPath("./tbody/tr/td[text()='" + effDate + "']")).Count > 0)
                            {
                                Tbl_EmployeePosition.FindElement(By.XPath("./tbody/tr/td[text()='" + effDate + "']")).Highlight();
                                flag = true;
                            }
                            else
                            {
                                return false;
                            }
                        }

                        if(arrayEmpDetails!="")
                        {
                            string[] empDetails = arrayEmpDetails.Split(';');
                            foreach (string empDetail in empDetails)
                            {
                                string field = empDetail.Split('-')[0];
                                string value = empDetail.Split('-')[1];

                                if (driver.FindElements(By.XPath("*//span[text()='" + field + "']/parent::*/following::*/span[text()='" + value + "']")).Count > 0)
                                {
                                    driver.FindElements(By.XPath("*//span[text()='" + field + "']/parent::*/following::*/span[text()='" + value + "']"))[0].Highlight();
                                    test.Pass("Field: " + field + " found with value: " + value);
                                    flag = true;
                                }
                                else if (driver.FindElements(By.XPath("*//span[text()='" + field + "']/parent::*/following::*/span/input[@value='" + value + "']")).Count > 0)
                                {
                                    driver.FindElements(By.XPath("*//span[text()='" + field + "']/parent::*/following::*/span/input[@value='" + value + "']"))[0].Highlight();
                                    test.Pass("Field: " + field + " found with value: " + value);
                                    flag = true;
                                }
                                else
                                {
                                    test.Fail("No Field: " + field + " with value: " + value + " is found");
                                    GenericMethods.CaptureScreenshot();
                                    flag = false;
                                }

                            }
                        }
                        else
                        {
                            test.Info("No Employee details provided to verify");
                        }
                        
                        driver.SwitchTo().DefaultContent();
                        Btn_CloseX.Click();
                    }
                    else
                    {
                        test.Fail("Employee Position window not opened");
                        GenericMethods.CaptureScreenshot();
                        flag = false;
                    }
                }
                else
                {
                    GenericMethods.CaptureScreenshot();
                    flag = false;
                }
            }
            catch (Exception ex)
            {
                test.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                GenericMethods.CaptureScreenshot();
                //EndTest();
                throw new Exception(ex.Message);
            }

            return flag;
        }

        //Method to calculate payroll in Payroll Process screen
        public bool Fn_Calculate_Payroll_In_Payroll_Process(string processGroup, string runType, string chequeDate)
        {
            bool flag = false;
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
            try
            {

                Fn_Undo_Payroll_Calculation(processGroup);

                if (DrpDwn_ProcessGrp.Exists(10))
                {
                    DrpDwn_ProcessGrp.SelectValueFromDropDown(processGroup);
                    Thread.Sleep(3000);
                }

                if (Fn_GetPayrollRunStatus() == "New")
                {
                    if (runType != "")
                    {
                        DrpDwn_RunType.SelectValueFromDropDown(runType);
                    }
                    Thread.Sleep(3000);
                    if (chequeDate != "")
                    {
                        //Txt_ChequeDate.SetText(chequeDate);
                        Calendar_ChequeDate.SelectDateFromCalendarPopup(chequeDate);
                    }
                    Thread.Sleep(2000);

                    Btn_Calculate.Click();
                    Thread.Sleep(3000);

                    if (wait.Until(drv => (Lbl_PayrollStatus.Text.ToLower() == "calculation processed") || (Lbl_PayrollStatus.Text.ToLower() == "calculation in arrears")))
                    {
                        flag = true;
                        test.Pass("Payroll calculated successfully");
                    }
                    else
                    {
                        test.Fail("Failed to calculate Payroll");
                        GenericMethods.CaptureScreenshot();
                        flag = false;
                    }
                    
                }
                else
                {
                    test.Fail("Failed to calculate Payroll");
                    GenericMethods.CaptureScreenshot();
                    flag = false;
                }

            }
            catch (Exception ex)
            {
                test.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                GenericMethods.CaptureScreenshot();
                EndTest();
                //throw new Exception(ex.Message);
            }
            return flag;
        }

        //Get Payroll run status from Payroll Process page
        public string Fn_GetPayrollRunStatus()
        {
            string status = null;
            try
            {
                if (Lbl_PayrollStatus.Exists(5))
                {
                    status = Lbl_PayrollStatus.Text;
                }
                else
                {
                    test.Fail("Unable to find Payroll run status");
                    GenericMethods.CaptureScreenshot();
                }
            }
            catch (Exception ex)
            {
                test.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                GenericMethods.CaptureScreenshot();
                EndTest();
                //throw new Exception(ex.Message);
            }
            return status;
        }

        //Verify Payroll Run
        public bool Fn_Verify_Payroll_Run_In_Payroll_Process(string processGroup, string runType, string runStatus)
        {
            bool flag = false;
            try
            {
                if (processGroup != "")
                {
                    if (DrpDwn_ProcessGrp.GetAttribute("value").ToLower().Equals(processGroup.ToLower()))
                    {
                        flag = true;
                    }
                    else
                    {
                        
                        test.Info("Required Process Group "+processGroup+" is not selected. Selected Process Group is : "+ DrpDwn_ProcessGrp.GetAttribute("value"));
                        return false;
                    }
                }

                if (runType != "")
                {
                    if (DrpDwn_RunType.GetAttribute("value").ToLower().Equals(runType.ToLower()))
                    {
                        flag = true;
                    }
                    else
                    {
                        
                        test.Info("Required Run Type "+runType+" is not selected. Selected Run Type is : "+ DrpDwn_RunType.GetAttribute("value"));
                        return false;
                    }
                }

                if (Fn_GetPayrollRunStatus().ToLower().Equals(runStatus.ToLower()))
                {
                    flag = true;
                    test.Pass("Payroll Run status verified successfully");
                }
                else
                {
                    flag = false;
                    test.Info("Failed to verify Payroll run status. Actual : " + Fn_GetPayrollRunStatus() + " and Expected : " + runStatus);
                    GenericMethods.CaptureScreenshot();
                }
            }
            catch (Exception ex)
            {
                test.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                GenericMethods.CaptureScreenshot();
                //EndTest();
                throw new Exception(ex.Message);
            }
            return flag;
        }

        //Undo Payroll calculation
        public bool Fn_Undo_Payroll_Calculation(string processGroup)
        {
            bool flag = false;
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
            try
            {
                if (!Lbl_PayrollStatus.Exists(5))
                {
                    if (DrpDwn_ProcessGrp.Exists(10))
                    {
                        DrpDwn_ProcessGrp.SelectValueFromDropDown(processGroup);
                    }
                }

                if (!Fn_GetPayrollRunStatus().Equals("New"))
                {
                    Btn_UndoCalc.Click();
                    wait.Until(drv => Lbl_PayrollStatus.Text == "New");
                    if (Fn_GetPayrollRunStatus().Equals("New"))
                    {
                        flag = true;
                        test.Pass("Undo Payroll calculation successful");
                    }
                    else
                    {
                        test.Fail("Undo payroll calculation failed");
                        GenericMethods.CaptureScreenshot();
                    }
                }
                else
                {
                    flag = true;
                    test.Pass("No calculated payroll found");
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

        //Get payroll period from screen
        public Dictionary<string, string> Fn_Get_Payroll_Period_From_PayrollProcess(string processGroup, string runType)
        {
            Dictionary<string, string> payPeriod = new Dictionary<string, string>();
            try
            {
                if (DrpDwn_ProcessGrp.Exists(10))
                {
                    DrpDwn_ProcessGrp.SelectValueFromDropDown(processGroup);
                }

                if (runType != "" && DrpDwn_RunType.Exists(10))
                {
                    Thread.Sleep(3000);
                    DrpDwn_RunType.SelectValueFromDropDown(runType);
                }

                if (Lbl_PeriodStartDate.Exists(10) && Lbl_PeriodEndDate.Exists(10))
                {
                    payPeriod.Add("StartDate", Lbl_PeriodStartDate.Text);
                    payPeriod.Add("EndDate", Lbl_PeriodEndDate.Text);
                }


            }
            catch (Exception ex)
            {
                test.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                GenericMethods.CaptureScreenshot();
                EndTest();
                //throw new Exception(ex.Message);
            }
            return payPeriod;
        }

        //Post payroll
        public bool Fn_Post_Calculated_Payroll_In_Payroll_Process()
        {
            bool flag = false;
            try
            {
                if (Btn_Post.Exists(5))
                {
                    Btn_Post.Click();
                    Thread.Sleep(2000);
                    driver.SwitchTo().Alert().Accept();
                    Thread.Sleep(10000);
                    if (!Lbl_PayrollStatus.Exists(5))
                    {
                        test.Pass("Payroll posted successfully");
                        flag = true;
                    }
                    else
                    {
                        test.Fail("Failed to post Payroll");
                        GenericMethods.CaptureScreenshot();
                    }
                }

            }
            catch (Exception ex)
            {
                test.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                GenericMethods.CaptureScreenshot();
                EndTest();
                //throw new Exception(ex.Message);
            }
            return flag;
        }

        //verify Reports in Payroll process page
        public bool Fn_Verify_Reports_In_Payroll_ProcessTable(string reportType, string reportName, string reportFormat, [Optional]string downloadFileName)
        {
            bool flag = false;
            IWebElement parent;
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            try
            {
                if (Tab_Reports.Exists(5) && Tab_Reports.Enabled)
                {
                    Tab_Reports.Click();
                    #region Select Submenu Standard or Custom
                    if (reportType.ToLower().Equals("standard"))
                    {
                        if (SubMenu_Standard.Exists(5)) SubMenu_Standard.Click();

                        Thread.Sleep(2000);
                        parent = SubMenu_Standard.FindElement(By.XPath("./parent::*/parent::li"));
                    }
                    else
                    {
                        Thread.Sleep(2000);
                        if (SubMenu_Custom.Exists(5)) SubMenu_Custom.Click();
                        Thread.Sleep(2000);
                        parent = SubMenu_Custom.FindElement(By.XPath("./parent::*/parent::li"));
                    }
                    #endregion

                    var reportListUI = parent.FindElements(By.XPath(".//span"));
                    int counter = 0;
                    foreach (var report in reportListUI)
                    {
                        counter++;
                        if (report.Text.ToLower().Equals(reportName.ToLower()))
                        {
                            report.Click();
                            Thread.Sleep(3000);
                            #region Download report
                            if (reportFormat != null)
                            {
                                if(reportFormat=="Excel")
                                {
                                    if (downloadFileName != null)
                                    {
                                        GenericMethods.DeleteFilesFromDirectory(downloadsFolder, downloadFileName + "*.xls*");
                                    }
                                    else
                                    {
                                        GenericMethods.DeleteFilesFromDirectory(downloadsFolder, report.Text + ".xlsx");
                                    }
                                }
                                
                                Thread.Sleep(2000);
                                report.FindElement(By.XPath("./parent::*/parent::li//span[text()='" + reportFormat + "']")).Click();
                                Thread.Sleep(2000);
                            }
                            else
                            {
                                report.FindElement(By.XPath("./parent::*/parent::li//span[text()='PDF']")).Click();
                                Thread.Sleep(2000);
                            }
                            #endregion

                            #region Verify Report downloaded
                            if (reportFormat.Equals("Excel"))
                            {
                                if (Pages.Home.Fn_Save_And_Verify_Excel(downloadFileName))
                                {
                                    flag = true;
                                }
                                else
                                {
                                    GenericMethods.CaptureScreenshot();
                                }
                            }
                            else
                            {
                                if (Win_PDFReportLoad.Exists(5))
                                {
                                    if (wait.Until(driver => Win_PDFReport.Exists(30)))
                                    {
                                        flag = true;
                                    }
                                }

                                if (Win_PDFReport.Exists(10))
                                {
                                    driver.SwitchTo().Frame(Frame_PDFReport);
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
                            #endregion
                            break;
                        }
                        if(counter==reportListUI.Count)
                        {
                            test.Fail("Unable to find report : " + report);
                            flag = false;
                        }

                    }


                }
                else
                {
                    GenericMethods.CaptureScreenshot();
                    flag = false;
                }
            }
            catch (Exception ex)
            {
                test.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                GenericMethods.CaptureScreenshot();
                EndTest();
                //throw new Exception(ex.Message);
            }
            return flag;
        }

        //Verify list of reports under Payroll Payments screen
        //reportNames: report1;report2;....
        public bool Fn_Verify_ReportsDisplayed_In_Payroll_PaymentsTable(string reportNames)
        {
            Boolean flag = false;
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            try
            {
                Btn_Search.Click();
                if (Tbl_PaymentsPayroll.Exists(10))
                {
                    if (Tbl_PaymentsPayroll.FindElements(By.XPath(".//tbody/tr")).Count > 0)
                    {
                        Tbl_PaymentsPayroll.FindElements(By.XPath(".//tbody/tr"))[0].Click();
                    }
                    else
                    {
                        test.Fail("No records exist in Payments table");
                        GenericMethods.CaptureScreenshot();
                        return false;
                    }

                    if (Tab_Reports.Exists(5) && Tab_Reports.Enabled)
                    {
                        Tab_Reports.Click();
                        if (SubMenu_Standard.Exists(5))
                        {
                            SubMenu_Standard.Click();
                        }
                    }
                    else
                    {
                        test.Fail("Tab Reports not present or not enabled");
                        GenericMethods.CaptureScreenshot();
                        return false;
                    }
                    Thread.Sleep(2000);
                    var parent = SubMenu_Standard.FindElement(By.XPath("./parent::*/parent::li"));
                
                    var reportListUI = parent.FindElements(By.XPath(".//li[contains(@class,'rmItem')]/a/span"));
                    
                    int failCnt = 0;
                    if (reportNames != "" && reportListUI.Count > 0)
                    {
                        string[] reports = reportNames.Split(';');
                        foreach (string report in reports)
                        {
                        Repeat:
                            //counter for individual report
                            int count = 0;
                            foreach(IWebElement ele in reportListUI)
                            {
                                count++;
                                if (report.ToLower().Equals(ele.Text.ToLower()))
                                {
                                    ele.Highlight();
                                    test.Pass(report + " : Found");
                                    flag = true;
                                    break;
                                }
                            }

                            if (count == reportListUI.Count)
                            {
                                if (Btn_ScrollDownArrow.Exists(5))
                                {
                                    /*Actions action = new Actions(driver);
                                    action.MoveToElement(Btn_ScrollDownArrow).Perform();*/
                                    Btn_ScrollDownArrow.Click();
                                    Thread.Sleep(2000);
                                    reportListUI = parent.FindElements(By.XPath(".//li[contains(@class,'rmItem')]/a/span"));
                                    goto Repeat;
                                }
                                //Reset it back to top
                                while (Btn_ScrollUpArrow.Exists(5))
                                {
                                    Btn_ScrollUpArrow.Click();
                                    Thread.Sleep(2000);
                                }
                                reportListUI = parent.FindElements(By.XPath(".//li[contains(@class,'rmItem')]/a/span"));
                                failCnt++;
                                test.Fail(report + " : Not Found");
                                flag = false;
                            }
                        }
                    }
                    else
                    {
                        GenericMethods.CaptureScreenshot();
                        flag = false;
                    }

                    //if one report is not found return fail
                    if (failCnt != 0)
                    {
                        GenericMethods.CaptureScreenshot(); flag = false;
                    }
                }
                else
                {
                    test.Fail("Payments table not found");
                    GenericMethods.CaptureScreenshot();
                    flag = false;
                }

            }
            catch (Exception ex)
            {
                test.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                GenericMethods.CaptureScreenshot();
                EndTest();
                //throw new Exception(ex.Message);
            }
            if (flag) test.Pass("All reports Available in UI");
            else test.Fail("Report verification in UI failed");
            return flag;
        }

        //Verify Reports in Payroll Payments Page
        public bool Fn_Verify_Reports_In_Payroll_PaymentsTable(string processGroup, string runType, string reportType, string reportName, string reportFormat, [Optional] string downloadFileName)
        {
            bool flag = false;
            IWebElement parent;
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            try
            {

                if (Fn_Search_Select_Latest_ProcessGroup(processGroup, runType))
                {
                    if (Tab_Reports.Exists(5) && Tab_Reports.Enabled)
                    {
                        Tab_Reports.Click();
                        #region Select Submenu Standard or Custom
                        if (reportType.ToLower().Equals("standard"))
                        {
                            if (SubMenu_Standard.Exists(5)) SubMenu_Standard.Click();

                            Thread.Sleep(2000);
                            parent = SubMenu_Standard.FindElement(By.XPath("./parent::*/parent::li"));
                        }
                        else
                        {
                            Thread.Sleep(2000);
                            if (SubMenu_Custom.Exists(5)) SubMenu_Custom.Click();
                            Thread.Sleep(2000);
                            parent = SubMenu_Custom.FindElement(By.XPath("./parent::*/parent::li"));
                        }
                        #endregion

                    
                        var reportListUI = parent.FindElements(By.XPath(".//li[contains(@class,'rmItem')]/a/span"));
                        
                    Repeat:
                        int count = 0;
                        foreach (var report in reportListUI)
                        {
                            count++;

                            //For Pre Year End reports
                            if (reportName.ToLower().Contains("pre-year end") && report.Text.ToLower().Contains("pre-year end"))
                            {
                                report.Click();
                                string PYE = reportName.Split('>')[1];
                                report.FindElements(By.XPath(".//following::span[text()='" + PYE + "']")).FirstOrDefault().Click();

                                #region Download Report

                                if (reportFormat == "Excel")
                                {
                                    if (downloadFileName != null)
                                    {
                                        GenericMethods.DeleteFilesFromDirectory(downloadsFolder, downloadFileName + "*.xls*");
                                    }
                                    else
                                    {
                                        GenericMethods.DeleteFilesFromDirectory(downloadsFolder, report.Text + ".xlsx");
                                    }
                                }

                                var typeList = report.FindElements(By.XPath("./ancestor::div[@class='rmScrollWrapContainer']/following::div//span[text()='" + reportFormat + "']"));
                                if (typeList.Count == 0)
                                {
                                    typeList = report.FindElements(By.XPath("./ancestor::li[contains(@class,'rmItem')]//div//span[text()='" + reportFormat + "']"));
                                }
                                foreach (var type in typeList)
                                {
                                    if (type.Displayed && type.Size.Height > 0)
                                    {
                                        type.Click();
                                        break;
                                    }
                                }
                                Thread.Sleep(2000);
                                #endregion

                                #region Verify report downloaded
                                if (reportFormat.Equals("Excel"))
                                {
                                    Pages.Home.Fn_Save_And_Verify_Excel(downloadFileName);

                                    Thread.Sleep(5000);
                                }
                                else
                                {
                                    Pages.Home.Fn_Verify_PDF_Opened_In_Window();
                                }
                                #endregion
                                flag = true;
                                break;
                            }
                            //if (report.Text.ToLower().Equals(reportName.ToLower()) || report.Text.Contains(reportName))
                            else if (report.Text.ToLower().Equals(reportName.ToLower()))
                            {
                                report.Click();
                                Thread.Sleep(3000);

                                #region Download Report

                                if (reportFormat == "Excel")
                                {
                                    if (downloadFileName != null)
                                    {
                                        GenericMethods.DeleteFilesFromDirectory(downloadsFolder, downloadFileName + "*.xls*");
                                    }
                                    else
                                    {
                                        GenericMethods.DeleteFilesFromDirectory(downloadsFolder, report.Text + ".xlsx");
                                    }
                                }

                                var typeList = report.FindElements(By.XPath("./ancestor::div[@class='rmScrollWrapContainer']/following::div//span[text()='" + reportFormat + "']"));
                                if (typeList.Count == 0)
                                {
                                    typeList = report.FindElements(By.XPath("./ancestor::li[contains(@class,'rmItem')]//div//span[text()='" + reportFormat + "']"));
                                }
                                foreach (var type in typeList)
                                {
                                    if (type.Displayed && type.Size.Height > 0)
                                    {
                                        type.Click();
                                        break;
                                    }
                                }
                                Thread.Sleep(2000);
                                #endregion

                                #region Verify report downloaded
                                if (reportFormat.Equals("Excel"))
                                {
                                    Pages.Home.Fn_Save_And_Verify_Excel(downloadFileName);

                                    Thread.Sleep(5000);
                                }
                                else
                                {
                                    Pages.Home.Fn_Verify_PDF_Opened_In_Window();
                                }
                                #endregion
                                flag = true;
                                break;
                            }

                            if (count == reportListUI.Count)
                            {
                                if (Btn_ScrollDownArrow.Exists(5))
                                {
                                    Btn_ScrollDownArrow.Click();
                                    Thread.Sleep(2000);
                                    reportListUI = parent.FindElements(By.XPath(".//li[contains(@class,'rmItem')]/a/span"));
                                    goto Repeat;
                                }
                                //Reset it back to top
                                while (Btn_ScrollUpArrow.Exists(5))
                                {
                                    Btn_ScrollUpArrow.Click();
                                    Thread.Sleep(2000);
                                }
                                reportListUI = parent.FindElements(By.XPath(".//li[contains(@class,'rmItem')]/a/span"));
                                test.Fail(report + " : Not Found");
                                flag = false;
                            }

                            
                        }


                    }
                    else
                    {
                        GenericMethods.CaptureScreenshot();
                        flag = false;
                    }
                }
                else
                {
                    test.Fail("Unable to select latest Payroll Process Group");
                    flag = false;
                }
            }
            catch (Exception ex)
            {
                test.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                GenericMethods.CaptureScreenshot();
                //EndTest();
                throw new Exception(ex.Message);
            }
            return flag;
        }

        //Search and select latest processgroup 
        public bool Fn_Search_Select_Latest_ProcessGroup(string processGroup, string runType)
        {
            bool flag = false;
            try
            {
                if (processGroup != "")
                {
                    DrpDwn_ProcessGrp.SelectValueFromDropDown(processGroup);
                }
                else
                {
                    test.Fail("Process Group can not be null");
                    return false;
                }

                if (runType != "")
                {
                    DrpDwn_RunType.SelectValueFromDropDown(runType);
                }

                Btn_Search.Click();
                Thread.Sleep(5000);
                if (Tbl_PaymentsPayroll.Exists(40))
                {
                    //if(Tbl_PaymentsPayroll.VerifyRecordDisplayedInTable(processGroup, false))
                    //{
                    //    test.Pass("Latest record found and selected");
                    //    flag = true;
                    //}
                    //else
                    //{
                    //    test.Fail("No records exist in Payments table");
                    //    GenericMethods.CaptureScreenshot();
                    //    flag = false;
                    //}

                    var trList = Tbl_PaymentsPayroll.FindElements(By.XPath(".//tbody/tr"));
                    if (trList.Count > 0)
                    {
                        foreach (IWebElement tr in trList)
                        {
                            if (tr.FindElements(By.XPath(".//td[text()='" + processGroup + "']")).Count > 0)
                            {
                                tr.Click();
                                test.Pass("Latest record found and selected");
                                flag = true;
                                break;
                            }
                        }
                    }
                    else
                    {
                        test.Fail("No records exist in Payments table");
                        GenericMethods.CaptureScreenshot();
                        flag = false;
                    }
                }
                else
                {
                    test.Fail("Payments table does not exist");
                    GenericMethods.CaptureScreenshot();
                    flag = false;
                }
            }
            catch (Exception ex)
            {
                test.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                GenericMethods.CaptureScreenshot();
                //EndTest();
                throw new Exception(ex.Message);
            }


            return flag;
        }

        /// <summary>
        /// Save Mass payslip file and verify
        /// </summary>
        /// <param name="payslipPath"></param>
        /// <param name="payslipName"></param>
        /// <returns></returns>
        public bool Fn_Save_Mass_Payslip_File_And_Verify(string payslipPath, string payslipName)
        {
            bool flag = false;
            try
            {
                //Delete .zip files if any
                GenericMethods.DeleteFilesFromDirectory(payslipPath, ".zip");

                if (Directory.Exists((payslipPath + payslipName).Replace(".zip", "")))
                {
                    Directory.Delete((payslipPath + payslipName).Replace(".zip", ""), true); //remove folder and sub folder
                }

                if (Tab_MassPayslipFile.Exists(10))
                {
                    Tab_MassPayslipFile.Click();
                    if (GenericMethods.SaveFileFromDialog(payslipPath, payslipName, 60))
                    {
                        if (GenericMethods.ExtractZipFile(payslipPath + payslipName, (payslipPath + payslipName).Replace(".zip", "")))
                        {
                            flag = true;
                            test.Pass("Mass Payslip file saved, extracted and verified successfully");
                        }
                        else
                        {
                            test.Fail("Failed to Extract or verify Zip file content");
                        }
                    }
                    else
                    {
                        test.Fail("Failed to Save file from Browser");
                    }
                }
            }
            catch (Exception ex)
            {
                test.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                GenericMethods.CaptureScreenshot();
                EndTest();
                //throw new Exception(ex.Message);
            }
            return flag;
        }

        /// <summary>
        /// Verify Export options in Payments
        /// </summary>
        /// <param name="optionList"></param>
        /// <returns></returns>
        public bool Fn_Verify_Export_Options_In_Payments(string optionList, [Optional] string fileName)
        {
            bool flag = true;
            try
            {
                foreach (string opt in optionList.Split(';'))
                {
                    if (Tab_Export.Exists(10))
                    {
                        Tab_Export.Click();
                        Thread.Sleep(3000);
                    }
                    else
                    {
                        test.Fail("Export tab is disabled or unavailable");
                        return false;
                    }

                    var option = driver.FindElements(By.XPath(".//ul[@class='rtbActive rtbGroup rtbLevel1']//span[text()='" + opt + "']"));
                    if (option.Count > 0)
                    {
                        if (fileName != "" || fileName != null)
                        {
                            if(!GenericMethods.DeleteFilesFromDirectory(downloadsFolder, fileName))
                            {
                                flag = false;
                            }
                        }

                        option[0].Click();
                        Thread.Sleep(3000);
                        if (opt.Equals("CSB"))
                        {
                            if (Btn_CloseX.Exists(10))
                            {
                                Btn_CloseX.Click();
                            }
                        }

                        if(fileName!="" || fileName!=null)
                        {
                            if(!GenericMethods.WaitForFileExists(downloadsFolder, fileName))
                            {
                                test.Fail("Export option: " + opt + " verification failed");
                                flag = false;
                            }
                        }

                        //Steps to verify the downloaded files
                        test.Pass("Export option: " + opt + " verified successfully");
                    }
                    else
                    {
                        test.Fail("Unable to find Export option: " + opt);
                        flag = false;
                    }
                }
            }
            catch (Exception ex)
            {
                test.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                GenericMethods.CaptureScreenshot();
                EndTest();
                //throw new Exception(ex.Message);
            }
            return flag;
        }

        /// <summary>
        /// Verifies if the payslip is displayed for the payperiod number from the latest
        /// Like, for second last pay period the payPeriodNumber will be 2
        /// </summary>
        /// <param name="payPeriodNumber"></param>
        /// <returns></returns>
        public bool Fn_Verify_Employee_Payslips_Displayed(int payPeriodNumber)
        {
            bool flag = false;
            try
            {
                if (Btn_PaySlips.Exists(10))
                {
                    Btn_PaySlips.Click();
                    if (Lbl_PaySlips.Exists(45))
                    {
                        driver.SwitchTo().Frame(Frame_EmployeeWizard);
                        if (Table_EmployeePayrollPeriod.Exists(10) && Table_EmployeePayrollPeriod.FindElements(By.XPath(".//tbody/tr")).Count > 0)
                        {
                            Table_EmployeePayrollPeriod.FindElements(By.XPath(".//tbody/tr"))[payPeriodNumber - 1].Click();
                            Thread.Sleep(5000);
                            if (PDFReportArea.Exists(180))
                            {
                                driver.SwitchTo().Frame(PDFReportArea);
                                if (driver.FindElement(By.XPath("//embed[@type='application/pdf']")).Exists(180))
                                {
                                    test.Pass("Payslip found for the selected pay period");
                                    driver.SwitchTo().DefaultContent();
                                    flag = true;
                                }
                                else
                                {
                                    test.Fail("Payslip file not displayed for selected pay period");
                                    driver.SwitchTo().DefaultContent();
                                    flag = false;
                                }
                            }
                            else
                            {
                                test.Fail("Payslip file not displayed for selected pay period");
                                flag = false;
                            }
                        }
                        Btn_CloseX.Click();
                        driver.SwitchTo().DefaultContent();
                    }
                    else
                    {
                        test.Fail("Unable to verify payslip window opened for the Employee");
                        flag = false;
                    }
                }
                else
                {
                    test.Fail("Payslips button not available or disabled for the Employee");
                    flag = false;
                }
            }
            catch (Exception ex)
            {
                test.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                GenericMethods.CaptureScreenshot();
                EndTest();
                //throw new Exception(ex.Message);
            }
            return flag;
        }

        /// <summary>
        /// Search Employee record in ROE creation with process Group and Employee Name
        /// </summary>
        /// <param name="processGroup"></param>
        /// <param name="empFName"></param>
        /// <param name="empLName"></param>
        /// <returns></returns>
        public bool Fn_Search_ROE_Creation(string processGroup, string empFName, string empLName)
        {
            bool flag = false;
            try
            {
                if (processGroup != "")
                {
                    DrpDwn_ProcessGrp.SelectValueFromDropDown(processGroup);
                }

                Btn_Search.Click();
                if (Tbl_ROECreationSearch.Exists(10))
                {
                    //int cnt = 0;
                    //var trList = Tbl_ROECreationSearch.FindElements(By.XPath(".//tbody/tr"));
                    //if (trList.Count > 0)
                    //{
                    //    foreach (IWebElement tr in trList)
                    //    {
                    //        cnt++;
                    //        tr.Click();
                    //        if (tr.FindElements(By.XPath(".//td[text()='" + empFName + "']")).Count > 0)
                    //        {
                    //            if (tr.FindElements(By.XPath(".//td[text()='" + empLName + "']")).Count > 0)
                    //            {
                    //                test.Pass("Employee record found in ROE Creation Search");
                    //                flag = true;
                    //                break;
                    //            }
                    //        }
                    //        if (trList.Count == cnt)
                    //        {
                    //            test.Fail("Failed to find Employee record in ROE Creation Search");
                    //            break;
                    //        }
                    //    }
                    //}
                    if (Tbl_ROECreationSearch.FindElements(By.XPath(".//tbody/tr//td[text()='" + empFName + "']/parent::tr//td[text()='" + empLName + "']")).Count > 0)
                    {
                        Tbl_ROECreationSearch.FindElement(By.XPath(".//tbody/tr//td[text()='" + empFName + "']/parent::tr")).Click();
                        test.Pass("Employee record found in ROE Creation Search");
                        flag = true;
                    }
                    else if (Tbl_ROECreationSearch.FindElements(By.XPath("//*[text()='No records to display.']")).Count > 0)
                    {
                        test.Fail("No records found in ROE Creation table");
                    }
                    else
                    {
                        test.Fail("No records found in ROE Creation table");
                    }
                }
                else
                {
                    test.Fail("ROE Creation search table not found");
                }
            }
            catch (Exception ex)
            {
                test.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                GenericMethods.CaptureScreenshot();
                EndTest();
                //throw new Exception(ex.Message);
            }
            return flag;
        }

        /// <summary>
        /// Export ROE created and verify the blk file
        /// </summary>
        /// <param name="empFName"></param>
        /// <param name="empLName"></param>
        /// <returns></returns>
        public bool Fn_Export_ROE_And_Verify_Record(string empFName, string empLName)
        {
            bool flag = false;
            try
            {
                //Delete .blk files if any
                GenericMethods.DeleteFilesFromDirectory(downloadsFolder, "*.blk");

                if (Tab_ExportROE.Exists(10))
                {
                    Tab_ExportROE.Click();
                    //Wait for file download

                    if (GenericMethods.WaitForFileExists(downloadsFolder, "*.blk", 30))
                    {
                        string FileText = File.ReadAllText(Directory.EnumerateFiles(downloadsFolder, "*.blk").First());
                        if (FileText.Contains("<FN>" + empFName + "</FN>") && FileText.Contains("<LN>" + empLName + "</LN>"))
                        {
                            test.Pass("ROE Exported and verified Employee : " + empFName + " " + empLName);
                            flag = true;
                        }
                    }
                    else
                    {
                        test.Fail("Export ROE file not found/downloaded");
                    }

                    Thread.Sleep(2000);
                }
                else
                {
                    test.Fail("Export ROE file not found/downloaded");
                }
                return true;
            }
            catch (Exception ex)
            {
                test.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                GenericMethods.CaptureScreenshot();
                EndTest();
                //throw new Exception(ex.Message);
            }
            return flag;
        }

        #endregion
    }
}
