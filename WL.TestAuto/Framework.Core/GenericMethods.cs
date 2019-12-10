using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using iText.IO.Source;
using iText.IO.Util;
using iText.Kernel.Pdf;
using iText.Kernel.Utils;
using iText.Layout;
using iText.Layout.Element;
using iText.IO.Font;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Kernel.Pdf.Tagging;
using iText.Kernel.Pdf.Tagutils;

namespace WL.TestAuto
{
    public static class GenericMethods
    {
        //private static readonly IWebDriver driver = Browsers.GetDriver;

        //Extension for Get Data from app.config
        public static string AppSettings(this string Key)
        {
            string val = string.Empty;
            if (System.Configuration.ConfigurationManager.AppSettings[Key] != null)
                val = ConfigurationManager.AppSettings[Key].ToString();
            return val;
        }

        //Clear text from text field
        public static void ClearText(this IWebElement element)
        {
            try
            {
                if (element.Exists(5))
                {
                    element.SendKeys(Keys.Control + "A" + Keys.Control);
                    element.SendKeys(Keys.Delete);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " " + ex.StackTrace);
            }
        }

        //Set Text
        public static void SetText(this IWebElement element, string text)
        {
            try
            {
                if (element.Exists(5))
                {
                    element.Clear();
                    element.SendKeys(text);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " " + ex.StackTrace);
            }
        }

        //Exists method
        public static bool Exists(this IWebElement element)
        {
            Boolean flag = false;
            try
            {
                if (element.Displayed && element.Size.Height > 0)
                {
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " " + ex.StackTrace);
            }
            return flag;
        }

