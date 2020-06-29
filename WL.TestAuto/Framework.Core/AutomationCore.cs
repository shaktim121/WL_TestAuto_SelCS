﻿using System;
using System.IO;
using System.Net;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using System.Data;
using System.Runtime.Remoting.Messaging;

namespace WL.TestAuto
{
    [TestClass]
    public class AutomationCore
    {
        public static readonly string downloadsFolder = @"C:\Users\" + Environment.UserName + "\\Downloads";
        // This will get the current WORKING directory (i.e. \bin\Debug)
        // or: Directory.GetCurrentDirectory() gives the same result
        public static readonly string workingDirectory = Environment.CurrentDirectory;
        // This will get the current PROJECT directory
        public static readonly string projectDirectory = Directory.GetParent(workingDirectory).Parent.FullName;

        /*private static readonly string pth = System.Reflection.Assembly.GetCallingAssembly().CodeBase;
        private static readonly string actualPath = pth.Substring(0, pth.LastIndexOf("bin"));
        private static readonly string projectPath = new Uri(actualPath).LocalPath;*/

        //Report initialize
        private static readonly string fileName = "TestName".AppSettings() + "_Report_" + Utilities.GetTimeStamp(DateTime.Now);
        private static readonly string fileNameExt = fileName + ".html";
        public static readonly string reportFolder = projectDirectory + "\\TestReports\\" + fileName;
        public static readonly string reportPath = reportFolder + "\\" + fileNameExt;

        public static ExtentReports extent;
        public static ExtentV3HtmlReporter htmlReporter;
        public static ExtentTest test;

        public static DataSet data;
        public TestContext TestContext { get; set; }

        // Core Automation class

        [AssemblyInitialize]
        public static void StartClass(TestContext context)
        {
            #region Report Initialize
            extent = new ExtentReports();
            htmlReporter = new ExtentV3HtmlReporter(reportPath);
            //htmlReporter.LoadConfig(projectDirectory + "\\" + @"Extent-Config.xml");
            htmlReporter.Config.DocumentTitle = "Worklinks Automation - " + "TestName".AppSettings() + " Report";
            htmlReporter.Config.ReportName = "TestName".AppSettings();

            extent.AddSystemInfo("OS : ", Environment.OSVersion.ToString());
            extent.AddSystemInfo("Host Name : ", Dns.GetHostName());
            extent.AddSystemInfo("Browser : ", "browser".AppSettings());
            extent.AddSystemInfo("Run User : ", Environment.UserName.ToString());
            extent.AddSystemInfo("WL Environment : ", "url".AppSettings());
            extent.AddSystemInfo("LogIn User : ", "user".AppSettings());

            extent.AttachReporter(htmlReporter);
            #endregion
        }

        [TestInitialize]
        public void StartUpTest()// This method fire at the start of the Test
        {
            string testName = TestContext.TestName;
            test = extent.CreateTest(testName);

            string baseURL = "url".AppSettings();
            string browser = "browser".AppSettings();
            string user = "user".AppSettings();
            string pwd = "pwd".AppSettings();

            try
            {
                //Get all test data from DB
                data = testName.LoadTestData();

                //Initiate App Launch
                Browsers.Init(browser, baseURL);

                //LogIn to Application
                Pages.LogIn.Fn_LogInToApplication(user, pwd);
            }
            catch (Exception ex)
            {
                test.Error("Failed to Launch/Login to Application or Browser with Exception: " + ex.StackTrace + " and Message: " + ex.Message);
                //GenericMethods.CaptureScreenshot();
                throw new Exception("Exception: " + ex.StackTrace + " and Message: " + ex.Message);
            }
        }

        [TestCleanup]
        public void EndTest()// This method will fire at the end of the Test
        {
            //LogOut of Application
            Pages.LogIn.Fn_LogOutOfApplication();
            Thread.Sleep(3000);
            Browsers.Close();
            extent.Flush();
            Thread.Sleep(3000);
        }

        [AssemblyCleanup]
        public static void EndClass()
        {
            if(File.Exists(reportPath))
            {
                File.Copy(reportPath, @"\\cowldv03\F$\WLAutomation_Reports\" + fileNameExt);
                Thread.Sleep(2000);
            }
        }
    }
}
