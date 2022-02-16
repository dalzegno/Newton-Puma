using System;
using System.Globalization;

namespace Logic.Helpers
{
    public static class ParseHelper
    {
        public static bool TryParseToInvariantCulture(this double input, out double value)
        {
            try
            {
                var valueStr = input.ToString(CultureInfo.InvariantCulture);
                value = Convert.ToDouble(valueStr, CultureInfo.InvariantCulture);
                return true;
            }
            catch (Exception)
            {
                value = 0;
                return false;
            }
        }
    }
}
