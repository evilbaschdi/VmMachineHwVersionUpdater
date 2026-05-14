using System.ComponentModel;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;
using FluentAvalonia.UI.Controls;

namespace VmMachineHwVersionUpdater.Avalonia.Converters;

/// <inheritdoc />
public class MachineStateConverter : IValueConverter
{
    /// <inheritdoc />
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not MachineState machineState)
        {
            return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
        }

        return machineState switch
        {
            MachineState.Off => FASymbol.PlayFilled,
            MachineState.Paused => FASymbol.PauseFilled,
            // converter used for the wrong type
            _ => new BindingNotification(new InvalidEnumArgumentException(), BindingErrorType.Error),
        };
    }

    /// <inheritdoc />
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}