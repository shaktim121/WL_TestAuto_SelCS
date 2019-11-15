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
    public class AdminPage : AutomationCore
    {
        private IWebDriver driver;

        #region Admin Page Object Collection
        [FindsBy(How = How.XPath, Using = "*//div[@id='ctl00_MainMenu']//span[text()='Admin']")]
        private IWebElement Menu_Admin { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='ctl00_MainMenu']/ul/li[5]/div")]
        private IWebElement Menu_SlideAdmin { get; set; }

        [FindsBy(How = How.XPath, Using = "*//input[@type=\"button\" and @value=\"Search\"]")]
        private IWebElement Btn_Search { get; set; }

        [FindsBy(How = How.XPath, Using = "*//a[text()='Code Maintenance']")]
        private IWebElement Link_CodeMaint { get; set; }

        [FindsBy(How = How.XPath, Using = "*//a[text()='Code System']")]
        private IWebElement Link_CodeSystem { get; set; }

        [FindsBy(How = How.XPath, Using = "*//a[text()='Mandatory Field Editor']")]
        private IWebElement Link_FieldEditor { get; set; }

        [FindsBy(How = How.XPath, Using = "*//a[text()='Language Editor']")]
        private IWebElement Link_LangEditor { get; set; }

        [FindsBy(How = How.XPath, Using = "*//a[text()='Export FTP']")]
        private IWebElement Link_ExportFTP { get; set; }

        [FindsBy(How = How.XPath, Using = "*//a[text()='Custom Field Config']")]
        private IWebElement Link_CustomFieldConfig { get; set; }

        [FindsBy(How = How.XPath, Using = "*//input[contains(@id,'CodeTableMaintenanceControl2_CodeTables_Field_Input')]")]
        private IWebElement Drpdwn_CodeTable { get; set; }

        [FindsBy(How = How.XPath, Using = "*//div[contains(@id,'CodeTableMaintenanceControl2_CodeTables_Field_DropDown')]")]
        private IWebElement List_CodeTable { get; set; }

        [FindsBy(How = How.XPath, Using = "*//table[contains(@class,'rgMasterTable') and contains(@id,\"MainContent_CodeSystemControl1_CodeSystemGrid\")]")]
        private IWebElement Tbl_CodeSystem { get; set; }
        
        [FindsBy(How = How.XPath, Using = "*//table[contains(@class,'rgMasterTable') and @id='ctl00_MainContent_FieldEditorControl1_FormGrid_ctl00']")]
        private IWebElement Tbl_FieldEditor { get; set; }

        [FindsBy(How = How.XPath, Using = "*//table[contains(@class,'rgMasterTable') and @id='ctl00_MainContent_FormSearchControl1_FormSummaryGrid_ctl00']")]
        private IWebElement Tbl_LangEditor { get; set; }
        
        [FindsBy(How = How.XPath, Using = "*//table[contains(@class,'rgMasterTable') and @id='ctl00_MainContent_ExportFtpControl1_ExportFtpGrid_ctl00']")]
        private IWebElement Tbl_ExportFTP { get; set; }

        [FindsBy(How = How.XPath, Using = "*//table[contains(@class,'rgMasterTable') and contains(@id,'MainContent_EmployeeCustomFieldConfigControl1_EmployeeCustomFieldConfigGrid')]")]
        private IWebElement Tbl_CustomFieldConfig { get; set; }


        #endregion

        #region Constructor
        public AdminPage()
        {
            driver = Browsers.GetDriver;
        }
        #endregion

        #region Admin Page reusable Methods
        //Naviate to Screens under Admin
        public bool Fn_Navigate_Through_Admin(string Option)
        {
            Boolean flag = false;
            try
            {
                if (Option.ToLower().Equals("code maintenance"))
                {
                    GenericMethods.SelectValueFromSlideDropDown(Menu_Admin, "Admin", Menu_SlideAdmin, Option);
                    if (Link_CodeMaint.Exists(30))
                    {
                        Link_CodeMaint.Highlight();
                        test.Pass("Verified 'Admin -> Code Maintenance' on page");
                        flag = true;
                    }
                    else
                    {
                        test.Fail("Failed to verify 'Admin -> Code Maintenance' on page");
                        flag = false;
                    }
                }

                if (Option.ToLower().Equals("code system"))
                {
                    GenericMethods.SelectValueFromSlideDropDown(Menu_Admin, "Admin", Menu_SlideAdmin, Option);
                    if (Link_CodeSystem.Exists(30))
                    {
                        Link_CodeSystem.Highlight();
                        test.Pass("Verified 'Admin -> Code System' on page");
                        flag = true;
                    }
                    else
                    {
                        test.Fail("Failed to verify 'Admin -> Code System' on page");
                        flag = false;
                    }
                }

                if (Option.ToLower().Equals("mandatory field editor"))
                {
                    GenericMethods.SelectValueFromSlideDropDown(Menu_Admin, "Admin", Menu_SlideAdmin, Option);
                    if (Link_FieldEditor.Exists(30))
                    {
                        Link_FieldEditor.Highlight();
                        test.Pass("Verified 'Admin -> Mandatory Field Editor' on page");
                        flag = true;
                    }
                    else
                    {
                        test.Fail("Failed to verify 'Admin -> Mandatory Field Editor' on page");
                        flag = false;
                    }
                }

                if (Option.ToLower().Equals("language editor"))
                {
                    GenericMethods.SelectValueFromSlideDropDown(Menu_Admin, "Admin", Menu_SlideAdmin, Option);
                    if (Link_LangEditor.Exists(30))
                    {
                        Link_LangEditor.Highlight();
                        test.Pass("Verified 'Admin -> Language Editor' on page");
                        flag = true;
                    }
                    else
                    {
                        test.Fail("Failed to verify 'Admin -> Language Editor' on page");
                        flag = false;
                    }
                }

                if (Option.ToLower().Equals("export ftp"))
                {
                    GenericMethods.SelectValueFromSlideDropDown(Menu_Admin, "Admin", Menu_SlideAdmin, Option);
                    if (Link_ExportFTP.Exists(30))
                    {
                        Link_ExportFTP.Highlight();
                        test.Pass("Verified 'Admin -> Export FTP' on page");
                        flag = true;
                    }
                    else
                    {
                        test.Fail("Failed to verify 'Admin -> Export FTP' on page");
                        flag = false;
                    }
                }

                if (Option.ToLower().Equals("custom field config"))
                {
                    GenericMethods.SelectValueFromSlideDropDown(Menu_Admin, "Admin", Menu_SlideAdmin, Option);
                    if (Link_CustomFieldConfig.Exists(30))
                    {
                        Link_CustomFieldConfig.Highlight();
                        test.Pass("Verified 'Admin -> Custom Field Config' on page");
                        flag = true;
                    }
                    else
                    {
                        test.Fail("Failed to verify 'Admin -> Custom Field Config' on page");
                        flag = false;
                    }
                }

                if (Option.Split('>')[0].ToLower().Equals("security"))
                {
                    //Code for Security - FORMAT: 'Security>Sub Menu'
                    GenericMethods.SelectValueFromSlideDropDown(Menu_Admin, "Admin", Menu_SlideAdmin, Option.Split('>')[0], Option.Split('>')[1]);
                    IWebElement Link_subReport = driver.FindElement(By.XPath("*//a[text()='" + Option.Split('>')[1] + "']"));

                    if (Link_subReport.Exists(30))
                    {
                        Link_subReport.Highlight();
                        test.Pass("Verified 'Admin -> Security -> " + Option.Split('>')[1] + "' on page");
                        flag = true;
                    }
                    else
                    {
                        test.Fail("Failed to verify 'Admin -> Security -> " + Option.Split('>')[1] + "' on page");
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
            if (flag) test.Pass("Navigated to " + Option + " Screen under Admin");
            else test.Fail("Failed to navigate to " + Option + "Screen under Admin");

            return flag;
        }

        /// <summary>
        /// Verify fields under Admin screens
        /// </summary>
        /// <param name="textFields"></param>
        /// <param name="buttons"></param>
        /// <param name="drpDowns"></param>
        /// <param name="checkBoxes"></param>
        /// <param name="toolBars"></param>
        /// <param name="tableColumns"></param>
        /// <returns></returns>
        public bool Fn_Verify_Fields_In_Admin_Screens(string textFields, string buttons, string drpDowns, string checkBoxes, string toolBars, string tableColumns, string labelFields)
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
                        IReadOnlyCollection<IWebElement> ele1 = driver.FindElements(By.XPath("*//th[text()=\"" + col + "\"]"));
                        if (ele.Count > 0)
                        {
                            ele.FirstOrDefault().Highlight();
                            test.Pass("Column name: " + col + " found");
                            flag = true;
                        }
                        else if (ele1.Count > 0)
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

        //Verify code types exist in the list
        public bool Fn_Verify_CodeTypes_Dropdown_In_CodeMaint(int num)
        {
            Boolean flag = false;
            try
            {
                if(Drpdwn_CodeTable.Exists(10))
                {
                    Drpdwn_CodeTable.Highlight();
                    Drpdwn_CodeTable.Click();

                    Thread.Sleep(1000);

                    if(List_CodeTable.Exists(10))
                    {
                        IReadOnlyList<IWebElement> listTags = List_CodeTable.FindElements(By.XPath(".//ul/li"));
                        if(listTags.Count >= num)
                        {
                            test.Pass("Code Table have Code types listed");
                        }
                        else
                        {
                            test.Fail("Code table have less than "+num+" or no value in it");
                        }
                        Drpdwn_CodeTable.Click();
                    }
                }
                else
                {
                    test.Fail("Code Table drop down does not exist");
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

        //Verify records displayed in Code System table
        public bool Fn_Verify_Record_Displayed_In_CodeSystemTable()
        {
            Boolean flag = false;
            try
            {
                if (Tbl_CodeSystem.Exists(10))
                {

                    if (Tbl_CodeSystem.VerifyRecordDisplayedInTable(false))
                    {
                        test.Pass("Code list displayed in table Code System");
                        flag = true;
                    }
                    else
                    {
                        test.Fail("No records present in table Code System");
                        flag = false;
                    }
                }
                else
                {
                    test.Fail("Unable to find table Code System");
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

        //Verify records displayed in Mandatory Field Editor table
        public bool Fn_Verify_Record_Displayed_In_FieldEditorTable()
        {
            Boolean flag = false;
            try
            {
                if (Tbl_FieldEditor.Exists(10))
                {

                    if (Tbl_FieldEditor.VerifyRecordDisplayedInTable(false))
                    {
                        test.Pass("Formname list displayed in table Mandatory Field Editor");
                        flag = true;
                    }
                    else
                    {
                        test.Fail("No records present in table Mandatory Field Editor");
                        flag = false;
                    }
                }
                else
                {
                    test.Fail("Unable to find table Mandatory Field Editor");
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

        //Verify records displayed in Language Editor table
        public bool Fn_Verify_Record_Displayed_In_LanguageEditorTable()
        {
            Boolean flag = false;
            try
            {
                Btn_Search.Click();
                Thread.Sleep(5000);

                if (Tbl_LangEditor.Exists(10))
                {

                    if (Tbl_LangEditor.VerifyRecordDisplayedInTable(false))
                    {
                        test.Pass("Formname list displayed in table Language Editor");
                        flag = true;
                    }
                    else
                    {
                        test.Fail("No records present in table Language Editor");
                        flag = false;
                    }
                }
                else
                {
                    test.Fail("Unable to find table Language Editor");
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

        //Verify records displayed in Export FTP table
        public bool Fn_Verify_Record_Displayed_In_ExportFTPTable()
        {
            Boolean flag = false;
            try
            {
                if (Tbl_ExportFTP.Exists(10))
                {

                    if (Tbl_ExportFTP.VerifyRecordDisplayedInTable(false))
                    {
                        test.Pass("FTP Type list displayed in table Export FTP");
                        flag = true;
                    }
                    else
                    {
                        test.Fail("No records present in table Export FTP");
                        flag = false;
                    }
                }
                else
                {
                    test.Fail("Unable to find table Export FTP");
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

        //Verify records displayed in Custom Field Config table
        public bool Fn_Verify_Record_Displayed_In_CustomFieldConfigTable()
        {
            Boolean flag = false;
            try
            {
                if (Tbl_CustomFieldConfig.Exists(10))
                {

                    if (Tbl_CustomFieldConfig.VerifyRecordDisplayedInTable(false))
                    {
                        test.Pass("Custom Field list displayed in table Custom Field Config");
                        flag = true;
                    }
                    else
                    {
                        test.Fail("No records present in table Custom Field Config");
                        flag = false;
                    }
                }
                else
                {
                    test.Fail("Unable to find table Custom Field Config");
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
