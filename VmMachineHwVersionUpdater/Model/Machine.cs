using System;
using System.ComponentModel;
using MahApps.Metro.IconPacks;
using VmMachineHwVersionUpdater.Internal;

namespace VmMachineHwVersionUpdater.Model
{
    /// <inheritdoc cref="INotifyPropertyChanged" />
    public class Machine : INotifyPropertyChanged
    {
        private readonly IHardwareVersion _hardwareVersion;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="hardwareVersion"></param>
        public Machine(IHardwareVersion hardwareVersion)
        {
            _hardwareVersion = hardwareVersion ?? throw new ArgumentNullException(nameof(hardwareVersion));
        }

        // ReSharper disable UnusedAutoPropertyAccessor.Global
        /// <summary />

        public string Id { get; set; }


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
        public bool SyncTimeWithHost { get; set; }


        /// <summary />
        public bool AutoUpdateTools { get; set; }


        /// <summary />
        public PackIconMaterialKind MachineState { get; set; }
        // ReSharper restore UnusedAutoPropertyAccessor.Global

        /// <summary />
        public int HwVersion
        {
            get => _hwVersion;
            set
            {
                if (_hwVersion == value)
                {
                    return;
                }

                _hwVersion = value;
                NotifyPropertyChanged(Path, _hwVersion);
            }
        }

        private int _hwVersion;

        /// <summary />
        public string GuestOs { get; set; }

        /// <inheritdoc />
        /// <summary />
        public event PropertyChangedEventHandler PropertyChanged;

        // This method is called by the Set accessors of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        private void NotifyPropertyChanged(string path, int newVersion)
        {
            if (PropertyChanged != null)
            {
                //var guestOsOutputStringMapping = new GuestOsOutputStringMapping();
                //var hardwareVersion = new HardwareVersion(guestOsOutputStringMapping);
                _hardwareVersion.Update(path, newVersion);
            }
        }
    }
}