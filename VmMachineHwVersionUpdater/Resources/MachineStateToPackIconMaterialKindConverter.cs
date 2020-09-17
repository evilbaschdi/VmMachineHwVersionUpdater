using System;
using System.Globalization;
using System.Windows.Data;
using MahApps.Metro.IconPacks;
using VmMachineHwVersionUpdater.Core.Enums;

namespace VmMachineHwVersionUpdater.Resources
{
    /// <inheritdoc />
    public class MachineStateToPackIconMaterialKindConverter : IValueConverter
    {
        /// <summary>
        ///     Calculates directory size of the current group and returns its string value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null && (MachineState) value == MachineState.Paused ? PackIconMaterialKind.Pause : PackIconMaterialKind.Power;
        }

        /// <summary>
        ///     does nothing
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}