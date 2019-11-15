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
    public class LandingPage : AutomationCore
    {
        private IWebDriver driver;

        #region Home Page Object Collection
        [FindsBy(How = How.Id, Using = "lblLoggedInValue")]
        private IWebElement Lbl_user { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_ProfileGroup_ProfileGroup_Input")]
        private IWebElement Drpdwn_UserGroup { get; set; }

        [FindsBy(How = How.Id, Using = "ctl00_ProfileGroup_ProfileGroup_DropDown")]
        private IWebElement List_UserGroup { get; set; }

        [FindsBy(How = How.XPath, Using = "*//span[text()=\"Home\" or text()=\"Maison\"]")]
        private IWebElement Menu_Home { get; set; }

        [FindsBy(How = How.XPath, Using = "*//a[@id='ChangeLanguage']")]
        private IWebElement Link_Language { get; set; }
        

        private bool IsAt()
        {
            return Browsers.Title.Contains("WorkLinks");
        }
        #endregion

        #region Constructor
        public LandingPage()
        {
            driver = Browsers.GetDriver;
        }
        #endregion

        #region Landing Page reusable Methods
        //Gets the signed in user name
        public String GetSignInUser()
        {
            try
            {
                Assert.IsTrue(IsAt());
                Lbl_user.Highlight();
            }
            catch (Exception ex)
            {
                test.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                EndTest();
                //throw new Exception(ex.Message);
            }
            return Lbl_user.Text.ToString();
        }

        public bool Fn_ChangeUserLanguage(string language)
        {
            Boolean cnt = false;
            Boolean flag = false;
            try
            {
                if(Link_Language.Exists(10))
                {
                    string lng = Link_Language.Text;
                    if(lng.Equals(language))
                    {
                        Link_Language.Click();
                        cnt = true;
                        test.Pass("User Language changed to :" + language);
                        flag = true;
                    }
                    if(!cnt)
                    {
                        test.Pass("User Language already in :" + language);
                        flag = true;
                    }
                }
                else
                {
                    test.Fail("Unable to change Language");
                    flag = false;
                }
                
            }
            catch (Exception ex)
            {
                test.Error(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                EndTest();
                //throw new Exception(ex.Message);
            }
            if(flag)
            {
                test.Pass("Language selected successfully");
            }
            else
            {
                test.Fail("Language selection failed");
            }
            return flag;
        }

        //Selects the Signed in user profile as Administrator or Others
        public bool Fn_SelectUserProfile(string Option)
        {
            Boolean flag = false;
            try
            {
                flag = GenericMethods.SelectValueFromDropDown(Drpdwn_UserGroup, List_UserGroup, Option);
                if (flag)
                {
                    test.Pass("User Type selected");
                }
                else
                {
                    test.Fail("User Type not selected");
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

        //Navigate to Home Screen
        public bool Fn_NavigateToHomeScreen()
        {
            Boolean flag = false;
            try
            {
                if (Menu_Home.Exists(30))
                {
                    Menu_Home.Highlight();
                    Menu_Home.Click();
                    IWebElement Link_Home = driver.FindElement(By.XPath("*//a[contains(text(),'Home') or contains(text(),'Maison')]"));
                    if (Link_Home.Exists(20))
                    {
                        flag = true;
                    }
                    else
                    {
                        flag= false;
                    }
                }
                else
                {
                    test.Fail("Unabe to find 'Home' Menu");
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

        //Verify records in a column in table
        public bool Fn_VerifyRecordsInSingleColumn(IWebElement table, string columnName, string colValueString)
        {
            Boolean flag = false;
            int colNum, rowCount, finalCount = 0;
            IReadOnlyList<IWebElement> rows;
            string[] colVals;
            try
            {
                if (table.Exists(10))
                {
                    colNum = table.GetColumnNumber(columnName);
                    rows = table.FindElements(By.XPath("./tbody/tr"));
                    rowCount = table.FindElements(By.XPath("./tbody/tr")).Count;
                    colVals = colValueString.Split(';');

                    if (rowCount > 0)
                    {
                        foreach (string colVal in colVals)
                        {
                            int valCount = 0;
                            for (int i = 0; i < rowCount; i++)
                            {
                                if (rows[i].FindElements(By.TagName("td"))[colNum].Text.ToLower().Equals(colVal.ToLower()))
                                {
                                    rows[i].FindElements(By.TagName("td"))[colNum].Highlight();
                                    valCount++;
                                    test.Pass("Value : " + colVal + " found in Column : " + columnName);
                                    break;
                                }
                            }
                            if (valCount == 0)
                            {
                                test.Fail("Value : " + colVal + " Not found in Column : " + columnName);
                                flag = false;
                            }
                            else
                            {
                                finalCount++;
                            }
                        }

                        if (finalCount == colVals.Length)
                        {
                            test.Pass("All records found in Column : " + columnName);
                            flag = true;
                        }
                        else
                        {
                            test.Fail("Few or all of the records are missing in Column : " + columnName);
                            flag = false;
                        }
                    }
                    else
                    {
                        test.Fail("No rows present in table");
                        flag = false;
                    }
                }
                else
                {
                    test.Fail("Required table does not Exist");
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
