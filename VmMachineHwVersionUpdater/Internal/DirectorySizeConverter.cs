using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using VmMachineHwVersionUpdater.Core;
using VmMachineHwVersionUpdater.Models;

namespace VmMachineHwVersionUpdater.Internal
{
    /// <inheritdoc />
    public class DirectorySizeConverter : IValueConverter
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
            if (value == DependencyProperty.UnsetValue)
            {
                return DependencyProperty.UnsetValue;
            }

            var collection = (ReadOnlyObservableCollection<object>) value;


            var updateEntries = collection?.Cast<Machine>();

            var sum = updateEntries?.Sum(machine => machine.DirectorySizeGb);
            var result = sum.GiBiBytesToKiBiBytes();

            return result.ToFileSize(2, CultureInfo.GetCultureInfo(1033));
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