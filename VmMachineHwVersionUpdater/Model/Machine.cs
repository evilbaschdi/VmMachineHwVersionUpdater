using System;
using System.ComponentModel;
using MahApps.Metro.IconPacks;
using VmMachineHwVersionUpdater.Internal;

namespace VmMachineHwVersionUpdater.Model
{
    /// <inheritdoc cref="INotifyPropertyChanged" />
    /// <inheritdoc cref="IMachine" />
    public class Machine : INotifyPropertyChanged, IMachine
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

        /// <inheritdoc />
        public string Id { get; set; }

        /// <inheritdoc />
        public string DisplayName { get; set; }

        /// <inheritdoc />
        public string ShortPath { get; set; }

        /// <inheritdoc />
        public string Path { get; set; }

        /// <inheritdoc />
        public string Directory { get; set; }

        /// <inheritdoc />
        public string DirectorySize { get; set; }

        /// <inheritdoc />
        public double DirectorySizeGb { get; set; }

        /// <inheritdoc />
        public string LogLastDate { get; set; }

        /// <inheritdoc />
        public string LogLastDateDiff { get; set; }


        /// <inheritdoc />
        public bool SyncTimeWithHost { get; set; }


        /// <inheritdoc />
        public bool AutoUpdateTools { get; set; }


        /// <inheritdoc />
        public PackIconMaterialKind MachineState { get; set; }

        /// <inheritdoc />
        public int HwVersion
        {
            get => _hwVersion;
            set
            {
                if (_hwVersion != value)
                {
                    _hwVersion = value;
                    NotifyPropertyChanged(Path, _hwVersion);
                }
            }
        }

        private int _hwVersion;

        /// <inheritdoc />
        public string GuestOs { get; set; }

        /// <inheritdoc />
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