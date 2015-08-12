namespace Vurdalakov
{
    using System;
    using System.ComponentModel;
    using System.Runtime.InteropServices;
    using System.Text;

    public static class AtomTable
    {
        public static UInt16 GlobalAdd(String name)
        {
            var atom = GlobalAddAtom(name);
            ThrowIfFailed(0 == atom, "GlobalAddAtom");
            return atom;
        }

        public static void GlobalDelete(UInt16 atom)
        {
            SetLastError(0);
            GlobalDeleteAtom(atom);
            ThrowIfFailed(Marshal.GetLastWin32Error() != 0, "GlobalDeleteAtom");
        }

        public static UInt16 GlobalFind(String name)
        {
            var atom = GlobalFindAtom(name);
            ThrowIfFailed((0 == atom) && (Marshal.GetLastWin32Error() != 2), "GlobalFindAtom");
            return atom;
        }

        public static String GlobalGetName(UInt16 atom)
        {
            var name = new StringBuilder(514);
            var failure = 0 == GlobalGetAtomName(atom, name, name.Capacity);
            ThrowIfFailed(failure && (Marshal.GetLastWin32Error() != 6), "GlobalGetAtomName");
            return failure ? null : name.ToString();
        }

        public static UInt16 NtAdd(String name)
        {
            UInt16 atom = 0;
            ThrowIfFailed(NtAddAtom(name, (UInt32)name.Length * 2, ref atom), "NtAddAtom");
            return atom;
        }

        public static void NtDelete(UInt16 atom)
        {
            ThrowIfFailed(NtDeleteAtom(atom), "NtDeleteAtom");
        }

        public static UInt16 NtFind(String name)
        {
            UInt16 atom = 0;
            var ntstatus = NtFindAtom(name, (UInt32)name.Length * 2, ref atom);
            if (0xC0000034 == ntstatus)
            {
                return 0;
            }
            ThrowIfFailed(ntstatus, "NtFindAtom");
            return atom;
        }

        public class AtomBasicInformation
        {
            public UInt16 Atom { get; private set; }
            public UInt16 ReferenceCount { get; private set; }
            public UInt16 Pinned { get; private set; }
            public String Name { get; private set; }

            public AtomBasicInformation(UInt16 atom, UInt16 referenceCount, UInt16 pinned, String name)
            {
                Atom = atom;
                ReferenceCount = referenceCount;
                Pinned = pinned;
                Name = name;
            }
        }

        public static AtomBasicInformation NtQueryBasicInformation(UInt16 atom)
        {
            var atomBasicInformation = new ATOM_BASIC_INFORMATION();

            var size = Marshal.SizeOf(atomBasicInformation);

            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(atomBasicInformation, ptr, false);

            UInt32 returnLength = 0;
            var ntstatus = NtQueryInformationAtom(atom, 0, ptr, (UInt32)size, ref returnLength);
            if (0xC0000008 == ntstatus)
            {
                return null;
            }
            ThrowIfFailed(ntstatus, "NtQueryInformationAtom");

            atomBasicInformation = (ATOM_BASIC_INFORMATION)Marshal.PtrToStructure(ptr, typeof(ATOM_BASIC_INFORMATION));

            var bytes = new Byte[1024];
            Marshal.Copy(ptr, bytes, 0, size);

            Marshal.FreeHGlobal(ptr);

            return new AtomBasicInformation(atom, atomBasicInformation.ReferenceCount, atomBasicInformation.Pinned, atomBasicInformation.Name);
        }

        public static UInt16 UserAdd(String name)
        {
            var atom = RegisterClipboardFormat(name);
            ThrowIfFailed(0 == atom, "RegisterClipboardFormat");
            return (UInt16)atom;
        }

        public static String UserGetName(UInt16 format)
        {
            var stringBuilder = new StringBuilder(256);
            return GetClipboardFormatName(format, stringBuilder, stringBuilder.Capacity) > 0 ? stringBuilder.ToString() : null;
        }

        private static void ThrowIfFailed(Boolean failed, String functionName)
        {
            if (failed)
            {
                Throw(Marshal.GetLastWin32Error(), functionName);
            }
        }

        private static void ThrowIfFailed(UInt32 ntstatus, String functionName)
        {
            if (ntstatus != 0)
            {
                Throw((Int32)ntstatus, functionName);
            }
        }

        private static void Throw(Int32 error, String functionName)
        {
            var message = String.Format("Function {0}() failed with error {1} (0x{1:X8}).", functionName, error);
            throw new Win32Exception(error, message);
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

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern UInt16 GlobalAddAtom(String lpString);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern UInt16 GlobalDeleteAtom(UInt16 nAtom);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern UInt16 GlobalFindAtom(String lpString);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern UInt32 GlobalGetAtomName(UInt16 nAtom, StringBuilder lpBuffer, int nSize);

        [DllImport("ntdll.dll", CharSet = CharSet.Unicode)]
        private static extern UInt32 NtAddAtom(String name, UInt32 length, ref UInt16 atom);

        [DllImport("ntdll.dll")]
        private static extern UInt32 NtDeleteAtom(UInt16 atom);

        [DllImport("ntdll.dll", CharSet = CharSet.Unicode)]
        private static extern UInt32 NtFindAtom(String name, UInt32 length, ref UInt16 atom);

        [DllImport("ntdll.dll")]
        private static extern UInt32 NtQueryInformationAtom(UInt16 atom, Int32 atomInformationClass, IntPtr atomInformation, UInt32 atomInformationLength, ref UInt32 returnLength);

        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Auto)]
        private struct ATOM_BASIC_INFORMATION
        {
            public UInt16 ReferenceCount;
            public UInt16 Pinned;
            public UInt16 NameLength;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public String Name;
        }

        [DllImport("win32kbase.sys", CharSet = CharSet.Unicode)]
        private static extern UInt16 UserAddAtom(String name, Boolean pin);

        [DllImport("win32kbase.sys")]
        private static extern UInt16 UserDeleteAtom(UInt16 nAtom);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern Int32 RegisterClipboardFormat(String lpszFormat);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern Int32 GetClipboardFormatName(UInt32 format, StringBuilder lpszFormatName, Int32 cchMaxCount);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern void SetLastError(UInt32 dwErrCode);
    }
}
