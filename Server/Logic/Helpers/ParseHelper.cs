using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Helpers
{
    public static class ParseHelper
    {
        public static bool TryParseToInvariantCulture(this double input, out double value)
        {
            try
            {
                value = Convert.ToDouble(input, CultureInfo.InvariantCulture);
                return true;
            }
            catch (Exception)
            {
                value = 0;
                return false;
            }
        }

        //public static bool TryParseToDouble(this string input, out double value)
        //{
        //    bool success = double.TryParse(input.Replace(".", ","), NumberStyles.Any, new CultureInfo("sv-SE"), out double latDouble);

        //    value = latDouble;
        //    return success;
        //}
    }
}
