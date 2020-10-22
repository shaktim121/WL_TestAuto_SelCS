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
    public class SetupPage : AutomationCore
    {
        private IWebDriver driver;

        #region Setup Page Object Collection
        [FindsBy(How = How.XPath, Using = "*//div[@id='ctl00_MainMenu']//span[text()='Setup']")]
        private IWebElement Menu_Setup { get; set; }
        
        [FindsBy(How = How.XPath, Using = "//*[@id='ctl00_MainMenu']/ul/li[4]/div")]
        private IWebElement Menu_SlideSetup { get; set; }

        [FindsBy(How = How.XPath, Using = "*//a[text()='Policy']")]
        private IWebElement Link_Policy { get; set; }

        [FindsBy(How = How.XPath, Using = "*//a[text()='Entitlement']")]
        private IWebElement Link_Entitlement { get; set; }

        [FindsBy(How = How.XPath, Using = "*//a[text()='Vendor']")]
        private IWebElement Link_Vendor { get; set; }

        [FindsBy(How = How.XPath, Using = "*//a[text()='Payroll Processing Group']")]
        private IWebElement Link_PayProcessGroup { get; set; }

        [FindsBy(How = How.XPath, Using = "*//a[text()='Organizational Unit' or text()='Organization Unit']")]
        private IWebElement Link_OrgUnitLevel { get; set; }

        [FindsBy(How = How.XPath, Using = "*//a[text()='Wizard Template']")]
        private IWebElement Link_WizardTemplate { get; set; }

        [FindsBy(How = How.XPath, Using = "*//input[@type=\"button\" and @value=\"Search\"]")]
        private IWebElement Btn_Search { get; set; }

        [FindsBy(How = How.XPath, Using = "*//input[@type=\"submit\" and @value=\"Refresh\"]")]
        private IWebElement Btn_Refresh { get; set; }

        [FindsBy(How = How.XPath, Using = "*//table[contains(@class,'rgMasterTable') and @id='ctl00_MainContent_PolicySearchControl1_PolicySummaryGrid_ctl00']")]
        private IWebElement Tbl_Policy { get; set; }

        [FindsBy(How = How.XPath, Using = "*//table[contains(@class,'rgMasterTable') and @id='ctl00_MainContent_EntitlementSearchControl1_EntitlementSummaryGrid_ctl00']")]
        private IWebElement Tbl_Entitlement { get; set; }

        [FindsBy(How = How.XPath, Using = "*//table[@class=\"rgMasterTable\" and contains(@id,\"MainContent_VendorControl1_VendorInfoGrid\")]")]
        private IWebElement Tbl_Vendor { get; set; }

        [FindsBy(How = How.XPath, Using = "*//table[@class=\"rgMasterTable\" and contains(@id,\"MainContent_PayrollProcessingGroupControl1_PayrollProcessGroupGrid\")]")]
        private IWebElement Tbl_PayProcessGroup { get; set; }

        [FindsBy(How = How.XPath, Using = "*//table[contains(@class,'rgMasterTable') and contains(@id,\"MainContent_OrganizationalUnitLevelControl_OrganizationalUnitLevelGrid\")]")]
        private IWebElement Tbl_OrgUnitLevel { get; set; }

        [FindsBy(How = How.XPath, Using = "*//table[contains(@class,'rgMasterTable') and @id='ctl00_MainContent_WizardTemplateControl1_TemplateSummaryGrid_ctl00']")]
        private IWebElement Tbl_WizardTemp { get; set; }

        [FindsBy(How = How.XPath, Using = "*//table[contains(@class,'rgMasterTable') and contains(@id,\"MainContent_PaycodeAssociationControl1_PaycodeAssociationGrid\")]")]
        private IWebElement Tbl_PaycodeAssc { get; set; }

        [FindsBy(How = How.XPath, Using = "*//table[contains(@class,'rgMasterTable') and contains(@id,\"MainContent_PaycodeAssociationControl1_PaycodeAssociationTypeGrid\")]")]
        private IWebElement Tbl_PaycodeAsscType { get; set; }

        [FindsBy(How = How.XPath, Using = "*//table[contains(@class,'rgMasterTable') and contains(@id,\"MainContent_PaycodeMaintenanceControl1_PaycodeMaintenanceGrid\")]")]
        private IWebElement Tbl_PaycodeMaint { get; set; }

        [FindsBy(How = How.XPath, Using = "*//table//div[text()='No records to display.']")]
        private IWebElement Tbl_NoRecord { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Add']")]
        private IWebElement Btn_AddToTable { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Country']/parent::*/parent::*//input[@type='text']")]
        private IWebElement DrpDwn_Country { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Pay Frequency']/parent::*/parent::*//input[@type='text']")]
        private IWebElement DrpDwn_PayFrequency { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Process Group']/parent::*/parent::*//input")]
        private IWebElement Txt_ProcessGroup { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='English Desc']/parent::*/parent::*//input")]
        private IWebElement Txt_EnglishDesc { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='French Desc']/parent::*/parent::*//input")]
        private IWebElement Txt_FrenchDesc { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Year']/parent::*//parent::*//span[@id='ctl00_MainContent_PayrollProcessingGroupControl1_PayrollProcessGroupGrid_ctl00_ctl02_ctl03_PeriodYear_Field']")]
        private IWebElement Lbl_Year { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Period']/parent::*/parent::*//input")]
        private IWebElement Txt_Period { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Start Date']/parent::*/parent::*//input[@class='riTextBox riEnabled']")]
        private IWebElement Txt_StartDate { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Cutoff Date']/parent::*/parent::*//input[@class='riTextBox riDisabled']")]
        private IWebElement Txt_CutoffDate { get; set; }

        [FindsBy(How = How.XPath, Using = "*//input[@type='button' and @value='Insert']")]
        private IWebElement Btn_InsertToTable { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Insert']")]
        private IWebElement Btn_Insert { get; set; }

        [FindsBy(How = How.XPath, Using = ".//span[text()='Cancel']")]
        private IWebElement Btn_Cancel { get; set; }

        [FindsBy(How = How.XPath, Using = "*//input[@type='submit' and @value='Cancel']")]
        private IWebElement Btn_CancelWindow { get; set; }

        [FindsBy(How = How.XPath, Using = "*//div[contains(@class,'RadComboBoxDropDown') and contains(@style,'display: block')]")]
        private IWebElement List_AllDrpDwn { get; set; }

        [FindsBy(How = How.XPath, Using = "*//iframe[contains(@name,'PaycodeMaintenanceWindows')]")]
        private IWebElement Frame_PaycodeMaint { get; set; }

        [FindsBy(How = How.XPath, Using = "*//a[@class='rwCloseButton' and @title='Close']")]
        private IWebElement Btn_CloseX { get; set; }


        #endregion

        #region Constructor
        public SetupPage()
        {
            driver = Browsers.GetDriver;
        }
        #endregion

        #region Setup Page reusable Methods
        //Naviate to Screens under Setup
        public bool Fn_Navigate_Through_Setup(string Option)
        {
            Boolean flag = false;
            try
            {
                if (Option.ToLower().Equals("policy"))
                {   
                    GenericMethods.SelectValueFromSlideDropDown(Menu_Setup, "Setup", Menu_SlideSetup, Option);
                    if (Link_Policy.Exists(30))
                    {
                        Link_Policy.Highlight();
                        test.Pass("Verified 'Setup -> Policy' on page");
                        flag = true;
                    }
                    else
                    {
                        test.Fail("Failed to verify 'Setup -> Policy' on page");
                        flag = false;
                    }
                }

                if (Option.ToLower().Equals("entitlement"))
                {
                    GenericMethods.SelectValueFromSlideDropDown(Menu_Setup, "Setup", Menu_SlideSetup, Option);
                    if (Link_Entitlement.Exists(30))
                    {
                        Link_Entitlement.Highlight();
                        test.Pass("Verified 'Setup -> Entitlement' on page");
                        flag = true;
                    }
                    else
                    {
                        test.Fail("Failed to verify 'Setup -> Entitlement' on page");
                        flag = false;
                    }
                }

                if (Option.ToLower().Equals("vendor"))
                {
                    GenericMethods.SelectValueFromSlideDropDown(Menu_Setup, "Setup", Menu_SlideSetup, Option);
                    if (Link_Vendor.Exists(30))
                    {
                        Link_Vendor.Highlight();
                        test.Pass("Verified 'Setup -> Vendor' on page");
                        flag = true;
                    }
                    else
                    {
                        test.Fail("Failed to verify 'Setup -> Vendor' on page");
                        flag = false;
                    }
                }

                if (Option.ToLower().Equals("payroll processing group"))
                {
                    GenericMethods.SelectValueFromSlideDropDown(Menu_Setup, "Setup", Menu_SlideSetup, Option);
                    if (Link_PayProcessGroup.Exists(30))
                    {
                        Link_PayProcessGroup.Highlight();
                        test.Pass("Verified 'Setup -> Payroll Processing Group' on page");
                        flag = true;
                    }
                    else
                    {
                        test.Fail("Failed to verify 'Setup -> Payroll Processing Group' on page");
                        flag = false;
                    }
                }

                if (Option.ToLower().Equals("organizational unit") || Option.ToLower().Equals("organization unit"))
                {
                    GenericMethods.SelectValueFromSlideDropDown(Menu_Setup, "Setup", Menu_SlideSetup, Option);
                    if (Link_OrgUnitLevel.Exists(30))
                    {
                        Link_OrgUnitLevel.Highlight();
                        test.Pass("Verified 'Setup -> Organizational Unit' on page");
                        flag = true;
                    }
                    else
                    {
                        test.Fail("Failed to verify 'Setup -> Organizational Unit' on page");
                        flag = false;
                    }
                }

                if (Option.ToLower().Equals("wizard template"))
                {
                    GenericMethods.SelectValueFromSlideDropDown(Menu_Setup, "Setup", Menu_SlideSetup, Option);
                    if (Link_WizardTemplate.Exists(30))
                    {
                        Link_WizardTemplate.Highlight();
                        test.Pass("Verified 'Setup -> Wizard Template' on page");
                        flag = true;
                    }
                    else
                    {
                        test.Fail("Failed to verify 'Setup -> Wizard Template' on page");
                        flag = false;
                    }
                }

                if (Option.Split('>')[0].ToLower().Equals("paycode"))
                {
                    //Code for Paycode - FORMAT: 'Paycode>Paycode Name'
                    GenericMethods.SelectValueFromSlideDropDown(Menu_Setup, "Setup", Menu_SlideSetup, Option.Split('>')[0], Option.Split('>')[1]);
                    IWebElement Link_subReport = driver.FindElement(By.XPath("*//a[text()='" + Option.Split('>')[1] + "']"));

                    if (Link_subReport.Exists(30))
                    {
                        Link_subReport.Highlight();
                        test.Pass("Verified 'Setup -> Paycode -> " + Option.Split('>')[1] + "' on page");
                        flag = true;
                    }
                    else
                    {
                        test.Fail("Failed to verify 'Setup -> Paycode -> " + Option.Split('>')[1] + "' on page");
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
            if (flag) test.Pass("Navigated to " + Option + " Screen under Setup");
            else test.Fail("Failed to navigate to "+ Option + "Screen under Setup");

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
        public bool Fn_Verify_Fields_In_Setup_Screens(string textFields, string buttons, string drpDowns, string checkBoxes, string toolBars, string tableColumns, string labelFields)
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
                        IReadOnlyCollection<IWebElement> ele = driver.FindElements(By.XPath("*//input[@type=\"text\"]/parent::*/parent::*//span[text()=\"" + t + "\"]"));
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
                            ele.FirstOrDefault().Highlight();
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

                //Code to verify checkboxes
                if (checkBoxes.Length > 0)
                {
                    string[] chk = checkBoxes.Split(';');
                    foreach (string ch in chk)
                    {
                        IReadOnlyCollection<IWebElement> ele = driver.FindElements(By.XPath("*//input[@type=\"checkbox\"]/parent::*/span[text()=\"" + ch + "\"]"));
                        IReadOnlyCollection<IWebElement> chkAll = driver.FindElements(By.XPath("*//input[@type='checkbox' and contains(@id,'" + ch + "')]"));
                        if (ele.Count > 0)
                        {
                            ele.FirstOrDefault().Highlight();
                            test.Pass("Checkbox: " + ch + " found");
                            flag = true;
                        }
                        else if (chkAll.Count > 0)
                        {
                            chkAll.FirstOrDefault().Highlight();
                            test.Pass("Checkbox: " + ch + " found");
                            flag = true;
                        }
                        else
                        {
                            test.Fail("Failed to find Checkbox: " + ch);
                            flag = false;
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
                        }

                        if (!flag)
                        {
                            test.Fail("Failed to find Column name: " + col);
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
            return flag;
        }

        //Verify records displayed in Policy table
        public bool Fn_Verify_Record_Displayed_In_PolicyTable()
        {
            Boolean flag = false;
            try
            {
                Btn_Search.Click();

                if(Tbl_Policy.Exists(10))
                {
                    if (Tbl_Policy.VerifyRecordDisplayedInTable(false))
                    {
                        flag = true;
                        test.Pass("Policy records exists in table");
                    }
                    else
                    {
                        flag = false;
                        test.Fail("No policy records displayed");
                    }
                }
                else
                {
                    test.Fail("Unable to find table Policy");
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

        //Verify records displayed in Entitlement table
        public bool Fn_Verify_Record_Displayed_In_EntitlementTable()
        {
            Boolean flag = false ;
            try
            {
                Btn_Search.Click();

                if(Tbl_Entitlement.Exists(10))
                {
                    if (Tbl_Entitlement.VerifyRecordDisplayedInTable(false))
                    {
                        flag = true;
                        test.Pass("Entitlement records exists in table");
                    }
                    else
                    {
                        flag = false;
                        test.Fail("No Entitlement records displayed");
                    }
                }
                else
                {
                    test.Fail("Unable to find table Entitlement");
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

        //Verify records displayed in Vendor table
        public bool Fn_Verify_Record_Displayed_In_VendorTable()
        {
            Boolean flag = false ;
            try
            {
                if (Tbl_Vendor.Exists(10))
                {
                    
                    if(Tbl_Vendor.VerifyRecordDisplayedInTable(false))
                    {
                        test.Pass("Vendor list displayed in table Vendor");
                        flag = true;
                    }
                    else
                    {
                        test.Fail("No records present in table Vendor");
                        flag = false;
                    }
                }
                else
                {
                    test.Fail("Unable to find table Vendor");
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

        //Verify records displayed in Payroll Processing Group table
        public bool Fn_Verify_Record_Displayed_In_PayProcessGroupTable()
        {
            Boolean flag = false;
            try
            {
                if (Tbl_PayProcessGroup.Exists(10))
                {

                    if (Tbl_PayProcessGroup.VerifyRecordDisplayedInTable(false))
                    {
                        test.Pass("Process Group list displayed in table Payroll Processing Group");
                        flag = true;
                    }
                    else
                    {
                        test.Fail("No records present in table Payroll Processing Group");
                        flag = false;
                    }
                }
                else
                {
                    test.Fail("Unable to find table Payroll Processing Group");
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

        //Verify records in Organizational Unit Level Table
        public bool Fn_Verify_Records_Of_Single_Column_In_OUL(string columnName, string colValues)
        {
            Boolean flag = false;
            try
            {
                if(Pages.Home.Fn_VerifyRecordsInSingleColumn(Tbl_OrgUnitLevel, columnName, colValues))
                {
                    test.Pass("Successfully verified records in Org Unit Level Table");
                    flag = true;
                }
                else
                {
                    test.Fail("Failed to verify records in Org Unit Level Table");
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

        //Verify records displayed in Wizard Template table
        public bool Fn_Verify_Record_Displayed_In_WizardTable()
        {
            Boolean flag = false;
            try
            {
                Btn_Search.Click();

                if (Tbl_WizardTemp.Exists(10))
                {
                    if (Tbl_WizardTemp.VerifyRecordDisplayedInTable(false))
                    {
                        flag = true;
                        test.Pass("Wizard Template records exists in table");
                    }
                    else
                    {
                        flag = false;
                        test.Fail("No Wizard Template records displayed");
                    }
                }
                else
                {
                    test.Fail("Unable to find table Wizard Template");
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

        //Verify records displayed in Paycode>Paycode Maintenance table
        public bool Fn_Verify_Record_Displayed_In_PaycodeMaintTable()
        {
            Boolean flag = false;
            try
            {
                Btn_Refresh.Click();
                Thread.Sleep(10000);

                if (Tbl_PaycodeMaint.Exists(10))
                {
                    if (Tbl_PaycodeMaint.VerifyRecordDisplayedInTable(false))
                    {
                        flag = true;
                        test.Pass("Paycode records exists in table");
                    }
                    else
                    {
                        flag = false;
                        test.Fail("No Paycode records displayed");
                    }
                }
                else
                {
                    test.Fail("Unable to find table Paycode Maintenance");
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

        //Verify records displayed in Paycode>Paycode Maintenance table
        public bool Fn_Verify_Record_Displayed_In_PaycodeAssociationTable()
        {
            Boolean flag = false;
            try
            {
                if (Tbl_PaycodeAsscType.Exists(10))
                {
                    if (Tbl_PaycodeAsscType.VerifyRecordDisplayedInTable(false))
                    {
                        flag = true;
                        test.Pass("Paycode Type records exists in table");
                    }
                    else
                    {
                        flag = false;
                        test.Fail("No Paycode Type records displayed");
                    }
                }
                else
                {
                    test.Fail("Unable to find table Paycode Association Type");
                    flag = false;
                }

                Thread.Sleep(5000);

                if (Tbl_PaycodeAssc.Exists(10))
                {
                    if (Tbl_PaycodeAssc.VerifyRecordDisplayedInTable(false))
                    {
                        flag = true;
                        test.Pass("Paycode records exists in table");
                    }
                    else
                    {
                        flag = false;
                        test.Fail("No Paycode records displayed");
                    }
                }
                else
                {
                    test.Fail("Unable to find table Paycode Association");
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

        //Create payroll processing group in Setup screen
        public bool Fn_Create_Payroll_Processing_Group(string country, string payFrequency, string processGrp, string engDesc, string frenchDesc,string period, string startDate, string cutoffDate)
        {
            bool flag = false;
            try
            {
                if (Tbl_PayProcessGroup.FindElements(By.XPath("./tbody/tr//td[text()='" + processGrp + "']/following::td[text()='"+engDesc+"']")).Count > 0)
                {
                    Tbl_PayProcessGroup.FindElements(By.XPath("./tbody/tr//td[text()='" + processGrp + "']"))[0].Highlight();
                    return true;
                }

                Btn_AddToTable.Click();
                DrpDwn_Country.SelectValueFromDropDown(country);
                Thread.Sleep(2000);
                DrpDwn_PayFrequency.SelectValueFromDropDown(payFrequency);
                Thread.Sleep(2000);
                Txt_ProcessGroup.SetText(processGrp);
                Txt_EnglishDesc.SetText(engDesc);
                Txt_FrenchDesc.SetText(frenchDesc);
                if (Lbl_Year.Text == DateTime.Now.Year.ToString())
                {
                    flag = true;
                }
                Txt_Period.SetText(period);
                Txt_StartDate.SetText(startDate);
                Txt_CutoffDate.Click();
                Thread.Sleep(5000);
                if (Txt_CutoffDate.GetAttribute("value") == cutoffDate) flag = true;
                else flag = false;

                Btn_InsertToTable.Click();
                Thread.Sleep(5000);
                if (Tbl_PayProcessGroup.FindElements(By.XPath("./tbody/tr//td[text()='" + processGrp + "']/following::td[text()='" + engDesc + "']")).Count > 0)
                {
                    Tbl_PayProcessGroup.FindElements(By.XPath("./tbody/tr//td[text()='" + processGrp + "']"))[0].Highlight();
                    flag = true;
                }
                else flag = false;

                if (flag)
                {
                    test.Pass("Payroll Process Group: " + processGrp + " created Successfully");
                }
                else
                {
                    test.Fail("Failed to create Payroll Process Group: " + processGrp);
                    GenericMethods.CaptureScreenshot();
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

        //Navigate to Add paycode screen and verify
        public bool Fn_Navigate_Verify_Add_Paycode_Screen(string paycodeType)
        {
            bool flag = false;
            try
            {
                if(Btn_AddToTable.Exists(10))
                {
                    Btn_AddToTable.Click();
                    if(driver.FindElement(By.XPath(".//span[text()='"+paycodeType+"']")).Exists())
                    {
                        driver.FindElement(By.XPath(".//span[text()='" + paycodeType + "']")).Click();
                    }
                    else
                    {
                        test.Fail(paycodeType + "Paycode not found");
                        return false;
                    }

                    if(driver.FindElement(By.XPath(".//em[contains(text(),'"+paycodeType+" Paycode')]")).Exists())
                    {
                        Thread.Sleep(2000);
                        driver.SwitchTo().Frame(Frame_PaycodeMaint);
                        if(Btn_Insert.Exists() && Btn_Cancel.Exists())
                        {
                            flag = true;
                        }
                        driver.SwitchTo().ParentFrame();
                        Btn_CloseX.Click();
                    }
                    else
                    {
                        test.Fail("Add paycode window failed to open");
                        return false;
                    }


                }
                else
                {
                    test.Fail(paycodeType + "Paycode not found");
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
