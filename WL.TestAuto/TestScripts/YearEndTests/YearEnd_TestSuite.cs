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
using System.Data;

namespace WL.TestAuto
{
    [TestClass]
    public class YearEnd_TestSuite : AutomationCore
    {
        public string xmlDataFile = projectDirectory + "\\TestScripts\\PayrollTests\\ProcessGroupDetails.xml";

        


    }
}
