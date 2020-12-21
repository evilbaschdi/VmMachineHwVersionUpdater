using System;
using System.Linq;
using EvilBaschdi.Core.Extensions;
using EvilBaschdi.Core.Internal;
using JetBrains.Annotations;
using MahApps.Metro.Controls.Dialogs;
using VmMachineHwVersionUpdater.Core.BasicApplication;
using VmMachineHwVersionUpdater.Core.PerMachine;
using VmMachineHwVersionUpdater.Core.Settings;

namespace VmMachineHwVersionUpdater.ViewModels.Internal
{
    /// <inheritdoc />
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    public class Init : IInit
    {
        private readonly IDialogCoordinator _dialogCoordinator;
        private readonly IPathSettings _pathSettings;
        private IToggleToolsSyncTime _toggleToolsSyncTime;
        private IToggleToolsUpgradePolicy _toggleToolsUpgradePolicy;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="dialogCoordinator"></param>
        /// <param name="pathSettings"></param>
        public Init([NotNull] IDialogCoordinator dialogCoordinator, [NotNull] IPathSettings pathSettings)
        {
            _dialogCoordinator = dialogCoordinator ?? throw new ArgumentNullException(nameof(dialogCoordinator));
            _pathSettings = pathSettings ?? throw new ArgumentNullException(nameof(pathSettings));
        }

        /// <inheritdoc />
        public object DialogCoordinatorContext { get; set; }

        /// <inheritdoc />
        public IUpdateMachineVersion UpdateMachineVersion { get; set; }

        /// <inheritdoc />
        public IGuestOsesInUse GuestOsesInUse { get; set; }

        /// <inheritdoc />
        public IDeleteMachine DeleteMachine { get; set; }

        /// <inheritdoc />
        public ILoad Load { get; set; }

        /// <inheritdoc />
        public void Run()
        {
            var vmPoolFromSettingExistingPaths = _pathSettings.VmPool.GetExistingDirectories();
            if (!vmPoolFromSettingExistingPaths.Any())
            {
                _dialogCoordinator.ShowMessageAsync(DialogCoordinatorContext, "No virtual machines found",
                    "Please verify settings and discs attached");
                return;
            }

            IFileListFromPath fileListFromPath = new FileListFromPath();
            IGuestOsStringMapping guestOsStringMapping = new GuestOsStringMapping();
            IGuestOsOutputStringMapping guestOsOutputStringMapping = new GuestOsOutputStringMapping(guestOsStringMapping);
            IReadLogInformation readLogInformation = new ReadLogInformation();
            UpdateMachineVersion = new UpdateMachineVersion();
            _toggleToolsSyncTime = new ToggleToolsSyncTime();
            _toggleToolsUpgradePolicy = new ToggleToolsUpgradePolicy();
            IReturnValueFromVmxLine returnValueFromVmxLine = new ReturnValueFromVmxLine();
            IVmxLineStartsWith vmxLineStartsWith = new VmxLineStartsWith();
            IConvertAnnotationLineBreaks convertAnnotationLineBreaks = new ConvertAnnotationLineBreaks();
            IHandleMachineFromPath handleMachineFromPath = new HandleMachineFromPath(guestOsOutputStringMapping, _pathSettings, UpdateMachineVersion, readLogInformation,
                returnValueFromVmxLine, vmxLineStartsWith, convertAnnotationLineBreaks, _toggleToolsUpgradePolicy, _toggleToolsSyncTime);

            GuestOsesInUse = new GuestOsesInUse(guestOsStringMapping);
            IMachinesFromPath machinesFromPath = new MachinesFromPath(_pathSettings, handleMachineFromPath, fileListFromPath);
            DeleteMachine = new DeleteMachine();
            Load = new Load(machinesFromPath);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        /// <summary>
        ///     Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            _toggleToolsSyncTime?.Dispose();
            _toggleToolsUpgradePolicy?.Dispose();
            UpdateMachineVersion?.Dispose();
        }
    }
}