using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
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
    public class Landing_HRPage : AutomationCore
    {
        private IWebDriver driver;

        #region Home Page Object Collection
        [FindsBy(How = How.XPath, Using = "*//a[text()=\"Employee\"]")]
        private IWebElement Link_Employee { get; set; }

        [FindsBy(How = How.XPath, Using = "*//input[@type=\"button\" and @value=\"Search\"]")]
        private IWebElement Btn_SearchEmployee { get; set; }

        [FindsBy(How = How.XPath, Using = "*//table[@class=\"rgMasterTable\" and contains(@id,\"ctl00_MainContent_EmployeeSearchControl1\")]")]
        private IWebElement Tbl_Employee { get; set; }

        [FindsBy(How = How.XPath, Using = "*//input[@value='View Report']")]
        private IWebElement Btn_ViewReport { get; set; }

        //[FindsBy(How = How.XPath, Using = "*//input[contains(@id,'EmployeeListing') and @value='View Report']")]
        //private IWebElement Btn_ViewReportEL { get; set; }*/

        [FindsBy(How = How.XPath, Using = "*//iframe[contains(@id,'AnniversaryListing')]")]
        private IWebElement PDFReportAreaAL { get; set; }

        [FindsBy(How = How.XPath, Using = "*//iframe[contains(@id,'EmployeeListing')]")]
        private IWebElement PDFReportAreaEL { get; set; }

        [FindsBy(How = How.XPath, Using = "*//input[@value='PDF']")]
        private IWebElement Btn_ExportPDF { get; set; }

        #endregion

        #region Constructor
        public Landing_HRPage()
        {
            driver = Browsers.GetDriver;
        }
        #endregion

        #region Human Resources Page reusable Methods
        /// <summary>
        /// Verify fields in Employee Screen with given parameters
        /// </summary>
        /// <param name="textFields"></param>
        /// <param name="buttons"></param>
        /// <param name="drpDowns"></param>
        /// <param name="checkBoxes"></param>
        /// <param name="toolBars"></param>
        /// <param name="tableColumns"></param>
        /// <returns></returns>
        public bool Fn_VerifyFieldsIn_HR_EmployeeScreen(string textFields, string buttons, string drpDowns, string checkBoxes, string toolBars, string tableColumns)
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
                            //ele.FirstOrDefault().Highlight();
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
                        IReadOnlyCollection<IWebElement> ele = driver.FindElements(By.XPath("*//input[@type=\"button\" and @value=\"" + b + "\"]"));
                        if (ele.Count > 0)
                        {
                            //ele.FirstOrDefault().Highlight();
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
                        if (ele.Count > 0)
                        {
                            //ele.FirstOrDefault().Highlight();
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
                            //ele.FirstOrDefault().Highlight();
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

                //Code to verify Toolbar
                if (toolBars.Length > 0)
                {
                    string[] toolBarL = toolBars.Split(';');
                    foreach (string tool in toolBarL)
                    {
                        IReadOnlyCollection<IWebElement> ele = driver.FindElements(By.XPath("*//div[contains(@class,\"ToolBar\")]//span[text()=\"" + tool + "\"]"));
                        if (ele.Count > 0)
                        {
                            //ele.FirstOrDefault().Highlight();
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
                            //ele.FirstOrDefault().Highlight();
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
            catch (Exception ex)
            {
                test.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                throw new Exception(ex.Message);
            }
            return flag;
        }

        /// <summary>
        /// Search if Records should be displayed or not
        /// </summary>
        public bool Fn_SearchRecordInEmployeeTable()
        {
            try
            {
                if (Tbl_Employee.VerifyRecordDisplayedInTable(true))
                {
                    Btn_SearchEmployee.Click();

                    if (Tbl_Employee.VerifyRecordDisplayedInTable(false))
                    {
                        test.Pass("Employee records loaded");
                        return true;
                    }
                    else
                    {
                        test.Fail("Employee records not loaded");
                        return false;
                    }
                }
                else
                {
                    test.Fail("No records displayed should be visible");
                    return false;
                }
            }
            catch (Exception ex)
            {
                test.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Open and verify if report is displayed on screen
        /// </summary>
        /// <param name="reportName"></param>
        /// <returns></returns>
        public bool Fn_ViewAndVerifyReportDisplayedOnScreen(string reportName)
        {
            Boolean flag = false;
            try
            {
                switch(reportName)
                {
                    case "Anniversary Listing":
                        if (Btn_ViewReport.Exists())
                        {
                            Btn_ViewReport.Click();
                            if(PDFReportAreaAL.Exists(10))
                            {
                                Thread.Sleep(5000);
                                //Btn_ExportPDF.Highlight();
                                Btn_ExportPDF.Click();
                                flag = true;
                            }
                        }
                        break;

                    case "Employee Listing":
                        if (Btn_ViewReport.Exists())
                        {
                            Btn_ViewReport.Click();
                            if (PDFReportAreaEL.Exists(15))
                            {
                                Thread.Sleep(5000);
                                //Btn_ExportPDF.Highlight();
                                Btn_ExportPDF.Click();
                                flag = true;
                            }
                        }
                        break;

                    default:
                        test.Fail("Invalid Report Name");
                        break;
                }
                
            }
            catch (Exception ex)
            {
                test.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                throw new Exception(ex.Message);
            }
            return flag;
        }

        #endregion
    }

}
