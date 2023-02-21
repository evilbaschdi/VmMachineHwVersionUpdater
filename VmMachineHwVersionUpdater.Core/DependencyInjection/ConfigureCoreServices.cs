using EvilBaschdi.Core.Internal;
using VmMachineHwVersionUpdater.Core.BasicApplication;
using VmMachineHwVersionUpdater.Core.Models;
using VmMachineHwVersionUpdater.Core.PerMachine;
using VmMachineHwVersionUpdater.Core.Settings;

namespace VmMachineHwVersionUpdater.Core.DependencyInjection;

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

        services.AddSingleton<IArchiveMachine, ArchiveMachine>();
        services.AddSingleton<IConvertAnnotationLineBreaks, ConvertAnnotationLineBreaks>();
        services.AddSingleton<ICopyMachine, CopyMachine>();
        services.AddSingleton<ICurrentItem, CurrentItem>();
        services.AddSingleton<IDeleteMachine, DeleteMachine>();
        services.AddSingleton<IFileListFromPath, FileListFromPath>();
        services.AddSingleton<IGuestOsesInUse, GuestOsesInUse>();
        services.AddSingleton<IGuestOsOutputStringMapping, GuestOsOutputStringMapping>();
        services.AddSingleton<IGuestOsStringMapping, GuestOsStringMapping>();
        services.AddSingleton<IHandleMachineFromPath, HandleMachineFromPath>();
        services.AddSingleton<ILoad, Load>();
        services.AddSingleton<IMachinesFromPath, MachinesFromPath>();
        services.AddSingleton<IParseVmxFile, ParseVmxFile>();
        services.AddSingleton<IPathSettings, PathSettings>();
        services.AddSingleton<IReadLogInformation, ReadLogInformation>();
        services.AddSingleton<IReturnValueFromVmxLine, ReturnValueFromVmxLine>();
        services.AddSingleton<ISetDisplayName, SetDisplayName>();
        services.AddSingleton<ISetMachineIsEnabledForEditing, SetMachineIsEnabledForEditing>();
        services.AddSingleton<ISettingsValid, SettingsValid>();
        services.AddSingleton<IToggleToolsSyncTime, ToggleToolsSyncTime>();
        services.AddSingleton<IToggleToolsUpgradePolicy, ToggleToolsUpgradePolicy>();
        services.AddSingleton<IUpdateAnnotation, UpdateAnnotation>();
        services.AddSingleton<IUpdateMachineVersion, UpdateMachineVersion>();
        services.AddSingleton<IVmPools, VmPools>();
        services.AddSingleton<IVmxLineStartsWith, VmxLineStartsWith>();
    }
}