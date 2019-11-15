using System;
using System.Configuration;
using System.Net;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.PageObjects;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;

namespace WL.TestAuto
{
    [TestClass]
    public class Tests_old
    {
        static IWebDriver driver;
        static string url;

        static ExtentHtmlReporter htmlReporter = new ExtentHtmlReporter("ReportTestGoogle1.html");
        static ExtentReports extent = new ExtentReports();
        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void Setup()
        {
            #region Report Initialize
            //var htmlReporter = new ExtentHtmlReporter("ReportTEST1.html");
            //var extent = new ExtentReports();
            extent.AttachReporter(htmlReporter);
            extent.AddSystemInfo("OS:", Environment.OSVersion.ToString());
            extent.AddSystemInfo("Host Name:", Dns.GetHostName());
            #endregion

            #region Launch App
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("http://www.google.com");
            #endregion
        }

        [TestMethod]
        public void LogIn_Test_Google()
        {
            string testName = TestContext.TestName;
            var test = extent.CreateTest(testName);

            test.Log(Status.Pass, "Title name: " + driver.Title);
            Thread.Sleep(5000);
        }

        [TestCleanup]
        public void TearDown()
        {
            driver.Close();
            extent.Flush();
        }
    }
}
