using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;

namespace WL.TestAuto
{
    public static class Browsers
    {
        //private static readonly string baseURL = ConfigurationManager.AppSettings["url"];
        //private static readonly string browser = ConfigurationManager.AppSettings["browser"];
        public static void Init(string browser, string baseURL)
        {
            switch (browser)
            {
                case "Chrome":
                    Utilities.Kill_Process("chromedriver");
                    GetDriver = new ChromeDriver();
                    break;
                case "IE":
                    //_driver = new InternetExplorerDriver();
                    break;
                case "Firefox":
                    GetDriver = new FirefoxDriver();
                    break;
            }

            Goto(baseURL);
        }
        public static string Title
        {
            get { return GetDriver.Title; }
        }
        public static IWebDriver GetDriver { get; private set; }
        public static void Goto(string url)
        {
            GetDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            GetDriver.Manage().Window.Maximize();
            GetDriver.Url = url;
        }
        public static void Close()
        {
            //_driver.Close();
            if (GetDriver != null)
            {
                GetDriver.Quit();
                GetDriver.Dispose();
                GetDriver = null;
            }

        }
    }
}
