using System.ComponentModel;
using VmMachineHwVersionUpdater.Internal;

namespace VmMachineHwVersionUpdater.Model
{
    /// <summary>
    /// </summary>
    public class Machine : INotifyPropertyChanged, IMachine
    {
        /// <summary>
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// </summary>
        public string ShortPath { get; set; }

        /// <summary>
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// </summary>
        public string Directory { get; set; }

        /// <summary>
        /// </summary>
        public string DirectorySize { get; set; }

        /// <summary>
        /// </summary>
        public double DirectorySizeGb { get; set; }

        /// <summary>
        /// </summary>
        public string LogLastDate { get; set; }

        /// <summary>
        /// </summary>
        public string LogLastDateDiff { get; set; }


        /// <summary>
        /// </summary>
        public bool SyncTimeWithHost { get; set; }


        /// <summary>
        /// </summary>
        public bool AutoUpdateTools { get; set; }

        /// <summary>
        /// </summary>
        public int HwVersion
        {
            get { return _hwVersion; }
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

        /// <summary>
        /// </summary>
        public string GuestOs { get; set; }

        /// <summary>
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        // This method is called by the Set accessors of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        private void NotifyPropertyChanged(string path, int newVersion)
        {
            if (PropertyChanged != null)
            {
                var guestOsOutputStringMapping = new GuestOsOutputStringMapping();
                var hardwareVersion = new HardwareVersion(guestOsOutputStringMapping);
                hardwareVersion.Update(path, newVersion);
            }
        }
    }
}