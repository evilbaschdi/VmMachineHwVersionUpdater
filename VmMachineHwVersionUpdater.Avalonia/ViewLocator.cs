using Avalonia.Controls;
using Avalonia.Controls.Templates;
using VmMachineHwVersionUpdater.Avalonia.ViewModels;
using VmMachineHwVersionUpdater.Avalonia.Views;

namespace VmMachineHwVersionUpdater.Avalonia;

/// <inheritdoc />
public class ViewLocator : IDataTemplate
{
    /// <inheritdoc />
    public Control Build([NotNull] object data)
    {
        ArgumentNullException.ThrowIfNull(data);
        
        return data switch
        {
            MainWindowViewModel => new MainWindow(),
            AddEditAnnotationDialogViewModel => new AddEditAnnotationDialog(),
            _ => new TextBlock { Text = "Not Found: " + data.GetType().FullName }
        };
    }

    /// <inheritdoc />
    public bool Match(object data)
    {
        ArgumentNullException.ThrowIfNull(data);
        return data is ViewModelBase;
    }
}