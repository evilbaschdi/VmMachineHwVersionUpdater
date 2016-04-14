using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using VmMachineHwVersionUpdater.Annotations;

namespace VmMachineHwVersionUpdater.Internal
{
    /// <summary>
    /// </summary>
    public class FileLocked
    {
        //http://dotnet-snippets.de/snippet/ermitteln-des-prozesses-der-eine-datei-gesperrt-hat-bzw-geoeffnet/10006

        /// <summary>
        ///     Return a list of processes that hold on the given file.
        /// </summary>
        public static List<Process> GetProcessesLockingFile(string filePath)
        {
            return (from process in Process.GetProcesses()
                let files = GetFilesLockedBy(process)
                where files.Contains(filePath)
                select process).ToList();
        }

        /// <summary>
        ///     Return a list of file locks held by the process.
        /// </summary>
        public static List<string> GetFilesLockedBy(Process process)
        {
            var outp = new List<string>();

            ThreadStart ts = () =>
            {
                try
                {
                    outp = UnsafeGetFilesLockedBy(process);
                }
                catch(Exception exception)
                {
                    MessageBox.Show("GetFilesLockedBy 1: " + exception.Message);
                }
            };

            try
            {
                dynamic t = new Thread(ts);
                t.IsBackground = true;
                t.Start();
                t.Join();
                //If Not t.Join(250) Then
                //    Try
                //        t.Abort()
                //    Catch
                //    End Try
                //End If
            }
            catch(Exception exception)
            {
                MessageBox.Show("GetFilesLockedBy 2: " + exception.Message);
            }

            return outp;
        }

        #region "Inner Workings"

        private static List<string> UnsafeGetFilesLockedBy(Process process)
        {
            try
            {
                var handles = GetHandles(process);

                return handles.Select(handle => GetFilePath(handle, process)).Where(file => file != null).ToList();
            }
            catch
            {
                return new List<string>();
            }
        }

        private const int CnstSystemHandleInformation = 16;
        private const uint StatusInfoLengthMismatch = 0xc0000004u;

        private static string GetFilePath(Win32Api.SystemHandleInformation sYstemHandleInformation, Process process)
        {
            if(sYstemHandleInformation.GrantedAccess == 0x12019f || sYstemHandleInformation.GrantedAccess == 0x1a019f || sYstemHandleInformation.GrantedAccess == 0x120189 ||
               sYstemHandleInformation.GrantedAccess == 0x100000)
            {
                return null;
            }

            var mIpProcessHwnd = Win32Api.OpenProcess(Win32Api.ProcessAccessFlags.All, false, process.Id);
            var ipHandle = IntPtr.Zero;
            var objBasic = new Win32Api.ObjectBasicInformation();
            var objObjectType = new Win32Api.ObjectTypeInformation();
            var objObjectName = new Win32Api.ObjectNameInformation();
            var strObjectName = "";
            var nLength = 0;
            var nReturn = 0;

            if(!Win32Api.DuplicateHandle(mIpProcessHwnd, sYstemHandleInformation.Handle, Win32Api.GetCurrentProcess(), ref ipHandle, 0, false, Win32Api.DuplicateSameAccess))
            {
                return null;
            }

            var ipBasic = Marshal.AllocHGlobal(Marshal.SizeOf(objBasic));
            Win32Api.NtQueryObject(ipHandle, Convert.ToInt32(Win32Api.ObjectInformationClass.ObjectBasicInformation), ipBasic, Marshal.SizeOf(objBasic), ref nLength);
            objBasic = (Win32Api.ObjectBasicInformation) Marshal.PtrToStructure(ipBasic, objBasic.GetType());
            Marshal.FreeHGlobal(ipBasic);

            var ipObjectType = Marshal.AllocHGlobal(objBasic.TypeInformationLength);
            nLength = objBasic.TypeInformationLength;
            while(
                Convert.ToUInt32(InlineAssignHelper(ref nReturn,
                    Win32Api.NtQueryObject(ipHandle, Convert.ToInt32(Win32Api.ObjectInformationClass.ObjectTypeInformation), ipObjectType, nLength, ref nLength))) ==
                Win32Api.StatusInfoLengthMismatch)
            {
                Marshal.FreeHGlobal(ipObjectType);
                ipObjectType = Marshal.AllocHGlobal(nLength);
            }

            objObjectType = (Win32Api.ObjectTypeInformation) Marshal.PtrToStructure(ipObjectType, objObjectType.GetType());
            var ipTemp = Is64Bits() ? new IntPtr(Convert.ToInt64(objObjectType.Name.Buffer.ToString(), 10) >> 32) : objObjectType.Name.Buffer;

            var strObjectTypeName = Marshal.PtrToStringUni(ipTemp, objObjectType.Name.Length >> 1);
            Marshal.FreeHGlobal(ipObjectType);
            if(strObjectTypeName != "File")
            {
                return null;
            }

            //nLength = objBasic.NameInformationLength
            nLength = 0x1000;
            var ipObjectName = Marshal.AllocHGlobal(nLength);
            while(
                Convert.ToUInt32(InlineAssignHelper(ref nReturn,
                    Win32Api.NtQueryObject(ipHandle, Convert.ToInt32(Win32Api.ObjectInformationClass.ObjectNameInformation), ipObjectName, nLength, ref nLength))) ==
                Win32Api.StatusInfoLengthMismatch)
            {
                Marshal.FreeHGlobal(ipObjectName);
                ipObjectName = Marshal.AllocHGlobal(nLength);
            }
            objObjectName = (Win32Api.ObjectNameInformation) Marshal.PtrToStructure(ipObjectName, objObjectName.GetType());

            ipTemp = Is64Bits() ? new IntPtr(Convert.ToInt64(objObjectName.Name.Buffer.ToString(), 10) >> 32) : objObjectName.Name.Buffer;

            if(ipTemp != IntPtr.Zero)
            {
                var baTemp = new byte[nLength];
                try
                {
                    Marshal.Copy(ipTemp, baTemp, 0, nLength);

                    strObjectName = Marshal.PtrToStringUni(Is64Bits() ? new IntPtr(ipTemp.ToInt64()) : new IntPtr(ipTemp.ToInt32()));
                }
                catch(AccessViolationException)
                {
                    return null;
                }
                finally
                {
                    Marshal.FreeHGlobal(ipObjectName);
                    Win32Api.CloseHandle(ipHandle);
                }
            }

            var path = GetRegularFileNameFromDevice(strObjectName);
            try
            {
                return path;
            }
            catch
            {
                return null;
            }
        }

