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

namespace WL.TestAuto
{
    public class AutomationCore
    {   
        // This will get the current WORKING directory (i.e. \bin\Debug)
        // or: Directory.GetCurrentDirectory() gives the same result
        public static readonly string workingDirectory = Environment.CurrentDirectory;
        // This will get the current PROJECT directory
        public static readonly string projectDirectory = Directory.GetParent(workingDirectory).Parent.FullName;

        /*private static readonly string pth = System.Reflection.Assembly.GetCallingAssembly().CodeBase;
        private static readonly string actualPath = pth.Substring(0, pth.LastIndexOf("bin"));
        private static readonly string projectPath = new Uri(actualPath).LocalPath;*/

        //Report initialize
        private static readonly string fileName = ConfigurationManager.AppSettings["TestName"] + "_Report_" + Utilities.GetTimeStamp(DateTime.Now);
        private static readonly string fileNameExt = fileName + ".html";
        public static readonly string reportPath = projectDirectory + "\\TestReports\\" + fileName + "\\" + fileNameExt;

        public static ExtentReports extent;
        public static ExtentV3HtmlReporter htmlReporter;
        public static ExtentTest test;

        public static DataSet data;
        public TestContext TestContext { get; set; }

        // Core Automation class
        
        [TestInitialize]
        public void StartUpTest()// This method fire at the start of the Test
        {   
            string testName = TestContext.TestName;
            test = extent.CreateTest(testName);
            
            try
            {
                //Get all test data from DB
                data = testName.LoadTestData();
                //Initiate App Launch
                Browsers.Init();

                //LogIn to Application
                Pages.LogIn.Fn_LogInToApplication();
            }
            catch (Exception ex)
            {
                test.Error("Failed to Launch/Login to Application or Browser with Exception: " + ex.StackTrace + " and Message: "+ ex.Message);
                Browsers.Close();
                extent.Flush();
                Thread.Sleep(5000);
                throw new Exception(ex.Message);
            }
        }

        [TestCleanup]
        public void EndTest()// This method will fire at the end of the Test
        {  
            //LogOut of Application
            Pages.LogIn.Fn_LogOutOfApplication();
            Thread.Sleep(5000);
            Browsers.Close();
            extent.Flush();
            Thread.Sleep(5000);
        }
        

        [AssemblyCleanup]
        public void EndClass()
        {
            
        }
    }
}
