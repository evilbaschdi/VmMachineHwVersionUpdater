using System;
using EvilBaschdi.Core;
using VmMachineHwVersionUpdater.Core.BasicApplication;
using VmMachineHwVersionUpdater.Core.PerMachine;

namespace VmMachineHwVersionUpdater.ViewModels.Internal
{
    /// <inheritdoc cref="IRun" />
    /// <inheritdoc cref="IDisposable" />
    /// <inheritdoc cref="IDialogCoordinatorContext" />
    public interface IInit : IRun, IDisposable, IDialogCoordinatorContext
    {
        // ReSharper disable UnusedMemberInSuper.Global
        /// <summary>
        /// </summary>

        public IDeleteMachine DeleteMachine { get; set; }

        /// <summary>
        /// </summary>

        public IGuestOsesInUse GuestOsesInUse { get; set; }

        /// <summary>
        /// </summary>
        public ILoad Load { get; set; }

        /// <summary>
        /// </summary>
        // ReSharper disable once UnusedMemberInSuper.Global
        public IUpdateMachineVersion UpdateMachineVersion { get; set; }
        // ReSharper restore UnusedMemberInSuper.Global
    }
}