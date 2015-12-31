using System;
using System.Windows.Forms;

namespace VmMachineHwVersionUpdater.Core
{
    /// <summary>
    /// </summary>
    public class AppBasics : IAppBasics
    {
        private readonly IAppSettings _appSettings;

        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="T:System.Object" />-Klasse.
        /// </summary>
        public AppBasics(IAppSettings appSettings)
        {
            if(appSettings == null)
            {
                throw new ArgumentNullException(nameof(appSettings));
            }
            _appSettings = appSettings;
        }

        /// <summary>
        /// </summary>
        public void BrowseFolder()
        {
            var folderDialog = new FolderBrowserDialog
            {
                SelectedPath = GetVMwarePool()
            };

            var result = folderDialog.ShowDialog();
            if(result.ToString() != "OK")
            {
                return;
            }

            _appSettings.VMwarePool = folderDialog.SelectedPath;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public string GetVMwarePool()
        {
            return string.IsNullOrWhiteSpace(_appSettings.VMwarePool)
                ? ""
                : _appSettings.VMwarePool;
        }
    }
}