//using System;
//using System.Linq;
//using EvilBaschdi.Core.Extensions;
//using JetBrains.Annotations;
//using MahApps.Metro.Controls.Dialogs;
//using VmMachineHwVersionUpdater.Core.PerMachine;
//using VmMachineHwVersionUpdater.Core.Settings;

//namespace VmMachineHwVersionUpdater.ViewModels.Internal
//{
//    /// <inheritdoc />
//    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
//    public class Init : IInit
//    {
//        private readonly IDeleteMachine _deleteMachine;
//        private readonly IDialogCoordinator _dialogCoordinator;
//        private readonly IPathSettings _pathSettings;
//        private readonly IToggleToolsSyncTime _toggleToolsSyncTime;
//        private readonly IToggleToolsUpgradePolicy _toggleToolsUpgradePolicy;
//        private readonly IUpdateMachineVersion _updateMachineVersion;

//        /// <summary>
//        ///     Constructor
//        /// </summary>
//        /// <param name="dialogCoordinator"></param>
//        /// <param name="pathSettings"></param>
//        /// <param name="updateMachineVersion"></param>
//        /// <param name="deleteMachine"></param>
//        /// <param name="toggleToolsUpgradePolicy"></param>
//        /// <param name="toggleToolsSyncTime"></param>
//        public Init([NotNull] IDialogCoordinator dialogCoordinator, [NotNull] IPathSettings pathSettings, [NotNull] IUpdateMachineVersion updateMachineVersion,
//                    [NotNull] IDeleteMachine deleteMachine, [NotNull] IToggleToolsUpgradePolicy toggleToolsUpgradePolicy,
//                    [NotNull] IToggleToolsSyncTime toggleToolsSyncTime)
//        {
//            _dialogCoordinator = dialogCoordinator ?? throw new ArgumentNullException(nameof(dialogCoordinator));
//            _pathSettings = pathSettings ?? throw new ArgumentNullException(nameof(pathSettings));
//            _updateMachineVersion = updateMachineVersion ?? throw new ArgumentNullException(nameof(updateMachineVersion));

//            _deleteMachine = deleteMachine ?? throw new ArgumentNullException(nameof(deleteMachine));

//            _toggleToolsUpgradePolicy = toggleToolsUpgradePolicy ?? throw new ArgumentNullException(nameof(toggleToolsUpgradePolicy));
//            _toggleToolsSyncTime = toggleToolsSyncTime ?? throw new ArgumentNullException(nameof(toggleToolsSyncTime));
//        }

//        /// <inheritdoc />
//        public object DialogCoordinatorContext { get; set; }

//        /// <inheritdoc />
//        public IUpdateMachineVersion UpdateMachineVersion { get; set; }

//        /// <inheritdoc />
//        public IDeleteMachine DeleteMachine { get; set; }

//        /// <inheritdoc />
//        public void Dispose()
//        {
//            Dispose(true);
//            GC.SuppressFinalize(this);
//        }

//        public void RunFor(object value)
//        {
//            var vmPoolFromSettingExistingPaths = _pathSettings.VmPool.GetExistingDirectories();
//            if (!vmPoolFromSettingExistingPaths.Any())
//            {
//                _dialogCoordinator.ShowMessageAsync(value, "No virtual machines found",
//                    "Please verify settings and discs attached");
//                return;
//            }

//            _dialogCoordinator.ShowMessageAsync(value, "Verifying VM pools from settings",
//                $"{vmPoolFromSettingExistingPaths.Count} paths found");

//            UpdateMachineVersion = _updateMachineVersion;
//            DeleteMachine = _deleteMachine;
//        }

//        /// <inheritdoc />
//        public void Run()
//        {
//            var vmPoolFromSettingExistingPaths = _pathSettings.VmPool.GetExistingDirectories();
//            if (!vmPoolFromSettingExistingPaths.Any())
//            {
//                _dialogCoordinator.ShowMessageAsync(DialogCoordinatorContext, "No virtual machines found",
//                    "Please verify settings and discs attached");
//                return;
//            }

//            _dialogCoordinator.ShowMessageAsync(DialogCoordinatorContext, "Verifying VM pools from settings",
//                $"{vmPoolFromSettingExistingPaths.Count} paths found");

//            UpdateMachineVersion = _updateMachineVersion;
//            DeleteMachine = _deleteMachine;
//        }


//        /// <summary>
//        ///     Dispose
//        /// </summary>
//        /// <param name="disposing"></param>
//        protected virtual void Dispose(bool disposing)
//        {
//            if (!disposing)
//            {
//                return;
//            }

//            _toggleToolsSyncTime?.Dispose();
//            _toggleToolsUpgradePolicy?.Dispose();
//            UpdateMachineVersion?.Dispose();
//        }
//    }
//}

