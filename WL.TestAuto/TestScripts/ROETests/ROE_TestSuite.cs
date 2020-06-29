using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.Threading;
using System.Globalization;

namespace WL.TestAuto
{
    [TestClass]
    public class ROE_TestSuite : AutomationCore
    {
        public string xmlDataFile = projectDirectory + "\\TestScripts\\PayrollTests\\ProcessGroupDetails.xml";

        [TestMethod]
        public void WL_CAN_ROE_001000_Terminate_Employee_with_Payperiod()
        {
            #region Data Variables
            string EmpNum = Utilities.Random_Number(100001, 9999999).ToString();
            string FirstName = data.GetTestData("First_Name");
            string LastName = data.GetTestData("Last_Name");
            string suffix = DateTime.Now.ToString("HHmm");
            //EmpNum = (Convert.ToInt32(EmpNum) + 1).ToString();
            FirstName += suffix;
            LastName += suffix;
            string SIN = SinGenerator.GetValidSin();

            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string hrScreen1 = data.GetTestData("HR_Screen1");
            string hrScreen2 = data.GetTestData("HR_Screen2");
            string payScreen = data.GetTestData("Payroll_Screen");
            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroup");
            string runType = GenericMethods.GetXMLNodeValue(xmlDataFile, "RunType");
            string chequeDate = GenericMethods.GetXMLNodeValue(xmlDataFile, "ChequeDate");
            if (chequeDate == "CURR_DATE")
            {
                chequeDate = DateTime.Now.AddDays(1).ToString("M/d/yyyy");
            }

            string startDate = string.Empty;
            string endDate = string.Empty;
            string eventName = data.GetTestData("Event");
            string reason = data.GetTestData("Reason");
            string ROEreason = data.GetTestData("ROE_Reason");

            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            #region Hire Employee
            //Navigate to Employee to search employee
            Pages.HR.Fn_NavigateThroughHumanResources(hrScreen1);
            if (Pages.HR.Fn_Search_Employee_Exists(EmpNum, FirstName, LastName, "", false))
            {
                test.Pass("Employee Does not Exist");
            }
            else
            {
                test.Pass("Employee already Exists");
                EndTest();
            }

            //Navigate to Add Wizard Scren in Human Resources
            Pages.HR.Fn_NavigateThroughHumanResources(hrScreen2);
            Thread.Sleep(5000);
            if (Pages.HR.Fn_Hire_Employee(EmpNum, FirstName, LastName, SIN, processGrp))
            {
                //Verify Employee hired Successfully
                Pages.Payroll.Fn_NavigateThroughPayroll(payScreen);
                if (Pages.HR.Fn_Search_Employee_Exists(EmpNum, FirstName, LastName, "", true))
                {
                    test.Pass("Employee Hired Successfully : " + FirstName + " " + LastName);
                }
                else
                {
                    test.Fail("Employee Hire failed/unable to find hired employee : " + FirstName + " " + LastName);
                    GenericMethods.CaptureScreenshot();
                    Assert.Fail("Failed to Hire Employee");
                }
            }
            else
            {
                Assert.Fail("Failed to Hire Employee");
            }

            Thread.Sleep(5000);


            #endregion

            //Navigate to Home
            Pages.Home.Fn_NavigateToHomeScreen();

            #region Get Pay Period
            //Navigate to Payroll process Screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll("Payroll Process");

            //Undo calculation if calculated already
            Pages.Payroll.Fn_Undo_Payroll_Calculation(processGrp);

            Dictionary<string, string> payPeriod = Pages.Payroll.Fn_Get_Payroll_Period_From_PayrollProcess(processGrp, runType);
            if (payPeriod.Count == 2)
            {
                startDate = payPeriod["StartDate"];
                endDate = payPeriod["EndDate"];
                test.Pass("Payroll period Start Date: " + startDate + " and Period End Date: " + endDate);
            }
            else
            {
                test.Fail("Failed to get Pay Period");
                GenericMethods.CaptureScreenshot();
                Assert.Fail("Failed to get Pay Period");
            }

            #endregion

            #region Terminate Employee
            //Navigate to Employee to search employee
            Pages.HR.Fn_NavigateThroughHumanResources("Employee");

            //Search Employee record
            if (!Pages.HR.Fn_Search_Employee_Exists(EmpNum, FirstName, LastName, "", true))
            {
                test.Fail("Employee does not exist");
                GenericMethods.CaptureScreenshot();
                Assert.Fail();
            }

            string terminationDate = DateTime.Parse(startDate, new CultureInfo("en-US")).AddDays(1).ToString("MM/dd/yyyy").Replace('-', '/');
            string lastPaidDate = terminationDate;

            test.Info("Termination Date : " + terminationDate + " & Last Paid Date : " + lastPaidDate);
            //Terminate selected Employee
            if (Pages.HR.Fn_Terminate_Employee(terminationDate, lastPaidDate, eventName, reason, ROEreason))
            {
                test.Pass("Employee Terminated Successfully");
            }
            else
            {
                test.Fail("Failed to terminate Employee");
                GenericMethods.CaptureScreenshot();
                Assert.Fail("Failed to terminate Employee");

            }

            //Navigate to Employee to search employee
            Pages.HR.Fn_NavigateThroughHumanResources("Employee");

            Pages.HR.Fn_Search_Employee_Exists(EmpNum, FirstName, LastName, "", true);

            if (Pages.Payroll.Fn_Verify_Employee_Details_In_Payroll_Position("Terminated", "Status-Terminated"))
            {
                test.Pass("Employee: " + FirstName + " " + LastName + " Termination verified Successfully");
            }
            else
            {
                test.Fail("Failed to verify terminated Employee : " + FirstName + " " + LastName);
                GenericMethods.CaptureScreenshot();
                Assert.Fail();
            }

            #endregion

            //Navigate to Home
            Pages.Home.Fn_NavigateToHomeScreen();
            Thread.Sleep(10000);

            #region Calculate and Post Payroll

            //Navigate to Payroll process Screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll("Payroll Process");

            //Calcualte payroll
            if (!Pages.Payroll.Fn_Calculate_Payroll_In_Payroll_Process(processGrp, runType, chequeDate))
            {
                GenericMethods.CaptureScreenshot();
                Assert.Fail("Calculate payroll failed");
            }

            //Post Calculated Payroll
            if (!Pages.Payroll.Fn_Post_Calculated_Payroll_In_Payroll_Process())
            {
                GenericMethods.CaptureScreenshot();
                Assert.Fail("Failed to Post payroll");
            }
            #endregion

            #region ROE Creation Search and Export
            //processGrp = "Semi-Monthly"; empFName = "Joseph"; empLName = "Ward";
            //Navigate to ROE Creation Search under Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll("ROE Creation Search");

            if (Pages.Payroll.Fn_Search_ROE_Creation(processGrp, FirstName, LastName))
            {
                if (!Pages.Payroll.Fn_Export_ROE_And_Verify_Record(FirstName, LastName))
                {
                    GenericMethods.CaptureScreenshot();
                    Assert.Fail("Failed to Export and verify ROE report");
                }
            }
            else
            {
                test.Fail("Failed to Search employee in ROE Creation");
                GenericMethods.CaptureScreenshot();
                Assert.Fail("Failed to Search employee in ROE Creation");
            }

            #endregion


        }

