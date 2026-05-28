namespace VmMachineHwVersionUpdater.Core.Models;

/// <summary />
public class VmFileChangedEventArgs
{
    /// <summary />
    public string FilePath { get; init; }

    /// <summary />
    public string OldFilePath { get; init; }

    /// <summary />
    public VmFileChangeType ChangeType { get; init; }
}