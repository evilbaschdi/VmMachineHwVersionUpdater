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
    private bool _autoUpdateTools;
    private int _hwVersion;
    private int _memSize;
    private bool _syncTimeWithHost;
    private bool _accelerate3DGraphics;

    private readonly IToggleToolsSyncTime _toggleToolsSyncTime =
        toggleToolsSyncTime ?? throw new ArgumentNullException(nameof(toggleToolsSyncTime));

    private readonly IToggleToolsUpgradePolicy _toggleToolsUpgradePolicy =
        toggleToolsUpgradePolicy ?? throw new ArgumentNullException(nameof(toggleToolsUpgradePolicy));

    private readonly IToggleMksEnable3D _toggleMksEnable3D =
        toggleMksEnable3D ?? throw new ArgumentNullException(nameof(toggleMksEnable3D));

    private readonly IUpdateMachineVersion _updateMachineVersion =
        updateMachineVersion ?? throw new ArgumentNullException(nameof(updateMachineVersion));

    private readonly IUpdateMachineMemSize _updateMachineMemSize =
        updateMachineMemSize ?? throw new ArgumentNullException(nameof(updateMachineMemSize));

    /// <summary />
    public bool AutoUpdateTools
    {
        // ReSharper disable once UnusedMember.Global
        get => _autoUpdateTools;
        set
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
        set
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
        set
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
        set
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
        set
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
        OnPropertyChanged(nameof(HwVersion));

        if (IsEnabledForEditing)
        {
            _updateMachineVersion.RunFor(Path, _hwVersion);
        }
    }

    // This method is called by the Set accessors of each property.
    // The CallerMemberName attribute that is applied to the optional propertyName
    // parameter causes the property name of the caller to be substituted as an argument.
    private void NotifyMemSizeChanged()
    {
        OnPropertyChanged(nameof(MemSize));

        if (IsEnabledForEditing)
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
        OnPropertyChanged(nameof(AutoUpdateTools));

        if (IsEnabledForEditing)
        {
            _toggleToolsUpgradePolicy.RunFor(Path, _autoUpdateTools);
        }
    }

    // This method is called by the Set accessors of each property.
    // The CallerMemberName attribute that is applied to the optional propertyName
    // parameter causes the property name of the caller to be substituted as an argument.
    private void NotifySyncTimeWithHostChanged()
    {
        OnPropertyChanged(nameof(SyncTimeWithHost));

        if (IsEnabledForEditing)
        {
            _toggleToolsSyncTime.RunFor(Path, _syncTimeWithHost);
        }
    }

    // This method is called by the Set accessors of each property.
    // The CallerMemberName attribute that is applied to the optional propertyName
    // parameter causes the property name of the caller to be substituted as an argument.
    private void NotifyAccelerate3DGraphicsChanged()
    {
        OnPropertyChanged(nameof(Accelerate3DGraphics));

        if (IsEnabledForEditing)
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
    public string EncryptionData { get; set; }

    /// <summary />
    public string EncryptionEncryptedKey { get; set; }

    /// <summary />
    public string EncryptionKeySafe { get; set; }

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
    public string GuestInfoDetailedData { get; set; }

    /// <summary />
    public Dictionary<string, string> ParsedGuestInfoDetailedData { get; set; }

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
    public string ManagedVmAutoAddVTpm { get; set; }

    /// <summary />
    public string Path { get; set; }

    /// <summary />
    public string ShortPath { get; set; }

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

    /// <summary>
    /// </summary>
    /// <param name="other"></param>
    public void ApplyFrom(Machine other)
    {
        ArgumentNullException.ThrowIfNull(other);

        if (ReferenceEquals(this, other))
        {
            return;
        }

        ApplyIfChanged(_autoUpdateTools, other._autoUpdateTools, value => _autoUpdateTools = value, nameof(AutoUpdateTools));
        ApplyIfChanged(_hwVersion, other._hwVersion, value => _hwVersion = value, nameof(HwVersion));
        ApplyIfChanged(_memSize, other._memSize, value => _memSize = value, nameof(MemSize));
        ApplyIfChanged(_syncTimeWithHost, other._syncTimeWithHost, value => _syncTimeWithHost = value, nameof(SyncTimeWithHost));
        ApplyIfChanged(_accelerate3DGraphics, other._accelerate3DGraphics, value => _accelerate3DGraphics = value, nameof(Accelerate3DGraphics));
        ApplyIfChanged(DirectorySizeGb, other.DirectorySizeGb, value => DirectorySizeGb = value, nameof(DirectorySizeGb));
        ApplyIfChanged(_machineState, other._machineState, value => _machineState = value, nameof(MachineState));
        ApplyIfChanged(Annotation, other.Annotation, value => Annotation = value, nameof(Annotation));
        ApplyIfChanged(Directory, other.Directory, value => Directory = value, nameof(Directory));
        ApplyIfChanged(DirectorySize, other.DirectorySize, value => DirectorySize = value, nameof(DirectorySize));
        ApplyIfChanged(DisplayName, other.DisplayName, value => DisplayName = value, nameof(DisplayName));
        ApplyIfChanged(EncryptionData, other.EncryptionData, value => EncryptionData = value, nameof(EncryptionData));
        ApplyIfChanged(EncryptionEncryptedKey, other.EncryptionEncryptedKey, value => EncryptionEncryptedKey = value, nameof(EncryptionEncryptedKey));
        ApplyIfChanged(EncryptionKeySafe, other.EncryptionKeySafe, value => EncryptionKeySafe = value, nameof(EncryptionKeySafe));
        ApplyIfChanged(_extendedInformation, other._extendedInformation, value => _extendedInformation = value, nameof(ExtendedInformation));
        ApplyIfChanged(_extendedInformationToolTip, other._extendedInformationToolTip, value => _extendedInformationToolTip = value, nameof(ExtendedInformationToolTip));
        ApplyIfChanged(GuestOs, other.GuestOs, value => GuestOs = value, nameof(GuestOs));
        ApplyIfChanged(GuestOsRaw, other.GuestOsRaw, value => GuestOsRaw = value, nameof(GuestOsRaw));
        ApplyIfChanged(GuestInfoDetailedData, other.GuestInfoDetailedData, value => GuestInfoDetailedData = value, nameof(GuestInfoDetailedData));
        if (!DictionaryValuesEqual(ParsedGuestInfoDetailedData, other.ParsedGuestInfoDetailedData))
        {
            ParsedGuestInfoDetailedData = other.ParsedGuestInfoDetailedData;
            OnPropertyChanged(nameof(ParsedGuestInfoDetailedData));
        }

        ApplyIfChanged(_logLastDate, other._logLastDate, value => _logLastDate = value, nameof(LogLastDate));
        ApplyIfChanged(_logLastDateDiff, other._logLastDateDiff, value => _logLastDateDiff = value, nameof(LogLastDateDiff));
        ApplyIfChanged(ManagedVmAutoAddVTpm, other.ManagedVmAutoAddVTpm, value => ManagedVmAutoAddVTpm = value, nameof(ManagedVmAutoAddVTpm));
        ApplyIfChanged(Path, other.Path, value => Path = value, nameof(Path));
        ApplyIfChanged(ShortPath, other.ShortPath, value => ShortPath = value, nameof(ShortPath));
        ApplyIfChanged(_isEnabledForEditing, other._isEnabledForEditing, value => _isEnabledForEditing = value, nameof(IsEnabledForEditing));
        ApplyIfChanged(MachineType, other.MachineType, value => MachineType = value, nameof(MachineType));
    }

    private void ApplyIfChanged<T>(T currentValue, T newValue, Action<T> update, string propertyName)
    {
        if (EqualityComparer<T>.Default.Equals(currentValue, newValue))
        {
            return;
        }

        update(newValue);
        OnPropertyChanged(propertyName);
    }

    private static bool DictionaryValuesEqual(Dictionary<string, string> left, Dictionary<string, string> right)
    {
        if (ReferenceEquals(left, right))
        {
            return true;
        }

        if (left is null || right is null || left.Count != right.Count)
        {
            return false;
        }

        return left.All(pair => right.TryGetValue(pair.Key, out var value) &&
                                EqualityComparer<string>.Default.Equals(pair.Value, value));
    }

    /// <inheritdoc />
    /// <summary />
    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}