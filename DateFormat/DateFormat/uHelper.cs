using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateFormat
{
    public class UHelper
    {
        public static DateTime UDateFormat(string target)
        {
            DateTime result = new DateTime();
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            double ms = Convert.ToDouble(target);
            result= epoch.AddMilliseconds(ms);
            return result;

        }
    }
}
