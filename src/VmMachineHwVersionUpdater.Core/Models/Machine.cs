using System.ComponentModel;

namespace VmMachineHwVersionUpdater.Core.Models;

/// <inheritdoc cref="INotifyPropertyChanged" />
public sealed class Machine(
    [NotNull] IToggleToolsSyncTime toggleToolsSyncTime,
    [NotNull] IToggleToolsUpgradePolicy toggleToolsUpgradePolicy,
    [NotNull] IToggleMksEnable3D toggleMksEnable3D,
    [NotNull] IUpdateMachineVersion updateMachineVersion,
    [NotNull] IUpdateMachineMemSize updateMachineMemSize) : INotifyPropertyChanged
{
    private readonly bool _autoUpdateTools;
    private readonly int _hwVersion;
    private readonly int _memSize;
    private readonly bool _syncTimeWithHost;
    private readonly bool _accelerate3DGraphics;
    private readonly IToggleToolsSyncTime _toggleToolsSyncTime = toggleToolsSyncTime ?? throw new ArgumentNullException(nameof(toggleToolsSyncTime));
    private readonly IToggleToolsUpgradePolicy _toggleToolsUpgradePolicy = toggleToolsUpgradePolicy ?? throw new ArgumentNullException(nameof(toggleToolsUpgradePolicy));
    private readonly IToggleMksEnable3D _toggleMksEnable3D = toggleMksEnable3D ?? throw new ArgumentNullException(nameof(toggleMksEnable3D));
    private readonly IUpdateMachineVersion _updateMachineVersion = updateMachineVersion ?? throw new ArgumentNullException(nameof(updateMachineVersion));
    private readonly IUpdateMachineMemSize _updateMachineMemSize = updateMachineMemSize ?? throw new ArgumentNullException(nameof(updateMachineMemSize));

    /// <summary />
    public bool AutoUpdateTools
    {
        // ReSharper disable once UnusedMember.Global
        get => _autoUpdateTools;
        init
        {
            if (_autoUpdateTools == value)
            {
                return;
            }

            _autoUpdateTools = value;
            NotifyAutoUpdateToolsChanged();
        }
    }

    /// <summary />
    public int HwVersion
    {
        // ReSharper disable once UnusedMember.Global
        get => _hwVersion;
        init
        {
            if (_hwVersion == value)
            {
                return;
            }

            _hwVersion = value;
            NotifyHwVersionChanged();
        }
    }

    /// <summary />
    public int MemSize
    {
        // ReSharper disable once UnusedMember.Global
        get => _memSize;
        init
        {
            if (_memSize == value)
            {
                return;
            }

            _memSize = value;
            NotifyMemSizeChanged();
        }
    }

    /// <summary />
    public bool SyncTimeWithHost
    {
        // ReSharper disable once UnusedMember.Global
        get => _syncTimeWithHost;
        init
        {
            if (_syncTimeWithHost == value)
            {
                return;
            }

            _syncTimeWithHost = value;
            NotifySyncTimeWithHostChanged();
        }
    }

    /// <summary />
    public bool Accelerate3DGraphics
    {
        // ReSharper disable once UnusedMember.Global
        get => _accelerate3DGraphics;
        init
        {
            if (_accelerate3DGraphics == value)
            {
                return;
            }

            _accelerate3DGraphics = value;
            NotifyAccelerate3DGraphicsChanged();
        }
    }

    // This method is called by the Set accessors of each property.
    // The CallerMemberName attribute that is applied to the optional propertyName
    // parameter causes the property name of the caller to be substituted as an argument.
    private void NotifyHwVersionChanged()
    {
        if (IsEnabledForEditing && PropertyChanged is not null)
        {
            _updateMachineVersion.RunFor(Path, _hwVersion);
        }
    }

    // This method is called by the Set accessors of each property.
    // The CallerMemberName attribute that is applied to the optional propertyName
    // parameter causes the property name of the caller to be substituted as an argument.
    private void NotifyMemSizeChanged()
    {
        if (IsEnabledForEditing && PropertyChanged is not null)
        {
            var memSizeMb = _memSize * 1024;
            _updateMachineMemSize.RunFor(Path, memSizeMb);
        }
    }

    // This method is called by the Set accessors of each property.
    // The CallerMemberName attribute that is applied to the optional propertyName
    // parameter causes the property name of the caller to be substituted as an argument.
    private void NotifyAutoUpdateToolsChanged()
    {
        if (IsEnabledForEditing && PropertyChanged is not null)
        {
            _toggleToolsUpgradePolicy.RunFor(Path, _autoUpdateTools);
        }
    }

    // This method is called by the Set accessors of each property.
    // The CallerMemberName attribute that is applied to the optional propertyName
    // parameter causes the property name of the caller to be substituted as an argument.
    private void NotifySyncTimeWithHostChanged()
    {
        if (IsEnabledForEditing && PropertyChanged is not null)
        {
            _toggleToolsSyncTime.RunFor(Path, _syncTimeWithHost);
        }
    }

    // This method is called by the Set accessors of each property.
    // The CallerMemberName attribute that is applied to the optional propertyName
    // parameter causes the property name of the caller to be substituted as an argument.
    private void NotifyAccelerate3DGraphicsChanged()
    {
        if (IsEnabledForEditing && PropertyChanged is not null)
        {
            _toggleMksEnable3D.RunFor(Path, _accelerate3DGraphics);
        }
    }

    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable PropertyCanBeMadeInitOnly.Global

    private MachineState _machineState;
    private string _logLastDate;
    private string _logLastDateDiff;
    private bool _isEnabledForEditing;
    private string _extendedInformation;
    private string _extendedInformationToolTip;

    /// <summary />
    public double DirectorySizeGb { get; set; }

    /// <summary />
    public MachineState MachineState
    {
        get => _machineState;
        set
        {
            if (_machineState == value)
            {
                return;
            }

            _machineState = value;
            OnPropertyChanged(nameof(MachineState));
        }
    }

    /// <summary />
    public string Annotation { get; set; }

    /// <summary />
    public string Directory { get; set; }

    /// <summary />
    public string DirectorySize { get; set; }

    /// <summary />
    public string DisplayName { get; set; }

    /// <summary />
    public string EncryptionData { get; init; }

    /// <summary />
    public string EncryptionEncryptedKey { get; init; }

    /// <summary />
    public string EncryptionKeySafe { get; init; }

    /// <summary />
    public string ExtendedInformation
    {
        get => _extendedInformation;
        set
        {
            if (_extendedInformation == value)
            {
                return;
            }

            _extendedInformation = value;
            OnPropertyChanged(nameof(ExtendedInformation));
        }
    }

    /// <summary />
    public string ExtendedInformationToolTip
    {
        get => _extendedInformationToolTip;
        set
        {
            if (_extendedInformationToolTip == value)
            {
                return;
            }

            _extendedInformationToolTip = value;
            OnPropertyChanged(nameof(ExtendedInformationToolTip));
        }
    }

    /// <summary />
    public string GuestOs { get; set; }

    /// <summary />
    public string GuestOsRaw { get; set; }

    /// <summary />
    public string GuestOsDetailedData { get; set; }

    /// <summary />
    public string LogLastDate
    {
        get => _logLastDate;
        set
        {
            if (_logLastDate == value)
            {
                return;
            }

            _logLastDate = value;
            OnPropertyChanged(nameof(LogLastDate));
        }
    }

    /// <summary />
    public string LogLastDateDiff
    {
        get => _logLastDateDiff;
        set
        {
            if (_logLastDateDiff == value)
            {
                return;
            }

            _logLastDateDiff = value;
            OnPropertyChanged(nameof(LogLastDateDiff));
        }
    }

    /// <summary />
    public string ManagedVmAutoAddVTpm { get; init; }

    /// <summary />
    public string Path { get; set; }

    /// <summary />
    public string ShortPath { get; init; }

    /// <summary />
    public bool IsEnabledForEditing
    {
        get => _isEnabledForEditing;
        set
        {
            if (_isEnabledForEditing == value)
            {
                return;
            }

            _isEnabledForEditing = value;
            OnPropertyChanged(nameof(IsEnabledForEditing));
        }
    }

    /// <summary />
    public MachineType MachineType { get; set; }

    // ReSharper restore PropertyCanBeMadeInitOnly.Global
    // ReSharper restore UnusedAutoPropertyAccessor.Global

    /// <inheritdoc />
    /// <summary />
    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}