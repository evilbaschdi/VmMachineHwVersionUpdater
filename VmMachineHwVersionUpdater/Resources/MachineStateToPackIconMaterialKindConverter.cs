using System;
using System.Globalization;
using System.Windows.Data;
using JetBrains.Annotations;
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
        public object Convert(object value, [NotNull] Type targetType, object parameter, [NotNull] CultureInfo culture)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (targetType == null)
            {
                throw new ArgumentNullException(nameof(targetType));
            }

            if (parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            if (culture == null)
            {
                throw new ArgumentNullException(nameof(culture));
            }

            return (MachineState)value == MachineState.Paused ? PackIconMaterialKind.Pause : PackIconMaterialKind.Power;
        }

        /// <summary>
        ///     does nothing
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, [NotNull] Type targetType, object parameter, [NotNull] CultureInfo culture)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (targetType == null)
            {
                throw new ArgumentNullException(nameof(targetType));
            }

            if (parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            if (culture == null)
            {
                throw new ArgumentNullException(nameof(culture));
            }

            throw new NotImplementedException();
        }
    }
}