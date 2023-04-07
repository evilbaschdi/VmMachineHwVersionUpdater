using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using VmMachineHwVersionUpdater.Core;
using VmMachineHwVersionUpdater.Core.Models;

namespace VmMachineHwVersionUpdater.Resources;

/// <inheritdoc />
[ValueConversion(typeof(ReadOnlyObservableCollection<object>), typeof(string))]
public sealed class DirectorySizeConverter : IValueConverter
{
    /// <summary>
    ///     Gets a static default instance of <see cref="DirectorySizeConverter" />.
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public static readonly DirectorySizeConverter Instance = new();

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
        ArgumentNullException.ThrowIfNull(value);

        ArgumentNullException.ThrowIfNull(targetType);

        ArgumentNullException.ThrowIfNull(parameter);

        ArgumentNullException.ThrowIfNull(culture);

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
        ArgumentNullException.ThrowIfNull(value);

        ArgumentNullException.ThrowIfNull(targetType);

        ArgumentNullException.ThrowIfNull(parameter);

        ArgumentNullException.ThrowIfNull(culture);

        throw new NotImplementedException();
    }
}