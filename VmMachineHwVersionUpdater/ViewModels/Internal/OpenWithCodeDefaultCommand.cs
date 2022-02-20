using System.IO;
using EvilBaschdi.CoreExtended.AppHelpers;
using EvilBaschdi.CoreExtended.Mvvm.ViewModel.Command;
using JetBrains.Annotations;
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
    public DefaultCommand Value => new()
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