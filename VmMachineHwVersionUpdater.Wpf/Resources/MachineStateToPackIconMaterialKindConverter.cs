using System.Globalization;
using System.Windows.Data;
using MahApps.Metro.IconPacks;
using VmMachineHwVersionUpdater.Core.Enums;

namespace VmMachineHwVersionUpdater.Wpf.Resources;

/// <inheritdoc />
[ValueConversion(typeof(MachineState), typeof(PackIconMaterialKind))]
public sealed class MachineStateToPackIconMaterialKindConverter : IValueConverter
{
    /// <summary>
    ///     Gets a static default instance of <see cref="MachineStateToPackIconMaterialKindConverter" />.
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public static readonly MachineStateToPackIconMaterialKindConverter Instance = new();

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
        ArgumentNullException.ThrowIfNull(value);

        ArgumentNullException.ThrowIfNull(targetType);

        ArgumentNullException.ThrowIfNull(parameter);

        ArgumentNullException.ThrowIfNull(culture);

        throw new NotImplementedException();
    }
}