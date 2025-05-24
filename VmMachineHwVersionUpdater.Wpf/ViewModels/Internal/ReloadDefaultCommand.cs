using System.Windows;
using MahApps.Metro.Controls.Dialogs;
using VmMachineHwVersionUpdater.Core.Commands;

namespace VmMachineHwVersionUpdater.Wpf.ViewModels.Internal;

/// <inheritdoc />
public class ReloadDefaultCommand(
    [NotNull] IDialogCoordinator instance,
    [NotNull] IReloadCommand reloadCommand) : IReloadDefaultCommand
{
    private readonly IDialogCoordinator _instance = instance ?? throw new ArgumentNullException(nameof(instance));
    private readonly IReloadCommand _reloadCommand = reloadCommand ?? throw new ArgumentNullException(nameof(reloadCommand));

    /// <inheritdoc />
    public DefaultCommand DefaultCommandValue
    {
        get
        {
            return new()
                   {
                       Command = new RelayCommand(Execute)
                   };

            
            async void Execute(object _) => await RunAsync();
        }
    }

    /// <inheritdoc />
    public async Task RunAsync()
    {
        var controller = await _instance.ShowProgressAsync(DialogCoordinatorContext, "Application is restarting", "Please wait...");
        controller.SetIndeterminate();
        if (controller.IsOpen)
        {
            await Task.Run(_reloadCommand.Run);

            await controller.CloseAsync();
            Application.Current.MainWindow?.Close();
        }
    }

    /// <inheritdoc />
    public object DialogCoordinatorContext { get; set; }
}