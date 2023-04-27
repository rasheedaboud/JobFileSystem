using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Core.Utils
{
    public static class SanitizeString
    {
        public static string SanitizeAllSpecials(this string stringToSanitize)
        {
            return new String(stringToSanitize.Where(Char.IsLetterOrDigit).ToArray());
        }

        public static string SanitizeReport(this string stringToSanitize)
        {
            var reg = new Regex(@"[,~!@#$%^&*()+={}:;<>/?\\''""\][]");
            var sanitizedString = reg.Replace(stringToSanitize, string.Empty);
            return sanitizedString;
        }
    }
    public static class NumberGenerator
    {
        public static string RandomNumber => GenerateRandomInt();
        public static string EstimateNumber => $"ARMM-{DateTime.UtcNow:yyyyMMddhhmmss}";
        public static string JobFileNumber => $"JF-{DateTime.UtcNow:yyyyMMddhhmmss}";
        private static string GenerateRandomInt()
        {
            uint randomvalue;

            using (var crypto =  RandomNumberGenerator.Create() )
            {
                byte[] val = new byte[10];
                crypto.GetBytes(val);
                randomvalue = BitConverter.ToUInt32(val, 1);
            }

            return randomvalue.ToString();
        }
    }
}
