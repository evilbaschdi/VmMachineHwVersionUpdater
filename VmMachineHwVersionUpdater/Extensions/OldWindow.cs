using System;
using System.Windows.Forms;

namespace VmMachineHwVersionUpdater.Extensions
{
    /// <summary>
    /// </summary>
    public class OldWindow : IWin32Window
    {
        private readonly IntPtr _handle;

        /// <summary>
        /// </summary>
        public OldWindow(IntPtr handle)
        {
            _handle = handle;
        }

        IntPtr IWin32Window.Handle => _handle;
    }
}