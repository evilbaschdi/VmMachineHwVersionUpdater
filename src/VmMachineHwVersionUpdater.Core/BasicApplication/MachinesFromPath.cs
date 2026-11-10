using System.Collections.Concurrent;
using System.Diagnostics;
using SWalter.Core.Extensions;
using SWalter.Core.Internal;
using SWalter.Core.Model;

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
            var filterExtensionsToEqual = (List<string>)["vmx", "vbox"];

            var fileListFromPathFilter = new FileListFromPathFilter
                                         {
                                             FilterExtensionsToEqual = filterExtensionsToEqual
                                         };

            machinePoolPaths.AddRange(archivePoolPaths);

            var existingMachinePoolPaths = machinePoolPaths.GetExistingDirectories();

            Parallel.ForEach(existingMachinePoolPaths,
                machinePoolPath =>
                {
                    IEnumerable<string> fileList;
                    try
                    {
                        fileList = _fileListFromPath.ValueFor(machinePoolPath, fileListFromPathFilter).ToArray();
                    }
                    catch (Exception exception) when (IsRecoverableLoadException(exception))
                    {
                        Trace.TraceError("Failed to enumerate VM files under '{0}': {1}", machinePoolPath, exception);
                        return;
                    }

                    Parallel.ForEach(fileList,
                        machineFilePath =>
                        {
                            try
                            {
                                var machinePath = new MachinePath
                                                  {
                                                      MachinePoolPath = machinePoolPath,
                                                      MachineFilePath = machineFilePath,
                                                  };
                                var machine = _handleMachineFromPath.ValueFor(machinePath);
                                if (machine is null)
                                {
                                    return;
                                }

                                machineBag.Add(machine);
                            }
                            catch (Exception exception) when (IsRecoverableLoadException(exception))
                            {
                                Trace.TraceError("Failed to load VM file '{0}': {1}", machineFilePath, exception);
                            }
                        });
                });

            return machineBag;
        }
    }

    private static bool IsRecoverableLoadException(Exception exception)
    {
        return exception is IOException
               or UnauthorizedAccessException
               or System.Security.SecurityException
               or ArgumentException
               or NotSupportedException
               or FormatException
               or OverflowException
               or System.Xml.XmlException;
    }
}