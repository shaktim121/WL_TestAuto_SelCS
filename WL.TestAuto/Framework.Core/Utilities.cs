using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WL.TestAuto
{
    public class Utilities
    {
        //  Get Time Stamp
        // String timeStamp = GetTimestamp(DateTime.Now);
        public static String GetTimeStamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffff");

        }

        //Kill process with process name
        public static void Kill_Process(string processName)
        {
            try
            {
                Process[] processList = Process.GetProcessesByName(processName);
                foreach (Process process in processList)
                {
                    process.Kill();
                    process.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " " + ex.StackTrace);
            }
            
        
        }

    }
}
