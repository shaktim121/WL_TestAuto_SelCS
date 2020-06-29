using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WL.TestAuto
{
    class SinGenerator
    {
        /// <summary>
        /// Generates a valid 9-digit SIN.
        /// </summary>
        public static string GetValidSin()
        {
            string sin = string.Empty;
            int[] s = GenerateSin();

            while (!SinIsValid(s))
                s = GenerateSin();

            sin = String.Join(",", s.Select(p => p.ToString()).ToArray());
            sin = sin.Replace(",", "");
            return sin;
        }

        /// <summary>
        /// Generates a potential SIN. Not guaranteed to be valid.
        /// </summary>
        private static int[] GenerateSin()
        {
            Random r = new Random();
            int[] s = new int[9];

            for (int i = 0; i < 9; i++)
                s[i] = r.Next(0, 10);

            return s;
        }

        /// <summary>
        /// Validates a 9-digit SIN.
        /// </summary>
        /// <param name="sin"></param>
        private static bool SinIsValid(int[] sin)
        {
            if (sin.Length != 9)
                throw new ArgumentException();

            int checkSum = 0;

            for (int i = 0; i < sin.Length; i++)
            {
                int m = (i % 2) + 1;
                int v = sin[i] * m;
                if (v > 10)
                    checkSum += 1 + (v - 10);
                else
                    checkSum += v;
            }

            return checkSum % 10 == 0;
        }
    }

    public class Utilities
    {
        //  Get Time Stamp
        // String timeStamp = GetTimestamp(DateTime.Now);
        public static string GetTimeStamp(DateTime value)
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

        //Random number generator
        public static int Random_Number(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

    }
}
