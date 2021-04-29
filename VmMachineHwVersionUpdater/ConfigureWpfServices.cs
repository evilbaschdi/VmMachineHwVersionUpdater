using System;
using EvilBaschdi.CoreExtended.AppHelpers;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using VmMachineHwVersionUpdater.Core.PerMachine;
using VmMachineHwVersionUpdater.Core.Settings;
using VmMachineHwVersionUpdater.ViewModels.Internal;

namespace VmMachineHwVersionUpdater
{
    /// <inheritdoc />
    public class ConfigureWpfServices : IConfigureWpfServices
    {
        /// <inheritdoc />
        public void RunFor([NotNull] IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddSingleton<ISelectedMachine, SelectedMachine>();
            services.AddSingleton<IVmPools, VmPools>();
            services.AddSingleton<IPathSettings, PathSettings>();
            services.AddSingleton<IArchiveMachine, ArchiveMachine>();
            services.AddSingleton<IProcessByPath, ProcessByPath>();
            services.AddSingleton<ICurrentItemSource, CurrentItemSource>();
            services.AddSingleton<IConfigureListCollectionView, ConfigureListCollectionView>();
            services.AddSingleton<IFilterItemSource, FilterItemSource>();
            services.AddSingleton<ITaskbarItemProgressState, CurrentTaskbarItemProgressState>();
            services.AddSingleton<IInitDefaultCommands, InitDefaultCommands>();
            services.AddSingleton<ILoadSearchOsItems, LoadSearchOsItems>();
        }
    }
}