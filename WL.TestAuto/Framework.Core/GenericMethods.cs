using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;
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
using AutoIt;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.IO.Compression;
using AventStack.ExtentReports.Model;
using Org.BouncyCastle.Bcpg.OpenPgp;
using AventStack.ExtentReports.Gherkin.Model;
using RazorEngine.Compilation.ImpromptuInterface;
using System.Globalization;

namespace WL.TestAuto
{
    public static class GenericMethods
    {
        #region Test Utilities
        //Method to take screenshot
        public static void CaptureScreenshot()
        {
            try
            {
                ((ITakesScreenshot)Browsers.GetDriver).GetScreenshot().SaveAsFile(AutomationCore.reportFolder + "\\" + "Capture_" + Utilities.GetTimeStamp(DateTime.Now) + ".png", ScreenshotImageFormat.Png);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " " + ex.StackTrace);
            }
        }

        //Extension for Get Data from app.config
        public static string AppSettings(this string Key)
        {
            string val = string.Empty;
            if (System.Configuration.ConfigurationManager.AppSettings[Key] != null)
                val = ConfigurationManager.AppSettings[Key].ToString();
            return val;
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

        #endregion

        #region Object actions/Operations
        //Clear text from text field
        public static void ClearText(this IWebElement element)
        {
            try
            {
                if (element.Exists())
                {
                    element.SendKeys(OpenQA.Selenium.Keys.Control + "A" + OpenQA.Selenium.Keys.Control);
                    element.SendKeys(OpenQA.Selenium.Keys.Delete);
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
                if (element.Exists())
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
        public static bool Exists(this IWebElement element, [Optional] double timeOutSec, [Optional] double polling)
        {
            Boolean flag = false;
            try
            {
                if(timeOutSec == 0 && polling == 0)
                {
                    timeOutSec = 30;
                    polling = .5;
                }

                DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(Browsers.GetDriver)
                {
                    /* Setting the timeout in seconds */
                    Timeout = TimeSpan.FromSeconds(timeOutSec),
                    /* Configuring the polling frequency in ms */
                    PollingInterval = TimeSpan.FromMilliseconds(polling),
                    Message = "Element to be searched not found"
                };
                /* Ignore the exception - NoSuchElementException that indicates that the element is not present */
                wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(InvalidOperationException));

                if (wait.Until(x => element.Displayed && element.Size.Height > 0))
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

        //Exists method - Overload with timeout
        public static bool Exists(this IWebElement element, double timeoutSeconds)
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
            catch (TimeoutException)
            {
                flag = true;
            }
            catch (Exception ex)
            {
                flag = true;
            }
            return flag;
        }

        //Select check boxes from list
        public static bool SelectCheckBoxFromList(this IWebElement element, string valueList, bool check)
        {
            Boolean flag = false;
            try
            {
                string[] chkValues = valueList.Split(';');
                foreach (string chk in chkValues)
                {
                    IReadOnlyList<IWebElement> chkTags = element.FindElements(By.XPath(".//ul/li//span[@class='rlbText']"));
                    for (int i = 0; i < chkTags.Count; i++)
                    {
                        //if name matches then select corresponding check box
                        if (chkTags[i].Text == chk)
                        {
                            if (chkTags[i].FindElement(By.XPath("./parent::*/input")).Selected == !check)
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

                        if (i == chkTags.Count - 1)
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
        public static bool SelectValueFromDropDown(this IWebElement SelectionObject, string value)
        {
            Boolean flag = false;
            WebDriverWait eWait = new WebDriverWait(Browsers.GetDriver, TimeSpan.FromSeconds(10));
            By List_AllDrpDwn = By.XPath("*//div[contains(@class,'RadComboBoxDropDown') and contains(@style,'display: block')]");
            try
            {
                Assert.IsTrue(SelectionObject.Exists(), "Dropdown exists in the page");
                SelectionObject.Highlight();
                string val_act = SelectionObject.GetAttribute("value").ToString();

                if (!val_act.ToLower().Contains(value.ToLower()))
                {
                    SelectionObject.Click();

                    IWebElement dropDown = Browsers.GetDriver.FindElement(List_AllDrpDwn);
                    if (!dropDown.Displayed)
                    {
                        SelectionObject.Click();
                    }
                    Thread.Sleep(2000);
                    Assert.IsTrue(dropDown.Displayed, "List Displayed");
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
                    if (table.Exists(20))
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
                if (table.Exists(10))
                {
                    IReadOnlyList<IWebElement> thTags = table.FindElements(By.XPath("./thead/tr/th[contains(@class,'rgHeader')]"));

                    foreach (IWebElement thTag in thTags)
                    {
                        colNum++;
                        if (!thTag.Text.Equals(""))
                        {
                            if (thTag.Text.ToLower().Equals(columnName.ToLower()))
                            {
                                break;
                            }
                        }
                        else if (!thTag.FindElement(By.TagName("a")).Text.Equals(""))
                        {
                            if (thTag.FindElement(By.TagName("a")).Text.ToLower().Equals(columnName.ToLower()))
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

        //Select Date from Calendar popup
        public static bool SelectDateFromCalendarPopup(this IWebElement calendarObj, string date)
        {
            bool flag = false;
            IWebDriver driver = Browsers.GetDriver;
            date = date.Replace('-', '/');
            //date = Convert.ToDateTime(date).AddDays(1).ToString("M/d/yyyy");
            DateTime givenDate = Convert.ToDateTime(date);

            string dayOfWeek = givenDate.DayOfWeek.ToString();
            string month = givenDate.ToString("MMM");
            string year = givenDate.Year.ToString();
            string day = givenDate.Day.ToString();
            //string fullDate = givenDate.ToString("D", CultureInfo.CreateSpecificCulture("en-US")); //Saturday, June 6, 2020
            string fullDate = dayOfWeek + ", " + givenDate.ToString("MMMM") + " " + givenDate.ToString("dd") + ", " + year; //Friday, June 05, 2020


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

                        if (date.Equals(DateTime.Now.ToString("M/d/yyyy")))
                        {
                            calTitle.Click();
                            IWebElement btnToday = driver.FindElement(By.XPath("*//input[@type='button' and @value='Today']"));
                            if (btnToday.Exists(10))
                            {
                                btnToday.Click();
                            }
                            Thread.Sleep(2000);
                            calMain.FindElement(By.XPath(".//td/a[text()='" + day + "']")).Click();
                            flag = true;

                        }
                        else
                        {
                            calTitle.Click();
                            if (driver.FindElements(By.XPath(".//td/a[text()='" + month + "']")).Count > 0 && driver.FindElements(By.XPath(".//td/a[text()='" + year + "']")).Count > 0)
                            {
                                driver.FindElement(By.XPath(".//td/a[text()='" + month + "']")).Click();
                                Thread.Sleep(1000);
                                driver.FindElement(By.XPath(".//td/a[text()='" + year + "']")).Click();
                                Thread.Sleep(1000);
                                driver.FindElement(By.Id("rcMView_OK")).Click();
                                Thread.Sleep(1000);

                                //if (driver.FindElements(By.XPath(".//td[@title='" + fullDate + "']")).Count > 0 || driver.FindElements(By.XPath(".//td/span[text()='"+day+"']")).Count > 0)
                                if (driver.FindElements(By.XPath(".//td/*[text()='"+day+"']")).Count > 0)
                                {
                                    var selectDate = driver.FindElement(By.XPath(".//td/*[text()='" + day + "']/parent::td"));
                                    while (selectDate.GetAttribute("class").Contains("rcWeekend") || selectDate.GetAttribute("class").Contains("rcOutOfRange"))
                                    {
                                        date = Convert.ToDateTime(date).AddDays(1).ToString("M/d/yyyy");
                                        givenDate = Convert.ToDateTime(date);
                                        day = givenDate.Day.ToString();
                                        fullDate = givenDate.DayOfWeek.ToString() + ", " + givenDate.ToString("MMMM") + " " + givenDate.ToString("dd") + ", " + year;
                                        selectDate = driver.FindElement(By.XPath(".//td/*[text()='" + day + "']/parent::td"));
                                    }

                                    selectDate.Click();
                                    if (calMain.Exists(5)) calendarObj.Click();
                                    Thread.Sleep(2000);
                                }

                            }
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

        //public static bool SelectDateFromCalendarPopup(this IWebElement calendarObj, string date)
        //{
        //    bool flag = false;
        //    IWebDriver driver = Browsers.GetDriver;
        //    string day = date.Replace('-', '/').Split('/')[1];
        //    try
        //    {
        //        if (calendarObj.Exists(10))
        //        {
        //            calendarObj.Click();
        //            IWebElement calMain = driver.FindElement(By.XPath("*//table[@class='RadCalendar RadCalendar_Silk']"));
        //            if (calMain.Exists())
        //            {
        //                Thread.Sleep(1000);
        //                IWebElement calTitle = driver.FindElement(By.XPath("*//td[@class='rcTitle']"));

        //                if (date.Equals(DateTime.Now.ToString("M/d/yyyy")))
        //                {
        //                    calTitle.Click();
        //                    IWebElement btnToday = driver.FindElement(By.XPath("*//input[@type='button' and @value='Today']"));
        //                    if (btnToday.Exists(10))
        //                    {
        //                        btnToday.Click();
        //                    }
        //                    Thread.Sleep(2000);
        //                    calMain.FindElement(By.XPath(".//td/a[text()='" + day + "']")).Click();
        //                    flag = true;

        //                }
        //                flag = true;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message + " " + ex.StackTrace);
        //    }

        //    return flag;
        //}

        #endregion

        #region File System Operations
        //Save Reports
        public static bool SaveFileFromDialog(string filePath, string fileName, [Optional] int waitTimeSec)
        {
            bool flag = false;
            try
            {

                if (AutoItX.WinWaitActive("Save As", "", 10) != 0)
                {
                    AutoItX.Send(filePath+"\\"+fileName);
                    SendKeys.SendWait(@"{Enter}");
                    //Thread.Sleep(TimeSpan.FromSeconds(waitTime));
                }

                if (waitTimeSec == 0)
                {
                    waitTimeSec = 30;
                }

                while (waitTimeSec != 0)
                {
                    if (WaitForFileExists(filePath, fileName, waitTimeSec))
                    {
                        flag = true;
                        break;
                    }
                    else
                    {
                        Thread.Sleep(1000);
                        waitTimeSec -= 1;
                    }
                    if (waitTimeSec == 0)
                    {
                        flag = false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " " + ex.StackTrace);
            }
            return flag;
        }

        //Zip File Exctract
        public static bool ExtractZipFile(string sourceFile, string destFolder)
        {
            bool flag = false;
            try
            {
                if (sourceFile != "" && destFolder != "")
                {
                    ZipFile.ExtractToDirectory(sourceFile, destFolder);
                    if (Directory.Exists(destFolder))
                    {
                        var dirs = Directory.GetDirectories(destFolder);
                        var files = Directory.GetFiles(destFolder);

                        if (dirs.Length != 0 || files.Length != 0)
                        {
                            flag = true;
                        }
                        else
                        {
                            flag = false;
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

        //Read From XML File with node value
        public static string GetXMLNodeValue(string XMLFilePath, string nodeName)
        {
            string nodeValue = string.Empty;
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(XMLFilePath);

                XmlNode node = xml.GetElementsByTagName(nodeName)[0];
                nodeValue = node.InnerText;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " " + ex.StackTrace);
            }

            return nodeValue;
        }

        //Delete files from directory with filename
        public static bool DeleteFilesFromDirectory(string path, string file)
        {
            try
            {
                //Delete files if any
                if (Directory.EnumerateFiles(path, "*"+file+"*").Any())
                {
                    var AllFiles = Directory.EnumerateFiles(path, "*" + file + "*").ToList();
                    foreach (var files in AllFiles)
                    {
                        File.Delete(files);
                    }
                }
                Thread.Sleep(TimeSpan.FromSeconds(2));
                if(Directory.EnumerateFiles(path, "*" + file + "*").Any())
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " " + ex.StackTrace);
            }
            return true;
        }

        //Wait for file Exist/downloaded
        public static bool WaitForFileExists(string path, string file, [Optional] int timeOut)
        {
            bool flag = false;
            int cnt = 0;
            try
            {
                if (timeOut == 0) timeOut = 30;
                while (!(Directory.EnumerateFiles(path, "*"+file+"*").Any()))
                {
                    cnt++;
                    Thread.Sleep(2000); // Polling for 2 seconds
                    if (cnt == timeOut)
                    {
                        break;
                    }
                }

                if (Directory.EnumerateFiles(path, "*" + file + "*").Any())
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

        //Get last modified file from Directory
        public static string GetLastModifiedFile(string path)
        {
            var directory = new DirectoryInfo(path);
            var myFile = directory.GetFiles()
                            .OrderByDescending(f => f.LastWriteTime)
                            .First();

            return myFile.Name;
        }


        #endregion


    }
}