        //Exists method - Overload with timeout
        public static bool Exists(this IWebElement element, int timeoutSeconds)
        {
            Boolean flag = false;
            try
            {
                WebDriverWait wait = new WebDriverWait(Browsers.GetDriver, TimeSpan.FromSeconds(timeoutSeconds));
                if (wait.Until(driver => element.Displayed && element.Size.Height > 0))
                {
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                return false;
                //throw new Exception(ex.Message + " " + ex.StackTrace);
            }
            return flag;
        }

        //NotExists method - with timeout
        public static bool NotExists(this IWebElement element, int timeoutSeconds)
        {
            Boolean flag = false;
            try
            {
                WebDriverWait wait = new WebDriverWait(Browsers.GetDriver, TimeSpan.FromSeconds(timeoutSeconds));
                if (wait.Until(driver => element.Displayed && element.Size.Height > 0))
                {
                    flag = false;
                }
                else
                {
                    flag = true;
                }
            }
            catch (NoSuchElementException)
            {
                flag = true;
            }
            catch(TimeoutException)
            {
                flag = true;
            }
            catch (Exception ex)
            {
                flag = true;
            }
            return flag;
        }

        //Highlight method
        public static void Highlight(this IWebElement element)
        {
            try
            {
                IJavaScriptExecutor jsDriver = (IJavaScriptExecutor)Browsers.GetDriver;
                /*string brWidth = element.GetCssValue("border-width");
                string brStyle = element.GetCssValue("border-style");
                string brColor = element.GetCssValue("border-color");
                string bgColor = element.GetCssValue("background");*/
                string highlightJavascript = @"arguments[0].style.cssText = ""border-width: 3px; border-style: solid; border-color: red; background: yellow"";";
                string clearHighlight = @"arguments[0].style.cssText = ""border-width: 0px; border-style: solid; border-color: red; background: none"";";

                for (int i = 0; i < 3; i++)
                {
                    jsDriver.ExecuteScript(highlightJavascript, new object[] { element });
                    Thread.Sleep(100);
                    jsDriver.ExecuteScript(clearHighlight, new object[] { element });
                    Thread.Sleep(100);
                }
                //JQuery
                /*string highlightJavascript = @"$(arguments[0]).css({ ""border-width"" : ""3px"", ""border-style"" : ""solid"", ""border-color"" : ""red"", ""background"" : ""yellow"" });";*/
                
                jsDriver = null;
                //driver = Browsers.GetDriver;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " " + ex.StackTrace);
            }

            Thread.Sleep(1000);
        }

        //Select check boxes from list
        public static bool SelectCheckBoxFromList(this IWebElement element, string valueList, bool check)
        {
            Boolean flag = false;
            try
            {
                string[] chkValues = valueList.Split(';');
                foreach(string chk in chkValues)
                {
                    IReadOnlyList<IWebElement> chkTags = element.FindElements(By.XPath(".//ul/li//span[@class='rlbText']"));
                    for(int i=0; i<chkTags.Count; i++)
                    {
                        //if name matches then select corresponding check box
                        if(chkTags[i].Text == chk)
                        {
                            if(chkTags[i].FindElement(By.XPath("./parent::*/input")).Selected == !check)
                            {
                                chkTags[i].FindElement(By.XPath("./parent::*/input")).Click();
                                flag = true;
                                break;
                            }
                            else
                            {
                                flag = true;
                                break;
                            }
                        }
                        
                        if(i==chkTags.Count-1)
                        {
                            flag = false;
                            break;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " " + ex.StackTrace);
            }
            return flag;
        }
        
        //Method to select value from dropdown (Dropdown with Arrow v)
        public static bool SelectValueFromDropDown(this IWebElement SelectionObject, IWebElement dropDown, string value)
        {
            Boolean flag = false;
            try
            {
                Assert.IsTrue(SelectionObject.Exists(), "Dropdown exists in the page");
                SelectionObject.Highlight();
                string val_act = SelectionObject.GetAttribute("value").ToString();

                if (!val_act.ToLower().Contains(value.ToLower()))
                {
                    SelectionObject.Click();
                    if (!dropDown.Displayed)
                    {
                        SelectionObject.Click();
                    }
                    Assert.IsTrue(dropDown.Displayed, "List Displayed");
                    Thread.Sleep(2000);
                    IReadOnlyList<IWebElement> dlist = dropDown.FindElements(By.TagName("li"));

                    foreach (IWebElement li in dlist)
                    {
                        if (li.Text.ToLower().Equals(value.ToLower()))
                        {
                            li.Click();
                            flag = true;
                            break;
                        }
                    }
                }
                else
                {
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " " + ex.StackTrace);
            }
            return flag;
        }

        //SelectMenuFromDropDown
        public static bool SelectValueFromSlideDropDown(IWebElement SelectionObject, string menu, IWebElement divDropDown, string value)
        {
            Boolean flag = false;
            try
            {
                Assert.IsTrue(SelectionObject.Exists(), "Dropdown exists in the page");
                SelectionObject.Highlight();
                string val_act = SelectionObject.Text.ToString();

                if (val_act.ToLower().Contains(menu.ToLower()))
                {
                    SelectionObject.Click();
                    Thread.Sleep(3000);
                    Assert.IsTrue(divDropDown.Exists(), "List Displayed");
                    IReadOnlyList<IWebElement> dlistVals = divDropDown.FindElements(By.TagName("span"));

                    foreach (IWebElement span in dlistVals)
                    {
                        if (span.Text.ToLower().Equals(value.ToLower()))
                        {
                            span.Click();
                            flag = true;
                            break;
                        }
                    }
                }
                else
                {
                    flag = false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " " + ex.StackTrace);
            }
            return flag;
        }

        //Select value from drop down and select submenu
        public static bool SelectValueFromSlideDropDown(IWebElement SelectionObject, string menu, IWebElement divDropDown, string value, string subValue)
        {
            Boolean flag = false;
            try
            {
                Assert.IsTrue(SelectionObject.Exists(), "Dropdown exists in the page");
                SelectionObject.Highlight();
                string val_act = SelectionObject.Text.ToString();

                if (val_act.ToLower().Contains(menu.ToLower()))
                {
                    SelectionObject.Click();
                    Thread.Sleep(3000);
                    Assert.IsTrue(divDropDown.Exists(), "List Displayed");
                    IReadOnlyList<IWebElement> dlistVals = divDropDown.FindElements(By.TagName("span"));

                    foreach (IWebElement span in dlistVals)
                    {
                        //Selecting Value
                        if (span.Text.ToLower().Equals(value.ToLower()))
                        {
                            span.Click();
                            IWebElement subVal = Browsers.GetDriver.FindElement(By.XPath("*//span[text()='" + subValue + "']"));
                            if (subVal.Exists(10))
                            {
                                subVal.Click();
                                flag = true;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    flag = false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " " + ex.StackTrace);
            }
            return flag;
        }
        
        //Verify if records available in table
        public static bool VerifyRecordDisplayedInTable(this IWebElement table, bool isTableEmpty)
        {
            Boolean flag = false;
            try
            {   
                if (isTableEmpty)
                {
                    IReadOnlyList<IWebElement> noElement = Browsers.GetDriver.FindElements(By.XPath("*//table//*[contains(text(),\"No records to display\")]"));
                    if (noElement.Count > 0)
                    {
                        noElement.FirstOrDefault().Highlight();
                        flag = true;
                    }
                }
                else
                {
                    if(table.Exists(20))
                    {
                        Thread.Sleep(5000);
                        IReadOnlyList<IWebElement> rows = table.FindElements(By.XPath("./tbody/tr[contains(@class,'rgRow') or contains(@class,'rgAltRow')]"));
                        if (rows.Count > 0)
                        {
                            table.Highlight();
                            flag = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " " + ex.StackTrace);
            }
            return flag;
        }

        //Get column number
        public static int GetColumnNumber(this IWebElement table, string columnName)
        {
            int colNum = -1;
            try
            {
                if(table.Exists(10))
                {
                    IReadOnlyList<IWebElement> thTags = table.FindElements(By.XPath("./thead/tr/th[contains(@class,'rgHeader')]"));
                    
                    foreach (IWebElement thTag in thTags)
                    {
                        colNum++;
                        if (!thTag.Text.Equals(""))
                        {
                            if(thTag.Text.ToLower().Equals(columnName.ToLower()))
                            {
                                break;
                            }
                        }
                        else if(!thTag.FindElement(By.TagName("a")).Text.Equals(""))
                        {
                            if(thTag.FindElement(By.TagName("a")).Text.ToLower().Equals(columnName.ToLower()))
                            {
                                break;
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " " + ex.StackTrace);
            }
            return colNum;
        }
        
        //Save Reports
        public static void SaveReport()
        {

        }
        
        //Read from PDF
        public static string GetTextFromPDF(string PDFFileName)
        {
            StringBuilder text = new StringBuilder();
            PdfReader PDF = new PdfReader(PDFFileName);
            PdfDocument docPDF = new PdfDocument(PDF);
            try
            {
                for (int i = 1; i <= docPDF.GetNumberOfPages(); i++)
                {
                    string txt = PdfTextExtractor.GetTextFromPage(docPDF.GetPage(i));
                    text.Append(txt);
                }
                docPDF.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " " + ex.StackTrace);
            }
            
            return text.ToString();
        }

        //Select Date from Calendar popup
        public static bool SelectDateFromCalendarPopup(this IWebElement calendarObj, string date)
        {
            bool flag = false;
            IWebDriver driver = Browsers.GetDriver;
            string day = date.Split('/')[1];
            try
            {
                if (calendarObj.Exists(10))
                {
                    calendarObj.Click();
                    IWebElement calMain = driver.FindElement(By.XPath("*//table[@class='RadCalendar RadCalendar_Silk']"));
                    if (calMain.Exists())
                    {
                        Thread.Sleep(1000);
                        IWebElement calTitle = driver.FindElement(By.XPath("*//td[@class='rcTitle']"));
                        
                        if (date.Equals(DateTime.Now.ToString("MM/dd/yyyy")))
                        {
                            calTitle.Click();
                            IWebElement btnToday = driver.FindElement(By.XPath("*//input[@type='button' and @value='Today']"));
                            if (btnToday.Exists(10))
                            {
                                btnToday.Click();
                            }
                            Thread.Sleep(2000);
                            calMain.FindElement(By.XPath(".//td/a[text()='"+day+"']")).Click();
                            flag = true;

                        }
                        flag = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " " + ex.StackTrace);
            }

            return flag;
        }
    }
}
