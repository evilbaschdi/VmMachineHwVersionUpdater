using System;
using EvilBaschdi.Core.Internal;
using EvilBaschdi.CoreExtended;
using EvilBaschdi.CoreExtended.AppHelpers;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using VmMachineHwVersionUpdater.ViewModels.Internal;

//using VmMachineHwVersionUpdater.Core.PerMachine;
//using VmMachineHwVersionUpdater.Core.Settings;

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

            services.AddSingleton<IConfigureListCollectionView, ConfigureListCollectionView>();
            services.AddSingleton<ICopyDirectoryWithFilesWithProgress, CopyDirectoryWithFilesWithProgress>();
            services.AddSingleton<ICopyDirectoryWithProgress, CopyDirectoryWithProgress>();
            services.AddSingleton<ICopyProgress, CopyProgress>();
            services.AddSingleton<ICurrentItemSource, CurrentItemSource>();
            services.AddSingleton<IFilterItemSource, FilterItemSource>();
            services.AddSingleton<IInitDefaultCommands, InitDefaultCommands>();
            services.AddSingleton<ILoadSearchOsItems, LoadSearchOsItems>();
            services.AddSingleton<IProcessByPath, ProcessByPath>();
            services.AddSingleton<ITaskbarItemProgressState, CurrentTaskbarItemProgressState>();
            services.AddSingleton<IRoundCorners, RoundCorners>();
        }
    }
}