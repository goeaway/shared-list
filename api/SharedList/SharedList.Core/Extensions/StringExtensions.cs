using System;
using System.Collections.Generic;
using System.Text;

namespace SharedList.Core.Extensions
{
    public static class StringExtensions
    {
        public static string Capitalise(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return text;
            }

            if (text.Length == 1)
            {
                return char.ToUpper(text[0]) + "";
            }

            return char.ToUpper(text[0]) + text.Substring(1, text.Length - 1);
        }
    }
}
