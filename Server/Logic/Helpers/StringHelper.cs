using System.Globalization;
using System.Linq;

namespace Logic.Helpers
{
    public static class StringHelper
    {
        /// <summary>
        /// Turns the first character in a string to upper case and the rest of the string to lower.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Adjusted string</returns>
        public static string FirstCharToUpper(this string input) => 
            input?.First().ToString().ToUpper() + input.Substring(1).ToLower();
    }
}
