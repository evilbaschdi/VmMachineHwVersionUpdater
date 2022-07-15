using System.IO;
using EvilBaschdi.Core.AppHelpers;
using EvilBaschdi.CoreExtended.Mvvm.ViewModel.Command;
using VmMachineHwVersionUpdater.Core.Models;

namespace VmMachineHwVersionUpdater.ViewModels.Internal;

/// <inheritdoc />
public class OpenWithCodeDefaultCommand : IOpenWithCodeDefaultCommand
{
    private readonly ICurrentItem _currentItem;
    private readonly IProcessByPath _processByPath;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="currentItem"></param>
    /// <param name="processByPath"></param>
    public OpenWithCodeDefaultCommand([NotNull] ICurrentItem currentItem, [NotNull] IProcessByPath processByPath)
    {
        _currentItem = currentItem ?? throw new ArgumentNullException(nameof(currentItem));
        _processByPath = processByPath ?? throw new ArgumentNullException(nameof(processByPath));
    }

    /// <inheritdoc />
    public DefaultCommand DefaultCommandValue => new()
                                                 {
                                                     Command = new RelayCommand(_ => Run())
                                                 };

    /// <inheritdoc />
    public void Run()
    {
        if (!File.Exists(_currentItem.Value.Path))
        {
            return;
        }

        _processByPath.RunFor($"vscode://file/{_currentItem.Value.Path}");
    }
}