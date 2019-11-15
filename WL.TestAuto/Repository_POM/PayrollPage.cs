using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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

        [FindsBy(How = How.XPath, Using = "*//span[contains(@class,'rmExpandRight') and text()='Standard']")]
        private IWebElement SubMenu_Standard { get; set; }

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
                if (Option.ToLower().Equals("employee"))
                {

                    //Code to verify Employee screen displayed
                    GenericMethods.SelectValueFromSlideDropDown(Menu_Payroll, "Payroll", Menu_SlidePayroll, Option);
                    if (Link_Employee.Exists(30))
                    {
                        Link_Employee.Highlight();
                        test.Pass("Verified 'Payroll -> Employee' on page");
                        flag = true;
                    }
                    else
                    {
                        test.Fail("Failed to verify 'Payroll -> Employee' on page");
                        flag = false;
                    }
                }

                if (Option.ToLower().Equals("batch"))
                {
                    //code for batch
                    GenericMethods.SelectValueFromSlideDropDown(Menu_Payroll, "Payroll", Menu_SlidePayroll, Option);
                    if (Link_Batch.Exists(30))
                    {
                        Link_Batch.Highlight();
                        test.Pass("Verified 'Payroll -> Batch' on page");
                        flag = true;
                    }
                    else
                    {
                        test.Fail("Failed to verify 'Payroll -> Batch' on page");
                        flag = false;
                    }
                }

                if (Option.ToLower().Equals("payroll transaction"))
                {
                    //code for payroll process
                    GenericMethods.SelectValueFromSlideDropDown(Menu_Payroll, "Payroll", Menu_SlidePayroll, Option);
                    if (Link_PayTransact.Exists(30))
                    {
                        Link_PayTransact.Highlight();
                        test.Pass("Verified 'Payroll -> Payroll Transaction' on page");
                        test.Pass("Navigated to Payroll Transaction Screen under Payroll");
                        flag = true;
                    }
                    else
                    {
                        test.Fail("Failed to verify 'Payroll -> Payroll Transaction' on page");
                        test.Fail("Failed to navigate to Payroll Transaction Screen under Payroll");
                        flag = false;
                    }
                   
                }

                if (Option.ToLower().Equals("payroll process"))
                {
                    //code for payroll process
                    GenericMethods.SelectValueFromSlideDropDown(Menu_Payroll, "Payroll", Menu_SlidePayroll, Option);
                    if (Link_PayProcess.Exists(30))
                    {
                        Link_PayProcess.Highlight();
                        test.Pass("Verified 'Payroll -> Payroll Process' on page");
                        test.Pass("Navigated to Payroll Process Screen under Payroll");
                        flag = true;
                    }
                    else
                    {
                        test.Fail("Failed to verify 'Payroll -> Payroll Process' on page");
                        test.Fail("Failed to navigate to Payroll Process Screen under Payroll");
                        flag = false;
                    }
                }

                if (Option.ToLower().Equals("payments"))
                {
                    //code for payroll process
                    GenericMethods.SelectValueFromSlideDropDown(Menu_Payroll, "Payroll", Menu_SlidePayroll, Option);
                    if (Link_Payments.Exists(30))
                    {
                        Link_Payments.Highlight();
                        test.Pass("Verified 'Payroll -> Payments' on page");
                        test.Pass("Navigated to Payments Screen under Payroll");
                        flag = true;
                    }
                    else
                    {
                        test.Fail("Failed to verify 'Payroll -> Payments' on page");
                        test.Fail("Failed to navigate to Payments Screen under Payroll");
                        flag = false;
                    }
                }

                if (Option.ToLower().Equals("import"))
                {
                    //code for payroll process
                    GenericMethods.SelectValueFromSlideDropDown(Menu_Payroll, "Payroll", Menu_SlidePayroll, Option);
                    if (Link_Import.Exists(30))
                    {
                        Link_Import.Highlight();
                        test.Pass("Verified 'Payroll -> Import' on page");
                        flag = true;
                    }
                    else
                    {
                        test.Fail("Failed to verify 'Payroll -> Import' on page");
                        flag = false;
                    }
                }

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
                        flag = false;
                    }
                }
            }
            catch (Exception ex)
            {
                test.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                EndTest();
                //throw new Exception(ex.Message);
            }
            return flag;
        }

        /// <summary>
        /// Verify fields under Payroll screens
        /// </summary>
        /// <param name="textFields"></param>
        /// <param name="buttons"></param>
        /// <param name="drpDowns"></param>
        /// <param name="checkBoxes"></param>
        /// <param name="toolBars"></param>
        /// <param name="tableColumns"></param>
        /// <returns></returns>
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

                            if(ele.Count > 0)
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
                EndTest();
                //throw new Exception(ex.Message);
            }
            if(flag) test.Pass("All fields in Payroll Process screen under Payroll are verified successfully");
            else test.Fail("Failed to verify all fields in Payroll Process screen under Payroll");

            return flag;
        }

        //reportNames: report1;report2;....
        public bool Fn_Verify_Reports_In_Payroll_PaymentsTable(string reportNames)
        {
            Boolean flag = false;
            try
            {
                Btn_Search.Click();
                if (Tbl_PaymentsPayroll.Exists(10))
                {
                    if(Tbl_PaymentsPayroll.FindElements(By.XPath(".//tbody/tr")).Count > 0)
                    {
                        Tbl_PaymentsPayroll.FindElements(By.XPath(".//tbody/tr"))[0].Click();
                    }
                    
                    if(Tab_Reports.Exists(5) && Tab_Reports.Enabled)
                    {
                        Tab_Reports.Click();
                        if(SubMenu_Standard.Exists(5))
                        {
                            SubMenu_Standard.Click();
                        }
                    }
                    Thread.Sleep(2000);
                    var parent = SubMenu_Standard.FindElement(By.XPath("./parent::*/parent::li"));
                    var reportListUI = parent.FindElements(By.XPath(".//span"));
                    int failCnt = 0;
                    if (reportNames != "" && reportListUI.Count > 0)
                    {
                        string[] reports = reportNames.Split(';');
                        foreach (string report in reports)
                        {
                            //counter for individual report
                            int cnt = 0;
                            for (int r=0; r < reportListUI.Count-1; r++)
                            {
                                if(report.ToLower().Equals(reportListUI[r].Text.ToLower()))
                                {
                                    cnt++;
                                    reportListUI[r].Highlight();
                                    test.Pass(report + " : Found");
                                    flag = true;
                                    break;
                                }
                            }
                            if(cnt==0)
                            {
                                failCnt++;
                                test.Fail(report + " : Not Found");
                                flag = false;
                            }
                        }
                    }
                    else
                    {
                        flag = false;
                    }

                    //if one report is not found return fail
                    if (failCnt != 0) flag = false;
                }
                
            }
            catch (Exception ex)
            {
                test.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                EndTest();
                //throw new Exception(ex.Message);
            }
            if(flag) test.Pass("All reports Available in UI");
            else test.Fail("Report verification in UI failed");
            return flag;
        }

        public bool Fn_Verify_ImportTypes_In_Payroll_Import(string typeNames)
        {
            Boolean flag = false;
            try
            {
                string[] types = typeNames.Split(';');
                if (DrpDwn_ImportType.Exists(5))
                {
                    DrpDwn_ImportType.Click();
                    var typeList = List_ImportType.FindElements(By.TagName("li"));
                    int failCnt = 0;
                    if (typeNames!="" && typeList.Count > 0)
                    {
                        foreach(string type in types)
                        {
                            int cnt = 0;
                            for (int t = 0; t < typeList.Count; t++)
                            {
                                if(type.ToLower().Equals(typeList[t].Text.ToLower()))
                                {
                                    cnt++;
                                    typeList[t].Highlight();
                                    test.Pass(type + " found");
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
                        flag = false;
                    }

                    //if one type is not found return false
                    if (failCnt != 0) flag = false;
                }
            }
            catch (Exception ex)
            {
                test.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                EndTest();
                //throw new Exception(ex.Message);
            }

            if (flag) test.Pass("All Import Types available in UI");
            else test.Fail("Import Type verification in UI failed");
            return flag;

        }

        public bool Fn_Verify_And_Navigate_To_Reports_In_Payroll(string reportName)
        {
            Boolean flag = false;
            try
            {   
                if (reportName=="")
                {
                    test.Error("No report name specified");
                    return false;
                }

                //report names Report1>subreport1>subsubreport...
                string[] reportNav = reportName.Split('>');

                if (Menu_Payroll.Exists(5))
                {
                    Menu_Payroll.Click();
                    Thread.Sleep(3000);
                    if (SubMenu_Reports.Exists(5))
                    {
                        SubMenu_Reports.Click();
                        Thread.Sleep(3000);
                        if(reportNav[reportNav.Length-1].ToString() == "Excel")
                        {
                            //Code to export report from menu in Excel
                            foreach (string report in reportNav)
                            {
                                if(report.Equals("Excel"))
                                {
                                    if (driver.FindElements(By.XPath("*//a/span[text()='" + report + "']")).Count > 0)
                                    {
                                        driver.FindElements(By.XPath("*//a/span[text()='" + report + "']"))[0].Click();
                                        Thread.Sleep(5000);
                                        test.Pass("Exported "+ reportNav[reportNav.Length - 1] + " report: " + reportNav[reportNav.Length - 2] + "from " + reportName);
                                        flag = true;
                                    }
                                    else
                                    {
                                        test.Fail("Failed to find or export report: " + reportNav[reportNav.Length - 2] + "from " + reportName);
                                        flag = false;
                                    }
                                }
                                else
                                {
                                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
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
                                    if(report.Equals(reportNav[reportNav.Length-1].ToString()))
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
                                    test.Fail("Failed to navigate to report: "+ report + "from " + reportName);
                                    flag = false;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                test.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                EndTest();
                //throw new Exception(ex.Message);
            }
            
            return flag;
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
                    }
                }
                else
                {
                    test.Fail("Unable to find table Payroll Transaction");
                    flag = false;
                }
            }
            catch (Exception ex)
            {
                driver.SwitchTo().DefaultContent();
                test.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                EndTest();
                //throw new Exception(ex.Message);
            }

            return flag;
        }

        //Enter Employee details to verify in Field1-Value1;Field2-Value2,etc
        public bool Fn_Verify_Employee_Details_In_Payroll_Position(string empState, string arrayEmpDetails)
        {
            Boolean flag = false;
            try
            {
                if(Btn_EmpPosition.Exists(10))
                {
                    Btn_EmpPosition.Click();
                    if(Lbl_EmployeePosition.Exists(15))
                    {
                        flag = true;
                        driver.SwitchTo().Frame(Frame_EmployeePosition);

                        if(Tbl_EmployeePosition.Exists(10) && Tbl_EmployeePosition.FindElements(By.XPath("./tbody/tr/td[text()='" + empState + "']")).Count > 0)
                        {
                            Tbl_EmployeePosition.FindElement(By.XPath("./tbody/tr/td[text()='" + empState + "']")).Click();
                            Thread.Sleep(15000);
                        }

                        string[] empDetails = arrayEmpDetails.Split(';');
                        foreach (string empDetail in empDetails)
                        {
                            string field = empDetail.Split('-')[0];
                            string value = empDetail.Split('-')[1];

                            if (driver.FindElements(By.XPath("*//span[text()='"+field+"']/parent::*/following::*/span[text()='"+value+"']")).Count > 0)
                            {
                                driver.FindElements(By.XPath("*//span[text()='" + field + "']/parent::*/following::*/span[text()='" + value + "']"))[0].Highlight();
                                test.Pass("Field: "+ field + " found with value: "+value);
                                flag = true;
                            }
                            else if(driver.FindElements(By.XPath("*//span[text()='"+field+"']/parent::*/following::*/span/input[@value='"+value+"']")).Count > 0)
                            {
                                driver.FindElements(By.XPath("*//span[text()='" + field + "']/parent::*/following::*/span/input[@value='" + value + "']"))[0].Highlight();
                                test.Pass("Field: " + field + " found with value: " + value);
                                flag = true;
                            }
                            else
                            {
                                test.Fail("No Field: " + field + " with value: " + value + " is found");
                                flag = false;
                            }

                        }
                        driver.SwitchTo().DefaultContent();
                        Btn_CloseX.Click();
                    }
                    else
                    {
                        test.Fail("Employee Position window not opened");
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
                EndTest();
                //throw new Exception(ex.Message);
            }

            return flag;
        }
        #endregion
    }
}