        [TestMethod]
        public void WL_CAN_ROE_001100_Terminate_Employee_outside_Payroll_Period()
        {
            #region Data Variables
            string EmpNum = Utilities.Random_Number(100001, 9999999).ToString();
            string FirstName = data.GetTestData("First_Name");
            string LastName = data.GetTestData("Last_Name");
            string suffix = DateTime.Now.ToString("HHmm");
            //EmpNum = (Convert.ToInt32(EmpNum) + 1).ToString();
            FirstName += suffix;
            LastName += suffix;
            string SIN = SinGenerator.GetValidSin();

            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string hrScreen1 = data.GetTestData("HR_Screen1");
            string hrScreen2 = data.GetTestData("HR_Screen2");
            string payScreen = data.GetTestData("Payroll_Screen");
            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroup");
            string runType = GenericMethods.GetXMLNodeValue(xmlDataFile, "RunType");
            string chequeDate = GenericMethods.GetXMLNodeValue(xmlDataFile, "ChequeDate");
            if (chequeDate == "CURR_DATE")
            {
                chequeDate = DateTime.Now.AddDays(1).ToString("M/d/yyyy");
            }

            string startDate = string.Empty;
            string endDate = string.Empty;
            string eventName = data.GetTestData("Event");
            string reason = data.GetTestData("Reason");
            string ROEreason = data.GetTestData("ROE_Reason");

            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            #region Hire Employee
            //Navigate to Employee to search employee
            Pages.HR.Fn_NavigateThroughHumanResources(hrScreen1);
            if (Pages.HR.Fn_Search_Employee_Exists(EmpNum, FirstName, LastName, "", false))
            {
                test.Pass("Employee Does not Exist");
            }
            else
            {
                test.Pass("Employee already Exists");
                EndTest();
            }

            //Navigate to Add Wizard Scren in Human Resources
            Pages.HR.Fn_NavigateThroughHumanResources(hrScreen2);
            Thread.Sleep(5000);
            if (Pages.HR.Fn_Hire_Employee(EmpNum, FirstName, LastName, SIN, processGrp))
            {
                //Verify Employee hired Successfully
                Thread.Sleep(5000);
                Pages.Payroll.Fn_NavigateThroughPayroll(payScreen);
                if (Pages.HR.Fn_Search_Employee_Exists(EmpNum, FirstName, LastName, "", true))
                {
                    test.Pass("Employee Hired Successfully : " + FirstName + " " + LastName);
                }
                else
                {
                    test.Fail("Employee Hire failed/unable to find hired employee : " + FirstName + " " + LastName);
                    GenericMethods.CaptureScreenshot();
                    Assert.Fail("Failed to Hire Employee");
                }
            }
            else
            {
                Assert.Fail("Failed to Hire Employee");
            }

            Thread.Sleep(5000);


            #endregion

            //Navigate to Home
            Pages.Home.Fn_NavigateToHomeScreen();
            Thread.Sleep(5000);

            #region Get Pay Period
            //Navigate to Payroll process Screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll("Payroll Process");

            //Undo calculation if calculated already
            Pages.Payroll.Fn_Undo_Payroll_Calculation(processGrp);

            Dictionary<string, string> payPeriod = Pages.Payroll.Fn_Get_Payroll_Period_From_PayrollProcess(processGrp, runType);
            if (payPeriod.Count == 2)
            {
                startDate = payPeriod["StartDate"];
                endDate = payPeriod["EndDate"];
                test.Pass("Payroll period Start Date: " + startDate + " and Period End Date: " + endDate);
            }
            else
            {
                test.Fail("Failed to get Pay Period");
                GenericMethods.CaptureScreenshot();
                Assert.Fail("Failed to get Pay Period");
            }

            #endregion

            #region Terminate Employee
            //Navigate to Employee to search employee
            Pages.HR.Fn_NavigateThroughHumanResources("Employee");

            //Search Employee record
            if (!Pages.HR.Fn_Search_Employee_Exists(EmpNum, FirstName, LastName, "", true))
            {
                test.Fail("Employee does not exist");
                GenericMethods.CaptureScreenshot();
                Assert.Fail();
            }

            string terminationDate = DateTime.Parse(endDate, new CultureInfo("en-US")).AddDays(5).ToString("MM/dd/yyyy").Replace('-', '/');
            string lastPaidDate = DateTime.Parse(endDate, new CultureInfo("en-US")).AddDays(5).ToString("MM/dd/yyyy").Replace('-', '/');

            test.Info("Termination Date : " + terminationDate + "& Last Paid Date : " + lastPaidDate);
            //Terminate selected Employee
            if (Pages.HR.Fn_Terminate_Employee(terminationDate, lastPaidDate, eventName, reason, ROEreason))
            {
                test.Pass("Employee Terminated Successfully");
            }
            else
            {
                test.Fail("Failed to terminate Employee");
                GenericMethods.CaptureScreenshot();
                Assert.Fail("Failed to terminate Employee");

            }

            //Navigate to Employee to search employee
            Pages.HR.Fn_NavigateThroughHumanResources("Employee");

            Pages.HR.Fn_Search_Employee_Exists(EmpNum, FirstName, LastName, "", true);

            if (Pages.Payroll.Fn_Verify_Employee_Details_In_Payroll_Position("Terminated", "Status-Terminated"))
            {
                test.Pass("Employee:" + FirstName + " " + LastName + " Terminated Successfully");
            }
            else
            {
                test.Fail("Failed to terminate Employee : " + FirstName + " " + LastName);
                GenericMethods.CaptureScreenshot();
                Assert.Fail();
            }

            #endregion

            //Navigate to Home
            Pages.Home.Fn_NavigateToHomeScreen();
            Thread.Sleep(5000);

            #region Calculate and Post Payroll

            //Navigate to Payroll process Screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll("Payroll Process");

            //Calcualte payroll
            if (!Pages.Payroll.Fn_Calculate_Payroll_In_Payroll_Process(processGrp, runType, chequeDate))
            {
                GenericMethods.CaptureScreenshot();
                Assert.Fail("Calculate payroll failed");
            }

            //Post Calculated Payroll
            if (!Pages.Payroll.Fn_Post_Calculated_Payroll_In_Payroll_Process())
            {
                GenericMethods.CaptureScreenshot();
                Assert.Fail("Failed to Post payroll");
            }
            #endregion

            #region ROE Creation Search and Export
            //processGrp = "Semi-Monthly"; empFName = "Joseph"; empLName = "Ward";
            //Navigate to ROE Creation Search under Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll("ROE Creation Search");

            if (Pages.Payroll.Fn_Search_ROE_Creation(processGrp, FirstName, LastName))
            {
                test.Fail("Employee found in Search - Failed");
                GenericMethods.CaptureScreenshot();
                Assert.Fail("Employee found in Search - Failed");
            }
            else
            {
                test.Pass("Employee record not found in ROE Creation Search - Expected");
            }

            #endregion
        }

