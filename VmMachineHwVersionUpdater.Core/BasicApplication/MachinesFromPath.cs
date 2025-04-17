using System.Collections.Concurrent;
using EvilBaschdi.Core.Extensions;
using EvilBaschdi.Core.Internal;
using EvilBaschdi.Core.Model;

namespace VmMachineHwVersionUpdater.Core.BasicApplication;

/// <inheritdoc />
public class MachinesFromPath(
    [NotNull] IPathSettings pathSettings,
    [NotNull] IHandleMachineFromPath handleMachineFromPath,
    [NotNull] IFileListFromPath fileListFromPath) : IMachinesFromPath
{
    private readonly IFileListFromPath _fileListFromPath = fileListFromPath ?? throw new ArgumentNullException(nameof(fileListFromPath));
    private readonly IHandleMachineFromPath _handleMachineFromPath = handleMachineFromPath ?? throw new ArgumentNullException(nameof(handleMachineFromPath));
    private readonly IPathSettings _pathSettings = pathSettings ?? throw new ArgumentNullException(nameof(pathSettings));

    /// <inheritdoc />
    public ConcurrentBag<Machine> Value
    {
        get
        {
            var machineBag = new ConcurrentBag<Machine>();
            var machinePoolPaths = _pathSettings.VmPool;
            var archivePoolPaths = _pathSettings.ArchivePath;
            var filterExtensionsToEqual = new List<string>
                                          {
                                              "vmx"
                                          };

            var fileListFromPathFilter = new FileListFromPathFilter
                                         {
                                             FilterExtensionsToEqual = filterExtensionsToEqual
                                         };

            machinePoolPaths.AddRange(archivePoolPaths);

            var existingMachinePoolPaths = machinePoolPaths.GetExistingDirectories();

            foreach (var machinePoolPath in existingMachinePoolPaths)
            {
                var fileList = _fileListFromPath.ValueFor(machinePoolPath, fileListFromPathFilter);
                Parallel.ForEach(fileList,
                    machineFilePath =>
                    {
                        var machinePath = new MachinePath
                                          {
                                              MachinePoolPath = machinePoolPath,
                                              MachineFilePath = machineFilePath,
                                          };
                        var machine = _handleMachineFromPath.ValueFor(machinePath);
                        if (machine == null)
                        {
                            return;
                        }

                        machineBag.Add(machine);
                    });
            }

            return machineBag;
        }
    }
}