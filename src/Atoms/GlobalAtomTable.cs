namespace Vurdalakov
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    public static class GlobalAtomTable
    {
        static public String GetRegisteredFormatName(UInt16 format)
        {
            var stringBuilder = new StringBuilder(256);
            return GetClipboardFormatName(format, stringBuilder, stringBuilder.Capacity) > 0 ? stringBuilder.ToString() : null;
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern Int32 GetClipboardFormatName(UInt32 format, StringBuilder lpszFormatName, Int32 cchMaxCount);
    }
}
