namespace Vurdalakov
{
    using System;
    using System.Globalization;
    using System.Numerics;

    public static class BigInt
    {
        public static String FromHex(String hexString)
        {
            hexString = hexString.ToLower();

            if (hexString.StartsWith("0x") || hexString.StartsWith("&h"))
            {
                hexString = hexString.Substring(2);
            }

            var bigint = BigInteger.Parse(hexString, NumberStyles.HexNumber);
            return bigint.ToString("D");
        }

        public static String ToHex(String decString)
        {
            var bigint = BigInteger.Parse(decString, NumberStyles.Integer);
            return bigint.ToString("X");
        }
    }
}
