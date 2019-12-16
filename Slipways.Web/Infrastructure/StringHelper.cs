using System;
using System.Linq;

namespace com.b_velop.Slipways.Web.Infrastructure
{
    public static class StringHelper
    {
        public static string FirstUp(string input)
        {
            var letter = input.First().ToString().ToUpper();
            var str = letter + input.Substring(1);
            return str;
        }

        public static string FirstUpper(this string input)
        {
            input = input.ToLower();
            var strs = input.Split(' ');
            var result = string.Empty;
            foreach (var str in strs)
            {
                result += FirstUp(str) + " ";
            }
            input = result.TrimEnd();
            strs = input.Split('-');
            if (strs.Length == 1)
                return input;
            result = string.Empty;
            foreach (var str in strs)
            {
                result += FirstUp(str) + "-";
            }
            result = result.Substring(0, result.Length - 1);
            return result;
        }
    }
}
