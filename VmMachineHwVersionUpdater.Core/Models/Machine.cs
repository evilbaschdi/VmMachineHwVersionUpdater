using System.ComponentModel;
using VmMachineHwVersionUpdater.Core.Enums;

namespace VmMachineHwVersionUpdater.Core.Models;

/// <inheritdoc cref="INotifyPropertyChanged" />
/// <summary>
///     Constructor
/// </summary>
/// <param name="updateMachineVersion"></param>
/// <param name="toggleToolsUpgradePolicy"></param>
/// <param name="toggleToolsSyncTime"></param>
public class Machine([NotNull] IToggleToolsSyncTime toggleToolsSyncTime,
               [NotNull] IToggleToolsUpgradePolicy toggleToolsUpgradePolicy,
               [NotNull] IUpdateMachineVersion updateMachineVersion) : INotifyPropertyChanged
{
    private readonly bool _autoUpdateTools;
    private readonly int _hwVersion;
    private readonly bool _syncTimeWithHost;
    private readonly IToggleToolsSyncTime _toggleToolsSyncTime = toggleToolsSyncTime ?? throw new ArgumentNullException(nameof(toggleToolsSyncTime));
    private readonly IToggleToolsUpgradePolicy _toggleToolsUpgradePolicy = toggleToolsUpgradePolicy ?? throw new ArgumentNullException(nameof(toggleToolsUpgradePolicy));
    private readonly IUpdateMachineVersion _updateMachineVersion = updateMachineVersion ?? throw new ArgumentNullException(nameof(updateMachineVersion));

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

    /// <inheritdoc />
    /// <summary />
    public event PropertyChangedEventHandler PropertyChanged;

    // This method is called by the Set accessors of each property.
    // The CallerMemberName attribute that is applied to the optional propertyName
    // parameter causes the property name of the caller to be substituted as an argument.
    private void NotifyHwVersionChanged()
    {
        if (IsEnabledForEditing && PropertyChanged != null)
        {
            _updateMachineVersion.RunFor(Path, _hwVersion);
        }
    }

    // This method is called by the Set accessors of each property.
    // The CallerMemberName attribute that is applied to the optional propertyName
    // parameter causes the property name of the caller to be substituted as an argument.
    private void NotifyAutoUpdateToolsChanged()
    {
        if (IsEnabledForEditing && PropertyChanged != null)
        {
            _toggleToolsUpgradePolicy.RunFor(Path, _autoUpdateTools);
        }
    }

    // This method is called by the Set accessors of each property.
    // The CallerMemberName attribute that is applied to the optional propertyName
    // parameter causes the property name of the caller to be substituted as an argument.
    private void NotifySyncTimeWithHostChanged()
    {
        if (IsEnabledForEditing && PropertyChanged != null)
        {
            _toggleToolsSyncTime.RunFor(Path, _syncTimeWithHost);
        }
    }

    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable PropertyCanBeMadeInitOnly.Global

    /// <summary />
    public double DirectorySizeGb { get; set; }

    /// <summary />
    public MachineState MachineState { get; set; }

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
    public string ExtendedInformation { get; set; }

    /// <summary />
    public string ExtendedInformationToolTip { get; set; }

    /// <summary />
    public string GuestOs { get; set; }

    /// <summary />
    public string GuestOsDetailedData { get; set; }

    /// <summary />
    public string LogLastDate { get; init; }

    /// <summary />
    public string LogLastDateDiff { get; init; }

    /// <summary />
    public string ManagedVmAutoAddVTpm { get; init; }

    /// <summary />
    public string Path { get; init; }

    /// <summary />
    public string ShortPath { get; init; }

    /// <summary />
    public bool IsEnabledForEditing { get; set; }
    // ReSharper restore PropertyCanBeMadeInitOnly.Global
    // ReSharper restore UnusedAutoPropertyAccessor.Global
}