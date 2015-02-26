namespace Vurdalakov
{
    using System;
    using System.Text;

    public static class Base64Converter
    {
        public static String Encode(String text)
        {
            var bytes = Encoding.UTF8.GetBytes(text);
            return Convert.ToBase64String(bytes);
        }

        public static String Decode(String text)
        {
            var bytes = Convert.FromBase64String(text);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
