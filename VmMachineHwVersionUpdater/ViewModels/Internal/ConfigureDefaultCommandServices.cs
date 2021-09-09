using System;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using VmMachineHwVersionUpdater.Core.PerMachine;

namespace VmMachineHwVersionUpdater.ViewModels.Internal
{
    /// <inheritdoc />
    public class ConfigureDefaultCommandServices : IConfigureDefaultCommandServices
    {
        /// <inheritdoc />
        public void RunFor([NotNull] IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddSingleton<IAddEditAnnotation, AddEditAnnotation>();
            services.AddSingleton<IAboutWindowClickDefaultCommand, AboutWindowClickDefaultCommand>();
            services.AddSingleton<IArchiveDefaultCommand, ArchiveDefaultCommand>();
            services.AddSingleton<ICopyDefaultCommand, CopyDefaultCommand>();
            services.AddSingleton<IOpenWithCodeDefaultCommand, OpenWithCodeDefaultCommand>();
            services.AddSingleton<IAddEditAnnotationDefaultCommand, AddEditAnnotationDefaultCommand>();
            services.AddSingleton<IReloadDefaultCommand, ReloadDefaultCommand>();
            services.AddSingleton<IDeleteDefaultCommand, DeleteDefaultCommand>();
            services.AddSingleton<IGotToDefaultCommand, GotToDefaultCommand>();
            services.AddSingleton<IStartDefaultCommand, StartDefaultCommand>();
            services.AddSingleton<IUpdateAllDefaultCommand, UpdateAllDefaultCommand>();
        }
    }
}