        private static string GetRegularFileNameFromDevice(string strRawName)
        {
            var strFileName = strRawName;
            foreach(var strDrivePath in Environment.GetLogicalDrives())
            {
                var sbTargetPath = new StringBuilder(Win32Api.MaxPath);
                if(Win32Api.QueryDosDevice(strDrivePath.Substring(0, 2), sbTargetPath, Win32Api.MaxPath) == 0)
                {
                    return strRawName;
                }
                var strTargetPath = sbTargetPath.ToString();
                if(!strFileName.StartsWith(strTargetPath))
                {
                    continue;
                }
                strFileName = strFileName.Replace(strTargetPath, strDrivePath.Substring(0, 2));
                break; // TODO: might not be correct. Was : Exit For
            }
            return strFileName;
        }

        private static List<Win32Api.SystemHandleInformation> GetHandles(Process process)
        {
            uint nStatus = 0;
            var nHandleInfoSize = 0x10000;
            var ipHandlePointer = Marshal.AllocHGlobal(nHandleInfoSize);
            var nLength = 0;
            IntPtr ipHandle;

            while((InlineAssignHelper(ref nStatus, Win32Api.NtQuerySystemInformation(CnstSystemHandleInformation, ipHandlePointer, nHandleInfoSize, ref nLength))) ==
                  StatusInfoLengthMismatch)
            {
                nHandleInfoSize = nLength;
                Marshal.FreeHGlobal(ipHandlePointer);
                ipHandlePointer = Marshal.AllocHGlobal(nLength);
            }

            var baTemp = new byte[nLength];
            Marshal.Copy(ipHandlePointer, baTemp, 0, nLength);

            long lHandleCount;
            if(Is64Bits())
            {
                lHandleCount = Marshal.ReadInt64(ipHandlePointer);
                ipHandle = new IntPtr(ipHandlePointer.ToInt64() + 8);
            }
            else
            {
                lHandleCount = Marshal.ReadInt32(ipHandlePointer);
                ipHandle = new IntPtr(ipHandlePointer.ToInt32() + 4);
            }

            var lstHandles = new List<Win32Api.SystemHandleInformation>();

            for(long lIndex = 0; lIndex <= lHandleCount - 1; lIndex++)
            {
                var shHandle = new Win32Api.SystemHandleInformation();
                if(Is64Bits())
                {
                    shHandle = (Win32Api.SystemHandleInformation) Marshal.PtrToStructure(ipHandle, shHandle.GetType());
                    ipHandle = new IntPtr(ipHandle.ToInt64() + Marshal.SizeOf(shHandle) + 8);
                }
                else
                {
                    ipHandle = new IntPtr(ipHandle.ToInt64() + Marshal.SizeOf(shHandle));
                    shHandle = (Win32Api.SystemHandleInformation) Marshal.PtrToStructure(ipHandle, shHandle.GetType());
                }
                if(shHandle.ProcessID != process.Id)
                {
                    continue;
                }
                lstHandles.Add(shHandle);
            }
            return lstHandles;
        }

