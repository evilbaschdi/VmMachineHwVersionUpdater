using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EvilBaschdi.Core.Internal;
using EvilBaschdi.Core.Model;
using JetBrains.Annotations;
using VmMachineHwVersionUpdater.Core.Models;
using VmMachineHwVersionUpdater.Core.PerMachine;
using VmMachineHwVersionUpdater.Core.Settings;

namespace VmMachineHwVersionUpdater.Core.BasicApplication;

/// <inheritdoc />
public class MachinesFromPath : IMachinesFromPath
{
    private readonly IFileListFromPath _fileListFromPath;
    private readonly IHandleMachineFromPath _handleMachineFromPath;
    private readonly IPathSettings _pathSettings;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="pathSettings"></param>
    /// <param name="handleMachineFromPath"></param>
    /// <param name="fileListFromPath"></param>
    public MachinesFromPath([NotNull] IPathSettings pathSettings, [NotNull] IHandleMachineFromPath handleMachineFromPath, [NotNull] IFileListFromPath fileListFromPath)
    {
        _pathSettings = pathSettings ?? throw new ArgumentNullException(nameof(pathSettings));
        _handleMachineFromPath = handleMachineFromPath ?? throw new ArgumentNullException(nameof(handleMachineFromPath));
        _fileListFromPath = fileListFromPath ?? throw new ArgumentNullException(nameof(fileListFromPath));
    }

    /// <inheritdoc />
    public List<Machine> Value
    {
        get
        {
            var machineList = new ConcurrentBag<Machine>();
            var machinePaths = _pathSettings.VmPool;
            var archivePaths = _pathSettings.ArchivePath;
            var filterExtensionsToEqual = new List<string>
                                          {
                                              "vmx"
                                          };

            var fileListFromPathFilter = new FileListFromPathFilter
                                         {
                                             FilterExtensionsToEqual = filterExtensionsToEqual
                                         };

            machinePaths.AddRange(archivePaths);

            foreach (var path in machinePaths)
            {
                if (!Directory.Exists(path))
                {
                    continue;
                }

                var fileList = _fileListFromPath.ValueFor(path, fileListFromPathFilter);
                Parallel.ForEach(fileList,
                    file =>
                    {
                        var machine = _handleMachineFromPath.ValueFor(path, file);
                        if (machine == null)
                        {
                            return;
                        }

                        machineList.Add(machine);
                    });
            }

            return machineList.ToList();
        }
    }
}