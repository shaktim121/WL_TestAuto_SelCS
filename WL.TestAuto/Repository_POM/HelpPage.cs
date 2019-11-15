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
    public class HelpPage : AutomationCore
    {
        private IWebDriver driver;

        #region Admin Page Object Collection
        [FindsBy(How = How.XPath, Using = "*//div[@id='ctl00_MainMenu']//span[text()='Help']")]
        private IWebElement Menu_Help { get; set; }

        [FindsBy(How = How.XPath, Using = "*//div[@id='ctl00_MainMenu']/ul/li[6]/div")]
        private IWebElement Menu_SlideHelp { get; set; }

        #endregion

        #region Constructor
        public HelpPage()
        {
            driver = Browsers.GetDriver;
        }
        #endregion

        #region Help Page reusable Methods

        //Verify options under Help menu
        public bool Fn_Verify_Options_Under_Help(string Options)
        {
            Boolean flag = false;
            try
            {
                if(!Options.Equals(""))
                {
                    string[] lstOption = Options.Split(';');

                    Menu_Help.Click();
                    Thread.Sleep(2000);

                    if(Menu_SlideHelp.Exists(5))
                    {
                        IReadOnlyList<IWebElement> lists = Menu_SlideHelp.FindElements(By.TagName("li"));

                        int cnt = 0;
                        foreach (string opt in lstOption)
                        {
                            for(int i=0; i<lists.Count; i++)
                            {
                                if(opt.ToLower().Equals(lists[i].Text.ToLower()))
                                {
                                    cnt++;
                                    lists[i].Highlight();
                                    test.Pass("Help option : " + opt + " Found");
                                    flag = true;
                                    break;
                                }

                                if(i==lists.Count-1)
                                {
                                    test.Fail("Help option : " + opt + " Not found");
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

        #endregion
    }
}
