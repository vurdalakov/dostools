namespace Vurdalakov
{
    using System;
    using System.ComponentModel;
    using System.Runtime.InteropServices;
    using System.Text;

    public static class GlobalAtomTable
    {
        static public String GetRegisteredFormatName(UInt16 format)
        {
            var stringBuilder = new StringBuilder(256);
            return GetClipboardFormatName(format, stringBuilder, stringBuilder.Capacity) > 0 ? stringBuilder.ToString() : null;
        }

        static public UInt16 Add(String name)
        {
            UInt16 atom = 0;
            ThrowIfFailed(NtAddAtom(name, (UInt32)name.Length, ref atom), "NtAddAtom");
            return atom;
        }

        static public UInt16 Find(String name)
        {
            UInt16 atom = 0;
            ThrowIfFailed(NtFindAtom(name, (UInt32)name.Length, ref atom), "NtFindAtom");
            return atom;
        }

        static public void Delete(UInt16 atom)
        {
            ThrowIfFailed(NtDeleteAtom(atom), "NtDeleteAtom");
        }

        static public ATOM_BASIC_INFORMATION QueryBasicInformation(UInt16 atom)
        {
            var atomBasicInformation = new ATOM_BASIC_INFORMATION();

            var size = Marshal.SizeOf(atomBasicInformation);

            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(atomBasicInformation, ptr, false);

            UInt32 returnLength = 0;
            ThrowIfFailed(NtQueryInformationAtom(atom, 0, ptr, (UInt32)size, ref returnLength), "NtQueryInformationAtom");

            atomBasicInformation = (ATOM_BASIC_INFORMATION)Marshal.PtrToStructure(ptr, typeof(ATOM_BASIC_INFORMATION));

            var bytes = new Byte[1024];
            Marshal.Copy(ptr, bytes, 0, size);

            Marshal.FreeHGlobal(ptr);

            return atomBasicInformation;
        }

        private static void ThrowIfFailed(UInt32 ntstatus, String functionName)
        {
            if (ntstatus != 0)
            {
                var message = String.Format("Function {0}() failed with error {1} (0x{1:X8}).", functionName, ntstatus);

                throw new Win32Exception((Int32)ntstatus, message);
            }
        }

        //private Int32 NtStatus2Win32Error(UInt32 ntstatus)
        //{
        //        DWORD oldError;
        //        DWORD result;
        //        DWORD br;
        //        OVERLAPPED o;

        //        o.Internal = ntstatus;
        //        o.InternalHigh = 0;
        //        o.Offset = 0;
        //        o.OffsetHigh = 0;
        //        o.hEvent = 0;
        //        oldError = GetLastError();
        //        GetOverlappedResult(NULL, &o, &br, FALSE);
        //        result = GetLastError();
        //        SetLastError(oldError);
        //        return result;
        //}

        [DllImport("user32.dll", SetLastError = true)]
        private static extern Int32 GetClipboardFormatName(UInt32 format, StringBuilder lpszFormatName, Int32 cchMaxCount);

        [DllImport("ntdll.dll", CharSet = CharSet.Unicode)]
        private static extern UInt32 NtAddAtom(String name, UInt32 length, ref UInt16 atom);

        [DllImport("ntdll.dll", CharSet = CharSet.Unicode)]
        private static extern UInt32 NtFindAtom(String name, UInt32 length, ref UInt16 atom);

        [DllImport("ntdll.dll")]
        private static extern UInt32 NtDeleteAtom(UInt16 atom);

        [DllImport("ntdll.dll")]
        private static extern UInt32 NtQueryInformationAtom(UInt16 atom, Int32 atomInformationClass, IntPtr atomInformation, UInt32 atomInformationLength, ref UInt32 returnLength);

        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Auto)]
        public struct ATOM_BASIC_INFORMATION
        {
            public UInt16 ReferenceCount;
            public UInt16 Pinned;
            public UInt16 NameLength;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public String Name;
        }
    }
}
