using System.Diagnostics;
using EvilBaschdi.Core.AppHelpers;

namespace VmMachineHwVersionUpdater.Core.Commands;

/// <inheritdoc />
public class ReloadCommand : IReloadCommand
{
    private readonly IProcessByPath _processByPath;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="processByPath"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public ReloadCommand([NotNull] IProcessByPath processByPath)
    {
        _processByPath = processByPath ?? throw new ArgumentNullException(nameof(processByPath));
    }

    /// <inheritdoc />
    public void Run()
    {
        var app = Process.GetCurrentProcess().MainModule?.FileName;
        var process = _processByPath.ValueFor(app);
        process.Start();
        process.WaitForInputIdle();
    }
}