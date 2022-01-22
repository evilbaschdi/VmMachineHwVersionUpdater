using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using JetBrains.Annotations;
using VmMachineHwVersionUpdater.Core;
using VmMachineHwVersionUpdater.Core.Models;

namespace VmMachineHwVersionUpdater.Resources;

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

        if (value == DependencyProperty.UnsetValue)
        {
            return DependencyProperty.UnsetValue;
        }

        var collection = (ReadOnlyObservableCollection<object>)value;
        var precision = System.Convert.ToInt32(parameter);
        var updateEntries = collection.Cast<Machine>();

        var sum = updateEntries.Sum(machine => machine.DirectorySizeGb);
        var result = sum.GiBiBytesToKiBiBytes();

        return result.ToFileSize(precision, culture);
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