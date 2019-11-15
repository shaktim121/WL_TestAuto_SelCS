using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.PageObjects;

namespace WL.TestAuto
{
    public static class Pages
    {
        private static T GetPages<T>() where T : new()
        {
            var page = new T();
            PageFactory.InitElements(Browsers.GetDriver, page);
            return page;
        }

        //LogIn Page
        public static LogInPage LogIn
        {
            get { return GetPages<LogInPage>(); }
        }

        //Landing Page or Home Page
        public static LandingPage Home
        {
            get { return GetPages<LandingPage>(); }
        }

        //Human Resources Page
        public static HRPage HR
        {
            get { return GetPages<HRPage>(); }
        }

        //Payroll Page
        public static PayrollPage Payroll
        {
            get { return GetPages<PayrollPage>(); }
        }

        //Setup page
        public static SetupPage Setup
        {
            get { return GetPages<SetupPage>(); }
        }

        //Admin page
        public static AdminPage Admin
        {
            get { return GetPages<AdminPage>(); }
        }

        //Help page
        public static HelpPage Help
        {
            get { return GetPages<HelpPage>(); }
        }
    }
}
