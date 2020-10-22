using iText.Layout.Element;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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

        [FindsBy(How = How.XPath, Using = "*//input[@type='button' and @value='Search']")]
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

        [FindsBy(How = How.XPath, Using = "*//table[contains(@class,'rgMasterTable') and contains(@id,'MainContent_YearEndControl1_YearEndGrid')]")]
        private IWebElement Tbl_YearEnd { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[contains(@class,'rmExpandDown') and text()='Reports']")]
        private IWebElement Tab_Reports { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[contains(@class,'rtbText') and text()='Mass Print']")]
        private IWebElement Tab_MassPrint { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[contains(@class,'rtbText') and text()='Summary']")]
        private IWebElement Tab_Summary { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[contains(@class,'rtbText') and text()='Mass File']")]
        private IWebElement Tab_MassFile { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[contains(@class,'rtbText') and text()='Export']")]
        private IWebElement Tab_Export { get; set; }

        [FindsBy(How = How.XPath, Using = "*//td[@class='rwWindowContent rwExternalContent rwLoading']")]
        private IWebElement Win_PDFReportLoad { get; set; }

        [FindsBy(How = How.XPath, Using = "*//td[@class='rwWindowContent rwExternalContent']")]
        private IWebElement Win_PDFReport { get; set; }

        [FindsBy(How = How.XPath, Using = "*//iframe[contains(@name,'RegisterReportWindow') or contains(@name,'PayrollProcessWindow') or contains(@name,'YearEndWindows') or contains(@name,'YearEndReportWindows')]")]
        private IWebElement Frame_PDFReport { get; set; }

        [FindsBy(How = How.XPath, Using = "*//a[@class='rwCloseButton' and @title='Close']")]
        private IWebElement Btn_CloseX { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Process Group']//following::input[@type='text'][1]")]
        public IWebElement DrpDwn_ProcessGrp { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Year']//following::input[@type='text'][1]")]
        public IWebElement DrpDwn_Year { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()='Form Type']//following::input[@type='text'][1]")]
        public IWebElement DrpDwn_FormType { get; set; }

        [FindsBy(How = How.XPath, Using = "*//table[contains(@class,'rgMasterTable') and @id='ctl00_MainContent_YearEndAdjustmentsSearchControl1_YearEndAdjustmentSearchGrid_ctl00']")]
        private IWebElement Tbl_YearEndAdjustments { get; set; }

        [FindsBy(How = How.XPath, Using = ".//span[@class='rtbText' and text()='Details']")]
        private IWebElement Tab_Details { get; set; }

        [FindsBy(How = How.XPath, Using = ".//span[@class='rtbText' and text()='Report']")]
        private IWebElement Tab_Report { get; set; }

        [FindsBy(How = How.XPath, Using = ".//span[@class='rtbText' and text()='Adjustments Audit Detail']")]
        private IWebElement Tab_AdjustmentsAuditDetail { get; set; }

        [FindsBy(How = How.XPath, Using = ".//span[@class='rtbText' and text()='Adjustments Audit']")]
        private IWebElement Tab_AdjustmentsAudit { get; set; }

        [FindsBy(How = How.XPath, Using = "*//em[text()='Year End Adjustment']")]
        private IWebElement Lbl_YearEndAdjustment { get; set; }

        [FindsBy(How = How.XPath, Using = "*//iframe[contains(@name,'YearEndWindows')]")]
        private IWebElement Frame_YEWizard { get; set; }

        [FindsBy(How = How.XPath, Using = "*//iframe[contains(@name,'YearEndExportWindows')]")]
        private IWebElement Frame_YEExportWizard { get; set; }




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

                if (Option.Split('>')[0].ToLower().Equals("year end"))
                {
                    //Code for Security - FORMAT: 'Year End>Sub Menu'
                    GenericMethods.SelectValueFromSlideDropDown(Menu_Admin, "Admin", Menu_SlideAdmin, Option.Split('>')[0], Option.Split('>')[1]);
                    IWebElement Link_subReport = driver.FindElement(By.XPath("*//a[text()='" + Option.Split('>')[1] + "']"));

                    if (Link_subReport.Exists(30))
                    {
                        Link_subReport.Highlight();
                        test.Pass("Verified 'Admin -> Year End -> " + Option.Split('>')[1] + "' on page");
                        flag = true;
                    }
                    else
                    {
                        test.Fail("Failed to verify 'Admin -> Year End -> " + Option.Split('>')[1] + "' on page");
                        flag = false;
                    }
                }
            }
            catch (Exception ex)
            {
                test.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                //EndTest();
                throw new Exception(ex.Message);
            }
            if (flag) test.Pass("Navigated to " + Option + " Screen under Admin");
            else
            {
                test.Fail("Failed to navigate to " + Option + "Screen under Admin");
                GenericMethods.CaptureScreenshot();
            }

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
                GenericMethods.CaptureScreenshot();
                EndTest();
                //throw new Exception(ex.Message);
            }
            return flag;
        }

        //Verify code types exist in the list
        public bool Fn_Verify_CodeTypes_Dropdown_In_CodeMaint(int codeTypeCnt)
        {
            Boolean flag = false;
            try
            {
                if (Drpdwn_CodeTable.Exists(10))
                {
                    Drpdwn_CodeTable.Highlight();
                    Drpdwn_CodeTable.Click();

                    Thread.Sleep(1000);

                    if (List_CodeTable.Exists(10))
                    {
                        IReadOnlyList<IWebElement> listTags = List_CodeTable.FindElements(By.XPath(".//ul/li"));
                        if (listTags.Count >= codeTypeCnt)
                        {
                            test.Pass("Code Table have Code types listed");
                            flag = true;
                        }
                        else
                        {
                            test.Fail("Code table have less than " + codeTypeCnt + " or no value in it");
                            GenericMethods.CaptureScreenshot();
                        }
                        Drpdwn_CodeTable.Click();
                    }
                }
                else
                {
                    test.Fail("Code Table drop down does not exist");
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
                        GenericMethods.CaptureScreenshot();
                        flag = false;
                    }
                }
                else
                {
                    test.Fail("Unable to find table Code System");
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
                        GenericMethods.CaptureScreenshot();
                        flag = false;
                    }
                }
                else
                {
                    test.Fail("Unable to find table Mandatory Field Editor");
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
                        GenericMethods.CaptureScreenshot();
                        flag = false;
                    }
                }
                else
                {
                    test.Fail("Unable to find table Language Editor");
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
                        GenericMethods.CaptureScreenshot();
                        flag = false;
                    }
                }
                else
                {
                    test.Fail("Unable to find table Export FTP");
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
                        GenericMethods.CaptureScreenshot();
                        flag = false;
                    }
                }
                else
                {
                    test.Fail("Unable to find table Custom Field Config");
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

        //Verify records displayed in Year End table
        public bool Fn_Verify_Record_Displayed_In_YearEndTable()
        {
            Boolean flag = false;
            try
            {
                if (Tbl_YearEnd.Exists(10))
                {

                    if (Tbl_YearEnd.VerifyRecordDisplayedInTable(false))
                    {
                        test.Pass("Year End list displayed in table Year End");
                        flag = true;
                    }
                    else
                    {
                        test.Fail("No records present in table Year End");
                        GenericMethods.CaptureScreenshot();
                    }
                }
                else
                {
                    test.Fail("Unable to find table Year End");
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

        //Verify Reports in Year End Table
        public bool Fn_Verify_Reports_In_YearEndTable(string reportName, string reportFormat, [Optional] string downloadFileName)
        {
            bool flag = false;
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            try
            {
                if (Tab_Reports.Exists(5) && Tab_Reports.Enabled)
                {
                    Tab_Reports.Click();
                    Thread.Sleep(2000);
                    var reportList = Tab_Reports.FindElements(By.XPath(".//following::ul//span[text()='" + reportName + "']"));

                    if(reportList.Count > 0)
                    {
                        reportList[0].Click();
                        Thread.Sleep(3000);

                        #region Download report
                        if (reportFormat != null)
                        {
                            if (reportFormat == "Excel")
                            {
                                if (downloadFileName != null)
                                {
                                    GenericMethods.DeleteFilesFromDirectory(downloadsFolder, downloadFileName + "*.xls*");
                                }
                                else
                                {
                                    GenericMethods.DeleteFilesFromDirectory(downloadsFolder, reportList[0].Text + ".xlsx");
                                }
                            }

                            Thread.Sleep(2000);
                            reportList[0].FindElement(By.XPath("./parent::*/parent::li//span[text()='" + reportFormat + "']")).Click();
                            Thread.Sleep(2000);
                        }
                        else
                        {
                            reportList[0].FindElement(By.XPath("./parent::*/parent::li//span[text()='PDF']")).Click();
                            Thread.Sleep(2000);
                        }
                        #endregion

                        #region Verify Report downloaded
                        if (reportFormat.Equals("Excel"))
                        {
                            if (downloadFileName != "")
                            {
                                if (GenericMethods.WaitForFileExists(downloadsFolder, downloadFileName + "*.xls*", 60))
                                {
                                    test.Pass("Excel report download successful");
                                    flag = true;
                                }
                                else
                                {
                                    GenericMethods.CaptureScreenshot();
                                    flag = false;
                                    test.Fail("Failed to verify Excel report download");
                                }
                                Thread.Sleep(5000);
                            }
                            else if (GenericMethods.SaveFileFromDialog(downloadsFolder, reportList[0].Text + ".xlsx", 30))
                            {
                                if (GenericMethods.WaitForFileExists(downloadsFolder, reportList[0].Text + ".xlsx", 30))
                                {
                                    test.Pass("Excel report download successful");
                                    flag = true;
                                }
                            }
                            else
                            {
                                GenericMethods.CaptureScreenshot();
                                flag = false;
                                test.Fail("Failed to verify Excel report download");
                            }
                        }
                        else
                        {
                            if (Win_PDFReportLoad.Exists(5))
                            {
                                if (wait.Until(driver => Win_PDFReport.Exists(45)))
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

        //Verify Mass File All
        public bool Fn_Verify_MassFileAll_In_YearEndTable(string downloadFileName)
        {
            bool flag = false;
            try
            {
                driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(300);
                if (Tab_MassFile.Exists(5) && Tab_MassFile.Enabled)
                {
                    Tab_MassFile.Click();
                    Thread.Sleep(2000);
                    var reportList = driver.FindElements(By.XPath(".//ul//span[text()='Mass File All']"));

                    if (GenericMethods.DeleteFilesFromDirectory(downloadsFolder, downloadFileName + "*.zip"))
                    {
                        if (reportList.Count > 0)
                        {
                            try
                            {
                                reportList[0].Click();
                            }
                            catch(Exception internalEx) // click timed out after 60 seconds
                            {
                                if(internalEx.Message.ToLower().Contains("click timed out"))
                                {
                                    //continue;
                                }
                                else
                                {
                                    throw;
                                }
                            }

                            if (GenericMethods.DownloadAndSaveFile(downloadsFolder, downloadFileName, 600))
                            {
                                Thread.Sleep(300000); // Wait for 5 mins
                                test.Pass("Mass File All downloaded and saved successfully");
                                flag = true;
                            }
                            else
                            {
                                test.Fail("Failed to download Mass File All");
                                GenericMethods.CaptureScreenshot();
                            }

                        }
                        else
                        {
                            test.Fail("No option available under Mass File");
                            GenericMethods.CaptureScreenshot();
                        }

                    }
                    else
                    {
                        test.Fail("Failed to delete existing file");
                        GenericMethods.CaptureScreenshot();
                    }
                    
                }
                else
                {
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

        //Verify Year End Export
        public bool Fn_Verify_Export_In_YearEndTable(string reportName, string downloadFileName)
        {
            bool flag = false;
            try
            { 
                if (Tab_Export.Exists(5) && Tab_Export.Enabled)
                {
                    Tab_Export.Click();
                    Thread.Sleep(2000);
                    var reportList = driver.FindElements(By.XPath(".//ul//span[text()='"+reportName+"']"));

                    if (GenericMethods.DeleteFilesFromDirectory(downloadsFolder, downloadFileName + "*.zip"))
                    {
                        if (reportList.Count > 0)
                        {
                            reportList[0].Click();
                            Thread.Sleep(3000);

                            driver.SwitchTo().Frame(Frame_YEExportWizard);
                            if(Tab_Export.Exists())
                            {
                                Tab_Export.Click();
                            }

                            if (GenericMethods.DownloadAndSaveFile(downloadsFolder, downloadFileName, 600))
                            {
                                test.Pass(reportName + " downloaded and saved successfully");
                                flag = true;
                            }
                            else
                            {
                                test.Fail("Failed to download "+reportName);
                                GenericMethods.CaptureScreenshot();
                            }
                            driver.SwitchTo().DefaultContent();
                            Btn_CloseX.Click();
                            
                        }
                        else
                        {
                            test.Fail("No option available under Export");
                            GenericMethods.CaptureScreenshot();
                        }

                    }
                    else
                    {
                        test.Fail("Failed to delete existing file");
                        GenericMethods.CaptureScreenshot();
                    }

                }
                else
                {
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

        //Verify Reports in Mass Print
        public bool Fn_Verify_MassPrint_In_YearEndTable(string reportName)
        {
            bool flag = false;
            try
            {
                if (Tab_MassPrint.Exists(5) && Tab_MassPrint.Enabled)
                {
                    Tab_MassPrint.Click();
                    Thread.Sleep(2000);
                    var reportList = driver.FindElements(By.XPath(".//ul//span[text()='" + reportName + "']"));

                    if (reportList.Count > 0)
                    {
                        reportList[0].Click();
                        Thread.Sleep(3000);
                        if (Pages.Home.Fn_Verify_PDF_Opened_In_Window())
                        {
                            flag = true;
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
                //EndTest();
                throw new Exception(ex.Message);
            }
            return flag;
        }

        //Verify Reports in Summary
        public bool Fn_Verify_Summary_In_YearEndTable(string reportName)
        {
            bool flag = false;
            try
            {
                if (Tab_Summary.Exists(5) && Tab_Summary.Enabled)
                {
                    Tab_Summary.Click();
                    Thread.Sleep(2000);
                    var reportList = driver.FindElements(By.XPath(".//ul//span[text()='" + reportName + "']"));

                    if (reportList.Count > 0)
                    {
                        reportList[0].Click();
                        Thread.Sleep(3000);
                        if (Pages.Home.Fn_Verify_PDF_Opened_In_Window())
                        {
                            flag = true;
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
                //EndTest();
                throw new Exception(ex.Message);
            }
            return flag;
        }

        //Search records in Year End Adjustments
        public bool Fn_Search_Records_In_YE_Adjustments(string year, string processgroup, string formType)
        {
            bool flag = false;
            try
            {
                DrpDwn_Year.SelectValueFromDropDown(year);
                Thread.Sleep(3000);
                if(processgroup!="")
                {
                    DrpDwn_ProcessGrp.SelectValueFromDropDown(processgroup);
                }
                
                Thread.Sleep(3000);
                DrpDwn_FormType.SelectValueFromDropDown(formType);
                Thread.Sleep(3000);
                Btn_Search.Click();

                if (Tbl_YearEndAdjustments.VerifyRecordDisplayedInTable(false))
                {
                    if(Tbl_YearEndAdjustments.FindElements(By.XPath(".//tbody//tr//td[text()='"+formType+"']")).Count > 0)
                    {
                        Tbl_YearEndAdjustments.FindElements(By.XPath(".//tbody//tr//td[text()='" + formType + "']")).FirstOrDefault().Click();
                        test.Pass("Search Successful in Year End Adjustments with form type : " + formType);
                        flag = true;
                    }
                    else
                    {
                        test.Fail("Failed to search records with Form type : " + formType + " in table");
                    }
                    
                }
                else
                {
                    test.Fail("No record found in Year End Adjustments table");
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

        //Verify Form Type in YE Adjustments

        public bool Fn_Verify_FormType_In_YE_Adjustments(string formType="")
        {
            bool flag = false;
            try
            {
                
                #region Verify Details for Employee
                string yearYE = (DateTime.Now.Year - 1).ToString();
                if (Tab_Details.Exists(5))
                {
                    Tab_Details.Click();
                    if(Lbl_YearEndAdjustment.Exists(10))
                    {
                        driver.SwitchTo().Frame(Frame_YEWizard);
                        if(driver.FindElements(By.XPath(".//span[text()='Year']//following::span[text()='"+yearYE+"']")).Count > 0)
                        {
                            test.Pass("Year End Adjustment Detail wizard opened successfully");
                            flag = true;
                        }
                        else
                        {
                            driver.SwitchTo().DefaultContent();
                            Btn_CloseX.Click();
                            return false;
                        }
                        driver.SwitchTo().DefaultContent();
                        Btn_CloseX.Click();
                    }
                    else
                    {
                        test.Fail("Unable to open Year End Adjustment Detail wizard");
                        return false;
                    }
                }
                else
                {
                    test.Fail("Details tab not found or is disabled");
                    return false;
                }
                Thread.Sleep(3000);
                #endregion

                #region Verify Report PDF

                if (Tab_Report.Exists(5))
                {
                    Tab_Report.Click();
                    if(Pages.Home.Fn_Verify_PDF_Opened_In_Window())
                    {
                        test.Pass("YE Adjustment Report PDF opened successfully");
                        flag = true;
                    }
                    else
                    {
                        test.Fail("Failed to verify Report PDF file opened");
                        return false;
                    }
                }
                else
                {
                    test.Fail("Report tab not found or is disabled");
                    return false;
                }
                Thread.Sleep(3000);

                #endregion

                #region Verify Adjustments Audit detail

                if (Tab_AdjustmentsAuditDetail.Exists(5))
                {
                    Tab_AdjustmentsAuditDetail.Click();
                    if(Pages.Home.Fn_Save_And_Verify_Excel("YeAdjustAudit"))
                    {
                        test.Pass("YE Adjustment Audit Detail Excel report verified successfully ");
                        flag = true;
                    }
                    else
                    {
                        test.Fail("YE Adjustment Audit Detail Excel report failed to verify");
                        return false;
                    }
                }
                else
                {
                    test.Fail("Adjustments Audit Detail tab not found or is disabled");
                    return false;
                }
                Thread.Sleep(3000);
                #endregion

                #region Verify Adjustments Audit

                if (Tab_AdjustmentsAudit.Exists(5))
                {
                    Tab_AdjustmentsAudit.Click();
                    if (Pages.Home.Fn_Save_And_Verify_Excel("YearEndAdjustmentReport"))
                    {
                        test.Pass("YE Adjustment Audit Excel report verified successfully ");
                        flag = true;
                    }
                    else
                    {
                        test.Fail("YE Adjustment Audit Excel report failed to verify");
                        return false;
                    }
                }
                else
                {
                    test.Fail("Adjustments Audit tab not found or is disabled");
                    return false;
                }
                Thread.Sleep(2000);
                #endregion

            }
            catch (Exception ex)
            {
                test.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                GenericMethods.CaptureScreenshot();
                throw new Exception(ex.Message);
            }
            return flag;
        }


        #endregion
    }
}
