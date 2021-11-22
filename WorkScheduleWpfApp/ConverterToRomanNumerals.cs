using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkScheduleWpfApp
{
    internal static class ConverterToRomanNumerals
    {
        private static readonly Dictionary<int, string> dict;

        static ConverterToRomanNumerals()
        {
            dict = CreateDictionary();
        }
        private static Dictionary<int, string> CreateDictionary()
        {
            return new Dictionary<int, string>
            {
                [1] = "I",
                [2] = "II",
                [3] = "III",
                [4] = "IV",
                [5] = "V",
                [6] = "VI",
                [7] = "VII",
                [8] = "VIII",
                [9] = "IX",
                [10] = "X",
                [20] = "XX",
                [30] = "XXX",
                [40] = "XL",
                [50] = "L",
                [60] = "LX",
                [70] = "LXX",
                [80] = "LXXX",
                [90] = "XC",
                [100] = "C",
                [200] = "CC",
                [300] = "CCC",
                [400] = "CD",
                [500] = "D",
                [600] = "DC",
                [700] = "DCC",
                [800] = "DCCC",
                [900] = "CM",
                [1000] = "M",
                [2000] = "MM",
                [3000] = "MMM"
            };
        }

        public static string ConvertToRomanNumeral(this int number)
        {
            if (number < 1 || number > 3999)
            {
                throw new ArgumentException();
            }

            string result = string.Empty;
            int k = 1;

            while (number > 0)
            {
                int mod = number % 10;

                if (mod != 0)
                {
                    result = dict[mod * k] + result;
                }

                number /= 10;
                k *= 10;
            }

            return result;
        }
    }
}
