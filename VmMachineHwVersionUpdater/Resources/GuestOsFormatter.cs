using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using JetBrains.Annotations;

namespace VmMachineHwVersionUpdater.Resources;

/// <inheritdoc />
public class GuestOsFormatter : IValueConverter
{
    /// <summary>
    ///     Replaces "' " with "'\r\n"
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

        var guestOsFormatter = (string)value;

        return guestOsFormatter.Replace("' ", $"'{Environment.NewLine}");
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