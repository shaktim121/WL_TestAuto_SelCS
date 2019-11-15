using System;
using System.Net;
using System.Threading;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.PageObjects;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;

namespace WL.TestAuto
{
    [TestClass]
    public class Tests_new : AutomationCore
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void LogIn_Test()
        {
            string testName = TestContext.TestName;
            var test = extent.CreateTest(testName);

            if (Pages.LogIn.Fn_LogInToApplication())
            {
                test.Log(Status.Pass, "Login to application successful");
            }
            else
            {
                test.Log(Status.Fail, "Login to application Failed");
            }

        }


    }
}
