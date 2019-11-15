using System;
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

    }
}
