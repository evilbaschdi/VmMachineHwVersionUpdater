using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using IWin32Window = System.Windows.Forms.IWin32Window;

namespace VmMachineHwVersionUpdater.Extensions
{
    /// <summary>
    /// </summary>
    public static class WpfExtensions
    {
        /// <summary>
        /// </summary>
        public static IWin32Window GetIWin32Window(this Visual visual)
        {
            var source = PresentationSource.FromVisual(visual) as HwndSource;
            IWin32Window win = new OldWindow(source.Handle);
            return win;
        }
    }
}