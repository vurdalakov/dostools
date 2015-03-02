namespace Vurdalakov
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public static class Base64Converter
    {
        public static String Encode(String text, Int32 maxLineLength)
        {
            var bytes = Encoding.UTF8.GetBytes(text);
            text = Convert.ToBase64String(bytes);

            return maxLineLength > 0 ? String.Join(Environment.NewLine, text.Split(maxLineLength)) : text;
        }

        public static String Decode(String text)
        {
            var stringBuilder = new StringBuilder(text.Length);

            foreach (var c in text)
            {
                var n = (Int32)c;
                if ((43 == n) || ((n >= 47) && (n <= 57)) || ((n >= 65) && (n <= 90)) || ((n >= 97) && (n <= 122)) || (61 == n)) // + / 0-9 A-Z a-z =
                {
                    stringBuilder.Append(c);
                }
            }

            var bytes = Convert.FromBase64String(stringBuilder.ToString());
            return Encoding.UTF8.GetString(bytes);
        }

        // TODO: move to StringExtensions class
        private static String[] Split(this String s, Int32 maxLineLength)
        {
            var strings = new List<String>();

            var length = s.Length;

            for (var i = 0; i < length; i += maxLineLength)
            {
                strings.Add(s.Substring(i, Math.Min(maxLineLength, length - i)));
            }

            return strings.ToArray();
        }
    }
}