        [TestMethod]
        public void WL_CAN_ROE_001200_Terminate_Employee_with_LPD_same_as_PPEndDate()
        {
            #region Data Variables
            string EmpNum = Utilities.Random_Number(100001, 9999999).ToString();
            string FirstName = data.GetTestData("First_Name");
            string LastName = data.GetTestData("Last_Name");
            string suffix = DateTime.Now.ToString("HHmm");
            //EmpNum = (Convert.ToInt32(EmpNum) + 1).ToString();
            FirstName += suffix;
            LastName += suffix;
            string SIN = SinGenerator.GetValidSin();

            string userLang = data.GetTestData("User_Language");
            string userProf = data.GetTestData("User_Profile");
            string hrScreen1 = data.GetTestData("HR_Screen1");
            string hrScreen2 = data.GetTestData("HR_Screen2");
            string payScreen = data.GetTestData("Payroll_Screen");
            string processGrp = GenericMethods.GetXMLNodeValue(xmlDataFile, "ProcessGroup");
            string runType = GenericMethods.GetXMLNodeValue(xmlDataFile, "RunType");
            string chequeDate = GenericMethods.GetXMLNodeValue(xmlDataFile, "ChequeDate");
            if (chequeDate == "CURR_DATE")
            {
                chequeDate = DateTime.Now.AddDays(1).ToString("M/d/yyyy");
            }

            string startDate = string.Empty;
            string endDate = string.Empty;
            string eventName = data.GetTestData("Event");
            string reason = data.GetTestData("Reason");
            string ROEreason = data.GetTestData("ROE_Reason");

            #endregion

            //Change User language after log in
            Pages.Home.Fn_ChangeUserLanguage(userLang);

            //Select User Profile
            Pages.Home.Fn_SelectUserProfile(userProf);

            #region Hire Employee
            //Navigate to Employee to search employee
            Pages.HR.Fn_NavigateThroughHumanResources(hrScreen1);
            if (Pages.HR.Fn_Search_Employee_Exists(EmpNum, FirstName, LastName, "", false))
            {
                test.Pass("Employee Does not Exist");
            }
            else
            {
                test.Pass("Employee already Exists");
                EndTest();
            }

            //Navigate to Add Wizard Scren in Human Resources
            Pages.HR.Fn_NavigateThroughHumanResources(hrScreen2);
            Thread.Sleep(5000);
            if (Pages.HR.Fn_Hire_Employee(EmpNum, FirstName, LastName, SIN, processGrp))
            {
                //Verify Employee hired Successfully
                Pages.Payroll.Fn_NavigateThroughPayroll(payScreen);
                if (Pages.HR.Fn_Search_Employee_Exists(EmpNum, FirstName, LastName, "", true))
                {
                    test.Pass("Employee Hired Successfully : " + FirstName + " " + LastName);
                }
                else
                {
                    test.Fail("Employee Hire failed/unable to find hired employee : " + FirstName + " " + LastName);
                    GenericMethods.CaptureScreenshot();
                    Assert.Fail("Failed to Hire Employee");
                }
            }
            else
            {
                Assert.Fail("Failed to Hire Employee");
            }

            Thread.Sleep(5000);


            #endregion

            //Navigate to Home
            Pages.Home.Fn_NavigateToHomeScreen();

            #region Get Pay Period
            //Navigate to Payroll process Screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll("Payroll Process");

            //Undo calculation if calculated already
            Pages.Payroll.Fn_Undo_Payroll_Calculation(processGrp);

            Dictionary<string, string> payPeriod = Pages.Payroll.Fn_Get_Payroll_Period_From_PayrollProcess(processGrp, runType);
            if (payPeriod.Count == 2)
            {
                startDate = payPeriod["StartDate"];
                endDate = payPeriod["EndDate"];
                test.Pass("Payroll period Start Date: " + startDate + " and Period End Date: " + endDate);
            }
            else
            {
                test.Fail("Failed to get Pay Period");
                GenericMethods.CaptureScreenshot();
                Assert.Fail("Failed to get Pay Period");
            }

            #endregion

            #region Terminate Employee
            //Navigate to Employee to search employee
            Pages.HR.Fn_NavigateThroughHumanResources("Employee");

            //Search Employee record
            if (!Pages.HR.Fn_Search_Employee_Exists(EmpNum, FirstName, LastName, "", true))
            {
                test.Fail("Employee does not exist");
                GenericMethods.CaptureScreenshot();
                Assert.Fail();
            }

            string terminationDate = DateTime.Parse(endDate, new CultureInfo("en-US")).AddDays(1).ToString("MM/dd/yyyy").Replace('-', '/');
            string lastPaidDate = endDate;
            test.Info("Termination Date : " + terminationDate + "& Last Paid Date : " + lastPaidDate);
            //Terminate selected Employee
            if (Pages.HR.Fn_Terminate_Employee(terminationDate, lastPaidDate, eventName, reason, ROEreason))
            {
                test.Pass("Employee Terminated Successfully");
            }
            else
            {
                test.Fail("Failed to terminate Employee");
                GenericMethods.CaptureScreenshot();
                Assert.Fail("Failed to terminate Employee");

            }

            //Navigate to Employee to search employee
            Pages.HR.Fn_NavigateThroughHumanResources("Employee");

            Pages.HR.Fn_Search_Employee_Exists(EmpNum, FirstName, LastName, "", true);

            if (Pages.Payroll.Fn_Verify_Employee_Details_In_Payroll_Position("Terminated", "Status-Terminated"))
            {
                test.Pass("Employee:" + FirstName + " " + LastName + " Terminated Successfully");
            }
            else
            {
                test.Fail("Failed to terminate Employee : " + FirstName + " " + LastName);
                GenericMethods.CaptureScreenshot();
                Assert.Fail();
            }

            #endregion

            //Navigate to Home
            Pages.Home.Fn_NavigateToHomeScreen();

            #region Calculate and Post Payroll

            //Navigate to Payroll process Screen in Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll("Payroll Process");

            //Calcualte payroll
            if (!Pages.Payroll.Fn_Calculate_Payroll_In_Payroll_Process(processGrp, runType, chequeDate))
            {
                GenericMethods.CaptureScreenshot();
                Assert.Fail("Calculate payroll failed");
            }

            //Post Calculated Payroll
            if (!Pages.Payroll.Fn_Post_Calculated_Payroll_In_Payroll_Process())
            {
                GenericMethods.CaptureScreenshot();
                Assert.Fail("Failed to Post payroll");
            }
            #endregion

            #region ROE Creation Search and Export
            //processGrp = "Semi-Monthly"; empFName = "Joseph"; empLName = "Ward";
            //Navigate to ROE Creation Search under Payroll
            Pages.Payroll.Fn_NavigateThroughPayroll("ROE Creation Search");

            if (Pages.Payroll.Fn_Search_ROE_Creation(processGrp, FirstName, LastName))
            {
                if (!Pages.Payroll.Fn_Export_ROE_And_Verify_Record(FirstName, LastName))
                {
                    GenericMethods.CaptureScreenshot();
                    Assert.Fail("Failed to Export and verify ROE report");
                }
            }
            else
            {
                test.Fail("Failed to Search employee in ROE Creation");
                GenericMethods.CaptureScreenshot();
                Assert.Fail("Failed to Search employee in ROE Creation");
            }

            #endregion
        }

    }
}