using System;
using EvilBaschdi.Core.Internal;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using VmMachineHwVersionUpdater.Core.BasicApplication;
using VmMachineHwVersionUpdater.Core.PerMachine;
using VmMachineHwVersionUpdater.Core.Settings;

namespace VmMachineHwVersionUpdater.Core
{
    /// <inheritdoc />
    public class ConfigureCoreServices : IConfigureCoreServices
    {
        /// <inheritdoc />
        public void RunFor([NotNull] IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddSingleton<ISettingsValid, SettingsValid>();
            services.AddSingleton<IFileListFromPath, FileListFromPath>();
            services.AddSingleton<IGuestOsStringMapping, GuestOsStringMapping>();
            services.AddSingleton<IGuestOsOutputStringMapping, GuestOsOutputStringMapping>();
            services.AddSingleton<IReadLogInformation, ReadLogInformation>();
            services.AddSingleton<IUpdateMachineVersion, UpdateMachineVersion>();
            services.AddSingleton<IToggleToolsSyncTime, ToggleToolsSyncTime>();
            services.AddSingleton<IToggleToolsUpgradePolicy, ToggleToolsUpgradePolicy>();
            services.AddSingleton<IReturnValueFromVmxLine, ReturnValueFromVmxLine>();
            services.AddSingleton<IVmxLineStartsWith, VmxLineStartsWith>();
            services.AddSingleton<IConvertAnnotationLineBreaks, ConvertAnnotationLineBreaks>();
            services.AddSingleton<IHandleMachineFromPath, HandleMachineFromPath>();
            services.AddSingleton<IGuestOsesInUse, GuestOsesInUse>();
            services.AddSingleton<IMachinesFromPath, MachinesFromPath>();
            services.AddSingleton<IDeleteMachine, DeleteMachine>();
            services.AddSingleton<ILoad, Load>();
        }
    }
}