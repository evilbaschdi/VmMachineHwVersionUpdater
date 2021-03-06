using System;
using System.ComponentModel;
using JetBrains.Annotations;
using VmMachineHwVersionUpdater.Core.Enums;
using VmMachineHwVersionUpdater.Core.PerMachine;

namespace VmMachineHwVersionUpdater.Core.Models
{
    /// <inheritdoc cref="INotifyPropertyChanged" />
    public class Machine : INotifyPropertyChanged
    {
        private readonly bool _autoUpdateTools;
        private readonly int _hwVersion;
        private readonly bool _syncTimeWithHost;
        private readonly IToggleToolsSyncTime _toggleToolsSyncTime;
        private readonly IToggleToolsUpgradePolicy _toggleToolsUpgradePolicy;
        private readonly IUpdateMachineVersion _updateMachineVersion;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="updateMachineVersion"></param>
        /// <param name="toggleToolsUpgradePolicy"></param>
        /// <param name="toggleToolsSyncTime"></param>
        public Machine([NotNull] IUpdateMachineVersion updateMachineVersion, [NotNull] IToggleToolsUpgradePolicy toggleToolsUpgradePolicy,
                       [NotNull] IToggleToolsSyncTime toggleToolsSyncTime)
        {
            _updateMachineVersion = updateMachineVersion ?? throw new ArgumentNullException(nameof(updateMachineVersion));
            _toggleToolsUpgradePolicy = toggleToolsUpgradePolicy ?? throw new ArgumentNullException(nameof(toggleToolsUpgradePolicy));
            _toggleToolsSyncTime = toggleToolsSyncTime ?? throw new ArgumentNullException(nameof(toggleToolsSyncTime));
        }

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
            if (PropertyChanged != null)
            {
                _updateMachineVersion.RunFor(Path, _hwVersion);
            }
        }

        // This method is called by the Set accessors of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        private void NotifyAutoUpdateToolsChanged()
        {
            if (PropertyChanged != null)
            {
                _toggleToolsUpgradePolicy.RunFor(Path, _autoUpdateTools);
            }
        }

        // This method is called by the Set accessors of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        private void NotifySyncTimeWithHostChanged()
        {
            if (PropertyChanged != null)
            {
                _toggleToolsSyncTime.RunFor(Path, _syncTimeWithHost);
            }
        }

        // ReSharper disable UnusedAutoPropertyAccessor.Global
        // ReSharper disable PropertyCanBeMadeInitOnly.Global
        /// <summary />
        public string DisplayName { get; set; }

        /// <summary />

        public string ShortPath { get; set; }

        /// <summary />
        public string Path { get; set; }

        /// <summary />
        public string Directory { get; set; }

        /// <summary />
        public string DirectorySize { get; set; }

        /// <summary> </summary>
        public double DirectorySizeGb { get; set; }

        /// <summary />
        public string LogLastDate { get; set; }

        /// <summary />
        public string LogLastDateDiff { get; set; }

        /// <summary />
        public string Annotation { get; set; }

        /// <summary />
        public string GuestOs { get; set; }

        /// <summary />
        public string GuestOsDetailedData { get; set; }

        /// <summary />
        public MachineState MachineState { get; set; }
        // ReSharper restore PropertyCanBeMadeInitOnly.Global
        // ReSharper restore UnusedAutoPropertyAccessor.Global
    }
}