namespace VmMachineHwVersionUpdater.Core.Models;

/// <summary>
/// </summary>
public class RawMachine
{
    /// <summary />
    public string Annotation { get; set; } = "";

    /// <summary />
    public string DetailedData { get; set; } = "";

    /// <summary />
    public string DisplayName { get; set; } = "";

    /// <summary />
    public string EncryptionData { get; set; } = "";

    /// <summary />
    public string EncryptionEncryptedKey { get; set; } = "";

    /// <summary />
    public string EncryptionKeySafe { get; set; } = "";

    /// <summary />
    public string GuestOs { get; set; } = "";

    /// <summary />
    public int HwVersion { get; set; }

    /// <summary />
    public int MemSize { get; set; }

    /// <summary />
    public string ManagedVmAutoAddVTpm { get; set; } = "";

    /// <summary />
    public string SyncTimeWithHost { get; set; } = "";

    /// <summary />
    public string ToolsUpgradePolicy { get; set; } = "";
}