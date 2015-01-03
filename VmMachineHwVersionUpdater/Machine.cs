using System.ComponentModel;
using VmMachineHwVersionUpdater.Internal;

namespace VmMachineHwVersionUpdater
{
    public class Machine : INotifyPropertyChanged
    {
        public string Id { get; set; }

        public string DisplayName { get; set; }

        public string Path { get; set; }

        public int HwVersion
        {
            get { return _hwVersion; }
            set
            {
                if(_hwVersion != value)
                {
                    _hwVersion = value;
                    NotifyPropertyChanged(Path, DisplayName, _hwVersion);
                }
            }
        }

        private int _hwVersion;

        public string GuestOs { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        private void NotifyPropertyChanged(string path, string displayName, int newVersion)
        {
            if(PropertyChanged != null)
            {
                var hardwareVersion = new HardwareVersion();
                hardwareVersion.Update(path, newVersion);

                //var window = new MainWindow();
                //window.ShowUpdateDialog(displayName, newVersion);
            }
        }
    }
}