        private static bool Is64Bits()
        {
            return Marshal.SizeOf(typeof(IntPtr)) == 8;
        }

        internal class Win32Api
        {
            [DllImport("ntdll.dll")]
            public static extern int NtQueryObject(IntPtr objectHandle, int objectInformationClass, IntPtr objectInformation, int objectInformationLength, ref int returnLength);

            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern uint QueryDosDevice(string lpDeviceName, StringBuilder lpTargetPath, int ucchMax);

            [DllImport("ntdll.dll")]
            public static extern uint NtQuerySystemInformation(int systemInformationClass, IntPtr systemInformation, int systemInformationLength, ref int returnLength);

            [DllImport("kernel32.dll")]
            public static extern IntPtr OpenProcess(ProcessAccessFlags dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, int dwProcessId);

            [DllImport("kernel32.dll")]
            public static extern int CloseHandle(IntPtr hObject);

            [DllImport("kernel32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool DuplicateHandle(IntPtr hSourceProcessHandle, ushort hSourceHandle, IntPtr hTargetProcessHandle, ref IntPtr lpTargetHandle,
                uint dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, uint dwOptions);

            [DllImport("kernel32.dll")]
            public static extern IntPtr GetCurrentProcess();

            public enum ObjectInformationClass
            {
                ObjectBasicInformation = 0,
                ObjectNameInformation = 1,
                ObjectTypeInformation = 2,
                ObjectAllTypesInformation = 3,
                ObjectHandleInformation = 4
            }

            [Flags()]
            public enum ProcessAccessFlags : uint
            {
                All = 0x1f0fff,
                Terminate = 0x1,
                CreateThread = 0x2,
                VmOperation = 0x8,
                VmRead = 0x10,
                VmWrite = 0x20,
                DupHandle = 0x40,
                SetInformation = 0x200,
                QueryInformation = 0x400,
                Synchronize = 0x100000
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct ObjectBasicInformation
            {
                // Information Class 0
                public int Attributes;

                public int GrantedAccess;
                public int HandleCount;
                public int PointerCount;
                public int PagedPoolUsage;
                public int NonPagedPoolUsage;
                public int Reserved1;
                public int Reserved2;
                public int Reserved3;
                public int NameInformationLength;
                public int TypeInformationLength;
                public int SecurityDescriptorLength;
                public System.Runtime.InteropServices.ComTypes.FILETIME CreateTime;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct ObjectTypeInformation
            {
                // Information Class 2
                public UnicodeString Name;

                public int ObjectCount;
                public int HandleCount;
                public int Reserved1;
                public int Reserved2;
                public int Reserved3;
                public int Reserved4;
                public int PeakObjectCount;
                public int PeakHandleCount;
                public int Reserved5;
                public int Reserved6;
                public int Reserved7;
                public int Reserved8;
                public int InvalidAttributes;
                public GenericMapping GenericMapping;
                public int ValidAccess;
                public byte Unknown;
                public byte MaintainHandleDatabase;
                public int PoolType;
                public int PagedPoolUsage;
                public int NonPagedPoolUsage;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct ObjectNameInformation
            {
                // Information Class 1
                public UnicodeString Name;
            }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct UnicodeString
            {
                public ushort Length;
                public ushort MaximumLength;
                public IntPtr Buffer;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct GenericMapping
            {
                public int GenericRead;
                public int GenericWrite;
                public int GenericExecute;
                public int GenericAll;
            }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct SystemHandleInformation
            {
                // Information Class 16
                public int ProcessID;

                public byte ObjectTypeNumber;
                public byte Flags;

                // 0x01 = PROTECT_FROM_CLOSE, 0x02 = INHERIT
                public ushort Handle;

                public int Object_Pointer;
                public UInt32 GrantedAccess;
            }

            public const int MaxPath = 260;
            public const uint StatusInfoLengthMismatch = 0xc0000004u;
            public const int DuplicateSameAccess = 0x2;
        }

        private static T InlineAssignHelper<T>([NotNull] ref T target, T value)
        {
            if(target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }
            target = value;
            return value;
        }

        #endregion "Inner Workings"
    }
}