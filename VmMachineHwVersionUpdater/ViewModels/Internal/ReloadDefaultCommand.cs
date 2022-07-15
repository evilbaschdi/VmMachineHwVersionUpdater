using System.Windows;
using EvilBaschdi.Core.AppHelpers;
using EvilBaschdi.CoreExtended.Mvvm.ViewModel.Command;
using MahApps.Metro.Controls.Dialogs;

namespace VmMachineHwVersionUpdater.ViewModels.Internal;

/// <inheritdoc />
public class ReloadDefaultCommand : IReloadDefaultCommand
{
    [NotNull] private readonly IDialogCoordinator _instance;
    private readonly IProcessByPath _processByPath;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="instance"></param>
    /// <param name="processByPath"></param>
    public ReloadDefaultCommand([NotNull] IDialogCoordinator instance, [NotNull] IProcessByPath processByPath)
    {
        _instance = instance ?? throw new ArgumentNullException(nameof(instance));
        _processByPath = processByPath ?? throw new ArgumentNullException(nameof(processByPath));
    }

    /// <inheritdoc />
    public DefaultCommand DefaultCommandValue
    {
        get
        {
            async void Execute(object _) => await Value();

            return new()
                   {
                       Command = new RelayCommand(Execute)
                   };
        }
    }

    /// <inheritdoc />
    public async Task Value()
    {
        var controller = await _instance.ShowProgressAsync(DialogCoordinatorContext, "Application is restarting", "Please wait...");
        controller.SetIndeterminate();
        if (controller.IsOpen)
        {
            await Task.Run(() =>
                           {
                               var app = Application.ResourceAssembly.Location.Replace("dll", "exe");
                               var process = _processByPath.ValueFor(app);
                               process.Start();
                               process.WaitForInputIdle();
                           });

            await controller.CloseAsync();
            Application.Current.MainWindow?.Close();
        }
    }

    /// <inheritdoc />
    public object DialogCoordinatorContext { get; set; }
}